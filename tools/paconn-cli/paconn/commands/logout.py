# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Login command.
"""

from paconn.authentication.auth import remove_authentication
from paconn.common.util import display_message


def logout():
    """
    Logout command.
    """
    remove_authentication()
    display_message('Logout successful.')
