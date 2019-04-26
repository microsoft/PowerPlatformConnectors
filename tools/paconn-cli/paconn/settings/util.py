# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Utility for loading settings.
"""

from paconn import _CREATE, _DOWNLOAD, _VALIDATE
from paconn.authentication.tokenmanager import TokenManager
from paconn.apimanager.powerappsrp import PowerAppsRPBuilder
from paconn.apimanager.flowrpbuilder import FlowRPBuilder
from paconn.common.prompts import get_environment, get_connector_id
from paconn.settings.settingsbuilder import SettingsBuilder


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


def load_settings_and_powerapps_rp(
        environment,
        settings_file,
        connector_id,
        powerapps_url,
        powerapps_version,
        command_context,
        api_properties,
        api_definition,
        icon):
    # Get settings
    settings = SettingsBuilder.get_settings(
        environment=environment,
        settings_file=settings_file,
        api_properties=api_properties,
        api_definition=api_definition,
        icon=icon,
        connector_id=connector_id,
        powerapps_url=powerapps_url,
        powerapps_version=powerapps_version,
        command_context=command_context)

    # Get credentials
    credentials = TokenManager().get_credentials()

    # Get powerapps rp
    powerapps_rp = PowerAppsRPBuilder.get_from_settings(
        credentials=credentials,
        settings=settings)

    # Get flow rp
    flow_rp = FlowRPBuilder.get_from_settings(
        credentials=credentials,
        settings=settings)

    # If the file names are missing for the download command
    # use default names
    if command_context is _DOWNLOAD:
        settings.api_properties = settings.api_properties or 'apiProperties.json'
        settings.api_definition = settings.api_definition or 'apiDefinition.swagger.json'
        settings.icon = settings.icon or 'icon.png'

    # Propmt for missing arguments
    if command_context is not _VALIDATE:
        prompt_for_environment(
            settings=settings,
            flow_rp=flow_rp)
        if command_context is not _CREATE:
            prompt_for_connector_id(
                settings=settings,
                powerapps_rp=powerapps_rp)

    return settings, powerapps_rp, flow_rp
