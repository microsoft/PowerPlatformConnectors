# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Save operation.
"""

import os
import json
import requests

from knack.util import CLIError

from paconn.common.util import format_json
from paconn.settings.settingsserializer import SettingsSerializer

from paconn.operations.json_keys import (
    _PROPERTIES,
    _API_DEFINITIONS,
    _ORIGINAL_SWAGGER_URL,
    _ICON_URI,
    _CONNECTION_PARAMETERS,
    _ICON_BRAND_COLOR,
    _CAPABILITIES,
    _POLICY_TEMPLATE_INSTANCES
)


def _prepare_directory(destination, connector_id):
    """
    Create directory for saving a connector.
    """
    if destination and os.path.isdir(destination):
        os.chdir(destination)

    if os.path.isdir(connector_id):
        dir_error = '{} directory already exists. Please remove the directory before continuing.'
        raise CLIError(dir_error.format(connector_id))
    os.mkdir(connector_id)
    os.chdir(connector_id)

    return os.getcwd()


def download(powerapps_rp, settings, destination):
    """
    Download operation.
    """
    # Prepare folders
    directory = _prepare_directory(
        destination=destination,
        connector_id=settings.connector_id)

    api_registration = powerapps_rp.get_connector(
        environment=settings.environment,
        connector_id=settings.connector_id)

    if _PROPERTIES not in api_registration:
        raise CLIError('Properties not present in the api registration information.')

    api_properties = api_registration[_PROPERTIES]

    # Save the settings
    SettingsSerializer.to_json(settings, 'settings.json')

    # Property whitelist
    property_keys_whitelist = [
        _CONNECTION_PARAMETERS,
        _ICON_BRAND_COLOR,
        _CAPABILITIES,
        _POLICY_TEMPLATE_INSTANCES
    ]

    # Remove the keys that aren't present in the property JSON
    properties_present = list(
        filter(lambda prop: prop in api_properties, property_keys_whitelist)
    )

    # Only output the white listed properties that are present in the property JSON
    api_properties_selected = {_PROPERTIES: {}}
    api_properties_selected[_PROPERTIES] = {
        prop: api_properties[prop]
        for prop in properties_present
    }

    # Write the api properties
    open(settings.api_properties, mode='w').write(format_json(api_properties_selected))

    # Write the open api definition,
    # either from swagger URL when available or from swagger property.
    if _API_DEFINITIONS in api_properties and _ORIGINAL_SWAGGER_URL in api_properties[_API_DEFINITIONS]:
        original_swagger_url = api_properties[_API_DEFINITIONS][_ORIGINAL_SWAGGER_URL]
        response = requests.get(original_swagger_url, allow_redirects=True)
        response_string = response.content.decode('utf-8-sig')
        swagger = format_json(json.loads(response_string))
        open(settings.api_definition, mode='w').write(swagger)

    # Write the icon
    if _ICON_URI in api_properties:
        icon_url = api_properties[_ICON_URI]
        response = requests.get(icon_url, allow_redirects=True)
        open(settings.icon, mode='wb').write(response.content)

    return directory
