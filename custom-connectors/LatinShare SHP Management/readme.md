## LatinShare SHP Management Connector
This connector provides a set of SharePoint-related actions. Using LatinShare SHP Management API, you can for example, document, folder, list and site management.  You can copy, move and delete documents and folders, delete subsites, and enable version control on lists via URL. 

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
Checkout full API Documentation on [LatinShare Documentation](http://flow.latinshare.com/sitio2020/documentacion-sp-managments/)