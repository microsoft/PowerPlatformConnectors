# HubSpot CRM V2
With the easy-to-use HubSpot CRM, you'll never have to manually update reports or wonder how your team is tracking toward quotas. Get a real-time view of your entire sales pipeline on a visual dashboard. HubSpot CRM will automatically organize, enrich, and track each contact in a tidy timeline.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have an account with [HubSpot](https://app.hubspot.com/signup-hubspot/crm) and be a Super Admin.

## Obtaining Credentials
Once you are logged in to your account, go to Settings -> Account Setup -> Integrations -> Private Apps. You will need to create a private app and assign it only the scopes you will use actions for. Once your private app is created, you will have an access token in the Auth section of the private app.

## Supported Operations
### Get companies by ID
Retrieve a batch of companies by internal ID, or unique property values.
### Create a batch of companies
Create a batch of companies.
### Update a batch of companies
Update a batch of companies.
### Archive a batch of companies by ID
Archive a batch of companies by identifier.
### Get companies
Retrieve a page of companies. Control what is returned via the properties query parameter.
### Create a company
Create a company with the given properties and return a copy of the object, including the identifier.
### Get a company by ID
Retrieve an object identified by companyidentifier. companyId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a company by ID
Move an object identified by companyId to the recycling bin.
### Update a company by ID
Perform a partial update of an object identified by companyidentifier. companyId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two companies
Merge two companies with same type.
### Delete a company to follow GDPR
Permanently delete a company and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search companies
Retrieve a list of companies.
### Archive a batch of contacts by ID
Archive a batch of contacts by identifier.
### Update a batch of contacts
Update a batch of contacts.
### Create a batch of contacts
Create a batch of contacts.
### Get a batch of contacts by ID
Retrieve a batch of contacts by internal ID, or unique property values.
### Get contacts
Retrieve a page of contacts. Control what is returned via the properties query parameter.
### Create a contact
Create a contact with the given properties and return a copy of the object, including the identifier.
### Get a contact by ID
Retrieve an object identified by contactidentifier. contactId refers to the internal object identifier.  Control what is returned via the properties query parameter.
### Archive a contact by ID
Move an object identified by contactId to the recycling bin.
### Update a contact by ID
Perform a partial update of an object identified by contactidentifier. contactId refers to the internal object identifier. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two contacts
Merge two contacts with same type.
### Delete a contact to follow GDPR
Permanently delete a contact and all associated content to follow.
### Get a batch of deals by ID
Retrieve a batch of deals by internal identifier, or unique property values.
### Get deals
Retrieve a page of deals. Control what is returned via the properties query parameter.
### Create a deal
Create a deal with the given properties and return a copy of the object, including the identifier.
### Get a deal
Retrieve an object identified by dealidentifier. dealId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a deal by ID
Move an object identified by dealId to the recycling bin.
### Update a deal
Perform a partial update of an object identified by dealidentifier. dealId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two deals
Merge two deals with same type.
### Delete a deal to follow GDPR
Permanently delete a deal and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search deals
Retrieve a list of deals.
### Create a batch of fees
Create a batch of fees.
### Update a batch of fees
Update a batch of fees.
### Archive a batch of fees by ID
Archive a batch of fees by identifier.
### Get a batch of fees by ID
Retrieve a batch of fees by internal identifier, or unique property values.
### Get a fee by ID
Retrieve an object identified by feeidentifier. feeId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a fee by ID
Move an object identified by feeId to the recycling bin.
### Update a fee by ID
Perform a partial update of an object identified by feeidentifier. feeId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Get fees
Retrieve a page of fees. Control what is returned via the properties query parameter.
### Create a fee
Create a fee with the given properties and return a copy of the object, including the identifier.
### Merge two fees
Merge two fees with same type.
### Delete a fee to follow GDPR
Permanently delete a fee and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search for fees
Retrieve a list of fees.
### Create a batch of goal targets
Create a batch of goal targets.
### Get a batch of goal targets by ID
Retrieve a batch of goal targets by internal identifier, or unique property values.
### Archive a batch of goal targets by ID
Archive a batch of goal targets by identifier.
### Update a batch of goal targets
Update a batch of goal targets.
### Get a goal target by ID
Retrieve an object identified by goalTargetidentifier. goalTargetId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a goal target
Move an object identified by goalTargetId to the recycling bin.
### Update a goal target
Perform a partial update of an object identified by goalTargetidentifier. goalTargetId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Get goal targets
Retrieve a page of goal targets. Control what is returned via the properties query parameter.
### Create a goal target
Create a goal target with the given properties and return a copy of the object, including the identifier.
### Merge two goal targets
Merge two goal targets with same type.
### Delete a goal target to follow GDPR
Permanently delete a goal target and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search goal targets
Retrieve a list of goal targets.
### Create a batch of line items
Create a batch of line items.
### Update a batch of line items
Update a batch of line items.
### Archive a batch of line items by ID
Archive a batch of line items by identifier.
### Get a batch of line items by ID
Retrieve a batch of line items by internal ID, or unique property values.
### Get line items
Retrieve a page of line items. Control what is returned via the properties query parameter.
### Create a line item
Create a line item with the given properties and return a copy of the object, including the identifier.
### Get line item
Retrieve an object identified by lineItemidentifier. lineItemId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a line item
Move an object identified by lineItemId to the recycling bin.
### Update a line item
Perform a partial update of an object identified by lineItemidentifier. lineItemId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two line items
Merge two line items with same type.
### Delete a line item to follow GDPR
Permanently delete a line item and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search line items
Retrieve a list of line items.
### Get owners
Retrieve a list of owners.
### Get an owner by ID
Retrieve an owner by given identifier or user identifier.
### Archive a batch of products by ID
Archive a batch of products by identifier.
### Get a batch of products by ID
Retrieve a batch of products by internal ID, or unique property values.
### Create a batch of products
Create a batch of products.
### Update a batch of products
Update a batch of products.
### Get products
Retrieve a page of products. Control what is returned via the properties query parameter.
### Create a product
Create a product with the given properties and return a copy of the object, including the identifier.
### Get a product
Retrieve an object identified by product identifier. productId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a product
Move an object identified by productId to the recycling bin.
### Update a product
Perform a partial update of an object identified by product identifier. productId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two products
Merge two products with same type.
### Delete a product to follow GDPR
Permanently delete a product and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search products
Retrieve a list of products.
### Get a batch of objects by ID
Retrieve a batch of objects by internal identifier, or unique property values.
### Archive a batch of objects by ID
Archive a batch of objects by identifier.
### Create a batch of objects
Create a batch of objects.
### Update a batch of objects
Update a batch of objects.
### Get an object by ID
Retrieve an object identified by object identifier. objectId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive an object
Move an object identified by objectId to the recycling bin.
### Update an object
Perform a partial update of an object identified by objectidentifier. objectId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Get objects
Retrieve a page of objects. Control what is returned via the properties query parameter.
### Create an object
Create a CRM object with the given properties and return a copy of the object, including the identifier.
### Merge two objects
Merge two objects with same type.
### Delete an object type to follow GDPR
Permanently delete an object type and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search by object type
Retrieve a list of objects by object type.
### Update a batch of discounts
Update a batch of discounts.
### Create a batch of discounts
Create a batch of discounts.
### Archive a batch of discounts by ID
Archive a batch of discounts by identifier.
### Get a batch of discounts by ID
Retrieve a batch of discounts by internal identifier, or unique property values.
### Get a discount
Retrieve an object identified by discount identifier. discountId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a discount
Move an object identified by discountId to the recycling bin.
### Update a discount
Perform a partial update of an object identified by discount identifier. discountId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### List discounts
Retrieve a page of discounts. Control what is returned via the properties query parameter.
### Create a discount
Create a discount with the given properties and return a copy of the object, including the identifier.
### Merge two discounts
Merge two discounts with same type.
### Delete a discount to follow GDPR
Permanently delete a discount and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search discounts
Retrieve a list of discounts.
### Update a batch of feedback submissions
Update a batch of feedback submissions.
### Create a batch of feedback submissions
Create a batch of feedback submissions.
### Archive a batch of feedback submissions by ID
Archive a batch of feedback submissions by identifier.
### Get a batch of feedback submissions by ID
Retrieve a batch of feedback submissions by internal identifier, or unique property values.
### Get a feedback submission
Retrieve an object identified by feedbackSubmissionidentifier. feedbackSubmissionId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a feedback submission
Move an object identified by feedbackSubmissionId to the recycling bin.
### Update a feedback submission
Perform a partial update of an object identified by feedbackSubmissionidentifier. feedbackSubmissionId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### List feedback submissions
Retrieve a page of feedback submissions. Control what is returned via the properties query parameter.
### Create a feedback submission
Create a feedback submission with the given properties and return a copy of the object, including the identifier.
### Merge two feedback submissions
Merge two feedback submissions with same type.
### Delete a feedback submission to follow GDPR
Permanently delete a feedback submission and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search feedback submissions
Retrieve a list of feedback submissions.
### Archive a batch of quotes by ID
Archive a batch of quotes by identifier.
### Update a batch of quotes
Update a batch of quotes.
### Create a batch of quotes
Create a batch of quotes.
### Get a batch of quotes by ID
Retrieve a batch of quotes by internal identifier, or unique property values.
### List quotes
Retrieve a page of quotes. Control what is returned via the properties query parameter.
### Create quote
Create a quote with the given properties and return a copy of the object, including the identifier.
### Get a quote by ID
Retrieve an object identified by quote identifier. quoteId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a quote by ID
Move an object identified by quoteId to the recycling bin.
### Update a quote
Perform a partial update of an object identified by quote identifier. quoteId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two quotes
Merge two quotes with same type.
### Delete a quote to follow GDPR
Permanently delete a quote and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search quotes
Retrieve a list of quotes.
### Update a batch of taxes
Update a batch of taxes.
### Create a batch of taxes
Create a batch of taxes.
### Get a batch of taxes by ID
Retrieve a batch of taxes by internal identifier, or unique property values.
### Archive a batch of taxes by ID
Archive a batch of taxes by identifier.
### Get taxes
Retrieve a page of taxes. Control what is returned via the properties query parameter.
### Create a tax
Create a tax with the given properties and return a copy of the object, including the identifier.
### Get a tax by ID
Retrieve an object identified by tax identifier. taxId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a tax
Move an object identified by taxId to the recycling bin.
### Update a tax
Perform a partial update of an object identified by taxidentifier. taxId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two taxes
Merge two taxes with same type.
### Delete a tax to follow GDPR
Permanently delete a tax and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search taxes
Retrieve a list of taxes.
### Archive a batch of tickets by ID
Archive a batch of tickets by identifier.
### Get a batch of tickets by ID
Retrieve a batch of tickets by internal identifier, or unique property values.
### Create a batch of tickets
Create a batch of tickets.
### Update a batch of tickets
Update a batch of tickets.
### Get a ticket
Retrieve an object identified by ticket identifier. ticketId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter.  Control what is returned via the properties query parameter.
### Archive a ticket
Move an object identified by ticketId to the recycling bin.
### Update a ticket
Perform a partial update of an object identified by ticket identifier. ticketId refers to the internal object identifier by default, or optionally any unique property value as specified by the idProperty query parameter. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Get tickets
Retrieve a page of tickets. Control what is returned via the properties query parameter.
### Create a ticket
Create a ticket with the given properties and return a copy of the object, including the identifier.
### Merge two tickets
Merge two tickets with same type.
### Delete a ticket to follow GDPR
Permanently delete a ticket and all associated content to follow GDPR. Use optional property 'ID Property' set to 'email' to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search tickets
Retrieve a list of tickets.
### List association types
List all the valid association types available between two object types.
### Delete specific association labels
Batch delete specific association labels for objects. Deleting an unlabeled association will also delete all labeled associations between those two objects.
### Read batch associations
Batch read associations for objects to specific object type. The 'after' field in a returned paging object  can be added alongside the 'id' to retrieve the next page of associations from that objectidentifier. The 'link' field is deprecated and should be ignored.
### Create batch associations
Batch create associations for objects.
### Delete batch associations
Batch delete associations for objects.
### Create default associations
Create the default (most generic) association type between two object types.
### Delete associations
Deletes all associations between two records.
### Create association labels
Set association labels between two records.
### Create default association
Create the default (most generic) association type between two object types.
### List associations for an object
List all associations of an object by object type. Limit 500 per call.
### Get all cards
Returns a list of cards for a given app.
### Create a new card
Defines a new card that will become active on an account when this app is installed.
### Get a card
Returns the definition for a card with the given identifier.
### Delete a card
Permanently deletes a card definition with the given identifier. Once deleted, data fetch requests for this card will no longer be sent to your service. This can't be undone.
### Update a card
Update a card definition with new details.
### Get sample card detail response
Returns an example card detail response. This is the payload with displayed details for a card that will be shown to a user. An app should send this in response to the data fetch request.
### Get task ID status
Retrieve the status for a task identifier.
### Start an export
Begins exporting CRM data for the portal as specified in the request body.
### Get the information on any import
A complete summary of an import record, including any updates.
### Cancel an active import
This allows a developer to cancel an active import.
### Get active imports
Returns a paged list of active imports for this account.
### Start a new import
Begins importing data from the specified file resources. This uploads the corresponding file and uses the import request object to convert rows in the files to objects.
### Get import errors
Retrieve a list of errors for an import.
### Add and/or remove records from a list
Add and/or remove records that have already been created in the system to and/or from a list. This endpoint only works for lists that have a processingType of Manual or Snapshot.
### Add records to a list
Add the records provided to the list. Records that do not exist or that are already members of the list are ignored. This endpoint only works for lists that have a processingType of Manual or Snapshot.
### Add all records from a source list to a destination list
Add all of the records from a source list (specified by the sourceListId) to a destination list (specified by the listId). Records that are already members of the destination list will be ignored. The destination and source list IDs must be different. The destination and source lists must contain records of the same type (e.g. contacts, companies, etc.). This endpoint only works for destination lists that have a processingType of Manual or Snapshot. The source list can have any processingType.
### Fetch list memberships
Fetch the memberships of a list in order sorted by the recordId of the records in the list. The recordId's are sorted in ascending order if an after offset or no offset is provided. If only a before offset is provided, then the records are sorted in descending order. The after offset parameter will take precedence over the before offset in a case where both are provided.
### Delete all records from a list
Remove all of the records from a list. Note: The list is not deleted. This endpoint only works for lists that have a processingType of Manual or Snapshot.
### Remove records from a list
Remove the records provided from the list. Records that do not exist or that are not members of the list are ignored. This endpoint only works for lists that have a processingType of Manual or Snapshot.
### Search lists
Search lists by list name or page through all lists by providing an empty query value.

## Known Issues and Limitations
There are no known issues at this time.
