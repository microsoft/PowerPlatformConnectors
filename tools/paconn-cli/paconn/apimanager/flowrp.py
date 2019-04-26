# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Flow API Manager
"""


# pylint: disable=too-few-public-methods
class FlowRP:
    """
    Flow API Manager class
    """
    def __init__(self, api_manager):
        self.api_manager = api_manager

    def get_environments(self):
        """
        Returns a list of environments
        """
        endpoint = self.api_manager.construct_url(
            path='environments')

        response = self.api_manager.request(
            verb='GET',
            endpoint=endpoint)

        return response.json()
