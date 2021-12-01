# GoQR
Generate QR Codes with specific content. You can create a QRCode with url link or text. You can use the connector in PowerApps and Power Automate to get the image bytes.

## Publisher: Rui Santos
Rui Santos - https://www.linkedin.com/in/ruisantosnor

## Prerequisites
No required prerequisites, but you should take a look at https://goqr.me/ for premium functionalities  

## Supported Operations
#### GET
##### Summary

Create QRCode with a specific size. Returns the binary data of the QR Code image.

##### Example 

Go the Data -> Add the Custom Connector. In PowerApps insert a Image component and in the Image property write GoQR.Create("150x150","https://www.linkedin.com/in/ruisantosnor/"). Thhis will generate a QR Code image 150x150 with the url https://www.linkedin.com/in/ruisantosnor/.

# Version: 1.0

### /

#### GET
##### Summary

Create QRCode with a specific size

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| size | query | The size represent with width and height of the QR code, expects a string. Example 150x150 | Yes | string |
| data | query | The data field represent the content of the QR code when is read. It can be text or urls. | Yes | string |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| default | default | binary |
