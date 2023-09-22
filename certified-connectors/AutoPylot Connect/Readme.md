# AutoPylot Connect

The AutoPylot API allows you to send and receive text messages from your application using AutoPylot phone numbers.

## Getting Started

To use this connector, you will need:

- An AutoPylot account - Sign up at [https://autopylot.com](https://autopylot.com)  
- Your AutoPylot User authentication credentials

## Authentication 

The AutoPylot API uses OAuth 2.0 authentication. Use your AutoPylot User authentication credentials to authenticate your requests from our connector.

## Supported Operations

The AutoPylot connector supports the following operations:

### Send Text Message

Sends a text message from your AutoPylot phone number to a recipient. 

**Parameters:**

- `to` - Recipient's phone number
- `body` - Message text  

**Returns:** A JSON response with details of the sent message.

### Receive Text Message (Trigger)

Triggers when an inbound text is received on any of your AutoPylot phone numbers. 

**Returns:** A JSON payload containing details of the received text message.

## Additional Information

- [AutoPylot portal](https://portal.autopylot.com)