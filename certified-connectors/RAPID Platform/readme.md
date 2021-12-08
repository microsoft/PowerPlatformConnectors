## RAPID Platform Connector
The RAPID Platform provides a highly customisable data creation and tracking tools for business. Using this API you can automate business logic based on your own specific needs.

## Prerequisites
You will need the following to proceed:
* A RAPID Platform site
* A API User registraion in your site
* The Power platform CLI tools

## Building the connector 
Since RAPID Platform APIs are secured by Azure Active Directory (AD), we first need to set up a few thing in Azure AD so that our connectors can securely access your RAPID site.  After that is completed, you can create and extend your RAPID connector.

### Set up an Azure AD application for your custom connector
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access RAPID Platform on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This Azure AD application will be used to identify the connector to RAPID Platform.  This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access the RAPID Platform API
This step will ensure that your application can successfully retrieve an access token to invoke Azure Key Vault on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, make sure the RAPID Platform permissions are added to your app "cd5db0ec-1419-4ae6-9434-21cfb83fc42d"
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access RAPID Platform.  The next step for us is to create a custom connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Register Webhooks`: Add a webhook registration to your RAPID site to trigger Flow when desired
* `Get Items`: Get a list of items on one of your tables in your site
* `Get Item`: Get a single item from one of your tables in your site
* `Create Item`: Creates a new item for a table on your site
* `Update Item`: Update Item on your site
* `Get inherit links`: Fetch the list of expected linked items for a given item you are creating
* `Add Attachments`: Add attachment records to items on your site