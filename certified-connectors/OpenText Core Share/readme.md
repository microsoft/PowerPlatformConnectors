## OpenText™ Core Share
OpenText™ Core is a fast-deploying SaaS content management solution that delivers simple, agile and secure cloud-based content management. Core integrates into crucial business process applications, including SAP® S/4HANA Public Cloud, Salesforce and Microsoft® 365 to maximize employee productivity, accelerate business processes and enhance governance. Now you can bring content and processes even more together by combining it in your low code solutions.

## Pre-requisites
You will need the following to proceed:
* A license to use this connector purchasable in Azure Marketplace or AppSource.
* OpenText Core Share login credentials.

## How to get credentials
Once a license has been purchased (or a trial has been requested) an email will be sent with a link to the settings portal and login credentials for this portal. In this portal the connector can be configured and the API key (the credentials) needed to use the connector can be retrieved.

## Supported Operations
The connector supports the following operations:

### Document
* `Create document`: Create the document in Core Share
* `Update document`: Update the document in Core Share
* `Get document`: Get the document from Core Share
* `Get document version content`: Get the content of the document version from Core Share
* `Delete document`: Delete the document in Core Share
* `Unlock document`: Unlock the document in Core Share
* `Lock document`: Lock the document in Core Share
* `Get document versions`: Get the versions of the document from Core Share
* `Move document`: Move the document to a new folder in Core Share
* `Copy document`: Copy the document to a different folder in Core Share

### Folder
* `Create folder`: Create the folder in Core Share
* `Update folder`: Update the folder in Core Share
* `Get folder`: Get the folder from Core Share
* `Delete folder`: Delete the folder in Core Share
* `Get folder children`: Get the children of the folder from Core Share

### Search
* `Search`: Search on name in Core Share
* `Advanced search`: Search with specified query in Core Share