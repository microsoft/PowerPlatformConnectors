# Cohesity Gaia MCP

## Overview

The Cohesity Gaia MCP connector lets Microsoft Copilot Studio agents work with Cohesity Gaia (Data Insights) through the Model Context Protocol (MCP). It exposes a single Streamable HTTP endpoint (`POST /mcp/gaia/mcp`) speaking JSON-RPC 2.0; Copilot Studio negotiates the MCP session at runtime and discovers the available Gaia tools through `tools/list`. All tool calls are authenticated with your existing Cohesity Gaia API key, and inbound headers are forwarded to the Gaia REST API so authorization remains consistent with direct REST usage.

## Publisher: Cohesity

## Prerequisites

- A Cohesity Helios tenant with **Cohesity Gaia (Data Insights)** enabled.
- A Cohesity Gaia API key with the `GAIA_VIEW` privilege.
- Microsoft Copilot Studio with **generative orchestration** and **MCP tools** enabled in your environment.

## Tools

The following tools are exposed by the Cohesity Gaia MCP server and surfaced to Copilot Studio at runtime.

### gaia_datasets

Lists Cohesity Gaia datasets. Acts as the entry point for the other tools — the dataset hex IDs returned here are required by `gaia_dataset_topics`, `gaia_ask`, and `gaia_search`.

- **Default page size:** 20
- **Privilege:** `GAIA_VIEW`

### gaia_dataset_topics

Returns the hierarchical topic tree for a given dataset.

- **Required parameter:** dataset hex ID (from `gaia_datasets`; the display name is **not** accepted)
- **Defaults:** `level: 1`, `numLevels: 2`
- **Privilege:** `GAIA_VIEW`

### gaia_ask

Executes an LLM-backed query across one or more datasets and returns the answer text together with supporting documents and progress identifiers.

- **Required parameters:** `datasetNames` (array of dataset names), `queryString`
- **Privilege:** `GAIA_VIEW`

### gaia_search

Performs an exhaustive search within a single dataset and returns matching documents with pagination tokens.

- **Required parameters:** `datasetName`, `queryString`, `pageSize` (default `10`)
- **Privilege:** `GAIA_VIEW`

## Authentication

Authentication uses an API key passed in the `apiKey` HTTP header (Cohesity Gaia's `ApiKeyAuth` security scheme).

To obtain a key:

1. Sign in to your Cohesity Helios tenant.
2. Generate or retrieve an API key with the `GAIA_VIEW` privilege from the Cohesity Helios console.
3. When adding the connector in Copilot Studio (or creating a connection in Power Apps), paste the API key into the **API Key** field.

The connector forwards the `apiKey` header — and most other inbound HTTP headers — to the Cohesity Gaia REST API, so authorization decisions remain consistent with direct REST API usage.

## Use Cases

| Scenario | Tools Used |
|---|---|
| "List my Gaia datasets and tell me which ones are about HR." | `gaia_datasets`, `gaia_dataset_topics` |
| "Show me the high-level topics covered in the *Customer Contracts* dataset." | `gaia_datasets`, `gaia_dataset_topics` |
| "What does our standard NDA say about confidentiality terms?" | `gaia_datasets`, `gaia_ask` |
| "Find every document in the *Engineering Wiki* dataset that mentions 'rate limit'." | `gaia_datasets`, `gaia_search` |

## Data Handling

- **Read-only operations.** All four tools are read-only; the connector does not modify, create, or delete content in Cohesity Gaia.
- **Permission scoping.** Results are scoped to the datasets and documents authorized by the API key's user, role assignments, and the `GAIA_VIEW` privilege.
- **Header forwarding.** Inbound HTTP headers (including `apiKey`) are forwarded to the Cohesity Gaia REST API to preserve authentication and authorization parity with direct REST usage.
- **Hidden fields.** Operational fields such as `llmId`, `conversationId`, `history`, `metadata`, `uuid`, `statuses`, `sortField`, and `sortOrder` are hidden from the connector surface to keep the agent contract minimal and stable.

## Known Issues and Limitations

- The connector targets the production Cohesity Helios endpoint (`https://helios.cohesity.com`). Customer-owned or on-premises Cohesity clusters are not currently supported through the marketplace listing.
- The Power Apps custom-connector **Test** tab cannot send the `Accept: application/json, text/event-stream` header that MCP requires; tests issued from that tab will fail. Validate end-to-end behavior in Microsoft Copilot Studio.
- Only the four tools listed above are exposed (those marked `x-mcp-tool: true` in Cohesity's Gaia OpenAPI specification). Additional Gaia REST endpoints are not surfaced through this connector.
- MCP **prompts** are not yet supported by Microsoft Copilot Studio; only **tools** and **resources** are.

## Deployment Instructions

Use the [Power Platform Connectors CLI (`paconn`)](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in your environment:

```bash
pip install paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png
```

Then, in Microsoft Copilot Studio, add the MCP tool with the **Server URL** `https://helios.cohesity.com/mcp/gaia/mcp`, **API key** authentication, and header name `apiKey`. Confirm `tools/list` returns `gaia_datasets`, `gaia_dataset_topics`, `gaia_ask`, and `gaia_search`.

For more information about Cohesity Gaia Data Insights, see [https://www.cohesity.com](https://www.cohesity.com).
