# Discord

Discord is a voice, video and text communication service used by over a hundred million people to hang out and talk with their friends and communities.

## Publisher: Daniel Laskewitz | Sogeti & Michael Guzowski | Developico

## Prerequisites

You need to have a Discord account. You can [get started](https://support.discord.com/hc/en-us/articles/360033931551-Getting-Started) for free or get a premium (Discord Nitro) account.

## Supported Operations

### Get Current User

Returns the user object of the requestors account.

### Get User Connections

Returns a list of connection objects.

### Get Current User Guilds

Returns a list of partial guild objects the current user is a member of.

### Execute Webhook

This request executes a webhook by using a webhook id and token.

## Obtaining Credentials

Go to the [Discord Developer Portal](https://discord.com/developers/applications) and login. Click the purple "New Application" button and provide a name for the app. Next, click on the purple "Create" button.

From the following page you can grab your client ID and navigate to the OAuth2 page to grab the client secret. On the OAuth2 page, you can also add the redirect URL.

## Known Issues and Limitations

No issues and limitations are known at this time.
