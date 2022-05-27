## Azure Communication Services Identity Connector

This Identity Connector can create and delete Azure Communication identities, which can be used to utilize the Chat and Calling capabilities in Azure Communication Services. You can read more about Azure Communication Identity [here](https://docs.microsoft.com/en-us/rest/api/communication/communicationidentity/communication-identity).

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* An Azure Communication Services resource

### Set up an Azure Communication Services resource
- [Quickstart doc](https://review.docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?branch=pr-en-us-192537&tabs=windows&pivots=platform-azp)

## Connection Setup
### Connection String Authentication
You can create a new connection using an [Azure Communication Services resource connection string](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp#access-your-connection-strings-and-service-endpoints).

## Supported Operations
The connector supports the following operations:
* `Create Communication Identity`: Create a new Azure Communication identity.
* `Delete Communication Identity`: Delete the Azure Communication identity, revoke all tokens for the identity and delete all associated data.
* `Issue Identity Access Token`: Issue a new access token for the Azure Communication identity.
* `Revoke Identity Access Tokens`: Revoke all access tokens for the specific identity.