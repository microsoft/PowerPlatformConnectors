# Cognito Forms
Cognito Forms lets you easily build powerful online forms, such as surveys, order forms, registration forms and more. For free.
Please see our [support documentation](https://www.cognitoforms.com/support/63/data-integration/microsoft-power-automate) 
or [contact us](https://www.cognitoforms.com/supportrequest/) for help with our public Power Automate connector.

## Publisher: Cognito Forms

## Prerequisites
A paid subscription is required.

## Supported Operations

### Triggers
Triggers will be sent shortly after the subscribed event occurs.

#### NewEntry

This will be triggered when a new entry is created for the specified form.
The output will be the entry data, including file data and temporary links to files, signatures, and entry documents.

##### Parameters:
- `form`: The name of the form for which new entry events will be subscribed.

#### UpdateEntry

This will be triggered when an entry is updated for the specified form.
The output will be the entry data, including file data and temporary links to files, signatures, and entry documents.

##### Parameters:
- `form`: The name of the form for which updated entry events will be subscribed.

#### EntryDeleted

This will be triggered when an entry is deleted for the specified form.
The output will be the entry data, excluding links.

##### Parameters:
- `form`: The name of the form for which entry deleted events will be subscribed.

### Actions

#### CreateEntry

This action will create an entry for the specified form.
The action will include settable properties for each entry field, excluding some field types.
The output will be the entry data.

##### Parameters:
- `form`: The internal form name or Id

#### EditEntry

This action will update an existing entry for the specified form.
The action will include settable properties for each entry field, excluding some field types.
This action will fail if the entry includes a paid order or the entryId does not exist.
The output will be the updated entry data.

##### Parameters:
- `form`: The internal form name or Id
- `entryId`: The entry number or entry Id for the entry to be updated

#### SetFormAvailability

This action will set the form's limit availability settings.
The `start` and `end` parameters should be blank to make the form available all of the time.
The output will be the updated availability settings.

##### Parameters:
- `form`: The internal form name or Id
- `start`: The datetime at which the form should become available.
- `end`: The datetime at which the form should no longer be available.
- `message`: The message which should be shown to users who try to access the form when outside of the availability period.

#### GetFile

This action will retrieve a file from storage.
The output will be an object containing the file data, the metadata, and a temporary link to the file.

##### Parameters:
- `id`: The file Id for the file to be retrieved.

#### GetDocument

This action will generate and retrieve an entry document using the specified form, entry, and template.
The output will be an object containing the document's file data, the metadata, and a temporary link to the document.

##### Parameters:
- `form`: The internal form name or Id
- `entry`: The entry Id or entry number of the entry for which the document should be generated
- `templateNumber`: The number of the template that should be used to generate the entry document

### Private Operations

These operations are enablers for the public triggers and actions.

#### GetForms

This operation will return a list of forms as objects that contain the `Id`, `InternalName`, and `Name`.
When used for the `form` parameter for actions and triggers, the user should see the `Name`, 
but the `InternalName` should be sent to the API.

#### GetFormSchema

This operation will return the schema for the specified form.
The `input` parameter should be true for actions to exclude read-only properties and false for triggers to include all properties.
The `includeLinks` parameter should be true for all actions and triggers except for EntryDeleted.

##### Parameters:
- `form`: The internal form name or Id
- `input`: A boolean indicating whether the schema returned should exclude properties that cannot be set. False by default.
- `includeLinks`: A boolean indicating whether link properties for files, signatures, and documents should be included in the schema. True by default.

#### UnsubscribeWebhook

This operation will unsubscribe the specified webhook. The webhook will no longer receive any events after all currently
queued events have been sent.

##### Parameters:
- `id`: The Id of the webhook that should be unsubscribed.
- `module`: The webhook's module. This should be set to "forms", as this connector only supports that module.

## Obtaining Credentials
An owner of the organization must log into Cognito Forms and authorize the connection.

## Known Issues
None

## Deployment Instructions
Cognito Forms requires a valid `Client id` and `Client secret` to create a connector.
These properties can be found in the key vault and should only be set in the custom connector's authentication settings.

1. Run:
	```paconn
	paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
	```

2. Login to PowerAutomate
3. Navigate to the new connector
4. Under "Security" update the `Client id` and `Client secret`
5. Update the connector