# ExchangeRate-API Custom Connector for Power Automate

A Power Automate custom connector for [ExchangeRate-API](https://www.exchangerate-api.com), providing easy access to currency exchange rates for 161+ world currencies.

## Features

This connector provides the following operations:

| Operation | Description | Plan Required |
|-----------|-------------|---------------|
| **Get Latest Rates** | Get current exchange rates from a base currency to all supported currencies | Free |
| **Get Pair Conversion Rate** | Get the exchange rate between two specific currencies | Free |
| **Convert Currency Amount** | Convert a specific amount from one currency to another | Free |
| **Get Enriched Currency Data** | Get exchange rate plus currency name, symbol, country, and flag | Business+ |
| **Get Historical Rates** | Get exchange rates for a specific date in the past | Pro+ |
| **Convert Historical Amount** | Convert an amount using historical rates | Pro+ |
| **Get Supported Codes** | Get a list of all supported currency codes | Free |
| **Get API Request Quota** | Check your remaining API request quota | Free |

## Prerequisites

1. A Power Automate account (Microsoft 365 license or standalone)
2. An ExchangeRate-API key - [Get a free key here](https://app.exchangerate-api.com/sign-up)

## Installation

### Method 1: Import via Power Automate Portal

1. Go to [Power Automate](https://make.powerautomate.com)
2. Navigate to **Data** → **Custom connectors**
3. Click **+ New custom connector** → **Import an OpenAPI file**
4. Name your connector (e.g., "ExchangeRate-API")
5. Upload the `ExchangeRate-API-Connector.swagger.json` file
6. Click **Continue**
7. Review the settings and click **Create connector**

### Method 2: Import via Power Apps Portal

1. Go to [Power Apps](https://make.powerapps.com)
2. Navigate to **Dataverse** → **Custom Connectors**
3. Click **+ New custom connector** → **Import an OpenAPI file**
4. Follow the same steps as above

## Configuration

After importing the connector:

1. Go to the **Security** tab
2. Authentication type should be **API Key**
3. The API key is passed as a path parameter (already configured)
4. Click **Update connector**

## Creating a Connection

1. Go to **Data** → **Connections**
2. Click **+ New connection**
3. Search for "ExchangeRate-API"
4. Enter your API key when prompted
5. Click **Create**

## Usage Examples

### Example 1: Get Latest USD Exchange Rates

```
Operation: Get Latest Rates
Base Currency Code: USD
```

Returns current rates for USD to all 161+ currencies.

### Example 2: Convert EUR to GBP

```
Operation: Convert Currency Amount
Base Currency Code: EUR
Target Currency Code: GBP
Amount: 100
```

Returns the conversion of 100 EUR to GBP.

### Example 3: Get Historical Rate

```
Operation: Get Historical Rates
Base Currency Code: USD
Year: 2023
Month: 6
Day: 15
```

Returns exchange rates as of June 15, 2023.

## Supported Currency Codes

The API uses [ISO 4217](https://en.wikipedia.org/wiki/ISO_4217) three-letter currency codes. Common examples:

- **USD** - US Dollar
- **EUR** - Euro
- **GBP** - British Pound
- **JPY** - Japanese Yen
- **CAD** - Canadian Dollar
- **AUD** - Australian Dollar
- **CHF** - Swiss Franc
- **CNY** - Chinese Yuan
- **BRL** - Brazilian Real
- **INR** - Indian Rupee

Use the **Get Supported Codes** operation to retrieve the full list programmatically.

## Error Handling

The connector returns standard error types:

| Error Type | Description |
|------------|-------------|
| `unsupported-code` | Currency code not supported |
| `malformed-request` | Request format is incorrect |
| `invalid-key` | API key is not valid |
| `inactive-account` | Email not confirmed |
| `quota-reached` | Monthly request limit exceeded |
| `no-data-available` | No data for specified date |
| `plan-upgrade-required` | Feature requires a higher plan |

## Rate Limits

- **Free Plan**: 1,500 requests/month
- **Pro Plan**: 30,000 requests/month
- **Business Plan**: 100,000 requests/month
- **Volume Plan**: 300,000+ requests/month

Check your quota using the **Get API Request Quota** operation.

## Support

- ExchangeRate-API Documentation: https://www.exchangerate-api.com/docs/overview
- ExchangeRate-API Support: support@exchangerate-api.com

## License

This connector definition is provided as-is for use with ExchangeRate-API. ExchangeRate-API is a product of AYR Tech (Pty) Ltd.
