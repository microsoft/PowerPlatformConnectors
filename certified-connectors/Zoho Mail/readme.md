# Zoho Mail

This connector allows you to send emails from Zoho Mail.

## Publisher: Zoho Corporation

## Prerequisites

A verified Zoho Mail account. Click [here](https://zoho.com/zohomail) to create your first account.


## How to get credentials

Zoho Mail API uses OAuth2 to authenticate and hence no secondary authentication is required once the account has been setup.

## Supported Operations

The connector supports the following operations:

### Send an email

Send a new email from a specified email address

### Save draft or template 

Save the email content either as a draft or as a template

### Get email content

Get the email content based on message ID

### Search Mail

List emails based on search parameters

## Supported Triggers

The connector supports the following operations:

### New Mail Notification

New mail notification based on criteria

### Known Issues and Limitations

* The total email size, including the email headers, body content, inline images, and attachments should not exceed the plan limit.
* Zoho Mail cannot be used for sending out bulk emails and other emails that violates Zoho Mail usage policy. Click [here](https://www.zoho.com/mail/help/usage-policy.html)

## FAQ

https://help.zoho.com/portal/en/kb/mail

##Deployment Instructions
Refer the documentation [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.


