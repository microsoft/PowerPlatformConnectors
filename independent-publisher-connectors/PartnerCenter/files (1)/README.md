# Partner Center Marketplace Insights Custom Connector

A Power Platform custom connector for accessing Microsoft Partner Center Commercial Marketplace Analytics API. This connector enables ISVs to programmatically track orders, usage, customers, revenue, and license quantities for their marketplace offers.

## Overview

This connector replaces the deprecated Microsoft Partner Center connector with a modern implementation focused on ISV marketplace analytics. It provides access to the following datasets:

| Dataset | Description |
|---------|-------------|
| **ISVOrder** | Orders, quantities, revenue, subscription status |
| **ISVUsage** | Usage metrics and metered billing data |
| **ISVCustomer** | Customer information and growth trends |
| **ISVRevenue** | Revenue and payout information |
| **ISVLicense** | License quantities and assignments |
| **ISVMarketplaceInsights** | Page visits, CTA clicks, engagement metrics |
| **ISVOfferRetention** | Customer retention analytics |
| **ISVQualityOfService** | Deployment quality metrics |
| **ISVVMImageVersion** | VM image version data |

## Prerequisites

Before using this connector, you need:

1. **Microsoft Partner Center Account** with Marketplace offers
2. **Azure AD App Registration** with Partner Center API permissions
3. **Power Platform environment** (Power Automate, Power Apps, or Logic Apps)

## Step 1: Create Azure AD App Registration

1. Go to [Azure Portal](https://portal.azure.com) → **Azure Active Directory** → **App registrations**
2. Click **New registration**
3. Configure:
   - **Name**: `Partner Center Marketplace Connector`
   - **Supported account types**: Accounts in any organizational directory
   - **Redirect URI**: Web → `https://global.consent.azure-apim.net/redirect`
4. Click **Register**
5. Note your **Application (client) ID** and **Directory (tenant) ID**

### Add Client Secret

1. Go to **Certificates & secrets** → **New client secret**
2. Add a description and expiry
3. **Copy the secret value immediately** (you won't see it again)

### Configure API Permissions

1. Go to **API permissions** → **Add a permission**
2. Select **APIs my organization uses**
3. Search for `Partner Center` or use the API: `https://api.partnercenter.microsoft.com`
4. Add the required permissions (typically delegated permissions)
5. Click **Grant admin consent** if required

## Step 2: Import the Custom Connector

### Option A: Power Automate / Power Apps Portal

1. Go to [Power Automate](https://make.powerautomate.com) or [Power Apps](https://make.powerapps.com)
2. Navigate to **Data** → **Custom Connectors**
3. Click **+ New custom connector** → **Import an OpenAPI file**
4. Upload `apiDefinition.swagger.json`
5. Configure the connector:

#### General Tab
- Verify the host is `api.partnercenter.microsoft.com`
- Base URL should be `/insights/v1.1/cmp`

#### Security Tab
- Authentication type: **OAuth 2.0**
- Identity Provider: **Azure Active Directory**
- Client ID: `[Your App Registration Client ID]`
- Client Secret: `[Your App Registration Client Secret]`
- Resource URL: `https://api.partnercenter.microsoft.com`
- Scope: Leave empty
- Authorization URL: `https://login.microsoftonline.com/common/oauth2/authorize`
- Token URL: `https://login.microsoftonline.com/common/oauth2/token`
- Redirect URL: Copy the provided URL (usually `https://global.consent.azure-apim.net/redirect`)

6. Click **Create connector**

### Option B: Using Power Platform CLI

```bash
# Install the Power Platform CLI
npm install -g @microsoft/powerplatform-cli

# Authenticate
pac auth create --url https://[your-env].crm.dynamics.com

# Import the connector
pac connector create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

## Step 3: Create a Connection

1. In Power Automate, go to **Data** → **Connections**
2. Click **+ New connection**
3. Search for your connector name
4. Sign in with your Partner Center account
5. Authorize the connection

## Available Actions

### Get Available Datasets
Retrieves all available datasets and their columns for building queries.

### Create Custom Query
Creates a SQL-like query against the datasets:
```sql
SELECT OrderId, Quantity, OfferName, CustomerCompanyName, OrderStatus, OrderPurchaseDate
FROM ISVOrder
WHERE OrderStatus = 'Active'
```

### Test Query
Validates a query and returns up to 100 sample rows.

### Create Scheduled Report
Creates a recurring report based on a saved query.

### Get Report Executions
Retrieves report execution history and download links.

### Pause/Resume Report
Controls report scheduling.

## Example: Track Orders and License Quantities

### Flow 1: Get Daily Order Summary

```
Trigger: Recurrence (Daily)
↓
Action: Test Query
  Query: SELECT OfferName, SKU, SUM(Quantity) as TotalQuantity, 
         COUNT(*) as OrderCount, SUM(BilledRevenue) as Revenue
         FROM ISVOrder
         WHERE OrderStatus = 'Active'
         GROUP BY OfferName, SKU
↓
Action: Send Email / Post to Teams / Update SharePoint
```

### Flow 2: Set Up Automated Reporting

```
1. Create Query:
   Name: "Active Orders Report"
   Query: SELECT * FROM ISVOrder WHERE OrderStatus = 'Active'

2. Create Scheduled Report:
   QueryId: [from step 1]
   StartTime: 2024-01-01T00:00:00Z
   RecurrenceInterval: 24 (daily)
   Format: csv

3. Get Report Executions (on schedule):
   Check executionStatus = 'Completed'
   Download from reportAccessSecureLink
```

## Common Queries for ISV Analytics

### Orders by Offer
```sql
SELECT OfferName, Quantity, OrderStatus, OrderPurchaseDate, CustomerCompanyName
FROM ISVOrder
ORDER BY OrderPurchaseDate DESC
```

### Active Subscriptions with Quantities
```sql
SELECT OfferName, SKU, Quantity, TermStartDate, TermEndDate, BilledRevenue
FROM ISVOrder
WHERE OrderStatus = 'Active' AND IsTrial = 'False'
```

### Customer Growth
```sql
SELECT CustomerCompanyName, CustomerCountry, IsNewCustomer, OrderPurchaseDate
FROM ISVOrder
WHERE IsNewCustomer = 'True'
```

### Revenue by Month
```sql
SELECT MonthStartDate, OfferName, SUM(BilledRevenue) as Revenue, Currency
FROM ISVOrder
GROUP BY MonthStartDate, OfferName, Currency
```

### License Usage
```sql
SELECT OfferName, SKU, Quantity, AssignedSeats, ActiveSeats
FROM ISVLicense
```

## Available Columns (ISVOrder Dataset)

| Column | Description |
|--------|-------------|
| MarketplaceSubscriptionId | Unique subscription ID |
| OrderId | Order identifier |
| MonthStartDate | Month of the order |
| OfferType | Type of offer (SaaS, VM, etc.) |
| OfferName | Name of your offer |
| SKU | SKU/Plan identifier |
| PlanId | Plan ID |
| Quantity | Number of licenses/seats |
| OrderStatus | Active, Canceled, etc. |
| OrderPurchaseDate | When order was placed |
| CustomerCompanyName | Customer company |
| CustomerCountry | Customer location |
| BilledRevenue | Revenue amount |
| Currency | Currency code |
| IsNewCustomer | First purchase indicator |
| IsTrial | Trial subscription flag |
| TermStartDate | Subscription start |
| TermEndDate | Subscription end |
| AutoRenew | Auto-renewal status |
| IsPrivateOffer | Private offer flag |

## Troubleshooting

### 401 Unauthorized
- Verify your Azure AD app has the correct permissions
- Ensure admin consent is granted
- Check that the resource URL is `https://api.partnercenter.microsoft.com`

### 400 Bad Request - Invalid Query
- Verify column names match exactly (case-sensitive)
- Check SQL syntax (use single quotes for strings)
- Use `Get Available Datasets` to see valid column names

### No Data Returned
- Ensure you have active offers in the marketplace
- Check date range filters in your query
- Verify your Partner Center account has analytics access

### Token Expired
- Tokens expire after 60 minutes
- The connector handles refresh automatically
- If issues persist, recreate the connection

## Rate Limits

| Limit | Value |
|-------|-------|
| API calls per connection | 100 per 60 seconds |
| Report recurrence minimum | 1-24 hours (varies by dataset) |

## Support

- [Partner Center Analytics Documentation](https://learn.microsoft.com/en-us/partner-center/insights/analytics)
- [Partner Center API Reference](https://learn.microsoft.com/en-us/partner-center/developer/)
- [Power Platform Custom Connectors](https://learn.microsoft.com/en-us/connectors/custom-connectors/)

## License

This connector definition is provided as-is for educational and development purposes.
