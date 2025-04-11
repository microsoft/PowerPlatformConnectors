# MailJet
Mailjet is an easy-to-use all-in-one e-mail platform.

## Publisher: Clément Olivier

## Prerequisites
You need to have a Mailjet account. You can [sign up](https://app.mailjet.com/signup) for free.

## Supported Operations
The connector supports the following operations:
* `Send Email v3.1`: Send an email message via Send API v3.1.
* `Send Email v3`: Send an email message via Send API v3.
* `Get Messages`: Get a list of messages with specific information on the type of content, tracking, sending and delivery.
* `Get Messages Information`: Retrieve sending / size / spam information about all messages.
* `Get Contacts`: Retrieve a list of all contacts. Includes information about contact status and creation / activity timestamps.
* `Create  Contact`: Add a new unique contact to your global contact list and select its exclusion status.
* `Get Contact By ID`: Retrieve a specific contact.
* `Update Contact`: Update the user-given name and exclusion status of a specific contact.
* `Create Contact List`: Create a new contact list.
* `Get All Contact Lists`: Get all contact lists.  -**new**-
* `Get Contact List by ID`: Get a contact list. -**new**-
* `Delete Contact List`: Delete a contact list.
* `Update Contact List`: Update a specific contact list by changing its name and / or deletion status.
* `Get Campaigns Draft`: Get all campaign drafts and their configuration.
* `Create Campaign Draft`: Create a new campaign draft. Newsletter and CampaignDraft objects are differentiated by the EditMode values.
* `Get Campaign Draft By ID`: Get a specific campaign draft and its configuration details.
* `Update Campaign Draft`: Update an existing campaign draft.
* `Get Campaign Overview`: Get general details and stats for all drafts, AB Testing objects and/or sent campaigns.
* `Get Contacts Statistics`: Retrieve a list of overall aggregated delivery and engagement statistics, grouped by contact.
* `Get Contacts Data`: Get information on all contacts and the property values associated with them. -**new**-
* `Get Contact Data by ID`: Get the contact property values relating to a specific contact. -**new**-

## Obtaining Credentials
### Mailjet side
Through the [API Key Management page](https://app.mailjet.com/account/api_keys), you can grab the API Key and the Secret Key.

### Custom connector side
Once you are configuring the connector on the Power Platform.
You will have to use the **API Key** and **Secret Key** in order to validate the security stage in the process.

## Getting Started
Visit Mailjet [REST API reference](https://dev.mailjet.com/email/reference/overview/) documentations page for further details. 

## Deployment Instructions
Import the swagger file in target Power Apps environment.
