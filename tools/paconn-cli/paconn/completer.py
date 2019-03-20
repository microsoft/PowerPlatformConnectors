# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Defines argument completer
"""


# pylint: disable=too-few-public-methods
class Completer:
    """
    Argument completer object
    """
    def __init__(self, func):
        self.func = func

    def __call__(self, **kwargs):
        namespace = kwargs['parsed_args']
        prefix = kwargs['prefix']
        cmd = namespace._cmd  # pylint: disable=protected-access
        return self.func(cmd, prefix, namespace)
