# Workast

Workast is a lightweight task and project management tool designed for messaging-platform teams. This connector enables Power Automate users to create, assign, update, and track tasks across Workast spaces — bridging Microsoft Teams workflows with Workast's task management capabilities. It is the first Workast connector for Power Platform, offering more operations than any other integration platform (Zapier has only 2 actions).

## Publisher

### Aaron Mah

## Prerequisites

To use this connector, you need:

1. A **Workast account** with a paid plan — Standard ($39/month) or Professional ($49/month). The free Essential plan does not include API access. A 14-day trial of the Professional plan also works.
2. A **Workast API token**. To generate one:
   - Log in to [my.workast.io](https://my.workast.io/) via your Slack workspace.
   - Click your name (top right) → **Preferences**.
   - Click the **API** section heading.
   - Click **Generate API token**.
   - Copy the token and paste it into the Power Automate connection dialog.

> **Note:** Workast accounts are created via Slack workspace OAuth. You must have a Slack workspace with the Workast app installed. Generating a new token invalidates any previous token for the same account.

For more details, see [How to generate an API token](https://www.workast.com/help/article/how-to-generate-an-api-token/).

## Supported Operations

### Get Current User
Retrieves profile information for the authenticated user. Use this to test the connection or look up your own user ID for task assignment.

### List Spaces
Returns all spaces accessible to the authenticated user. Use this to find space IDs for task operations. Supports filtering by space type (group, personal, direct, template).

### Get a Space
Retrieves details of a specific space by its ID, including name, description, type, and privacy settings.

### List Tasks in Space
Returns tasks within a specific space. Supports filtering by status (open, in progress, completed), sorting by creation or completion date, and pagination via limit/offset.

### Get a Task
Retrieves full details of a single task by its ID, including title, description, status, assignees, dates, and tags.

### Create a Task
Creates a new task in the specified space. Supports setting the title, description, assignees (by user ID or email), start date, and due date. Returns the created task with its assigned ID.

### Update a Task
Updates an existing task's fields. Only include the fields you want to change — title, description, start date, or due date.

### Delete a Task
Permanently removes a task by its ID. This action cannot be undone.

### List Lists in Space
Returns all lists (columns or sections) within a specific space. Lists organize tasks into categories within a space.

### List Members in Space
Returns all members of a specific space. Use this to look up user IDs for task assignment.

### Add Comment to Task
Adds a text comment to an existing task. Use this to log notes, status updates, or contextual information on tasks.

## API Documentation

Visit [Workast Developer Docs](https://developers.workast.com/) for further details on the Workast API.

## Known Issues and Limitations

- **Paid plan required**: API access requires a Standard ($39/mo) or Professional ($49/mo) plan, or an active 14-day trial. Free Essential accounts cannot generate API tokens.
- **Single token per user**: Generating a new API token invalidates the previous one. Only one active token per user account.
- **Slack-dependent accounts**: Workast accounts are provisioned via Slack workspace OAuth — you cannot create a Workast account without a Slack workspace.
- **No API versioning**: The Workast API has no version prefix. Breaking changes could occur without prior notice.
- **Rate limits**: Not explicitly documented. The API may return HTTP 429 (Too Many Requests) under heavy load.
- **Pagination**: List operations use offset/limit pagination. The default limit is 50 results per request.
- **Professional-only features**: Subtasks, custom tags, templates, time tracking, and task dependencies require the $49/mo Professional plan and are not included in this connector version.

## License

Distributed under the MIT License.
