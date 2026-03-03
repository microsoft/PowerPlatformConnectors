# Confluence

Confluence is Atlassian's wiki and knowledge management platform used by teams to create, organize, and collaborate on documentation. This connector enables Power Automate flows to interact with Confluence Cloud — read and create pages, monitor spaces for changes with polling triggers, search content using CQL, and manage comments.

## Publisher
### Aaron Mah

## Prerequisites

You need a Confluence Cloud account. To create an API token:

1. Go to [https://id.atlassian.com/manage-profile/security/api-tokens](https://id.atlassian.com/manage-profile/security/api-tokens) and log in.
2. Click **"Create API token"**, name it (e.g., `Power Automate Connector`), and click **Create**.
3. Copy the token — it is only shown once.

When adding the connection in Power Automate, you will need:
- **Domain**: Your Atlassian subdomain (e.g., `mycompany` from `mycompany.atlassian.net`)
- **Email Address**: Your Atlassian account email
- **API Token**: The token you created above

## Supported Operations

### When a page is created or updated in a space
Returns pages in a Confluence space, sorted by modification date. Use as a polling trigger to detect new or updated pages.

### When a new comment is posted on a page
Returns footer comments on a specific Confluence page. Use as a polling trigger to detect new comments.

### Get a page
Returns a specific Confluence page including its title, body content, version info, and links.

### List spaces
Returns all Confluence spaces the authenticated user can access. Use to discover space IDs for other operations.

### Create a page
Creates a new page in a Confluence space. Supports storage format (XHTML) body content.

### Add a comment to a page
Creates a footer comment on a Confluence page. Can also reply to an existing comment.

### Get a space
Returns details of a specific Confluence space by its ID, including name, key, description, and homepage.

### Update a page
Updates an existing Confluence page's title, body, or status. Requires the current version number for optimistic concurrency. Call "Get a page" first to obtain the current version number, then increment by 1.

### Search content
Searches Confluence content using Confluence Query Language (CQL). Supports full-text search, space filters, type filters, and date range queries.

### List blog posts in a space
Returns blog posts in a specific Confluence space, sorted by creation or modification date.

### Get a blog post
Returns a specific Confluence blog post by its ID, including title, body content, and version info.

## API Documentation
Visit [Confluence Cloud REST API v2](https://developer.atlassian.com/cloud/confluence/rest/v2/intro/) for further details.

## Known Issues and Limitations

- **Polling triggers only**: Confluence Cloud webhooks require Connect/Forge app registration, which is not available for Independent Publisher connectors. This connector uses polling triggers instead.
- **Storage format**: Page and comment bodies use Confluence storage format (XHTML). Users familiar with HTML can use it directly; others can use Power Automate's HTML composing capabilities.
- **Update page requires version number**: When updating a page, you must first call "Get a page" to obtain the current version number, then increment it by 1 for the update request.
- **API rate limits**: Confluence Cloud REST API has rate limits. High-frequency polling may be throttled by Atlassian.
- **Search uses v1 API**: The Search Content operation uses the Confluence v1 REST API (`/rest/api/search`) as no v2 equivalent exists for CQL search.

## License
Distributed under the MIT License.
