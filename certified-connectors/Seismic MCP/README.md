# Seismic MCP Connector for Microsoft 365 Copilot

## Overview

The Seismic MCP connector for Microsoft 365 Copilot integrates the Seismic sales enablement platform directly into the M365 Copilot experience. It enables authenticated Seismic users to access Seismic capabilities — such as content search, answer generation, and content sharing — without leaving their workflow. All results are scoped to content the authenticated user is authorized to access, ensuring data security and compliance. The connector exposes its capabilities as tools through the MCP (Model Context Protocol) specification, and additional tools may be added over time.

## Prerequisites

Tenant must have the _MCP Connector for M365 Copilot_ app installed from Seismic Exchange and tools enabled by a tenant admin.  

## Tools

The following tools are currently available:

### 1. Generate Answers or Summaries by Generative Search

Generates a natural-language answer grounded in Seismic content, along with supporting source documents and citations.

- **Scope**: `seismic.mcp`

### 2. Get Contents by Generative Search

Retrieves and ranks the most relevant content sources for a query without generating an answer. Returns source documents ranked by relevance.

- **Scope**: `seismic.mcp`

### 3. Generate LiveSend Link

Creates a shareable LiveSend link for Seismic content with configurable settings such as expiration, password protection, and engagement tracking.

- **Scope**: `seismic.mcp`

## Authentication

All tools require an authenticated Seismic user. The connector uses OAuth-based authentication with the `seismic.mcp` scope. Results are always scoped to the content the authenticated user is authorized to access.

## Use Cases

| Scenario | Tools Used |
|---|---|
| "Search top 2 documents for Aura Copilot from last month" | Get Contents by Generative Search |
| "Find me the latest sales deck for Q4, get the top content, and generate a LiveSend link" | Get Contents by Generative Search, Generate LiveSend Link |
| "Get me links to our competitive battle cards" | Get Contents by Generative Search |
| "What collateral do we have on product X?" | Generate Answers or Summaries |

## Data Handling

- **Read operations**: Generative search tools are read-only and idempotent. They do not modify any content in the Seismic platform.
- **Write operations**: The LiveSend link tool creates a new shareable link entity. It is not idempotent — each call creates a new LiveSend.
- **Content scoping**: All content access respects the authenticated user's permissions. No content outside the user's authorization scope is returned.
- **No external data leakage**: The connector does not expose external-facing URLs if restricted by tenant configuration.
