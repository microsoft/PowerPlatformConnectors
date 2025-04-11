# SmartDialog Connector

This connector can be used to send and recieve SMS messages using the SmartDialog platform.  

## Prerequisites

1. A SmartDialog subscription and account. Please be in contact with sales@arenainteractive.fi 
2. OAuth needs to be enabled for your account and a client secret needs to be requested. This can be done by sending us an email at support@arenainteractive.fi.

## Setup

1. Clone the PowerPlatform Connectors GitHub repository.
2. Open a terminal session and change to the SmartDialog directory.
3. If nescessary download the [paconn tool](https://docs.microsoft.com/connectors/custom-connectors/paconn-cli).
4. Run `paconn login` and follow authentication steps
5. Run `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret *client secret*`
6. Select your target environment.
7. Create a Flow or PowerApp and add a SmartDialog action from the custom section. Sign in with your SmartDialog account.

## Supported Actions

- New Message (Trigger for recieving messages)
- Send Message
- Send WhatsApp Message
- Send Discussion Reply Message
- Send Reply Message
- WhatsApp Opt In
- WhatsApp Opt Out
- Create WhatsApp Template
- Get Group Contact
- Create Group Contact
- Update Group Contact
- Delete Group Contact
- Delete All Group Contacts
