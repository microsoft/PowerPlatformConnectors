# Azure Active Directory Connector for Application Registrations
Azure Active Directory provides a powerful and very extensive REST API. This connector exposes just the /Applications endpoint in Microsoft Flow and PowerApps. My main intent was to provide an easy way to find client secrets that were expired or near to expiry. To acheive this I have tweaked it to pull credential metadata along with the app details via $expand=Owners (see [https://learn.microsoft.com/en-us/graph/api/application-list?view=graph-rest-1.0&tabs=http#optional-query-parameters](https://learn.microsoft.com/en-us/graph/api/application-list?view=graph-rest-1.0&tabs=http#optional-query-parameters)

## Publisher: Paul Culmsee

## Pre-requisites
1. The application registration associated to this connector requires the delegated permission: Application.Read.All

2. If you want to retrieve ownership details beyond just the AAD object ID of the owner (eg displayName or UPN), you have have to Grant User.ReadBasic.All to the application registration associated to this connector

3. If you have restricted users ability to view the profile info of other users in their company (ie Set-MsolCompanySettings -UsersPermissionToReadOtherUsersEnabled $true), you will also need to grant the Directory Reader AAD Role to any user consenting to this connector.  

## Supported Operations
The connector supports the following operations:

### List applications
Get the list of applications in this organization.

### Get application
Get the details for the specified list application id.

### Get Application Owners
Get ownership details of the specificed applications id.

## Building the connector

Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD for connector to securely access them. After this setup, you can create and test the connector.

### Set up an Azure AD application for your custom connector

Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD. This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf. You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
   This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app). Once created, note down the value of Application (Client) ID. You will need this later.

2. Configure (Update) your Azure AD application to access the Graph API API
   This step will ensure that your application can successfully retrieve an access token to invoke Graph API calls. To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis). - For redirect URI, use "https://global.consent.azure-apim.net/redirect" - For the credentials, use a client secret (and not certificates). Remember to note the secret down, you will need this later and it is shown only once. - For API permissions, make sure the Graph API "Application.Read.All" and "User.ReadBasic.All" delegated permissions are added.

At this point, we now have a valid Azure AD application that can be used to getapp registration details. The next step for us is to create a custom connector.

### Deploying the sample

Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Things to consider
Do not edit this connector in the out of the box UI. This connector supports pagination, which is implemented by a policy template that does not appear in the list of available templates. If you modify the connector in UI, this policy template will be lost and pagination will not work. 

## Frequently Asked Questions
### Why have you not added the other application endpoints?
My intent here was to write flows that would look for application registrations with soon to be expiring client secrets. If you would like see other endpoints added, let me know or submit changes to this connector via github. 

