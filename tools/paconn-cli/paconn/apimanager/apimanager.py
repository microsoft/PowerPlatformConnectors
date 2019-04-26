# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
A manager class for the API calls
"""

import json
from urllib.parse import urljoin, urlencode, urlunparse, quote
import requests

from knack.util import CLIError
from knack.log import get_logger

from paconn.common.util import display, format_json
from paconn.authentication.tokenmanager import (
    _ACCESS_TOKEN,
    _TOKEN_TYPE,
    _OID
)

LOGGER = get_logger(__name__)


class APIManager:
    """
    A manager class for API calls
    """
    # pylint: disable=too-many-arguments
    def __init__(self, scheme, region, netlocation, base_path, api_version, credentials=None):
        self.scheme = scheme

        if not region:
            self.netloc = netlocation
        else:
            self.netloc = '{region}.{netlocation}'.format(
                region=region,
                netlocation=netlocation)

        self.base_path = base_path.rstrip('/') + '/'

        self.api_version = api_version

        self.credentials = credentials

    def add_object_id(self, api):
        """
        Add object id to a given api endpoint
        """
        object_id = self.credentials.get(_OID, '')
        path = 'objectIds/{object_id}/{api}'.format(
            object_id=object_id,
            api=api)

        return path

    def construct_url(self, path, params=None, query=None, fragment=None):
        """
        Contruct a URL from a set of parameters
        """
        urlparts = [''] * 6

        urlparts[0] = self.scheme

        urlparts[1] = self.netloc

        urlparts[2] = urljoin(self.base_path, path)

        if params:
            urlparts[3] = urlencode(params)

        all_query = {
            'api-version': self.api_version
        }

        if query:
            all_query.update(query)
        urlparts[4] = urlencode(all_query, quote_via=quote)

        if fragment:
            urlparts[5] = fragment

        endpoint = urlunparse(urlparts)

        return endpoint

    def request(self, verb, endpoint, headers=None, payload=None):
        """
        Send a request to the given url
        """
        all_headers = {}
        if self.credentials:
            token_type = self.credentials[_TOKEN_TYPE]
            token = self.credentials[_ACCESS_TOKEN]
            all_headers = {
                'Authorization': '{token_type} {token}'.format(
                    token_type=token_type,
                    token=token)
            }
        if headers:
            all_headers.update(headers)

        response = requests.request(
            verb,
            endpoint,
            headers=all_headers,
            json=payload)
        try:
            response.raise_for_status()
        except requests.exceptions.HTTPError as exception:
            exception_str = str(exception)
            response_content = json.loads(response.content)
            response_content = format_json(response_content)
            if payload:
                LOGGER.debug('PAYLOAD')
                LOGGER.debug(payload)
            LOGGER.debug('RESPONSE')
            LOGGER.debug(response_content)
            display(response_content)
            raise CLIError(exception_str)

        return response
