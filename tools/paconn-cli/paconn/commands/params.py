# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
CLI parameter definitions
"""

from knack.arguments import ArgumentsContext
from paconn import _LOGIN, _DOWNLOAD, _CREATE, _UPDATE, _VALIDATE

CLIENT_ID = 'client_id'
CLIENT_ID_OPTIONS = ['--clid', '-i']
CLIENT_ID_HELP = 'The client ID.'

CLIENT_SECRET = 'client_secret'
CLIENT_SECRET_OPTIONS = ['--secret', '-r']
CLIENT_SECRET_HELP = 'The client secret.'
CLIENT_SECRET_CONN_HELP = 'The OAuth2 client secret for the connector.'

TENANT = 'tenant'
TENANT_OPTIONS = ['--tenant', '-t']
TENANT_HELP = 'The tenant.'

ENVIRONMENT = 'environment'
ENVIRONMENT_OPTIONS = ['--env', '-e']
ENVIRONMENT_HELP = 'Power Platform environment ID.'

CONNECTOR_ID = 'connector_id'
CONNECTOR_ID_OPTIONS = ['--cid', '-c']
CONNECTOR_ID_HELP = 'The custom connector ID.'

DESTINATION = 'destination'
DESTINATION_OPTIONS = ['--dest', '-d']
DESTINATION_HELP = 'Destination directory.'

POWERAPPS_URL = 'powerapps_url'
POWERAPPS_URL_OPTIONS = ['--pau', '-u']
POWERAPPS_URL_HELP = 'Power Platform URL.'

POWERAPPS_VERSION = 'powerapps_version'
POWERAPPS_VERSION_OPTIONS = ['--pav', '-v']
POWERAPPS_VERSION_HELP = 'Power Platform api version.'

SETTINGS = 'settings_file'
SETTINGS_OPTIONS = ['--settings', '-s']
SETTINGS_HELP = 'A settings file containing required parameters. When a settings file is specified some commandline parameters are ignored.'  # noqa: E501

API_PROPERTIES = 'api_properties'
API_PROPERTIES_OPTIONS = ['--api-prop']
API_PROPERTIES_HELP = 'Location of the API properties JSON document.'

API_DEFINITION = 'api_definition'
API_DEFINITION_OPTIONS = ['--api-def']
API_DEFINITION_HELP = 'Location of the Open API definition JSON document.'

ICON = 'icon'
ICON_OPTIONS = ['--icon']
ICON_HELP = 'Location for the icon file.'


# pylint: disable=unused-argument
def load_arguments(self, command):
    """
    Load command line arguments
    """
    with ArgumentsContext(self, _LOGIN) as arg_context:
        pass

    with ArgumentsContext(self, _DOWNLOAD) as arg_context:
        arg_context.argument(
            ENVIRONMENT,
            options_list=ENVIRONMENT_OPTIONS,
            type=str,
            required=False,
            help=ENVIRONMENT_HELP)
        arg_context.argument(
            CONNECTOR_ID,
            options_list=CONNECTOR_ID_OPTIONS,
            type=str,
            required=False,
            help=CONNECTOR_ID_HELP)
        arg_context.argument(
            DESTINATION,
            options_list=DESTINATION_OPTIONS,
            type=str,
            required=False,
            help=DESTINATION_HELP)
        arg_context.argument(
            POWERAPPS_URL,
            options_list=POWERAPPS_URL_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_URL_HELP)
        arg_context.argument(
            POWERAPPS_VERSION,
            options_list=POWERAPPS_VERSION_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_VERSION_HELP)
        arg_context.argument(
            SETTINGS,
            options_list=SETTINGS_OPTIONS,
            type=str,
            required=False,
            help=SETTINGS_HELP)

    with ArgumentsContext(self, _CREATE) as arg_context:
        arg_context.argument(
            ENVIRONMENT,
            options_list=ENVIRONMENT_OPTIONS,
            type=str,
            required=False,
            help=ENVIRONMENT_HELP)
        arg_context.argument(
            API_PROPERTIES,
            options_list=API_PROPERTIES_OPTIONS,
            type=str,
            required=False,
            help=API_PROPERTIES_HELP)
        arg_context.argument(
            API_DEFINITION,
            options_list=API_DEFINITION_OPTIONS,
            type=str,
            required=False,
            help=API_DEFINITION_HELP)
        arg_context.argument(
            ICON,
            options_list=ICON_OPTIONS,
            type=str,
            required=False,
            help=ICON_HELP)
        arg_context.argument(
            POWERAPPS_URL,
            options_list=POWERAPPS_URL_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_URL_HELP)
        arg_context.argument(
            POWERAPPS_VERSION,
            options_list=POWERAPPS_VERSION_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_VERSION_HELP)
        arg_context.argument(
            CLIENT_SECRET,
            options_list=CLIENT_SECRET_OPTIONS,
            type=str,
            required=False,
            help=CLIENT_SECRET_CONN_HELP)
        arg_context.argument(
            SETTINGS,
            options_list=SETTINGS_OPTIONS,
            type=str,
            required=False,
            help=SETTINGS_HELP)

    with ArgumentsContext(self, _UPDATE) as arg_context:
        arg_context.argument(
            ENVIRONMENT,
            options_list=ENVIRONMENT_OPTIONS,
            type=str,
            required=False,
            help=ENVIRONMENT_HELP)
        arg_context.argument(
            API_PROPERTIES,
            options_list=API_PROPERTIES_OPTIONS,
            type=str,
            required=False,
            help=API_PROPERTIES_HELP)
        arg_context.argument(
            API_DEFINITION,
            options_list=API_DEFINITION_OPTIONS,
            type=str,
            required=False,
            help=API_DEFINITION_HELP)
        arg_context.argument(
            ICON,
            options_list=ICON_OPTIONS,
            type=str,
            required=False,
            help=ICON_HELP)
        arg_context.argument(
            CONNECTOR_ID,
            options_list=CONNECTOR_ID_OPTIONS,
            type=str,
            required=False,
            help=CONNECTOR_ID_HELP)
        arg_context.argument(
            POWERAPPS_URL,
            options_list=POWERAPPS_URL_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_URL_HELP)
        arg_context.argument(
            POWERAPPS_VERSION,
            options_list=POWERAPPS_VERSION_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_VERSION_HELP)
        arg_context.argument(
            CLIENT_SECRET,
            options_list=CLIENT_SECRET_OPTIONS,
            type=str,
            required=False,
            help=CLIENT_SECRET_CONN_HELP)
        arg_context.argument(
            SETTINGS,
            options_list=SETTINGS_OPTIONS,
            type=str,
            required=False,
            help=SETTINGS_HELP)

    with ArgumentsContext(self, _VALIDATE) as arg_context:
        arg_context.argument(
            API_DEFINITION,
            options_list=API_DEFINITION_OPTIONS,
            type=str,
            required=False,
            help=API_DEFINITION_HELP)
        arg_context.argument(
            POWERAPPS_URL,
            options_list=POWERAPPS_URL_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_URL_HELP)
        arg_context.argument(
            POWERAPPS_VERSION,
            options_list=POWERAPPS_VERSION_OPTIONS,
            type=str,
            required=False,
            help=POWERAPPS_VERSION_HELP)
        arg_context.argument(
            SETTINGS,
            options_list=SETTINGS_OPTIONS,
            type=str,
            required=False,
            help=SETTINGS_HELP)
