
## NodefusionPortal Connector
This is a Nodefusion Portal connector. The connector is published here with the intent to provide Nodefusion customers with the actions they already have in Nodefusion Portal so they can automate its processes.

## Prerequisites
You will need the following to proceed:
* Valid Nodefusion Customer account on 3procloud.onmicrosoft.com with valid access to your organization
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* Azure App Credentials provided by Nodefusion

### Creating a connection
You first need to acquire OAuth ClientId and ClientSecret from your cloud provider, Nodefusion. This will allow the connector to acquire user impersonated connection for accessing the Nodefusion Portal backend:

1. Contact support@nodefusion.com and ask for your ClientId-ClientSecret pair.

2. Configure (Update) your Azure AD application to access the Azure Key Vault API
This step will ensure that your application can successfully retrieve an access token to invoke Azure Key Vault on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, make sure “Azure Key Vault” and “user_impersonation” are added.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Azure Key Vault.  The next step for us is to create a custom connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Get Organization Profile`: Get your organization profile properties
