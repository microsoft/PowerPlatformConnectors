# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Validate command.
"""

from paconn import _VALIDATE

from paconn.common.util import display
from paconn.settings.util import load_settings_and_powerapps_rp

import paconn.operations.validate


def validate(
        api_definition,
        powerapps_url,
        powerapps_version,
        settings_file):
    """
    Validate command.
    """
    settings, powerapps_rp, _ = load_settings_and_powerapps_rp(
        environment=None,
        settings_file=settings_file,
        connector_id=None,
        powerapps_url=powerapps_url,
        powerapps_version=powerapps_version,
        command_context=_VALIDATE,
        api_properties=None,
        api_definition=api_definition,
        icon=None)

    result = paconn.operations.validate.validate(
        powerapps_rp=powerapps_rp,
        settings=settings)

    if result:
        display(result)
    else:
        display('{} validated successfully.'.format(settings.api_definition))
