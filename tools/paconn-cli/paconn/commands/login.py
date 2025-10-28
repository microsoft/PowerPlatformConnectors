# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Login command.
"""

from paconn.authentication.auth import get_authentication, get_authentication_interactive
from paconn.common.util import display
from paconn.settings.settingsbuilder import SettingsBuilder


def login(client_id, tenant, authority_url, resource, settings_file, force, interactive=False):
    """
    Login command.
    
    Args:
        client_id: OAuth2 client ID
        tenant: Azure AD tenant
        authority_url: Authentication authority URL
        resource: Resource URL for the token
        settings_file: Path to settings file
        force: Force re-authentication
        interactive: Use interactive browser authentication instead of device code
    """
    # Get settings
    settings = SettingsBuilder.get_authentication_settings(
        settings_file=settings_file,
        client_id=client_id,
        tenant=tenant,
        authority_url=authority_url,
        resource=resource)

    if interactive:
        get_authentication_interactive(
            settings=settings,
            force_authenticate=force)
    else:
        get_authentication(
            settings=settings,
            force_authenticate=force)
    
    display('Login successful.')
