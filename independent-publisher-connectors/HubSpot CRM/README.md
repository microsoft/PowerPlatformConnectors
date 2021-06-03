# HubSpot CRM
HubSpot’s CRM platform has all the tools and integrations you need for marketing, sales, content management, and customer service. Powerful sales CRM software to help teams close more deals, deepen relationships, and manage their pipeline more effectively — all on one easy-to-use platform.

## Publisher: Hitachi Solutions

## Prerequisites
A paid or trial HubSpot account.

## Supported Operations

### Companies

#### List companies
Return a list of companies.
#### Create a company
Create a company with the given properties and return a copy of the object, including the ID.
#### Get a company
Read a company identified by {companyId}.
#### Update a company
Perform a partial update of a company identified by {companyId}. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
#### Delete a company
Move a company identified by {companyId} to the recycling bin.

### Contacts

#### List contacts
Read a list of contacts.
#### Create a contact
Create a contact with the given properties and return a copy of the object, including the ID.
#### Get a contact
Read a contact identified by {contactId}.
#### Update a contact
Perform a partial update of a contact identified by {contactId}. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
#### Delete a contact
Move a contact identified by {contactId} to the recycling bin.

### Deals

#### List deals
Return a list of deals.
#### Create a deal
Create a deal with the given properties and return a copy of the object, including the ID.
#### Get a deal
Read a deal identified by {dealId}.
#### Update a deal
Perform a partial update of a deal identified by {dealId}. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
#### Delete a deal
Move a deal identified by {dealId} to the recycling bin.

### Products

#### List products
Return a list of products.
#### Create a product
Create a product with the given properties and return a copy of the object, including the ID.
#### Get a product
Read a product identified by {productId}.
#### Update a product
Perform a partial update of a product identified by {productId}. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
#### Delete a product
Move a product identified by {productId} to the recycling bin.

### Owners

#### List owners
Return a list of owners.
#### Get an owner
Read an owner by given id or userId.

### Pipelines

#### List pipelines
Return all pipelines for the object type specified by {objectType}.
#### Create a pipeline
Create a new pipeline with the provided property values. The entire pipeline object, including its unique ID, will be returned in the response.
#### Get a pipeline
Return a single pipeline object identified by its unique {pipelineId}.
#### Update a pipeline
Perform a partial update of the pipeline identified by {pipelineId}. The updated pipeline will be returned in the response.
#### Replace a pipeline
Replace all the properties of an existing pipeline with the values provided. This will overwrite any existing pipeline stages. The updated pipeline will be returned in the response.
#### Delete a pipeline
Move a pipeline identified by {pipelineId} to the recycling bin.

### Pipeline Stages

#### Create a pipeline stage
Create a new stage associated with the pipeline identified by {pipelineId}. The entire stage object, including its unique ID, will be returned in the response.
#### Get a pipeline stage
Return the stage identified by {stageId} associated with the pipeline identified by {pipelineId}.
#### Update a pipeline stage
Perform a partial update of the pipeline stage identified by {stageId} associated with the pipeline identified by {pipelineId}. Any properties not included in this update will keep their existing values. The updated stage will be returned in the response.
#### Replace a pipeline stage
Replace all the properties of an existing pipeline stage with the values provided. The updated stage will be returned in the response.
#### Delete a pipeline stage
Archive the pipeline stage identified by {stageId} associated with the pipeline identified by {pipelineId}.

### Properties

#### List properties
Read all existing properties for the specified object type and HubSpot account.
#### Create a property
Create and return a copy of a new property for the specified object type.
#### Get a property
Read a property identified by {propertyName}.
#### Update a property
Perform a partial update of a property identified by {propertyName}. Provided fields will be overwritten.
#### Delete a property
Move a property identified by {propertyName} to the recycling bin.

### Line Items

#### List line items
Return a list of line items. Control what is returned via the properties query param.
#### Create a line item
Create a line item with the given properties and return a copy of the object, including the ID.
#### Get a line item
Read a line item identified by {lineItemId}. {lineItemId} refers to the internal object ID by default, or optionally any unique property value as specified by the idProperty query param. Control what is returned via the properties query param.
#### Update a line item
Perform a partial update of a line item identified by {lineItemId}. {lineItemId} refers to the internal object ID by default, or optionally any unique property value as specified by the idProperty query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
#### Delete a line item
Move an Object identified by {lineItemId} to the recycling bin.

### Tickets

#### List tickets
Read a list of tickets. Control what is returned via the properties query param.
#### Create a ticket
Create a ticket with the given properties and return a copy of the object, including the ID. Documentation and examples for creating standard tickets is provided.
#### Get a ticket
Read a ticket identified by {ticketId}. {ticketId} refers to the internal object ID by default, or optionally any unique property value as specified by the idProperty query param. Control what is returned via the properties query param.
#### Update a ticket
Perform a partial update of a ticket identified by {ticketId}. {ticketId} refers to the internal object ID by default, or optionally any unique property value as specified by the idProperty query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
#### Archive a ticket
Move a ticket identified by {ticketId} to the recycling bin.

### Search

#### Search Companies
Perform a search across companies.
#### Search Contacts
Perform a search across contacts.
#### Search Deals
Perform a search across deals.
#### Search Products
Perform a search across products.
#### Search Tickets
Perform a search across tickets.
#### Search Line Items
Perform a search across line items.
#### Search Quotes
The CRM search endpoints make getting data more efficient by allowing developers to filter, sort, and search across quotes.
#### Search Custom Objects
Perform a search across custom objects.

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
