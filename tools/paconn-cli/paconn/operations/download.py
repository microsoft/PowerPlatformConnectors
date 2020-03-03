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
from knack.prompting import prompt_y_n

from paconn.common.util import format_json
from paconn.settings.util import write_settings, SETTINGS_FILE

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

    # Use the destination directory when provided
    if destination:
        if not os.path.exists(destination):
            # Create all sub-directories
            os.makedirs(destination)
    # Create a sub-directory in the current directory
    # when a destination isn't provided
    else:
        if os.path.isdir(connector_id):
            error = '{} directory already exists. Please remove the directory before continuing.'
            raise CLIError(error.format(connector_id))
        else:
            os.mkdir(connector_id)
            destination = connector_id

    if os.path.isdir(destination):
        os.chdir(destination)
    else:
        error = 'Couldn\'t download to the desination directory {}.'
        raise CLIError(error.format(destination))

    return os.getcwd()


def _ensure_overwrite(settings):
    """
    Ensure the files can be overwritten, if exists
    """
    overwrite = False
    files = [settings.api_properties, settings.api_definition, settings.icon, SETTINGS_FILE]
    existing_files = [file for file in files if os.path.exists(file)]
    if len(existing_files) > 0:
        msg = '{} file(s) exist. Do you want to overwrite?'.format(existing_files)
        overwrite = prompt_y_n(msg)
        if not overwrite:
            raise CLIError('{} files not overwritten.'.format(existing_files))

    return overwrite


def download(powerapps_rp, settings, destination, overwrite):
    """
    Download operation.
    """
    # Prepare folders
    directory = _prepare_directory(
        destination=destination,
        connector_id=settings.connector_id)

    # Check if files could be overwritten
    if not overwrite:
        overwrite = _ensure_overwrite(settings)

    api_registration = powerapps_rp.get_connector(
        environment=settings.environment,
        connector_id=settings.connector_id)

    if _PROPERTIES not in api_registration:
        raise CLIError('Properties not present in the api registration information.')

    api_properties = api_registration[_PROPERTIES]

    # Save the settings
    write_settings(settings, overwrite)

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
    api_prop = format_json(
        content=api_properties_selected,
        sort_keys=False)

    open(
        file=settings.api_properties,
        mode='w'
        ).write(api_prop)

    # Write the open api definition,
    # either from swagger URL when available or from swagger property.
    if _API_DEFINITIONS in api_properties and _ORIGINAL_SWAGGER_URL in api_properties[_API_DEFINITIONS]:
        original_swagger_url = api_properties[_API_DEFINITIONS][_ORIGINAL_SWAGGER_URL]
        response = requests.get(original_swagger_url, allow_redirects=True)
        response_string = response.content.decode('utf-8-sig')

        swagger = format_json(
            content=json.loads(response_string),
            sort_keys=False)

        open(
            file=settings.api_definition,
            mode='w'
            ).write(swagger)

    # Write the icon
    if _ICON_URI in api_properties:
        icon_url = api_properties[_ICON_URI]
        response = requests.get(icon_url, allow_redirects=True)

        open(
            file=settings.icon,
            mode='wb'
            ).write(response.content)

    return directory
