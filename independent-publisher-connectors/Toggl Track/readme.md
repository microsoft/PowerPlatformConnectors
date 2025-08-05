# Toggl Track
The only time tracking software that builds custom reports from your team's time data to maximize productivity and revenue. Our time tracking solution records a calendar timeline of all your work activities throughout the day.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must sign up for an account with [Toggl](https://toggl.com/).

## Obtaining Credentials
This connector uses basic authentication with your email address and password.

## Supported Operations
### Get current user
Returns details for the current user.
### Update current user
Updates details for the current user.
### Get user clients
Get clients for the current user.
### Get user features
Get features available to the current user.
### Get user location
Returns the client's IP-based location. If no data is present, empty response will be yielded.
### Check authentication
Used to check if authentication works.
### Get user organizations
Get all organizations a given user is part of.
### Get user projects
Get projects for the current user.
### Get user projects paginated
Get paginated projects for the current user.
### Get user tags
Returns tags for the current user.
### Get user tasks
Returns tasks from projects in which the user is participating.
### Get track reminders
Returns a list of track reminders.
### Get web timer
Get web timer configuration.
### Get user workspaces
Lists workspaces for given user.
### Get time entries
Lists latest time entries.
### Get current time entry
Load running time entry for user ID.
### Get time entry by ID
Load time entry by ID that is accessible by the current user.
### Get workspace users (organization level)
Returns any users who belong to the workspace directly or through at least one group.
### Get workspace users
List all users for a given workspace.
### Get workspace project users
List all projects users for a given workspace.
### Get user clients
Get clients for the current user.
### Get user organizations
Get all organizations a given user is part of.
### Get workspace users (organization level)
Returns any users who belong to the workspace directly or through at least one group.
### Get user projects
Get projects for the current user.
### Get user projects paginated
Get paginated projects for the current user.
### Get workspace project users
List all projects users for a given workspace.
### Get workspace projects
Get projects for given workspace.
### Get workspace project
Get project for given workspace.
### Get user tags
Returns tags for the current user.
### Get user tasks
Returns tasks from projects in which the user is participating.
### Get user workspaces
Lists workspaces for given user.
### Create time entry
Creates a new workspace time entry.
### Delete time entry
Deletes a workspace time entry.
### Update time entry
Updates a workspace time entry.
### Bulk edit time entries
Bulk editing time entries. Patch will be executed partially when there are errors with some records. No transaction, no rollback.
### Stop time entry
Stops a workspace time entry.
### Get workspace users (organization level)
Returns any users who belong to the workspace directly or through at least one group.
### Get single workspace
Get information of single workspace.
### Get rates list
Get rates by level (workspace\|project\|task\|user).
### Get workspace statistics
Returns workspace admins list, members count and groups count.
### Get workspace time entry constraints
Get the time entry constraints for a given workspace.
### Get workspace track reminders
Returns a list of track reminders.
### Get workspace users
List all users for a given workspace.
### Get workspace project users
List all projects users for a given workspace.
### Get workspace projects
Get projects for given workspace.
### Get workspace project
Get project for given workspace.
### Get time entries
Lists latest time entries.
### Get current time entry
Load running time entry for user ID.
### Get time entry by ID
Load time entry by ID that is accessible by the current user.
### When a time entry is created
Triggers when a new time entry is created in the workspace.
### When a time entry is updated
Triggers when an existing time entry is modified in the workspace.
### When a time entry is deleted
Triggers when a time entry is deleted from the workspace.
### When a time entry is started
Triggers when a time entry is started (timer begins) in the workspace.
### When a time entry is stopped
Triggers when a running time entry is stopped in the workspace.

## Known Issues and Limitations
There are no known issues at this time.
