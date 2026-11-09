# Agent Nexus
Agent Nexus is a live-monitored registry of APIs, MCP servers and CLIs for AI agents. Use it to find the right external service for a task and to check whether that service is currently up.

## Publisher: Agent Nexus

## Prerequisites
A free Agent Nexus API key from https://agentnexus.app/free-key (1,000 calls/day free).

## Supported Operations
### Find a service for a task
Returns APIs, MCP servers or CLIs able to perform a task described in plain words, ranked by relevance and live reliability.
### Search the registry
Keyword search over approved registry entries.
### Check live status
Returns the latest monitored health of one registry entry.

## Obtaining Credentials
Request a free key at https://agentnexus.app/free-key and paste it when creating the connection.

## Known Issues and Limitations
Free plan is limited to 1,000 calls per day. Agent Nexus is a discovery registry: it does not proxy calls to the listed services.

## Validation status
The three HTTPS operations were checked on October 30, 2026: discovery for `send email`, registry search for `brave`, and live status for `brave-search-api` returned HTTP 200. Discovery exposes nested `call` and `trust` fields, and status exposes nested `entry` health fields.

Import, connection creation with a private key, and operation testing inside a Power Platform environment remain pending. HTTP checks are not a substitute for that certification test. The publisher name and the independent-publisher title convention must be confirmed during certification; no approval or public availability is implied.
