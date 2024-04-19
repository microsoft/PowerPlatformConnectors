# Zoho Mail

Zoho Mail connector gives you the power to manage your emails and perform actions such as saving them as drafts, receiving them, and triggering flows when new emails arrive.

## Publisher: Zoho Corporation

## Prerequisites

A verified Zoho Mail account. Click [here](https://zoho.com/zohomail) to create your first account.

## Supported Operations

The connector supports the following operations:

### Send an email

Send a new email from a specified email address

### Save draft or template 

Save the email content either as a draft or as a template

### Get email details

Get the email content based on message ID

### Get account details

Get account details of the currently authenticated user such as their Account ID and Account display name.

### Get folders

Get details of all folders for the specified user account such as Folder ID and Folder name.

### Get labels

Get details of all labels for the specified user account such as Label ID and Label name.

### Search emails

List emails based on search parameters that displays information like the Message ID, Summary, Sent on, Sender name, and From.

## Supported Triggers

The connector supports the following operations:

### New Mail Notification

Receive a new email notification from sender.

## Obtaining Credentials

Zoho Mail API uses OAuth2.0 to authenticate and hence no secondary authentication is required once the account has been set up.

## Known Issues and Limitations

* The total email size, including the email headers, body content, inline images, and attachments should not exceed the plan limit.
* Zoho Mail cannot be used for sending out bulk emails and other emails that violates Zoho Mail usage policy. Click [here](https://www.zoho.com/mail/help/usage-policy.html) for more details.

## Frequently Asked Questions

https://help.zoho.com/portal/en/kb/mail

## Deployment Instructions
Refer the documentation [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.


