# Office 365 Users Connector

The connector uses Microsoft Graph to perform operations on Office 365 account users and their relationships with other users. It aims to enable actions such as getting and updating the signed-in user profile, listing users within the organizations, getting user's manager and direct reports, listing the trending documents, etc.

## Pre-requisites

You will need the following to proceed:

- A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
- An Azure subscription
- [Microsoft Power Platform Connectors CLI](https://github.com/microsoft/PowerPlatformConnectors/tree/master/tools/paconn-cli)

## Setup

### Registering an Azure Active Directory application

Since the connector retrieve information about users and user-relationships stored in Azure Active Directory (AD), we need to register a [client application](https://docs.microsoft.com/en-us/azure/active-directory/develop/developer-glossary#client-application) in Azure AD to create and test the connector. The user of Power Apps or Power Automate will authorize this application to access the Microsoft Graph API on user's behalf. [Here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) is a great resource to understand the basics of authentication.

Please follow the steps as below:

1. Register a new application by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app). When configuring the application, use following selections:

   - **Supported account types** Select an option based on whether you would restrict users to your application's Azure AD tenant or if you would allow users from other tenants.
   - **Redirect URI** Select 'Web' and enter 'https://global.consent.azure-apim.net/redirect'.

2. After the app is registered, open 'API permissions' page from application's sidebar and click 'Add a permission'. Now select 'Micrsoft Graph' > 'Delegated permissions' and add all permissions as defined in [apiProperties.json](apiProperties.json) i.e. `Sites.Read.All User.ReadBasic.All User.ReadWrite`.

3. Open 'Certificates and secrets' page and click 'New client secret' > 'Add'. This will create a new row with secret. Note down the secret value somewhere. It is displayed only once.

4. Open Overview page and note down the value of 'Application (client) ID'.

### Creating the connector

1. Make sure that you have [Microsoft Power Platform Connectors CLI](https://github.com/microsoft/PowerPlatformConnectors/tree/master/tools/paconn-cli) installed.
2. Clone this GitHub repo. Open the command shell and change to this directory.
3. Open file [apiProperties.json](apiProperties.json). Use the client ID you noted down above as the value of `clientId` property.
4. Run the following commands with `<clientSecret>` replaced with the actual client secret and follow the prompts:

```
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <clientSecret>
```

## Supported Operations

| Operation Name                | Description                                                                                  |
| ----------------------------- | -------------------------------------------------------------------------------------------- |
| **Update my profile**         | Updates the profile of the current user                                                      |
| **Update my profile photo**   | Updates the profile photo of the current user. The size of the photo must be less than 4 MB. |
| **Get my trending documents** | Retrieves the trending documents for the signed in user                                      |
