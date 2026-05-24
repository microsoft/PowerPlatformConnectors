# AllRatesToday

Real-time and historical currency exchange rate API supporting 160+ currencies. Rates are sourced from Reuters/Refinitiv and interbank feeds, updated every 60 seconds.

## Publisher: Cahthurana G

## Prerequisites

An AllRatesToday account is required. Sign up for a free plan (no credit card needed) at [allratestoday.com/register](https://allratestoday.com/register). Paid plans are available for higher usage limits.

## Obtaining Credentials

1. Register at [allratestoday.com/register](https://allratestoday.com/register)
2. Log in to your dashboard
3. Copy your API key from the dashboard

When creating a connection, enter your API key. The connector automatically sends it as a Bearer token in the Authorization header.

## Supported Operations

### Get Exchange Rate
Get the live mid-market exchange rate between two currencies. Supports multiple target currencies (comma-separated).

### Get Public Rate
Get a single exchange rate between two currencies without authentication. Useful for quick lookups.

### Get Historical Rate
Get historical exchange rates for a specific date range. Supports grouping by day, week, or month.

### List Supported Currencies
List all 160+ supported currency codes, names, and symbols.

## Known Issues and Limitations

- Free plan has monthly request limits. Upgrade to a paid plan for higher limits.
- Historical data requires an authenticated API key.
- The public rate endpoint (`/api/rate`) does not require authentication but returns only a single pair at a time.
- Rate data is cached for 60 seconds.

## API Documentation

Full API documentation is available at [allratestoday.com/developers](https://allratestoday.com/developers)
