# RapidStart Exchange Rate - Dataverse Table Definitions

## Table 1: Exchange Rate Setting (fw_exchangeratesetting)

### Table Properties
```yaml
Schema Name: fw_exchangeratesetting
Display Name: Exchange Rate Setting
Plural Name: Exchange Rate Settings
Description: Configuration settings for automatic exchange rate synchronization
Ownership: Organization
Primary Column: fw_name
Track Changes: No
Enable Auditing: Yes
```

### Columns

| Display Name | Schema Name | Type | Required | Description |
|--------------|-------------|------|----------|-------------|
| Name | fw_name | Text (100) | Yes | Setting record name |
| API Key | fw_apikey | Text (100) | No | ExchangeRate-API key |
| Enabled | fw_enabled | Yes/No | No | Enable automatic sync (Default: No) |
| Update Frequency | fw_updatefrequency | Choice | No | How often to update |
| Sync All Currencies | fw_syncallcurrencies | Yes/No | No | Sync all or selected (Default: Yes) |
| Last Updated | fw_lastupdated | Date/Time | No | Last successful update timestamp |
| Last Update Status | fw_lastupdatestatus | Choice | No | Status of last update |
| Last Update Message | fw_lastupdatemessage | Text Area (2000) | No | Details or error message |

### Choice: Update Frequency (fw_updatefrequency)

| Value | Label |
|-------|-------|
| 271500000 | Every Hour |
| 271500001 | Every 6 Hours |
| 271500002 | Every 12 Hours |
| 271500003 | Daily |
| 271500004 | Weekly |

**Default Value**: 271500003 (Daily)

### Choice: Last Update Status (fw_lastupdatestatus)

| Value | Label |
|-------|-------|
| 271500000 | Success |
| 271500001 | Failed |
| 271500002 | Partial |
| 271500003 | Never Run |

**Default Value**: 271500003 (Never Run)

---

## Table 2: Exchange Rate Log (fw_exchangeratelog)

### Table Properties
```yaml
Schema Name: fw_exchangeratelog
Display Name: Exchange Rate Log
Plural Name: Exchange Rate Logs
Description: Audit log of exchange rate update operations
Ownership: Organization
Primary Column: fw_name
Track Changes: No
Enable Auditing: No
```

### Columns

| Display Name | Schema Name | Type | Required | Description |
|--------------|-------------|------|----------|-------------|
| Name | fw_name | Text (200) | Yes | Auto-generated log name |
| Updated On | fw_updatedon | Date/Time | No | When the update occurred |
| Status | fw_status | Choice | No | Update result status |
| Currencies Updated | fw_currenciesupdated | Whole Number | No | Count of successfully updated |
| Currencies Failed | fw_currenciesfailed | Whole Number | No | Count of failures |
| Message | fw_message | Text Area (4000) | No | Detailed log message |
| Base Currency | fw_basecurrency | Text (10) | No | Base currency code used |
| Trigger Type | fw_triggertype | Choice | No | How update was triggered |

### Choice: Status (fw_status)

| Value | Label |
|-------|-------|
| 271500000 | Success |
| 271500001 | Failed |
| 271500002 | Partial |

### Choice: Trigger Type (fw_triggertype)

| Value | Label |
|-------|-------|
| 271500000 | Manual |
| 271500001 | Scheduled |

---

## Table 3: Currency Sync (fw_currencysync)

### Table Properties
```yaml
Schema Name: fw_currencysync
Display Name: Currency Sync
Plural Name: Currency Syncs
Description: Tracks which currencies to include in sync and their last values
Ownership: Organization
Primary Column: fw_name
Track Changes: No
Enable Auditing: No
```

### Columns

| Display Name | Schema Name | Type | Required | Description |
|--------------|-------------|------|----------|-------------|
| Currency Code | fw_name | Text (10) | Yes | ISO currency code |
| Transaction Currency | fw_transactioncurrencyid | Lookup | No | Link to Transaction Currency |
| Enabled | fw_enabled | Yes/No | No | Include in sync (Default: Yes) |
| Last Rate | fw_lastrate | Decimal (10,6) | No | Last synced exchange rate |
| Last Updated | fw_lastupdated | Date/Time | No | When rate was last updated |

### Lookup: Transaction Currency
```yaml
Schema Name: fw_transactioncurrencyid
Related Table: transactioncurrency
Relationship Type: Many-to-One
Relationship Name: fw_currencysync_transactioncurrency
```

---

## Environment Variable

### Exchange Rate API Key

```yaml
Schema Name: fw_ExchangeRateApiKey
Display Name: Exchange Rate API Key
Type: Text
Description: API key for ExchangeRate-API service. Get a free key at https://app.exchangerate-api.com/sign-up
Default Value: (empty)
```

---

## Views

### Exchange Rate Setting Views

#### Active Settings (Default)
```yaml
Name: Active Exchange Rate Settings
Filter: statecode eq 0
Columns: Name, Enabled, Update Frequency, Last Updated, Last Update Status
Sort: Name ascending
```

### Exchange Rate Log Views

#### Recent Logs (Default)
```yaml
Name: Recent Exchange Rate Logs
Filter: statecode eq 0
Columns: Name, Updated On, Status, Currencies Updated, Base Currency, Trigger Type
Sort: Updated On descending
```

#### Failed Updates
```yaml
Name: Failed Exchange Rate Updates
Filter: statecode eq 0 and fw_status eq 271500001
Columns: Name, Updated On, Message, Base Currency
Sort: Updated On descending
```

### Currency Sync Views

#### All Currencies (Default)
```yaml
Name: All Currency Syncs
Filter: statecode eq 0
Columns: Currency Code, Transaction Currency, Enabled, Last Rate, Last Updated
Sort: Currency Code ascending
```

#### Enabled Currencies
```yaml
Name: Enabled Currency Syncs
Filter: statecode eq 0 and fw_enabled eq true
Columns: Currency Code, Transaction Currency, Last Rate, Last Updated
Sort: Currency Code ascending
```

---

## Forms

### Exchange Rate Setting - Main Form

```yaml
Name: Main Form
Type: Main
Columns: 2

Header:
  - fw_lastupdatestatus (read-only)

Tab 1: General
  Section 1.1: API Configuration
    - fw_apikey
    - fw_enabled
    
  Section 1.2: Schedule
    - fw_updatefrequency
    - fw_syncallcurrencies
    
  Section 1.3: Status (read-only)
    - fw_lastupdated
    - fw_lastupdatemessage

Tab 2: Currencies
  Section 2.1: Currency Selection
    - Subgrid: Currency Syncs (fw_currencysync)
      View: All Currency Syncs
      Editable: Yes

Tab 3: History
  Section 3.1: Update History
    - Subgrid: Exchange Rate Logs (fw_exchangeratelog)
      View: Recent Exchange Rate Logs
      Editable: No
```

### Exchange Rate Log - Main Form

```yaml
Name: Main Form
Type: Main
Columns: 2
Read-Only: Yes

Tab 1: Details
  Section 1.1: Summary
    - fw_name
    - fw_updatedon
    - fw_status
    - fw_triggertype
    
  Section 1.2: Results
    - fw_basecurrency
    - fw_currenciesupdated
    - fw_currenciesfailed
    
  Section 1.3: Message
    - fw_message (full width)
```

### Currency Sync - Quick Create Form

```yaml
Name: Quick Create
Type: Quick Create

Fields:
  - fw_name
  - fw_transactioncurrencyid
  - fw_enabled
```

---

## Security Roles

### Exchange Rate Administrator

```yaml
Name: Exchange Rate Administrator
Business Unit: Organization

Privileges:
  fw_exchangeratesetting:
    Create: Organization
    Read: Organization
    Write: Organization
    Delete: Organization
    Append: Organization
    AppendTo: Organization
    
  fw_exchangeratelog:
    Create: Organization
    Read: Organization
    Write: Organization
    Delete: Organization
    
  fw_currencysync:
    Create: Organization
    Read: Organization
    Write: Organization
    Delete: Organization
    Append: Organization
    AppendTo: Organization
    
  transactioncurrency:
    Read: Organization
    Write: Organization
```

### Exchange Rate User

```yaml
Name: Exchange Rate User
Business Unit: Organization

Privileges:
  fw_exchangeratesetting:
    Read: Organization
    
  fw_exchangeratelog:
    Read: Organization
    
  fw_currencysync:
    Read: Organization
    
  transactioncurrency:
    Read: Organization
```

---

## Sitemap Configuration

Add to RapidStart Settings sitemap:

```xml
<Group Id="fw_exchangerates" 
       ResourceId="SitemapDesigner.NewGroup" 
       Title="Exchange Rates"
       DescriptionResourceId="SitemapDesigner.NewGroup"
       IsProfile="false"
       ToolTip="Exchange Rates">
       
  <SubArea Id="fw_exchangeratesetting"
           Entity="fw_exchangeratesetting"
           Title="Settings"
           Icon="/WebResources/fw_icon_exchangerate_settings"
           GetStartedPanePath="$webresource:fw_exchangerate_gettingstarted.html" />
           
  <SubArea Id="fw_exchangeratelog"
           Entity="fw_exchangeratelog"
           Title="Update History"
           Icon="/WebResources/fw_icon_exchangerate_history" />
           
  <SubArea Id="fw_currencysync"
           Entity="fw_currencysync"
           Title="Currency Selection"
           Icon="/WebResources/fw_icon_exchangerate_currency" />
</Group>
```

---

## Reference Data

### Default Setting Record (created on solution import)

```json
{
  "fw_name": "Exchange Rate Settings",
  "fw_enabled": false,
  "fw_updatefrequency": 271500003,
  "fw_syncallcurrencies": true,
  "fw_lastupdatestatus": 271500003,
  "fw_lastupdatemessage": "Not yet configured. Enter your API key and enable sync to get started."
}
```
