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
            api_properties,
            api_definition,
            icon,
            client_id=None,
            tenant=None,
            authority_url=None,
            resource=None):
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
                icon=icon,
                client_id=client_id,
                tenant=tenant,
                authority_url=authority_url,
                resource=resource
            )
        return settings

    # pylint: disable=too-many-arguments
    @staticmethod
    def get_authentication_settings(
            settings_file,
            client_id,
            tenant,
            authority_url,
            resource):
        """
        Loads settings into a settings object.
        """

        # Load from settings file if it is available
        if settings_file:
            settings = SettingsSerializer.from_json(settings_file)
        else:
            settings = Settings(
                connector_id=None,
                environment=None,
                powerapps_url=None,
                powerapps_api_version=None,
                api_properties=None,
                api_definition=None,
                icon=None,
                client_id=client_id,
                tenant=tenant,
                authority_url=authority_url,
                resource=resource
            )
        return settings
