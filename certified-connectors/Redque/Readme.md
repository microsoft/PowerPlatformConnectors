# Redque
Redque is an AI engine for automated document processing and field extraction. Using this API, you can manage your documents, accounting units, approval workflows, enums, folders, tenant settings, and users in a Redque system. Very often, you may want to leverage those functions in your accounting application or in your process automation.

## Publisher: Redque s.r.o.

## Prerequisites

You will need the following to proceed:

* A Redque account
* A client application registered in Redque system with API access enabled
* Client ID and Client Secret for your application

## Getting started

* Prepare your Redque account or create a new one at https://redque.cz/uvodni-stranka/

## Obtaining Credentials

Register a client application in the Redque system to obtain your Client ID and Client Secret. These credentials are used for the OAuth2 Client Credentials flow to authenticate API requests.

## Deployment Instructions

* In the [Power Automate portal](https://make.powerautomate.com/), create a new flow or edit an existing one.

* Add a new action to your flow and in the "Choose an operation" menu, search for "Redque" under the Premium tab.

* Select your preferred action.

* You will be prompted to supply your Client ID and Client Secret. This Connection will be saved by Power Automate, and available for use in future flows.

* That's it! You can now use Redque actions in all your Power Automate flows.

## Supported Operations

The connector supports the following operations:

### Accounting Units

* `Create a new Accounting unit`: Creates a new accounting unit
* `Get all Accounting units`: Lists all accounting units
* `Get current user's primary Accounting unit`: Returns the primary accounting unit of the current user
* `Delete Accounting unit`: Deletes an accounting unit by ID
* `Get Accounting unit by Id`: Returns an accounting unit by ID
* `Update Accounting unit`: Updates an existing accounting unit

### Approval Workflow

* `List documents for approval`: Lists documents which are ready for approval
* `Return number of documents waiting for approval`: Returns count of documents awaiting approval
* `Approve document`: Approves document and moves it to the next approval step
* `Reject document`: Rejects document and stops the approval workflow
* `Delegate document`: Delegates document to be approved by someone else
* `Start document approval workflow`: Starts the approval workflow for a document
* `Restart document approval workflow`: Restarts approval workflow from the beginning
* `Get approval workflow history`: Returns approval workflow history for a given document
* `Search approval field values`: Searches document approval field values

### Attachments

* `Upload attachment`: Uploads an attachment for a document
* `Download attachment`: Downloads an attachment file
* `Delete attachment`: Deletes an attachment
* `Change attachment to document`: Converts an attachment to a standalone document
* `Rename attachment`: Renames an attachment

### Client Applications

* `Create client application`: Creates a new client application configuration
* `Get all client applications`: Lists all client applications of the current tenant
* `Get client application`: Returns a client application by ID
* `Update client application`: Updates a client application configuration
* `Delete client application`: Deletes a client application

### Dashboards

* `Generate dashboard data`: Generates dashboard data

### Document Types

* `Get active document types`: Returns active document types for the tenant
* `Get available document types`: Returns available document types for the tenant
* `Get localized document types`: Returns localized document types
* `Get document type definition`: Returns field definition for a given document type
* `Get document type detail`: Returns document type detail
* `Get document type fields`: Returns fields of a document type
* `Create field`: Creates a new field for a document type
* `Update field`: Updates a field definition
* `Delete field`: Deletes a field from a document type
* `Activate/deactivate document type`: Activates or deactivates a document type
* `Set document type as default`: Sets a specific document type as the default
* `Change document type order`: Changes the order of document types
* `Update document type`: Updates document type definition
* `Search document field values`: Searches document field values by document type
* `Get enum values of field`: Returns all possible enum values for a field

### Documents

* `Upload document`: Uploads a document and starts extraction
* `Upload document (JSON)`: Uploads a document in Base64 JSON format
* `Upload document without extraction`: Uploads a document without starting extraction
* `Upload document page`: Uploads a single page of a document
* `List documents`: Lists documents with optional filtering
* `Get document`: Returns document fields including extracted data
* `Update document`: Updates document fields
* `Delete document`: Removes an existing document
* `Delete documents`: Removes multiple documents
* `Download file`: Downloads the original document file
* `Download exported file`: Downloads an extracted document in a specified export format
* `Get document page image`: Returns an image of a given document page
* `Count documents`: Returns count of documents matching criteria
* `Count extracting documents`: Returns count of documents currently being extracted
* `Get document counts`: Returns document count statistics grouped by status and type
* `Export document`: Triggers document export
* `Export documents`: Triggers export for multiple documents
* `List documents for export`: Returns documents ready for export
* `Mark as exported`: Marks document as exported
* `Mark documents as exported`: Marks multiple documents as exported
* `Mark as export failure`: Marks documents as failed to export
* `Merge and extract documents`: Merges multiple documents into one and extracts
* `Split document`: Splits a document by ID
* `Change document type`: Changes the document class
* `Change document type (batch)`: Changes document class for multiple documents
* `Change document state`: Changes state of document
* `Return to issuer`: Returns document to the sender
* `Rotate page`: Rotates a document page
* `Set tags`: Sets tags on a document
* `Get document history`: Returns document history
* `Get validation errors`: Returns document validation errors
* `Set validation errors`: Manually sets validation errors for a document
* `Validate field`: Validates a document field and returns validation errors
* `Set field values`: Sets values of fields for a document
* `Set field values (batch)`: Sets values of fields for multiple documents
* `Send feedback`: Sends feedback to report an error in a document
* `Get QR payment`: Returns QR code for payment
* `Get company from ARES`: Gets company info from the ARES register
* `Get document exchange rate`: Returns exchange rate for a document
* `Rename document`: Renames a document
* `Attach to another document`: Converts a document into an attachment of another document
* `Update accounting unit`: Changes the accounting unit of a document
* `Requeue extraction`: Reprocesses document extraction
* `Requeue validation`: Requeues document validation
* `Get ABO payment preview`: Returns ABO payment command preview
* `List tags`: Returns all possible tags of the current tenant

### Enums

* `Create enum`: Creates a new enum
* `Return list of all enums`: Returns all enums
* `Return specific enum`: Returns a specific enum by ID
* `Return localized enum`: Returns a specific enum in a given language
* `Update enum`: Updates an enum and all its values
* `Upsert enum values`: Adds or updates selected values of an enum
* `Delete enum`: Deletes an enum by ID
* `Synchronize from accounting system`: Synchronizes enumerations from an external accounting system

### External Data Tables

* `Get all data tables`: Returns all external data tables
* `Create data table`: Creates a new external data table
* `Get data table`: Returns a specific data table by ID
* `Update data table`: Replaces all values in a data table
* `Upsert data table values`: Modifies values of a data table
* `Delete data table`: Deletes a data table
* `Search external table enums`: Searches for possible enum values from an external table

### Extraction

* `Extract document`: Starts document extraction without saving it in the system
* `Extract document (JSON)`: Starts extraction from Base64-encoded JSON content
* `Get extracted document`: Returns the extracted document result

### Notifications

* `Get notifications`: Returns current news and notifications

### Tenant

* `Get current tenant`: Returns current tenant information
* `Get account information`: Returns license stats and reported document counts
* `Get license info`: Returns license information
* `Verify license`: Verifies tenant license
* `Get features`: Returns tenant features
* `Update features`: Updates tenant features
* `Get/set color scheme`: Gets or sets the tenant color scheme
* `Get/set prefilled notes`: Gets or sets tenant prefilled notes
* `Get/set deduction start date`: Gets or sets the deduction start date setting
* `Get document limit status`: Checks if the document extraction limit is exceeded
* `Download extraction report`: Downloads extraction report for a given time period
* `Get extraction statistics`: Returns extraction statistics

#### Accounting Systems
* `Create accounting system`: Creates an accounting system configuration
* `Update accounting system`: Updates an accounting system configuration
* `Delete accounting system`: Deletes an accounting system
* `Get accounting systems`: Lists accounting systems of the current tenant
* `Validate accounting system`: Validates an accounting system before creation

#### Approval Workflows (Tenant)
* `Create approval workflow`: Creates a tenant approval workflow
* `Update approval workflow`: Updates a tenant approval workflow
* `Delete approval workflow`: Deletes a tenant approval workflow
* `Get approval workflows`: Lists approval workflows of the current tenant
* `Update approval workflow settings`: Updates approval workflow settings

#### Import Connections
* `Create import connection`: Creates an import connection
* `Update import connection`: Updates an import connection
* `Delete import connection`: Deletes an import connection
* `Get import connections`: Lists import connections of the current tenant

#### Export Settings
* `Update export settings`: Updates export general settings
* `Update export trigger`: Updates export trigger configuration
* `Update export finished action`: Updates action after finished export
* `Update download naming`: Updates document naming when downloading
* `Get/set Pohoda settings`: Gets or sets export configuration for Pohoda
* `Get/set MRP settings`: Gets or sets export configuration for MRP
* `Get/set Duel settings`: Gets or sets export configuration for Duel

#### Email Notifications
* `Get email notification`: Returns tenant email notification settings
* `Create email notification`: Creates tenant email notification
* `Delete email notification`: Deletes tenant email notification

#### Webhooks
* `Create webhook`: Creates a tenant webhook
* `Update webhook`: Updates a tenant webhook
* `Delete webhook`: Deletes a tenant webhook
* `Get webhooks`: Lists tenant webhooks

### Users

* `Create user`: Creates a new user
* `Create trial user`: Creates a trial user
* `Delete trial user`: Deletes a trial user
* `List users`: Lists users by criteria
* `List users (basic)`: Returns a basic list of users
* `Get user`: Finds a user by ID
* `Update user`: Partially updates a specified user
* `Delete user`: Deletes a user
* `Get current user`: Returns current user's info
* `Update current user`: Updates current user's info
* `Change own password`: Changes the password of the currently authenticated user
* `Change user password`: Changes password for a specified user
* `Send invitation`: Sends an invitation to a new user
* `Reset password`: Resets a user's password
* `Send password reset token`: Sends a token for resetting a user's password
* `Get/set web settings`: Gets or sets web application settings
* `Create user grid view`: Creates a user grid view
* `Update user grid view`: Updates a user grid view
* `Delete user grid view`: Deletes a user grid view
* `Get user grid views`: Lists user grid views

## Frequently Asked Questions

### Does Redque connector support full maintenance of Redque system?

Yes, Redque connector includes all of the benefits of Redque API.

## Known Issues and Limitations

There are no known issues so far.
