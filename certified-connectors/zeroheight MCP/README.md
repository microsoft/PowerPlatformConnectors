# zeroheight

The zeroheight connector lets Microsoft Copilot Studio agents access zeroheight design system documentation through the Model Context Protocol (MCP). Agents discover tools at runtime and use them to list styleguides, browse pages, retrieve component guidance, and search across design system content.

## Publisher: zeroheight

## Prerequisites

- A zeroheight account with access to at least one styleguide.
- Microsoft Copilot Studio with **generative orchestration** and **MCP tools** enabled in your environment.

## Supported Operations

### list-styleguides

Lists all design system styleguides accessible to the authenticated user. Use this first to discover available styleguide IDs.

### list-pages

Returns the full hierarchical navigation tree for a styleguide — categories, pages, and tabs — so agents can locate the right page before fetching content. Accepts an optional `releaseId` to scope results to a specific release.

### list-releases

Lists all available releases (versions) for a styleguide, newest first. Use this when the user asks about a specific version of the design system.

### search-pages

Performs a semantic search across styleguide pages and returns results ordered by relevance. Faster than walking the full navigation tree when you know the component or topic name. Requires AI features to be enabled for the zeroheight organisation.

### get-page

Returns the full content of a styleguide page, including usage notes, component guidance, prop and variant documentation, and embedded asset references. Accepts an optional `releaseId`.

### get-page-asset

Fetches images or file attachments referenced in a page, using the `zeroheight://image/...` or `zeroheight://attachment/...` URIs returned by `get-page`.

## Obtaining Credentials

Authentication uses OAuth 2.0. A zeroheight account with access to at least one styleguide is required. No API keys or additional credentials are needed.

When adding the connector in Copilot Studio:

1. Select **zeroheight** from the connector gallery.
2. Click **Sign in** — you will be redirected to zeroheight to authenticate.
3. Grant the connector read access to your styleguides and pages.
4. Once authorized, you will be returned to Copilot Studio and the connection will be active.

The connector will only surface the styleguides and pages that the authenticated user has permission to view in zeroheight. Contact [support@zeroheight.com](mailto:support@zeroheight.com) if you need help confirming access.

## Getting Started

1. In Microsoft Copilot Studio, open your agent and go to **Actions**.
2. Search for **zeroheight** in the connector gallery and select it.
3. Sign in to your zeroheight account when prompted to create a connection.
4. Add the zeroheight tools you want the agent to use (for example, `get-page` and `search-pages`).
5. Test your agent by asking it about a component or guideline from your design system.

## Frequently Asked Questions

### Can the connector modify content in zeroheight?

No. All tools are read-only. The connector cannot create, edit, or delete any content in zeroheight.

### Why is `search-pages` not returning results?

Semantic search requires AI features to be enabled for your zeroheight organisation. An organisation admin must enable this from zeroheight settings. See [AI-powered features in zeroheight](https://help.zeroheight.com/hc/en-us/articles/35887017130651-AI-powered-features-in-zeroheight).

## Known Issues and Limitations

- Semantic search (`search-pages`) requires AI features to be enabled for the zeroheight organisation by an admin. See [AI-powered features in zeroheight](https://help.zeroheight.com/hc/en-us/articles/35887017130651-AI-powered-features-in-zeroheight).

## Deployment Instructions

Use the [Power Platform Connectors CLI (`paconn`)](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in your environment:

```bash
pip install paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png
```

For more information about zeroheight, see [https://zeroheight.com](https://zeroheight.com).
