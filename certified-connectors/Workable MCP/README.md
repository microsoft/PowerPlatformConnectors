# Workable MCP

[Workable](https://www.workable.com) is a hiring and HR platform used by more than 30,000 companies to post jobs, move candidates through a hiring pipeline, and manage employees once they are hired.

The Workable MCP server gives an agent authenticated, permission-aware access to that data. An agent can answer questions about open roles and applicants, act on a candidate's pipeline position, look up employee and org-chart information, and handle the approval workflows that surround requisitions, offers and time off — all as the signed-in Workable user, and only within what that user is allowed to see and do.

## Publisher: Workable Inc

## Prerequisites

- A Workable account on a plan that includes API access.
- Microsoft Copilot Studio with **generative orchestration** and **MCP tools** enabled in your environment.
- A Workable user account. The tools act on behalf of the signed-in user, so the data and actions available depend on that user's role and permissions in Workable.

## Obtaining Credentials

The server uses OAuth 2.0. No API key, client ID or secret needs to be entered by hand.

When you create the connection, you are redirected to Workable to sign in and consent to the permissions the server requests. Workable issues an access token scoped to your user, and the connection stores it. Tokens are refreshed automatically; you only sign in again if you revoke access or your Workable administrator removes your account.

You can review or revoke the authorization at any time from **Settings > Integrations** in Workable. For the full tool reference, see the [Workable MCP server documentation](https://workable.readme.io/update/reference/workable-mcp-server).

## Getting Started

Add the Workable MCP server as a tool on your agent, create a connection, and sign in when prompted.

**Every tool other than `get_accounts` takes an `account` argument.** A Workable user can belong to more than one account, so the agent must first call `get_accounts` to list the accounts it can reach, then pass the chosen account's `subdomain` as `account` on every subsequent call. Well-behaved orchestrators do this automatically; if you work with a single account it is worth naming it in your agent's instructions so the agent does not have to ask.

Some useful things to try once connected:

- "Which candidates applied for the Senior Backend Engineer role this week, and where are they in the pipeline?"
- "Move candidate 12345 to the interview stage and add a note about their portfolio."
- "Who is out of office in the Berlin office next month?"
- "Show me the requisitions waiting on my approval."
- "Who reports to the VP of Engineering?"

## Supported Operations

The server exposes just over 100 tools. They are grouped as follows; the authoritative list for a given deployment is whatever the server returns from `tools/list`, since an account or deployment may have tools disabled.

| Area | What the agent can do | Representative tools |
| --- | --- | --- |
| Accounts | Discover reachable accounts and the account's custom permission sets | `get_accounts`, `get_permission_sets` |
| Jobs | List, search and inspect jobs, their stages, members, recruiters, application forms and activity history | `get_jobs`, `search_jobs`, `get_job`, `get_job_stages`, `get_job_activities` |
| Candidates | List and inspect candidates; create them; move, copy, relocate, disqualify or requalify them; add comments, evaluations and ratings; manage tags and read attached files | `get_candidates`, `get_candidate`, `create_candidate`, `move_candidate`, `disqualify_candidate`, `revert_disqualification`, `add_comment`, `add_review` |
| Employees | List and inspect employees, read the org chart and employee documents, update employee records | `get_employees`, `get_employee`, `get_orgchart`, `update_employee` |
| Members | List account members, invite new ones, update roles and permissions, enable or remove members | `get_members`, `invite_member`, `update_member`, `delete_member` |
| Requisitions | Read requisitions and their custom attributes; create, update, approve and reject them | `get_requisitions`, `create_requisition`, `approve_requisition`, `reject_requisition` |
| Offers | Read a candidate's offer and approve or reject it | `get_offer`, `get_candidate_offer`, `approve_offer`, `reject_offer` |
| Time off | Read balances, requests and categories; create requests; approve or reject pending ones | `get_timeoff_balances`, `get_timeoff_requests`, `create_timeoff_request`, `update_timeoff_approval` |
| Time tracking | List, create and update time entries, including bulk entry, and clock in or out | `list_time_entries`, `clock_in`, `clock_out`, `bulk_create_time_entries` |
| Performance reviews | Read review cycles, tasks, forms and results; create cycle templates; submit, share and sign reviews | `get_review_cycles`, `list_review_tasks`, `submit_review`, `sign_review` |
| Organization | Read and manage departments, legal entities and work schedules | `get_departments`, `create_department`, `get_legal_entities`, `get_work_schedules` |
| Reporting | Run filtered searches over employees, detailed candidate records and profile updates, after discovering the available fields and filter options | `get_employee_fields`, `search_employees`, `search_candidates_detailed`, `search_profile_updates` |
| Reference data | Read pipeline stages, disqualification reasons, account custom attributes and events | `get_stages`, `get_disqualification_reasons`, `get_account_custom_attributes`, `get_events` |

Read-only tools are annotated as such, and tools that change or remove data are annotated as destructive, so a host that surfaces tool annotations can prompt for confirmation before a write.

## Known Issues and Limitations

- **Tools only.** The server exposes MCP tools. It does not publish MCP resources or prompts, and it does not use sampling or elicitation.
- **Streamable HTTP only.** Requests are `POST` JSON-RPC over the streamable HTTP transport. The deprecated SSE transport is not supported; a `GET` to the endpoint returns `405`.
- **Stateless.** Each request is handled independently. There is no session resumption and the server does not push notifications to the client between calls.
- **Multi-account handling is explicit.** There is no implicit "current account". Omitting the `account` argument fails rather than guessing, which is deliberate: acting on the wrong Workable account is not a recoverable mistake.
- **Permissions are enforced by Workable, not by the server.** A tool call can succeed for one user and be refused for another on the same account. Refusals come back as tool errors describing the restriction; they are not connector faults.
- **Rate limits.** Calls are subject to the Workable API rate limits. A rate-limited call returns an error asking the agent to retry after a short wait.
- **Plan-dependent tools.** HR tools (employees, time off, time tracking, performance reviews) require the corresponding Workable products to be enabled on the account. On an ATS-only account those tools return "not found" or "forbidden" errors.

## Common Errors and Remedies

| Error | Meaning | Remedy |
| --- | --- | --- |
| Unauthorized (401) | The access token is missing, invalid or expired. | Sign in again to refresh the connection. |
| Forbidden (403) | The token is valid, but this user or account may not perform the action. | Check the signed-in user's Workable role and permissions, and confirm the relevant product is enabled on the account. |
| Not found (404) | The resource does not exist or is not visible to this account. | Confirm the `account` subdomain is correct, then re-fetch the list the id came from to get valid ids. |
| Conflict (409) | The resource is in a state that does not allow the action — for example, reverting a candidate who is not disqualified. | Re-read the resource's current state and act accordingly. |
| Unprocessable (422) | The request was understood but could not be processed as given. | Check the argument values against the tool's schema, especially enum-like fields such as stage names and disqualification reasons, which should be read from the account rather than assumed. |
| Rate limited (429) | Too many requests to the Workable API. | Wait briefly and retry. |

## Frequently Asked Questions

**Which Workable account does the agent act on?**
Whichever account subdomain is passed as the `account` argument, out of those returned by `get_accounts`. The agent should call `get_accounts` before its first data call.

**Can the agent see data the signed-in user cannot?**
No. Every call is made with the user's own access token and is subject to the same permissions as the Workable web app.

**Can the agent change data?**
Yes, where the user's permissions allow it — for example moving or disqualifying candidates, approving offers and time off, updating employee records and managing members. Tools that write are annotated as such so the host can ask for confirmation first.

**Is candidate and employee data used for training?**
No. The server reads and writes your Workable account through the Workable API and does not retain content beyond what is needed to serve the request.

## Deployment Instructions

This connector is submitted for Microsoft certification. Once certified, it can be added from the official connector catalog.

## Support

- Documentation: [Workable MCP server reference](https://workable.readme.io/update/reference/workable-mcp-server)
- Support: [Workable Help Center](https://help.workable.com/hc/en-us)
- Terms and conditions: [Workable terms](https://www.workable.com/terms)
