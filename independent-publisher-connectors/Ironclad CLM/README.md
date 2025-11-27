# Ironclad CLM
A powerful custom connector to [Ironclad CLM](https://ironcladapp.com/), enabling users to interact with workflows, records, users, and groups within Ironclad. The connector and its actions are available in Power Automate, Copilot Studio and PowerApps, with the latter two having some limitations. A custom C# script transforms API traffic, making the non-OpenAPI compatible Ironclad API work seamlessly with OpenAPI standards and Microsoft PowerPlatform.

## Publisher: Independent Publisher
Maximilian Henkensiefken (Amadeus IT Group, S.A.) in collaboration with Ironclad

## Prerequisites
- An Ironclad CLM account with APIs enabled (additional charges may apply)
- Credential (client id and secret) from a registered application for the custom connector in the Ironclad Admin panel with correct scopes registered (see below)
- Licensed access to Microsoft Power Platform (Power Automate, Power Apps, or Copilot Studio)

## Supported Operations

Almost all Ironclad api operations are available in the custom connector. For a list of all api operations, visit the [Ironcald Developer Center](https://developer.ironcladapp.com/docs/getting-started)

### Workflow Operations
- **List all Workflow Schemas**: Returns a list of workflow schemas
- **Retrieve a Workflow Schema**: Returns the fields used in the workflow's launch form
- **List all Workflows**: List all workflows in your Ironclad account
- **Create a Workflow Synchronously**: Launch a new workflow synchronously
- **Create a Workflow Asynchronously**: Launch a new workflow asynchronously
- **Retrieve a Workflow**: View the data associated with a specific workflow
- **Update Workflow Metadata**: Update the attributes on a workflow in the Review step
- **Retrieve the Status of an Async Workflow Create Job**: Check the status of an asynchronously created workflow
- **List all Workflow Approvals**: Returns a list of approvals for the workflow
- **Update Approval on a Workflow**: Updates an approval to the specified status
- **Cancel Workflow**: Cancel a workflow
- **Pause Workflow**: Pause a workflow
- **Resume Workflow**: Resume a workflow
- **Revert a Workflow to the Review Step**: Reverts a workflow to the Review step
- **List All Workflow Signers**: Returns a list of workflow signers and their signature status
- **Retrieve the Approval Requests on a Workflow**: Returns a list of workflow approval requests
- **List all Comments on a Workflow**: Return a list of comments on a workflow
- **Create a Comment on a Workflow**: Creates a comment in the workflow's activity feed
- **Retrieve a Comment from a Specified Workflow**: Return a single comment for a specified workflow
- **Retrieve a Workflow Document**: Download a document associated with a specific workflow
- **Create a Workflow Document**: Create a document in the specified workflow attribute
- **Retrieve Email Threads from a Workflow**: List all email threads in the specified workflow
- **Retrieve an Email Thread from a Workflow**: List a single email thread for a specified workflow
- **Retrieve an Attachment from an Email Thread**: Retrieve an attachment from the specified email thread
- **List all Workflow Participants**: Returns a list of workflow participants
- **Retrieve the Turn History on a Workflow**: An array of objects for each turn on a workflow
- **Create a Signed Document on a Workflow**: Upload a fully or partially signed document to a workflow in sign step

### Record Operations
- **List All Records**: View all records in the company, with filtering available
- **Create a Record**: Create a contract record with specified metadata properties
- **Retrieve XLSX Export File of Records**: Export a records report with filtering options
- **Retrieve Record Schemas**: View the schema associated with contract records
- **Retrieve Predictions**: Get status of predictions for smart import records
- **Create a Smart Import Record**: Upload a file to create a record with smart import
- **Upload a Smart Import Record to an existing Import**: Add a file to an existing import
- **Retrieve Record**: View a specific record and its associated data
- **Replace a Record**: Update an existing record with new metadata
- **Update Record Metadata**: Update specific fields on a record
- **Delete a Record**: Delete an existing record
- **Retrieve Record Signed Copy**: View the signed copy of a specific record
- **Create Record Signed Copy**: Create a signed copy for a specific record
- **Remove Record Signed Copy**: Remove the signed copy from a specific record
- **Retrieve an Attachment on a Record**: View an attachment on a specific record
- **Create an Attachment on a Record**: Create an attachment for a specific record
- **Delete an Attachment on a Record**: Remove an attachment from a specific record

### User Operations
- **List all Users**: List all Users via SCIM
- **Create a User**: Create a User via SCIM
- **Retrieve a User**: Retrieve a User via SCIM
- **Replace a User**: Replace a User via SCIM
- **Update a User**: Update a User via SCIM
- **Delete a User**: Delete a User via SCIM

### Group Operations
- **List all Groups**: List all Groups via SCIM
- **Create a Group**: Create a Group via SCIM
- **Retrieve a Group**: Retrieve a Group via SCIM
- **Replace a Group**: Replace a Group via SCIM
- **Update a Group**: Update a Group via SCIM
- **Delete a Group**: Delete a Group via SCIM

## Obtaining Credentials
The Ironclad CLM Custom Connector uses OAuth 2.0 for authentication. To obtain credentials:

1. Log in to your Ironclad account
2. Navigate to Company Settings > API tab
3. Click "Create new app"
4. Enter app name and configure:
   - Title and Description
   - Display Image
   - Grant Types (select "Authorization Code")
   - Redirect URIs
   - Required Resource Scopes
5. Save the client ID and secret securely

Visit the [Ironclad Developer Hub pages on API Authentication](https://developer.ironcladapp.com/reference/authentication-api) for more details.

## Getting Started

### Environments
Ironclad operates in multiple environments:
- Production: ironcladapp.com (majority of customers)
- EU Production: eu1.ironcladapp.com (EU customers)
- Demo: demo.ironcladapp.com

Each environment is supported, but requires registration of separate client applications.

### Connecting the Custom Connector
1. Select your Ironclad instance (Demo, EU1, or Global)
2. Enter your client ID and secret
3. Authorize the connection through Ironclad's login page

## Known Issues and Limitations

### Complex Data Types in Change Operations
When updating workflow or record metadata through change operations (such as Update Workflow Metadata and Update Record Metadata), you must provide values in their complex data type format rather than natural representation. For example:
- Monetary Amount: Use `{"currency": "EUR", "amount": "1598.12"}` instead of "EUR 1,598.12"
- Date: Use "2024-01-31:00:00:00Z" instead of "31st January 2024"
- Address: Use newline characters (\n) to separate address lines
This limitation only applies to change operations and does not affect read operations.

### Synchronous Workflow Creation Limitations
The Create Workflow Synchronously operation has significant limitations:
- Only reliable for simple workflows with basic approver configurations
- Often fails for complex workflows due to the 5-second timeout limit imposed by custom scripts
- For complex workflows, users should instead:
  1. Use the Create Workflow Asynchronously operation
  2. Add a delay action of 10 seconds or more
  3. Check the async job status using Retrieve the Status of an Async Workflow Create Job
  4. Use Retrieve a Workflow to get the created workflow details
  5. Add any workflow documents using Create a Workflow Document

### Dynamic Schema Parsing Limitations
Dynamic schema parsing is not fully supported across all actions:
- For operations involving approver lists, the workflow must be explicit for the script to retrieve the approver list
- When using dynamic values for workflow IDs, schema cannot be fetched as it doesn't exist at runtime
- This particularly affects operations like Update Approval on a Workflow, where the schema must be known in advance

## Frequently Asked Questions

### How do I create a workflow with multiple documents?
For workflows requiring multiple documents, use the asynchronous workflow creation pattern:
1. Create the workflow asynchronously
2. Wait for completion (minimum 10 seconds)
3. Add documents using separate Create Workflow Document operations

### What should I do if my workflow creation times out?
Always use the asynchronous workflow creation pattern for complex workflows or when attaching documents. The synchronous operation is only suitable for simple approval workflows.

## Deployment Instructions
This connector is available as a custom connector in the Microsoft Power Platform. For custom deployment:
1. Ensure you have the necessary Ironclad API credentials
2. Configure the connector with the appropriate environment URL
3. Set up the OAuth 2.0 authentication with your client credentials
