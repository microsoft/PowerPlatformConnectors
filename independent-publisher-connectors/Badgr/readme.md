# Badgr
Organizations around the world use Badgr to create branded learning ecosystems that support their communities with skills-based digital credentials, stackable learning pathways, and portable learner records.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need a free or paid [Badgr](https://info.badgr.com/) account in order to use this connector. There is an enterprise [Badgr Pro plan](https://info.badgr.com/features-and-pricing.html) that offers additional customization options for the Badgr platform, but the API operations are the same for both plans.

## Obtaining Credentials
This connector uses OAuth 2.0. In order to receive a client ID and secret, you will need to email Badgr support the following information:
- Your service name and homepage URL
- An app logo asset (square, at least 200x200 pixels, PNG format for use against white/light backgrounds)
- Your privacy policy URL
- Your terms of service URL
- The permission scopes you are requesting (https://badgr.org/app-developers/#scopes)
- Your application OAuth2 Redirect URIs
- The Badgr server you wish to connect to (US, AU, CA, EU)


## Supported Operations
### Get Issuers
Get a list of Issuers for authenticated user.
### Create a Issuer
Create a new Issuer.
### Get an Issuer
Get a single Issuer.
### Delete an Issuer
Delete a single Issuer.
### Update an Issuer
Update a single Issuer.
### Get Assertions
Get a list of Assertions for a single Issuer.
### Issue an Assertion
Issue a new Assertion to a recipient.
### Get Issuer's BadgeClasses
Get a list of BadgeClasses for a single Issuer. Authenticated user must have owner, editor, or staff status on the Issuer.
### Create an Issuer's BadgeClass
Create a new BadgeClass associated with an Issuer. Authenticated user must have owner, editor, or staff status on the Issuer.
### Get user's BadgeClasses
Get a list of BadgeClasses for authenticated user.
### Create a BadgeClass
Create a new BadgeClass.
### Get a BadgeClass
Get a single BadgeClass.
### Delete a BadgeClass
Delete a BadgeClass. Restricted to owners or editors (not staff) of the corresponding Issuer.
### Update a BadgeClass
Update an existing BadgeClass. Previously issued BadgeInstances will not be updated.
### Get BadgeClass's Assertions
Get a list of Assertions for a single BadgeClass.
### Issue an Assertion
Issue an Assertion to a single recipient.
### Get an Assertion
Get a single Assertion
### Revoke an Assertion
Revoke an Assertion.
### Update an Assertion
Update an Assertion.
### Get user's access tokens
Get a list of access tokens for authenticated user.
### Get an AccessToken
Get a single AccessToken.
### Revoke an AccessToken
Revoke a user's AccessToken.
### Get a BadgeUser's profile
Get a single BadgeUser profile. Use the entityId 'self' to retrieve the authenticated user's profile.
### Create a BadgeUser account
Make a BadgeUser account and profile.
### Update a BadgeUser
Update a BadgeUser's account. Use the entityId 'self' to update the authenticated user's profile
### Get Assertions in a user's backpack
Get a list of Assertions in authenticated user's backpack.
### Upload a Assertion to backpack
Upload a new Assertion to a user's backpack.
### Get Backpack Assertion details
Get details on an Assertion in a user's Backpack.
### Remove an assertion from backpack
Remove an assertion from a user's backpack.
### Update Assertion acceptance in Backpack
Update acceptance of an Assertion in a user's Backpack.
### Get Collections
Get a list of Collections.
### Create a Collection
Create a new Collection.
### Get a Collection
Get a single Collection.
### Delete a collection
Delete a collection from a backpack.
### Update a Collection
Update a Collection in a backpack.
### Issue multiple BadgeClass
Issue multiple copies of the same BadgeClass to multiple recipients.
### Revoke multiple Assertions
Revoke multiple Assertions for multiple recipients.
### Retrieve issuer tokens
Retrieve issuer tokens.
### Request an account recovery email
Request an account recovery email for a user.
### Recover an account
Recover an account and set a new password.
### Import a Assertion to backpack
Import a new Assertion to a user's backpack.

## Known Issues and Limitations
There are no known issues at this time.
