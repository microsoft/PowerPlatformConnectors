# Infini Analytics

## Publisher: Infini Analytics

## Overview

Infini Analytics lets you track and monitor the complete lifecycle of your automated processes directly from Power Automate. For every execution of an automation in your organization, you can record its start, intermediate events, warnings, errors, and end — giving you full traceability and audit history in a single platform.

## Prerequisites

- A Microsoft Power Apps or Power Automate plan with the ability to use custom connectors.
- An active [Infini Analytics](https://analytics.infini.es/signup) account.
- An **organization token** from the Infini platform (Settings → Organization).
- The **automation ID** (`automation_id`) of each automation you want to monitor.

## Authentication

This connector uses **API Key** authentication via an HTTP header named `token`.

1. Log in to the [Infini Analytics platform](https://analytics.infini.es/signup).
2. Navigate to **Settings → Organization**.
3. Copy your **Organization Token**.
4. When creating a new connection in Power Automate, paste this token in the **Organization Token** field.

> A missing or invalid token returns **HTTP 401 Unauthorized**.

## Supported Operations

### Register Execution Event (`registerEvent`)

Records an event in the lifecycle of an automation. Use this action at each key point of your flow.

| Event Type | Description |
|---|---|
| `START` | Marks the beginning of an execution |
| `EVENT` | Records a relevant intermediate milestone |
| `WARNING` | Records a non-critical warning (execution continues) |
| `ERROR` | Records a failure or exception — marks the execution as **failed** |
| `END` | Marks the successful end of an execution |

**Input fields:**

| Field | Required | Description |
|---|---|---|
| `automation_id` | Yes | UUID of the automation in the Infini platform |
| `execution_id` | Yes | Unique identifier for this execution (internal ID or timestamp) |
| `event` | Yes | Event type: START, EVENT, WARNING, END or ERROR |
| `description` | No | Free text describing the event |
| `error_id` | No | Error code — only applicable when event is ERROR |
| `error_description` | No | Detailed error message — only applicable when event is ERROR |

## Getting Started

1. Add the **Infini Analytics** connector to your flow.
2. Create a new connection using your Organization Token.
3. At the **start** of your flow, add a *Register Execution Event* action with `event = START`.
4. Add more actions with `event = EVENT` or `event = WARNING` at key points.
5. If an error occurs, add an action with `event = ERROR` and fill in `error_id` and `error_description`.
6. At the **end** of your flow, add a final action with `event = END`.

> **Important:** Use the same `execution_id` value for all events belonging to the same execution.

## Known Issues and Limitations

- `error_id` and `error_description` are optional but should only be used when `event = ERROR`.
- Each `execution_id` must be unique across simultaneous executions of the same automation.
- The API only accepts HTTPS connections (HTTP requests are redirected with HTTP 301).
- Persistent 5XX errors indicate a temporary server issue. Contact [analytics@infini.es](mailto:analytics@infini.es) if the problem persists.
