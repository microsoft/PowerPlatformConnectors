# Asana



Asana is a work management platform that helps teams organize, track, and manage their work. This connector extends the certified Asana connector with task updates, search, section management, subtasks, tags, and project status reporting — closing the biggest capability gaps between Power Automate and competing platforms like Zapier and Make.



## Publisher



### Aaron Mah



## Prerequisites



You need an Asana account (free or paid) and a Personal Access Token (PAT).



1. Sign in to [Asana](https://app.asana.com/)

2. Go to [Developer Console](https://app.asana.com/0/my-apps) (My Settings > Apps > Manage Developer Apps)

3. Under **Personal Access Tokens**, click **Create new token**

4. Name it (e.g., "Power Automate") and click **Create**

5. Copy the token immediately — it is shown only once



**Note:** The Search Tasks operation requires a paid Asana plan (Premium, Business, or Enterprise). All other operations work with the free plan.



## Supported Operations



### Update Task

Updates an existing Asana task. Can change the name, assignee, due date, notes, completion status, and other fields.



### List Tasks in Project

Returns a list of tasks in a specific Asana project. Use opt_fields to control which task fields are returned.



### Search Tasks

Searches for tasks in a workspace using advanced filters including text search, assignee, project, tags, due dates, and modification dates. Requires a paid Asana plan. Rate-limited to 60 requests per minute.



### List Sections in Project

Returns all sections (columns in Board view, groups in List view) in a project.



### Add Task to Section

Adds an existing task to a specific section in a project, effectively moving it to that workflow stage.



### Create Subtask

Creates a new subtask under an existing parent task.



### List Subtasks

Returns all subtasks of a given parent task.



### List Projects

Returns projects in a workspace, optionally filtered by team or archived status.



### Add Tag to Task

Adds an existing tag to a task. Tags are workspace-level labels that can be applied across projects.



### List Tags in Workspace

Returns all tags in a workspace.



### Create Status Update

Creates a status update on a project, portfolio, or goal. Status updates communicate project health to stakeholders.



### List Status Updates

Returns status updates for a project, portfolio, or goal, ordered by creation date (newest first).



### Delete Task

Permanently deletes a task. This action cannot be undone.



## Obtaining Credentials



1. Go to [app.asana.com/0/my-apps](https://app.asana.com/0/my-apps)

2. Under **Personal Access Tokens**, click **Create new token**

3. Name it and click **Create**

4. Copy the token and paste it into the connector's API Key field in Power Automate



## API Documentation



Visit [Asana Developer Docs](https://developers.asana.com/reference/rest-api-reference) for further details.



## Known Issues and Limitations



- **Search Tasks** requires a paid Asana plan (Premium, Business, or Enterprise) and is rate-limited to 60 requests per minute.

- Asana returns **compact representations** by default. This connector includes default `opt_fields` values for rich responses, but you can override them in advanced mode.

- Pagination uses cursor-based offsets via the `next_page.offset` value. To retrieve all results, loop with the offset until `next_page` is null.

- Personal Access Tokens do not expire by default, but can be revoked by the user at any time.

- This connector is complementary to the certified Asana connector. Operations like Create Task, Complete Task, Add Comment, and triggers are available in the certified connector.



## Deployment Instructions



Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.



## License



Distributed under the MIT License.

