# The IT Tipster
This node js application contains 2 api's that generate QR codes and Barcodes. Each APi is a post request and takes in a parameter of barcodeNumber and qrcodeText.
Returned from each API is the base64 of a png image. 

## Publisher: Publisher's Name
The IT Tipster

## Prerequisites
You will need the following to use this connector:
* An API Key (Can be obtained by contacting support@theittipster.com

## Supported Operations
The connector supports the following operations:
* Generate Barcodes : Generates a code 128 barcode from a string
* Generate QR Code : Generates a QR Code from a string
### Operation 1
This api is a post request and takes in an API key of X-API-KEY and a body parameter of barcodeNumber, it then returns a base64 representation of a barcode.

### Operation 2
This api is a post request and takes in an API key of X-API-KEY and a body parameter of qrcodeText, it then returns a base64 representation of a QR Code.

## Obtaining Credentials
Authentication is done via an API key. In order to obtain an API key please contact us at support@theittipster.com

## Known Issues and Limitations
The barcode and QR Code are returned in base64 format, in order for there to be saved as image files the user will need to convert the base64 to binary. This can be easily done in power automate using the base64tobinary() function.

## Deployment Instructions
As the IT Tipster connector uses an API key to validate the user, you will need to contact our support team to obtain one. The email is support@theittipster.com
Once you receive your api key, you will need to add it as the connection when selecting our connector.
When using the connector, either the barcodeGenerator or QRCodeGenerator you will need to pass a string to each action in order for them to work. When they work successfully you will get them returned in base64 format.
