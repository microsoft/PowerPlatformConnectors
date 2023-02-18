# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Authentication methods
"""
from paconn.authentication.msalprofile import MsalProfile
from paconn.authentication.tokenmanager import TokenManager
from paconn.settings.authsettings import AuthSettings
from paconn.settings.authsettingsserializer import AuthSettingsSerializer

def get_authentication(settings, auth_type='interactive'):
    """
    Logs the user in and saves the token in a file.
    """
    
    # Read authentication settings
    auth_settings = AuthSettingsSerializer.read_with_cli_settings(settings)

    # Read last saved token
    tokenmanager = TokenManager()
    token_cache = tokenmanager.read()
    
    # Get new token
    profile = MsalProfile(
        auth_settings=auth_settings,
        token_cache=token_cache)

    if auth_type == 'interactive':
        (result, account) = profile.auth_interactive()
    else:
        (result, account) = profile.authenticate_silent()
    
    # Save token
    if token_cache.has_state_changed:
        tokenmanager.write(token_cache)

    # Save authentication settings
    if account:
        auth_settings.username = account['username']
        AuthSettingsSerializer.write(auth_settings)

    return (result, account)

def get_silent_authentication():
    return get_authentication(
        settings = None,
        auth_type = 'silent')

def remove_authentication():
    tokenmanager = TokenManager()
    tokenmanager.delete_token_file()
