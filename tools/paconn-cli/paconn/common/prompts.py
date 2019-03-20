# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Prompts the user for missing arguments
"""

from knack.prompting import prompt_choice_list

from paconn.apimanager.apimanager import APIManagerBuilder
from paconn.apimanager.flowmanager import FlowManager

_DEFAULT_FLOW_URL = 'https://preview.api.flow.microsoft.com'
_DEFAULT_FLOW_BASE_PATH = 'providers/Microsoft.ProcessSimple'
_DEFAULT_FLOW_API_VERSION = '2016-11-01'

_PROPERTIES = 'properties'
_VALUE = 'value'
_DISPLAY_NAME = 'displayName'
_NAME = 'name'
_IS_CUSTOM_API = 'isCustomApi'
_CREATED_BY = 'createdBy'


def get_environment(credentials):
    """
    Prompt for environment if not provided.
    """
    flow_api_manager = APIManagerBuilder.get_from_url(
        url=_DEFAULT_FLOW_URL,
        base_path=_DEFAULT_FLOW_BASE_PATH,
        api_version=_DEFAULT_FLOW_API_VERSION,
        credentials=credentials)
    flow_manager = FlowManager(api_manager=flow_api_manager)

    environments_val = flow_manager.get_environments()
    environments_list = environments_val[_VALUE]
    environments = {
        env[_PROPERTIES][_DISPLAY_NAME]: env[_NAME]
        for env in environments_list
    }

    environment_keys = list(environments.keys())

    sid = prompt_choice_list('Please select an environment:', environment_keys)
    environment = environments[environment_keys[sid]]

    print('Environment selected: {}'.format(environment_keys[sid]))

    return environment


def get_connector_id(powerapps_rp, environment):
    """
    Select connector id if not provided.
    """
    connectors_val = powerapps_rp.get_all_connectors(environment)
    connectors_list = connectors_val[_VALUE]
    custom_connectors = filter(lambda conn: conn[_PROPERTIES][_IS_CUSTOM_API], connectors_list)
    connectors = {
        conn[_PROPERTIES][_DISPLAY_NAME] + ' - ' + conn[_PROPERTIES][_CREATED_BY][_DISPLAY_NAME]: conn[_NAME]
        for conn in custom_connectors
    }

    connectors_keys = list(connectors.keys())

    sid = prompt_choice_list('Please select a connector:', connectors_keys)
    connector_id = connectors[connectors_keys[sid]]

    print('Connector selected: {}'.format(connectors_keys[sid]))

    return connector_id
