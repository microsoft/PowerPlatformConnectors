## Tyntec Conversations API - Send Viber

Tyntec Conversations API allows you to send customized Viber messages.

Reach over 1 billion Viber users and interact with them to acquire new customers, expand your customer engagement, and provide customer care right on the messaging app. Use the connector to provide instant support for customer queries, notify delivery updates, send promotional offers with images and interactive buttons.

### Use Cases of this connector
- Use this connector in PowerApps Workflows (great for Marketing, CRM or Sales)
- Send SMS messages using Azure Logic Apps
- Build SMS support in your Microsoft Power Automate automatizations

And many more!


## Pre-requisites
You will need the following to proceed:
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- [tyntec API Key](http://my.tyntec.com/api-settings)
- [Viber Service ID](https://www.tyntec.com/viber-business-messages#contact)

## Supported requests
- **Send Viber** using tyntec Conversations API [reference](https://api.tyntec.com/reference/#conversations-send-messages-send-a-message)
 - To make a successful request, please, populate the followings fields:
 - **to** - receiver's phone number in _international_ form without leading 00 (_E.g. 4989202451100_)
 - **from** - your Viber Service ID
 - **text** - text of your Viber message
- **Status Check**
 - To make a successful status check, please, provide the connector with the following values

 - **id** - messageID of your message (_returned after each request_)

## Supported Triggers
- **Incoming Message** Trigger any flow on new incoming messages
    - **Response Fields**
        - **messageId** (the message ID)
        - **channel** - the channel on which the message was received (viber, whatsapp or sms)
        - **from** - the sender's phone number
        - **to** - the reciever's phone number (you)
        - **receivedAt** - when was the message received
        - **text** - the text of the message
        - **file** - any attached file to the message
        - **event** - on which event the webhook was triggered
        - **timestamp** - the timestamp of the message

## How to get API key 
Please [sign up for a free account](https://www.tyntec.com/create-account). In your account, select the [API settings](http://my.tyntec.com/api-settings) and copy your API key.

## How to get Viber Service ID
Please, fill our [form](https://www.tyntec.com/viber-business-messages#contact). Tyntec will guide you through the whole process.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

