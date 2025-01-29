# HubSpot Settings V2
HubSpot Settings allows you to retrieve information about a given HubSpot account, including the account settings, account activity, business units, and managing users.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have an account with [HubSpot](https://app.hubspot.com/signup-hubspot/crm) and be a Super Admin.

## Obtaining Credentials
Once you are logged in to your account, go to Settings -> Account Setup -> Integrations -> Private Apps. You will need to create a private app and assign it only the scopes you will use actions for. Once your private app is created, you will have an access token in the Auth section of the private app.

## Supported Operations
### Get login activity
Get login activity for a HubSpot account.
### Get security activity
Get security activity for a HubSpot account.
### Get API usage
Get the daily API usage and limits for a HubSpot account.
### Get account details
Get account details for a HubSpot account.
### Get business units for a user
Get business Units identified by `userId`. The `userId` refers to the user's ID.
### Get account teams
Retrieve the details for the teams of this account.
### Get account roles
Retrieves the roles on an account.
### Get users
Retrieves a list of users from an account.
### Add user
Adds a new user. New users will only have minimal permissions, which is contacts-base. A welcome email will prompt them to set a password and log in to HubSpot.
### Get user
Retrieves a user identified by `userId`. `userId` refers to the user's ID by default, or optionally email as specified by the `IdProperty` query param.
### Remove user
Removes a user identified by `userId`. `userId` refers to the user's ID by default, or optionally email as specified by the `IdProperty` query param.
### Modifies a user
Modifies a user identified by `userId`. `userId` refers to the user's ID by default, or optionally email as specified by the `IdProperty` query param.

## Known Issues and Limitations
There are no known issues at this time.
