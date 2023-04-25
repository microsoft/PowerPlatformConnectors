# ConvertKit
ConvertKit is the go-to marketing hub for creators that helps you grow and monetize your audience with ease. Convert your followers from social media, YouTube, and more to your email list with a beautiful landing page and opt-in templates.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
A free or paid plan with ConvertKit.

## Obtaining Credentials
Under your profile settings Advanced, you will find your API secret key. As the API secret can be used in place of the API key (which only some of the actions use the key), the connector uses the secret for all actions.

## Supported Operations
### Get account
Retrieves the current account.
### Get forms
Retrieve a list of all the forms for your account.
### Add subscriber to a form
Subscribe an email address to one of your forms.
### List subscriptions to a form
Retrieves a list of subscriptions to a form including subscriber data.
### List sequences
Retrieves a list of sequences for the account.
### Add subscriber to a sequence
Subscribe an email address to one of your sequences.
### List subscriptions to a sequence
Retrieves subscriptions to a sequence including subscriber data.
### List tags
Retrieves a list of tags for the account.
### Create a tag
Creates a new tag.
### Tag a subscriber
Tags are handled as subscriptions. Subscribe an email address to a tag to have that tag applied to the subscriber with that email address.
### Remove tag from subscriber
Removes a tag from a subscriber.
### Remove tag from subscriber by email
Remove a tag from a subscriber by email address.
### List subscriptions to a tag
Retrieves a list of subscriptions to a tag including subscriber data.
### List subscribers
Returns a list of your subscribers. For unsubscribes only, use the cancelled at value for sort field param (currently the only supported extra sort field).
### Get a subscriber
Retrieves a single subscriber record.
### Update subscriber
Updates the information for a single subscriber.
### Unsubscribe subscriber
Unsubscribe a subscriber from all your forms and sequences.
### List subscriber's tags
Retrieves a list of all the tags for a subscriber.
### List broadcasts
Retrieves a list of all the broadcasts for your account.
### Create a broadcast
Create a draft or scheduled broadcast. You can create a draft broadcast without any attributes. Scheduled broadcasts at a minimum should contain a subject line and your content.
### Retrieve a broadcast
Retrieves the details of a specific broadcast, including draft, scheduled, and previously-sent broadcasts.
### Update a broadcast
Updates the attributes of a specific broadcast. Broadcasts that are currently sending or that have been sent may not be updated.
### Destroy a broadcast
Permanently delete a draft or scheduled broadcast record. Broadcasts that are currently sending or that have been sent may not be deleted.
### Get broadcast stats
Retrieves the statistics (recipient count, open rate, click rate, unsubscribe count, total clicks, status, and send progress) for a specific broadcast.
### List purchases
Retrieve a list of all purchases for an account.
### Create a purchase
Create a purchase.
### Get a purchase
Retrieve a specific purchase by ID.

## Known Issues and Limitations
There are no known issues at this time.
