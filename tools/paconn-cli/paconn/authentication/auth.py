# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Authentication methods
"""
import os
import sys
from paconn.authentication.msalprofile import MsalProfile
from paconn.settings.authsettings import AuthSettings
from paconn.settings.authsettingsserializer import AuthSettingsSerializer
from paconn.common.util import get_config_dir
from msal_extensions import *

TOKEN_FILE = 'accessTokens.bin'

def _build_persistence(token_file, fallback_to_plaintext=False):
    """Build a suitable persistence instance based your current OS"""
    if sys.platform.startswith('win'):
        return FilePersistenceWithDataProtection(token_file)
        
    if sys.platform.startswith('darwin'):
        return KeychainPersistence(token_file, "paconn", "paconn")

    if sys.platform.startswith('linux'):
        try:
            return LibsecretPersistence(
                token_file,
                schema_name='paconn',
                attributes={"appName": "paconn"})
        except:
            if not fallback_to_plaintext:
                raise
            logging.exception("Encryption unavailable. Opting in to plain text.")
        
    return FilePersistence(token_file)

def _get_token_cache():
    token_file = os.path.join(get_config_dir(), TOKEN_FILE)
    persistence = _build_persistence(token_file)
    token_cache = PersistedTokenCache(persistence)

    return token_cache

def get_authentication(settings, auth_type='interactive', force_interactive=False):
    """
    Logs the user in and saves the token in a file.
    """

    # Read authentication settings
    auth_settings = AuthSettingsSerializer.read_with_cli_settings(settings)

    # Read last saved token
    token_cache = _get_token_cache()
    
    # Get new token
    profile = MsalProfile(
        auth_settings=auth_settings,
        token_cache=token_cache)

    if auth_type == 'interactive':
        (result, account) = profile.auth_interactive(force_interactive=force_interactive)
    else:
        (result, account) = profile.authenticate_silent()
    
    # Save authentication settings
    if account and 'username' in account:
        auth_settings.username = account['username']
    else:
        auth_settings.username = None
    
    AuthSettingsSerializer.write(auth_settings)

    return (result, account)

def get_silent_authentication():
    return get_authentication(
        settings = None,
        auth_type = 'silent')

def remove_authentication():
    # Read authentication settings
    auth_settings = AuthSettingsSerializer.read()
    token_cache = _get_token_cache()
    profile = MsalProfile(
        auth_settings=auth_settings,
        token_cache=token_cache)
    account = profile.logout()
    return account
