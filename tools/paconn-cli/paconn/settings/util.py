# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Utility for loading settings.
"""

from paconn import _UPDATE, _DOWNLOAD, _VALIDATE
from paconn.common.util import write_with_prompt
from paconn.apimanager.powerappsrpbuilder import PowerAppsRPBuilder
from paconn.apimanager.flowrpbuilder import FlowRPBuilder
from paconn.common.prompts import get_environment, get_connector_id
from paconn.settings.settingsserializer import SettingsSerializer
from paconn.authentication.auth import get_silent_authentication
from paconn.common.util import display
from knack.util import CLIError

# Setting file name
SETTINGS_FILE = 'settings.json'


def prompt_for_environment(settings, flow_rp):
    # Select environment if not provided
    if not hasattr(settings, 'environment') or not settings.environment:
        settings.environment = get_environment(
            flow_rp=flow_rp)


def prompt_for_connector_id(settings, powerapps_rp):
    # Select connector id if not provided
    if (not hasattr(settings, 'connector_id') or not settings.connector_id):
        settings.connector_id = get_connector_id(
            powerapps_rp=powerapps_rp,
            environment=settings.environment)


def load_powerapps_and_flow_rp(settings, command_context):

    # Get credentials
    (credentials, account) = get_silent_authentication()

    if credentials and "error" in credentials:
        raise CLIError(credentials['error_description'])
    elif not credentials:
        raise CLIError('Couldn\'t obtain login token. Please log in again.')

    if account and 'username' in account:
        display('Using account {}.'.format(account['username']))

    # Get powerapps rp
    powerapps_rp = PowerAppsRPBuilder.get_from_settings(
        credentials=credentials,
        account=account,
        settings=settings)

    # Get flow rp
    flow_rp = FlowRPBuilder.get_from_settings(
        credentials=credentials,
        account=account,
        settings=settings)

    # If the file names are missing for the download command
    # use default names
    if command_context is _DOWNLOAD:
        settings.api_properties = settings.api_properties or 'apiProperties.json'
        settings.api_definition = settings.api_definition or 'apiDefinition.swagger.json'
        settings.icon = settings.icon or 'icon.png'
        settings.script = settings.script or 'script.csx'

    # Prompt for environment for any
    # operation other than `validate`
    if command_context is not _VALIDATE:
        prompt_for_environment(
            settings=settings,
            flow_rp=flow_rp)

    # Prompt for connector id for
    # operation `update` and `download`
    if command_context is _UPDATE or command_context is _DOWNLOAD:
        prompt_for_connector_id(
            settings=settings,
            powerapps_rp=powerapps_rp)

    return powerapps_rp, flow_rp


def write_settings(settings, overwrite):
    filename = SETTINGS_FILE
    settings_json = SettingsSerializer.to_json_string(settings)
    write_with_prompt(
        filename=filename,
        mode='w',
        content=settings_json,
        overwrite=overwrite)
