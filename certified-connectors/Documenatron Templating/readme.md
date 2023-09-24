## Documenatron Templating Connector
Documenatron Templating is a fast, simple, and feature rich solution for automating the creation of office documents (docx, pptx and xlsx). It has connectors ready to go for Power Automate and Logic Apps, and can be integrated into any solution through an easy to use REST API.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A valid [Documenatron Account](https://www.documenatron.com/signin) and API key.
* The Power platform CLI tools

## Deployment Instructions
The connector can be deployed in one of two ways. 

### Using the Power Platform Connector CLI
Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
### Manual upload
Use the [Power Automate Portal](https://flow.microsoft.com) to create a new custom connector. Use **Import an OpenAPI file** and select the apiDefinition.swagger.json file.

## Obtaining Credentials
An API Key can be obtained via the [Documenatron](https://www.documenatron.com) web site, by selecting, signing up to, and paying for a subscription.  

### Connector Security setup
Use the following values for the the Oath 2.0 security page:
* `Authentication type` : API Key
* `API Key` provided through your Documenatron account.

## Supported Operations
The connector supports the following operations:
* `Create New Document From Template`: Creates a new document based on a Template (Word, PowerPoint or Excel) that contains any number of Document Replacement Tokens (e.g. {{Name}}, {{Date}}, {{Invoice.Amount}}). The service match any number of text, image, document or tables to these tokens, returning a new document that merges it all together.

## Known Issues and Limitations
- Number of Text Token limited to a maximum of 128.
- Total Rows across all tables limited to 10,000.
- Size of all images should be below 200kb.
- Size of all sub-documents combined should be below 200kb. 
