# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
User profile management class.`
"""
import json
import msal
import time


class Profile:
    """
    A Class representing user profile.
    """

    def __init__(self, client_id, tenant, resource, authority_url):
        self.client_id = client_id
        self.tenant = tenant
        self.resource = resource
        self.authority_url = authority_url

    def _get_msal_app(self):
        """
        Creates and returns an MSAL PublicClientApplication instance.
        """
        authority = f"{self.authority_url.rstrip('/')}/{self.tenant}"
        
        return msal.PublicClientApplication(
            client_id=self.client_id,
            authority=authority
        )

    def authenticate_device_code(self):
        """
        Authenticate the end-user using device auth.
        """
        app = self._get_msal_app()
        
        # Start device flow
        flow = app.initiate_device_flow(scopes=[f"{self.resource}/.default"])
        
        if "user_code" not in flow:
            raise ValueError(
                "Fail to create device flow. Error: %s" % json.dumps(flow, indent=2))
        
        print(flow["message"])
        
        # Block until the user has entered the device code
        result = app.acquire_token_by_device_flow(flow)
        
        if "access_token" in result:
            # Convert MSAL result to the expected format
            token_data = {
                'access_token': result['access_token'],
                'token_type': result.get('token_type', 'Bearer'),
                'expires_on': int(time.time()) + result.get('expires_in', 3600),
                'resource': self.resource
            }
            
            if 'id_token_claims' in result and 'oid' in result['id_token_claims']:
                token_data['oid'] = result['id_token_claims']['oid']
            
            return token_data
        else:
            raise Exception(f"Failed to acquire token: {result.get('error_description', 'Unknown error')}")

    def authenticate_interactive(self):
        """
        Authenticate the end-user using interactive authentication (browser).
        """
        app = self._get_msal_app()
        
        # Try to get token silently first
        accounts = app.get_accounts()
        if accounts:
            result = app.acquire_token_silent(
                scopes=[f"{self.resource}/.default"],
                account=accounts[0]
            )
            if result and "access_token" in result:
                # Convert MSAL result to the expected format
                token_data = {
                    'access_token': result['access_token'],
                    'token_type': result.get('token_type', 'Bearer'),
                    'expires_on': int(time.time()) + result.get('expires_in', 3600),
                    'resource': self.resource
                }
                
                if 'id_token_claims' in result and 'oid' in result['id_token_claims']:
                    token_data['oid'] = result['id_token_claims']['oid']
                
                return token_data
        
        # If silent acquisition fails, perform interactive authentication
        result = app.acquire_token_interactive(
            scopes=[f"{self.resource}/.default"],
            prompt="select_account"  # Force account selection
        )
        
        if "access_token" in result:
            # Convert MSAL result to the expected format
            token_data = {
                'access_token': result['access_token'],
                'token_type': result.get('token_type', 'Bearer'),
                'expires_on': int(time.time()) + result.get('expires_in', 3600),
                'resource': self.resource
            }
            
            if 'id_token_claims' in result and 'oid' in result['id_token_claims']:
                token_data['oid'] = result['id_token_claims']['oid']
            
            return token_data
        else:
            raise Exception(f"Failed to acquire token: {result.get('error_description', 'Unknown error')}")
