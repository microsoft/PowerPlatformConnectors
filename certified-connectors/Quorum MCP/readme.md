# Quorum MCP

Quorum MCP gives your agents access to Quorum's public policy data: bills, regulations, votes, officials, committees and events, plus the other records your Quorum license covers. Agents can count and list records, find one by name, read and quote what a bill or regulation says, and see how bills moved over a period. Access follows the signed-in user's own Quorum permissions.

## Publisher: Quorum Analytics Inc

## Prerequisites

You need a Quorum account whose organization has Quorum MCP access. To find out whether your organization has access, contact your Customer Success Manager.

## Supported Operations

### InvokeServer

Invokes the Quorum MCP endpoint. The agent discovers and calls the tools below through it:

- `quorum_orient`: takes a question in plain words and returns the kinds of Quorum records that can answer it, best first, each with its evidence.
- `quorum_describe`: explains how one kind of record can be filtered, and which filters work.
- `quorum_query`: counts, groups, measures, lists and finds records by name, and checks each figure a second way.
- `quorum_get`: returns one record by its id.
- `quorum_read`: returns cited passages from a record's own text, and says what was not read.
- `quorum_changes`: lists what moved in a set of records over a period: each dated status change, before and after, from Quorum's own history.

## Obtaining Credentials

The connector signs in with OAuth 2.0. When you create the connection, sign in with your own Quorum account. No app registration is needed.

## Getting Started

1. In Microsoft Copilot Studio, open your agent and go to Tools.
2. Select Add a tool, search for Quorum MCP and select it.
3. Create the connection and sign in with your Quorum account.
4. Ask the agent a question about bills, regulations, votes, officials, committees or events.

## Known Issues and Limitations

- The agent sees only the records the signed-in user can see in Quorum.
- Changes are reported as dated status changes. Differences in a text between its versions are not reported.
- A long answer can come back in parts. The agent can ask for the next part.

## Frequently Asked Questions

### Who can use this connector?

Anyone with a Quorum account whose organization has Quorum MCP access.

### How does Microsoft review this connector?

Microsoft's certification team gets a reviewer account through the certification contact; email support@quorum.us.

## Deployment Instructions

To deploy a custom (non-certified) copy of this connector, use the Power Platform connectors CLI (paconn). The certified connector does not need these steps.

1. Install it with `pip install paconn` and sign in with `paconn login`.
2. From this folder, run `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png`.
3. A custom copy also needs its own OAuth client from Quorum; contact support@quorum.us.

## Support

Email support@quorum.us.

## Legal

- Terms: https://www.quorum.us/global-terms
- Privacy policy: https://www.quorum.us/privacy-policy/
