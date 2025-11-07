# touchSMS Connector

## touchSMS Messaging Platform

Add SMS to your workflows with touchSMS. Have an inbound SMS to your Virtual Number initiate a workflow, or trigger an SMS to be sent based on workflow parameters.

## Publisher: touchsSMS

[touchSMS](https://touchsms.com.au)

## Prerequisites

- Create a touchsms account [here](https://app.touchsms.com.au/register)
- Once the account is created, get your API keys from your touchSMS account [here](https://app.touchsms.com.au/settings/api)
- You receive 10 free test messages. To send more messages, make a purchase.

## Supported Operations

Below actions are supported now.

### Send Message

Send SMS supports below parameters.

`Required fields`
- To - the recipient number, ideally in E164 format.
- From - select an approved sender (number or alpha sender)
- Content - the message content

`Optional fields`
- Campaign Name - shown in reporting.
- Reporting Reference - shown in detailed reports and exports.
- Schedule Date - UTC ISO8061 schedule date for message

### Inbound Message

This is a trigger. When you receive a message on your virtual number this trigger will be invoked.

`Required fields`

- Virtual Number - Virtual number assigned to your account

## Obtaining Credentials

Get your API keys from your touchSMS account [here](https://app.touchsms.com.au/settings/api)

## Getting Started

You can test our power automate integration for free with your test credits. After the 10 test messages you will need to purchase more credit.

## Known Issues and Limitations

N/A

## Frequently Asked Questions

Read the FAQs [here](https://support.touchsms.com.au/frequently-asked-questions/)

## Deployment Instructions

Please use [these](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) instructions to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.