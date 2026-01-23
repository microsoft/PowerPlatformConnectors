# RapidStart Exchange Rate Addon

## Solution Design Document

A Power Platform addon solution for RapidStart CRM that automatically updates Dataverse currency exchange rates using the ExchangeRate-API.

---

## Solution Overview

### Purpose
Automatically synchronize exchange rates from ExchangeRate-API to Dataverse Transaction Currencies, ensuring accurate multi-currency pricing, reporting, and conversions.

### Key Features
- Scheduled exchange rate updates (configurable frequency)
- Manual "Update Now" capability
- Configuration UI in RapidStart Settings app
- Currency selection (sync all or selected currencies)
- Update history/logging
- Support for Free and Paid ExchangeRate-API plans

---

## Solution Components

### Publisher
- **Publisher**: Forceworks (frcwrks)
- **Solution Name**: RapidStart Exchange Rate
- **Unique Name**: fw_RapidStartExchangeRate

### Dependencies
- RapidStart CRM (fw_RapidStartCRM)
- RapidStart Settings (fw_RapidStartSettings)

---

## Dataverse Components

### 1. Tables

#### fw_exchangeratesetting (Exchange Rate Setting)
Configuration table for the addon (singleton record pattern).

| Column | Type | Description |
|--------|------|-------------|
| fw_exchangeratesettingid | GUID | Primary key |
| fw_name | Text | Setting name (default: "Exchange Rate Settings") |
| fw_apikey | Text | ExchangeRate-API key |
| fw_updatefrequency | Choice | Update frequency |
| fw_lastupdated | DateTime | Last successful update |
| fw_lastupdatestatus | Choice | Last update status |
| fw_lastupdatemessage | Text (Multi) | Last update message/error |
| fw_syncallcurrencies | Boolean | Sync all or selected only |
| fw_enabled | Boolean | Enable/disable auto-sync |

**Update Frequency Choices:**
| Value | Label |
|-------|-------|
| 271500000 | Every Hour |
| 271500001 | Every 6 Hours |
| 271500002 | Every 12 Hours |
| 271500003 | Daily |
| 271500004 | Weekly |

**Update Status Choices:**
| Value | Label |
|-------|-------|
| 271500000 | Success |
| 271500001 | Failed |
| 271500002 | Partial |
| 271500003 | Never Run |

#### fw_exchangeratelog (Exchange Rate Log)
Audit log of exchange rate updates.

| Column | Type | Description |
|--------|------|-------------|
| fw_exchangeratelogid | GUID | Primary key |
| fw_name | Text | Auto-generated name |
| fw_updatedon | DateTime | When update occurred |
| fw_status | Choice | Success/Failed/Partial |
| fw_currenciesupdated | Integer | Count of currencies updated |
| fw_currenciesfailed | Integer | Count of failures |
| fw_message | Text (Multi) | Details/error message |
| fw_basecurrency | Text | Base currency used |
| fw_triggertype | Choice | Manual/Scheduled |

**Trigger Type Choices:**
| Value | Label |
|-------|-------|
| 271500000 | Manual |
| 271500001 | Scheduled |

#### fw_currencysync (Currency Sync)
Junction table to track which currencies to sync (when not syncing all).

| Column | Type | Description |
|--------|------|-------------|
| fw_currencysyncid | GUID | Primary key |
| fw_name | Text | Currency code |
| fw_transactioncurrencyid | Lookup | Link to Transaction Currency |
| fw_enabled | Boolean | Include in sync |
| fw_lastrate | Decimal | Last synced rate |
| fw_lastupdated | DateTime | Last update time |

---

### 2. Custom Connector

**Name**: ExchangeRate-API
**Unique Name**: fw_ExchangeRateAPI

Use the OpenAPI definition created earlier with these operations:
- GetLatestRates
- GetPairConversion
- ConvertAmount
- GetSupportedCodes
- GetQuotaStatus
- GetHistoricalRates (Pro+)
- GetEnrichedData (Business+)

---

### 3. Connection Reference

**Name**: ExchangeRate-API Connection
**Unique Name**: fw_ExchangeRateAPIConnection
**Connector**: fw_ExchangeRateAPI

---

### 4. Environment Variables

| Name | Schema Name | Type | Description | Default |
|------|-------------|------|-------------|---------|
| Exchange Rate API Key | fw_ExchangeRateApiKey | Text | API key for ExchangeRate-API | (empty) |

---

### 5. Power Automate Flows

#### Flow 1: RS - Update Exchange Rates (Scheduled)
**Type**: Scheduled Cloud Flow
**Trigger**: Recurrence (configurable)

**Logic:**
1. Get Exchange Rate Setting record
2. Check if enabled
3. Check if due for update based on frequency
4. Get organization base currency
5. Call ExchangeRate-API GetLatestRates
6. Loop through Transaction Currencies
7. Update exchange rates where changed
8. Create Exchange Rate Log entry
9. Update Setting with last update info

#### Flow 2: RS - Update Exchange Rates (Manual)
**Type**: Instant Cloud Flow
**Trigger**: When a row is selected (fw_exchangeratesetting)

**Logic:**
1. Same as scheduled, but triggered manually
2. Set trigger type to "Manual" in log

#### Flow 3: RS - Initialize Currency Sync Records
**Type**: Instant Cloud Flow  
**Trigger**: When a row is selected (fw_exchangeratesetting)

**Logic:**
1. Get all Transaction Currencies in environment
2. Create Currency Sync records for each (if not exists)
3. Set all to enabled by default

---

### 6. Model-Driven App Integration

#### Sitemap Addition to RapidStart Settings

Add new Area or SubArea:
```xml
<SubArea Id="fw_exchangeratesetting" 
         Entity="fw_exchangeratesetting"
         Title="Exchange Rates"
         Icon="/WebResources/fw_exchangerate_icon" />
```

#### Forms

**Exchange Rate Setting - Main Form**
- **Header**: Status fields
- **Tab 1: Configuration**
  - Section: API Settings
    - API Key (with link to get free key)
    - Enabled toggle
  - Section: Schedule
    - Update Frequency
    - Sync All Currencies toggle
  - Section: Status
    - Last Updated
    - Last Update Status
    - Last Update Message
- **Tab 2: Currency Selection** (subgrid)
  - Currency Sync subgrid (when not syncing all)
- **Tab 3: History** (subgrid)
  - Exchange Rate Log subgrid

**Exchange Rate Log - Main Form**
- Read-only display of log details

---

### 7. Security Roles

#### Exchange Rate Administrator
- Full access to fw_exchangeratesetting
- Full access to fw_exchangeratelog
- Full access to fw_currencysync
- Can run manual update flows

#### Exchange Rate User
- Read access to fw_exchangeratesetting
- Read access to fw_exchangeratelog
- Read access to fw_currencysync

---

## Flow Details

### RS - Update Exchange Rates (Scheduled)

```yaml
Trigger: Recurrence
  - Frequency: Day
  - Interval: 1
  - Start time: 00:00

Actions:
  1. List rows - Get Exchange Rate Setting
     - Table: fw_exchangeratesetting
     - Row count: 1
     
  2. Condition - Is Enabled?
     - If fw_enabled = true
     
  3. Get organization base currency
     - List rows: organizations
     - Select: basecurrencyid
     
  4. Get Base Currency Code
     - Get row: transactioncurrency
     - Row ID: basecurrencyid from org
     
  5. Initialize variables
     - varUpdatedCount (Integer) = 0
     - varFailedCount (Integer) = 0
     - varMessages (Array) = []
     
  6. Call ExchangeRate-API - GetLatestRates
     - Connection: fw_ExchangeRateAPIConnection
     - api-key: fw_ExchangeRateApiKey env var
     - base_code: Base currency ISO code
     
  7. Condition - API Success?
     - If result = "success"
     
  8. List Transaction Currencies
     - Table: transactioncurrency
     - Filter: statecode eq 0
     
  9. Apply to each - Currency
     - For each transaction currency:
     
     a. Get rate from API response
        - Compose: body('GetLatestRates')?['conversion_rates']?[items('Apply_to_each')?['isocurrencycode']]
        
     b. Condition - Rate exists and different?
        - If rate is not null AND rate != current exchangerate
        
     c. Update Transaction Currency
        - Table: transactioncurrency
        - Row ID: transactioncurrencyid
        - exchangerate: calculated rate (1/api_rate for non-base)
        
     d. Increment varUpdatedCount
     
  10. Create Exchange Rate Log
      - Table: fw_exchangeratelog
      - fw_updatedon: utcNow()
      - fw_status: Success/Failed/Partial
      - fw_currenciesupdated: varUpdatedCount
      - fw_basecurrency: base currency code
      - fw_triggertype: Scheduled
      
  11. Update Exchange Rate Setting
      - fw_lastupdated: utcNow()
      - fw_lastupdatestatus: Success/Failed
```

---

## Exchange Rate Calculation

Dataverse stores exchange rates as: **Base Currency to Transaction Currency**

ExchangeRate-API returns: **Base Currency to Other Currencies**

**Example:**
- Organization base currency: USD
- API returns: EUR = 0.92 (1 USD = 0.92 EUR)
- Dataverse expects: 1 EUR = X USD
- Calculation: 1 / 0.92 = 1.087 (store this for EUR)

**For the base currency itself:**
- exchangerate = 1.0 (always)

---

## Configuration UI Mockup

```
┌─────────────────────────────────────────────────────────────┐
│ Exchange Rate Settings                                       │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  API Configuration                                           │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ API Key: [________________________] 🔗 Get Free Key    │ │
│  │                                                        │ │
│  │ ☑ Enable Automatic Updates                             │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  Schedule                                                    │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Update Frequency: [ Daily            ▼]               │ │
│  │                                                        │ │
│  │ ○ Sync All Currencies                                  │ │
│  │ ● Sync Selected Currencies Only                        │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  Status                                                      │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Last Updated: Jan 21, 2026 08:00 AM                    │ │
│  │ Status: ✅ Success                                      │ │
│  │ Message: Updated 12 currencies                         │ │
│  │                                                        │ │
│  │ [🔄 Update Now]                                        │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ─────────────────────────────────────────────────────────  │
│                                                              │
│  Currency Selection (when "Selected Only")                   │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Currency     │ Enabled │ Last Rate │ Last Updated      │ │
│  │──────────────┼─────────┼───────────┼──────────────────│ │
│  │ USD          │   ☑     │ 1.0000    │ Jan 21, 2026     │ │
│  │ EUR          │   ☑     │ 1.0870    │ Jan 21, 2026     │ │
│  │ GBP          │   ☑     │ 1.2650    │ Jan 21, 2026     │ │
│  │ JPY          │   ☐     │ 0.0067    │ Jan 20, 2026     │ │
│  │ CAD          │   ☑     │ 0.7150    │ Jan 21, 2026     │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ─────────────────────────────────────────────────────────  │
│                                                              │
│  Update History                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Date/Time          │ Status  │ Updated │ Trigger       │ │
│  │────────────────────┼─────────┼─────────┼──────────────│ │
│  │ Jan 21, 2026 08:00 │ ✅      │ 12      │ Scheduled    │ │
│  │ Jan 20, 2026 08:00 │ ✅      │ 12      │ Scheduled    │ │
│  │ Jan 19, 2026 14:30 │ ✅      │ 5       │ Manual       │ │
│  │ Jan 19, 2026 08:00 │ ⚠️      │ 10      │ Scheduled    │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## Installation Steps

1. **Prerequisites**
   - RapidStart CRM installed
   - RapidStart Settings installed
   - ExchangeRate-API key (free or paid)

2. **Import Solution**
   - Import fw_RapidStartExchangeRate_managed.zip

3. **Configure Connection**
   - When prompted, create connection for ExchangeRate-API
   - Enter your API key

4. **Initial Setup**
   - Navigate to RapidStart Settings → Exchange Rates
   - Verify API key is set
   - Enable automatic updates
   - Select update frequency
   - Choose currency sync mode
   - Click "Update Now" to test

5. **Verify**
   - Check Exchange Rate Log for success
   - Verify Transaction Currencies have updated rates

---

## Considerations

### Free Plan Limitations
- 1,500 requests/month
- Updates once daily
- No historical data

**Recommendation**: Set frequency to "Daily" for free plan users

### Rate Calculation Timing
- Scheduled flow runs at configured interval
- Actual API data updates vary by plan:
  - Free: Once per day
  - Pro: Every hour
  - Business: Every 5 minutes

### Error Handling
- API errors logged with details
- Partial updates still committed (some currencies may update while others fail)
- Setting shows last error message for troubleshooting

### Multi-Currency Setup
- Solution assumes organization already has Transaction Currencies configured
- Does not create new currencies (only updates rates)
- Base currency rate is always 1.0

---

## Future Enhancements

1. **Historical Rate Sync** (Pro plan)
   - Backfill historical rates for reporting

2. **Rate Change Alerts**
   - Notify when rates change by threshold %

3. **Currency Auto-Discovery**
   - Suggest adding currencies used in records but not configured

4. **Dashboard Widget**
   - Show exchange rate trends on home dashboard
