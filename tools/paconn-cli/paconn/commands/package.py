# -----------------------------------------------------------------------------
# Copyright (c) 2025 Troy Taylor (troy@troystaylor.com). All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# 
# Permission is hereby granted to Microsoft Corporation and any other party
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of this software under the terms of the MIT License.
# -----------------------------------------------------------------------------
"""
Package command - Creates a Power Platform solution package from component files.
"""

from paconn import _PACKAGE

from paconn.common.util import display
from paconn.settings.settingsbuilder import SettingsBuilder

import paconn.operations.package


# pylint: disable=too-many-arguments
def package(
        source,
        destination,
        package_format,
        settings_file,
        overwrite,
        custom_mappings=None):
    """
    Package command - Creates a Power Platform solution package.
    
    Processes Power Platform solution zip files in the specified directory:
    - Files containing "Connector" are renamed to "Connector.zip" (required)
    - Files containing "Flow" are renamed to "Flow.zip" (required) 
    - Files containing "AIPlugin" are renamed to "AIPlugin.zip" (optional)
    After renaming, all zip files are moved to a new "PkgAssets" folder.
    The readme.md file (or first available .md file) is copied to intro.md.
    The PkgAssets folder is compressed into a "package.zip" file.
    Finally, a "ConnectorPackage.zip" is created containing intro.md and package.zip.
    """
    # Parse custom mappings if provided
    parsed_custom_mappings = None
    if custom_mappings:
        try:
            import json
            parsed_custom_mappings = json.loads(custom_mappings)
        except json.JSONDecodeError:
            from knack.util import CLIError
            raise CLIError('Invalid JSON format for --custom-mappings parameter.')
    
    # Get settings (minimal settings needed for this operation)
    settings = SettingsBuilder.get_settings(
        environment=None,
        settings_file=settings_file,
        api_properties=None,
        api_definition=None,
        icon=None,
        script=None,
        connector_id=None,
        powerapps_url=None,
        powerapps_version=None)

    package_path = paconn.operations.package.package(
        source=source,
        destination=destination,
        package_format=package_format,
        settings=settings,
        overwrite=overwrite,
        custom_mappings=parsed_custom_mappings)

    display('Power Platform solution package created: {}'.format(package_path))
