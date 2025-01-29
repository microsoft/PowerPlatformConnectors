# HubSpot Engagements V2
HubSpot Engagements is used for keeping your CRM records up-to-date on any interactions that take place outside of HubSpot. Activity reporting in the CRM also feeds off of this data.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have an account with [HubSpot](https://app.hubspot.com/signup-hubspot/crm) and be a Super Admin.

## Obtaining Credentials
Once you are logged in to your account, go to Settings -> Account Setup -> Integrations -> Private Apps. You will need to create a private app and assign it only the scopes you will use actions for. Once your private app is created, you will have an access token in the Auth section of the private app.

## Supported Operations
### Create batch of calls
Creates a batch of calls.
### Get batch of calls
Retrieves a batch of calls by internal ID, or unique property values.
### Update batch of calls
Updates a batch of calls.
### Archive batch of calls
Archives a batch of calls by ID.
### List calls
Retrieves a page of calls. Control what is returned via the `properties` query param.
### Create a call
Creates a call with the given properties and return a copy of the object, including the ID.
### Read an object
Retrieves an object identified by `callId`. `callId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param.  Control what is returned via the `properties` query param.
### Archive an object
Moves an object identified by `callId` to the recycling bin.
### Update an object
Performs a partial update of an object identified by `callId`. `callId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two calls
Merges two calls with same type.
### Delete a call - GDPR
Permanently deletes a call and all associated content to follow GDPR. Use optional property `idProperty` set to `email` to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search calls
Retrieves a list of calls.
### Create a batch of emails
Creates a batch of emails.
### Get batch of emails
Retrieves a batch of emails by internal ID, or unique property values.
### Archive batch of emails
Archives a batch of emails by ID.
### Update batch of emails
Updates a batch of emails.
### List emails
Retrieves a page of emails. Control what is returned via the `properties` query param.
### Create an email
Creates a email with the given properties and return a copy of the object, including the ID.
### Get email
Retrieves an object identified by `emailId`. `emailId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param.  Control what is returned via the `properties` query param.
### Archive email
Moves an object identified by `emailId` to the recycling bin.
### Update email
Performs a partial update of an object identified by `emailId`. `emailId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two emails
Merges two emails with same type.
### Delete an email - GDPR
Permanently deletes an email and all associated content to follow GDPR. Use optional property `idProperty` set to `email` to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search email
Retrieve a list of emails.
### Update batch of meetings
Updates a batch of meetings.
### Archive batch of meetings
Archives a batch of meetings by ID.
### Get batch of meetings
Retrieves a batch of meetings by internal ID, or unique property values.
### Create a batch of meetings
Creates a batch of meetings.
### Get meeting
Retrieves an object identified by `meetingId`. `meetingId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param.  Control what is returned via the `properties` query param.
### Archive meeting
Moves an object identified by `meetingId` to the recycling bin.
### Update meeting
Performs a partial update of an object identified by `meetingId`. `meetingId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### List meetings
Retrieves a page of meetings. Control what is returned via the `properties` query param.
### Create meeting
Creates a meeting with the given properties and return a copy of the object, including the ID.
### Merge two meetings
Merges two meetings with same type.
### Delete a meeting - GDPR
Permanently deletes a meeting and all associated content to follow GDPR. Use optional property `idProperty` set to `email` to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search meetings
Retrieve a list of meetings.
### Get batch of notes
Retrieves a batch of notes by internal ID, or unique property values.
### Archive batch of notes
Archives a batch of notes by ID.
### Update batch of notes
Updates a batch of notes.
### Create a batch of notes
Creates a batch of notes.
### List notes
Retrieves a page of notes. Control what is returned via the `properties` query param.
### Create note
Creates a note with the given properties and return a copy of the object, including the ID.
### Get note
Retrieves an object identified by `noteId`. `noteId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param.  Control what is returned via the `properties` query param.
### Archive note
Moves an object identified by `noteId` to the recycling bin.
### Update note
Performs a partial update of an object identified by `noteId`. `noteId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two notes
Merges two notes with same type.
### Delete a note - GDPR
Permanently deletes a note and all associated content to follow GDPR. Use optional property `idProperty` set to `email` to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search notes
Retrieves a list of notes.
### Get batch of postal mail
Retrieves a batch of postal mail by internal ID, or unique property values.
### Archive batch of postal mail
Archives a batch of postal mail by ID.
### Create a batch of postal mail
Creates a batch of postal mail.
### Update batch of postal mail
Updates a batch of postal mail.
### List postal mail
Retrieves a page of postal mail. Control what is returned via the `properties` query param.
### Create postal mail
Creates a postal mail with the given properties and return a copy of the object, including the ID.
### Get postal mail
Retrieves an object identified by `postalMailId`. `postalMailId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param.  Control what is returned via the `properties` query param.
### Archive postal mail
Moves an object identified by `postalMailId` to the recycling bin.
### Update postal mail
Performs a partial update of an object identified by `postalMailId`. `postalMailId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two postal mail
Merges two postal mail with same type.
### Delete a postal mail - GDPR
Permanently deletes a postal mail and all associated content to follow GDPR. Use optional property `idProperty` set to `email` to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search postal mail
Retrieve a list of postal mail.
### Archive batch of tasks
Archives a batch of tasks by ID.
### Create a batch of tasks
Creates a batch of tasks.
### Update batch of tasks
Updates a batch of tasks.
### Get batch of tasks
Retrieves a batch of tasks by internal ID, or unique property values.
### Get task
Retrieves an object identified by `taskId`. `taskId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param.  Control what is returned via the `properties` query param.
### Archive task
Moves an object identified by `taskId` to the recycling bin.
### Update task
Performs a partial update of an object identified by `taskId`. `taskId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### List tasks
Retrieves a page of tasks. Control what is returned via the `properties` query param.
### Create task
Creates a task with the given properties and return a copy of the object, including the ID. Documentation and examples for creating standard tasks is provided.
### Merge two tasks
Merges two tasks with same type.
### Delete a task - GDPR
Permanently deletes a task and all associated content to follow GDPR. Use optional property `idProperty` set to `email` to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search tasks
Retrieve a list of tasks.
### Update batch of communications
Updates a batch of communications.
### Archive batch of communications
Archives a batch of communications by ID.
### Create a batch of communications
Creates a batch of communications.
### Get batch of communications
Retrieves a batch of communications by internal ID, or unique property values.
### List communications
Retrieves a page of communications. Control what is returned via the `properties` query param.
### Create communication
Creates a communication with the given properties and return a copy of the object, including the ID.
### Get communication
Retrieves an object identified by `communicationId`. `communicationId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param.  Control what is returned via the `properties` query param.
### Archive communication
Moves an object identified by `communicationId` to the recycling bin.
### Update communication
Performs a partial update of an object identified by `communicationId`. `communicationId` refers to the internal object ID by default, or optionally any unique property value as specified by the `idProperty` query param. Provided property values will be overwritten. Read-only and non-existent properties will be ignored. Properties values can be cleared by passing an empty string.
### Merge two communications
Merges two communications with same type.
### Delete a communication - GDPR
Permanently deletes a communication and all associated content to follow GDPR. Use optional property `idProperty` set to `email` to identify contact by email address. If email address is not found, the email address will be added to a blocklist and prevent it from being used in the future.
### Search communications
Retrieve a list of communications.

## Known Issues and Limitations
There are no known issues at this time.
