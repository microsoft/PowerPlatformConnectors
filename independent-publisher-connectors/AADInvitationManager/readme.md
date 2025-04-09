## Azure AD Identity and Access

The Graph API is a powerful and very extensive REST API. Using this API, you can create and manage objects (e.g. users, groups etc.) in Azure Active Directory, create Microsoft Teams teams, and a lot more.

This connector exposes the Identity and Access resources of the Graph API in the Microsoft Power Platform.

## Publisher: Daniel Laskewitz | Sogeti

## Pre-requisites

You will need the following to proceed:

- A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
- An Azure subscription
- The Power platform CLI tools

## Building the connector

Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD for connector to securely access them. After this setup, you can create and test the connector.

### Set up an Azure AD application for your custom connector

Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD. This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf. You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
   This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app). Once created, note down the value of Application (Client) ID. You will need this later.

2. Configure (Update) your Azure AD application to access the Graph API API
   This step will ensure that your application can successfully retrieve an access token to invoke Graph API calls on behalf of your users. To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis). - For redirect URI, use "https://global.consent.azure-apim.net/redirect" - For the credentials, use a client secret (and not certificates). Remember to note the secret down, you will need this later and it is shown only once. - For API permissions, make sure the Graph API "User.Invite.All" permission is added.

At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Azure Active Directory. The next step for us is to create a custom connector.

### Deploying the sample

Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations

The connector supports the following operations:

- `Create invitation`: Create invitation adds an external user to the organization.
