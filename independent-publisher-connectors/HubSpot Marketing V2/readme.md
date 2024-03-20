# HubSpot Marketing V2
Marketing software that enables you to drive revenue, save time and resources, and measure and optimize your investments. All on one easy-to-use platform. Drive revenue by connecting with leads at the right time and place across email, landing pages, forms, and more. Manage your contacts and campaigns in one place and use automation tools to scale your efficiency. Measure the success of your campaigns using powerful reporting tools.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have an account with [HubSpot](https://app.hubspot.com/signup-hubspot/crm) and be a Super Admin.

## Obtaining Credentials
Once you are logged in to your account, go to Settings -> Account Setup -> Integrations -> Private Apps. You will need to create a private app and assign it only the scopes you will use actions for. Once your private app is created, you will have an access token in the Auth section of the private app.

## Supported Operations
### Send email
Asynchronously send a transactional email. Returns the status of the email send with a status identifier that can be used to continuously query for the status using the Email Send Status.
### Subscribe contact
Subscribes a contact to the given subscription type. This action is not valid to use for subscribing a contact at a brand or portal level and will return an error.
### Unsubscribe contact
Unsubscribes a contact from the given subscription type. This action is not valid to use for unsubscribing a contact at a brand or portal level and will return an error.
### Get contact subscription statuses
Returns a list of subscriptions and their status for a given contact.
### Create bulk communication preferences
Creates a bulk communication preferences request. This request will return a status identifier that can be used to continuously query for the status using the Bulk Communication Preferences Status.
### Get subscription definitions
Retrieves a list of all subscription definitions for the portal.
### Get aggregated statistics
Retrieves aggregated statistics of emails sent in a specified time span. It also returns the list of emails that were sent during the time span.
### Get aggregated statistic intervals
Retrieves aggregated statistics in intervals for a specified time span. Each interval contains aggregated statistics of the emails that were sent in that time.
### Get AB marketing email variation
Retrieves the variation of an A/B marketing email. If the email is variation A (master) it will return variation B (variant) and vice versa.
### Reset draft
Resets the draft back to a copy of the live object.
### Restore draft marketing email
Restores a previous revision of a marketing email to DRAFT state. If there is currently something in the draft for that object, it is overwritten.
### Get marketing email draft
Retrieves the draft version of an email (if it exists). If no draft version exists, the published email is returned.
### Create or update draft email
Creates or updates the draft version of a marketing email. If no draft exists, the system creates a draft from the current “live” email then applies the request body to that draft. The draft version only lives on the buffer—the email is not cloned.
### Get marketing email revisions
Retrieves a list of all versions of a marketing email, with each entry including the full state of that particular version. The current revision has the ID -1.
### Get a revision of a marketing email
Retrieves a specific revision of a marketing email.
### Create AB test marketing email
Creates a variation of a marketing email for an A/B test. The new variation will be created as a draft. If an active variation already exists, a new one won't be created.
### Get marketing emails
Retrieve a list of all marketing emails. The results can be filtered, allowing you to find a specific set of emails.
### Create marketing email
Creates a new marketing email.
### Restore marketing email revision
Restores a previous revision of a marketing email. The current revision becomes old, and the restored revision is given a new version number.
### Clone marketing email
Creates a duplicate email with the same properties as the original, with the exception of a unique identifier.
### Get marketing email
Retrieves the details for a marketing email.
### Delete marketing email
Deletes a marketing email.
### Update marketing email
Change properties of a marketing email.
### Create SMTP token
Creates a SMTP token.
### Query SMTP tokens
Queries multiple SMTP tokens by campaign name or a single token by email campaign identifier.
### Delete token
Deletes a single token by identifier.
### Query token
Queries a single token by identifier.
### Reset token password
Allows the creation of a replacement password for a given token. Once the password is successfully reset, the old password for the token will be invalid.

## Known Issues and Limitations
There are no known issues at this time.
