# PowerBI Management Api
Power BI is a business analytics service by Microsoft. It aims to provide interactive visualizations and business intelligence capabilities with an interface simple enough for end users to create their own reports and dashboards. This connector provides access to the Power BI Admin API (https://docs.microsoft.com/en-us/rest/api/power-bi/admin) with a focus on collecting data on Workspaces, Reports, Dashboards, Datasets and real-time activity.  

## Publisher: Paul Culmsee

## Prerequisites
The user must have administrator rights (such as Office 365 Global Administrator or Power BI Service Administrator) to call this API 

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

## Deployment Instructions
Create a new application registration in Azure AD and enable API access to the PowerBI Service with Tenant.Read for Delegated permissions.
After downloading the connector, edit the client ID and tenant ID in apiProperties.json to your app registration and tenant. 
Please use paconn CLI to deploy.