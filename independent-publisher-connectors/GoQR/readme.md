# GoQR
Generate QR Codes with specific content. You can create a QRCode with url link or text. You can use the connector in PowerApps and Power Automate to get the image bytes.

## Publisher: Rui Santos
Rui Santos - https://www.linkedin.com/in/ruisantosnor

## Prerequisites
No required prerequisites, but you should take a look at https://goqr.me/ for premium functionalities

## Supported Operations
### Create QR Code
#### Summary

Generate QRCode in a easy way. The connector will return the QR Code image. QR codes are the way to go to create a link between the real world products (tagged with the QR code) and the Internet. Mobile, anywhere, anytime.

#### Example 

Go the Data -> Add the Custom Connector. In PowerApps insert a Image component and in the Image property write GoQR.Create("150x150","https://www.linkedin.com/in/ruisantosnor/"). Thhis will generate a QR Code image 150x150 with the url https://www.linkedin.com/in/ruisantosnor/.

#### Parameters

| Name | Description | Required | Schema |
| ---- | ----------- | -------- | ---- |
| data | The text to store within the QR code | Yes | string |
| size | Specifies the size of the QR code image you want to generate (in px for raster graphic formats like png, gif or jpeg); as logical unit for vector graphics (svg, eps). | No | string |
| charset-source | Specifies the charset the text submitted via data parameter is encoded in. Note: you don't have to care about converting your data if charset-source and charset-target got different values, the API does all the needed work automatically. | No | string |
| charset-target | Specifies the charset to encode the text submitted via data parameter with, before storing it within the QR code. Note: you don't have to care about converting your data if charset-source and charset-target got different values, the API does all the needed work automatically. | No | string |
| ecc | Defines the error correction code (ECC) which determines the degree of data redundancy. The more data redundancy exists, the more data can be restored if a QR code is damaged (i.e. scratches on a QR code sticker or something like that). | No | string |
| color | Color of the data modules as RGB value. | No | string |
| bgcolor | Color of the background as RGB value. | No | string |
| margin | Thickness of a margin in pixels. The margin will always have the same color as the background (you can configure this via bgcolor). It will not be added to the width of the image set by size, therefore it has to be smaller than at least one third of the size value. The margin will be drawn in addition to an eventually set qzone value. The margin parameter will be ignored if svg or eps is used as QR code format (=if the QR code output is a vector graphic). | No | integer |
| qzone | Thickness of a margin (="quiet zone", an area without disturbing elements to help readers locating the QR code), in modules as measuring unit. This means a value of 1 leads to a drawn margin around the QR code which is as thick as a data pixel/module of the QR code. The quiet zone will always have the same color as the background (you can configure this via bgcolor). The quiet zone will be drawn in addition to an eventually set margin value. | No | integer |
| format | It is possible to create the QR code picture using different file formats, available are PNG, GIF, JPEG and the vector graphic formats SVG and EPS (=you can create QR code vector graphics / QR code EPS / QR code SVG as needed for professional printing). | No | string |

#### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| default | Returned QR Code Image | binary |

#### Read more

Check [API Docs](https://goqr.me/api/doc/create-qr-code/) for the details regarding the endpoint.