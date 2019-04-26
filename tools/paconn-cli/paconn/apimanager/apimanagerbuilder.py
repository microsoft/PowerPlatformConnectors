# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
A builder class to create an API Manager object
"""

from urllib.parse import urlparse
from paconn.apimanager.apimanager import APIManager


# pylint: disable=too-few-public-methods
class APIManagerBuilder:
    """
    A builder class to create an API Manager object from an url
    """
    @staticmethod
    def get_from_url(url, base_path, api_version, credentials):
        """
        Creates an APIManager object from given URL, Base Path and credentials
        """
        # pylint: disable=unused-variable
        (scheme, netloc, path, params, query, fragment) = urlparse(url)

        return APIManager(
            scheme=scheme,
            region=None,
            netlocation=netloc,
            base_path=base_path,
            api_version=api_version,
            credentials=credentials)
