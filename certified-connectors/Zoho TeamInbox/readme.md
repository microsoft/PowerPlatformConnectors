
# Zoho TeamInbox

Zoho TeamInbox connector allows you to perform actions such as creating and saving contacts receiving them, and searching emails from your shared inboxes based on email ID, sender address, email subject or content.

## Publisher: Zoho Corporation

## Prerequisites

A verified Zoho TeamInbox account. Click [here] (https://www.zoho.com/teaminbox/signup.html) to create your first account.

## Supported Operations

The connector supports the following operations:

### Create contact

Create a new contact under the team, with the name, email address and phone number.

### Send Mail

Compose and send a new email, to one or more recipients.

### Search Mail

Search emails in an inbox filtered by subject or keyword

### Archive a thread

Move a conversation or email thread into the archive folder for record-keeping.

### Assign a thread

Assign ownership of a conversation or email thread to a specific team member.

### Trash a thread

Move a conversation or email thread to the trash, marking it for deletion.

### Follow a Thread

Follow to get all notification of the thread.

## Obtaining Credentials

Zoho TeamInbox uses OAuth2.0 to authenticate and hence no secondary authentication is required once the account has been set up.

## Known Issues and Limitations

* The total email size, including the email headers, body content, inline images, and attachments should not exceed the plan limit.
* Zoho TeamInbox cannot be used for sending out bulk emails and other emails that violates Zoho TeamInbox usage policy.

## Frequently Asked Questions

https://www.zoho.com/teaminbox/help/getting-started.html

## Deployment Instructions
Refer the documentation [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.




