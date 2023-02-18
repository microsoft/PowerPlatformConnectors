# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Represents a settings object consructed from settings.json
"""

import os
import json
from paconn.common.util import format_json
from paconn.common.util import get_config_dir
from paconn.settings.authsettings import AuthSettings
from knack.util import CLIError

# Authentication settings
_CLIENT_ID = 'clientId'
_TENANT = 'tenant'
_AUTHORITY_URL = 'authorityUrl'
_SCOPES = 'scopes'
_USERNAME = 'username'

AUTH_SETTINGS_FILE = 'authSettings.json'

# pylint: disable=too-few-public-methods
class AuthSettingsSerializer:
    """
    Serializes and deserializes a settings object
    """
    @staticmethod
    def settings_file():
        return os.path.join(get_config_dir(), AUTH_SETTINGS_FILE)

    """
    Serializes and deserializes a settings object
    """
    @staticmethod
    def to_json_string(settings):
        """
        Serializes a settings object into string
        """
        settings_dict = AuthSettingsSerializer.serialize(settings)
        return format_json(settings_dict)

    @staticmethod
    def write(settings):
        """
        Serializes a settings object into the settings.json
        """
        settings_file = AuthSettingsSerializer.settings_file()
        json_str = AuthSettingsSerializer.to_json_string(settings)
        try:
            with os.fdopen(os.open(settings_file, os.O_RDWR | os.O_CREAT | os.O_TRUNC, 0o600), 'w+') as auth_settings_file:
                auth_settings_file.write(json_str)
        except Exception as exception:
            raise CLIError("Failed to save auth settings. (Inner Error: {})".format(exception))

    @staticmethod
    def read_with_cli_settings(cli_settings):
        """
        Deserializes a settings object and override if the settings provided in cli
        """
        settings = AuthSettingsSerializer.read();

        if not settings:
            return cli_settings

        if not cli_settings:
            return settings

        return AuthSettings(
            client_id = cli_settings.client_id or settings.client_id,
            tenant = cli_settings.tenant or settings.tenant,
            authority_url = cli_settings.authority_url or settings.authority_url,
            scopes = cli_settings.scopes or settings.scopes,
            username = cli_settings.username or settings.username
        )

    @staticmethod
    def read():
        """
        Deserializes a settings object
        """
        settings = None

        settings_file = AuthSettingsSerializer.settings_file()

        if os.path.isfile(settings_file):
            try:
                with open(settings_file, 'r') as file:
                    settings_dict = json.load(file)  # pylint: disable=attribute-defined-outside-init
            except ValueError as exception:
                raise CLIError("Failed to load auth settings. (Inner Error: {})".format(exception))

            settings = AuthSettingsSerializer.deserialize(settings_dict)
        return settings

    @staticmethod
    def serialize(settings):
        """
        Serializes a settings object into a dict
        """
        return {
            _CLIENT_ID: settings.client_id,
            _TENANT: settings.tenant,
            _AUTHORITY_URL: settings.authority_url,
            _SCOPES: settings.scopes,
            _USERNAME: settings.username
        }

    @staticmethod
    def deserialize(settings_dict):
        """
        Deserializes a dictionary to a settings object
        """
        return AuthSettings(
            client_id=settings_dict.get(_CLIENT_ID, None),
            tenant=settings_dict.get(_TENANT, None),
            authority_url=settings_dict.get(_AUTHORITY_URL, None),
            scopes=settings_dict.get(_SCOPES, None),
            username=settings_dict.get(_USERNAME, None)
        )
