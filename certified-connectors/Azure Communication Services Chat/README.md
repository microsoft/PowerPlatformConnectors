# Azure Communication Services Chat
This Chat Connector can manage users in a chat, create chats threads, and send messages in a chat. [API ref for chat](https://docs.microsoft.com/en-us/rest/api/communication/chat/chat),  [API ref for chat thread](https://docs.microsoft.com/en-us/rest/api/communication/chat/chat-thread). 

## Publisher: Microsoft

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* An Azure Communication Services resource

## Supported Operations
The connector supports the following operations:

### Send Chat
Send a chat to a specified chat thread given an access token.

### Create Chat
Create a chat given a chat topic. 

### List Chat Threads
List all chat threads.

### List Thread Messages
List Messages From Thread.

### Get Thread Properties
Get the properties of a chat thread.

### Add Participant
Add a participant to a chat thread.

### Remove Participant
Remove a participant from a chat thread.

### List Participants
List participants in a chat thread.

## Obtaining Credentials

1 Set up an Azure Communication Services resource
- [[Quickstart doc](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp)

2 Connection String Authentication
- You can create a new connection using an [Azure Communication Services resource endpoint url](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp#access-your-connection-strings-and-service-endpoints).

## Known Issues and Limitations
Does not support Service principal (Azure AD application) Authentication at this time.

## Deployment Instructions
Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --script script.csx --icon icon.png
```