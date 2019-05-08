# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Defines main entry point for the CLI
"""

from __future__ import print_function
import sys

from knack import CLI, CLICommandsLoader
# pylint: disable=unused-import
from paconn.commands.help import helps  # noqa: F401
from paconn.common.util import get_config_dir


class ConnectorsCli(CLI):
    """
    The CLI CLass
    """
    def get_cli_version(self):
        """
        Returns cli version
        """
        from . import __VERSION__
        return __VERSION__


class ConnectorsCliCommandsLoader(CLICommandsLoader):
    """
    Commands loader
    """
    def load_command_table(self, args):
        """
        Loads command table
        """
        from paconn.commands.commands import load_command_table

        load_command_table(self, args)
        return super(ConnectorsCliCommandsLoader, self).load_command_table(args)

    def load_arguments(self, command):
        """
        Load arguments
        """
        from paconn.commands.params import load_arguments

        load_arguments(self, command)
        super(ConnectorsCliCommandsLoader, self).load_arguments(command)


def main():
    """
    The main method
    """

    try:
        from paconn import __CLI_NAME__
        cli_context = ConnectorsCli(
            cli_name=__CLI_NAME__,
            commands_loader_cls=ConnectorsCliCommandsLoader,
            config_dir=get_config_dir())

        exit_code = cli_context.invoke(sys.argv[1:])
        sys.exit(exit_code)
    except KeyboardInterrupt:
        sys.exit(1)


if __name__ == '__main__':
    main()
