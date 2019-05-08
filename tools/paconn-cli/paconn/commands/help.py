# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Help strings for the CLI
"""

from knack.help_files import helps  # pylint: disable=unused-import
from paconn import _COMMAND_GROUP, _LOGIN, _DOWNLOAD, _CREATE, _UPDATE, _VALIDATE

helps[_COMMAND_GROUP] = """
    short-summary: Microsoft Power Platform Connectors CLI
"""

helps[_LOGIN] = """
    type: command
    short-summary: Login to Power Platform.
    examples:
        - name: Login
          text: paconn login
"""


helps[_DOWNLOAD] = """
    type: command
    short-summary: Downloads a given custom connector to the local directory.
    examples:
        - name: Download connector
          text: paconn download
"""

helps[_CREATE] = """
    type: command
    short-summary: Creates a new custom connector from the given directory.
    examples:
        - name: Create connector
          text: paconn create
"""

helps[_UPDATE] = """
    type: command
    short-summary: Update a given custom connector from the local directory.
    examples:
        - name: Update connector.
          text: paconn update
"""

helps[_VALIDATE] = """
    type: command
    short-summary: Validate the swagger for certification.
    examples:
        - name: Validate swagger
          text: paconn validate
"""
