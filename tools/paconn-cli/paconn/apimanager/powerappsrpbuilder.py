# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
A builder class to create a PowerAppsRP object
"""

from paconn.apimanager.apimanagerbuilder import APIManagerBuilder
from paconn.apimanager.powerappsrp import PowerAppsRP


# pylint: disable=too-few-public-methods
class PowerAppsRPBuilder:
    """
    A builder class to create a PowerAppsRP object
    """
    def get_from_settings(credentials, settings):
        """
        Returns powerapps rp object from a given settings and credentials.
        """

        # Create the API Manager
        powerapps_api_manager = APIManagerBuilder.get_from_url(
            url=settings.powerapps_url,
            base_path=settings.powerapps_base_path,
            api_version=settings.powerapps_api_version,
            credentials=credentials)

        powerapps_rp = PowerAppsRP(api_manager=powerapps_api_manager)
        return powerapps_rp
