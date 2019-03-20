# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Login command.
"""

from paconn.authentication.auth import get_authentication
from paconn.common.util import display


def login(client_id, client_secret, tenant):
    """
    Login command.
    """
    get_authentication(
        client_id=client_id,
        client_secret=client_secret,
        tenant=tenant)
    display('Login successful.')
