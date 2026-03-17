# Hivelight Connector

Hivelight is a legal practice management platform that helps law firms and legal teams manage matters, tasks, milestones, and workflows. This connector enables Power Automate, Power Apps, and Logic Apps to interact with the Hivelight API.

## Publisher: Hivelight

## Prerequisites

- A Hivelight workspace with an active subscription.
- An API Key generated from your Hivelight workspace settings. See [Authentication Guide](https://developers.hivelight.com/guides/authentication/).

## Supported Operations

### Matters
| Operation | Description |
|---|---|
| **Get Matters** | Retrieve all matters the user has access to, with pagination and filtering. |
| **Get Matter** | Get details of a specific matter by ID, including milestones. |
| **Create Matter** | Create a new matter with optional workflow application. |
| **Update Matter Details** | Update the name, description, or external reference of a matter. |
| **Delete Matter** | Permanently delete an archived matter. |
| **Apply Roadmap to Matter** | Apply a roadmap to a matter. |
| **Apply Task List to Matter** | Apply a task list workflow to a matter. |
| **Update Matter Roles** | Update the team roles assigned to a matter. |
| **Archive Matter** | Archive a matter. |
| **Unarchive Matter** | Restore a previously archived matter. |
| **Create Matter Note** | Add a note to a matter. |
| **Create Task Note** | Add a note to a specific task within a matter. |
| **Update Milestone Status** | Updates the status of a specific milestone in a matter. |
| **Get Matter By External Reference ID** | Retrieves a matter using its External Reference ID. |

### Users
| Operation | Description |
|---|---|
| **Get All Users** | Retrieve all users in the workspace. |
| **Get User by ID** | Get a specific user's details. Use `whoami` for the current user. |

### Tasks
| Operation | Description |
|---|---|
| **Get Tasks** | Retrieve tasks with optional filtering by matter, milestone, due date, and status. |
| **Get Task by ID** | Get details of a specific task. |
| **Update Task Status** | Update the status of a task (TODO, INPROGRESS, INREVIEW, DONE). |

### Workflows
| Operation | Description |
|---|---|
| **Get Workflows** | Retrieve available workflows (roadmaps and task lists). |
| **Get Workflow by ID** | Get details of a specific workflow. |

### Workspace
| Operation | Description |
|---|---|
| **Get Current Workspace** | Retrieve workspace details including available matter types. |

## Supported Triggers

| Trigger | Description |
|---|---|
| **When a Matter is Created** | Fires when a new matter is created. |
| **When a Task is Created** | Fires when a new task is created within a matter. |
| **When a Task Status is Updated** | Fires when a task's status changes. |
| **When a Milestone is Created** | Fires when a new milestone is created. |
| **When a Milestone Status is Updated** | Fires when a milestone's status changes. |

## Obtaining Credentials

1. Log in to your [Hivelight workspace](https://app.hivelight.com).
2. Navigate to **Settings** > **API Keys**.
3. Click **Create API Key**, select a user, and enter a label.
4. Copy the generated key — it will only be shown once.
5. Use this key when creating a connection in Power Automate.

For detailed instructions, see the [Authentication Guide](https://developers.hivelight.com/guides/authentication/).

## API Documentation

Full API documentation is available at [https://developers.hivelight.com](https://developers.hivelight.com).

## Known Issues and Limitations

- The connector uses the **global endpoint** (`api.hivelight.com`). Workspaces hosted in Australia (`au.api.hivelight.com`) are not currently supported through this connector.
- Webhook-based triggers require the Hivelight workspace to have webhook capabilities enabled.
- Dates are represented as milliseconds since Unix epoch (integer format).
- The connector uses the **global endpoint** (`api.hivelight.com`). Workspaces hosted in Australia (`au.api.hivelight.com`) are not currently supported through this connector.

## Support

- **Email**: [support@hivelight.com](mailto:support@hivelight.com)
- **Documentation**: [https://docs.hivelight.com](https://docs.hivelight.com)
- **API Docs**: [https://developers.hivelight.com](https://developers.hivelight.com)
