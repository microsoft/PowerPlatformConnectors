
## Azure Active Directory Connector
Azure Active Directory provides a powerful and very extensive REST API.  Using this API, you can create and manage objects (e.g. users, groups etc.) in Azure Active Directory. This connector exposes a subset of these APIs as operations in Microsoft Flow and PowerApps.



## Pre-requisites
You will need the following to proceed:
* A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools

## Building the connector 
Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few thing in Azure AD for connector to securely access  them.  After this setup, you can create and test the connector.

### Set up an Azure AD application for your custom connector
Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD.  This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access the Azure Active Directory API
This step will ensure that your application can successfully retrieve an access token to invoke Azure Active Directory rest APIs on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use "https://global.consent.azure-apim.net/redirect"
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, make sure "Azure Active Directory" and "user_impersonation" are added.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Azure Active Directory.  The next step for us is to create a custom connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Create Office 365 group`: Create an Office 365 group in your AAD tenant
* `Create Security group`: Create a security group in your AAD tenant
* `Get group`: Get details for a group
* `Get user`: Get details for a user
* `Get groups of a user`: Get the groups a user is a member of
* `Create user`: Create a new user in your AAD tenant
* `Get group members`: Get the users who are members of a group
* `Remove Member From Group`: Remove Member From Group
* `Add user to group`: Add a user to a group in this AAD tenant
* `Check group membership`: If the user is a member of the given group, the result will contain the given id. Otherwise the result will be empty
* `Assign manager`: Assign a manager for a user
* `Update user`: Update the info for a user




