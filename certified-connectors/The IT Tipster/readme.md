# The IT Tipster

This node js application contains 2 api's that generate QR codes and Barcodes. Each APi is a post request and takes in a parameter of barcodeNumber and qrcodeText.

Returned from each API is the base64 of a png image. 

## /api/generateBarcode
This api is a post request and takes in an API key of X-API-KEY and a body parameter of barcodeNumber, it then returns a base64 representation of a barcode.

## /api/generateQRCode
This api is a post request and takes in an API key of X-API-KEY and a body parameter of qrcodeText, it then returns a base64 representation of a QR Code.