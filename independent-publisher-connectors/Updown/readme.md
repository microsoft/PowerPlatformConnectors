# Updown
Updown is an online service that checks your website's status by periodically sending an HTTP request to the URL of your choice. It then notifies you by email, sms or even a webhook, when your website is not responding correctly. With the help of this connector, you can manage your checks, recipients and many more.

## Publisher: Fördős András

## Prerequisites
There are no prerequisites needed for this connector apart from having credentials to Updown.io.

## Obtaining Credentials
Updown provides a free subscription with initial credits, and after that, a 'pay-as-you-go' model. Sign up on their website [https://updown.io/users/sign_up](https://updown.io/users/sign_up), and then head to your Settings menu. You can find your API Key under the API section.

## Supported Operations
### List checks
List all your checks.
### Get check
Show a single check.
### Get check metrics
Get detailed metrics about the check.
### Create check
Add a new check.
### Delete check
Delete a check.
### List API nodes
List all updown.io monitoring nodes.
### List recipients
List all the possible alert recipients/channels on your account.
### Create recipient
Add a new recipient.
### Delete recipient
Delete a recipient.

## Known Issues and Limitations
There are no known issues at this time for the connector.

Be aware that the underlying service has more endpoints available, which the current version of the connector does not support. If you have a requirement to use any of those, let's get in touch!