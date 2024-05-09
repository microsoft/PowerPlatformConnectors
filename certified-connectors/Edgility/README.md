# Edgility Connector

## Edgility Messaging Platform

Add SMS to your workflows with Edgility. Have an inbound SMS to your Virtual Number initiate a workflow, or trigger an SMS to be sent based on workflow parameters.

## Publisher: touchsSMS

[Edgility](https://edgility.com.au)

## Prerequisites

- Apply for an Edgility account [here](https://sms.edgility.com.au/application/edgility)
- Once the account is created, get your API keys from your Edgility account [here](https://app.messageport.com.au/settings/api)
- You receive free test messages, your account manager will inform you of your testing limit. To send more messages, contact your account manager to switch to a paid plan.

## Supported Operations

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

Get your API keys from your Edgility account [here](https://app.messageport.com.au/settings/api)

## Getting Started

You can test our power automate integration for free with your test credits. After the 10 test messages you will need to purchase more credit.

## Known Issues and Limitations

N/A

## Frequently Asked Questions

Read the FAQs in the Support Documentation [here](https://support.edgility.com.au/docs/faqs/)

## Deployment Instructions

Please use [these](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) instructions to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.