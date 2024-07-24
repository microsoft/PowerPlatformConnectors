# Telstra SMS Connector

## Telstra Messaging API

Telstra Messaging API leverages Australia’s leading mobile network to support reliable, automated and feature-rich SMS and MMS interactions with global reach.

## Publisher: Telstra

[Telstra Dev](https://dev.telstra.com/)

## Prerequisites

- Send your first message by creating an account [here](https://dev.telstra.com/tdev/user/register)
- Once the account is created, [get a virtual number](https://dev.telstra.com/apis/messaging-api/tutorials/get-a-virtual-number). This number is dedicated to you and comes with free of cost.
- You can also [send SMS from a alpha numeric sender name](https://dev.telstra.com/apis/messaging-api/tutorials/send-an-sms-from-a-sendername) (Available only for paid plans)

## Supported Operations

Below actions are supported now.

### Send Message

Send SMS supports below parameters.

`Required fields`

- From - the virtual number provisioned to you or an approved sender name
- To - the recipient number
- Message Content - the message content

Check this [documentation](https://dev.telstra.com/apis/messaging-api/endpoints#tag/messages/operation/sendMessages) for other optional fields

### When you receive a Message

This is a trigger. When you receive a message on your virtual number this trigger will be invoked.

`Required fields`

- Virtual-number - Virtual number assigned to your account

## Obtaining Credentials

Once you register with us, you will be given the needed credentials to use this connector.

## Getting Started

Try out Messaging API v3 with our no-commitment Free Trial. Send up to 100 messages to up to 10 recipients with no time limit, no obligation and no billing information required.
Required. Include any known issues and limitations a user may encounter.

## Known Issues and Limitations

If you are already using a callback url for your virtual number, creating this trigger will overwrite your existing callback URL.

## Frequently Asked Questions

Read the FAQs [here](https://dev.telstra.com/apis/messaging-api/faqs)

## Deployment Instructions

Please use [these](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) instructions to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
