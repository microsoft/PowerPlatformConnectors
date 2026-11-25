# Hive MCP

[Hive](https://hive.com/) is an AI-native project management, resource planning, and collaboration platform built for teams. The **Hive MCP** server exposes Hive's production Model Context Protocol (MCP) Streamable HTTP endpoint (`https://hive-mind.hive.com/mcp`) so agents can securely discover and invoke Hive tools under the signed-in user's workspace permissions.

## Publisher

**Hive Technology, Inc.** — Publisher and operator of [hive.com](https://hive.com/) and `https://hive-mind.hive.com/mcp`.

## Prerequisites

- An active [Hive](https://app.hive.com/) account with access to one or more Hive workspaces.
- OAuth 2.0 authorization granted during connection setup (`mcp:read` and `mcp:write` scopes).

## Supported Operations

Hive MCP uses the Model Context Protocol Streamable HTTP transport (`POST /mcp`, `x-ms-agentic-protocol: mcp-streamable-1.0`). Tools, schemas, and annotations (`title`, `readOnlyHint`, `destructiveHint`, `idempotentHint`, `openWorldHint`) are discovered dynamically at runtime via `tools/list`.

Key tool areas available to agents include:

- **Projects & Portfolios:** List, search, create, and update projects, parent-child project hierarchies, and project metadata.
- **Actions & Subactions:** Search, filter, create, assign, schedule, comment on, and update status, priority, labels, and custom fields on actions and subactions.
- **Goals & OKRs:** Read and track workspace goals, key results, and progress updates.
- **Notes & Meeting Context:** Search, read, create, and update Hive Notes and linked meeting context.
- **Messages & Collaboration:** Search workspace chats and threads, read conversation history, and post updates.
- **Time Tracking & Resourcing:** Query timesheets, time entries, workspace members, and team availability.
- **Files & Workspace Data:** Browse workspace entities, search records, and prepare bounded file library uploads.

Every tool call executes strictly under the authenticated Hive user's role and workspace access controls.

## Obtaining Credentials

Authentication uses **OAuth 2.0 Authorization Code flow with PKCE (`S256`)** against Hive's authorization server (`https://auth.hive.com`):

1. Add the **Hive MCP** connection to your agent.
2. Select **Sign in** and authenticate with your Hive account (`app.hive.com`).
3. Review and approve the requested scopes (`mcp:read`, `mcp:write`) on the Hive consent screen.
4. Once approved, access tokens and refresh tokens are managed automatically.

## Known Issues and Limitations

- **Workspace Scoping:** Most Hive tools require a `workspaceId`. Agents should list available workspaces first when a workspace ID is not already known.
- **Permissions Boundary:** Tools only return projects, actions, notes, and messages visible to the signed-in Hive user. Destructive or ledger-posting tools carry `destructiveHint: true` annotations so agents can request user confirmation before execution.
- **Binary File Uploads:** File binary content is never sent over JSON-RPC; file library uploads use signed upload URLs returned by preparation tools.
- **Rate Limits:** Standard Hive API and workspace rate limits apply per authenticated user.

## Support and Legal

- **Support:** [https://help.hive.com/en/](https://help.hive.com/en/) (`help@hive.com`)
- **Privacy Policy:** [https://hive.com/policy-documents/privacy-policy](https://hive.com/policy-documents/privacy-policy)
- **Terms of Service:** [https://hive.com/policy-documents/terms-of-service](https://hive.com/policy-documents/terms-of-service)
