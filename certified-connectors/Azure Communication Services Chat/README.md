# Azure Communication Services Chat Connector

This Chat Connector can be used to add real-time text communication to your cross-platform applications. You can read more about Azure Communication Services Chat [here](https://docs.microsoft.com/en-us/rest/api/communication/chat/chat-thread).

## Publisher: Microsoft

## Prerequisites

You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* An Azure Communication Services resource

### Set up an Azure Communication Services resource
- [Quickstart doc](https://review.docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?branch=main&tabs=windows&pivots=platform-azp)


## Obtaining Credentials

### Connection String Authentication
You can create a new connection using an [Azure Communication Services resource connection string](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp#access-your-connection-strings-and-service-endpoints).

## Deployment Instructions

Run the following commands and follow the prompts:

```paconn
paconn create --api-def .\apiDefinition.swagger.json --api-prop .\apiProperties.json --script .\script.csx --icon .\icon.png
```

## Supported Operations

### Send Chat
Send a chat to a sepcified chat thread given an access token.

### Create Chat
Create a chat given a chat topic. 

### List Chat Threads
List all chat threads.

### List Thread Messages
List Messages From Thread.

### Get Thread Properties
Get the properties of a chat thread.

### Add Participant
Add a participant to a chat thread

### Remove Participant
Remove a participant from a chat thread

### List Participants
List participants in a chat thread