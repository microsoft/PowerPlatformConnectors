# NotionV2

NotionV2 provides access to Notion workspaces from Power Automate, Power Apps, and Logic Apps. Create and update pages, query databases with filters and sorts, manage content blocks, search across your workspace, and monitor databases for new or updated items using polling triggers. This connector uses the latest Notion API version (2025-09-03) and replaces the original Notion Independent Publisher connector with modern features including polling triggers, Update Page support, and complete response schemas.

## Publisher
### Aaron Mah

## Prerequisites

You need a Notion account (free or paid) and an Internal Integration Token:

1. Log into Notion at [notion.so](https://www.notion.so)
2. Navigate to **Settings & Members → My connections → Develop or manage integrations** (or go directly to [notion.so/profile/integrations](https://www.notion.so/profile/integrations))
3. Click **"New integration"** → select **"Internal"** as the type
4. Name it (e.g., "Power Automate") and select the workspace
5. Under the **Configuration** tab, copy the **Internal Integration Token** (starts with `ntn_` or `secret_`)
6. **Important:** Share specific pages and databases with the integration: open the page in Notion → click the `•••` menu → **Add connections** → select your integration
7. Paste the token into the Power Automate connection dialog

## Supported Operations

### Create a Page
Creates a new page in a Notion database or as a child of an existing page. Supports Markdown content, block arrays, emoji icons, and cover images.

### Get a Page
Retrieves the properties of a Notion page by its ID. Does not return page content — use Get Block Children for content.

### Update a Page
Updates the properties, icon, cover, or archived status of a Notion page.

### Query a Database
Queries pages (items) in a Notion database with optional filters and sorts. Returns a paginated list of matching pages.

### Get a Database
Retrieves the schema and metadata of a Notion database, including its property definitions. Does not return database rows.

### Append Block Children
Appends new content blocks as children of a specified page or block. Use this to add content to an existing page.

### Get Block Children
Retrieves the content blocks of a page or block. Returns only first-level children — check has_children on returned blocks for nesting.

### Search Pages and Databases
Searches all pages and databases shared with the integration. Can filter by object type and sort by last edited time.

### When a database item is created (Trigger)
Triggers when a new item (page) is added to a specified Notion database. Polls periodically using Query Database sorted by creation time.

### When a database item is updated (Trigger)
Triggers when an existing item (page) is modified in a specified Notion database. Polls periodically using Query Database sorted by last edited time.

### Create a Comment
Creates a comment on a Notion page or replies to an existing discussion thread.

### List Comments
Retrieves unresolved comments from a Notion page or block.

### List Users
Retrieves all users in the Notion workspace. Does not include guests.

### Get a User
Retrieves a specific Notion user by their ID.

### Delete a Block
Moves a block to trash (soft delete). The block can be restored. Can also be used to archive pages by deleting the page block.

## API Documentation
Visit [Notion Developer Docs](https://developers.notion.com/reference/intro) for further details on the Notion API.

## Known Issues and Limitations

- **Rate limits:** Notion enforces a rate limit of 3 requests per second per integration token. Exceeding this results in HTTP 429 responses with a `Retry-After` header.
- **Page content not in Get Page:** The Get a Page operation returns page properties only. Use Get Block Children with the page ID to retrieve actual page content.
- **Integration sharing required:** Your integration must be explicitly shared with each page or database it needs to access. Parent page access grants access to all child pages.
- **No webhooks:** Notion does not support webhooks. Both triggers use polling, which means there may be a delay before new or updated items are detected.
- **Block append limit:** The Append Block Children operation accepts a maximum of 100 blocks per request.
- **Pagination:** All list operations return a maximum of 100 items per page. Use the `start_cursor` / `next_cursor` pattern for pagination.
- **Properties schema is dynamic:** The `properties` field in page and database objects has a dynamic schema that depends on the database configuration. Power Automate treats this as a generic object.

## License
Distributed under the MIT License.
