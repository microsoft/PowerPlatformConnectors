# PDFMonkey

PDFMonkey generates PDF and image documents from your reusable HTML/Liquid templates and a JSON data payload, through a simple REST API. Design templates in the PDFMonkey dashboard, then generate documents on demand from your flows and download the results.

## Publisher: PDFMonkey

## Prerequisites
You will need a PDFMonkey account. You can create one on the [PDFMonkey sign-up page](https://dashboard.pdfmonkey.io/register).

## Obtaining Credentials
Sign in to PDFMonkey and open your [Account page](https://dashboard.pdfmonkey.io/account). Copy your Secret Key and paste it when you create a connection for this connector.

## Supported Operations
### Generate a document
Creates a document from one of your templates and a JSON payload. Generation is asynchronous: the response returns immediately with the status "pending". Use the *When a document is generated* trigger, or *Get a document*, to retrieve the finished file.
### Get a document
Retrieves a document by its ID, including its download URL once generation has succeeded.
### Delete a document
Deletes a document by its ID.
### When a document is generated
Triggers a flow when a document finishes generating successfully in the selected workspace. You can optionally filter to specific templates.

## Known Issues and Limitations
Document generation is asynchronous. *Generate a document* returns a document with the status "pending" and an empty download URL; the download URL is populated only once the status becomes "success". Use the *When a document is generated* trigger, or poll *Get a document*, to obtain the finished file. Download URLs are valid for one hour.
