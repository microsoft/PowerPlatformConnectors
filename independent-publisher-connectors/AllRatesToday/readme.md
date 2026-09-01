# AllRatesToday (Independent Publisher)

Real-time and historical currency exchange rate API supporting 160+ currencies. Rates are sourced from institutional interbank market data, updated every 60 seconds.

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

### Get Single Rate
Get a single exchange rate between two currencies. Useful for quick lookups and testing. Requires an API key, like every other operation.

**Parameters:**
- `source` (required): Source currency code, e.g. `USD`
- `target` (required): Target currency code, e.g. `EUR`

**Response:** Object with `rate` and `source` fields.

### Get Historical Rates
Get the historical exchange rate series for a currency pair over a preset period (`1d`, `7d`, `30d` or `1y`). Requires authentication.

**Parameters:**
- `source` (required): Source currency code, e.g. `USD`
- `target` (required): Target currency code, e.g. `EUR`
- `period` (required): Length of the series — `1d`, `7d`, `30d` or `1y`

**Response:** Object with `source`, `target`, `period`, `source_api`, and a `data` array of `{date, rate, timestamp}` entries.

### List Supported Currencies
List all 160+ supported currency codes, names, and symbols. No authentication required.

**Response:** Object with `currencies` array and `count` field.

## Known Issues and Limitations

- Free plan has monthly request limits. Upgrade to a paid plan for higher limits.
- Historical data requires an authenticated API key.
- The public rate endpoint (`/api/rate`) does not require authentication but returns only a single pair at a time.
- Rate data is sourced from institutional interbank market data and updated every 60 seconds.

## Deployment Instructions

1. Download the connector files from this folder.
2. Create a new custom connector in Power Automate or Power Apps.
3. Import the `apiDefinition.swagger.json` file.
4. Import the `apiProperties.json` file.
5. Create a new connection using your AllRatesToday API key.
6. Test the connector using the Test tab.

## API Documentation

Full API documentation is available at [allratestoday.com/docs](https://allratestoday.com/docs)
