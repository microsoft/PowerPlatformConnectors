# Daraja api connector
 
## Table of contents

- [overview](#overview)
- [Prerequisites](#Prerequisites)
- [Known_issues](#known_issues)
- [Authors](#authors)

## overview
The daraja api connector allows low code developers to access safaricom mpesa services and intergrate them to their flows,power app or any othe service in power platform

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


## known_issues
known issues will come here

## authors
- bernard karaba  bkaraba14@gmail.com
- john muchiri  jeanfrancaiseharicot@gmail.com
