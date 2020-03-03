# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""Microsoft Power Platform Connectors CLI package that can be installed using setuptools"""

import os
from setuptools import setup

__VERSION__ = '0.0.13'


def read(fname):
    """Local read helper function for long documentation"""
    return open(os.path.join(os.path.dirname(__file__), fname)).read()


setup(
    name='paconn',
    version=__VERSION__,
    description='Microsoft Power Platform Connectors CLI',
    long_description=read('README.md'),
    long_description_content_type="text/markdown",
    author='Microsoft Corporation',
    author_email='connectors@microsoft.com',
    license='MIT',
    classifiers=[
        'Development Status :: 4 - Beta',
        'Intended Audience :: Developers',
        'Topic :: Software Development :: Build Tools',
        'Environment :: Console',
        'License :: OSI Approved :: MIT License',
        'Natural Language :: English',
        'Programming Language :: Python :: 3.5',
        'Programming Language :: Python :: 3.6'
    ],
    keywords='azure, powerapps, flow, power platform, connectors',
    python_requires='>=3.5,<4',
    packages=[
        'paconn',
        'paconn.apimanager',
        'paconn.authentication',
        'paconn.commands',
        'paconn.common',
        'paconn.config',
        'paconn.operations',
        'paconn.settings'
    ],
    install_requires=[
        'docutils',
        'flake8',
        'future',
        'knack~=0.5.1',
        'pytest',
        'pytest-xdist',
        'virtualenv',
        'requests',
        'adal',
        'msrestazure',
        'azure-storage-blob>=2.1,<12.0'
    ],
    extras_require={
        ":python_version<'3.0'": ['pylint~=1.9.2'],
        ":python_version>='3.0'": ['pylint~=2.0.0']
    },
    package_data={'paconn.config': ['*.*']},
    include_package_data=True,
    entry_points={
        'console_scripts': ['paconn=paconn.__main__:main']
    }
)
