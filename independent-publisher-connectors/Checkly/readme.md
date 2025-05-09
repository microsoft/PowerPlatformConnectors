# Checkly
Checkly is the monitoring platform for the modern stack: programmable, flexible and loving JavaScript. Monitor and validate your crucial site transactions. Automatically collect error traces, screenshots and performance metrics with every check you run.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to sign up for a [Checkly](https://checklyhq.com/) account.

## Obtaining Credentials
Under your account Settings, find the API Keys section and generate an API key to be used for this connector.

## Supported Operations
### List all alert channels
Lists all configured alert channels and their subscribed checks.
### Create an alert channel
Creates a new alert channel.
### Retrieve an alert channel
Show details of a specific alert channel.
### Delete an alert channel
Permanently removes an alert channel.
### Update an alert channel
Update an alert channel.
### Update the subscriptions of an alert channel
Update the subscriptions of an alert channel. Use this to add a check to an alert channel so failure and recovery alerts are send out for that check. Note: when passing the subscription object, you can only specify a "checkId" or a "groupId, not both.
### Get check status badge
Get check status badge.
### Get group status badge
Get group status badge.
### List all alerts for your account
List all alerts for your account.
### List alerts for a specific check
Lists all the alerts for a specific check.
### List all check groups
Lists all current check groups in your account. The "checks" property is an array of check UUID's for convenient referencing. It is read only and you cannot use it to add checks to a group.
### Create a check group
Creates a new check group. You can add checks to the group by setting the "groupId" property of individual checks.
### Retrieve one check in a specific group with group settings applied
Show details of one check in a specific check group with the group settings applied.
### Retrieve a check group
Show details of a specific check group.
### Delete a check group
Permanently removes a check group. You cannot delete a check group if it still contains checks.
### Update a check group
Updates a check group.
### Retrieve all checks in a specific group with group settings applied
Lists all checks in a specific check group with the group settings applied.
### Lists all check results
Lists the full, raw check results for a specific check. We keep raw results for 30 days. After 30 days they are erased. However we keep the rolled up results for an indefinite period. You can filter by check type and result type to narrow down the list. Use the `to` and `from` parameters to specify a date range (UNIX timestamp in seconds). Depending on the check type, some fields might be null. This endpoint will return data within a six-hours timeframe. If from and to params are set, they must be at most six hours apart. If none are set, we will consider the to param to be now and from param to be six hours earlier. If only the to param is set we will set from to be six hours earlier. On the contrary, if only the from param is set we will consider the to param to be six hours later. Rate-limiting is applied to this endpoint, you can send 5 requests / 10 seconds at most.
### Retrieve a check result
Show details of a specific check result.
### List all check statuses
Shows the current status information for all checks in your account. The check status records are continuously updated as new check results come in.
### Retrieve check status details
Show the current status information for a specific check.
### List all checks
Lists all current checks in your account.
### Create an API check
Creates a new API check.
### Update an API check
Updates an API check.
### Create a browser check
Creates a new browser check.
### Update a browser check
Updates a browser check.
### Retrieve a check
Show details of a specific API or browser check.
### Delete a check
Permanently removes a API or browser check and all its related status and results data.
### List all dashboards
Lists all current dashboards in your account.
### Create a dashboard
Creates a new dashboard. Will return a 409 when attempting to create a dashboard with a custom URL or custom domain that is already taken.
### Delete a dashboard
Permanently removes a dashboard.
### Update a dashboard
Updates a dashboard.
### Lists all supported locations
Lists all supported locations.
### List all maintenance windows
Lists all maintenance windows in your account.
### Create a maintenance window
Creates a new maintenance window.
### Retrieve a maintenance window
Show details of a specific maintenance window.
### Delete a maintenance window
Permanently removes a maintenance window.
### Update a maintenance window
Update a maintenance window.
### List all private locations
Lists all private locations in your account.
### Create a private location
Creates a new private location.
### Retrieve a private location
Show details of a specific private location.
### Remove a private location
Permanently removes a private location.
### Update a private location
Update a private location.
### Get private location health metrics from a window of time
Get private location health metrics from a window of time.
### Generates a report with aggregate statistics for checks and check groups
Generates a report with aggregated statistics for all checks or a filtered set of checks over a specified time window.
### Lists all supported runtimes
Lists all supported runtimes and the included NPM packages for Browser checks and setup & teardown scripts for API checks.
### Shows details for one specific runtime
Shows the details of all included NPM packages and their version for one specific runtime.
### List all snippets
Lists all current snippets in your account.
### Create a snippet
Creates a new snippet.
### Retrieve a snippet
Show details of a specific snippet.
### Delete a snippet
Permanently removes a snippet.
### Update a snippet
Update a snippet.
### Get the check group trigger
Finds the check group trigger.
### Delete the check group trigger
Deletes the check group trigger.
### Create the check group trigger
Creates the check group trigger.
### Get the check trigger
Finds the check trigger
### Delete the check trigger
Deletes the check trigger.
### Create the check trigger
Create the check trigger.
### List all environment variables
Lists all current environment variables in your account.
### Create a environment variable
Creates a new environment variable.
### Retrieve an environment variable
Show details of a specific environment variable.
### Delete an environment variable
Permanently removes an environment variable.
### Update a environment variable
Updates a environment variable.
### Generates a customizable report for all Browser Check metrics
Generates a report with aggregated or non-aggregated statistics for a specific Browser Check over a specified time window.

## Known Issues and Limitations
There are no known issues at this time.
