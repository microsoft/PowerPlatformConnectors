This custom connector is used to convert MS Word document to PDF file with fidelity in imbedded charts, graphs and graphics. This connector calls our company API service which passes the document and credentials , the API service receives the MS Word document and all content and convert it PDF file., once the conversion is completed the API send it back as a PDF to the connector / environment, depending on the configuration of the workflow the file is then deposited in the identified location. We created this tool for our clients to serve a need and feel this may be a connector that has a space in the market

Convert Doc to PDF: convert list attached word doc file to PDF file
Operation requestion:
	1. Display name:  this is the file name
	2. Attachment Content: file content
	3. Publickey: a public key that issued by Spot Solutions
	4. Apikey; a api key that issued by Spot Solutions

Operation responses
	1. status: return string "200 OK" is convert success. Otherwise will be the error message.
	2. File name: is the PDF file name
	3. File content: is the PDF file content in base64 encode format.