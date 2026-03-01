# Linear

Linear is a modern issue tracking and project management tool designed for software teams. This connector enables Power Automate users to create, update, query, and search issues in their Linear workspace, enabling seamless integration between Linear and hundreds of other services.

## Publisher

Aaron Mah (Independent Publisher)

## Prerequisites

To use this connector, you need a Linear account with API access:

1. Log into your Linear workspace at [https://linear.app](https://linear.app).
2. Navigate to **Settings → Account → Security**.
3. Click **Create new API key**.
4. Copy the API key (it is shown only once).
5. Paste the key into the Power Automate connection dialog when prompted.

> **Note:** The API key provides full read/write access to your workspace. Keep it secure and do not share it.

## Supported Operations

| Operation | Description |
|---|---|
| **List Teams** | Retrieves all teams in the Linear workspace. |
| **List Workflow States** | Retrieves all workflow states (statuses) in the workspace. |
| **List Users** | Retrieves all users in the Linear workspace. |
| **Create Issue** | Creates a new issue in Linear. |
| **Get Issue** | Retrieves a single issue by ID or identifier. |
| **List Issues** | Lists issues with optional filtering and pagination. |
| **Update Issue** | Updates an existing issue in Linear. |
| **List Labels** | Retrieves all issue labels in the workspace. |
| **Add Comment** | Creates a comment on an existing issue. |
| **Search Issues** | Searches issues by text in title or description. |
| **When an issue is created or updated** | Polls for new or updated issues (trigger). |

## API Documentation

- [Linear Developer Documentation](https://linear.app/developers)
- [Linear GraphQL API Reference](https://linear.app/developers/graphql)
- [Linear API Schema Explorer (Apollo Studio)](https://studio.apollographql.com/public/Linear-API/variant/current/home)

## Known Issues and Limitations

- **GraphQL-only API:** Linear uses a GraphQL API. All operations are POST requests to a single `/graphql` endpoint with query/mutation payloads. The connector abstracts this complexity.
- **Polling trigger latency:** The "When an issue is created or updated" trigger uses polling (not webhooks). There is a 1–5 minute delay between an issue change in Linear and the flow triggering, depending on the polling interval configured in Power Automate.
- **Rate limits:** Linear enforces rate limits on API requests. Complex queries cost more against the rate limit budget. If you encounter rate limit errors (HTTP 429), add delays between actions in your flow.
- **Pagination:** List and search operations return up to 100 items per request. Use the `after` cursor parameter with the `endCursor` value from the response to retrieve additional pages.
- **Label replacement:** When updating labels on an issue via Update Issue, the `labelIds` array replaces all existing labels rather than appending to them.
- **API key scope:** The API key grants full access to the workspace. There is no way to limit it to specific teams or operations.
