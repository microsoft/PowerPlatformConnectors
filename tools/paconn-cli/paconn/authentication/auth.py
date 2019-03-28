# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Authentication methods
"""

from knack.util import CLIError

from .profile import Profile
from .tokenmanager import TokenManager


def get_authentication():
    """
    Logs the user in and saves the token in a file.
    """
    tokenmanager = TokenManager()
    credentials = tokenmanager.read()

    token_expired = TokenManager.is_expired(credentials)

    # Valid token exists
    if not token_expired:
        return

    profile = Profile()

    # Get new token
    if token_expired:
        credentials = profile.authenticate_device_code()
        tokenmanager.write(credentials)
        token_expired = TokenManager.is_expired(credentials)

    # Couldn't acquire valid token
    if token_expired:
        raise CLIError('Couldn\'t get authentication')
