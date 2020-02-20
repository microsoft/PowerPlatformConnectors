

## Aquaforest PDF Connector
The Aquaforest PDF Connector contains a group of actions that use the information available in PDF files to perform some simple operations for Office 365 and Flow.


## Pre-requisites
First of all, you need to [Create an Aquaforest PDF API Account](https://aquaforest-pdf.portal.azure-api.net/signup).


## API documentation
The API documentation can be found [here]([https://www.aquaforest.com/en/aquaforest-flow-doc.asp](https://www.aquaforest.com/en/aquaforest-flow-doc.asp))


## Supported Operations

 - **Get Data from PDF:** This action will extract important data from PDF files in the form of Key/Value pairs.
 
 - **Get Text from PDF:** Extracts text from a PDF files in a smart way, the extracted information can be used to rename the file in Power Automate, it can also be used as an input to other processes.
 - **Get Barcode Value:** Extracts barcode from a PDF files in a smart way, the extracted information can be used to rename the file in Power Automate, it can also be used as an input to other processes.
 - **Split PDF by Barcode:** Uses barcode values in PDF files to split the PDF file, you can also generate file names for the split files based on the barcode values.
 - **Split PDF by Text match:** Uses text matches in PDF files to split the PDF file, you can also generate file names for the split files based on the barcode text matches.
 - **Extract pages by Barcode:** Uses barcode values in PDF files to extract pages from the PDF file, you can also generate file names for the extracted files based on the barcode values.
 - **Extract pages by text:** Uses text matches in PDF files to extract pages from the PDF file, you can also generate file names for the split files based on the barcode text matches.
 - **OCR PDF or images:** Generate searchable PDF from an image PDF or scanned images.
 - **Get PDF properties:** Gets the information about a PDF file.

## How to get credentials

Instructions to get credentials can be found [here]([https://www.aquaforest.com/en/aquaforest-flow-doc.asp#chapter1-2](https://www.aquaforest.com/en/aquaforest-flow-doc.asp#chapter1-2)).

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps

