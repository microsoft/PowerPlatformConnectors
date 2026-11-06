# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Authentication methods
"""

from knack.util import CLIError

from paconn.authentication.profile import Profile
from paconn.authentication.tokenmanager import TokenManager


def get_authentication(settings, force_authenticate, interactive=False):
    """
    Logs the user in and saves the token in a file.
    
    Args:
        settings: Configuration settings containing authentication parameters
        force_authenticate: Force re-authentication even if token exists
        interactive: Use interactive browser authentication instead of device code
    """
    tokenmanager = TokenManager()
    credentials = tokenmanager.read()

    token_expired = TokenManager.is_expired(credentials)

    # Get new token
    if token_expired or force_authenticate:
        profile = Profile(
            client_id=settings.client_id,
            tenant=settings.tenant,
            resource=settings.resource,
            authority_url=settings.authority_url)

        if interactive:
            credentials = profile.authenticate_interactive()
        else:
            credentials = profile.authenticate_device_code()

        tokenmanager.write(credentials)

        token_expired = TokenManager.is_expired(credentials)

    # Couldn't acquire valid token
    if token_expired:
        raise CLIError('Couldn\'t get authentication')


def get_authentication_interactive(settings, force_authenticate=False):
    """
    Logs the user in using interactive authentication and saves the token in a file.
    """
    return get_authentication(settings, force_authenticate, interactive=True)


def remove_authentication():
    tokenmanager = TokenManager()
    tokenmanager.delete_token_file()
