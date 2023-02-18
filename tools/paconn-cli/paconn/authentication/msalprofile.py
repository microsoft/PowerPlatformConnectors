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
from paconn.common.util import format_json
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

    def authenticate_silent(self):
        result = None
        account = None
        accounts = self.app.get_accounts()
        if accounts:
            acc_id = 0
            if len(accounts) > 1:
                usernames = list(account['username'] for account in accounts)
                
                # Find the last used username
                acc_id = 1
                try:
                    acc_id = usernames.index(self.auth_settings.username)
                except ValueError:
                    acc_id = 1

                acc_id = prompt_choice_list('Please select an account:', usernames, acc_id)
            account = accounts[acc_id]
            result = self.app.acquire_token_silent(self.auth_settings.scopes, account=account)

        return (result, account)

    def authenticate_device_flow(self):
        """
        Authenticate the end-user using device auth.
        """
        (result, account) = self.authenticate_silent()

        if not result:
            flow = self.app.initiate_device_flow(scopes=self.auth_settings.scopes)
            if "user_code" not in flow:
                raise CLIError("Fail to create device flow. Err: %s" % format_json(flow))

            print(flow['message'])
            sys.stdout.flush()
            
            result = self.app.acquire_token_by_device_flow(flow)

        if not account:
            accounts = self.app.get_accounts()
            if accounts:
                account = accounts[0]

        return (result, account)

    def auth_interactive(self):
        (result, account) = self.authenticate_silent()

        if not result:
            result = self.app.acquire_token_interactive(scopes=self.auth_settings.scopes)

        if not account:
            accounts = self.app.get_accounts()
            if accounts:
                account = accounts[0]

        return (result, account)