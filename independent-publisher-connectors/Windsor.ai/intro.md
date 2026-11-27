# Windsor.ai Connector

Windsor.ai provides a unified API to access marketing and analytics data from over 300 platforms including Google Ads, Facebook Ads, LinkedIn Ads, HubSpot, Shopify, TikTok, and more. This connector allows you to retrieve, aggregate, and filter cross-channel marketing data directly in Power Automate and Power Apps workflows.

## Prerequisites

- A Windsor.ai account ([sign up](https://windsor.ai)).
- An active data source connected in your Windsor.ai dashboard.
- An API key from your Windsor.ai account.

## How to get credentials

1. Log in to your [Windsor.ai](https://windsor.ai) account.
2. Navigate to your account settings or API section.
3. Copy your API key.

All API requests must include the `api_key` parameter.

## Supported Operations

- **Get Data** – Retrieve marketing data from a specific connector (e.g., Google Ads, Facebook) with optional date range and field selection.
- **List Connectors** – Returns a list of all available data source connectors.
- **Get Fields** – Returns available fields for a specific connector.
- **Get Options** – Returns connector-specific configuration options.

## Known Issues and Limitations

- The API has a rate limit of 600 requests per minute.
- OAuth connectors are not supported for independent publishers at this time; this connector uses API key authentication.
- Some connectors may require active account connections in the Windsor.ai dashboard before data can be retrieved.

## FAQ

For questions or support, visit [https://windsor.ai](https://windsor.ai) or refer to the [API documentation](https://windsor.ai/api-documentation/).
