# HubSpot Marketing
HubSpot’s CRM platform has all the tools and integrations you need for marketing, sales, content management, and customer service. With Marketing Hub, all your marketing tools and data are on one easy-to-use, powerful platform. You’ll save valuable time and get all the context you need to provide a personalized experience that attracts and converts the right customers at scale.

## Publisher: Hitachi Solutions

## Prerequisites
A paid or trial HubSpot account.

## Getting Started
You will need to gather either an API key or OAuth credentials to use with this connector.

## Obtaining Credentials
A HubSpot account (trial or paid) is needed for API key access. The API key is specific to a HubSpot account, not each user, and only one key is allowed at a time. The key can be found in Account Settings > Account Setup > Integrations > API Keys. More information can be found [here](https://knowledge.hubspot.com/articles/kcs_article/integrations/how-do-i-get-my-hubspot-api-key).

A free HubSpot developer account is needed to install an OAuth app in your account for OAuth 2.0 access. Installed apps can be found in Account Settings > Account Setup > Integrations > Connected Apps. More information can be found [here](https://developers.hubspot.com/docs/api/working-with-oauth).

## Supported Operations

### Marketing Event
#### Get a marketing event (Preview)
Returns the details of the Marketing Event with the specified id, if one exists.
#### Create or update a marketing event (Preview)
Upserts a Marketing Event. If there is an existing Marketing event with the specified id, it will be updated; otherwise a new event will be created.
#### Delete a marketing event (Preview)
Archives an existing Marketing Event with the specified id, if one exists.

### Marketing Email
#### Get all marketing emails (V1)
Get all marketing emails for a HubSpot account.
#### Get a marketing email (V1)
Get the details for a specific marketing email.
#### Get campaign data for a particular campaign (V1)
For a given campaign, return data associated with the campaign.
#### Create a marketing email (V1)
Create a new marketing email.
#### Update a marketing email (V1)
Update a marketing email.
#### Delete a marketing email (V1)
Archive a marketing email.

### Forms
#### Get a list of forms (Preview)
Returns a list of forms based on the search filters. By default, it returns the first 20 hubspot forms.
#### Get a form (Preview)
Returns a form based on the form ID provided.
#### Create a form (Preview)
Add a new HubSpot form.
#### Update a form (Preview)
Update all fields of a HubSpot form definition.
#### Delete a form (Preview)
Archive a form definition. New submissions will not be accepted and the form definition will be permanently deleted after 3 months.

## Obtaining Credentials
A HubSpot account (trial or paid) is needed for API key access. The API key is specific to a HubSpot account, not each user, and only one key is allowed at a time. The key can be found in Account Settings > Account Setup > Integrations > API Keys. More information can be found [here](https://knowledge.hubspot.com/articles/kcs_article/integrations/how-do-i-get-my-hubspot-api-key).

A free HubSpot developer account is needed to install an OAuth app in your account for OAuth 2.0 access. Installed apps can be found in Account Settings > Account Setup > Integrations > Connected Apps. More information can be found [here](https://developers.hubspot.com/docs/api/working-with-oauth).

## Getting Started
No specific instruction required for getting started.

## Known Issues and Limitations
No issues and limitations are known at this time.

## Frequently Asked Questions
### How do I obtain API key?
If you are not the HubSpot administrator for your account, check with the administrator before generating a new API key. If you have a HubSpot developer account connected to your company account, multiple OAuth apps can be installed in each company account.
