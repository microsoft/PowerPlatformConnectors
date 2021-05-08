
## Azure AD Risky Users Connector
With this connector you can get and dismiss risky users from Azure AD Identity Protection



## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature

## Building the connector 
Since the Azure AD authentication methods APIs are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD so that our connectors can securely access the phone methods. After that is completed, you can create and test the sample connector.

### Set up an Azure AD application for your custom connector
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access Graph API data on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This Azure AD application will be used to identify the connector to the Microsoft Graph API.  This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access the Risky Users API
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, Graph API **delegated** permissions **IdentityRiskyUser.ReadWrite.All** needs to be added to the app registration. Don't forget to grant admin access.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Azure AD authentication methods.  The next step for us is to create a custom connector.

### Create the custom connector
You can install the connector using the paconn CLI tool, or manually upload the file to Power Automate.

#### Using paconn
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

#### Manual upload
Use the [Power Automate Portal](https://flow.microsoft.com) to create a new custom connector. Use **Import from Github** and select the master branche. This connector will show up in the list.

### Connector Security setup

Use the following values for the the Oath 2.0 security page:

* `Authentication type` : OAuth 2.0
* `Client id`: the application ID from the app registration
* `Client secret` the secret from the app registration
* `Tenant ID`: common
* `Resource URL` : https://graph.microsoft.com


## Supported Operations
The connector supports the following operations:
* `List Risky users`: Retrieve all users that are tagged as risky.
* `Dismiss Risky user`: Dismiss the risky for a user.
* `Get Risky user`: Get the details of single risky user.

#### Blog post example
Please, check this blog post [Bulk dismiss risky users with Power Automate or Logic Apps](https://janbakker.tech/bulk-dismiss-risky-users-with-power-automate-or-logic-apps/) for detailed steps. 
