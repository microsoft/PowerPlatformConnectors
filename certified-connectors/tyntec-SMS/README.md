## Tyntec Conversations API - Send SMS

Tyntec Conversations API allows you to send customized SMS messages.

Use the most universal texting channel SMS when it’s critical that any user must get the message, no matter which chat app they’re using. For use cases such as sending a one-time-passcode or two-factor authentication (2FA), the SMS channel is the most-widely used method for phone-based authentication, thanks to its global coverage and high open rates.

### Use Cases of this connector
-   Use this connector in PowerApps Workflows (great for Marketing, CRM or Sales)
-   Send SMS messages using Azure Logic Apps
-   Build SMS support in your Microsoft Power Automate automatizations

And many more!


## Pre-requisites
You will need the following to proceed:
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- [tyntec API Key](http://my.tyntec.com/api-settings)
- _If you are using a free account, be sure to whitelist your phone number in [SMS's first steps](https://my.tyntec.com/products/sms#first-steps)._

## Supported requests
-   **Send_SMS** using tyntec Conversations API [reference](https://api.tyntec.com/reference/#conversations-send-messages-send-a-message)
    -   To make a successful request, please, populate the followings fields:

        -   **to** - receiver's phone number in _international_ form without leading 00 (_E.g. 4989202451100_)
        -   **from** - sender's phone number
        -   **text** - text of your SMS
- **Status_Check**
    -   To make a successful status check, please, provide the connector with the following values

        -   **id** - messageID of your message (_returned after each Send_SMS request_)

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

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
