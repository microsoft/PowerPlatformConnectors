# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
User profile management class.`
"""
import adal
from urllib.parse import urljoin
# AADTokenCredentials for multi-factor authentication
from msrestazure.azure_active_directory import AADTokenCredentials


class Profile:
    """
    A Class representing user profile.
    """

    def __init__(self, client_id, tenant, resource, authority_url):
        self.client_id = client_id
        self.tenant = tenant
        self.resource = resource
        self.authority_url = authority_url

    def _get_authentication_context(self):
        auth_url = urljoin(self.authority_url, self.tenant)

        return adal.AuthenticationContext(
            authority=auth_url,
            api_version=None)

    def authenticate_device_code(self):
        """
        Authenticate the end-user using device auth.
        """
        context = self._get_authentication_context()

        code = context.acquire_user_code(
            resource=self.resource,
            client_id=self.client_id)

        print(code['message'])

        mgmt_token = context.acquire_token_with_device_code(
            resource=self.resource,
            user_code_info=code,
            client_id=self.client_id)

        credentials = AADTokenCredentials(
            token=mgmt_token,
            client_id=self.client_id)

        return credentials.token
