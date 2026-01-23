# ExchangeRate-API Custom Connector for Power Automate

## Complete Setup and Usage Guide

A comprehensive Power Automate custom connector for [ExchangeRate-API](https://www.exchangerate-api.com), providing easy access to currency exchange rates for 161+ world currencies.

---

## Table of Contents

1. [Overview](#overview)
2. [Plan Comparison](#plan-comparison)
3. [Getting Your API Key](#getting-your-api-key)
4. [Installing the Connector](#installing-the-connector)
5. [Testing the Connector](#testing-the-connector)
6. [Available Operations](#available-operations)
7. [Usage Examples](#usage-examples)
8. [Error Handling](#error-handling)
9. [Best Practices](#best-practices)
10. [Troubleshooting](#troubleshooting)

---

## Overview

ExchangeRate-API is a reliable currency conversion API that has been operating since 2010. This custom connector allows you to integrate exchange rate data directly into your Power Automate flows for:

- **E-Commerce**: Display localized pricing for international customers
- **Financial Dashboards**: Track international sales and earnings
- **Expense Management**: Convert receipts and invoices to home currency
- **Reporting**: Generate multi-currency financial reports
- **Data Processing**: Automate currency conversions in your workflows

---

## Plan Comparison

### Feature Availability by Plan

| Feature | Free | Pro ($10/mo) | Business ($30/mo) | Volume ($70/mo) |
|---------|:----:|:------------:|:-----------------:|:---------------:|
| **API Requests/Month** | 1,500 | 30,000 | 125,000 | 300,000+ |
| **Update Frequency** | Once per day | Every 60 min | Every 5 min | Every 5 min |
| **Get Latest Rates** | ✅ | ✅ | ✅ | ✅ |
| **Pair Conversion** | ✅ | ✅ | ✅ | ✅ |
| **Convert Amount** | ✅ | ✅ | ✅ | ✅ |
| **Supported Codes List** | ✅ | ✅ | ✅ | ✅ |
| **Quota Status** | ✅ | ✅ | ✅ | ✅ |
| **Historical Data** | ❌ | ✅ | ✅ | ✅ |
| **Enriched Data** | ❌ | ❌ | ✅ | ✅ |
| **Email Support** | ❌ | ✅ | ✅ Priority | ✅ Priority |
| **High Availability Infrastructure** | ✅ | ✅ | ✅ | ✅ |
| **Long Term Support (LTS)** | ❌ | ✅ | ✅ | ✅ |

### Connector Operations by Plan

#### ✅ Free Plan Operations

| Operation | Description |
|-----------|-------------|
| **Get Latest Rates** | Returns current exchange rates from your base currency to all 161+ supported currencies |
| **Get Pair Conversion Rate** | Returns the exchange rate between two specific currencies |
| **Convert Currency Amount** | Converts a specific amount from one currency to another with the calculated result |
| **Get Supported Codes** | Returns a list of all supported currency codes and their names |
| **Get API Request Quota** | Returns your current quota status (requests remaining, refresh date) |

#### 💰 Pro Plan Operations (and above)

| Operation | Description | Minimum Plan |
|-----------|-------------|--------------|
| **Get Historical Rates** | Returns exchange rates for a specific date in the past (data from 1990+) | Pro |
| **Convert Historical Amount** | Converts an amount using historical rates for a specific date | Pro |

#### 💼 Business Plan Operations (and above)

| Operation | Description | Minimum Plan |
|-----------|-------------|--------------|
| **Get Enriched Data** | Returns exchange rate plus currency name, symbol, country, and flag URL | Business |

### Yearly Billing Discounts

Save 2 months by paying annually:
- **Pro**: $100/year (vs $120 monthly)
- **Business**: $300/year (vs $360 monthly)
- **Volume**: $700/year (vs $840 monthly)

---

## Getting Your API Key

1. Go to [https://app.exchangerate-api.com/sign-up](https://app.exchangerate-api.com/sign-up)
2. Choose your plan:
   - **Free**: No credit card required
   - **Paid Plans**: 2-week free trial available
3. Enter your email address
4. Confirm your email
5. Your API key will be displayed in your dashboard

> **Note**: Your API key looks like: `a1b2c3d4e5f6g7h8i9j0` (alphanumeric string)

---

## Installing the Connector

### Method 1: Power Automate Portal

1. Go to [Power Automate](https://make.powerautomate.com)
2. Navigate to **Data** → **Custom connectors**
3. Click **+ New custom connector** → **Import an OpenAPI file**
4. Enter a name: `ExchangeRate-API`
5. Click **Import** and select the `ExchangeRate-API-Connector.swagger.json` file
6. Click **Continue**
7. Review the **General** tab settings:
   - Host: `v6.exchangerate-api.com`
   - Base URL: `/v6`
8. Skip the **Security** tab (authentication is handled via path parameter)
9. Review the **Definition** tab to see all operations
10. Click **Create connector**

### Method 2: Power Apps Portal

1. Go to [Power Apps](https://make.powerapps.com)
2. Navigate to **Dataverse** → **Custom Connectors**
3. Click **+ New custom connector** → **Import an OpenAPI file**
4. Follow steps 4-10 above

---

## Testing the Connector

### In the Custom Connector Editor

1. Open your connector in **Data** → **Custom connectors**
2. Click on the **5. Test** tab
3. Click **+ New connection**
4. The connection will be created (no credentials needed at this step)
5. Select the connection from the dropdown
6. Choose an operation to test

### Test Examples

#### Test 1: Get Latest Rates (Free Plan)

| Parameter | Value |
|-----------|-------|
| API Key | `your-api-key` |
| Base Currency Code | `USD` |

**Expected Response:**
```json
{
  "result": "success",
  "base_code": "USD",
  "conversion_rates": {
    "EUR": 0.92,
    "GBP": 0.79,
    "JPY": 149.50,
    ...
  }
}
```

#### Test 2: Convert Amount (Free Plan)

| Parameter | Value |
|-----------|-------|
| API Key | `your-api-key` |
| Base Currency Code | `EUR` |
| Target Currency Code | `USD` |
| Amount | `100` |

**Expected Response:**
```json
{
  "result": "success",
  "base_code": "EUR",
  "target_code": "USD",
  "conversion_rate": 1.0856,
  "conversion_result": 108.56
}
```

#### Test 3: Historical Rates (Pro Plan Required)

| Parameter | Value |
|-----------|-------|
| API Key | `your-api-key` |
| Base Currency Code | `USD` |
| Year | `2023` |
| Month | `6` |
| Day | `15` |

**Expected Response:**
```json
{
  "result": "success",
  "year": 2023,
  "month": 6,
  "day": 15,
  "base_code": "USD",
  "conversion_rates": { ... }
}
```

---

## Available Operations

### 1. Get Latest Exchange Rates

**Description**: Returns current exchange rates from your base currency to all supported currencies.

**Plan**: Free and above

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |
| base_code | string | Yes | Base currency code (e.g., USD, EUR) |

**Response Fields**:
| Field | Description |
|-------|-------------|
| result | "success" or "error" |
| base_code | The base currency you specified |
| time_last_update_utc | When rates were last updated |
| time_next_update_utc | When rates will next update |
| conversion_rates | Object with all currency rates |

---

### 2. Get Pair Conversion Rate

**Description**: Returns the exchange rate between two specific currencies.

**Plan**: Free and above

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |
| base_code | string | Yes | Base currency code |
| target_code | string | Yes | Target currency code |

**Response Fields**:
| Field | Description |
|-------|-------------|
| conversion_rate | Exchange rate from base to target |
| base_code | Base currency |
| target_code | Target currency |

---

### 3. Convert Currency Amount

**Description**: Converts a specific amount from one currency to another.

**Plan**: Free and above

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |
| base_code | string | Yes | Base currency code |
| target_code | string | Yes | Target currency code |
| amount | number | Yes | Amount to convert |

**Response Fields**:
| Field | Description |
|-------|-------------|
| conversion_rate | Exchange rate used |
| conversion_result | The converted amount |

---

### 4. Get Enriched Currency Data

**Description**: Returns exchange rate plus additional localization data.

**Plan**: Business and above ⚠️

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |
| base_code | string | Yes | Base currency code |
| target_code | string | Yes | Target currency code |

**Response Fields**:
| Field | Description |
|-------|-------------|
| conversion_rate | Exchange rate |
| target_data.locale | Country/region name |
| target_data.two_letter_code | ISO country code (e.g., "JP") |
| target_data.currency_name | Full currency name |
| target_data.currency_name_short | Short name (e.g., "Yen") |
| target_data.display_symbol | Unicode hex for symbol |
| target_data.flag_url | URL to country flag image |

---

### 5. Get Historical Exchange Rates

**Description**: Returns exchange rates for a specific date in the past.

**Plan**: Pro and above ⚠️

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |
| base_code | string | Yes | Base currency code |
| year | integer | Yes | Year (e.g., 2023) |
| month | integer | Yes | Month (1-12, no leading zero) |
| day | integer | Yes | Day (1-31, no leading zero) |

**Data Availability**:
- **1990-2020**: Limited currency set (major currencies only)
- **2021-Present**: All 161+ currencies available

---

### 6. Convert Amount Using Historical Rates

**Description**: Converts an amount using exchange rates from a specific historical date.

**Plan**: Pro and above ⚠️

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |
| base_code | string | Yes | Base currency code |
| year | integer | Yes | Year |
| month | integer | Yes | Month (1-12) |
| day | integer | Yes | Day (1-31) |
| amount | number | Yes | Amount to convert |

---

### 7. Get Supported Currency Codes

**Description**: Returns a list of all supported currency codes and names.

**Plan**: Free and above

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |

**Response**: Array of [code, name] pairs, e.g., `["USD", "United States Dollar"]`

---

### 8. Get API Request Quota

**Description**: Returns your current API quota status.

**Plan**: Free and above

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| api-key | string | Yes | Your ExchangeRate-API key |

**Response Fields**:
| Field | Description |
|-------|-------------|
| plan_quota | Total requests allowed per month |
| requests_remaining | Requests left this period |
| refresh_day_of_month | Day when quota resets |

---

## Usage Examples

### Example Flow 1: Daily Exchange Rate Email

**Scenario**: Send a daily email with key exchange rates

1. **Trigger**: Recurrence (daily at 9 AM)
2. **Action**: ExchangeRate-API → Get Latest Rates
   - Base Currency: `USD`
3. **Action**: Send an email
   - Include rates from response

### Example Flow 2: Invoice Currency Conversion

**Scenario**: Convert invoice amounts when a new row is added to Excel

1. **Trigger**: When a row is added (Excel)
2. **Action**: ExchangeRate-API → Convert Amount
   - Base: Invoice currency from Excel
   - Target: `USD`
   - Amount: Invoice amount from Excel
3. **Action**: Update Excel row with converted amount

### Example Flow 3: Historical Rate Lookup

**Scenario**: Look up exchange rate on a transaction date (requires Pro plan)

1. **Trigger**: Manual or automated
2. **Action**: ExchangeRate-API → Get Historical Rates
   - Year: `year(triggerBody()?['transaction_date'])`
   - Month: `month(triggerBody()?['transaction_date'])`
   - Day: `day(triggerBody()?['transaction_date'])`

---

## Error Handling

### Error Types

| Error Type | Description | Solution |
|------------|-------------|----------|
| `unsupported-code` | Currency code not supported | Check [supported currencies](https://www.exchangerate-api.com/docs/supported-currencies) |
| `malformed-request` | Request format incorrect | Verify parameter format |
| `invalid-key` | API key is invalid | Check your API key in dashboard |
| `inactive-account` | Email not confirmed | Confirm your email address |
| `quota-reached` | Monthly limit exceeded | Upgrade plan or wait for reset |
| `no-data-available` | No data for specified date | Try a different date |
| `plan-upgrade-required` | Feature requires higher plan | Upgrade to Pro/Business |

### Handling Errors in Flows

Use a **Condition** action after the API call:

```
If result equals "error"
  → Send notification with error-type
  → Terminate flow
Else
  → Continue with conversion_rates
```

---

## Best Practices

### 1. Cache Rates When Possible

Free plan updates once daily, so cache the response:
- Store rates in a SharePoint list or Dataverse
- Refresh once per day
- Use cached rates for conversions

### 2. Monitor Your Quota

Add a weekly flow to check quota:
1. Call **Get API Request Quota**
2. If `requests_remaining` < 20%, send alert

### 3. Use Pair Conversion for Specific Needs

If you only need EUR→USD, use **Get Pair Conversion** instead of **Get Latest Rates** (smaller response, same quota cost).

### 4. Handle Date Parameters Correctly

For historical data:
- Month and day have **no leading zeros**
- ✅ Correct: Year=2023, Month=6, Day=5
- ❌ Wrong: Year=2023, Month=06, Day=05

### 5. Store API Key Securely

Use environment variables or Azure Key Vault instead of hardcoding the API key in flows.

---

## Troubleshooting

### "Invalid-key" Error

1. Verify your API key in the [dashboard](https://app.exchangerate-api.com/dashboard)
2. Ensure no extra spaces before/after the key
3. Check that your account email is confirmed

### "Plan-upgrade-required" Error

You're trying to use a feature not available on your plan:
- **Historical Data** → Requires Pro ($10/mo)
- **Enriched Data** → Requires Business ($30/mo)

### Connection Test Fails

1. Verify host is `v6.exchangerate-api.com`
2. Verify base URL is `/v6`
3. Ensure HTTPS is enabled
4. Try updating the connector

### Type Mismatch Warnings

The connector uses `int32` for timestamps. If you see type warnings:
1. Re-import the latest connector JSON
2. Update and save the connector

### No Response Data

1. Check the `result` field - should be "success"
2. Verify your quota hasn't been exceeded
3. For historical data, ensure the date is valid (not in the future)

---

## Support Resources

- **ExchangeRate-API Documentation**: [https://www.exchangerate-api.com/docs/overview](https://www.exchangerate-api.com/docs/overview)
- **Supported Currencies**: [https://www.exchangerate-api.com/docs/supported-currencies](https://www.exchangerate-api.com/docs/supported-currencies)
- **API Status**: [http://stats.pingdom.com/qv69spvrz94m](http://stats.pingdom.com/qv69spvrz94m)
- **Email Support**: support@exchangerate-api.com (paid plans)

---

## Common Currency Codes

| Code | Currency | Country/Region |
|------|----------|----------------|
| USD | US Dollar | United States |
| EUR | Euro | European Union |
| GBP | British Pound | United Kingdom |
| JPY | Japanese Yen | Japan |
| CAD | Canadian Dollar | Canada |
| AUD | Australian Dollar | Australia |
| CHF | Swiss Franc | Switzerland |
| CNY | Chinese Yuan | China |
| INR | Indian Rupee | India |
| MXN | Mexican Peso | Mexico |
| BRL | Brazilian Real | Brazil |
| KRW | South Korean Won | South Korea |
| SGD | Singapore Dollar | Singapore |
| HKD | Hong Kong Dollar | Hong Kong |
| NZD | New Zealand Dollar | New Zealand |

Use the **Get Supported Codes** operation for the complete list of 161+ currencies.

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | January 2026 | Initial release with all 8 operations |

---

*This connector is not officially affiliated with ExchangeRate-API. ExchangeRate-API is a product of AYR Tech (Pty) Ltd.*
