# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Defines the command table
"""

from knack.commands import CommandGroup

from paconn import __CLI_NAME__
from paconn import _COMMAND_GROUP, _LOGIN, _DOWNLOAD, _CREATE, _UPDATE, _VALIDATE


# pylint: disable=unused-argument
def load_command_table(self, args):
    """
    Loads the command table
    """
    def operation_group(name):
        return '{cli_name}.commands.{name}#{name}'.format(cli_name=__CLI_NAME__, name=name)

    with CommandGroup(self, _COMMAND_GROUP, operation_group(_LOGIN)) as command_group:
        command_group.command(_LOGIN, _LOGIN)

    with CommandGroup(self, _COMMAND_GROUP, operation_group(_DOWNLOAD)) as command_group:
        command_group.command(_DOWNLOAD, _DOWNLOAD)

    with CommandGroup(self, _COMMAND_GROUP, operation_group(_CREATE)) as command_group:
        command_group.command(_CREATE, _CREATE)

    with CommandGroup(self, _COMMAND_GROUP, operation_group(_UPDATE)) as command_group:
        command_group.command(_UPDATE, _UPDATE)

    with CommandGroup(self, _COMMAND_GROUP, operation_group(_VALIDATE)) as command_group:
        command_group.command(_VALIDATE, _VALIDATE)
