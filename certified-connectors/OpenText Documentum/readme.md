# OpenText Documentum
Empower OpenText Documentum as the primary repository for all things content related. Together with the OpenText Documentum Power Platform Connector by One Fox you can streamline content processes across a widespread application landscape and limit information silos.

## Publisher
One Fox

## Prerequisites
You will need the following to proceed:
* A license to use this connector purchasable in Azure Marketplace or AppSource.
* OpenText Documentum with the REST service online reachable.
* OpenText Documentum login credentials.

## Supproted Operations

### Document
* `Create document`: Create the document in Documentum
* `Update document`: Update the document in Documentum
* `Update document properties`: Update the document properties in Documentum
* `Update document content`: Update the content of the document in Documentum
* `Get document`: Get the document from Documentum
* `Get document properties`: Get the properties of the document from Documentum
* `Get document content`: Get the content of the document from Documentum
* `Delete document`: Delete the document in Documentum

## How to get credentials
Once a license has been purchased (or a trial has been requested) an email will be sent with a link to the settings portal and login credentials for this portal. In this portal the connector can be configured and the API key (the credentials) needed to use the connector can be retrieved.

## Known issues and limitations
The amount of data is monthly limited depending on the license (amount of requests and total transfer size).

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
