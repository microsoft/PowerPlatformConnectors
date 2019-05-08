# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
PowerApps RP manager
"""

import json
from urllib.parse import urljoin


class PowerAppsRP:
    """
    PowerAppsRP manager.
    """

    def __init__(self, api_manager):
        self.api_manager = api_manager

    @staticmethod
    def _get_filter_query(environment):
        return {'$filter': 'environment eq \'{}\''.format(environment)}

    def get_connector(self, environment, connector_id):
        """
        Returns API registration JSON for a given connector.
        """
        api = urljoin('apis/', connector_id)

        endpoint = self.api_manager.construct_url(
            path=api,
            query=PowerAppsRP._get_filter_query(environment))

        response = self.api_manager.request(
            verb='GET',
            endpoint=endpoint)

        return response.json()

    def create_connector(self, environment, payload):
        """
        Creates a new custom connector.
        """
        endpoint = self.api_manager.construct_url(
            path='apis',
            query=PowerAppsRP._get_filter_query(environment))

        response = self.api_manager.request(
            verb='POST',
            endpoint=endpoint,
            payload=payload)

        return response.text

    def update_connector(self, environment, connector_id, payload):
        """
        Updates a custom connector.
        """
        api = urljoin('apis/', connector_id)

        endpoint = self.api_manager.construct_url(
            path=api,
            query=PowerAppsRP._get_filter_query(environment))

        response = self.api_manager.request(
            verb='PATCH',
            endpoint=endpoint,
            payload=payload)

        return response.text

    def get_all_connectors(self, environment):
        """
        Returns all connectors.
        """
        endpoint = self.api_manager.construct_url(
            path='apis',
            query=PowerAppsRP._get_filter_query(environment))

        response = self.api_manager.request(
            verb='GET',
            endpoint=endpoint)

        return response.json()

    def validate_connector(self, payload, enable_certification_rules):
        """
        Validates a custom connector.
        """
        api = self.api_manager.add_object_id('validateApiSwagger')

        query = None
        if enable_certification_rules:
            query = {'enableConnectorCertificationRules': 'true'}

        endpoint = self.api_manager.construct_url(
            path=api,
            query=query)

        response = self.api_manager.request(
            verb='POST',
            endpoint=endpoint,
            payload=payload)

        return response.text

    def generate_resource_storage(self, environment):
        """
        Generates a resource storage
        """
        api = self.api_manager.add_object_id('generateResourceStorage')

        endpoint = self.api_manager.construct_url(path=api)

        payload = {'environment': {'name': environment}}

        response = self.api_manager.request(
            verb='POST',
            endpoint=endpoint,
            payload=payload)

        return json.loads(response.text)
