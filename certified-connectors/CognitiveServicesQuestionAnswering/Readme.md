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

## Building the connector for AAD Authentication 
Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD so that our connector can securely access the Microsoft Cognitive Services APIs.  After this setup, you can create and test the connector.

### Set up an Azure AD application for your custom connector
Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD.  This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This can be done using [Azure Portal](https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access Microsoft Cognitive Services APIs.
This step will ensure that your application can successfully retrieve an access token to Microsoft Cognitive Services rest APIs on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use "https://global.consent.azure-apim.net/redirect"
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, make sure "Microsoft Cognitive Services" and "user_impersonation" are added.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Microsoft Cognitive Services. The next step for us is to create a custom connector.

## Deploying the connector

Run the following command and follow the prompts:

```paconn
paconn create --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json] --icon [Path to icon.png] --secret <client_secret>
```

## Supported actions

The connector supports the following actions:

- `Generate answer from Project`: This action helps in answering the specified question using your knowledge base in your project.
- `Generate answer from provided text`: This action helps in answering the specified question using the provided text. To use only this action, Custom Question Answering need not be enabled on Language resource.
- `Get Project Metadata`: This action helps in getting all the metadata of your project. 