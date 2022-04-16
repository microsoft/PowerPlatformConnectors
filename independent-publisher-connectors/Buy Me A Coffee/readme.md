# Buy Me A Coffee
Buy Me a Coffee makes supporting fun and easy. In just a couple of taps, your fans can make the payment (buy you a coffee) and leave a message. They don’t even have to create an account!

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
A Buy Me A Coffee account.

## Obtaining Credentials
After logging to the [developer page](https://developers.buymeacoffee.com/login), you will be given the only option to create a personal access token. The token will be used with this connector.

## Supported Operations
### Get members
This endpoint will return all of your members with both active and inactive statuses.
### Get a member
This endpoint will return membership details by passing its unique identifier.
### Get onetime-supporters
This endpoint returns all of your onetime supporters and their messages, if any.
### Get onetime-supporters by ID
This endpoint will return all of your onetime supporters and their messages by their unique ID.
### Get extras (BETA)
Get all your extra purchases data.
### Get extras by ID (BETA)
This endpoint provides data about all of your extra purchases by its unique ID. Please note ID is the same as the Purchase ID returned in the response.

## Known Issues and Limitations
There are two actions with (BETA) in their name - these are currently restricted to read-only.
