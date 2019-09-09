# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
Uploads an icon for the custom connector
"""
import os
import mimetypes
from urllib.parse import urlparse, urlunparse
from azure.storage.blob import ContentSettings, BlockBlobService


def upload_icon(sas_url, file_path):
    # Break the SAS URL
    (scheme, netloc, path, params, query, fragment) = urlparse(sas_url)
    # Account is the first part of the netlocation upto the dot
    account_name = netloc[0:netloc.index('.')]

    # The assumption here is that the blob URL will be in the
    # form accountname.blob.core.windows.net or
    # accountname.blob.core.usgovcloudapi.net.
    # Chopping off accountname.blob. to obtain the
    # endpoint suffix.
    endpoint_suffix = netloc.replace(account_name+'.blob.', '')

    # Container name is the path
    container_name = path.strip('/')

    # Create a block blob service
    blockblob_service = BlockBlobService(
        account_name=account_name,
        sas_token=query,
        endpoint_suffix=endpoint_suffix)

    # Get the file name of the icon
    file_name = os.path.basename(file_path)
    # Determine the content type and encoding for the file
    (content_type, content_encoding) = mimetypes.guess_type(file_name)
    content_settings = ContentSettings(
        content_type=content_type,
        content_encoding=content_encoding)

    # Upload the icon
    blockblob_service.create_blob_from_path(
        container_name=container_name,
        blob_name=file_name,
        file_path=file_path,
        content_settings=content_settings)

    # Append the icon name to the path to generate the download link
    path = path + '/' + file_name
    urlparts = (scheme, netloc, path, params, query, fragment)
    sas_download_url = urlunparse(urlparts)

    return sas_download_url
