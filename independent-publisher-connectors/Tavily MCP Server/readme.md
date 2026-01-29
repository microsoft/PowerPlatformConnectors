# Tavily MCP Server (Independent Publisher)

The Tavily MCP Server connector enables AI agents to access Tavily's powerful web research capabilities through the Model Context Protocol (MCP). It provides real-time web search, content extraction, site mapping, and crawling tools designed specifically for Large Language Models and AI agents.

## Publisher: Evan Rimer, Tavily Inc.

## Prerequisites

To use this connector, you must have a Tavily account and API key. Sign up at [tavily.com](https://app.tavily.com/home).

## Supported Operations

### Invoke Tavily MCP Server

Invokes the Tavily MCP endpoint, providing access to the following MCP tools:

- **tavily-search**: Search the web for real-time, accurate information optimized for AI consumption
- **tavily-extract**: Extract clean content from specified URLs
- **tavily-map**: Obtain a sitemap starting from a base URL
- **tavily-crawl**: Traverse a website starting from a base URL

## Obtaining Credentials

1. Go to [tavily.com](https://app.tavily.com/home) and create an account
2. Once logged in, your API key will be displayed on your home page
3. Copy the API key and use it when creating a connection

## Known Issues and Limitations

- This connector uses the MCP (Model Context Protocol) and is designed for use with Microsoft Copilot Studio and AI agents
- The connector exposes a single MCP endpoint that dynamically provides access to all Tavily tools

## API Documentation

For more information about Tavily's capabilities, visit the [Tavily documentation](https://docs.tavily.com/).
