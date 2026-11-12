# 1ToolRing (Independent Publisher)

## What the service is

1ToolRing is a pay-per-call tool API for AI agents, made by Khaisa Studio (Pontianak, Indonesia). One API key and one prepaid balance give a caller access to 3,051 tools from 99 third-party providers: search, maps, shopping, market data, web pages, images and more. Every call shows its price before it runs, a failed call is never charged, and there is no subscription. Site: https://1toolring.com. Tool catalog with prices: https://1toolring.com/tools/.

## Why a Power Platform connector

Copilot Studio, Power Automate and Power Apps makers can already reach any MCP server, but there is no first-class connector for a general "find a tool, read its price, run it" API. A connector gives makers three named operations they can drop into an agent or a flow, with the price visible in the action itself, instead of hand-writing HTTP requests and parsing errors.

## Proposed operations

All operations are REST, JSON, and authenticated with an API key sent in the `Authorization: Bearer <key>` header (or the built-in connectivity to the 1ToolRing OAuth 2.1 server).

1. `DiscoverTools` - POST /api/v1/tools/discover: describe the job in words, get back matching tools with their ids and prices.
2. `InspectTool` - POST /api/v1/tools/inspect: read a tool's input schema and exact price before calling it.
3. `RunTool` - POST /api/v1/tools/run: run the chosen tool with its inputs and get the result plus the amount charged.
4. `GetToolRun` - GET /api/v1/tools/runs/{id}: fetch a previous run by id.
5. `SearchPricing` - GET /api/v1/pricing/tools/search?q=: free text search over the public price list.

The same service is also available as a remote MCP server at https://mcp.1toolring.com/mcp, published as `com.1toolring/mcp` in the official MCP Registry.

## Authentication

- API key: create one at https://app.1toolring.com/keys and send it as `Authorization: Bearer <key>`. This is what the connector will use (apiKey, header).
- OAuth 2.1 is also supported by the MCP endpoint (DCR + PKCE S256).

## Pricing and availability

Sign-up is open to everyone. A verified email address gets a one-time Rp10.000 starter balance; top-ups start at Rp10.000 by Indonesian bank virtual account, Alfamart or Indomaret, or $1 by card. Calls are charged per call from the prepaid balance; failed calls are not charged.

## What I can provide during certification

- Complete OpenAPI 2.0 definition and apiProperties.json.
- A test API key with a funded balance for the certification reviewers.
- Public documentation at https://docs.1toolring.com/overview/ (agent index: https://docs.1toolring.com/llms.txt) and a public MIT package at https://github.com/khmuhtadin/1toolring-mcp.

## Publisher

Khaisa Studio, Pontianak, Indonesia. Contact: hello@1toolring.com. Submitted as an Independent Publisher connector by the service owner.
