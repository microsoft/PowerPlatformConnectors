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
from paconn.settings.util import load_powerapps_and_flow_rp
from paconn.settings.settingsbuilder import SettingsBuilder

import paconn.operations.validate
import paconn.operations.script_validate


def validate(
        api_definition,
        script,
        powerapps_url,
        powerapps_version,
        settings_file):
    """
    Validate command - supports either API definition OR script validation (mutually exclusive).
    """
    
    # Get settings first
    settings = SettingsBuilder.get_settings(
        environment=None,
        settings_file=settings_file,
        api_properties=None,
        api_definition=api_definition,
        icon=None,
        script=script,
        connector_id=None,
        powerapps_url=powerapps_url,
        powerapps_version=powerapps_version)
    
    # Check for mutual exclusion after settings are loaded
    has_api_def = settings.api_definition is not None
    has_script = settings.script is not None
    
    if has_api_def and has_script:
        display("ERROR: Cannot specify both api_definition and script. Choose one validation type.")
        return
    
    if not has_api_def and not has_script:
        display("ERROR: Must specify either api_definition or script for validation.")
        return
    
    if has_api_def:
        # Existing OpenAPI validation path
        powerapps_rp, _ = load_powerapps_and_flow_rp(settings=settings, command_context=_VALIDATE)
        result = paconn.operations.validate.validate(powerapps_rp=powerapps_rp, settings=settings)
        
        if result:
            display(result)
        else:
            display('{} validated successfully.'.format(settings.api_definition))
    
    elif has_script:
        # New script validation path (always strict)
        result = paconn.operations.script_validate.validate_script(settings.script)
        
        if result.has_errors:
            # Format errors similar to API validation output
            error_output = f"Script validation failed for {settings.script}:\n\n"
            error_output += "Errors:\n"
            error_output += result.format_errors()
            error_output += f"\n\nResult: Validation failed. Please fix the errors above."
            display(error_output)
        elif result.has_warnings:
            # Format warnings similar to API validation output  
            warning_output = f"Script validation completed with warnings for {settings.script}:\n\n"
            warning_output += "Warnings:\n"
            warning_output += result.format_warnings()
            warning_output += f"\n\nResult: {settings.script} validated successfully with warnings."
            display(warning_output)
        else:
            # Format success similar to API validation output
            success_output = f"Script validation successful for {settings.script}:\n\n"
            success_output += "Validation Summary:\n"
            success_output += "✓ Required Script class structure validated\n"
            success_output += "✓ ExecuteAsync method signature validated\n" 
            success_output += "✓ Namespace usage validated (26 approved namespaces)\n"
            success_output += "✓ Security constraints validated\n"
            success_output += "✓ File size within limits (1MB max)\n"
            success_output += "✓ Best practices checked\n"
            success_output += f"\nResult: {settings.script} validated successfully."
            display(success_output)
