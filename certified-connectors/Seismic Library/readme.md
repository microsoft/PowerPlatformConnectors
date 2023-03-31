# Seismic Power Automate Library connector

The Seismic Power Automate Library connector includes actions related to add, update, delete, publish and download library content.

## Publisher: Seismic

## Prerequisites

You need a Seismic Premium user account for the tenant with Full Control (System Administrator) permissions.

## Supported Operations

### Add a file

Adds a new file using a multipart/form-data POST containing the file's metadata and the file contents.

### Change file information or properties

This endpoint can be use to move a file or update its properties, including system properties such as the name, owner, description, as well as custom properties.

### Get file information or properties

Get library file content information.

### Add a new file version

Adds a new file version using a multipart/form-data POST containing the file's content in the content form data.

### Download a file

Downloads the binary contents of a file.

### Download a file version

Downloads the binary contents of a file version.

### Copy a file

Copy a file to any target folder.

### Add a folder

Adds a new folder inside a given folder. Use the special keywork "root" as the parentFolderId to create a new folder in the teamsite's root.

### Change folder information

This endpoint can be use to move or rename a folder.

### Get folder information

Get folder information.

### Copy a folder

Copy any type of item such as a file, folder, url, article, etc... to any target folder.

### Get list of items in a folder

Gets the list of items in a given folder. Each item's structure is identical to the structure returned by the GET endpoint for that item type.

### Delete an item

Delete an item.

### Get item information or properties

This is the generalized endpoint to get information (both built in and custom properties) for any item type.

### Copy an item

Copy any type of item such as a file, folder, url, article, etc... to any target folder.

### Get all versions for an item

Gets the list of versions for a given item.

### Get items via query

Gets the list of items based on a set of filters up to a max of 50 items. Each item's structure is identical to the structure returned by the GET endpoint for that item type.

### Replace thumbnail on an item

Replace thumbnail on an item. Supported image types include jpg, png, and tif. Thumbnails shall be passed as a base64 encoded string in the body.

### Add a url

Adds a new URL using an application/json POST with all required information in the POST body.

### Change url information or properties

This endpoint allows any of the url's properties to be modified including system properties such as the name, owner, description, as well as custom properties.

### Get url information or properties

Get url information or properties.

### Copy a url

Copy a url to any target folder.

### Add an instruction on an item

Add an instruction on an item.

### Get instructions on an item

Get list of instructions on an item.

### Delete an instruction on an item

Delete an instruction on an item.

### Submit a document into workflow

Submit a document into workflow.

### Recall a document from workflow

Recall a document from workflow.

### Approve or reject workflow step

Approve or reject a given workflow step. Must be authenticated as the assigned user in order to complete this action.

### Get information for a particular workflow

Get information for a particular workflow given by workflow id.

### Get list of approval workflows

Get list of approval workflows. Use optional query string parameters to filter the response.

### Publish one or more documents

This endpoint can be used to immediately promote and publish content, or schedule publish of content for a future date for up to 10 unpublished documents.

### Unpublish a document

This endpoint is to unpublish a document that has been published.

## Getting Started

The Seismic Power Automate Library connector includes the following actions. Each action corresponds to an API endpoint. Refer to the article for each corresponding API endpoint for further information on the fields and properties associated with the action.

* [Add a file](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddafile)
* [Change file information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementchangefileinformationproperties)
* [Get file information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetfileinformationproperties)
* [Add a new file version](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddanewfileversion)
* [Download a file](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementdownloadafile)
* [Download a file version](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementdownloadafileversion)
* [Copy a file](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyafile)
* [Add a folder](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddafolder)
* [Change folder information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementchangefolderinformation)
* [Get folder information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetfolderinformation)
* [Copy a folder](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyafolder)
* [Get list of items in a folder](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetlistofitemsinafolder)
* [Delete an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementdeleteanitem)
* [Get item information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetiteminformationproperties)
* [Copy an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyanitem)
* [Get all versions for an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetallversionsforanitem)
* [Get items via query](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetitemsviaquery)
* [Replace thumbnail on an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementreplacethumbnailonanitem)
* [Add a url](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddaurl)
* [Change url information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementchangeurlinformationproperties)
* [Get url information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgeturlinformationproperties)
* [Get all versions for an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetallversionsforanitem)
* [Copy a url](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyaurl)
* [Add an instruction on an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryinstructionsaddaninstructiononanitem)
* [Get instructions on an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryinstructionsgetinstructionsonanitem)
* [Get all versions for an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryinstructionsdeleteaninstructiononanitem)
* [Submit a document into workflow](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowsubmitadocumentintoworkflow)
* [Recall a document from workflow](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowrecalladocumentfromworkflow)
* [Approve or reject workflow step](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowapproveorrejectworkflowstep)
* [Get information for a particular workflow](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowgetinformationforaparticularworkflow)
* [Get list of approval workflows](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowgetlistofapprovalworkflows)
* [Publish one or more documents](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarypublishingpublishoneormoredocuments)
* [Unpublish a document](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarypublishingunpublishadocument)

## Obtaining Credentials

You need a Seismic Premium user account for the tenant with Full Control (System Administrator) permissions. Contact your Seismic system administrator if you do not have sufficient permissions.

## Known Issues and Limitations

No issues and limitations are known at this time. All APIs operate in accordance with Seismic API policy, including Rate Limits. Please refer to the [Seismic Developer portal](https://developer.seismic.com/) for API specifications, restrictions, and standards.