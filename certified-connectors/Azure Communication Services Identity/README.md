# Azure Communication Services Identity
This Identity Connector can create and delete Azure Communication Servicies user, as well as issue and revoke Azure Communication Services user access tokens, which are used for Chat and Calling capabilities in Azure Communication Services. You can read more about Azure Communication Identity [here](https://docs.microsoft.com/en-us/rest/api/communication/communicationidentity/communication-identity).

## Publisher: Microsoft

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* An Azure Communication Services resource

## Supported Operations

The connector supports the following operations:

### Create a user
Create a new user, and optionally, an access token.

### Delete a user
Delete the user, revoke all access tokens for the user and delete all associated data.

### Issue a user access token
Issue a new access token for the user.

### Revoke user access tokens
Revoke all access tokens for the user.

## Obtaining Credentials
After setting up your Azure Communication Services resource (with this [quickstart doc](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource)), obtain the connection credentials by [accessing your connection string](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp#access-your-connection-strings-and-service-endpoints).

## Getting Started
To get started setting up an Azure Communication Services resource, follow this [quickstart doc](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource).

## Known Issues and Limitations
Does not support Service Principal (Azure AD application) Authentication at this time.

## Deployment Instructions
Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --script script.csx --icon icon.png
```