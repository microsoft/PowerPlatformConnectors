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


def validate(powerapps_rp, settings):
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
        enable_certification_rules=True)

    # Replace \r\n in the string to newlines
    result = bytes(result, 'utf-8').decode('unicode-escape')
    # Remove quotes at the beginning and end
    result = result.strip('"')

    return result
