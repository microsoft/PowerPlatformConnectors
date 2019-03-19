# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Utility for loading settings.
"""

from paconn import _CREATE, _DOWNLOAD
from paconn.apimanager.apimanager import APIManagerBuilder
from paconn.apimanager.powerappsrp import PowerAppsRP
from paconn.authentication.tokenmanager import TokenManager

from .settings import Settings
from .settings_serializer import SettingsSerializer
from .prompts import get_environment, get_connector_id

# PowerApps URL
_DEFAULT_POWERAPPS_URL = 'https://preview.api.powerapps.com'
_DEFAULT_POWERAPPS_BASE_PATH = 'providers/Microsoft.PowerApps'
_DEFAULT_POWERAPPS_API_VERSION = '2016-11-01'

# File names
API_PROPERTIES_FILE = 'apiProperties.json'
API_DEFINITION_FILE = 'apiDefinition.swagger.json'
ICON_FILE = 'icon.png'
SETTINGS_FILE = 'settings.json'


# pylint: disable=too-many-arguments
def load_settings(
        environment,
        settings_file,
        connector_id,
        powerapps_url,
        powerapps_version,
        command_context,
        api_properties=API_PROPERTIES_FILE,
        api_definition=API_DEFINITION_FILE,
        icon=ICON_FILE):
    """
    Loads settings into a settings object.
    """
    # Get Authentication Token
    tokenmanager = TokenManager()
    credentials = tokenmanager.get_credentials()

    if settings_file:
        settings = SettingsSerializer.from_json(settings_file)
    else:
        settings = Settings(
            connector_id=connector_id,
            environment=environment,
            powerapps_url=powerapps_url,
            powerapps_api_version=powerapps_version,
            api_properties=api_properties,
            api_definition=api_definition,
            icon=icon
        )

    if not hasattr(settings, 'powerapps_url') or not settings.powerapps_url:
        settings.powerapps_url = _DEFAULT_POWERAPPS_URL
    if not hasattr(settings, 'powerapps_api_version') or not settings.powerapps_api_version:
        settings.powerapps_api_version = _DEFAULT_POWERAPPS_API_VERSION

    if command_context is _DOWNLOAD:
        if not hasattr(settings, 'api_properties') or not settings.api_properties:
            settings.api_properties = api_properties
        if not hasattr(settings, 'api_definition') or not settings.api_definition:
            settings.api_definition = api_definition
        if not hasattr(settings, 'icon') or not settings.icon:
            settings.icon = icon

    # Select environment if not provided
    if not hasattr(settings, 'environment') or not settings.environment:
        settings.environment = get_environment(
            credentials=credentials)

    # Create the API Manager
    powerapps_api_manager = APIManagerBuilder.get_from_url(
        url=settings.powerapps_url,
        base_path=_DEFAULT_POWERAPPS_BASE_PATH,
        api_version=settings.powerapps_api_version,
        credentials=credentials)

    powerapps_rp = PowerAppsRP(api_manager=powerapps_api_manager)

    # Select connector id if not provided
    if (not hasattr(settings, 'connector_id') or not settings.connector_id) and command_context is not _CREATE:
        settings.connector_id = get_connector_id(
            powerapps_rp=powerapps_rp,
            environment=settings.environment)

    return settings, powerapps_rp


def save_settings(settings, settings_file=SETTINGS_FILE):
    """
    Saves settings.
    """
    # Write the setings.json
    if settings:
        SettingsSerializer.to_json(settings, settings_file)
