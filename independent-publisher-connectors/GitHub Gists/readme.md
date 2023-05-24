# GitHub Gists
GitHub Gists enables the authorized user to list, create, update and delete the Gists on GitHub.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have a GitHub account.

## Obtaining Credentials
This connector uses the GitHub OAuth authentication. You will need to go into your Developer Settings and configure a new OAuth App for this connector. You will then use the app Client ID and Secret to authenticate the connector.

## Supported Operations
### List Gists for the authenticated user
Lists the authenticated user's Gists or if called anonymously, this action returns all public Gists.
### Create Gist
Allows you to add a new Gist with one or more files.
### List public Gists
List public gists sorted by most recently updated to least recently updated.
### List starred Gists
List the authenticated user's starred Gists.
### Get Gist
Retrieve a Gist.
### Delete Gist
Delete a Gist.
### Update Gist
Allows you to update a Gist's description and to update, delete, or rename Gist files. Files from the previous version of the Gist that aren't explicitly changed during an edit are unchanged.
### List Gist comments
List all comments for a Gist.
### Create Gist comment
Create a Gist comment.
### Get Gist comment
Retrieve a Gist comment.
### Delete Gist comment
Delete a Gist comment.
### Update Gist comment
Update a Gist comment.
### List Gist commits
Retrieve a list of Gist commits.
### List Gist forks
Retrieve a list of Gist forks.
### Fork Gist
Fork a gist.
### Check for starred Gist
Check if a gist is starred.
### Unstar Gist
Unstar a Gist.
### Star Gist
Star a Gist.
### Get Gist revision
Retrieve a Gist revision.

## Known Issues and Limitations
There are no known issues at this time.
