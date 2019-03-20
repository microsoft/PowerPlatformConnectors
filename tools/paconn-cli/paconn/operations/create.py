# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Create command.
"""

from paconn.common.util import display
from paconn.common.settings_util import load_settings
from paconn import _CREATE

from .create_update import create_update


# pylint: disable=too-many-arguments
def create(
        environment,
        api_properties,
        api_definition,
        icon,
        powerapps_url,
        powerapps_version,
        client_secret,
        settings_file):
    """
    Create command.
    """
    settings, powerapps_rp = load_settings(
        environment=environment,
        settings_file=settings_file,
        api_properties=api_properties,
        api_definition=api_definition,
        icon=icon,
        connector_id=None,
        powerapps_url=powerapps_url,
        powerapps_version=powerapps_version,
        command_context=_CREATE)

    connector_id = create_update(
        powerapps_rp=powerapps_rp,
        settings=settings,
        client_secret=client_secret,
        is_update=False)

    display('{} created successfully.'.format(connector_id))
