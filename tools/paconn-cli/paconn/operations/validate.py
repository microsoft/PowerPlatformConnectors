# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Method for create/update operation
"""

import io
import json

from paconn.common.util import ensure_file_exists


def validate(powerapps_rp, enablecertificatevalidation, ignorewarnings, settings):
    """
    Method for create/update operation
    """

    # Make sure the required files exist
    ensure_file_exists(
        file=settings.api_definition,
        file_type='API Definition')

    # Load swagger definition
    with io.open(settings.api_definition, 'r', encoding='utf-8-sig') as file:
        openapi_definition = json.load(file)

    # Validate Open API Definition
    result = powerapps_rp.validate_connector(
        payload=openapi_definition,
        enable_certification_rules=enablecertificatevalidation)

    message = None

    if result:
        try:
            result_json = json.loads(result)
            message = result_json['error']['message']
        except ValueError:
            message = result

    if message:
        message = message.replace(': ', '.\n', 1)

        if ignorewarnings:
            messages = message.split('\n')
            new_messages = []

            for m in messages:
                if not m.startswith('Warning :'):
                    new_messages.append(m)
            
            message = ''
            # First line is the generic message.
            # Ignore first line if there is no warning
            if len(new_messages) > 1:
                message = '\n'.join(new_messages)

    return message
