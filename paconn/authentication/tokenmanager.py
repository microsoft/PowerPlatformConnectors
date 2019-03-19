# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Token file manager.
"""

import os
import json

import time

from knack.util import CLIError

from paconn.common.util import get_config_dir

TOKEN_FILE = 'accessTokens.json'

# Token specific variables
_TOKEN_TYPE = 'token_type'
_ACCESS_TOKEN = 'access_token'
_EXPIRES_ON = 'expires_on'
_OID = 'oid'


# Number of seconds to request a login before the token expires
TOKEN_BUFFER_SECONDS = 600


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
        creds = []
        if os.path.isfile(self.token_file):
            try:
                with open(self.token_file, 'r') as file:
                    creds = json.load(file)
            except ValueError as exception:
                raise CLIError("Failed to load token files. (Inner Error: {})".format(exception))
        return creds

    def write(self, credentials):
        """
        Writes the login credentials to a token file.
        """
        with os.fdopen(os.open(self.token_file, os.O_RDWR | os.O_CREAT | os.O_TRUNC, 0o600), 'w+') as cred_file:
            cred_file.write(json.dumps(credentials))

    @staticmethod
    def is_expired(credentials):
        """
        Returns true if the token is expired.
        """
        token_expired = _ACCESS_TOKEN not in credentials

        # Check for timeout
        if not token_expired and _EXPIRES_ON in credentials:
            # time.time() returns number of seconds since epoch, 01/01/1970 UTC
            expiration_time = time.time() + TOKEN_BUFFER_SECONDS
            token_expired = credentials[_EXPIRES_ON] < expiration_time

        return token_expired
