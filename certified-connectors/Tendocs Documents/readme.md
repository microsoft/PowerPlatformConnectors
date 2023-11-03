## Tendocs Templating Connector
Tendocs Templating is a fast, simple, and feature rich solution for automating the creation of office documents (docx, pptx and xlsx). It has connectors ready to go for Power Automate and Logic Apps, and can be integrated into any solution through an easy to use REST API.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A valid [Tendocs Account](https://www.tendocs.com/pricing), plan subscription and API key.
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
An API Key can be obtained via the [Tendocs](https://www.tendocs.com) web site, by selecting, signing up to, and paying for a subscription plan.

### Connector Security setup
Use the following values for the the connector security page:
* `Authentication type` : API Key
* `API Key` provided through your Tendocs account.

## Supported Operations
The connector supports the following operations:
* `Create New Document From Template`: Creates a new document based on a Template (Word, PowerPoint or Excel) that contains any number of Document Replacement Tokens (e.g. {{Name}}, {{Date}}, {{Invoice.Amount}}). The service match any number of text, image, document or tables to these tokens, returning a new document in it's original, PDF or HTML format, that merges it all together.
* `Convert Document To New File Format`: Provides for the conversion of standard Office documents (docx, pptx, xlsx) to a variety of other document formats, including PDF and HTML.

## Known Issues and Limitations
- Number of Text Token limited to a maximum of 128.
- Total Rows across all tables limited to 10,000.
- Size of all images should be below 200kb.
- Size of all sub-documents combined should be below 200kb.
- Images are not exported in HTML conversions.