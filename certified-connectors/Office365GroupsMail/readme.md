# Microsoft 365 Groups Mail Connector

Microsoft Graph REST API v1.0 allows users to create and manage Microsoft 365 Groups functionality. This connector exposes a subset of these APIs as operations in Microsoft Flow and PowerApps.
See more documentation [[here](https://docs.microsoft.com/en-us/graph/api/resources/groups-overview?view=graph-rest-1.0)].

## Pre-requisites

You will need the following to proceed:

- A Microsoft PowerApps or Microsoft Flow plan with custom connector feature.
- An Azure subscription
- The Power platform CLI tools
- Be a member of a Microsoft 365 Group

## Building the connector

Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few thing in Azure AD for connector to securely access them. After this setup, you can create and test the connector.

### Set up a new application in the Azure Active Directory for your custom connector

Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD. This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf. You can read more about this [[here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-vs-authorization)] and follow the steps below:

1. Create an Azure AD application This can be done using [Azure Portal](https://portal.azure.com), by following the steps [[here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)]. Once created, note down the value of Application (Client) ID. You will need this later.

2. Configure (or update) your application to access the Azure Active Directory API This step will ensure that your application can successfully retrieve an access token on behalf of your users. To do this, follow the steps [[here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis)].

   - For redirect URI, use `https://global.consent.azure-apim.net/redirect`
   - For the credentials, use a **client secret** (and not certificates).
        >Remember to note the secret down, you will need this later and it is shown only once.

### Deploying the connector

Run the following commands and follow the prompts:

```bash
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations

The connector supports the following operations:

- `ListConversations`: Get all the conversations in this group.
- `GetGroupConversation`: Retrieves the properties of a particular conversation.
- `ListConversationThreads`: Get all the threads in a group conversation.
- `ListGroupThreads`: Get all the threads of a group.
- `GetConversationThread`: Get a specific thread that belongs to a group.
- `DeleteConversationThread`: Deletes the specified conversation thread.
- `ListThreadPosts`: Get all the posts of the specified conversation thread.
- `GetThreadPost`: Retrieves a post in a specified thread.
- `GetAttachments`: Retrieve a list of attachments that belong to a post.
- `ListGroups`: Retrieves all Microsoft 365 Groups the user is part of (operation with `internal` visibility used to populate the dropdowns).

## Notes

If you want that all users are able to sign in (and use the connector), instead of only admins, you can grant admin consent to the application. Follow instructions on how to grant admin consent to an application [[here](https://docs.microsoft.com/en-us/azure/active-directory/manage-apps/grant-admin-consent)].
