"""Draft 7 validation for the public API properties schema."""

import json
import os
import subprocess
from copy import deepcopy
from pathlib import Path

import json5
import pytest
from jsonschema import Draft7Validator


ROOT = Path(__file__).resolve().parents[2]
SCHEMA_PATH = ROOT / "schemas" / "paconn-apiProperties.schema.json"
FIXTURES = Path(__file__).parent / "fixtures"


@pytest.fixture(scope="module")
def schema() -> dict:
    return json5.loads(SCHEMA_PATH.read_text(encoding="utf-8-sig"))


@pytest.fixture(scope="module")
def validator(schema: dict) -> Draft7Validator:
    return Draft7Validator(schema)


@pytest.fixture(scope="module")
def baseline() -> dict:
    path = os.environ.get("API_PROPERTIES_BASELINE")
    if not path:
        pytest.skip("Set API_PROPERTIES_BASELINE to compare with an earlier schema")
    return json5.loads(Path(path).read_text(encoding="utf-8-sig"))


def document(**properties: object) -> dict:
    return {"properties": {"iconBrandColor": "#007EE6", **properties}}


def parameter_sets(parameters: dict) -> dict:
    return {"values": [{"name": "example-auth", "parameters": parameters}]}


def test_draft7_schema(schema: dict) -> None:
    Draft7Validator.check_schema(schema)


@pytest.mark.parametrize("filename", [
    "multi-auth-basic-api-key.json",
    "multi-auth-oauth.json",
])
def test_documented_multi_auth(validator: Draft7Validator, filename: str) -> None:
    # https://learn.microsoft.com/en-us/connectors/custom-connectors/multi-auth
    validator.validate(json.loads((FIXTURES / filename).read_text(encoding="utf-8")))


@pytest.mark.parametrize("mode", ["Global", "Direct", "GlobalPerConnector"])
@pytest.mark.parametrize("nested", [False, True])
def test_redirect_modes(validator: Draft7Validator, mode: str, nested: bool) -> None:
    parameters = {
        "token": {
            "type": "oauthSetting",
            "oAuthSettings": {"identityProvider": "oauth2", "redirectMode": mode},
        }
    }
    properties = (
        {"connectionParameterSets": parameter_sets(parameters)}
        if nested else {"connectionParameters": parameters}
    )
    validator.validate(document(**properties))


@pytest.mark.parametrize("operations", [[], ["getCall", "postCall", "putCall"], None])
def test_script_operations(validator: Draft7Validator, operations: object) -> None:
    # pac connector init emits null; the paconn documentation also allows [].
    validator.validate(document(scriptOperations=operations))


@pytest.mark.parametrize("operations", ["getCall", {}, [1], [None], True])
def test_malformed_script_operations(
    validator: Draft7Validator, operations: object
) -> None:
    assert not validator.is_valid(document(scriptOperations=operations))


@pytest.mark.parametrize("parameter_type", ["string", "securestring", "int", "bool"])
def test_ui_schema_is_metadata(
    validator: Draft7Validator, parameter_type: str
) -> None:
    validator.validate(document(connectionParameters={
        "input": {
            "type": parameter_type,
            "uiDefinition": {
                "schema": {"type": parameter_type, "description": "Connection input"},
                "description": "Legacy description",
                "constraints": {"required": "true", "hidden": "false"},
            },
        }
    }))


@pytest.mark.parametrize("metadata", [None, [], "string", {"type": 1},
                                      {"description": False}, {"unexpected": True}])
def test_malformed_ui_schema(validator: Draft7Validator, metadata: object) -> None:
    assert not validator.is_valid(document(connectionParameters={
        "input": {"type": "string", "uiDefinition": {"schema": metadata}}
    }))


@pytest.mark.parametrize("display_key", ["displayName", "displayname"])
def test_parameter_set_ui_casing(
    validator: Draft7Validator, display_key: str
) -> None:
    ui = {display_key: "Authentication", "description": "Select authentication"}
    validator.validate(document(connectionParameterSets={
        "uiDefinition": ui,
        "values": [{
            "name": "basic-auth",
            "uiDefinition": ui,
            "parameters": {"username": {"type": "string"}},
        }],
    }))


@pytest.mark.parametrize("sets", [
    None, [], {}, {"values": {}}, {"values": [None]},
    {"values": [{}]},
    {"values": [{"name": "auth"}]},
    {"values": [{"parameters": {}}]},
    {"values": [{"name": "", "parameters": {}}]},
    {"values": [{"name": 42, "parameters": {}}]},
    {"values": [{"name": "auth", "parameters": []}]},
    {"values": [{"name": "auth", "parameters": {}, "unexpected": True}]},
    {"values": [], "unexpected": True},
    {"values": [], "uiDefinition": None},
    {"values": [], "uiDefinition": {"displayName": 1}},
    {"values": [], "uiDefinition": {"description": False}},
    {"values": [], "uiDefinition": {"unexpected": "value"}},
    {"values": [{"name": "auth", "parameters": {}, "uiDefinition": []}]},
    parameter_sets({"bad-name": {"type": "string"}}),
    parameter_sets({"key": {"type": "unsupported"}}),
    parameter_sets({"key": {"type": "string", "unexpected": True}}),
    parameter_sets({"key": {"type": "string", "uiDefinition": {
        "constraints": {"required": True}
    }}}),
    parameter_sets({"token": {"type": "oauthSetting", "oAuthSettings": {
        "identityProvider": "undocumented"
    }}}),
])
def test_malformed_parameter_sets(validator: Draft7Validator, sets: object) -> None:
    assert not validator.is_valid(document(connectionParameterSets=sets))


def test_flat_and_multi_auth_can_coexist(validator: Draft7Validator) -> None:
    validator.validate(document(
        connectionParameters={"key": {"type": "securestring"}},
        connectionParameterSets=parameter_sets({"username": {"type": "string"}}),
    ))


@pytest.mark.parametrize("provider", [
    "oauth2", "oauth2generic", "aad", "aadcertificate", "facebook",
])
def test_legacy_providers(validator: Draft7Validator, provider: str) -> None:
    validator.validate(document(connectionParameters={
        "token": {
            "type": "oauthSetting",
            "oAuthSettings": {
                "identityProvider": provider,
                "clientId": None,
                "clientSecret": None,
                "properties": {"IsFirstParty": "True"},
            },
        }
    }))


@pytest.mark.parametrize("parameter_type", [
    "string", "securestring", "int", "bool", "gatewaySetting", "oauthSetting",
])
@pytest.mark.parametrize("ui", [
    {"displayName": "Legacy input"},
    {"schema": {"type": "securestring", "description": "Connection input"}},
])
def test_parameter_oneof_remains_exclusive(
    validator: Draft7Validator, schema: dict, parameter_type: str, ui: dict
) -> None:
    parameter = {"type": parameter_type, "uiDefinition": ui}
    branches = schema["definitions"]["ConnectionParameter"]["oneOf"]
    assert sum(not list(validator.descend(parameter, branch))
               for branch in branches) == 1
    validator.validate(document(connectionParameters={"input": parameter}))


@pytest.mark.parametrize("provider", [
    "oauth2", "oauth2generic", "aad", "aadcertificate", "facebook",
])
def test_provider_oneof_remains_exclusive(
    validator: Draft7Validator, schema: dict, provider: str
) -> None:
    settings = {
        "identityProvider": provider,
        "properties": {"IsFirstParty": "True"},
        "redirectMode": "GlobalPerConnector",
    }
    branches = schema["definitions"]["oAuthSettings"]["oneOf"]
    assert sum(not list(validator.descend(settings, branch))
               for branch in branches) == 1
    validator.validate(document(connectionParameters={
        "token": {"type": "oauthSetting", "oAuthSettings": settings}
    }))


@pytest.mark.parametrize("parameters", [
    {},
    {"key": {"type": "securestring"}},
    {"gateway": {"type": "gatewaySetting"}},
    {"token": {"type": "oauthSetting"}},
    {"token:clientId": {}},
    {"token:clientId": {"uiDefinition": {}}},
    {"count": {"type": "int", "defaultValue": "1"}},
    {"flag": {"type": "bool", "defaultValue": "false"}},
    {"environment": {
        "type": "string",
        "allowedValues": [{
            "value": "sandbox",
            "uiDefinition": {"displayName": "Sandbox", "description": "Testing"},
        }],
        "uiDefinition": {
            "displayName": "Environment", "description": "Legacy metadata",
            "tooltip": "Choose an environment", "tabIndex": 1,
            "constraints": {
                "clearText": True, "required": "legacy", "hidden": "false",
                "tabIndex": 2, "capability": ["gateway"],
                "allowedValues": [{"value": "sandbox", "text": "Sandbox"}],
                "dependencies": {"other": {"values": ["enabled"]}},
            },
        },
    }},
    {"token:TenantId": {
        "type": "string",
        "metadata": {"sourceType": "AzureActiveDirectoryTenant"},
        "uiDefinition": {
            "displayName": "Tenant",
            "description": "The tenant ID of for the Azure Active Directory application",
            "constraints": {"hidden": "true", "required": "false"},
        },
    }},
    {"token": {
        "type": "oauthSetting",
        "oAuthSettings": {
            "identityProvider": "oauth2generic",
            "ClientID": None, "ClientSecret": "PLACEHOLDER",
            "customParameters": {"legacy": {"unvalidated": True}},
            "properties": {"IsFirstParty": "False", "legacyMetadata": True},
        },
    }},
])
def test_legacy_parameter_forms(validator: Draft7Validator, parameters: dict) -> None:
    validator.validate(document(connectionParameters=parameters))
    validator.validate(document(connectionParameterSets=parameter_sets(parameters)))


@pytest.mark.parametrize("instance", [
    {}, {"properties": {}}, {"properties": {"iconBrandColor": 1}},
    {"properties": {"iconBrandColor": "#007EE6"}, "unexpected": True},
    document(unexpected=True),
    document(connectionParameters={"key": {"type": "string", "unexpected": True}}),
    document(connectionParameters={"key": {
        "type": "string", "uiDefinition": {"unexpected": True}
    }}),
    document(connectionParameters={"token": {
        "type": "oauthSetting", "oAuthSettings": {
            "identityProvider": "github"
        }
    }}),
    document(connectionParameters={"token": {
        "type": "oauthSetting", "oAuthSettings": {
            "identityProvider": "oauth2", "redirectMode": "unknown"
        }
    }}),
    document(connectionParameters={"key": {}}),
    document(connectionParameters={"key": {
        "type": "string", "oAuthSettings": {"identityProvider": "oauth2"}
    }}),
    document(connectionParameters={"gateway": {
        "type": "gatewaySetting", "defaultValue": "legacy"
    }}),
    document(connectionParameters={"token": {
        "type": "oauthSetting", "allowedValues": []
    }}),
    document(connectionParameters={"token": {
        "type": "oauthSetting", "oAuthSettings": {}
    }}),
    document(connectionParameters={"token": {
        "type": "oauthSetting", "oAuthSettings": {
            "identityProvider": "oauth2", "customParameters": {"unexpected": {}}
        }
    }}),
    document(connectionParameters={"token": {
        "type": "oauthSetting", "oAuthSettings": {
            "identityProvider": "aad", "customParameters": {}
        }
    }}),
    document(connectionParameters={"token": {
        "type": "oauthSetting", "oAuthSettings": {
            "identityProvider": "aadcertificate",
            "properties": {"IsFirstParty": "False"}
        }
    }}),
])
def test_existing_guards(validator: Draft7Validator, instance: dict) -> None:
    assert not validator.is_valid(instance)


def test_only_intended_additive_deltas(schema: dict, baseline: dict) -> None:
    """Removing only the new surface must reproduce the baseline exactly."""
    original = deepcopy(schema)
    properties = original["properties"]["properties"]["properties"]
    del properties["connectionParameterSets"]
    del properties["scriptOperations"]
    definitions = original["definitions"]
    del definitions["ConnectionParameterSets"]
    del definitions["ConnectionParameterSet-uiDefinition"]
    del definitions["ConnectionParameter-uiDefinition"]["properties"]["schema"]
    redirect = definitions["oAuthSettings"]["properties"]["redirectMode"]
    old_modes = baseline["definitions"]["oAuthSettings"]["properties"]["redirectMode"]["enum"]
    assert redirect["enum"] == old_modes + ["GlobalPerConnector"]
    redirect["enum"].remove("GlobalPerConnector")
    del redirect["description"]
    assert original == baseline


def test_corpus_preserves_every_old_valid_document(
    validator: Draft7Validator, baseline: dict
) -> None:
    old_validator = Draft7Validator(baseline)
    Draft7Validator.check_schema(baseline)
    tracked = subprocess.run(
        ["git", "ls-files", "-z"], cwd=ROOT, check=True, capture_output=True
    ).stdout.decode("utf-8").split("\0")
    paths = sorted(path for path in tracked
                   if Path(path).name.lower() == "apiproperties.json")
    assert paths, "No tracked apiProperties.json files found"
    results = {name: [] for name in (
        "old_valid", "newly_valid", "still_invalid", "regressions", "invalid_json"
    )}
    for path in paths:
        try:
            instance = json.loads((ROOT / path).read_text(encoding="utf-8-sig"))
        except json.JSONDecodeError as error:
            results["invalid_json"].append({"path": path, "error": str(error)})
            continue
        old_valid = old_validator.is_valid(instance)
        new_errors = list(validator.iter_errors(instance))
        if old_valid:
            results["old_valid"].append(path)
            if new_errors:
                results["regressions"].append(path)
        elif not new_errors:
            results["newly_valid"].append(path)
        else:
            results["still_invalid"].append({
                "path": path,
                "errors": [error.message for error in new_errors],
            })
    counts = {name: len(values) for name, values in results.items()}
    counts["preserved"] = counts["old_valid"] - counts["regressions"]
    counts["new_valid"] = counts["preserved"] + counts["newly_valid"]
    report = os.environ.get("API_PROPERTIES_REPORT")
    if report:
        Path(report).write_text(
            json.dumps({"counts": counts, "files": results}, indent=2),
            encoding="utf-8",
        )
    print("\nCorpus compatibility: " + json.dumps(counts, sort_keys=True))
    assert results["old_valid"], "Baseline accepted no corpus files"
    assert not results["regressions"], results["regressions"]
