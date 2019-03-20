# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------
"""
Utility methods.
"""
import sys
import os
import json


def get_config_dir():
    """
    Returns the user config directory.
    """
    from paconn import __CLI_NAME__
    return os.path.expanduser(os.path.join('~', '.{}'.format(__CLI_NAME__)))


def display(txt):
    """
    Displayed the text to stderr stream.
    """
    print(txt, file=sys.stderr)


def format_json(content):
    """
    Format a given dictionary to a json formatted string.
    """
    json_string = json.dumps(
        content,
        sort_keys=True,
        indent=2,
        separators=(',', ': '))
    return json_string
