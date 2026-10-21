# QuickBooks Online (Independent Publisher)

> **Important:** This is an independent publisher connector created by Forceworks. It is not affiliated with, endorsed by, or supported by Intuit Inc. For official QuickBooks integrations, please visit the [Intuit App Store](https://quickbooks.intuit.com/app/apps/home/).

A comprehensive Power Platform connector for QuickBooks Online, providing access to accounting data including customers, vendors, invoices, bills, payments, items, and financial reports.

## Publisher: Forceworks

**Website:** https://www.forceworks.com  
**Support Contact:** connect@forceworks.com

## Prerequisites

1. A QuickBooks Online account (any tier: Simple Start, Essentials, Plus, or Advanced)
2. An Intuit Developer account at [developer.intuit.com](https://developer.intuit.com)
3. An app created in the Intuit Developer Portal with OAuth 2.0 credentials

## Supported Operations

This connector provides **60 operations** organized into the following categories:

### Query & Sync Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **Query** | GET | Execute SQL-like queries against any QuickBooks entity |
| **GetChanges** | GET | Change Data Capture (CDC) - get entities changed since a date |
| **Batch** | POST | Execute up to 30 operations in a single request |

### Customer Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetCustomer** | GET | Retrieve a customer by ID |
| **CreateOrUpdateCustomer** | POST | Create a new customer or update existing (include Id and SyncToken to update) |

### Vendor Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetVendor** | GET | Retrieve a vendor by ID |
| **CreateOrUpdateVendor** | POST | Create a new vendor or update existing |

### Invoice Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetInvoice** | GET | Retrieve an invoice by ID |
| **GetInvoicePDF** | GET | Download invoice as PDF |
| **CreateOrUpdateInvoice** | POST | Create a new invoice or update existing |
| **SendInvoice** | POST | Email an invoice to the customer |

### Bill Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetBill** | GET | Retrieve a bill by ID |
| **CreateOrUpdateBill** | POST | Create a new bill or update existing |

### Payment Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetPayment** | GET | Retrieve a customer payment by ID |
| **CreateOrUpdatePayment** | POST | Record a customer payment or update existing |
| **GetBillPayment** | GET | Retrieve a bill payment by ID |
| **CreateOrUpdateBillPayment** | POST | Record a vendor payment or update existing |

### Estimate Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetEstimate** | GET | Retrieve an estimate by ID |
| **GetEstimatePDF** | GET | Download estimate as PDF |
| **CreateOrUpdateEstimate** | POST | Create a new estimate or update existing |
| **SendEstimate** | POST | Email an estimate to the customer |

### Sales Receipt Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetSalesReceipt** | GET | Retrieve a sales receipt by ID |
| **GetSalesReceiptPDF** | GET | Download sales receipt as PDF |
| **CreateOrUpdateSalesReceipt** | POST | Create a sales receipt or update existing |
| **SendSalesReceipt** | POST | Email a sales receipt |

### Credit & Refund Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetCreditMemo** | GET | Retrieve a credit memo by ID |
| **CreateOrUpdateCreditMemo** | POST | Create a credit memo or update existing |
| **GetRefundReceipt** | GET | Retrieve a refund receipt by ID |
| **CreateOrUpdateRefundReceipt** | POST | Create a refund receipt or update existing |
| **GetVendorCredit** | GET | Retrieve a vendor credit by ID |
| **CreateOrUpdateVendorCredit** | POST | Create a vendor credit or update existing |

### Purchase Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetPurchase** | GET | Retrieve a purchase/expense by ID |
| **CreateOrUpdatePurchase** | POST | Create an expense, check, or credit card charge |
| **GetPurchaseOrder** | GET | Retrieve a purchase order by ID |
| **CreateOrUpdatePurchaseOrder** | POST | Create a purchase order or update existing |
| **SendPurchaseOrder** | POST | Email a purchase order to vendor |

### Item Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetItem** | GET | Retrieve a product or service item by ID |
| **CreateOrUpdateItem** | POST | Create an item or update existing |

### Account Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetAccount** | GET | Retrieve a chart of accounts entry by ID |
| **CreateOrUpdateAccount** | POST | Create an account or update existing |

### Journal & Banking Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetJournalEntry** | GET | Retrieve a journal entry by ID |
| **CreateOrUpdateJournalEntry** | POST | Create a journal entry or update existing |
| **GetDeposit** | GET | Retrieve a deposit by ID |
| **CreateOrUpdateDeposit** | POST | Create a deposit or update existing |
| **GetTransfer** | GET | Retrieve a transfer by ID |
| **CreateOrUpdateTransfer** | POST | Create an account transfer or update existing |

### Employee Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetEmployee** | GET | Retrieve an employee by ID |
| **CreateOrUpdateEmployee** | POST | Create an employee or update existing |

### Company Operations
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetCompanyInfo** | GET | Get company information |
| **GetPreferences** | GET | Get company preferences and settings |

### Financial Reports
| Operation | Method | Description |
|-----------|--------|-------------|
| **GetProfitAndLoss** | GET | Profit and Loss (Income Statement) report |
| **GetBalanceSheet** | GET | Balance Sheet report |
| **GetCashFlow** | GET | Statement of Cash Flows |
| **GetTrialBalance** | GET | Trial Balance report |
| **GetGeneralLedger** | GET | General Ledger report |
| **GetAgedReceivables** | GET | Aged Receivables (AR Aging) report |
| **GetAgedPayables** | GET | Aged Payables (AP Aging) report |
| **GetCustomerBalance** | GET | Customer Balance Summary report |
| **GetVendorBalance** | GET | Vendor Balance Summary report |
| **GetTransactionList** | GET | Transaction List report |

## Obtaining Credentials

### Step 1: Create an Intuit Developer Account

1. Go to [developer.intuit.com](https://developer.intuit.com)
2. Click **Sign Up** in the top right corner
3. Create a free developer account using your email or Intuit account
4. Verify your email address

### Step 2: Create a QuickBooks App

1. Log in to the [Intuit Developer Portal](https://developer.intuit.com)
2. Click **Dashboard** in the top navigation
3. Click **Create an app**
4. Select **QuickBooks Online and Payments**
5. Enter an app name (e.g., "Power Automate Connector")
6. Select the scope: **Accounting** (com.intuit.quickbooks.accounting)
7. Click **Create app**

### Step 3: Configure OAuth 2.0 Settings

1. In your app dashboard, go to **Keys & OAuth**
2. You'll see two sets of credentials:
   - **Development** (for sandbox testing)
   - **Production** (for live data)
3. Copy the **Client ID** and **Client Secret** for your environment
4. Under **Redirect URIs**, click **Add URI**
5. Enter exactly: `https://global.consent.azure-apim.net/redirect`
6. Click **Save**

### Step 4: Configure the Custom Connector

When creating the connection in Power Platform, use these OAuth settings:

| Setting | Value |
|---------|-------|
| Authorization URL | `https://appcenter.intuit.com/connect/oauth2` |
| Token URL | `https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer` |
| Refresh URL | `https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer` |
| Scope | `com.intuit.quickbooks.accounting` |
| Client ID | *(from Step 3)* |
| Client Secret | *(from Step 3)* |

### Step 5: Get Your Company ID (Realm ID)

The Company ID (also called Realm ID or realmId) is required for every API call. Here's how to find it:

**Method 1: From OAuth Redirect URL**
After authorizing the connection, check the redirect URL:
```
https://...?realmId=9341456161184198&...
```
The number after `realmId=` is your Company ID.

**Method 2: From QuickBooks Online**
1. Log in to QuickBooks Online
2. Go to **Settings** (gear icon) → **Account and Settings**
3. Look at your browser's URL bar
4. The number in the URL is your Company ID

**Method 3: From Intuit Developer Portal**
1. Go to Dashboard → Sandbox (for test companies)
2. The Company ID is displayed for each sandbox company

**Example Company ID:** `9341456161184198`

> **Important:** Store your Company ID securely. You'll enter it for each operation in your flows.

### Sandbox vs Production

| Environment | Host | Use |
|-------------|------|-----|
| **Sandbox** | `sandbox-quickbooks.api.intuit.com` | Testing with fake data |
| **Production** | `quickbooks.api.intuit.com` | Real company data |

To switch environments, change the **Host** in the connector's General tab.

## Key Concepts

### SyncToken (Required for Updates)

Every QuickBooks record has a `SyncToken` that tracks versions. When updating:

1. First, **GET** the current record to obtain the latest SyncToken
2. Include the SyncToken in your update request
3. If the SyncToken doesn't match (someone else modified it), the update fails

### Sparse Updates

Set `sparse: true` in your request body for partial updates:
- `sparse: true` → Only fields you send are updated
- `sparse: false` → Fields you don't send are cleared

**Recommendation:** Always use `sparse: true` for updates.

### Query Syntax

The Query operation uses QuickBooks' SQL-like syntax:

```sql
SELECT * FROM Customer WHERE Active = true MAXRESULTS 100
SELECT * FROM Invoice WHERE TotalAmt > '1000' AND TxnDate > '2024-01-01'
SELECT * FROM Item WHERE Type = 'Service'
SELECT Id, DisplayName FROM Customer WHERE DisplayName LIKE '%Smith%'
```

**Limitations:**
- No OR operator (use multiple queries)
- No GROUP BY or JOIN
- String values require single quotes
- Maximum 1000 results per query

## Known Issues and Limitations

1. **Void and Delete operations** are not included in this connector version. Use the QuickBooks UI for these operations.
2. **PDF downloads** return binary content - use appropriate Power Automate actions to handle file output.
3. **Reports** have a maximum of 400,000 cells. Reduce the date range if you exceed this limit.
4. **CDC (Change Data Capture)** has a maximum 30-day lookback period.
5. **Rate limits:** Maximum 500 requests per minute per company.

## API Limits

| Limit | Value |
|-------|-------|
| Requests per minute | 500 |
| Batch operations per request | 30 |
| Query results per request | 1000 |
| CDC lookback period | 30 days |
| Access token lifetime | 1 hour (auto-refreshed) |
| Refresh token lifetime | 100 days of inactivity |

## Frequently Asked Questions

**Q: Why do I get "ApplicationAuthorizationFailed" (Error 3100)?**
A: The Company ID doesn't match the authorized company. Re-create the connection and ensure the Company ID matches the company you selected during OAuth.

**Q: Why do updates fail with "Stale Object" (Error 5010)?**
A: The SyncToken is outdated. Fetch the latest record first to get the current SyncToken, then retry.

**Q: How do I find a Customer/Vendor/Item ID?**
A: Use the Query operation:
```sql
SELECT Id, DisplayName FROM Customer WHERE DisplayName = 'Customer Name'
```

## Support

- **Connector Issues:** connect@forceworks.com
- **QuickBooks API Documentation:** [developer.intuit.com](https://developer.intuit.com/app/developer/qbo/docs/get-started)
- **Power Platform Connectors:** [GitHub Repository](https://github.com/microsoft/PowerPlatformConnectors)
