
## PKIsigning Platform Connector
The PKIsigning platform provides an extensive REST API. Using this API, you can manage all files and their contained documents and requests for signing, approving and downloading files. 

## Prerequisites
You will need the following to proceed:
* A PKIsigning subscription

## Supported Operations
The connector supports the following operations:
* `Retrieve all actors of a document`: List all actors that are linked to a document within a file
* `Add actor`: Create a new actor and add it to a specific document in a file
* `Get actor`: Retrieve all information on a specific actor of a specific document in a file
* `Remove an actor`: Remove a specified actor from a document in a file
* `Update an actor`: Update information on a specified actor of a document in a file
* `Retrieve a list of actors for the dossier`: List all actors that are linked to a file regardless of the document they are linked to
* `Resend an invite to the specified actor`: Resend an invite to the specified actor in the specified file (actorid is ignored for now, the invite for the next actor in line will be sent again)
* `Withdraw an invite`: Withdraw the invite for the specified actor (actorid is ignored for now, the invite for the next actor in line will be withdrawn)
* `Create a new document by uploading it`: Add a new document to an existing file
* `Acquire metadata of a document`: Retrieve all information of a specific document in file
* `Delete a document from a dossier`: Remove a specific document from a file
* `Update metadata of a document`: Update name and filename of a specific document
* `Create a new empty dossier`: Create a new dossier without any documents
* `Get all metadata of a dossier`: Retrieve all information that is available on a dossier including all documents and actors
* `Permanently delete a dossier`: Remove the dossier from the platform permanently
* `Update metadata of a dossier`: Update metadata of a dossier (e.g. name, clearancelevel and workgroup) <!-- was Service to update...> 
* `Download a dossier and its document as a zip file`: Download a dossier as a zip-file. <!-- remove (raw, no base64) -->
* `Signal the beginning of the workflow`: Send an invite to the first actor in the dossier and mark the dossier with a status waiting for signing/download/approval
* `Get a list of papertypes with thumbnails`: List all available papertypes that can be used to set page backgrounds
* `Create a new papertype`: Upload a pdf to create a new papertype
* `Delete an existing papertype`: Remove the specified papertype
* `Update an existing papertype`: Update the name of the specified papertype
* `Get all workgroups for an organisation`: Get all workgroups that belong to a the specified organisation
* `Remove multiple pages from a document`: Remove the specified pages from the document
* `Change the background of one or more pages in the document`: Set a background for one or more pages in the document 

## Supported triggers
The connector supports the following triggers
* `Register a new webhook for this organisation`: Register a new trigger that fires upon statusupdates of all dossiers in the specified organisation (to be called with value "status")