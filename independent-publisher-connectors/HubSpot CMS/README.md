# HubSpot CRM
HubSpot’s CRM platform has all the tools and integrations you need for marketing, sales, content management, and customer service. CMS Hub powers your website with a CMS that makes it easy to grow better. Developers build using flexible themes and content structures. Marketers easily edit and create pages on their own. Customers get a personalized, secure experience.

## Publisher: Hitachi Solutions

## Prerequisites
A paid or trial HubSpot account.

## Supported Operations

### Pages
#### List pages (V2)
Get all of your pages.
#### Create a new page (V2)
Create a new page.
#### Update a page (V2)
Updates a page in the database. If not all top level fields are included in the body, we will only update the included fields.
#### Delete a page (V2)
Marks a page as deleted. Returns 204 No Content letting you know that the page was successfully deleted. The page can be restored later via a POST to the restore-deleted endpoint.
#### Publish or unpublish a page (V2)
Either publishes or cancels publishing based on the POSTed JSON.

### Templates
#### List templates (V2)
Get all templates.
#### Create a new template (V2)
Create a new coded template object in Design Manager.
#### Update the template (V2)
Updates a template. If not all the fields are included in the body, we will only update the included fields.
#### Delete the template (V2)
Marks the selected Template as deleted. The Template can be restored later via a POST to the restore-deleted endpoint.

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
