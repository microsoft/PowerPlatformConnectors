

## Scrive eSign Connector
The Scrive eSign connector makes functionality of the Scrive API available for use in Power Automate.


## Pre-requisites
A Scrive account with an API license. [Create a free trial account](https://www.scrive.com/get-started/)


## How to get credentials
Credentials are the username and password for the Scrive account.


## API documentation
The API documentation can be found [here](https://apidocs.scrive.com/)


## Supported Operations

- **When a document is signed by all signatories:** This trigger waits for the selected document to be signed.
- **When a document from template is signed by all signatories:** This trigger tracks any documents that were created using a specific template and waits for any of them to be signed.
- **When a document is signed by all signatories (Start Signing):** This trigger starts the signing process for the selected document and waits for it to be signed.
- **Create a document from template:** This action creates a new document, using the selected template.
- **Update fields based on template:** This action updates the fields of a selected document, based on the selected template.
- **Update properties based on template:** This action updates the properties of the selected document, based on the selected template.
- **Start signing process:** This action starts the signing process for the selected document.
- **Get document PDF content:** This action gets the PDF content of the selected document document, making it available in future steps in the flow.
- **Get document JSON:** This action gets the all the meta-data for the selected document in JSON format.
- **Update document JSON:** This action updates the document meta-data using a provided JSON.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
