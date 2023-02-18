# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Token file manager.
"""

import os
import msal
from knack.util import CLIError
from paconn.common.util import get_config_dir

TOKEN_FILE = 'accessTokens.json'


class TokenManager:
    """
    Class to manager login token.
    """
    def __init__(self, token_file=TOKEN_FILE):
        self.token_file = os.path.join(get_config_dir(), token_file)

    def get_credentials(self):
        """
        Returns credential object from token file.
        """
        credentials = self.read()
        token_expired = TokenManager.is_expired(credentials)
        if token_expired:
            raise CLIError('Access token invalid. Please login again.')

        return credentials

    def read(self):
        """
        Reads a login token file.
        """
        token_cache = msal.SerializableTokenCache()
        if os.path.isfile(self.token_file):
            try:
                with open(self.token_file, 'r') as cred_file:
                    token_cache.deserialize(cred_file.read())
            except ValueError as exception:
                raise CLIError("Failed to load access token. (Inner Error: {})".format(exception))
        return token_cache

    def write(self, token_cache):
        """
        Writes the login credentials to a token file.
        """
        try:
            with os.fdopen(os.open(self.token_file, os.O_RDWR | os.O_CREAT | os.O_TRUNC, 0o600), 'w+') as cred_file:
                cred_file.write(token_cache.serialize())
        except Exception as exception:
            raise CLIError("Failed to save access token. (Inner Error: {})".format(exception))

    def delete_token_file(self):
        if os.path.isfile(self.token_file):
            os.remove(self.token_file)
