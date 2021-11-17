# PowerBI Management Api
Power BI is a business analytics service by Microsoft. It aims to provide interactive visualizations and business intelligence capabilities with an interface simple enough for end users to create their own reports and dashboards. This connector provides access to the Power BI Admin API (https://docs.microsoft.com/en-us/rest/api/power-bi/admin) with a focus on collecting data on Workspaces, Reports, Dashboards, Datasets and real-time activity.  

## Publisher: Paul Culmsee

## Prerequisites
The user must have administrator rights (such as Office 365 Global Administrator or Power BI Service Administrator) to call this API 

## Building the connector

Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD for connector to securely access them. After this setup, you can create and test the connector.

### Set up an Azure AD application for your custom connector

Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD. This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf. You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
   This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app). Once created, note down the value of Application (Client) ID. You will need this later.

2. Configure (Update) your Azure AD application to access the PowerBI Api.
   This step will ensure that your application can successfully retrieve an access token to invoke API calls on behalf of your users. To do this, Register an application in Azure AD and grant it delegated Tenant.Read access to the PowerBI Service (https://analysis.windows.net/powerbi/api). For redirect URI, use "https://global.consent.azure-apim.net/redirect" - For the credentials, use a client secret (and not certificates). Remember to note the secret down, you will need this later and it is shown only once.

At this point, we now have a valid Azure AD application that can be used to get application registration information from Azure Active Directory. The next step for us is to create a custom connector.

### Deploying the sample

After downloading the connector, edit the client ID and tenant ID in apiProperties.json to your app registration and tenant. Use paconn CLI to deploy by running the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```


## Supported Operations
Required. Describe actions, triggers, and other endpoints.​
### Get Groups as Admin
Returns a list of workspaces for the organization

### Get Dataflows As Admin
Returns a list of dataflows for the organization

### Get Dataflow Datasources As Admin
Returns a list of datasources for the specified dataflow

### Get Dashboards As Admin
Returns a list of dashboards for the organization

### Get Tiles As Admin
Returns a list of tiles within the specified dashboard

### Get Reports As Admin
Returns a list of reports for the organization

### Get Dashboards In Group As Admin
Returns a list of dashboards from the specified workspace

### Get Dataflows In Group As Admin
Returns a list of dataflows from the specified workspace

### Get Reports In Group As Admin
Returns a list of reports from the specified workspace

### Get Datasets As Admin
Returns a list of datasets for the organization

### Get Datasets In Group As Admin
Returns a list of datasets from the specified workspace

### Get Data Sources for Dataset As Admin
Returns a list of datasources for the specified dataset

### Get Activities As Admin
Returns a list of audit activity events for a tenant

## Obtaining Credentials
This connector uses oAuth with Tenant.Read.All on Delegated AND Application scope ​

## Getting Started
Optional. How to get started with your connector.

## Known Issues and Limitations
Required. Include any known issues and limitations a user may encounter.

## Frequently Asked Questions
### How to I create a client ID and secret?
Register an application in Azure AD and grant it delegated Tenant.Read access to the PowerBI Service (https://analysis.windows.net/powerbi/api).

### I received a 'Could not find member '1stackOwner' on object of type 'ApiPropertiesDefinition' error when deploying via paconn CLI
Currently, there is a limitation that prevents you from updating your connector's artificats in your environment using Paconn when the stackOwner property is present in your apiProperties.json file. As a workaround to this, edit apiProperties.json and remove the stackOwner property. Microsoft are working to remove the limitation and will update this FAQ once complete.

