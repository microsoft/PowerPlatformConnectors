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


def get_authentication(client_id, client_secret, tenant):
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
        if not client_id and not client_secret:
            credentials = profile.authenticate_device_code()
        else:
            credentials = profile.authenticate_client_credentials(
                client_id=client_id,
                client_secret=client_secret,
                tenant=tenant)
        tokenmanager.write(credentials)
        token_expired = TokenManager.is_expired(credentials)

    # Couldn't acquire valid token
    if token_expired:
        raise CLIError('Couldn\'t get authentication')
