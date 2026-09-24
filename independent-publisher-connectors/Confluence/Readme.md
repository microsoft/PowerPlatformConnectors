# Confluence

Confluence Cloud is Atlassian's team wiki and knowledge management platform. This connector enables Power Automate makers to read pages, create and update content, post comments, search using Confluence Query Language (CQL), and access blog posts — all without leaving Power Automate.

## Publisher
### Aaron Mah

## Prerequisites

You need a Confluence Cloud instance (e.g., `yourcompany.atlassian.net`) and an Atlassian account with appropriate permissions.

1. Go to [https://id.atlassian.com/manage-profile/security/api-tokens](https://id.atlassian.com/manage-profile/security/api-tokens) and log in.
2. Click **"Create API token"**, name it (e.g., `Power Automate Connector`), and click **Create**.
3. Copy the token — it is only shown once.
4. In Power Automate, when adding the Confluence connection, enter:
   - **Domain**: Your Atlassian subdomain (e.g., `mycompany`)
   - **Email Address**: Your Atlassian account email
   - **API Token**: The API token you just copied

A free Confluence plan is available at [https://www.atlassian.com/software/confluence/pricing](https://www.atlassian.com/software/confluence/pricing).

## Supported Operations

### Get a page
Returns a specific Confluence page including its title, body content, version info, and links.

### List spaces
Returns all Confluence spaces the authenticated user can access. Use to discover space IDs for other operations.

### Create a page
Creates a new page in a Confluence space. Supports storage format (XHTML) body content.

### Add a comment to a page
Creates a footer comment on a Confluence page. Can also reply to an existing comment by providing a parent comment ID.

### Get a space
Returns details of a specific Confluence space by its ID, including name, key, description, and homepage.

### Update a page
Updates an existing Confluence page title, body, or status. Requires the current version number incremented by 1. Call "Get a page" first to obtain the current version number.

### Search content
Searches Confluence content using Confluence Query Language (CQL). Supports full-text search, space filters, type filters, and date range queries. Example CQL: `type=page AND space=ENG AND text~"onboarding"`.

### List blog posts in a space
Returns blog posts in a specific Confluence space, sorted by creation or modification date.

### Get a blog post
Returns a specific Confluence blog post by its ID, including title, body content, and version info.

## API Documentation

Visit [Confluence Cloud REST API v2](https://developer.atlassian.com/cloud/confluence/rest/v2/intro/) for further details.

For CQL syntax reference, see [Advanced Searching using CQL](https://developer.atlassian.com/cloud/confluence/advanced-searching-using-cql/).

## Known Issues and Limitations

- **Rate Limits**: Confluence Cloud REST API enforces rate limits. If you receive HTTP 429 responses, reduce your flow's polling frequency or add delays between operations.
- **Storage Format**: Write operations (Create Page, Update Page, Add Comment) use storage format, which is XHTML-based (e.g., `<p>Hello world</p>`). Plain text is not directly supported in the body.
- **Update Page Versioning**: The Update Page operation requires the `version.number` field set to the current version + 1. Always call "Get a page" first to get the current version number.
- **Search API Version**: The Search Content operation uses the v1 REST API (`/rest/api/search`) since no v2 equivalent is available. All other operations use the v2 API.
- **ID Format**: The API returns IDs as strings in responses but expects integers in URL path parameters.
- **Pagination**: List operations support pagination via cursor (v2) or start offset (v1 search). Use the `_links.next` value or increment `start` to retrieve additional pages of results.
- **No Triggers**: This connector version provides actions only. Use a Recurrence trigger with the Search Content action to approximate polling behavior.

## License

Distributed under the MIT License.
