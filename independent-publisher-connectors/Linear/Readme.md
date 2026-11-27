# Linear

Linear is a modern issue tracking and project management tool designed for software teams. This connector enables Power Automate users to create, update, query, and search issues in their Linear workspace, enabling seamless integration between Linear and hundreds of other services.

## Publisher

### Aaron Mah

## Prerequisites

To use this connector, you need a Linear account with API access:

1. Log into your Linear workspace at [https://linear.app](https://linear.app).
2. Navigate to **Settings → Account → Security**.
3. Click **Create new API key**.
4. Give it a label (e.g., "Power Automate").
5. Copy the API key (it is shown only once — starts with `lin_api_`).
6. Paste the key into the Power Automate connection dialog when prompted.

> **Note:** The API key provides full read/write access to your workspace. Keep it secure and do not share it. Linear API keys are free on all plans.

## Obtaining Credentials

1. Log into your Linear workspace at [https://linear.app](https://linear.app).
2. Navigate to **Settings → Account → Security**.
3. Click **Create new API key**.
4. Give it a label (e.g., "Power Automate").
5. Copy the API key (it is shown only once — starts with `lin_api_`).
6. Paste the key into the Power Automate connection dialog when prompted.

API keys are free on all Linear plans.

## Supported Operations

### List Teams
Retrieves all teams in the Linear workspace. Use this to get team IDs for creating issues and to power team dropdowns in your flows.

### List Workflow States
Retrieves all workflow states (statuses) in the workspace, such as Backlog, In Progress, and Done. Use this to get state IDs for creating or updating issues.

### List Users
Retrieves all users in the Linear workspace. Use this to get user IDs for assigning issues.

### List Labels
Retrieves all issue labels in the workspace. Use this to get label IDs for tagging issues during creation or update.

### Create Issue
Creates a new issue in a Linear team. Supports setting title, description, assignee, priority, due date, estimate, workflow state, and labels.

### Get Issue
Retrieves a single issue by its UUID or human-readable identifier (e.g., "ENG-42"). Returns full issue details including state, assignee, labels, and timestamps.

### List Issues
Lists issues sorted by last updated time with pagination support. Returns issue summaries including state, assignee, team, and priority.

### Update Issue
Updates properties of an existing issue. Supports changing title, description, state, assignee, priority, due date, estimate, and labels.

### Add Comment
Creates a comment on an existing issue. Supports markdown formatting. Useful for posting automated updates like "PR merged" or "Build failed".

### Search Issues
Searches issues by text across titles and descriptions. Supports pagination. Use this to find existing issues before creating duplicates.

## API Documentation

- [Linear Developer Documentation](https://linear.app/developers)
- [Linear GraphQL API Reference](https://studio.apollographql.com/public/Linear-API/variant/current/home)

## Known Issues and Limitations

- **GraphQL-only API:** Linear uses a GraphQL API. All operations are POST requests to a single `/graphql` endpoint with query/mutation payloads. The connector abstracts this complexity.
- **Rate limits:** Linear enforces rate limits on API requests. Complex queries cost more against the rate limit budget. If you encounter rate limit errors (HTTP 429), add delays between actions in your flow.
- **Pagination:** List and search operations return up to 100 items per request. Use the `After Cursor` parameter with the `End Cursor` value from the response to retrieve additional pages.
- **Label replacement:** When updating labels on an issue via Update Issue, the `labelIds` array replaces all existing labels rather than appending to them.
- **API key scope:** The API key grants full access to the workspace. There is no way to limit it to specific teams or operations.
- **No filtering on list operations:** List Issues returns all issues in the workspace (paginated). Server-side filtering by team or state is not available in this version due to GraphQL optional parameter limitations. Use Power Automate's Filter Array action to filter results client-side.

## License

Distributed under the MIT License.
