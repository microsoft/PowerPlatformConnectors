
## Planner Connector
Planner API in Microsoft Graph lets you create tasks and assign them to users in a group in Office 365. It enables you to easily bring together teams, tasks, documents, and conversations for better results. This connector exposes a subset of this API as operations in Microsoft Power Automate and Power Apps.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools

## Building the connector 
Since the API used by the connector is secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD for connector to securely access  them.  After this setup, you can create and test the connector.

### Set up an Azure AD application for your custom connector
Since the connector uses OAuth as the authentication type, we first need to register an application in Azure AD.  This application will be used to get the authorization token required to invoke the rest API used by the connector on the user's behalf.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This can be done on the [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access Microsoft Graph API. This step will ensure that your application can successfully retrieve an access token to invoke the Microsoft Graph Rest API on behalf of your users. To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use "https://global.consent.azure-apim.net/redirect"
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, among other required permissions under "Microsoft Graph", please make sure "User.Read" is added. "User.Read" permission is required in order to login the current user and retrieve relevant information.
   
We now have a valid Azure AD application that can be used to get permissions from end users and authorize calls to Microsoft Graph. The next step for us is to create a custom connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Get a task`: Get an existing Planner task
* `Create a task`: Create a new task in Planner
* `Update a task (V2)`: Update an existing Planner task
* `Add assignees to a task`: Add assignees to an existing Planner task
* `Remove assignees from a task`: Remove assignees from an existing Planner task
* `Get task details`: Get the task details for an existing task
* `Update task details`: Update the task details for an existing task
* `List tasks`: List the tasks in a plan
* `List my tasks`: List the tasks assigned to me
* `List buckets`: List the buckets in a plan
* `Create a bucket`: Create a bucket in Planner for the specified plan and group
* `List plans for a group`: List plans owned by the group specified




