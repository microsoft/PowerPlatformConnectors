# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Download command.
"""

from paconn import _DOWNLOAD

from paconn.common.util import display
from paconn.settings.util import load_settings_and_powerapps_rp

import paconn.operations.download


# pylint: disable=too-many-arguments
def download(environment, connector_id, destination, powerapps_url, powerapps_version, settings_file):
    """
    Download command.
    """
    settings, powerapps_rp, _ = load_settings_and_powerapps_rp(
        environment=environment,
        settings_file=settings_file,
        connector_id=connector_id,
        powerapps_url=powerapps_url,
        powerapps_version=powerapps_version,
        command_context=_DOWNLOAD,
        api_properties=None,
        api_definition=None,
        icon=None)

    directory = paconn.operations.download.download(
        powerapps_rp=powerapps_rp,
        settings=settings,
        destination=destination)

    display('The connector is downloaded to {}.'.format(directory))
