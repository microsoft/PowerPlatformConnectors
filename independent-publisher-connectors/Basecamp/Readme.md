# Basecamp

Basecamp is a project management and team communication platform by 37signals. This connector enables Power Automate flows to interact with Basecamp projects, to-do lists, messages, schedule entries, and comments — bridging Basecamp workflows with Microsoft 365.

## Publisher
### Aaron Mah

## Prerequisites

- A **Basecamp account** (free plan works). Sign up at [basecamp.com/signup](https://basecamp.com/signup).
- Your **Account ID**: Log into Basecamp — your URL looks like `https://3.basecamp.com/1234567/...`. The number (`1234567`) is your Account ID.
- When creating a connection, enter your Account ID and sign in via the OAuth popup. Basecamp does not use granular scopes — the token inherits your user permissions.

## Supported Operations

### List Projects
Lists all active projects in the Basecamp account. Optionally filter by status (archived or trashed).

### Get Project
Gets details of a specific project, including its dock with tool IDs for to-do sets, message boards, and schedules.

### List To-do Lists
Lists all to-do lists in a project's to-do set.

### List To-dos
Lists to-do items in a specific to-do list. Optionally filter by status or completion.

### Get To-do
Gets the full details of a specific to-do item by its ID.

### Create To-do
Creates a new to-do item in a specific to-do list with optional description, assignees, and due date.

### Complete To-do
Marks a to-do item as complete.

### List Messages
Lists all messages on a project's message board.

### Get Message
Gets the full details of a specific message by its ID.

### Create Message
Posts a new message to a project's message board.

### List Schedule Entries
Lists all entries on a project's schedule.

### Create Schedule Entry
Creates a new event on a project's schedule.

### Create To-do List
Creates a new to-do list in a project's to-do set.

### Get Schedule Entry
Gets the full details of a specific schedule entry by its ID.

### Create Comment
Adds a comment to any Basecamp recording (to-do, message, schedule entry, etc.).

## API Documentation
Visit [Basecamp API Documentation](https://github.com/basecamp/bc3-api) for further details.

## Known Issues and Limitations

- **Rate limit**: Basecamp enforces 50 requests per 10-second window. Power Automate handles `429 Too Many Requests` retries automatically.
- **Pagination**: Basecamp uses Link header pagination which Power Automate does not natively support. List operations return the first page of results only.
- **No update operations**: Basecamp's PUT endpoints require all fields (omitting a field clears it). Update operations are deferred to avoid accidental data loss.
- **Rich text HTML**: Message and comment content fields accept HTML. Basecamp supports tags like `<strong>`, `<em>`, `<a>`, `<ul>`, `<ol>`, `<li>`, `<blockquote>`, and `<p>`.
- **Token lifetime**: Access tokens expire after 14 days. Power Automate refreshes them automatically.
- **Free plan limit**: The free Basecamp plan is limited to 1 project.

## License
Distributed under the MIT License.
