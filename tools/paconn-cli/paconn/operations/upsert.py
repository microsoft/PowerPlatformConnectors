# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Method for create/update operation
"""

import os
import json
import urllib.parse

from knack.util import CLIError

from paconn.common.util import ensure_file_exists
from paconn.settings.util import write_settings
from paconn.apimanager.iconuploader import upload_icon
from paconn.operations.json_keys import (
    _PROPERTIES,
    _ICON_URI,
    _OPEN_API_DEFINITION,
    _ENVIRONMENT,
    _NAME,
    _BACKEND_SERVICE,
    _SERVICE_URL,
    _SCHEMES,
    _HOST,
    _BASE_PATH,
    _DISPLAY_NAME,
    _CONNECTION_PARAMETERS,
    _TOKEN,
    _OAUTH_SETTINGS,
    _CLIENT_SECRET,
    _DESCRIPTION,
    _INFO,
    _TITLE,
    _SHARED_ACCESS_SIGNATURE
)


def _create_backendservice_url(openapi_definition):
    """
    Create a backend service URL from the Open API definition.
    """

    schemes = openapi_definition.get(_SCHEMES, [])
    scheme = next(iter(schemes), '')
    netloc = openapi_definition.get(_HOST, '')
    path = openapi_definition.get(_BASE_PATH, '')

    params = ''
    query = ''
    fragment = ''

    parts = (scheme, netloc, path, params, query, fragment)

    url = urllib.parse.urlunparse(parts)

    return url


def upsert(powerapps_rp, settings, client_secret, is_update, overwrite_settings):
    """
    Method for create/update operation
    """

    # Make sure the required files exist
    ensure_file_exists(
        file=settings.api_properties,
        file_type='API Properties')
    ensure_file_exists(
        file=settings.api_definition,
        file_type='API Definition')

    # Open the property file
    with open(settings.api_properties, 'r') as file:
        property_definition = json.load(file)

    # Get the property object
    properties = property_definition[_PROPERTIES]

    # Add secret
    token_property = properties.get(_CONNECTION_PARAMETERS, {}).get(_TOKEN, None)
    if token_property:
        oauth_settings = token_property.get(_OAUTH_SETTINGS, None)
        if oauth_settings and client_secret:
            oauth_settings[_CLIENT_SECRET] = client_secret
        elif oauth_settings and not client_secret and not is_update:
            raise CLIError('Please provide OAuth2 client secret using the --secret argument.')

    # Load swagger definition
    with open(settings.api_definition, 'r') as file:
        openapi_definition = json.load(file)

    # Append swagger
    properties[_OPEN_API_DEFINITION] = openapi_definition

    # Add backend service
    backend_service_url = _create_backendservice_url(openapi_definition)
    properties[_BACKEND_SERVICE] = {_SERVICE_URL: backend_service_url}

    # Append the environment id
    properties[_ENVIRONMENT] = {_NAME: settings.environment}

    # Add displayName only when creating a new connector
    if is_update is not True:
        properties[_DISPLAY_NAME] = openapi_definition[_INFO][_TITLE]

    # Add description
    properties[_DESCRIPTION] = openapi_definition[_INFO][_DESCRIPTION]

    # Validate Open API Definition
    powerapps_rp.validate_connector(
        payload=openapi_definition,
        enable_certification_rules=False)

    # Get the shared access signature
    response = powerapps_rp.generate_resource_storage(settings.environment)
    sas_url = response[_SHARED_ACCESS_SIGNATURE]

    # Upload the icon
    if settings.icon and os.path.exists(settings.icon):
        icon_uri = upload_icon(
            sas_url=sas_url,
            file_path=settings.icon)
        properties[_ICON_URI] = icon_uri

    # Update or create the connector
    if is_update is True:
        api_registration = powerapps_rp.update_connector(
            environment=settings.environment,
            connector_id=settings.connector_id,
            payload=property_definition)
        connector_id = settings.connector_id
    else:
        api_registration = powerapps_rp.create_connector(
            environment=settings.environment,
            payload=property_definition)
        connector_id = json.loads(api_registration)[_NAME]

        # Save the settings
        settings.connector_id = connector_id
        write_settings(settings, overwrite_settings)

    return connector_id
