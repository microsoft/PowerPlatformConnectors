# NatureLM-Idun-5-MoE

The NatureLM-Idun-5-MoE connector exposes the QMFI-Research `NatureLM-Idun-5-MoE` research agent through the Azure AI Foundry Agent Service OpenAI responses protocol. It provides a secure, Microsoft Entra-authenticated connection so that Power Automate, Logic Apps, Power Apps and custom MCP clients can invoke the agent for research tasks such as literature synthesis, document analysis, and web-grounded Q&A.

## Publisher: QMFI-Research

## Prerequisites

- An Azure subscription and the QMFI-Research Microsoft Entra tenant (`885f01ab-7364-4484-be0a-231d541c9e7f`).
- Access to the Azure AI Foundry project `qmfi-research-project` and the agent `NatureLM-Idun-5-MoE`.
- An Entra bearer token (scope `https://ai.azure.com/.default`). No client secret is required (public client or managed identity).

## Supported Operations

### CreateResponse

Invoke the agent with a prompt via the OpenAI responses protocol. Set `api-version` to `2025-05-15-preview`, `model` to `model-router` (not the agent name), and `input` to your prompt.

## Obtaining Credentials

Authentication uses Microsoft Entra ID (OAuth 2.0, delegated). Provide the Foundry resource endpoint and the Entra tenant ID in the connection parameters; the connector obtains a bearer token at call time (scope `https://ai.azure.com/.default`). On Termux/Android, acquire a token with the bundled `device_code_login.py` (azure-identity `DeviceCodeCredential`).

## Getting Started

Call `CreateResponse` with `model: model-router` and your prompt in `input`. The agent responds with the OpenAI responses schema (`output[].content[].text`).

## Known Issues and Limitations

- The agent is bound to a model-router; the request body `model` field must be set to `model-router`.
- The connector targets a single Foundry project/agent deployment.

## Frequently Asked Questions

### Why do I get HTTP 400 invalid_payload?

Because `model` was set to the agent name. It must be `model-router` when the agent is specified in the URL path.

### Do I need a client secret?

No. Use Entra device-code (public client) or a managed identity.
