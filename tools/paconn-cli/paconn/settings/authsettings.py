# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Represents a settings object consructed from settings.json
"""
from urllib.parse import urljoin


# pylint: disable=too-few-public-methods
class AuthSettings:
    """
    Represents a settings object consructed from settings.json
    """
    # pylint: disable=too-many-arguments
    def __init__(
            self,
            client_id = None,
            tenant = None,
            authority_url = None,
            scopes = None,
            username = None):
        self.client_id = client_id or '04b07795-8ddb-461a-bbee-02f9e1bf7b46'
        self.tenant = tenant or 'common'
        self.authority_url = authority_url or 'https://login.microsoftonline.com/'
        self.scopes = scopes or ['https://service.powerapps.com/.default']
        self.username=username
    
    def get_authority_tenant(self):
        return urljoin(self.authority_url, self.tenant)