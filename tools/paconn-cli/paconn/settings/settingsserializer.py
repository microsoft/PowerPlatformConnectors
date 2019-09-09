# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Represents a settings object consructed from settings.json
"""

import json
from paconn.common.util import format_json
from paconn.settings.settings import Settings

# Connector sepecific settings
_CONNECTOR_ID = 'connectorId'
_ENVIRONMENT = 'environment'

# PowerApps RP settings
_POWERAPPS_URL = 'powerAppsUrl'
_POWERAPPS_API_VERSION = 'powerAppsApiVersion'

# Flow RP Settings
_FLOW_URL = 'flowUrl'
_FLOW_API_VERSION = 'flowApiVersion'

# Files
_API_PROPERTIES = 'apiProperties'
_API_DEFINITION = 'apiDefinition'
_ICON = 'icon'

# Authentication settings
_CLIENT_ID = 'clientId'
_TENANT = 'tenant'
_AUTHORITY_URL = 'authorityUrl'
_RESOURCE = 'resource'


# pylint: disable=too-few-public-methods
class SettingsSerializer:
    """
    Serializes and deserializes a settings object
    """
    @staticmethod
    def to_json_string(settings):
        """
        Serializes a settings object into string
        """
        settings_dict = SettingsSerializer.serialize(settings)
        json_str = format_json(settings_dict)
        return json_str

    @staticmethod
    def to_json(settings, filename):
        """
        Serializes a settings object into the settings.json
        """
        json_str = SettingsSerializer.to_json_string(settings)
        open(filename, 'w').write(json_str)

    @staticmethod
    def from_json(filename):
        """
        Deserializes a settings object from the settings.json file
        """
        with open(filename, 'r') as file:
            settings_dict = json.load(file)  # pylint: disable=attribute-defined-outside-init
        settings = SettingsSerializer.deserialize(settings_dict)
        return settings

    @staticmethod
    def serialize(settings):
        """
        Serializes a settings object into a dict
        """
        settings_dict = {
            # Connector sepecific settings
            _CONNECTOR_ID: settings.connector_id,
            _ENVIRONMENT: settings.environment,

            # Files
            _API_PROPERTIES: settings.api_properties,
            _API_DEFINITION: settings.api_definition,
            _ICON: settings.icon,

            # PowerApps RP settings
            _POWERAPPS_URL: settings.powerapps_url,
            _POWERAPPS_API_VERSION: settings.powerapps_api_version
            # powerapps_base_path IGNORED

            # Flow RP Settings - DO NOT WRITE
            # flow_url IGNORED
            # flow_api_version IGNORED
            # flow_base_path IGNORED

            # Authentication Settings - DO NOT WRITE
            # client_id IGNORED
            # tenant IGNORED
            # authority_url IGNORED
            # resource IGNORED
        }
        return settings_dict

    @staticmethod
    def deserialize(settings_dict):
        """
        Deserializes a dictionary to a settings object
        """
        settings = Settings(
            # Connector sepecific settings
            connector_id=settings_dict.get(_CONNECTOR_ID, None),
            environment=settings_dict.get(_ENVIRONMENT, None),

            # Files
            api_properties=settings_dict.get(_API_PROPERTIES, None),
            api_definition=settings_dict.get(_API_DEFINITION, None),
            icon=settings_dict.get(_ICON, None),

            # PowerApps RP settings
            powerapps_url=settings_dict.get(_POWERAPPS_URL, None),
            powerapps_api_version=settings_dict.get(_POWERAPPS_API_VERSION, None),
            # powerapps_base_path IGNORED

            # Flow RP Settings
            flow_url=settings_dict.get(_FLOW_URL, None),
            flow_api_version=settings_dict.get(_FLOW_API_VERSION, None),
            # flow_base_path IGNORED

            # Authentication Settings
            client_id=settings_dict.get(_CLIENT_ID, None),
            tenant=settings_dict.get(_TENANT, None),
            authority_url=settings_dict.get(_AUTHORITY_URL, None),
            resource=settings_dict.get(_RESOURCE, None)
        )
        return settings
