# Affinity MCP Connector

## Overview

Affinity is the relationship intelligence platform built for dealmakers. The Affinity MCP connector brings Affinity's CRM capabilities directly into Microsoft Copilot Studio and Power Automate through the Model Context Protocol (MCP), enabling AI agents to search your network, find warm introduction paths, prep for meetings, and keep deals and CRM records up to date — without leaving your Microsoft 365 workflow.

The connector communicates with Affinity's hosted MCP server at `mcp.affinity.co` using the Streamable HTTP transport (`x-ms-agentic-protocol: mcp-streamable-1.0`).

## Prerequisites

### 1. Affinity account

You need an active Affinity account with access to your organization's CRM data. Contact [Affinity Sales](https://www.affinity.co) if your organization does not yet have an account.

### 2. OAuth credentials

Authentication is handled via OAuth 2.0 using Affinity's identity provider (`login.affinity.co`). When creating a connection in Power Automate or Copilot Studio, you will be redirected to Affinity's login page to sign in and grant access. No API key is required for the Power Platform connector — OAuth handles authentication automatically.

### 3. Power Automate license

Using this connector in Power Automate cloud flows requires a **Power Automate Premium** license. Using it as a tool in Copilot Studio agents requires a **Copilot Studio** license.

## How to get started

1. In Power Automate or Copilot Studio, search for **Affinity MCP** in the connector gallery.
2. Click **Add a connection** and sign in with your Affinity account when prompted.
3. The connector is now available as a tool for your Copilot Studio agents or in your Power Automate flows.

## Supported capabilities

The Affinity MCP connector exposes Affinity's full CRM toolset to AI agents. Tools are discovered dynamically at runtime via the MCP protocol. Capabilities include:

**People & Companies**
- Search persons and companies by keyword, filters, or natural language
- Retrieve detailed profiles, organization associations, and relationship strengths
- Create and update person and company records
- Surface warm introduction paths and coworker connections to a target company

**Opportunities & Deals**
- Search and update opportunities
- Track deal progress and associated entities

**Notes**
- Create, query, and search notes across persons, companies, and opportunities
- Retrieve all notes attached to a specific entity

**Lists & Views**
- Browse and manage CRM lists and list entries
- Access saved views and their configured field data

**Meetings & Interactions**
- Retrieve past and future meetings for entities
- Log meetings, calls, and chat interactions

**Fields & Field Values**
- Read and write custom field values for entities and list entries
- Manage dropdown options and audit field value changes

**Reminders**
- Create, update, retrieve, and delete reminders

**Files & Transcripts**
- Search and retrieve files attached to CRM entities
- Access meeting transcript fragments

## Known issues and limitations

- The connector uses the MCP agentic protocol (`x-ms-agentic-protocol: mcp-streamable-1.0`). It is designed for use with AI agents in Copilot Studio and is not compatible with traditional Power Automate flow builder actions.
- All tools are discovered dynamically at runtime. The set of available tools may expand as Affinity releases new MCP capabilities; no connector update is required to access new tools.
- The connector requires an active Affinity organization. Guest or trial accounts may have limited access to certain capabilities depending on their plan.

## Support

- Documentation: [https://developer.affinity.co/pages/mcp/setup](https://developer.affinity.co/pages/mcp/setup)
- Support: [https://support.affinity.co](https://support.affinity.co)
- Privacy policy: [https://www.affinity.co/legal/privacy-policy](https://www.affinity.co/legal/privacy-policy)
- Website: [https://www.affinity.co](https://www.affinity.co)
