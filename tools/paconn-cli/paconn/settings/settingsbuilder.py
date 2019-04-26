# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Represents a settings builder class
"""

from paconn.settings.settings import Settings
from paconn.settings.settingsserializer import SettingsSerializer


# pylint: disable=too-few-public-methods
class SettingsBuilder:
    """
    A builder class to create a Settings object
    """

    # pylint: disable=too-many-arguments
    @staticmethod
    def get_settings(
            environment,
            settings_file,
            connector_id,
            powerapps_url,
            powerapps_version,
            command_context,
            api_properties,
            api_definition,
            icon):
        """
        Loads settings into a settings object.
        """

        # Load from settings file if it is available
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
        return settings
