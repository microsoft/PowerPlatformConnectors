# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Prompts the user for missing arguments
"""

from knack.prompting import prompt_choice_list

_PROPERTIES = 'properties'
_VALUE = 'value'
_DISPLAY_NAME = 'displayName'
_NAME = 'name'
_IS_CUSTOM_API = 'isCustomApi'
_CREATED_BY = 'createdBy'


def get_environment(flow_rp):
    """
    Prompt for environment if not provided.
    """
    environments_val = flow_rp.get_environments()
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
