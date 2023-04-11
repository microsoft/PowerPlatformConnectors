# BulkSMS
SMS enable your workflows and automations with the Power Automate Connector for BulkSMS.

## Publisher: BulkSMS.com
BulkSMS.com ​

## Prerequisites
You will need an active BulkSMS.com account to use this connector. Sign-up for a free account at https://www.bulksms.com. New accounts will receive 5 free SMS text credits. Additional credits can be purchased on a Pay-As-you-Go basis. Credits don't expire. No long term contracts or commitments. High volume pricing with post-pay options are also available. Please contact sales@bulksms.com for additional information.

## Supported Operations

### SendSMSMessage
Send an SMS Message to a mobile number


## Obtaining Credentials
We recommend that you use an **API Token**, but you can also use your BulkSMS account `username` and `password` directly. To generate an API Token, sign into https://www.bulksms.com, then navigate to **Settings / Advanced /  Manage API Tokens / Create Token**

## Getting Started
- Create a new Automation 
- Select your trigger (for example, an **Instant cloud flow**)
- Select the BulkSMS connector, name it and enter your Credentials

### Fields
| Name | Description |
| ------ | ------ |
| Auto-Unicode (required) | When `Yes` is selected, then messages are automatically sent as Unicode if required. Unicode messages allow 70 characters per message part. Standard messages allow 160 characters per message part. If `No` is selected, then the message is sent as a standard 7-bit message. Unicode characters are substituted by the mobile network. This may result in `?` substitutions.  |
| to | The destination mobile / cell number. Numbers must begin with a country code (eg. 1 for US. 44 for UK) and must be numeric with no spaces.  Valid example : `1234567890`, Invalid example: `(0123)456-7890`.|
| body (required) | The message content. For example `Hello World`. Standard messages allow 160 characters per message part. Unicode messages allow 70 characters per message part. If message length is exceeded, then messages will automatically be concatenated to a maximum of `longMessageMaxParts` message parts. |
| longMessageMaxParts | See `body` above for additional information. Each message part costs the same as one SMS. Concatenation reduces each message part by 7 characters. For example, a message of 161 characters will use 2 message parts and allow a total of 153 + 153 characters (thus 306 characters) |
| from | This is the `Sender ID` field. When left blank, the message will be sent from a reply pool of numbers or default sender ID, depending on region and account settings. If the message is from a reply number, then reply messages are routed back to the Inbox of your BulkSMS account. Additional Webhooks and other triggers can be configured from your BulkSMS account (see **Settings / Message / Replies** and **Settings / Advanced / Webhooks**. You may specify a custom sender ID in this field, but you must first have this sender ID registered and added to your BulkSMS account. See **Settings / Message / Sender IDs**. See the **Notes on Sender ID** lower in this document for additional information.|


## Known Issues and Limitations
This connector offers full featured outbound SMS messaging. You can only send to one number at a time. We don't yet relay reply messages and other events through this connector. You can however, from your BulkSMS account, configure Webhooks for mobile originated / reply messages and delivery / status reports. Navigate to **Settings / Advanced / Webhooks**

## Notes on Sender ID
Custom Sender ID's need to be preloaded on your BulkSMS account. Sender ID rules :
- can be numeric or alpha-numeric (eg 15554321 or BobsPizza), 
- must not contain any special characters or punctuation (eg #, $, %)
- must not exceed 11 characters in length

To request one or more custom sender ID's, please sign into your BulkSMS account and navigate to **Settings / Message / Sender IDs**

**Note ! Messages sent with a Custom sender ID are not repliable.** 

To send repliable messages, ensure your account has the `Default Repliable` setting enabled.



## Frequently Asked Questions

### Can I use the same account for multiple products.
Yes. You can use the same BulkSMS account for Power Automate and any of our other SMS products and integrations.
### Do SMS credits expire
No. Credits do not expire.
### Do you offer an API for deeper integration
Yes. Just head over to our [developer portal](https://www.bulksms.com/developer/)
### Can you help us if we get stuck or have Questions
Absolutely. Just email support@bulksms.com and make sure you mention the Microsoft Power Automate Connector when you describe your problem.





