## LatinShare Documents Connector
Using LatinShare Documents API, you can generate documents in Microsoft Power Automate, Azure Logic Apps or Power Apps.
This connector provides a set of actions related to Docx, HTML and PDF documents. You can create documents from templates (DOCX or HTML files) and convert them to PDF.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A valid account. generated when purchasing a product at http://flow.latinshare.com/
* Generate an API key in the account provided at the time of purchase
* The Power platform CLI tools

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon [Path to icon.png]
``

## Supported Operations
Checkout full API Documentation on [LatinShare Documentation](http://flow.latinshare.com/sitio2020/documentacion-documents/)