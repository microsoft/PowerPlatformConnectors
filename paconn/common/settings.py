# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Represents a settings object consructed from settings.json
"""


# pylint: disable=too-few-public-methods
class Settings:
    """
    Represents a settings object consructed from settings.json
    """
    # pylint: disable=too-many-arguments
    def __init__(
            self,
            connector_id,
            environment,
            powerapps_url,
            powerapps_api_version,
            api_properties,
            api_definition,
            icon):
        self.connector_id = connector_id
        self.environment = environment
        self.powerapps_url = powerapps_url
        self.powerapps_api_version = powerapps_api_version
        self.api_properties = api_properties
        self.api_definition = api_definition
        self.icon = icon
