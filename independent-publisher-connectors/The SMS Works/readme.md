# The SMS Works
The SMS Works is a low cost SMS service for developers based in the UK. You are only billed for delivered text messages.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
Use of this connectors required an account with The SMS Works and available credits.

## Obtaining Credentials
In your account, go to the API Key section. You will then generate an API key and secret, after which a JWT token will be then generated. You will use the JWT token to authenticate this connector.

## Supported Operations
### Send an SMS message
Sends an SMS message based on your parameters.
### Send flash message
Sends an SMS flash message, which appears on the recipients lock screen.
### Schedule message
Schedules an SMS message to be sent at the date-time you specify.
### Search messages
Retrieve up to 1000 messages matching your search criteria.
### Get unread messages
Get unread incoming messages matching your search criteria.
### Get failed messages
Retrieve failed messages matching your search criteria.
### Get scheduled messages
Returns a list of messages scheduled from your account, comprising any messages scheduled in the last 3 months and any scheduled to send in the future.
### Get message
Retrieve a logged message by the message ID.
### Delete message
Delete the message with the matching message ID.
### Cancel scheduled message
Cancels a scheduled SMS message.
### Send batch
Send a single SMS message to multiple recipients.
### Send collection of messages
Sends a collection of unique SMS messages. Batches may contain up to 5000 messages at a time.
### Retrieve batch messages
Retrieve all messages in a batch with the given batch ID.
### Schedule batch
Schedules a batch of SMS messages to be sent at the date time you specify.
### Cancel scheduled batch
Cancels a scheduled SMS message.
### Get credits
Returns the number of credits currently available on the account.
### Get customer ID
Returns the customer ID to the caller.

## Known Issues and Limitations
There are no known issues at this time.
