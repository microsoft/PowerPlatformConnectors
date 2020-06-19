
### NOTE
> This is a *sample* connector.  The connector is provided here with the intent to illustrate certain features and functionality around connectors.

## Azure Key Vault [Sample] Connector
Azure Key Vault provides a powerful and very extensive REST API.  Using this API, you can manage your keys, certificates, and secrets in an Azure Key Vault account.  Very often, you may want to leverage those secrets in your application, or in your process automation.



## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools

## Building the connector 
Since Key Vault APIs are secured by Azure Active Directory (AD), we first need to set up a few thing in Azure AD so that our connectors can securely access the Key Vault.  After that is completed, you can create and test the sample connector.

### Set up an Azure AD application for your custom connector
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access Key Vault data on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This Azure AD application will be used to identify the connector to Azure Key Vault.  This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

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
* `List keys`: List keys in the specified vault
* `Get key`: Gets the public part of a stored key
* `Create key`: Creates a new key
* `Decrypt data`: Decrypts a single block of encrypted data
* `Encrypt data`: Encrypts an arbitrary sequence of bytes using an encryption key
* `List secrets`: List secrets in a specified key vault
* `Get secret`: Get a specified secret from a given key vault
* `Create or update secret value`: Sets a secret in a specified key vault
* `List secret versions`: List all versions of the specified secret
* `Get secret version`: Get the value of a specified secret version from a given key vault




