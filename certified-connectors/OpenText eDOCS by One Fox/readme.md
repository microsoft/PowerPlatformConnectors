## OpenText™ eDOCS
OpenText™ eDOCS is a flexible, collaborative enterprise content management (ECM) system designed to support legal and other professional services organizations. Now you can bring content and processes more together by combining it in your low code solutions to ensure that critical business content is managed and secure throughout the content lifecycle, while remaining easily accessible for day-to-day operations.

## Pre-requisites
You will need the following to proceed:
* A license to use this connector purchasable in Azure Marketplace or AppSource.
* OpenText eDOCS with the REST and SOAP services online reachable.
* OpenText eDOCS login credentials.

## How to get credentials
Once a license has been purchased an email will be sent with a link to the settings portal and login credentials to this portal. In this portal the connector can be configured and the API key needed to use the connector can be copied.

## Supported Operations
The connector supports the following operations:

### Document
* `Create document`: Create the document in eDOCS
* `Update document`: Update the document in eDOCS
* `Update document properties`: Update the document properties in eDOCS
* `Update document content`: Update the content of the document in eDOCS
* `Get document`: Get the document from eDOCS
* `Get document properties`: Get the properties of the document from eDOCS
* `Get document content`: Get the content of the document from eDOCS
* `Get document version content`: Get the content of the document version from eDOCS
* `Delete document`: Delete the document in eDOCS
* `Delete document version`: Delete the document version in eDOCS
* `Check in document`: Check in the document in eDOCS
* `Check out document`: Check out the document in eDOCS
* `Get document versions`: Get the versions of the document from eDOCS

### Folder
* `Create folder`: Create the folder in eDOCS
* `Update folder`: Update the folder in eDOCS
* `Get folder`: Get the folder from eDOCS
* `Delete folder`: Delete the folder in eDOCS
* `Get folder children`: Get the children of the folder from eDOCS
* `Add reference to folder`: Add a reference to a folder in eDOCS
* `Remove reference from folder`: Remove a reference from a folder in eDOCS

### Search
* `Search`: Search on all profile metadata in eDOCS
* `Advanced search`: Search on specified profile metadata in eDOCS

### Lookup Entry
* `Create lookup entry`: Create the lookup entry in eDOCS
* `Update lookup entry`: Update the lookup entry in eDOCS
* `Get lookup entries`: Get all lookup entries for the specified content type from eDOCS

## Supported Triggers
The connector supports the following triggers:

### Document
* `When a document is created`: When a document has been created in eDOCS
* `When document content is updated`: When a document file content has been updated in eDOCS
* `When document properties are updated`: When document properties have been updated in eDOCS
* `When a document is deleted`: When a document has been deleted in eDOCS (in case of queue for deletion)

### Folder
* `When a folder is created`: When a folder has been created in eDOCS
* `When a folder is updated`: When a folder has been updated in eDOCS
* `When a folder is deleted`: When a folder has been deleted in eDOCS (in case of queue for deletion)

### Item
* `When an item is deleted`: When a folder or document has been deleted in eDOCS (in case of hard delete)