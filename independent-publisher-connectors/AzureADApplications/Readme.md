# Azure Active Directory Connector
Azure Active Directory provides a powerful and very extensive REST API. This connector exposes just the /Applications endpoint in Microsoft Flow and PowerApps. My main intent was to provide an easy way to find client secrets that were expired or near to expiry. 

## Publisher: Paul Culmsee

## Pre-requisites

## Supported Operations
The connector supports the following operations:
### `List Applications`: Get the list of applications in this organization

### List applications
Get the list of applications in this organization.

## Obtaining Credentials 
This connector uses oAuth with Application.Read.All on Delegated AND Application scope 

## Known Issues and Limitations
Required. Include any known issues and limitations a user may encounter.

## Frequently Asked Questions
### Why have you not added the other application endpoints?
My intent here was to write flows that would look for application registrations with soon to be expiring client secrets. If you would like see other endpoints added, let me know or submit changes to this connector via guthub. 

### How to I create a client ID and secret?
Register an application in Azure AD and grant it delegated Application.Read.All access to the Microsoft Graph Service (https://docs.microsoft.com/en-us/graph/api/resources/application?view=graph-rest-1.0).

## Deployment Instructions
Create a new application registration in Azure AD and enable API access to the Microsoft Graph with Application.Read.All for Delegated permissions.
After downloading the connector, edit the client ID and tenant ID in apiProperties.json to your app registration and tenant. 
Please use paconn CLI to deploy.


