
## YakChat
The YakChat connector allows you to send and receive SMS and MMS using your YakChat numbers. YakChat adds SMS and MMS to Microsoft Teams so that you can connect instantly with your customers and co-workers using text messaging.

## Publisher: YakChat Ltd.

## Prerequisites
You will need the following to use the YakChat connector:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A YakChat subscription

## Obtaining Credentials
The YakChat connector uses OAuth Authentication through the Microsoft account linked to a YakChat account and subscription. Please use [this link] (https://my.yakchat.com/#/) or install the YakChat application in Teams to register with YakChat.

## Supported Operations
The YakChat connector supports the following operations:
* `Send Message` Action: Send an SMS message using through YakChat in a flow
* `When a Message is Received` Trigger: Trigger a flow when a message is received by YakChat
* `When a Message is Sent` Trigger: Trigger a flow when a message is sent by YakChat
* `When a Message is Sent or Received` Trigger: Trigger a flow when a message is sent or receieved by YakChat

## Known Issues and Limitations
* The YakChat connector currently does not support sending MMS using the `Send Message` Action.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps