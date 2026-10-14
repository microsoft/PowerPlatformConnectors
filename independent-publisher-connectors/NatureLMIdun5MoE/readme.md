# NatureLM-Idun-5-MoE

The NatureLM-Idun-5-MoE connector exposes an Azure AI Foundry research agent through the Azure AI Foundry Agent Service OpenAI **responses** protocol. It provides a secure, Microsoft Entra-authenticated connection so that Power Automate, Logic Apps, Power Apps, and custom MCP clients can invoke a Foundry-hosted agent for research tasks such as literature synthesis, document analysis, and web-grounded Q&A.

This connector is tenant-agnostic. You supply your own Foundry resource, project, and agent names, plus your Entra tenant ID, through the connection parameters. No client secret is required.

## Publisher: Alexander Kleine

## Prerequisites

- An Azure subscription with access to an Azure AI Foundry resource.
- A Foundry project that hosts the target agent and the agent deployment itself.
- A Microsoft Entra ID tenant that owns the Foundry resource.
- An Entra bearer token (scope `https://ai.azure.com/.default`). No client secret is required (public client or managed identity).

## Creating the OAuth app (for reviewers / certification team)

Authentication uses Microsoft Entra ID (OAuth 2.0, delegated). To register the app used by this connector:

1. In the Entra admin center, go to **App registrations → New registration**.
2. Set **Supported account types** to *Accounts in any organizational directory (Multitenant)* or your specific tenant, as appropriate.
3. Under **Authentication**, enable **Allow public client flows** (Advanced → Default client type = Yes). No redirect URI or client secret is required for the device-code / authorization-code flow used here.
4. Under **API permissions**, add the delegated permission **Azure AI Foundry** → `https://ai.azure.com/.default` (user_impersonation) and grant admin consent.
5. Note the **Application (client) ID** and **Directory (tenant) ID**; the connector's `tenantId` connection parameter takes the tenant ID and the `tokenScope` parameter takes the scope.

> Note: the connector's `securityDefinitions` in `apiDefinition.swagger.json` already declares the Entra authorization/token endpoints and the `https://ai.azure.com/.default` scope, so the Power Platform connection is built from those values.

## Supported Operations

### CreateResponse

Invoke the agent with a prompt via the OpenAI responses protocol. Set `api-version` to `2025-05-15-preview`, `model` to `model-router` (not the agent name), and `input` to your prompt. The host, project, and agent are taken from the connection parameters (`foundryResource`, `projectName`, `agentName`).

## Obtaining Credentials

Authentication uses Microsoft Entra ID (OAuth 2.0, delegated). Provide the Entra tenant ID in the `tenantId` connection parameter; the connector obtains a bearer token at call time (scope `https://ai.azure.com/.default`). Use Entra device-code (public client) or a managed identity — no client secret is needed.

## Getting Started

1. Create a connection and fill in `tenantId` (your Entra tenant GUID) and, optionally, `tokenScope` (defaults to `https://ai.azure.com/.default`).
2. Call **CreateResponse** with `model: model-router` and your prompt in `input`. The agent responds with the OpenAI responses schema (`output[].content[].text`).

## Known Issues and Limitations

- The agent is bound to a model-router; the request body `model` field must be set to `model-router`.
- The connector targets a single Foundry project/agent deployment per connection.

## Frequently Asked Questions

### Why do I get HTTP 400 invalid_payload?

Because `model` was set to the agent name. It must be `model-router` when the agent is specified in the URL path.

### Do I need a client secret?

No. Use Entra device-code (public client) or a managed identity.
