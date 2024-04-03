# Mailinator
The Mailinator service provides programmatic access to the Mailinator system. This includes fetching and injecting messages into the Mailinator system and creating routing rules for specific message domains within the system.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must sign up for an account with [Mailinator](https://www.mailinator.com/v4/public/inboxes.jsp?trialshow=true).

## Obtaining Credentials
Once logged in to your Inbox, go to the Team Settings page to find your API Token.

## Supported Operations
### Fetch inbox
This endpoint retrieves a list of messages summaries. You can retrieve a list by inbox, inboxes, or entire domain.
### Get message
This endpoint retrieves a specific message by identifier.
### Delete message
This endpoint deletes a specific message.
### Get message attachments
This endpoint retrieves a list of attachments for a message. Note attachments are expected to be in Email format.
### Get message links
This endpoint retrieves all links found within a given email.
### Inject message
This endpoint allows you to deliver a JSON message into your private domain. This is similar to simply emailing a message to your private domain, except that you use HTTP Post and can programmatically inject the message.
### Get stats
This endpoint retrieves usage information for your team.
### Get all domains
Retrieve a list of your domains.
### Get domain
Retrieve a specific domain.

## Known Issues and Limitations
There are no known issues at this time.
