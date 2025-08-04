# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Help strings for the CLI
"""

from knack.help_files import helps  # pylint: disable=unused-import
from paconn import _COMMAND_GROUP, _LOGIN, _DOWNLOAD, _CREATE, _UPDATE, _VALIDATE, _PACKAGE

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

helps[_PACKAGE] = """
    type: command
    short-summary: Package Power Platform solution components into a distributable format.
    long-summary: |
        Creates a structured Power Platform solution package from component ZIP files.
        
        The packaging process:
        1. Renames ZIP files according to Power Platform conventions:
           - Files containing "Connector" → "Connector.zip" (required)
           - Files containing "Flow" → "Flow.zip" (required)  
           - Files containing "AIPlugin" → "AIPlugin.zip" (optional)
        2. Moves all ZIP files to a "PkgAssets" folder
        3. Creates "intro.md" from "readme.md" (or first available .md file)
        4. Compresses PkgAssets folder into "package.zip"
        5. Creates final "ConnectorPackage.zip" containing intro.md and package.zip
        6. Cleans up intermediate files
        
        The final ConnectorPackage.zip is ready for distribution and deployment.
    examples:
        - name: Package solution components in current directory
          text: paconn package
        - name: Package components from specific source directory
          text: paconn package --source ./my-solution
        - name: Package with custom file mappings
          text: >
            paconn package --custom-mappings
            '{"*MyConnector*": "Connector.zip", "*MyFlow*": "Flow.zip"}'
        - name: Package and overwrite existing files
          text: paconn package --overwrite
"""
