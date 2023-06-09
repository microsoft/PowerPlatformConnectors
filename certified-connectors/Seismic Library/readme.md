# Seismic Power Automate Library connector

The Seismic Power Automate Library connector includes actions related to adding, updating, deleting, publishing, and downloading Library content.

## Publisher: Seismic

## Prerequisites

You need a Seismic Premium user account for the tenant with Full Control (System Administrator) permissions.

## Supported Operations

### Add a file

Adds a new file to the Seismic library.

### Add a folder

Adds a new subfolder to a specified library folder.

### Add a new file version

Adds a new version of an existing Library file.

### Add a url

Adds a new URL content item to the Library.

### Add an instruction on an item

Adds an instruction to a library content item.

### Approve or reject workflow step

Approves or rejects a workflow step. You must be authenticated as the assigned user in order to complete this action.

### Change file information or properties

Moves a file or updates its properties, including system properties and custom properties.

### Change folder information

Moves a folder or updates its properties, including system properties and custom properties.

### Change url information or properties

Changes the properties for a URL, including system properties and custom properties.

### Copy a file

Copies a file to a specified target folder.

### Copy a folder

Copies a library folder to a specified target folder.

### Copy a url

Copies a URL to a specified target folder.

### Copy an item

Copies a library content item to a specified target folder.

### Create or get folders by folder path

Gets the ID value for the folder at the specified path. When a folder does not exist at the specified path, a folder is created.

### Delete an instruction on an item

Deletes a specified instruction on a library content item.

### Delete an item

Deletes a specified library content item.

### Download a file

Downloads the binary contents of a library content item file.

### Download a file version

Downloads the binary contents of a library content item file version.

### Get all versions for an item

Gets the list of versions for a specified library content item.

### Get file information or properties

Gets the properties for a library content item.

### Get folder information

Gets the information for a specified library folder.

### Get information for a particular workflow

Gets the information for a specified library approval workflow.

### Get instructions on an item

Gets the list of instructions for a specified library item.

### Get item information or properties

Gets the properties for a library content item.

### Get items by query

Gets a list of up to 50 items based on the specified External ID and External Connection ID values.

### Get list of approval workflows

Gets the list of library approval workflows configured for the Seismic tenant.

### Get list of items in a folder

Gets the list of items in a specified library folder.

### Get url information or properties

Gets the properties for a specified library URL content item.

### Publish one or more documents

Immediately promotes and publishes content or schedules the publishing of content for a future date for up to 10 unpublished documents.

### Recall a document from workflow

Recalls a specified content item from a library approval workflow.

### Replace thumbnail on an item

Replaces the thumbnail for a library content item. Supported image types include jpg, png, and tif.

### Submit a document into workflow

Submits a specified library content item into an approval workflow.

### Unpublish a document

Unpublishes a specified library content item.

## Getting Started

The Seismic Power Automate Library connector includes the following actions. Each action corresponds to an API endpoint. Refer to the article for each corresponding API endpoint for further information on the fields and properties associated with the action.

* [Add a file](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddafile)
* [Add a folder](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddafolder)
* [Add a new file version](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddanewfileversion)
* [Add a url](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementaddaurl)
* [Add an instruction on an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryinstructionsaddaninstructiononanitem)
* [Approve or reject workflow step](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowapproveorrejectworkflowstep)
* [Change file information or properties](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementchangefileinformationproperties)
* [Change folder information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementchangefolderinformation)
* [Change url information or properties](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementchangeurlinformationproperties)
* [Copy a File](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyafile)
* [Copy a folder](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyafolder)
* [Copy a url](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyaurl)
* [Copy an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementcopyanitem)
* [Create or get folders by folder path](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetcreatefolderbypath)
* [Delete an instruction on an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryinstructionsdeleteaninstructiononanitem)
* [Delete an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementdeleteanitem)
* [Download a file](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementdownloadafile)
* [Download a file version](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementdownloadafileversion)
* [Get all versions for an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetallversionsforanitem)
* [Get file information or properties](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetfileinformationproperties)
* [Get folder information](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetfolderinformation)
* [Get information for a particular workflow](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowgetinformationforaparticularworkflow)
* [Get instructions on an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryinstructionsgetinstructionsonanitem)
* [Get item information or properties](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetiteminformationproperties)
* [Get items by query](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetitemsviaquery)
* [Get list of approval workflows](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowgetlistofapprovalworkflows)
* [Get list of items in a folder](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgetlistofitemsinafolder)
* [Get url information or properties](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementgeturlinformationproperties)
* [Publish one or more documents](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarypublishingpublishoneormoredocuments)
* [Recall a document from workflow](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowrecalladocumentfromworkflow)
* [Replace thumbnail on an item](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarycontentmanagementreplacethumbnailonanitem)
* [Submit a document into workflow](https://developer.seismic.com/seismicsoftware/reference/seismiclibraryworkflowsubmitadocumentintoworkflow)
* [Unpublish a document](https://developer.seismic.com/seismicsoftware/reference/seismiclibrarypublishingunpublishadocument)

## Obtaining Credentials

You need a Seismic Premium user account for the tenant with Full Control (System Administrator) permissions. Contact your Seismic system administrator if you do not have sufficient permissions.

## Known Issues and Limitations

No issues and limitations are known at this time. All APIs operate in accordance with Seismic API policy, including Rate Limits. Please refer to the [Seismic Developer portal](https://developer.seismic.com/) for API specifications, restrictions, and standards.