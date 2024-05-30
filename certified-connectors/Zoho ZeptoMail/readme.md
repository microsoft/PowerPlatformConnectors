# Zoho ZeptoMail

This connector allows you to send emails from ZeptoMail. You can send custom emails or use existing email templates in your ZeptoMail account.

## Publisher: Zoho Corporation

## Prerequisites

A verified ZeptoMail account. Click [here](https://zoho.com/zeptomail)to create your first account.


## How to get credentials

ZeptoMail API uses OAuth2 to authenticate and hence no secondary authentication is required once the account has been setup.

## Supported Operations

The connector supports the following operations:

### Send email

Send emails customized according to your needs.

### Send template email 

Send emails using pre-built templates available in your ZeptoMail account. 

### Get all Mail Agent

This endpoint fetches details of all the Mail Agents in the account. It returns data including Mail Agent name, Mail Agent key, Created time, Status.  \n\nData Fields Explained:\n- mailagent_name: Name of the Mail Agent.\n- mailagent_key: Mail Agent Key.\n- mailagent_alias: Mail Agent Alias.\n- created_time: Time at which the Mail Agent was created.\n- status: Status of the Mail Agent.\n\nExample 1: To retrieve details of all Mail Agents in the ZeptoMail account. Sample prompt 1: Get me details of all the Mail Agents in the account.

### Get processed email details

This endpoint fetches detailed information about emails processed by Mail Agent identified by its Mail Agent key. It returns data including the Subject, From Address, Sent time and Request ID for emails sent for the time range. The mailagent_key parameter should match the  Mail Agent key of the Mail Agent as listed in our system.

### Processed emails analytics

This endpoint fetches emails analytics for emails sent by Mail Agent identified by its Mail Agent name. It returns data including total count of Hardbounce, Softbounce and Sent statistics over a period of time and each day . The mailagent parameter should match the Mail Agent name as listed in our system.

### Known Issues and Limitations

The total email size, including the email headers, body content, inline images, and attachments should not exceed 15 MB.

## FAQ

https://help.zoho.com/portal/en/kb/zeptomail/faqs

##Deployment Instructions
Refer the documentation [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.


