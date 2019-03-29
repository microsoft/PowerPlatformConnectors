# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
User profile management class.`
"""
import adal
# AADTokenCredentials for multi-factor authentication
from msrestazure.azure_active_directory import AADTokenCredentials

CLIENT_ID = '04b07795-8ddb-461a-bbee-02f9e1bf7b46'
AUTHORITY_URL = 'https://login.microsoftonline.com/{tenant}'
RESOURCE = 'https://management.core.windows.net/'


class Profile:
    """
    A Class representing user profile.
    """

    def __init__(self, resource=RESOURCE, authority_url=AUTHORITY_URL):
        self.resource = resource
        self.authority_url = authority_url

    def _get_authentication_context(self, tenant):
        auth_url = self.authority_url.format(
            tenant=tenant)
        return adal.AuthenticationContext(
            authority=auth_url,
            api_version=None)

    def authenticate_device_code(self, client_id=CLIENT_ID):
        """
        Authenticate the end-user using device auth.
        """
        context = self._get_authentication_context('common')

        code = context.acquire_user_code(
            resource=self.resource,
            client_id=client_id)

        print(code['message'])

        mgmt_token = context.acquire_token_with_device_code(
            resource=self.resource,
            user_code_info=code,
            client_id=client_id)

        credentials = AADTokenCredentials(
            token=mgmt_token,
            client_id=client_id)

        return credentials.token
