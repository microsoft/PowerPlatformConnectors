# Azure Active Directory Connector
Azure Active Directory provides a powerful and very extensive REST API. This connector exposes just the /Applications endpoint in Microsoft Flow and PowerApps. My main intent was to provide an easy way to find client secrets that were expired or near to expiry. 

## Publisher: Paul Culmsee

## Pre-requisites

## Supported Operations
The connector supports the following operations:

### List applications
Get the list of applications in this organization.

## Obtaining Credentials 
### Create an app registration

The Microsoft Graph API uses OAuth 2.0 authentication. For that, we need to create a service principal (app registration) in our Azure Active Directory tenant. To create the app registration, head over the Azure Active Directory admin portal, and create a new app registration.

![Azure App Registration](https://powerusers.microsoft.com/t5/image/serverpage/image-id/165899i200E70914878DEF1/image-size/large?v=v2&px=999)

Give it a name that you will easily recognize later, and click Register. Next, we are going to add the API permissions. This connector uses oAuth with Application.Read.All on Delegated scope, so add the Graph API permissions Application.Read.All. In this case, pick the delegated permissions.

![API Permission Request](https://github.com/paulculmsee/PowerPlatformConnectors/blob/azureadapplications/independent-publisher-connectors/AzureADApplications/APIPermissions.jpg?raw=true)

Important - Don't forget to grant admin consent on behalf of the organization.

### Create a client secret
From the Certificates & secrets menu, create a Client secret. Give it the life-time that you prefer, and copy this to notepad. You need to do this right away because the secret will be masked after a while.

![Obtaining client secret](https://powerusers.microsoft.com/t5/image/serverpage/image-id/165904iA0808E952E09A3A3/image-size/large?v=v2&px=999)

There is more information that we have to collect from the app registration to set up the connector. Copy and paste the Directory and Client ID in the same notepad for later use. 

![Other application registration info](https://powerusers.microsoft.com/t5/image/serverpage/image-id/165905i8DB6779D9F7D74F9/image-size/large?v=v2&px=999)

## Known Issues and Limitations
Required. Include any known issues and limitations a user may encounter.

## Frequently Asked Questions
### Why have you not added the other application endpoints?
My intent here was to write flows that would look for application registrations with soon to be expiring client secrets. If you would like see other endpoints added, let me know or submit changes to this connector via guthub. 

## Deployment Instructions
Create a new application registration in Azure AD and enable API access to the Microsoft Graph with Application.Read.All for Delegated permissions.
After downloading the connector, edit the client ID and tenant ID in apiProperties.json to your app registration and tenant. 
Please use paconn CLI to deploy.


