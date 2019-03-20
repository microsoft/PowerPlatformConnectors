# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Download command.
"""

from paconn.common.util import display
from paconn.common.settings_util import load_settings
from paconn import _DOWNLOAD

from .save import save


# pylint: disable=too-many-arguments
def download(environment, connector_id, destination, powerapps_url, powerapps_version, settings_file):
    """
    Download command.
    """
    settings, powerapps_rp = load_settings(
        environment=environment,
        settings_file=settings_file,
        connector_id=connector_id,
        powerapps_url=powerapps_url,
        powerapps_version=powerapps_version,
        command_context=_DOWNLOAD)

    directory = save(
        powerapps_rp=powerapps_rp,
        settings=settings,
        destination=destination)

    display('The connector is downloaded to {}.'.format(directory))
