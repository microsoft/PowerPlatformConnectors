# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Login command.
"""

from paconn.authentication.auth import get_authentication
from paconn.common.util import display
from paconn.settings.authsettings import AuthSettings


def login(client_id, tenant, authority_url, scopes):
    """
    Login command.
    """

    # Convert string scope to array
    if scopes and type(scopes) == str:
        scopes = [] + scopes

    # Scope must be an array
    if scopes and not hasattr(scopes, "__len__"):
        display('Scopes must be an array of scopes or a string scope.')
        return

    # Get settings
    settings = AuthSettings(
        client_id=client_id,
        tenant=tenant,
        authority_url=authority_url,
        scopes=scopes)

    (result, account) = get_authentication(settings=settings)

    if result:
        if account:
            display('Logged in with account {}.'.format(account['username']))
        else:
            display('Login successful.')
    else:
        display('Login failed.')
