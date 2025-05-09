# Quickbase
Quickbase is an application development platform that unites business and IT teams by enabling problem solvers of any technical background to work together to safely, securely and sustainably create an ecosystem of applications. Quickbase helps businesses accelerate the continuous innovation of unique processes by enabling citizen development at scale across one common platform.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
A paid plan (Team, Business, or Enterprise) or free trial is required

## Obtaining Credentials
In the preferences for your profile, there is a link for Manage User Tokens. On that page, click the button for "+ New user token" and give the token a name. You will also want to assign the token to each app it should have access to. After saving the token, you will be brought back to the list of your user tokens and copy the newly created token value. This token value you will use in the authorization for the connector in the form "QB-USER-TOKEN yourToken".

## Supported Operations
### Apps
#### Create an app
Creates an application in an account. You must have application creation rights in the respective account. Main properties and application variables can be set with this API.
#### Get an app
Returns the main properties of an application, including application variables.
#### Delete an app
Deletes an entire application, including all of the tables and data.
#### Update an app
Updates the main properties and/or application variables for a specific application. Any properties of the app that you do not specify in the request body will remain unchanged.
#### Get app events
Get a list of events that can be triggered based on data or user actions in this application, includes: Email notification, Reminders, Subscriptions, QB Actions, Webhooks, record change triggered Automations (does not include scheduled).
#### Copy an app
Copies the specified application. The new application will have the same schema as the original. See below for additional copy options.
### Tables
#### Get tables for an app
Gets a list of all the tables that exist in a specific application. The properties for each table are the same as what is returned in get table.
#### Create a table
Creates a table in an application.
#### Get a table
Gets the properties of an individual table that is part of an application.
#### Delete a table
Deletes a specific table in an application, including all of the data within it.
#### Update a table
Updates the main properties of a specific table. Any properties of the table that you do not specify in the request body will remain unchanged.
#### Get all relationships
Get a list of all relationships, and their definitions, for a specific table. Details are provided for both the parent and child sides of relationships within a given application. Limited details are returned for cross-application relationships.
#### Create a relationship
Creates a relationship in a table as well as lookup/summary fields. Relationships can only be created for tables within the same app.
#### Delete a relationship
Use this endpoint to delete an entire relationship, including all lookup and summary fields. The reference field in the relationship will not be deleted.
#### Update a relationship
Use this endpoint to add lookup fields and summary fields to an existing relationship. Updating a relationship will not delete existing lookup/summary fields.
### Reports
#### Get all table reports
Get the schema (properties) of all reports for a table. If the user running the API is an application administrator, the API will also return all personal reports with owner's user ID.
#### Get a report
Get the schema (properties) of an individual report.
#### Run a report
Runs a report, based on an ID and returns the underlying data associated with it. The format of the data will vary based on the report type. Reports that focus on record-level data (table, calendar, etc.) return the individual records. Aggregate reports (summary, chart) will return the summarized information as configured in the report. UI-specific elements are not returned, such as totals, averages and visualizations. Returns data with intelligent pagination based on the approximate size of each record. The metadata object will include the necessary information to iterate over the response and gather more data.
### Fields
#### Get fields for a table
Gets the properties for all fields in a specific table. The properties for each field are the same as in get field.
#### Delete fields
Deletes one or many fields in a table, based on field id. This will also permanently delete any data or calculations in that field.
#### Create a field
Creates a field within a table, including the custom permissions of that field.
#### Get field
Gets the properties of an individual field, based on field ID. Properties present on all field types are returned at the top level. Properties unique to a specific type of field are returned under the 'properties' attribute.
#### Update a field
Updates the properties and custom permissions of a field. The attempt to update certain properties might cause existing data to no longer obey the field’s new properties and may be rejected. See the descriptions of required, unique, and choices, below, for specific situations. Any properties of the field that you do not specify in the request body will remain unchanged.
#### Get usage for all fields
Get all the field usage statistics for a table. This is a summary of the information that can be found in the usage table of field properties.
#### Get a field usage
Get a single fields usage statistics. This is a summary of the information that can be found in the usage table of field properties.
### Formula
#### Run a formula
Allows running a formula via an API call. Use this method in custom code to get the value back of a formula without a discrete field on a record.
### Records
#### Delete records
Deletes record(s) in a table based on a query. Alternatively, all records in the table can be deleted.
#### Insert/update records
Insert and/or update record(s) in a table. In this single API call, inserts and updates can be submitted. Update can use the key field on the table, or any other supported unique field. This operation allows for incremental processing of successful records, even when some of the records fail.
#### Query for data
Pass in a query in the [Quickbase query language](https://help.quickbase.com/api-guide/componentsquery.html).
### Auth
#### Get a temporary token for a DBID
Use this endpoint to get a temporary authorization token, scoped to either an app or a table. You can then use this token to make other API calls. This token expires in 5 minutes.
### User Token
#### Clone a user token
Clones the authenticated user token. All applications associated with that token are automatically associated with the new token.
#### Deactivate a user token
Deactivates the authenticated user token. Once this is done, the user token must be reactivated in the user interface.
#### Delete a user token
Deletes the authenticated user token. This is not reversible.
### Files
#### Download an attachment
Downloads the file attachment, with the file attachment content encoded in base64 format. The API response returns the file name in the `Content-Disposition` header. Meta-data about files can be retrieved from the /records and /reports endpoints, where applicable. Use those endpoints to get the necessary information to fetch files.
#### Delete an attachment
Deletes one file attachment version. Meta-data about files can be retrieved from the /records and /reports endpoints, where applicable. Use those endpoints to get the necessary information to delete file versions.

## Known Issues and Limitations
There are no known issues at this time.
