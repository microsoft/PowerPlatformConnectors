# Airtable

Airtable is a collaborative platform that combines spreadsheet simplicity with database power. This connector enables Power Automate users to discover bases and tables dynamically, perform full CRUD operations on records with formula filtering and sorting, manage comments for team collaboration, and trigger flows automatically when new records are created.

## Publisher

### Aaron Mah

## Prerequisites

You need an Airtable account (free or paid). To create a Personal Access Token:

1. Log in to Airtable at [https://airtable.com](https://airtable.com)
2. Navigate to [https://airtable.com/create/tokens/new](https://airtable.com/create/tokens/new)
3. Name the token (e.g., `Power Automate Connector`)
4. Add scopes: `data.records:read`, `data.records:write`, `data.recordComments:read`, `data.recordComments:write`, `schema.bases:read`, `user.email:read`
5. Under "Access", select the bases this connector should access (or all bases)
6. Click **Create token** and copy the value immediately (shown only once)

## Supported Operations

### List Bases
Lists all bases accessible to the authenticated user.

### List Tables
Lists all tables and their field schemas in a base.

### List Records
Lists records in a table with optional filtering, sorting, and pagination.

### Get Record
Retrieves a single record by its ID.

### Create Record
Creates a new record in a table.

### Update Record
Updates specific fields on an existing record without clearing unspecified fields.

### Delete Record
Permanently deletes a record from a table.

### Get Current User
Returns information about the authenticated user and their token scopes.

### When a Record Is Created (Trigger)
Triggers when a new record is created in the specified table.

### Create Comment
Adds a comment to a specific record.

### List Comments
Lists all comments on a specific record.

## API Documentation

Visit [Airtable Developer Docs](https://airtable.com/developers/web/api/introduction) for further details.

## Known Issues and Limitations

- **Rate limits**: Airtable enforces 5 requests per second per base and 50 requests per second per user. After a 429 response, wait at least 30 seconds.
- **Page size**: List Records returns a maximum of 100 records per page. Use the `offset` parameter for pagination.
- **Dynamic fields**: Record `fields` are dynamic — each table has different field names and types. Use the "Parse JSON" action to work with specific fields in Power Automate.
- **Trigger detection**: The polling trigger detects new records only (based on `createdTime`). Detecting updated records requires a "Last Modified" field and formula filtering.
- **Monthly API limits**: Free plan is limited to 1,000 API calls per month. Team plan allows 100,000 per month.
- **Formula syntax**: The `filterByFormula` parameter uses Airtable's formula language, which differs from standard expressions.

## License

Distributed under the MIT License.
