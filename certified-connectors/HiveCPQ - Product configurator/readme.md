# HiveCPQ Product Configurator
HiveCPQ is a comprehensive Configure Price Quote (CPQ) solution that empowers sales teams to generate accurate quotes for complex products. Streamline your sales process by seamlessly integrating HiveCPQ to automate workflows, sync data, and optimize your operations using our connector. Maximize efficiency and deliver exceptional sales experiences with HiveCPQ's powerful connector.

## Publisher: Publisher's Name
NimbleOps NV

## Prerequisites
You have to:
* be an active customer of HiveCPQ
* have access to a HiveCPQ environment

## Supported Operations

### Get manufacturers
Get all manufacturers to which you are connected.

### Search companies
Search companies linked to a manufacturer.

### Add a new company
Add a new company to a manufacturer.

### Get a company
Get a specific company linked to a manufacturer.

### Update a company
Update a specific company of a manufacturer.

### Archive a company
Archive a company together with all of its contacts.

### Unarchive a company
Unarchive a company without unarchiving its contacts.

### Search default addresses of a company
Search the default invoice and delivery addresses of a company.

### Add a new default address
Add a new default address to a company.

### Get a default address of a company
Get a specific default address of a company.

### Delete a default address of a company
Delete a specific default address of a company

### Update a default address of a company
Update a specific default address of a company.

### Search manufacturer contacts
Search contacts linked to a manufacturer or its companies.

### Add manufacturer contact
Add a new contact to a company of a manufacturer or its companies.

### Get a manufacturer contact
Get a specific contact of a manufacturer or its companies.

### Update a manufacturer contact
Update a specific contact of a manufacturer or its companies.

### Archive a contact
Archive a specific contact and remove its invites.

### Search distributor accounts
Search accounts in the address book of a distributor.

### Add a distributor account
Add an account to the address book of a distributor.

### Delete a distributor account
Delete a specific account from the address book of a distributor. An account can only be removed if it is not used in any projects and has no contacts.

### Update distributor account
Update a specific account in the address book of a distributor.

### Search distributor contacts
Search contacts in the address book of a distributor.

### Add a distributor contact
Add a new contact to the address book of a distributor.

### Delete a distributor contact
Delete a specific contact from the address book of a distributor.

### Update a distributor contact
Update a specific contact in the address book of a distributor.

### Search invites
Search invites of a manufacturer.

### Add an invite
Add an invite for a contact to join a company.

### Send invitation email
Send an invitation email using the HiveCPQ email system.

### Search container versions
Search container versions of a manufacturer.

### Search components
Search components in master data.

### Add a component
Add a component to master data.

### Get a component
Get a specific component in master data.

### Delete a component
Delete a component from master data

### Update a component
Update a specific component in master data

### Delete components in bulk
Delete multiple components in master data

### Search component prices
Search prices of a component in master data.

### Add a component price
Add a new price to a component in master data.

### Delete a component price
Delete a price from a component in master data.

### Search component attachments
Search attachments of a component in master data.

### Add a component attachment
Add a new attachment to a component in master data.

### Delete a component attachment
Delete an attachment from a component in master data.

### Search component property values
Search property values of a component in master data.

### Set component property value
Set a single property value on a component in master data.

### Set component property values
Set multiple property values on a component in master data. Existing values are overwritten or deleted.

### Link features to a component
Links one or more features to a component in master data. Existing links will be overwritten or deleted.

### Unlink a feature from a component
Unlinks a feature from a specific component in master data.

### Link a feature to a component
Links one feature to a component in master data.

### Search properties
Search properties in master data.

### Add a new property
Add a new property to master data.

### Delete a property
Delete a specific property from master data

### Update a property
Update a specific property in master data.

### Search feature groups
Search feature groups in master data.

### Add new feature group
Add a new feature group to master data.

### Reorder the feature groups
Reorder the feature groups to change how they are displayed in the product store. All existing groups must be part of the request.

### Get a feature group
Get a specific feature group in master data.

### Delete a feature group
Delete a specific feature group from master data.

### Update a feature group
Update a specific feature group in master data.

### Search features
Search features in a specific feature group in master data.

### Add a feature to a feature group
Add a new feature to a specific feature group in master data.

### Reorder features in a feature group
Reorder the features in a specific feature group to change how they are displayed in the product store. All existing features in the group must be part of the request.

### Get a feature from a feature group
Get a feature in a specific feature group in master data.

### Delete a feature
Delete a feature from a specific feature group in master data

### Update a feature
Update a feature in a specific feature group in master data.

### Change the feature group of a feature
Change the feature group of a specific feature in master data.

### Search units
Search units in master data.

### Add a unit
Add a new unit to master data.

### Get a unit
Get a specific unit from master data.

### Delete a unit
Delete a specific unit from master data.

### Update a unit
Update a specific unit in master data.

### Search custom object types
Search custom object types of a manufacturer.

### Search custom objects
Search custom objects of a specific type.

### Add a custom object
Add a new custom object of a specific type. This type will be created if it does not exist.

### Get a custom object
Get a custom object of a specific type.

### Delete a custom object
Delete a custom object of a specific type.

### Update a custom object
Update a specific custom object of a specific type.

### Reset custom objects cache
Configurators may use caching when handling custom objects. Calling this endpoint will reset those caches.

### Search projects
Search projects of a manufacturer.

### Add a project
Add a new project for a company.

### Get a project
Get a specific project of a manufacturer.

### Update a project
Update a specific project of a manufacturer.

### Add products to a project
Add one or more products to a project of a manufacturer.

### Copy a project
Copies all data of a project to a new project.

### Generate a document for a project
Generate a PDF document for a specific project. The template ID is optional only if the document type is QUOTE and a default template has been configured.

### Add a configuration to a project
Add a new V2 configuration to a project.

### Refresh the prices in running projects
Refresh prices of all projects which have not yet reached step 2 in the order process with master data.

### Search project segments
Search project segments of a manufacturer.

### Get a project segment
Get a specific project segment of a manufacturer.

### Update a project segment
Update a specific project segment of a manufacturer.

### Archive a project segment
Archive a specific project segment of a manufacturer.

### Unarchive a project segment
Unarchive a specific project segment of a manufacturer.

### Move a project segment to step 1
Move a specific project segment to step 1. This starts the order process.

### Move a project segment to step 2
Move a specific project segment to step 2. This creates an order for the project segment.

### Move a project segment to step 3
Move a specific project segment to step 3.

### Move a project segment to step 4
Move a specific project segment to step 4.

### Cancel order process of a project segment
Cancel the order process and move a project segment back to step 2.

### Change the settings of a project segment
Change the settings of a project segment. Settings are only available if a segment configurator is configured.

### Add a configuration to a segment
Add a V1 configuration to a specific project segment.

### Search project segment items
Search project segment items of a specific manufacturer.

### Get project segment item
Get a specific project segment item of a manufacturer.

### Delete a project segment item
Delete a specific project segment item from a project.

### Update a project segment item
Update a specific project segment item of a manufacturer.

### Copy project segment item to another project
Copies a project segment item to another project. If the item has a configuration attached it will automatically be upgraded to the newest version.

### Add a standalone configuration
Create a standalone configuration by launching a configuration check for a V2 configurator

### Get a configuration
Get a specific configuration of a manufacturer.

### Update configuration
Update the values of the nodes in a configuration.

### Get file from a configuration
Get the file attached to a file node in the configuration.

### Search integrated system links
Search integrated system links of a manufacturer. An Integrated System link stores a link between a Hive entity and an external system entity.

### Add integrated system link
Add a new integrated system link for a resource.

### Delete an integrated system link
Delete a specific integrated system link.

### Update a integrated system link
Update a specific integrated system link of a manufacturer.

### Search webhooks
Search webhooks configured for a manufacturer.

### Add a webhook
Add a new webhook for a manufacturer.

### Get a webhook
Get a specific webhook of a manufacturer.

### Delete a webhook
Delete a specific webhook of a manufacturer.

### Update a webhook
Update a webhook of a manufacturer.

### Search webhook headers
Search headers of a specific webhook.

### Add webhook header
Add a new header to a specific webhook.

### Delete the header of a webhook
Delete the header of a specific webhook.

### Search event subscriptions
Search event subscriptions to which you are subscribed.

### Subscribe to events
Subscribe to events of a manufacturer. Existing subscriptions will be overwritten or deleted.

### Search webhook triggers
Search triggers of a specific webhook.

### Add a webhook trigger
Add a new trigger to a specific webhook.

### Get a webhook trigger
Get a specific trigger of a webhook.

### Delete a webhook trigger
Delete a specific trigger of a webhook.

### Update a webhook trigger
Update a specific trigger of a webhook.

### Search price catalogs
Search price catalogs of a manufacturer.

### Delete a price catalog
Delete a specific price catalog.

### Search complaints
Search complaints of a manufacturer

### Add a complaint
Add a new complaint for a manufacturer.

### Get a complaint
Get a specific complaint of a manufacturer.

### Delete a complaint
Delete a specific complaint of a manufacturer.

### Update a complaint
Update a specific complaint of a manufacturer.

### Search complaint comments
Search comments of a specific complaint.

### Add a complaint comment
Add a new comment to a specific complaint.

### Replace complaint comments
Replace all comments of a specific complaint.

### Search complaint attachments
Search attachments of a specific complaint.

### Add a complaint attachment
Add a new attachment to a specific complaint.

### Get a complaint attachment
Get an attachment of a specific complaint.

### Removes the attachment from a complaint
Removes the attachment from a complaint

### Get a user account
Get a specific user account connected to the given manufacturer.

### Search output document templates
Search output document templates of a manufacturer.

### Search roles
Search roles the manufacturer has defined for its own contacts and those of its distributors.

### Search permissions
Search permissions of a manufacturer.

### Search events
Search platform events of a manufacturer.

### Get plugin token information
Get information about the user who is currently using a plugin from within HiveCPQ.

### Search delivery conditions
Search delivery conditions of a manufacturer.

### Search payment conditions
Search payment conditions of a manufacturer.

### Search tax conditions
Search tax conditions of a manufacturer.


## Obtaining Credentials
A request needs to be made to support@hivecpq.com requesting API credentials in the form of a username & password. 
In case of a custom installation you also need to request your own client ID and secret.

## Known Issues and Limitations
Query parameters which allow to pass a list of values will only have the first value respected by the underlying service. This is because of a conflict between the supported formats which powerautomate can use to send multi-value parameters and the format the underlying service accepts.


## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

For custom connector installations please enter the correct client_id, which you need to request from HiveCPQ support, in the properties file of the connector.
