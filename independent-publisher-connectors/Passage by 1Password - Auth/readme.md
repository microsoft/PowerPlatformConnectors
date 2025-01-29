# Passage by 1Password - Auth
Passage is backed by 1Password's 17+ years of industry-leading security expertise. Completely replace your existing authentication flow or build from scratch with a robust solution for passwordless authentication and customer identity management. Realize the full security, business, and user experience benefits of eliminating passwords by implementing login flows powered by passkeys, magic links, and login codes.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to sign up for an account with [Passage](https://console.passage.id/register).

## Obtaining Credentials
Once logged in to your account, you will need to create a new application. In the new application, there will be an API Keys section in Settings, where you can create a new API Key.

## Supported Operations
### Get app
Retrieve information about an application.
### Get JWKS
Retrieve JWKS for an app. KIDs in the JWT can be used to match the appropriate JWK, and use the JWK's public key to verify the JWT.
### Get OpenID configuration
Retrieve OpenID configuration for an app.
### Get current user
Retrieve information about a user that is currently authenticated via bearer token.
### List devices
Retrieve a list of all WebAuthn devices for the authenticated user. User must be authenticated via bearer token.
### Finish WebAuthn add device
Completes a WebAuthn add device operation for the current user. User must be authenticated via a bearer token.
### Start WebAuthn add device
Initiate a WebAuthn add device operation for the current user. This endpoint creates a WebAuthn credential creation challenge that is used to perform the registration ceremony from the browser. User must be authenticated via a bearer token.
### Revoke device
Revoke a device by ID for the current user. User must be authenticated via a bearer token.
### Update device
Updates a device by ID for the current user. Currently the only field that can be updated is the friendly name. User must be authenticated via a bearer token.
### Change email
Initiate an email change for the authenticated user. An email change requires verification, so an email will be sent to the user which they must verify before the email change takes effect.
### Change Phone
Initiate a phone number change for the authenticated user. A phone number change requires verification, so an SMS with a link will be sent to the user which they must verify before the phone number change takes effect.
### Get user's metadata
Retrieve the user-metadata for the current user.
### Update user's metadata
Updates the metadata for the current user. Only valid metadata fields are accepted. Invalid metadata fields that are present will abort the update. User must be authenticated via a bearer token.
### Get social connections
Gets social connections for the current user. User must be authenticated via a bearer token.
### Delete social connection
Deletes a social connection for the current user. User must be authenticated via a bearer token.
### Login with magic link
Send a login email or SMS to the user. The user will receive an email or text with a link to complete their login.
### Finish WebAuthn login
Completes a WebAuthn login and authenticate the user.
### Start WebAuthn login
Initiate a WebAuthn login for a user. This endpoint creates a WebAuthn credential assertion challenge that is used to perform the login ceremony from the browser.
### Login with OTP
Send a login email or SMS to the user. The user will receive an email or text with a one-time passcode to complete their login.
### Authenticate magic link
Authenticates a magic link for a user. This endpoint checks that the magic link is valid, then returns an authentication token for the user.
### Magic link status
Check if a magic link has been activated yet or not. Once the magic link has been activated, this endpoint will return an authentication token for the user. This endpoint can be used to initiate a login on one device and then poll and wait for the login to complete on another device.
### Register with magic link
Create a user and send an registration email or SMS to the user. The user will receive an email or text with a link to complete their registration.
### Finish WebAuthn registration
Completes a WebAuthn registration and authenticate the user.
### Register with OTP
Create a user and send a registration email or SMS to the user. The user will receive an email or text with a one-time passcode to complete their registration.
### Start WebAuthn registration
Initiate a WebAuthn registration and create the user. This endpoint creates a WebAuthn credential creation challenge that is used to perform the registration ceremony from the browser.
### Revoke refresh token
Revokes the refresh token.
### Create new auth and refresh token
Creates and returns a new auth token and a new refresh token.
### Get user
Retrieve user information, if the user exists. This endpoint can be used to determine whether a user has an existing account and if they should login or register.
### Create user
Create a user.
### Link an account to a connection
Links an existing account to an OAuth2 connection.
### Start OAuth2 flow
Kicks off an OAuth2 flow with connection provider request params described in <https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest>
### Exchange OAuth2 for auth token
Exchanges OAuth2 connection data for an auth token.
### Handle OAuth2 callback
Handles an OAuth2 flow callback.
### Authenticate OTP
Authenticates a one-time passcode for a user. This endpoint checks that the one-time passcode is valid, then returns an authentication token for the user.

## Known Issues and Limitations
There are no known issues at this time.
