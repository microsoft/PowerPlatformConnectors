# HubSpot Conversations V2
The HubSpot conversations services enable you to manage and interact with the conversations inbox, channels, and messages.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have an account with [HubSpot](https://app.hubspot.com/signup-hubspot/crm) and be a Super Admin.

## Obtaining Credentials
Once you are logged in to your account, go to Settings -> Account Setup -> Integrations -> Private Apps. You will need to create a private app and assign it only the scopes you will use actions for. Once your private app is created, you will have an access token in the Auth section of the private app.

## Supported Operations
### Get conversations inboxes
Retrieve a list of conversations inboxes.
### Get a single thread
Retrieves a single thread.
### Archives a thread
Archives a single thread. The thread will be permanently deleted 30 days after placed in an archived state.
### Update a thread
Updates a single thread. Either a thread's status can be updated, or the thread can be restored.
### Get the original content of a single message
Returns the complete original text and rich text bodies of a message. This will be different from the text and rich text in the message itself if the message's truncation status is anything other than NOT_TRUNCATED.
### Get message history for a thread
Retrieve a list of message history for a thread.
### Send a message to a thread
Send a new message on a thread at the current timestamp.
### Get actors
Resolve actors identifiers to the underlying actors and participants.
### Get channel accounts
Retrieve a list of channel accounts.
### Get a single channel
Retrieve a single channel.
### Get a single message
Retrieve a single message.
### Get channels
Retrieve a list of channels.
### Get a single actor
Retrieve a single actor.
### Get threads
Retrieve a list of threads.
### Get a single channel account
Retrieve a single channel account.
### Get a single conversations inbox
Retrieve a single conversations inbox.
### Generate a token
Generates a new visitor identification token. This token will be unique every time this endpoint is called, even if called with the same email address. This token is temporary and will expire after 12 hours.

## Known Issues and Limitations
There are no known issues at this time.
