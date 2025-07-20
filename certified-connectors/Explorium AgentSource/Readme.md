# Explorium - Agent Source Connector

Explorium empowers businesses to build high-performance GTM agents with specialized data infrastructure. Our seamless API integrations and high-quality data drive faster agent development and better results. With years of experience and robust data sets, we deliver context-aware solutions that help AI agents achieve human-level support. Explorium is the essential data partner for teams building agent-driven technologies.

**Full documentation:** [https://developers.explorium.ai/reference/quick-starts](https://developers.explorium.ai/reference/quick-starts)

## Publisher: Explorium

## Prerequisites

To use this connector, you will need:

- A Microsoft Power Apps or Power Automate plan that supports custom connectors.
- An active Explorium account.
- An Explorium API key.

## Supported Operations

The connector supports the following operations:

### **Businesses Matching**

- **Match Businesses**: Match a list of businesses by attributes to retrieve their `business_id`s.

### **Businesses (Bulk) Enrichments**

- **Technographics Enrichment**: Get detailed information about the technologies used by a business.
- **Workforce Trends Enrichment**: Retrieve workforce-related insights.
- **Firmographics Enrichment**: Access firmographic data (size, industry, etc.).
- **Financial Indicators Enrichment**: Retrieve financial metrics.
- **Funding and Acquisition Enrichment**: See funding rounds and acquisition history.
- **Company Website Keywords Enrichment**: Discover relevant website keywords.
- **Website Changes Enrichment**: Monitor website updates.
- **LinkedIn Posts Enrichment**: Get recent company LinkedIn activity.
- **Company Ratings by Employees Enrichment**: View aggregated employee ratings.
- **PC Business Challenges Enrichment**: Learn about challenges identified in public company filings.
- **PC Competitive Landscape Enrichment**: Analyze competitors from filings.
- **PC Strategy Enrichment**: Access strategy insights from filings.

### **Prospects Matching**

- **Match Prospects**: Match a list of prospects by attributes to retrieve their `prospect_id`s.

### **Prospects (Bulk) Enrichments**

- **Contacts Information Enrichment**: Enrich multiple prospects with contact information in bulk.
- **LinkedIn Prospects Posts Enrichment**: Retrieve LinkedIn post activity for multiple prospects.
- **Profiles Enrichment**: Access enriched profile data for multiple prospects.


> **Note:** Each enrichment operation requires the relevant `business_id` or `prospect_id` obtained from a **Match** call.

## Obtaining Credentials

To create an Explorium account and retrieve your API key, see:

- [Account Access](https://developers.explorium.ai/reference/account_access)
- [Getting Your API Key](https://developers.explorium.ai/reference/getting_your_api_key)

## Getting Started

Setting up the connector is simple:

1. Create a connection and enter your Explorium API Key.
2. Use the available actions to match and enrich businesses or prospects.

This connector uses **API Key authentication**. You will be prompted to enter your API Key when creating the connection in Power Automate.

**Recommended flow:**

- **Businesses Enrichment**
  - First, call **Match Businesses** with basic information (e.g., name & website URL or name & domain) to retrieve the `business_id`.
  - Then, use any enrichment action (e.g., Technographics, Workforce Trends) with that `business_id` to get rich, contextual businesses data.
- **Prospects Enrichment**
  - First, call **Match Prospects** to retrieve the `prospect_id`.
  - Then, use any enrichment action (e.g., Contacts Information, Linkedin Prospects Posts) with that `prospect_id` to get rich, contextual prospects data.

**Quick Start Guides**: [https://developers.explorium.ai/reference/quick-starts](https://developers.explorium.ai/reference/quick-starts)

For questions or support, contact [support@explorium.ai](mailto\:support@explorium.ai).

## Known Issues and Limitations
As long as you have an active Explorium account with sufficient credits, there should be no issues or limitations.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps
