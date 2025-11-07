# Luware Nimbus
The Luware Nimbus Connector allows for integration between Nimbus and 3rd party software such as CRMs or ticketing systems. It lets you listen to task and scheduler entry events happening in Luware Nimbus. You can create tasks, enrich existing tasks with caller information data, add routing parameters or trigger an event in your 3rd party system. Additionally you can manage address books, schedule outbound calls or add external tasks.

## Publisher: Luware?

## Prerequisites
* A Microsoft PowerApps or PowerAutomate plan with "Premium" tier
* An Azure tenant
* The Power platform CLI tools
* At least one Luware Nimbus Service provisioned
* A role in Luware Nimbus allowing the usage of the Luware Nimbus connector (https://help.luware.com/power-automate-roles)


## Supported Operations

| Operation Name                | Description                                                                                  |
| ----------------------------- | -------------------------------------------------------------------------------------------- |
| **When a task changes state (*Trigger*)**   | Raised whenever a task matching the filter criteria changes its state. |
| **Update task** | Update customer information, context parameter and/or preferred user settings for a task in progress. |
| **When a scheduler entry changes state (*Trigger*)**   | Raised whenever a scheduler entry matching the filter criteria changes its state. |
| **Add a new external task**   | Add a new external task |
| **Remove an external task**   | Remove an external task |
| **Update a scheduler entry**   | Change settings for an already created scheduler entry. |
| **Remove a scheduler entry** | Remove a scheduler entry from the system. Only possible, while the scheduler entry is not yet in progress. |
| **Get all scheduler entries** | Returns all scheduler entries for the service specified. |
| **Schedule a new outbound call** | Schedule a new outbound call (scheduler entry). |
| **Add a contact to an address book** | Adds a new contact to a specific address book. The ExternalId is used as reference in further requests.  |
| **Update a contact in an address book** | Updates a single contact in the specified address book. |
| **Get contact(s) from an address book** | Returns contacts from an address book. |
| **Remove contact(s) from an address book** | Remove one or many contacts from an address book. |
| **Emtpy an address book** | Remove all contacts in the specified address book. |

## Obtaining Credentials
This connector uses OAuth via an Azure AD / Entra ID app for authentication. See below for instructions how to set up an Azure AD / Entra ID application. 

## Getting Started
Check out our use cases describing common scenarios for the Luware Nimbus connector: https://help.luware.com/power-automate-use-cases

## Known Issues and Limitations
* Losing permissions (RBAC) in Luware Nimbus will disable triggers in power automate without transparent information for the end user. Flows will just not be triggered anymore.
* Luware doesn't have influence on the execution time of flows, so results can arrive back in Nimbus with a delay. Workflows depending on the completion of the flow should be built keeping this limitation in mind.

## Deployment Instructions
Since Nimbus APIs are secured by Azure Active Directory / Microsoft Entra ID, we first need to set up a few things in Azure AD / Entra ID so that the connector can securely access Luware Nimbus.  After that is completed, you can create and test the connector.
### Set up an Azure AD / Entra ID application for your custom connector
You first need to register your connector as an application in Azure AD / Entra ID.  This will allow the connector to identify itself to Azure AD / Entra ID so that it can ask for permissions to access Luware Nimbus API on behalf of the end user.  You can read more about this here: https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios and follow the steps below:

1. Create an Azure AD / Entra ID application.
This Azure AD / Entra ID application will be used to identify the connector to Luware Nimbus.  This can be done using Azure Portal (https://portal.azure.com), by following the steps here(https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure or Update your Azure AD / Entra ID application to access the Luware Nimbus API.
This step will ensure that your application can successfully retrieve an access token to invoke Luware Nimbus on behalf of your users.  To do this, follow the steps here: https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis.
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to take note of the secret, you will need this later and it is shown only once.
    - For API permissions, make sure “User.Read” (delegated) are added.
   
At this point, we now have a valid Azure AD / Entra ID application that can be used to get permissions from end users and access Luware Nimbus.  The next step for us is to create a custom connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret> --icon icon.png
```

For reference, head to https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli