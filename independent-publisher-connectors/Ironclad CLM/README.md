# Ironclad CLM (Independent Publisher) Custom Connector

## Introduction

The Ironclad CLM (Independent Publisher) Custom Connector is a powerful integration tool designed to seamlessly connect PowerPlatform with IroncladCLM. This connector enables users to interact with workflows, records, users, and groups within Ironclad, providing a robust set of operations for managing contracts and related processes. The connector and it's actions are available in Power Automate, Copilot Studio and PowerApps, tough the latter two with some limitations.

Key features of this connector include:

- Comprehensive support for workflow and repository management
- Extensive record manipulation capabilities
- User and group administration through SCIM protocol
- Dynamic interaction with Ironclad's API, enhanced by a custom script for improved compatibility

The C# script included in this connector plays a crucial role in transforming API traffic, making the non-OpenAPI compatible Ironclad API work seamlessly with OpenAPI standards and therefore MS PowerPlatform. This script executes at runtime, enabling users to interact dynamically with their workflows and records, thus significantly enhancing the connector's functionality and user experience.

## Pre-Requisites

- An Ironclad CLM account with API add-on enabled
- Credential (client id and secret) from a registered application for the custom connector in the Ironclad Admin panel
- Licensed access to Microsoft Power Platform (Power Automate, Power Apps, or Copilot Studio)
## Table of Contents

2. [Supported Endpoints](#supported-endpoints)
   - [Workflow Operations](#workflow-operations)
   - [Record Operations](#record-operations)
   - [User Operations](#user-operations)
   - [Group Operations](#group-operations)
3. [Authentication](#authentication)
   - [Registering an OAuth Client in Ironclad](#registering-an-oauth-client-in-ironclad)
   - [Environments](#environments)
   - [Entering Credentials in the Custom Connector](#entering-credentials-in-the-custom-connector)
4. [Usage Examples](#usage-examples)
   - [Create a Workflow Synchronously](#create-a-workflow-synchronously)
   - [Update Approval on a Workflow](#update-approval-on-a-workflow)
   - [Create a Record](#create-a-record)
  
## Supported Endpoints

>**Note**: Endpoints that are affected by scripts return transformed properties that may look or behave differently to the official [Ironclad API Reference](https://developer.ironcladapp.com/reference/getting-started-api) documentation. This is to ensure compatibility with the OpenAPI standard. For some of the endpoints, you will find examples of use towards the end of this document. They are indicated with * in the table below.

### Workflow Operations

| Operation Name                                      | API | Object   | Method  | Path                                                            | Description                                                                                 | Script Action                                                |
| --------------------------------------------------- | --- | -------- | ------- | --------------------------------------------------------------- | ------------------------------------------------------------------------------------------- | ------------------------------------------------------------ |
| List all Workflow Schemas                           | CLM | Workflow | `GET`   | `/workflow-schemas`                                             | Returns a list of workflow schemas                                                          | Transforms the response to format workflow schemas           |
| Retrieve a Workflow Schema                          | CLM | Workflow | `GET`   | `/workflow-schemas/{Schema}`                                    | Returns the fields used in the workflow's launch form                                       | Transforms the response to format the workflow schema        |
| List all Workflows                                  | CLM | Workflow | `GET`   | `/workflows`                                                    | List all workflows in your Ironclad account                                                 | Transforms the response to add a label property              |
| Create a Workflow Synchronously                     | CLM | Workflow | `POST`  | `/workflows`                                                    | Launch a new workflow synchronously                                                         | Transforms the request to handle file attachments            |
| Create a Workflow Asynchronously                    | CLM | Workflow | `POST`  | `/workflows/async`                                              | Launch a new workflow asynchronously                                                        | Transforms the request to handle file attachments            |
| Retrieve a Workflow                                 | CLM | Workflow | `GET`   | `/workflows/{Workflow}`                                         | View the data associated with a specific workflow                                           | Transforms the response to format workflow attributes        |
| Update Workflow Metadata                            | CLM | Workflow | `PATCH` | `/workflows/{Workflow}/attributes`                              | Update the attributes on a workflow in the Review step                                      | N/A                                                          |
| Retrieve the Status of an Async Workflow Create Job | CLM | Workflow | `GET`   | `/workflows/async/{AsyncJob}`                                   | Check the status of a Workflow you created while using the Create a Workflow Async route    | N/A                                                          |
| List all Workflow Approvals                         | CLM | Workflow | `GET`   | `/workflows/{Workflow}/approvals`                               | Returns a list of approvals for the workflow                                                | N/A                                                          |
| Update Approval on a Workflow                       | CLM | Workflow | `PATCH` | `/workflows/{Workflow}/approvals/{Role}`                        | Updates an approval to the specified status                                                 | N/A                                                          |
| Cancel Workflow                                     | CLM | Workflow | `POST`  | `/workflows/{Workflow}/cancel`                                  | Cancel a workflow                                                                           | N/A                                                          |
| Pause Workflow                                      | CLM | Workflow | `POST`  | `/workflows/{Workflow}/pause`                                   | Pause a workflow                                                                            | N/A                                                          |
| Resume Workflow                                     | CLM | Workflow | `POST`  | `/workflows/{Workflow}/resume`                                  | Resume a workflow                                                                           | N/A                                                          |
| Revert a Workflow to the Review Step                | CLM | Workflow | `PATCH` | `/workflows/{Workflow}/revert-to-review`                        | Reverts a workflow to the Review step                                                       | N/A                                                          |
| List All Workflow Signers                           | CLM | Workflow | `GET`   | `/workflows/{Workflow}/signatures`                              | Returns a list of workflow signers and the status of their signature                        | N/A                                                          |
| Retrieve the Approval Requests on a Workflow        | CLM | Workflow | `GET`   | `/workflows/{Workflow}/approval-requests`                       | Returns a list of approval requests that have taken place on the workflow                   | N/A                                                          |
| List all Comments on a Workflow                     | CLM | Workflow | `GET`   | `/workflows/{Workflow}/comments`                                | Return a list of comments on a workflow                                                     | N/A                                                          |
| Create a Comment on a Workflow                      | CLM | Workflow | `POST`  | `/workflows/{Workflow}/comments`                                | Creates a comment in the specified workflow's activity feed                                 | N/A                                                          |
| Retrieve a Comment from a Specified Workflow        | CLM | Workflow | `GET`   | `/workflows/{Workflow}/comments/{Comment}`                      | Return a single comment for a specified workflow                                            | N/A                                                          |
| Retrieve a Workflow Document                        | CLM | Workflow | `GET`   | `/workflows/{Workflow}/document/{Key}/download`                 | Download a document associated with a specific workflow via a reference to its document key | N/A                                                          |
| Create a Workflow Document                          | CLM | Workflow | `POST`  | `/workflows/{Workflow}/documents/{Attribute}`                   | Create a document in the specified workflow attribute                                       | Transforms the request to handle file attachments            |
| Retrieve Email Threads from a Workflow              | CLM | Workflow | `GET`   | `/workflows/{Workflow}/emails`                                  | List all email threads in the specified workflow                                            | N/A                                                          |
| Retrieve an Email Thread from a Workflow            | CLM | Workflow | `GET`   | `/workflows/{Workflow}/emails/{Email}`                          | List a single email thread for a specified workflow                                         | Transforms the response to add a key property to attachments |
| Retrieve an Attachment from an Email Thread         | CLM | Workflow | `GET`   | `/workflows/{Workflow}/emails/{Email}/attachments/{Attachment}` | Retrieve an attachment from the specified email thread                                      | N/A                                                          |
| List all Workflow Participants                      | CLM | Workflow | `GET`   | `/workflows/{Workflow}/participants`                            | Returns a list of workflow participants                                                     | N/A                                                          |
| Retrieve the Turn History on a Workflow             | CLM | Workflow | `GET`   | `/workflows/{Workflow}/turn-history`                            | An array of objects for each turn on a workflow                                             | N/A                                                          |
| Create a Signed Document on a Workflow              | CLM | Workflow | `POST`  | `/workflows/{Workflow}/upload-signed`                           | Upload a fully or partially signed document to a specified workflow that is in sign step    | N/A                                                          |
### Record Operations

| Operation Name                                     | API | Object | Method   | Path                                                     | Description                                                                                                | Script Action                                                                               |
| -------------------------------------------------- | --- | ------ | -------- | -------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------- |
| List All Records                                   | CLM | Record | `GET`    | `/records`                                 | View all records in the company, with filtering available via query parameters                             | Transforms the response to add counterpartyName and label properties                        |
| Create a Record                                    | CLM | Record | `POST`   | `/records`                                 | Create a contract record by specifying its intended metadata properties                                    | Transforms the request to format record properties and the response to add counterpartyName |
| Retrieve XLSX Export File of Records               | CLM | Record | `GET`    | `/records/export`                          | Export a records report with filtering available via query parameters                                      | N/A                                                                                         |
| Retrieve Record Schemas                            | CLM | Record | `GET`    | `/records/metadata`                        | View the schema associated with contract records, including available record types and metadata properties | Transforms the response to format record schemas                                            |
| Retrieve Predictions                               | CLM | Record | `GET`    | `/records/smart-import`                    | Retrieve status of predictions of specific smart import record or all records in an import                 | N/A                                                                                         |
| Create a Smart Import Record                       | CLM | Record | `POST`   | `/records/smart-import`                    | Upload a file to create a record with smart import and predictions                                         | N/A                                                                                         |
| Upload a Smart Import Record to an existing Import | CLM | Record | `POST`   | `/records/smart-import/{Import}/`          | Upload a file to an existing import and create a record with smart import and predictions                  | N/A                                                                                         |
| Retrieve Record                                    | CLM | Record | `GET`    | `/records/{Record}`                        | View a specific record and its associated data                                                             | Transforms the response to format record properties and schema                              |
| Replace a Record                                   | CLM | Record | `PUT`    | `/records/{Record}`                        | Update an existing record with a new set of metadata                                                       | Transforms the request to format record properties and the response to add counterpartyName |
| Update Record Metadata                             | CLM | Record | `PATCH`  | `/records/{Record}`                        | Update specific fields on a record                                                                         | Transforms the request to format record properties and the response to add counterpartyName |
| Delete a Record                                    | CLM | Record | `DELETE` | `/records/{Record}`                        | Delete an existing record                                                                                  | N/A                                                                                         |
| Retrieve Record Signed Copy                        | CLM | Record | `GET`    | `/records/{Record}/attachments/signedCopy` | View the signed copy associated with a specific record                                                     | N/A                                                                                         |
| Create Record Signed Copy                          | CLM | Record | `POST`   | `/records/{Record}/attachments/signedCopy` | Create a signed copy associated with a specific record                                                     | Transforms the request to handle file attachments                                           |
| Remove Record Signed Copy                          | CLM | Record | `DELETE` | `/records/{Record}/attachments/signedCopy` | Remove the signed copy associated with a specific record                                                   | N/A                                                                                         |
| Retrieve an Attachment on a Record                 | CLM | Record | `GET`    | `/records/{Record}/attachments/{Key}`      | View an attachment associated with a specific record                                                       | N/A                                                                                         |
| Create an Attachment on a Record                   | CLM | Record | `POST`   | `/records/{Record}/attachments/{Key}`      | Create an attachment associated with a specific record                                                     | Transforms the request to handle file attachments                                           |
| Delete an Attachment on a Record                   | CLM | Record | `DELETE` | `/records/{Record}/attachments/{Key}`      | Remove an attachment associated with a specific record                                                     | N/A                                                                                         |
### User Operations

| Operation Name  | API  | Object | Method   | Path                    | Description              | Script Action                                         |
| --------------- | ---- | ------ | -------- | ----------------------- | ------------------------ | ----------------------------------------------------- |
| List all Users  | SCIM | User   | `GET`    | `/scim/v2/Users`        | List all Users via SCIM  | Transforms the response to add a displayName property |
| Create a User   | SCIM | User   | `POST`   | `/scim/v2/Users`        | Create a User via SCIM   | N/A                                                   |
| Retrieve a User | SCIM | User   | `GET`    | `/scim/v2/Users/{User}` | Retrieve a User via SCIM | N/A                                                   |
| Replace a User  | SCIM | User   | `PUT`    | `/scim/v2/Users/{User}` | Replace a User via SCIM  | N/A                                                   |
| Update a User   | SCIM | User   | `PATCH`  | `/scim/v2/Users/{User}` | Update a User via SCIM   | N/A                                                   |
| Delete a User   | SCIM | User   | `DELETE` | `/scim/v2/Users/{User}` | Delete a User via SCIM   | N/A                                                   |
### Group Operations

| Operation Name   | API  | Object | Method   | Path                      | Description               | Script Action |
| ---------------- | ---- | ------ | -------- | ------------------------- | ------------------------- | ------------- |
| List all Groups  | SCIM | Group  | `GET`    | `/scim/v2/Groups`         | List all Groups via SCIM  | N/A           |
| Create a Group   | SCIM | Group  | `POST`   | `/scim/v2/Groups`         | Create a Group via SCIM   | N/A           |
| Retrieve a Group | SCIM | Group  | `GET`    | `/scim/v2/Groups/{Group}` | Retrieve a Group via SCIM | N/A           |
| Replace a Group  | SCIM | Group  | `PUT`    | `/scim/v2/Groups/{Group}` | Replace a Group via SCIM  | N/A           |
| Update a Group   | SCIM | Group  | `PATCH`  | `/scim/v2/Groups/{Group}` | Update a Group via SCIM   | N/A           |
| Delete a Group   | SCIM | Group  | `DELETE` | `/scim/v2/Groups/{Group}` | Delete a Group via SCIM   | N/A           |
## Authentication

The Ironclad CLM Custom Connector uses OAuth 2.0 for authentication. To use this connector, you'll need to register an OAuth client application with Ironclad and obtain the necessary credentials. Here's how to do it:

### Registering an OAuth Client in Ironclad

1. Log in to your Ironclad account.
2. Click on your user profile dropdown at the top right-hand corner and select "Company Settings".
3. Select the "API" tab on the left sidebar.

   > **Note**: The "API" tab will only be visible if you have the API add-on enabled for your instance.

4. Click on the "Create new app" button.
5. In the popup, enter a name for your OAuth application and click "Create app".
6. You will be provided with a client ID and secret. Save these securely, as you won't be able to access the secret again.
7. After closing the modal, fill out the client application details:
   - Title: This will appear in the Authorization Code Grant consent dialog.
   - Description
   - Display Image: This will appear in the Authorization Code Grant consent dialog.
   - Grant Types: Select the eligible OAuth grants for this client - by default, this should be set to "Authorization Code".
   - Redirect URIs: Enter the allowed URIs for the Authorization Code Grant. The URI for the use with Power Platform is "INSERT HERE ONCE KNOWN".
   - Requested Resource Scopes: Select the allowed resource scopes for this client application.

   > **Note**: For the custom connector to work as expected, check the scope table below and add them to your list of scopes in the registered application. The scopes public.records.readSchemas, public.workflows.readSchemas, scim.users.readUsers and scim.groups.readGroups need to be enabled for any scenario.

1. Click "Save Changes" to complete the registration.

| Object    | Read                                                                                                                                                                                                                                                                                                                            | Write                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| --------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Workflows | public.workflows.readWorkflows<br>public.workflows.readApprovals<br>public.workflows.readSignatures<br>public.workflows.readParticipants<br>public.workflows.readComments<br>public.workflows.readDocuments<br>public.workflows.readSchemas<br>public.workflows.readTurnHistory<br>public.workflows.readEmailCommunications<br> | public.workflows.readWorkflows<br>public.workflows.readApprovals<br>public.workflows.readSignatures<br>public.workflows.readParticipants<br>public.workflows.readComments<br>public.workflows.readDocuments<br>public.workflows.readSchemas<br>public.workflows.readTurnHistory<br>public.workflows.readEmailCommunications<br>public.workflows.createWorkflows<br>public.workflows.updateWorkflows<br>public.workflows.updateApprovals<br>public.workflows.uploadSignedDocuments<br>public.workflows.revertToReview<br>public.workflows.cancel<br>public.workflows.pauseAndResume<br>public.workflows.createComments<br>public.workflows.createDocuments |
| Records   | public.records.readRecords<br>public.records.readSchemas<br>public.records.readAttachments<br>public.records.readSmartImportRecords<br>public.export.readReports                                                                                                                                                                | public.records.readRecords<br>public.records.readSchemas<br>public.records.readAttachments<br>public.records.readSmartImportRecords<br>public.records.createRecords<br>public.records.updateRecords<br>public.records.deleteRecords<br>public.records.createAttachments<br>public.records.deleteAttachments<br>public.records.createSmartImportRecords                                                                                                                                                                                                                                                                                                    |
| Users     | scim.users.readUsers                                                                                                                                                                                                                                                                                                            | scim.users.readUsers<br>scim.users.createUsers<br>scim.users.updateUsers<br>scim.users.deleteUsers                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        |
| Groups    | scim.groups.readGroups                                                                                                                                                                                                                                                                                                          | scim.groups.readGroups<br>scim.groups.createGroups<br>scim.groups.updateGroups<br>scim.groups.deleteGroups                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                |

### Environments

Ironclad operates in multiple environments:
- Production: ironcladapp.com (majority of customers)
- EU Production: eu1.ironcladapp.com (EU customers)
- Demo: demo.ironcladapp.com

Each environment operates independently. A client application generated on one environment will only work on that same environment. You'll need to create separate client applications for each environment you plan to use.

### Entering Credentials in the Custom Connector

When you first use the Ironclad CLM Custom Connector, you'll need to enter your credentials. Here's how:

1. When prompted to create a new connection, you'll see a dropdown to select the Ironclad instance you want to connect to (Demo, EU1, or Global).
2. After selecting the instance, you'll need to provide the following information:
   - Ironclad Client Id: Enter the client ID you received when registering your OAuth application.
   - Ironclad Client Secret: Enter the client secret you received when registering your OAuth application.
3. After entering these details, you'll be redirected to the Ironclad login page to authorize the connection.
4. Once authorized, the connector will use these credentials to authenticate API requests to Ironclad.

> Note: The connector uses different authentication endpoints based on the selected instance (demo.ironcladapp.com, eu1.ironcladapp.com, or ironcladapp.com). Make sure you're using the correct client ID and secret for the environment you're connecting to.


## Usage Examples

### Create a Workflow Synchronously

To accomplish this action in the connector, the script is transforming the response from the Retrieve Workflow Schema endpoint to display the launch form fields dynamically. To display them, you first have to select the workflow configuration you would like to launch to retrieve the fields. Because of this, it is not possible to "Enter a custom value" for this field.

### Update Approval on a Workflow

As the Retrieve Workflow Schema endpoint does not retrieve information about  approver roles that are not present in the form, this action requires you to select a workflow that was already launched to retrieve the applicable schema.

To configure this action:

1. Select a workflow from the dropdown list that was already launched with the same workflow configuration and wait for the approval roles to appear in the form;
2. Enter the relevant field values for the approvers, as if you were configuring this for a future workflow; and
3. Replace the workflow value with your dynamic value. You will notice the field labels being converted to system property names. This is normal and your action will execute successfully.

### Create a Record

The Ironclad repository data model support attaching any property to any record. As such, displaying a full dynamic list of properties is not feasible, as the action form would contain all properties in your instance. As such, record properties can be attached in the action form as an array. The script will then convert the content of the form to the schema supported by the Ironclad api. In MS Power Platform, custom connector must have an explicit data type associate with each schema. This means that you must provide a complex representation of your property value. See below for formatting examples for each Ironclad data type.

| Data Type       | Natural Representation                                                 | Complex Representation                                             |
| --------------- | ---------------------------------------------------------------------- | ------------------------------------------------------------------ |
| String          | The Republic of Ireland                                                | The Republic of Ireland                                            |
| Number          | 1342                                                                   | 1342                                                               |
| Email           | sandy.sales@ironcladapp.com                                            | sandy.sales@ironcladapp.com                                        |
| Date            | 31st January 2024                                                      | 2024-01-31:00:00:00Z                                               |
| Monetary Amount | EUR 1,598.12                                                           | {<br>"currency": "EUR",<br>"amount": "1598.12"<br>}                |
| Address         | 1233 Howard Street<br>San Francisco, California 94103<br>United States | 1233 Howard Street\nSan Francisco, California 94103\nUnited States |
Refer to [this Ironclad API Reference document]([Create a Record (ironcladapp.com)](https://developer.ironcladapp.com/docs/create-a-record-1)) for more details.

### List All Records

The List All Records operation allows you to retrieve records from your Ironclad repository with several options for filtering and formatting the response:

1. Basic listing with default parameters:
    - Returns 25 records per page
    - Sorted by last updated date in descending order
    - Includes basic record metadata
2. Filtered listing with properties:
    - Use the Record Properties parameter to specify which properties to include in the response
    - Properties must be provided as a comma-separated list
    - The response will include a formatted schema specific to the requested properties

Example configurations:

The response includes:

- record ID and metadata
- formatted properties based on your request
- the Counterparty Name at the root level (if available)
- a display label combining the Ironclad ID and name
- attachments formatted as an array for easier processing

> Note: When requesting specific properties, the response includes only those properties that are available in your repository schema
