# Language - Question Answering Connector
The Language - Question Answering API is a cloud-based service that provides advanced natural language processing with state of the art transformer models to 
generate answers against custom question answering projects or raw text. Question answering is a feature in Language service which is enhanced from the QnA Maker service with additional features 
like support for unstructured documents and precise answering.
This connector includes three  main actions: 
1) Generate answer from Project
2) Generate answer from provided text
3) Get Project Metadata

This connector exposes these actions as operations in Microsoft Power Automate and Power Apps.

This connector is a new connector which can be readily used with Question Answering.

To use this integration, you will need an [Azure Cognitive Service for language](https://aka.ms/create-language-resource) resource with Custom Question Answering enabled. 
You will get an endpoint and a key for authenticating your applications.
To make a connection, provide the Account key, site URL and select Create connection. For operation costs on your connection, learn more at [Pricing - Language Service](https://azure.microsoft.com/en-us/pricing/details/cognitive-services/language-service/) .

You can create connectors in these regions : [Regions overview for Power Automate](https://docs.microsoft.com/en-us/power-automate/regions-overview)
## Pre-requisites

You will need the following to proceed:

- A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
- The Power platform CLI tools

## Deploying the connector

Run the following command and follow the prompts:

```paconn
paconn create --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json] --icon [Path to icon.png] 
```

## Supported actions

The connector supports the following actions:

- `Generate answer from Project`: This action helps in answering the specified question using your knowledge base in your project.
- `Generate answer from provided text`: This action helps in answering the specified question using the provided text. To use only this action, Custom Question Answering need not be enabled on Language resource.
- `Get Project Metadata`: This action helps in getting all the metadata of your project. 