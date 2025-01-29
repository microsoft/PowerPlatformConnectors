# Microsoft Graph Add Users
Creates internal users or invites an external users using Microsoft Graph.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to register an Entra ID application with the permissions User.ReadWrite.All and Directory.ReadWrite.All.

## Obtaining Credentials
This connector will use OAuth to perform the actions as your signed in user.

## Supported Operations
### Create a user
Creates a new user with the given properties.
### Invite external user
Creates a new invitation for an external user to the organization.
### Add members to group
Adds a member or multiple members to a group. Note that up to 20 members can be added in a single request.

## Known Issues and Limitations
There are no known issues at this time.
