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
from .settings import Settings

_CONNECTOR_ID = 'connectorId'
_ENVIRONMENT = 'environment'
_POWERAPPS_URL = 'powerAppsUrl'
_POWERAPPS_API_VERSION = 'powerAppsApiVersion'
_API_PROPERTIES = 'apiProperties'
_API_DEFINITION = 'apiDefinition'
_ICON = 'icon'


# pylint: disable=too-few-public-methods
class SettingsSerializer:
    """
    Serializes and deserializes a settings object
    """
    @staticmethod
    def to_json(settings, filename):
        """
        Serializes a settings object into the settings.json
        """
        settings_dict = SettingsSerializer.serialize(settings)
        json_str = format_json(settings_dict)
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
            _CONNECTOR_ID: settings.connector_id,
            _ENVIRONMENT: settings.environment,
            _POWERAPPS_URL: settings.powerapps_url,
            _POWERAPPS_API_VERSION: settings.powerapps_api_version,
            _API_PROPERTIES: settings.api_properties,
            _API_DEFINITION: settings.api_definition,
            _ICON: settings.icon
        }
        return settings_dict

    @staticmethod
    def deserialize(settings_dict):
        """
        Deserializes a dictionary to a settings object
        """
        settings = Settings(
            connector_id=settings_dict.get(_CONNECTOR_ID, None),
            environment=settings_dict.get(_ENVIRONMENT, None),
            powerapps_url=settings_dict.get(_POWERAPPS_URL, None),
            powerapps_api_version=settings_dict.get(_POWERAPPS_API_VERSION, None),
            api_properties=settings_dict.get(_API_PROPERTIES, None),
            api_definition=settings_dict.get(_API_DEFINITION, None),
            icon=settings_dict.get(_ICON, None)
        )
        return settings
