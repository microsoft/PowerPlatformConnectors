## OpenText™ Extended ECM
OpenText™ Extended ECM is an enterprise content management platform that securely governs the information lifecycle by integrating with leading enterprise applications, such as SAP®, Microsoft® 365, Salesforce and SAP SuccessFactors®. You can bring content and processes more together by combining it in your low code solutions and provide access to information when and where it’s needed which improves decision-making and drives operational effectiveness. 
Also works with OpenText™ Content Server.

## Pre-requisites
You will need the following to proceed:
* A license to use this connector purchasable in Azure Marketplace or AppSource.
* OpenText Extended ECM with the REST and SOAP services online reachable.
* OpenText Extended ECM login credentials.

## How to get credentials
Once a license has been purchased an email will be sent with a link to the settings portal and login credentials to this portal. In this portal the connector can be configured and the API key needed to use the connector can be copied.

## Supported Operations
The connector supports the following operations:

### Document
* `Create document`: Create the document in Extended ECM
* `Update document`: Update the document in Extended ECM
* `Update document properties`: Update the document properties in Extended ECM
* `Update document content`: Update the content of the document in Extended ECM
* `Get document`: Get the document from Extended ECM
* `Get document properties`: Get the properties of the document from Extended ECM
* `Get document content`: Get the content of the document from Extended ECM
* `Get document version content`: Get the content of the document version from Extended ECM
* `Delete document`: Delete the document in Extended ECM
* `Delete document version`: Delete the document version in Extended ECM
* `Move document`: Move the document to a new folder in Extended ECM
* `Copy document`: Copy the document to a different folder in Extended ECM
* `Reserve document`: Reserve the document in Extended ECM
* `Unreserve document`: Unreserve the document in Extended ECM
* `Get document versions`: Get the versions of the document from Extended ECM

### Folder
* `Create folder`: Create the folder in Extended ECM
* `Update folder`: Update the folder in Extended ECM
* `Get folder`: Get the folder from Extended ECM
* `Delete folder`: Delete the folder in Extended ECM
* `Get folder children`: Get the children of the folder from Extended ECM

### Search
* `Search`: Search in Extended ECM
* `Advanced search`: Search in Extended ECM

### Web Report
* `Execute web report`: Execute a WebReport in Extended ECM

### Business Workspace
* `Create business workspace`: Create the business workspace in Extended ECM
* `Update business workspace`: Update the business workspace in Extended ECM
* `Get business workspace`: Get the business workspace from Extended ECM

## Supported Triggers
The connector supports the following triggers:

### Document
* `When a document is created`: When a document has been created in Extended ECM
* `When a document is updated`: When document properties or content have been updated in Extended ECM
* `When a document is deleted`: When a document has been deleted in Extended ECM

### Folder
* `When a folder is created`: When a folder has been created in Extended ECM
* `When a folder is updated`: When a folder has been updated in Extended ECM
* `When a folder is deleted`: When a folder has been deleted in Extended ECM

### Business Workspace
* `When a business workspace is created`: When a business workspace has been created in Extended ECM
* `When a business workspace is updated`: When a business workspace has been updated in Extended ECM
* `When a business workspace is deleted`: When a business workspace has been deleted in Extended ECM