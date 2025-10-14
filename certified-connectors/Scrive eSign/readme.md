

## Scrive eSign Connector
The Scrive eSign connector makes functionality of the Scrive API available for use in Power Automate.


## Pre-requisites
A Scrive account with an API license. [Create a free trial account](https://www.scrive.com/get-started/)


## How to get credentials
Credentials are the email and password for the Scrive account.


## API documentation
The API documentation can be found [here](https://apidocs.scrive.com/)


## Supported Operations

- **When a document is signed by all signatories:** This trigger waits for the selected document to be signed.
- **When a document from template is signed by all signatories:** This trigger tracks any documents that were created using a specific template and waits for any of them to be signed.
- **When a document is signed by all signatories (Start Signing):** This trigger starts the signing process for the selected document and waits for it to be signed.
- **Add new party:** Adds a new party to the document as a signatory, viewer or approver.
- **Append file:** Appends a PDF to the main PDF of the document.
- **Cancel:** Cancels the signing process for a started document.
- **Create a document from template:** Creates a new document using the selected template.
- **Create a document from PDF:** Creates a new document using the provided PDF content.
- **Get document JSON:** Gets the all the meta-data for the selected document in JSON format.
- **Get document PDF content:** Gets the PDF content of the selected document document, making it available in future steps in the flow.
- **Set attachment:** Adds provided PDF content as an attachment to the document.
- **Set file for document:** Updates the main PDF file for the document.
- **Start signing process:** Starts the signing process for the selected document.
- **Update document JSON:** Updates the document meta-data using a provided JSON.
- **Update fields based on template:** Updates the fields of a selected document, based on the selected template.
- **Update properties based on template:** Updates the properties of the selected document, based on the selected template.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
