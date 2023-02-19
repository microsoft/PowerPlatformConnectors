# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
User profile management class.`
"""
from msal import PublicClientApplication
from knack.prompting import prompt_choice_list
from knack.util import CLIError
from paconn.settings.authsettings import AuthSettings

class MsalProfile:
    """
    A Class representing user profile.
    """

    def __init__(self, auth_settings, token_cache = None):
        self.auth_settings = auth_settings

        self.app = PublicClientApplication(
            client_id=auth_settings.client_id,
            authority=auth_settings.get_authority_tenant(),
            token_cache=token_cache)

    def get_account_from_username(self, username, prompt=False):
        accounts = self.app.get_accounts(username)
        if accounts:
            return accounts[0]

        return None

    def authenticate_silent(self):
        account = self.get_account_from_username(
            username=self.auth_settings.username)

        result = self.app.acquire_token_silent_with_error(
            scopes=self.auth_settings.scopes,
            account=account,
            force_refresh=True)

        return (result, account)

    def get_account_from_username_prompt(self, username, prompt_option):
        account = None
        accounts = None

        accounts = self.app.get_accounts()

        if accounts:
            acc_id = 0
            usernames = list(account['username'] for account in accounts)

            try:
                acc_id = usernames.index(username)
            except ValueError:
                acc_id = len(usernames)
            
            usernames.append(prompt_option)

            acc_id = acc_id + 1
            acc_id = prompt_choice_list(
                'Please select an account:',
                usernames, 
                acc_id)

            if acc_id < len(accounts):
                account = accounts[acc_id]

        return account

    def authenticate_silent_prompt(self):
        account = self.get_account_from_username_prompt(
            username=self.auth_settings.username,
            prompt_option='Add a new account')

        result = self.app.acquire_token_silent_with_error(
            scopes=self.auth_settings.scopes,
            account=account,
            force_refresh=True)

        return (result, account)

    def auth_interactive(self, force_interactive=False):
        (result, account) = (None, None)

        if not force_interactive:
            (result, account) = self.authenticate_silent_prompt()

        if not result or 'error' in result:
            if not account:
                account = self.get_account_from_username(self.auth_settings.username)

            username = None
            if account and 'username' in account:
                username = account['username']

            result = self.app.acquire_token_interactive(
                scopes=self.auth_settings.scopes, 
                login_hint=username,
                prompt='select_account')

            if result and not 'error' in result:
                if 'id_token' in result and 'preferred_username' in result['id_token_claims']:
                    username = result['id_token_claims']['preferred_username']
                    account = self.get_account_from_username(username)

        return (result, account)

    def logout(self):
        account = self.get_account_from_username_prompt(
            username=self.auth_settings.username,
            prompt_option='None')

        if account:
            self.app.remove_account(account)
        
        return account