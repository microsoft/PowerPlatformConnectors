# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
A builder class to create a PowerAppsRP object
"""

from paconn.apimanager.apimanagerbuilder import APIManagerBuilder
from paconn.apimanager.flowrp import FlowRP


# pylint: disable=too-few-public-methods
class FlowRPBuilder:
    """
    A builder class to create a FlowRP object
    """
    def get_from_settings(credentials, settings):
        """
        Returns flow rp object from a given settings and credentials.
        """

        # Create the API Manager
        flow_api_manager = APIManagerBuilder.get_from_url(
            url=settings.flow_url,
            base_path=settings.flow_base_path,
            api_version=settings.flow_api_version,
            credentials=credentials)

        flow_rp = FlowRP(api_manager=flow_api_manager)

        return flow_rp
