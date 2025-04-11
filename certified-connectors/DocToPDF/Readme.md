# Doc To PDF Converter
This connector provides you with the ability to generate a PDF from a Word document while maintaining the fidelity of the images, charts or graphs included in the source document.
## Publisher: Spot Solutions Ltd.

## Prerequisites
There are no prerequisites to using this connector

## Supported Operations
Convert Word Document to PDF: Converts list attached word doc file to PDF file

1.	Convert Word Document to PDF: Converts list attached word doc file to PDF file
	Operation requestion:
	1. Display name:  this is the file name
	2. Attachment Content: file content
	3. Publickey: a public key that issued by Spot Solutions
	4. Apikey; a api key that issued by Spot Solutions

	Operation responses:
	1. status: return string "200 OK" is convert success. Otherwise will be the error message.
	2. File name: is the PDF file name
	3. File content: is the PDF file content in base64 encode format.

2.	Convert Word Document to PDF: Converts word doc file in a Document Library to PDF file
	Operation requestion:
	1. Display name:  this is the file name
	3. Publickey: a public key that issued by Spot Solutions
	4. Apikey; a api key that issued by Spot Solutions
	
	Operation responses:
	1. status: return string "200 OK" is convert success. Otherwise will be the error message.
	2. File name: is the PDF file name
	3. File content: is the PDF file content in base64 encode format.

## Obtaining Credentials
Goto website https://registration.powerstore.spotsolutions.com/ to register 
Or contact Spot Solutions Support at support@spotsolutions.com to request the APIKey.

## Getting Started
The first step in using this connector is having a List or Library prepared for the attachment or storage of your source word documents. Once you have this in place you can create a workflow that uses this connector to convert the source MS Word file into a PDF.

## Known Issues and Limitations
This connector does not convert Excel or PowerPoint files to PDF

## Deployment Instructions
Once the Connector is downloaded and the APIkey requested enter the provided key into the provided connector fields to active the connector for the workflow.
