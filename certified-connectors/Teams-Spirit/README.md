
## Teams-Spirit Connector
Teams-Spirit provides an extensive REST API.  Using this API, you can manage your approvals, teams, and users via Teams-Spirit.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Teams-Spirit Azure AD Enterprise Application (usually this will be present if you are using Teams-Spirit)
* A Teams-Spirit Subscription
* The Power platform CLI tools

## Building the connector 
Since the Teams-Spirit API is secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD so that our connectors can securely access the API.  After that is completed, you can create and test the connector.

### Set up an Azure AD application for your custom connector
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access Teams-Spirit data on behalf of the end user. You can follow the steps below:

1. Create an Azure AD application
This Azure AD application will be used to identify the connector to Teams-Spirit.  This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access the Teams-Spirit API
This step will ensure that your application can successfully retrieve an access token to invoke Teams-Spirit on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, add the Teams-Spirit ``access_as_user`` permission. To do that click ``Add a permission``, navigate to ``APIs my organization uses``, search for Teams-Spirit and add the ``access_as_user`` permission.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Teams-Spirit.

### Deploying the sample

First replace ``{client_id}`` in the ``apiProperties.json`` with your Azure AD applications Client ID. 

Then run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector provides the following triggers:
* Action-Trigger: Can be Triggered via a Teams-Spirit Action
The connector supports the following operations:
* Get All Approvals: lists all approvals
* Approves: approves an approval
* Reject: reject an approval
* Archive Team: archives a team and optionally marks the groups SharePoint site as read-only
* Delete Team: deletes a team
* Change Role To Member: changes a users role to member
* Change Role To Owner: changes a users role to owner
* Remove All Guests: removes all guests from a team
* Remove All Users Except Owner: removes all team members except for a given owner
* Remove User From Team: removes a user from a team
* Change Tag Value: changes the value of a Teams-Spirit Tag
* Get Tag Value: returns the value of a Teams-Spirit Tag
* Extend Expiration Date: extends a teams expiration date