# Tavily MCP

Enables AI agents to access Tavily's web research capabilities through the Model Context Protocol (MCP). Provides five tools: search, extract, map, crawl, and research designed for Large Language Models and AI agents.

## Publisher: Evan Rimer, Tavily

## Prerequisites

To use this connector, you must have a Tavily account and API key. Sign up at [tavily.com](https://app.tavily.com/home).

## Supported Operations

### Invoke Tavily MCP Server

Invokes the Tavily MCP endpoint, providing access to the following MCP tools:

- **tavily_search**: Search the web for current information on any topic. Use for news, facts, or data beyond your knowledge cutoff. Returns snippets and source URLs.
- **tavily_extract**: Extract content from URLs. Returns raw page content in markdown or text format.
- **tavily_crawl**: Crawl a website starting from a URL. Extracts content from pages with configurable depth and breadth.
- **tavily_map**: Map a website's structure. Returns a list of URLs found starting from the base URL.
- **tavily_research**: Perform comprehensive research on a given topic or question. Gathers information from multiple sources including web pages, documents, and other resources to answer a question or complete a task.

## Obtaining Credentials

1. Go to [tavily.com](https://app.tavily.com/home) and create an account
2. Once logged in, your API key will be displayed on your home page
3. Copy the API key and use it when creating a connection

## Known Issues and Limitations

- This is an MCP (Model Context Protocol) connector designed for use with AI agents in Microsoft Copilot Studio.
- The connector exposes a single MCP endpoint that dynamically provides access to all five Tavily tools.
- Response schemas are dynamic based on the MCP protocol.

## API Documentation

For more information about Tavily's capabilities, visit the [Tavily documentation](https://docs.tavily.com/).
