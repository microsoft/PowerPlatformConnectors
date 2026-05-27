# AllRatesToday (Independent Publisher)

Real-time and historical currency exchange rate API supporting 160+ currencies. Rates are sourced from Reuters/Refinitiv and interbank feeds, updated every 60 seconds.

## Publisher: Cahthurana G

## Prerequisites

An AllRatesToday account is required. Sign up for a free plan (no credit card needed) at [allratestoday.com/register](https://allratestoday.com/register). Paid plans are available for higher usage limits.

## Obtaining Credentials

1. Register at [allratestoday.com/register](https://allratestoday.com/register)
2. Log in to your dashboard at [allratestoday.com/profile](https://allratestoday.com/profile)
3. Copy your API key from the dashboard
4. When creating a connection in Power Automate, paste your API key. The connector automatically sends it as a Bearer token in the Authorization header.

## Supported Operations

### Get Exchange Rate
Get the live mid-market exchange rate between two currencies. Supports multiple target currencies (comma-separated). Requires authentication.

**Parameters:**
- `source` (required): Source currency code, e.g. `USD`
- `target` (required): Target currency code(s), e.g. `EUR` or `EUR,GBP,JPY`

**Response:** Array of rate objects with `source`, `target`, `rate`, and `time` fields.

### Get Public Rate
Get a single exchange rate between two currencies without authentication. Useful for quick lookups and testing.

**Parameters:**
- `source` (required): Source currency code, e.g. `USD`
- `target` (required): Target currency code, e.g. `EUR`

**Response:** Object with `data.source`, `data.target`, `data.rate`, and `data.time` fields.

### Get Historical Rate
Get historical exchange rates for a specific date range. Supports grouping by day, week, or month. Requires authentication.

**Parameters:**
- `source` (required): Source currency code, e.g. `USD`
- `target` (required): Target currency code, e.g. `EUR`
- `from` (required): Start date in ISO format, e.g. `2024-01-01T00:00:00+00:00`
- `to` (required): End date in ISO format, e.g. `2024-01-31T23:59:59+00:00`
- `group` (optional): Grouping interval: `day`, `week`, or `month`

**Response:** Array of rate objects with `source`, `target`, `rate`, and `time` fields.

### List Supported Currencies
List all 160+ supported currency codes, names, and symbols. No authentication required.

**Response:** Object with `currencies` array and `count` field.

## Known Issues and Limitations

- Free plan has monthly request limits. Upgrade to a paid plan for higher limits.
- Historical data requires an authenticated API key.
- The public rate endpoint (`/api/rate`) does not require authentication but returns only a single pair at a time.
- Rate data is sourced from Reuters/Refinitiv and updated every 60 seconds.

## Deployment Instructions

1. Download the connector files from this folder.
2. Create a new custom connector in Power Automate or Power Apps.
3. Import the `apiDefinition.swagger.json` file.
4. Import the `apiProperties.json` file.
5. Create a new connection using your AllRatesToday API key.
6. Test the connector using the Test tab.

## API Documentation

Full API documentation is available at [allratestoday.com/docs](https://allratestoday.com/docs)
