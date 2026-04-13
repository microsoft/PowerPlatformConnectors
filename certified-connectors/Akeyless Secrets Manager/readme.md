# Akeyless Secrets Manager

This connector calls the Akeyless REST API to retrieve secret values using **Access ID** and **Access Key** on each action, matching the layout exported from a Power Platform managed solution (`openapidefinition.json` + empty `connectionparameters.json` + `GetSecret` / `GetPassword` script operations).

## Publisher: Akeyless

## Prerequisites

- Akeyless account with permission to authenticate and read the target secrets.
- An **API Key** authentication method with **Access ID** and **Access Key** ([Authenticate with API Key](https://docs.akeyless.io/docs/auth-with-api-key)).

## Where to enter Access Id and Access Key

The exported connector uses **no connection-level parameters** (`connectionParameters` is empty). In Power Automate / Power Apps, **each action** shows **Access Id**, **Access Key**, and **Secret name** (and any other body fields from the OpenAPI). Users enter credentials **on the action card** for every run (or map them from variables—avoid logging secrets).

If you later want a single **connection** that stores Access Id/Key once, add `username` / `password` (or similar) under `connectionParameters` in `apiProperties.json` and extend `script.csx` to read the `Authorization: Basic` header—this repo was aligned to the **zip export** you shared, not that pattern.

## Supported operations

### Get Secret

`POST` path `/get-secret-value/text` — retrieves the secret value (same operation id as export: `GetSecret`).

### Get Password

`POST` path `/get-secret-value/json` — same Akeyless `/get-secret-value` call as in the exported script (`GetPassword`).

**Note:** The managed solution’s `customizations.xml` description may mention list/describe; the **exported** OpenAPI and script only contained these **two** operations. This GitHub folder matches that export.

## Known issues and limitations

- **Per-action credentials:** Access Id and Access Key are supplied on **each** action, not on a connection screen (unless you change `apiProperties.json` as above).
- **`iconBrandColor`:** Set to `#0E4D45` so it is not `#ffffff` or `#007ee5` (certified connector rules). Replace with marketing-approved color if required.
- **Read-focused:** Get secret / password only; no list-items in this aligned version.

## Deployment instructions

1. Validate: `paconn validate --api-def apiDefinition.swagger.json`.
2. Import `apiDefinition.swagger.json`, `apiProperties.json`, and `script.csx` as a custom connector and test **Get Secret** and **Get Password** with real Akeyless paths.

## Certification

Open source under `certified-connectors/`, PR to `dev`, label `certified-connector`; after merge, submit via [ISV Studio](https://isvstudio.powerapps.com) per [Submit your connector for certification](https://learn.microsoft.com/connectors/custom-connectors/submit-certification).
