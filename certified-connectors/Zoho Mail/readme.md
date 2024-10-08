# Zoho Mail

Zoho Mail connector gives you the power to manage your emails and perform actions such as saving them as drafts, receiving them, and triggering flows when new emails arrive.

## Publisher: Zoho Corporation

## Prerequisites

A verified Zoho Mail account. Click [here](https://zoho.com/mail) to create your first account.

## Supported Operations

The connector supports the following operations:

### Send an email

Send a new email from a specified email address

### Save draft or template 

Save the email content either as a draft or as a template

### Get email details

This endpoint gets the email details based on Account ID and message ID such as Message ID, Summary, Email content, Sent on, Sender name, and From. 
The operation takes the message ID and Account ID as input. 
Sample prompt "get the email details of message ID 1234 for the account 4567"

### Get account details

This endpoint gets the account details of the currently authenticated user such as their Account ID and Account display name. Sample prompt as "Get my mail account details" (The plugin would respond with the retrieved Account ID and Account display name).

### Get folders

This endpoint gets details of all folders for the specified user account such as Folder ID and Folder name based on the Account ID.
The user enters Account ID as the input. 
Sample prompt "List all folders for account 1234”, and the operation retrieves all the folders associated with the account 1234.

### Get labels

This endpoint gets the details of all labels for the specified user account such as Label ID and Label name based on the Account ID. 
The user enters Account ID as the input. 
prompt "Show me all the labels for account 1234”, and the operation retrieves all the labels associated with the account 1234.

### Search emails

This endpoint lists emails for the specified Account ID based on search parameters that display information like the Message ID, Summary, Sent on, Sender name, and From. The user enters keywords by Subject, Email content or sender as the search parameter for the specified account.

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


