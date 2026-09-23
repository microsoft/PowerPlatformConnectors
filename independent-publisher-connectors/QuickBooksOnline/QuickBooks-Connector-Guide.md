# QuickBooks Online Connector for Power Automate

A comprehensive guide to setting up and using the QuickBooks Online custom connector for Microsoft Power Automate.

---

## Table of Contents

1. [Overview](#overview)
2. [Setup](#setup)
   - [Prerequisites](#prerequisites)
   - [Create Intuit Developer Account](#create-intuit-developer-account)
   - [Create QuickBooks App](#create-quickbooks-app)
   - [Import Custom Connector](#import-custom-connector)
   - [Configure OAuth](#configure-oauth)
   - [Create Connection](#create-connection)
3. [Key Concepts](#key-concepts)
   - [Company ID (realmId)](#company-id-realmid)
   - [SyncToken](#synctoken)
   - [Sparse Updates](#sparse-updates)
   - [References (Refs)](#references-refs)
4. [Actions Reference](#actions-reference)
   - [Query Operations](#query-operations)
   - [Customer Operations](#customer-operations)
   - [Vendor Operations](#vendor-operations)
   - [Invoice Operations](#invoice-operations)
   - [Bill Operations](#bill-operations)
   - [Payment Operations](#payment-operations)
   - [Bill Payment Operations](#bill-payment-operations)
   - [Estimate Operations](#estimate-operations)
   - [Sales Receipt Operations](#sales-receipt-operations)
   - [Credit Memo Operations](#credit-memo-operations)
   - [Refund Receipt Operations](#refund-receipt-operations)
   - [Vendor Credit Operations](#vendor-credit-operations)
   - [Purchase Operations](#purchase-operations)
   - [Purchase Order Operations](#purchase-order-operations)
   - [Item Operations](#item-operations)
   - [Account Operations](#account-operations)
   - [Journal Entry Operations](#journal-entry-operations)
   - [Deposit Operations](#deposit-operations)
   - [Transfer Operations](#transfer-operations)
   - [Employee Operations](#employee-operations)
   - [Company Operations](#company-operations)
   - [Report Operations](#report-operations)
   - [Advanced Operations](#advanced-operations)
5. [Common Patterns](#common-patterns)
6. [Error Handling](#error-handling)
7. [Best Practices](#best-practices)

---

## Overview

This connector provides 60 operations to interact with QuickBooks Online, including:

- **CRUD operations** for customers, vendors, invoices, bills, payments, items, and more
- **Financial reports** including P&L, Balance Sheet, Aged Receivables/Payables
- **Query capability** using SQL-like syntax
- **Batch operations** for bulk processing
- **Change Data Capture (CDC)** for sync scenarios

---

## Setup

### Prerequisites

- QuickBooks Online account (Plus, Essentials, or Simple Start)
- Microsoft Power Automate license
- Intuit Developer account (free)

### Create Intuit Developer Account

1. Go to [developer.intuit.com](https://developer.intuit.com)
2. Click **Sign Up** and create an account
3. Verify your email address

### Create QuickBooks App

1. Log in to [Intuit Developer Portal](https://developer.intuit.com)
2. Go to **Dashboard** → **Create an app**
3. Select **QuickBooks Online and Payments**
4. Enter app name (e.g., "Power Automate Connector")
5. Select scopes: **Accounting**
6. Click **Create app**

**Get Credentials:**
1. Go to **Keys & OAuth** tab
2. Copy the **Client ID** and **Client Secret**
3. Note: Use **Development** keys for sandbox, **Production** keys for live data

**Add Redirect URI:**
1. In **Redirect URIs** section, add:
   ```
   https://global.consent.azure-apim.net/redirect
   ```
2. Click **Save**

### Import Custom Connector

1. Go to [Power Automate](https://make.powerautomate.com)
2. Navigate to **Data** → **Custom connectors**
3. Click **+ New custom connector** → **Import an OpenAPI file**
4. Name: `QuickBooks Online`
5. Upload `apiDefinition-v2.swagger.json`
6. Click **Continue**

**Important:** After importing, you also need to upload the `apiProperties.json` file:
1. In the custom connector editor, go to **General** tab
2. Click **Upload** under the swagger editor
3. Select `apiProperties.json` to apply connection settings and policies

### Configure OAuth

In the **Security** tab, verify these settings are populated (from apiProperties.json):

| Field | Value |
|-------|-------|
| Authentication | OAuth 2.0 |
| Identity Provider | Generic Oauth 2 |
| Client ID | *(enter from Intuit Developer Portal)* |
| Client Secret | *(enter from Intuit Developer Portal)* |
| Authorization URL | `https://appcenter.intuit.com/connect/oauth2` |
| Token URL | `https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer` |
| Refresh URL | `https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer` |
| Scope | `com.intuit.quickbooks.accounting` |

Click **Create connector** (or **Update connector** if editing)

### Create Connection

1. Go to **Test** tab
2. Click **+ New connection**
3. Enter your **Company ID (Realm ID)** - this is your QuickBooks company identifier
4. Click **Create**
5. Sign in with your QuickBooks credentials
6. Select the company to connect (must match the Company ID you entered)
7. Click **Authorize**

**Finding your Company ID:**
- After OAuth authorization, it appears in the URL as `realmId=XXXXXXXXXX`
- In QuickBooks: **Settings** → **Account and Settings** → check browser URL
- In Intuit Developer Portal: **Sandbox** section shows sandbox company IDs

**Note:** The Company ID is now stored with the connection, so you don't need to enter it for each action!

**For Sandbox Testing:**
Change the **Host** in General tab to: `sandbox-quickbooks.api.intuit.com`

---

## Key Concepts

### Company ID (realmId)

The Company ID (also called realmId) identifies which QuickBooks company you're accessing. 

**With this connector, the Company ID is stored in the connection** - you enter it once when creating the connection and don't need to specify it for each action.

**How to find your Company ID:**
- Appears in URL after OAuth authorization: `...&realmId=9341456161184198&...`
- In QuickBooks: **Settings** → **Account and Settings** → look at browser URL
- In Intuit Developer Portal: **Sandbox** section shows sandbox company IDs

**Example:** `9341456161184198`

**Note:** If you work with multiple QuickBooks companies, create a separate connection for each one.

### SyncToken

A version number that tracks changes to a record. Required for updates to prevent overwriting concurrent changes.

**How it works:**
1. Get a record → SyncToken is "0"
2. Update the record → SyncToken becomes "1"
3. Someone else updates → SyncToken becomes "2"
4. Your update with SyncToken "1" fails (stale data)

**Best practice:** Always fetch the latest record before updating to get the current SyncToken.

### Sparse Updates

Controls whether an update is partial or full.

| sparse | Behavior |
|--------|----------|
| `true` | Only fields you send are updated (recommended) |
| `false` | Fields you don't send are cleared/reset to defaults |

**Example - Update only phone number:**
```json
{
  "Id": "58",
  "SyncToken": "0",
  "sparse": true,
  "PrimaryPhone": {
    "FreeFormNumber": "555-123-4567"
  }
}
```

### References (Refs)

Many fields reference other entities using a Ref object:

```json
{
  "CustomerRef": {
    "value": "58",
    "name": "Test Customer"
  }
}
```

- `value` (required): The ID of the referenced entity
- `name` (optional): Display name for readability

**Common Refs:**
- `CustomerRef` - References a customer
- `VendorRef` - References a vendor
- `ItemRef` - References a product/service item
- `AccountRef` - References a chart of accounts entry

---

## Actions Reference

### Query Operations

#### Query

Execute SQL-like queries against any QuickBooks entity.

**When to use:** Finding records by criteria, listing entities, searching

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| Query | Yes | SQL-like query string |

**Supported Entities:**
Customer, Vendor, Invoice, Bill, Payment, Item, Account, Employee, Estimate, SalesReceipt, CreditMemo, PurchaseOrder, JournalEntry, Deposit, Transfer, and more.

**Query Syntax:**
```sql
SELECT * FROM EntityName WHERE condition ORDERBY field MAXRESULTS n
```

**Examples:**

*Get all active customers:*
```sql
SELECT * FROM Customer WHERE Active = true
```

*Find invoices over $1000:*
```sql
SELECT * FROM Invoice WHERE TotalAmt > '1000'
```

*Search customer by name:*
```sql
SELECT * FROM Customer WHERE DisplayName LIKE '%Smith%'
```

*Get recent invoices:*
```sql
SELECT * FROM Invoice WHERE TxnDate > '2024-01-01' ORDERBY TxnDate DESC MAXRESULTS 50
```

*Find unpaid invoices:*
```sql
SELECT * FROM Invoice WHERE Balance > '0'
```

*Get items by type:*
```sql
SELECT * FROM Item WHERE Type = 'Service'
```

**Limitations:**
- No OR operator (use multiple queries)
- No GROUP BY or aggregations
- Maximum 1000 results per query
- String values must be in single quotes

---

### Customer Operations

#### GetCustomer

Retrieve a single customer by ID.

**When to use:** Getting full details of a known customer, fetching before update

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| ID | Yes | Customer ID |

**Example Response:**
```json
{
  "Customer": {
    "Id": "58",
    "SyncToken": "1",
    "DisplayName": "Test Customer 001",
    "PrimaryEmailAddr": {
      "Address": "test@example.com"
    },
    "PrimaryPhone": {
      "FreeFormNumber": "555-123-4567"
    },
    "Balance": 150.00,
    "Active": true
  }
}
```

#### CreateOrUpdateCustomer

Create a new customer or update an existing one.

**When to use:** 
- Create: Adding new customers from forms, imports, or integrations
- Update: Changing contact info, addresses, settings

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| Body | Yes | Customer object |

**Create Example:**
```json
{
  "DisplayName": "Acme Corporation",
  "CompanyName": "Acme Corporation",
  "GivenName": "John",
  "FamilyName": "Doe",
  "PrimaryEmailAddr": {
    "Address": "john@acme.com"
  },
  "PrimaryPhone": {
    "FreeFormNumber": "555-987-6543"
  },
  "BillAddr": {
    "Line1": "123 Main St",
    "City": "San Francisco",
    "CountrySubDivisionCode": "CA",
    "PostalCode": "94105"
  }
}
```

**Update Example:**
```json
{
  "Id": "58",
  "SyncToken": "1",
  "sparse": true,
  "PrimaryPhone": {
    "FreeFormNumber": "555-NEW-NUMB"
  }
}
```

---

### Vendor Operations

#### GetVendor

Retrieve a single vendor by ID.

**When to use:** Getting vendor details, fetching before update

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| ID | Yes | Vendor ID |

#### CreateOrUpdateVendor

Create a new vendor or update an existing one.

**When to use:** Adding suppliers, updating vendor information

**Create Example:**
```json
{
  "DisplayName": "Office Supplies Inc",
  "CompanyName": "Office Supplies Inc",
  "PrimaryEmailAddr": {
    "Address": "orders@officesupplies.com"
  },
  "Vendor1099": false
}
```

**Update Example (set as 1099 vendor):**
```json
{
  "Id": "42",
  "SyncToken": "0",
  "sparse": true,
  "Vendor1099": true,
  "TaxIdentifier": "12-3456789"
}
```

---

### Invoice Operations

#### GetInvoice

Retrieve a single invoice by ID.

**When to use:** Getting invoice details, checking status/balance

#### CreateOrUpdateInvoice

Create a new invoice or update an existing one.

**When to use:** Billing customers, modifying invoices

**Create Example:**
```json
{
  "CustomerRef": {
    "value": "58"
  },
  "TxnDate": "2024-01-15",
  "DueDate": "2024-02-15",
  "Line": [
    {
      "Amount": 150.00,
      "DetailType": "SalesItemLineDetail",
      "SalesItemLineDetail": {
        "ItemRef": {
          "value": "1"
        },
        "Qty": 3,
        "UnitPrice": 50.00
      },
      "Description": "Consulting services"
    }
  ],
  "BillEmail": {
    "Address": "customer@example.com"
  }
}
```

**Update Example (change due date):**
```json
{
  "Id": "101",
  "SyncToken": "0",
  "sparse": true,
  "DueDate": "2024-03-01"
}
```

#### SendInvoice

Send an invoice to the customer via email.

**When to use:** After creating invoice, for reminders

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| ID | Yes | Invoice ID |
| Send To Email | No | Override recipient email |

#### GetInvoicePDF

Download the invoice as a PDF file.

**When to use:** Archiving, attaching to emails via other systems

---

### Bill Operations

#### GetBill

Retrieve a single bill by ID.

**When to use:** Getting bill details, checking what's owed

#### CreateOrUpdateBill

Create a new bill or update an existing one.

**When to use:** Recording bills from vendors

**Create Example:**
```json
{
  "VendorRef": {
    "value": "42"
  },
  "TxnDate": "2024-01-10",
  "DueDate": "2024-02-10",
  "Line": [
    {
      "Amount": 500.00,
      "DetailType": "AccountBasedExpenseLineDetail",
      "AccountBasedExpenseLineDetail": {
        "AccountRef": {
          "value": "7"
        }
      },
      "Description": "Office supplies"
    }
  ]
}
```

---

### Payment Operations

#### GetPayment

Retrieve a customer payment by ID.

#### CreateOrUpdatePayment

Record a customer payment.

**When to use:** Recording payments received, applying to invoices

**Create Example (apply to specific invoice):**
```json
{
  "CustomerRef": {
    "value": "58"
  },
  "TotalAmt": 150.00,
  "TxnDate": "2024-01-20",
  "Line": [
    {
      "Amount": 150.00,
      "LinkedTxn": [
        {
          "TxnId": "101",
          "TxnType": "Invoice"
        }
      ]
    }
  ],
  "DepositToAccountRef": {
    "value": "35"
  }
}
```

**Create Example (unapplied payment):**
```json
{
  "CustomerRef": {
    "value": "58"
  },
  "TotalAmt": 500.00,
  "TxnDate": "2024-01-20"
}
```

---

### Bill Payment Operations

#### GetBillPayment

Retrieve a bill payment by ID.

#### CreateOrUpdateBillPayment

Record a payment to a vendor.

**When to use:** Paying bills

**Create Example (pay by check):**
```json
{
  "VendorRef": {
    "value": "42"
  },
  "TotalAmt": 500.00,
  "PayType": "Check",
  "TxnDate": "2024-01-25",
  "CheckPayment": {
    "BankAccountRef": {
      "value": "35"
    }
  },
  "Line": [
    {
      "Amount": 500.00,
      "LinkedTxn": [
        {
          "TxnId": "201",
          "TxnType": "Bill"
        }
      ]
    }
  ]
}
```

---

### Estimate Operations

#### GetEstimate

Retrieve an estimate by ID.

#### CreateOrUpdateEstimate

Create or update an estimate (quote).

**When to use:** Creating quotes for customers

**Create Example:**
```json
{
  "CustomerRef": {
    "value": "58"
  },
  "TxnDate": "2024-01-15",
  "ExpirationDate": "2024-02-15",
  "Line": [
    {
      "Amount": 1000.00,
      "DetailType": "SalesItemLineDetail",
      "SalesItemLineDetail": {
        "ItemRef": {
          "value": "1"
        },
        "Qty": 10,
        "UnitPrice": 100.00
      }
    }
  ]
}
```

#### SendEstimate

Email the estimate to the customer.

#### GetEstimatePDF

Download estimate as PDF.

---

### Sales Receipt Operations

#### GetSalesReceipt

Retrieve a sales receipt by ID.

#### CreateOrUpdateSalesReceipt

Create a sales receipt for immediate payment (cash sale).

**When to use:** Point of sale, cash transactions, immediate payment

**Create Example:**
```json
{
  "CustomerRef": {
    "value": "58"
  },
  "TxnDate": "2024-01-15",
  "Line": [
    {
      "Amount": 75.00,
      "DetailType": "SalesItemLineDetail",
      "SalesItemLineDetail": {
        "ItemRef": {
          "value": "2"
        },
        "Qty": 1,
        "UnitPrice": 75.00
      }
    }
  ],
  "PaymentMethodRef": {
    "value": "1"
  },
  "DepositToAccountRef": {
    "value": "35"
  }
}
```

#### SendSalesReceipt

Email the sales receipt to the customer.

#### GetSalesReceiptPDF

Download sales receipt as PDF.

---

### Credit Memo Operations

#### GetCreditMemo

Retrieve a credit memo by ID.

#### CreateOrUpdateCreditMemo

Create a credit memo (customer credit).

**When to use:** Returns, adjustments, credits to customer account

**Create Example:**
```json
{
  "CustomerRef": {
    "value": "58"
  },
  "TxnDate": "2024-01-20",
  "Line": [
    {
      "Amount": 50.00,
      "DetailType": "SalesItemLineDetail",
      "SalesItemLineDetail": {
        "ItemRef": {
          "value": "1"
        },
        "Qty": 1,
        "UnitPrice": 50.00
      },
      "Description": "Return - defective item"
    }
  ]
}
```

---

### Refund Receipt Operations

#### GetRefundReceipt

Retrieve a refund receipt by ID.

#### CreateOrUpdateRefundReceipt

Create a refund receipt (money returned to customer).

**When to use:** Cash refunds, returning money to customer

---

### Vendor Credit Operations

#### GetVendorCredit

Retrieve a vendor credit by ID.

#### CreateOrUpdateVendorCredit

Create a vendor credit (credit from vendor).

**When to use:** Recording credits received from vendors

**Create Example:**
```json
{
  "VendorRef": {
    "value": "42"
  },
  "TxnDate": "2024-01-20",
  "Line": [
    {
      "Amount": 100.00,
      "DetailType": "AccountBasedExpenseLineDetail",
      "AccountBasedExpenseLineDetail": {
        "AccountRef": {
          "value": "7"
        }
      },
      "Description": "Credit for returned items"
    }
  ]
}
```

---

### Purchase Operations

#### GetPurchase

Retrieve a purchase/expense by ID.

#### CreateOrUpdatePurchase

Create an expense, check, or credit card charge.

**When to use:** Recording expenses, writing checks, credit card purchases

**Create Example (expense/cash):**
```json
{
  "PaymentType": "Cash",
  "AccountRef": {
    "value": "35"
  },
  "TxnDate": "2024-01-15",
  "EntityRef": {
    "value": "42",
    "type": "Vendor"
  },
  "Line": [
    {
      "Amount": 45.00,
      "DetailType": "AccountBasedExpenseLineDetail",
      "AccountBasedExpenseLineDetail": {
        "AccountRef": {
          "value": "13"
        }
      },
      "Description": "Office coffee"
    }
  ]
}
```

**Create Example (check):**
```json
{
  "PaymentType": "Check",
  "AccountRef": {
    "value": "35"
  },
  "DocNumber": "1234",
  "TxnDate": "2024-01-15",
  "Line": [
    {
      "Amount": 200.00,
      "DetailType": "AccountBasedExpenseLineDetail",
      "AccountBasedExpenseLineDetail": {
        "AccountRef": {
          "value": "7"
        }
      }
    }
  ]
}
```

**Create Example (credit card):**
```json
{
  "PaymentType": "CreditCard",
  "AccountRef": {
    "value": "41"
  },
  "TxnDate": "2024-01-15",
  "Line": [
    {
      "Amount": 150.00,
      "DetailType": "AccountBasedExpenseLineDetail",
      "AccountBasedExpenseLineDetail": {
        "AccountRef": {
          "value": "15"
        }
      },
      "Description": "Software subscription"
    }
  ]
}
```

---

### Purchase Order Operations

#### GetPurchaseOrder

Retrieve a purchase order by ID.

#### CreateOrUpdatePurchaseOrder

Create or update a purchase order.

**When to use:** Ordering from vendors

**Create Example:**
```json
{
  "VendorRef": {
    "value": "42"
  },
  "TxnDate": "2024-01-15",
  "Line": [
    {
      "Amount": 500.00,
      "DetailType": "ItemBasedExpenseLineDetail",
      "ItemBasedExpenseLineDetail": {
        "ItemRef": {
          "value": "5"
        },
        "Qty": 10,
        "UnitPrice": 50.00
      }
    }
  ]
}
```

#### SendPurchaseOrder

Email the purchase order to the vendor.

---

### Item Operations

#### GetItem

Retrieve a product or service item by ID.

#### CreateOrUpdateItem

Create or update an item.

**When to use:** Adding products/services, updating prices

**Create Service Item:**
```json
{
  "Name": "Consulting",
  "Type": "Service",
  "UnitPrice": 150.00,
  "IncomeAccountRef": {
    "value": "1"
  }
}
```

**Create Inventory Item:**
```json
{
  "Name": "Widget",
  "Type": "Inventory",
  "UnitPrice": 25.00,
  "PurchaseCost": 10.00,
  "TrackQtyOnHand": true,
  "QtyOnHand": 100,
  "InvStartDate": "2024-01-01",
  "IncomeAccountRef": {
    "value": "1"
  },
  "ExpenseAccountRef": {
    "value": "7"
  },
  "AssetAccountRef": {
    "value": "8"
  }
}
```

**Create Non-Inventory Item:**
```json
{
  "Name": "Shipping",
  "Type": "NonInventory",
  "UnitPrice": 15.00,
  "IncomeAccountRef": {
    "value": "1"
  }
}
```

---

### Account Operations

#### GetAccount

Retrieve a chart of accounts entry by ID.

#### CreateOrUpdateAccount

Create or update an account.

**When to use:** Setting up chart of accounts, adding new accounts

**Create Example:**
```json
{
  "Name": "Marketing Expenses",
  "AccountType": "Expense",
  "AccountSubType": "AdvertisingPromotional"
}
```

**Account Types:**
- Bank
- Accounts Receivable
- Other Current Asset
- Fixed Asset
- Other Asset
- Accounts Payable
- Credit Card
- Other Current Liability
- Long Term Liability
- Equity
- Income
- Cost of Goods Sold
- Expense
- Other Income
- Other Expense

---

### Journal Entry Operations

#### GetJournalEntry

Retrieve a journal entry by ID.

#### CreateOrUpdateJournalEntry

Create a journal entry.

**When to use:** Adjusting entries, complex transactions, corrections

**Create Example:**
```json
{
  "TxnDate": "2024-01-31",
  "DocNumber": "ADJ-001",
  "Line": [
    {
      "DetailType": "JournalEntryLineDetail",
      "JournalEntryLineDetail": {
        "PostingType": "Debit",
        "AccountRef": {
          "value": "7"
        }
      },
      "Amount": 100.00,
      "Description": "Adjusting entry - debit"
    },
    {
      "DetailType": "JournalEntryLineDetail",
      "JournalEntryLineDetail": {
        "PostingType": "Credit",
        "AccountRef": {
          "value": "1"
        }
      },
      "Amount": 100.00,
      "Description": "Adjusting entry - credit"
    }
  ]
}
```

**Note:** Debits and credits must balance.

---

### Deposit Operations

#### GetDeposit

Retrieve a deposit by ID.

#### CreateOrUpdateDeposit

Create a bank deposit.

**When to use:** Grouping payments into a single deposit

**Create Example:**
```json
{
  "DepositToAccountRef": {
    "value": "35"
  },
  "TxnDate": "2024-01-20",
  "Line": [
    {
      "Amount": 500.00,
      "DetailType": "DepositLineDetail",
      "DepositLineDetail": {
        "AccountRef": {
          "value": "4"
        }
      },
      "Description": "Customer payment deposit"
    }
  ]
}
```

---

### Transfer Operations

#### GetTransfer

Retrieve a transfer by ID.

#### CreateOrUpdateTransfer

Create a transfer between accounts.

**When to use:** Moving money between bank accounts

**Create Example:**
```json
{
  "FromAccountRef": {
    "value": "35"
  },
  "ToAccountRef": {
    "value": "36"
  },
  "Amount": 1000.00,
  "TxnDate": "2024-01-15"
}
```

---

### Employee Operations

#### GetEmployee

Retrieve an employee by ID.

#### CreateOrUpdateEmployee

Create or update an employee record.

**When to use:** Setting up employees for time tracking

**Create Example:**
```json
{
  "DisplayName": "Jane Smith",
  "GivenName": "Jane",
  "FamilyName": "Smith",
  "PrimaryEmailAddr": {
    "Address": "jane@company.com"
  },
  "PrimaryPhone": {
    "FreeFormNumber": "555-234-5678"
  }
}
```

---

### Company Operations

#### GetCompanyInfo

Retrieve company information.

**When to use:** Getting company details, verifying connection

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|
| ID | Yes | Company ID (same as your realmId from connection) |

**Example Response:**
```json
{
  "CompanyInfo": {
    "CompanyName": "My Company",
    "LegalName": "My Company LLC",
    "CompanyAddr": {
      "Line1": "123 Main St",
      "City": "San Francisco",
      "CountrySubDivisionCode": "CA",
      "PostalCode": "94105"
    },
    "FiscalYearStartMonth": "January"
  }
}
```

#### GetPreferences

Retrieve company preferences and settings.

**When to use:** Checking company settings, feature availability

---

### Report Operations

All reports support common parameters:
- **start_date**: Report period start (YYYY-MM-DD)
- **end_date**: Report period end (YYYY-MM-DD)
- **accounting_method**: "Cash" or "Accrual"

#### GetProfitAndLoss

Generate Profit & Loss (Income Statement) report.

**When to use:** Reviewing revenue and expenses, financial analysis

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| Start Date | No | Period start |
| End Date | No | Period end |
| Accounting Method | No | Cash or Accrual |
| Summarize By | No | Total, Month, Week, Quarter, Year |

#### GetBalanceSheet

Generate Balance Sheet report.

**When to use:** Reviewing assets, liabilities, equity

#### GetCashFlow

Generate Statement of Cash Flows.

**When to use:** Analyzing cash movement

#### GetAgedReceivables

Generate Aged Receivables (AR Aging) report.

**When to use:** Reviewing overdue customer invoices

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| Report Date | No | As-of date |
| Aging Method | No | Current or Report_Date |
| Days Per Period | No | Days per aging bucket |

#### GetAgedPayables

Generate Aged Payables (AP Aging) report.

**When to use:** Reviewing bills owed to vendors

#### GetCustomerBalance

Get balances for all customers.

#### GetVendorBalance

Get balances owed to all vendors.

#### GetGeneralLedger

Generate General Ledger report.

**When to use:** Detailed transaction review by account

#### GetTrialBalance

Generate Trial Balance report.

**When to use:** Verifying debits equal credits

#### GetTransactionList

List all transactions for a period.

**When to use:** Comprehensive transaction review

---

### Advanced Operations

#### Batch

Execute up to 30 operations in a single request.

**When to use:** Bulk operations, reducing API calls

**Example:**
```json
{
  "BatchItemRequest": [
    {
      "bId": "1",
      "operation": "create",
      "Customer": {
        "DisplayName": "Batch Customer 1"
      }
    },
    {
      "bId": "2",
      "operation": "create",
      "Customer": {
        "DisplayName": "Batch Customer 2"
      }
    }
  ]
}
```

#### GetChanges (CDC)

Get entities that changed since a specific date.

**When to use:** Sync operations, incremental updates

**Parameters:**
| Parameter | Required | Description |
|-----------|----------|-------------|

| Entities | Yes | Comma-separated list (e.g., "Customer,Invoice") |
| Changed Since | Yes | ISO 8601 datetime |

**Example:**
- Entities: `Customer,Invoice,Payment`
- Changed Since: `2024-01-01T00:00:00-08:00`

**Limitations:** Maximum 30-day lookback

---

## Common Patterns

### Pattern 1: Create Invoice When Order Received

```
Trigger: When new order in [other system]
   ↓
Action: Query (find customer by email)
   ↓
Condition: Customer found?
   ├─ Yes → Use existing customer ID
   └─ No → CreateOrUpdateCustomer
   ↓
Action: CreateOrUpdateInvoice
   ↓
Action: SendInvoice
```

### Pattern 2: Sync Customers from CRM

```
Trigger: Recurrence (daily)
   ↓
Action: GetChanges
   - Entities: Customer
   - Changed Since: [yesterday]
   ↓
Action: For each changed customer
   ↓
Action: Update CRM record
```

### Pattern 3: Payment Reconciliation

```
Trigger: When payment in [payment system]
   ↓
Action: Query (find open invoices for customer)
   ↓
Action: CreateOrUpdatePayment (link to invoice)
   ↓
Condition: Invoice fully paid?
   └─ Yes → Send thank you email
```

### Pattern 4: Weekly Financial Summary

```
Trigger: Recurrence (weekly)
   ↓
Action: GetProfitAndLoss (this week)
   ↓
Action: GetAgedReceivables
   ↓
Action: GetAgedPayables
   ↓
Action: Send email with summary
```

---

## Error Handling

### Common Errors

| Code | Message | Cause | Solution |
|------|---------|-------|----------|
| 3100 | Authorization Failed | Invalid/expired token, wrong company | Re-authorize connection |
| 3200 | Missing Required Field | Required field not provided | Check required fields in schema |
| 5010 | Stale Object | SyncToken mismatch | Fetch latest record, retry |
| 6000 | Duplicate Name | Display name already exists | Use unique name |
| 6240 | Validation Error | Invalid data | Check field values and formats |

### Best Practices for Error Handling

1. **Use Try-Catch scopes** around QuickBooks actions
2. **Retry on 5010 errors** - fetch fresh SyncToken and retry
3. **Log errors** to a SharePoint list or database
4. **Send notifications** for critical failures

---

## Best Practices

### Performance

1. **Use Query with MAXRESULTS** - Don't fetch more data than needed
2. **Use Batch operations** - Group multiple creates/updates
3. **Use CDC for sync** - Don't query all records repeatedly
4. **Cache reference data** - Items, Accounts change infrequently

### Data Integrity

1. **Always use sparse: true** for updates
2. **Fetch before update** - Get current SyncToken
3. **Validate before sending** - Check required fields
4. **Use meaningful DocNumbers** - For easy identification

### Security

1. **Use production credentials for production** - Not sandbox
2. **Secure Client Secret** - Don't expose in flows
3. **Limit permissions** - Use minimal required scopes
4. **Monitor API usage** - Check for unusual activity

### API Limits

| Limit | Value |
|-------|-------|
| Requests per minute | 500 |
| Batch operations | 30 per request |
| Query results | 1000 max |
| CDC lookback | 30 days |

---

## Troubleshooting

### Connection Issues

#### "ApplicationAuthorizationFailed" (Error 3100)

**Symptoms:** API returns error 3100, authorization failed

**Causes & Solutions:**

| Cause | Solution |
|-------|----------|
| Wrong Company ID | Verify realmId matches the authorized company |
| Token expired | Delete connection, create new one |
| Wrong environment | Sandbox credentials with production host (or vice versa) |
| App not authorized for company | Re-authorize and select correct company |

**How to verify:**
1. Delete the connection in Power Automate
2. Create a new connection
3. When authorizing, note which company you select
4. Use that company's realmId

#### "Invalid Grant" on Token Refresh

**Symptoms:** Connection stops working after some time

**Causes:**
- Refresh token expired (100-day lifetime)
- Refresh token was revoked
- User changed QuickBooks password

**Solution:**
1. Delete the connection
2. Create a new connection
3. Re-authorize with QuickBooks

**Prevention:** Use connections regularly (at least once every 100 days)

#### Connection Works in Test but Fails in Flow

**Symptoms:** Test tab works, but flow fails

**Causes & Solutions:**

| Cause | Solution |
|-------|----------|
| Different connection used | Check which connection the flow is using |
| Connection owner left org | Create new connection with active user |
| Shared connection issues | Owner must re-consent for sharing |

---

### Data Errors

#### "Stale Object Exception" (Error 5010)

**Symptoms:** Update fails with stale object error

**Cause:** The SyncToken you sent doesn't match the current version

**Solution:**
```
1. GetCustomer (or relevant Get action) → get current SyncToken
2. Use the fresh SyncToken in your update
3. Retry the update
```

**Flow pattern for auto-retry:**
```
Action: Get[Entity]
   ↓
Action: CreateOrUpdate[Entity] (use SyncToken from Get)
   ↓
Configure retry policy: 
   - Count: 3
   - Interval: PT5S
```

#### "Duplicate Name Existing Error" (Error 6000)

**Symptoms:** Create fails saying name already exists

**Cause:** DisplayName must be unique in QuickBooks

**Solutions:**
1. Query first to check if entity exists
2. Use a unique identifier in the name (e.g., "John Smith - 12345")
3. Update existing record instead of creating new

**Flow pattern:**
```
Action: Query
   - Query: SELECT * FROM Customer WHERE DisplayName = 'Name'
   ↓
Condition: QueryResponse.Customer is not empty?
   ├─ Yes → Update existing (use returned Id)
   └─ No → Create new
```

#### "Required Field Missing"

**Symptoms:** Create fails with validation error

**Common missing fields by entity:**

| Entity | Often Missing |
|--------|---------------|
| Invoice | CustomerRef, Line with DetailType |
| Bill | VendorRef, Line |
| Payment | CustomerRef, TotalAmt |
| Item | IncomeAccountRef (for Service/NonInventory) |
| Inventory Item | IncomeAccountRef, ExpenseAccountRef, AssetAccountRef, InvStartDate, QtyOnHand |
| JournalEntry | Balanced debit/credit lines |

#### "Invalid Reference" Errors

**Symptoms:** Error says CustomerRef, ItemRef, or AccountRef is invalid

**Causes & Solutions:**

| Cause | Solution |
|-------|----------|
| ID doesn't exist | Query to find correct ID |
| Entity is inactive | Reactivate in QuickBooks or use different entity |
| Wrong entity type | Verify you're referencing correct type |
| ID from wrong company | Verify realmId matches |

**Debugging tip:** 
```sql
SELECT Id, DisplayName, Active FROM Customer WHERE Id = '58'
```

---

### Query Issues

#### Query Returns Empty Results

**Symptoms:** QueryResponse is empty but data exists

**Causes & Solutions:**

| Cause | Solution |
|-------|----------|
| Wrong field name | Check exact field names (case-sensitive) |
| Quotes missing | String values need single quotes: `WHERE Name = 'Test'` |
| Active filter | Add `WHERE Active = true` or check inactive records |
| Wrong date format | Use YYYY-MM-DD: `WHERE TxnDate > '2024-01-01'` |

**Common field name mistakes:**
- `CustomerName` → Should be `DisplayName`
- `Amount` → Should be `TotalAmt`
- `Date` → Should be `TxnDate`

#### "Invalid Query" Error

**Symptoms:** Query fails with syntax error

**Common issues:**

| Wrong | Correct |
|-------|---------|
| `WHERE Name = "Test"` | `WHERE Name = 'Test'` (single quotes) |
| `WHERE Amount > 100` | `WHERE TotalAmt > '100'` (quotes around numbers) |
| `WHERE A = 1 OR B = 2` | Not supported (use 2 queries) |
| `WHERE Name LIKE "%test"` | `WHERE Name LIKE '%test%'` (single quotes) |
| `SELECT COUNT(*)` | Not supported (count in flow) |

#### Query is Slow or Times Out

**Symptoms:** Query takes too long, may timeout

**Solutions:**
1. Add `MAXRESULTS 100` to limit data
2. Add date filter to narrow results
3. Query specific fields instead of `SELECT *`
4. Use CDC instead for large sync operations

**Optimized query:**
```sql
SELECT Id, DisplayName, Balance 
FROM Invoice 
WHERE TxnDate > '2024-01-01' AND Balance > '0' 
MAXRESULTS 200
```

---

### Invoice & Transaction Issues

#### Invoice Created but Line Items Missing

**Symptoms:** Invoice created but has no line items

**Cause:** Incorrect Line structure

**Correct structure:**
```json
{
  "Line": [
    {
      "Amount": 100.00,
      "DetailType": "SalesItemLineDetail",
      "SalesItemLineDetail": {
        "ItemRef": {"value": "1"},
        "Qty": 1,
        "UnitPrice": 100.00
      }
    }
  ]
}
```

**Common mistakes:**
- Missing `DetailType` field
- `DetailType` doesn't match the detail object name
- `SalesItemLineDetail` misspelled or wrong case

#### Invoice Total is Wrong

**Symptoms:** TotalAmt doesn't match expected

**Causes:**
- Tax calculations applied automatically
- Discount lines affecting total
- Amount vs Qty × UnitPrice mismatch

**Solution:** Let QuickBooks calculate:
```json
{
  "Line": [
    {
      "DetailType": "SalesItemLineDetail",
      "SalesItemLineDetail": {
        "ItemRef": {"value": "1"},
        "Qty": 2,
        "UnitPrice": 50.00
      }
    }
  ]
}
```
Don't include `Amount` at line level - let it calculate.

#### SendInvoice Fails

**Symptoms:** Invoice created but email fails

**Causes & Solutions:**

| Cause | Solution |
|-------|----------|
| No email on customer | Add BillEmail to invoice or customer |
| Invalid email format | Verify email address format |
| Email not configured in QBO | Check QuickBooks email settings |

**Include email in invoice:**
```json
{
  "CustomerRef": {"value": "58"},
  "BillEmail": {"Address": "customer@example.com"},
  "Line": [...]
}
```

---

### Payment Issues

#### Payment Not Applying to Invoice

**Symptoms:** Payment created but invoice still shows balance

**Cause:** Payment not linked to invoice

**Correct structure:**
```json
{
  "CustomerRef": {"value": "58"},
  "TotalAmt": 150.00,
  "Line": [
    {
      "Amount": 150.00,
      "LinkedTxn": [
        {
          "TxnId": "101",
          "TxnType": "Invoice"
        }
      ]
    }
  ]
}
```

**Common mistakes:**
- Missing `LinkedTxn` array
- Wrong `TxnType` (must be "Invoice")
- `TxnId` as number instead of string

#### Overpayment or Underpayment

**Symptoms:** Payment amount doesn't match invoice

**Solutions:**

*For partial payment:*
```json
{
  "TotalAmt": 50.00,
  "Line": [
    {
      "Amount": 50.00,
      "LinkedTxn": [{"TxnId": "101", "TxnType": "Invoice"}]
    }
  ]
}
```

*For overpayment (creates credit):*
- QuickBooks automatically creates unapplied credit

*For payment spanning multiple invoices:*
```json
{
  "TotalAmt": 300.00,
  "Line": [
    {
      "Amount": 150.00,
      "LinkedTxn": [{"TxnId": "101", "TxnType": "Invoice"}]
    },
    {
      "Amount": 150.00,
      "LinkedTxn": [{"TxnId": "102", "TxnType": "Invoice"}]
    }
  ]
}
```

---

### Report Issues

#### Report Returns No Data

**Symptoms:** Report response is empty

**Causes & Solutions:**

| Cause | Solution |
|-------|----------|
| No transactions in period | Expand date range |
| Wrong date format | Use YYYY-MM-DD |
| Future dates | Reports can't show future data |
| Wrong accounting method | Try both Cash and Accrual |

#### Report Data Doesn't Match QuickBooks UI

**Symptoms:** Numbers different from QuickBooks reports

**Causes:**
- Different date range
- Different accounting method (Cash vs Accrual)
- Report run at different time (data changed)
- Different fiscal year settings

**Solution:** Match parameters exactly:
- Same start_date and end_date
- Same accounting_method
- Run at same time as QuickBooks report

---

### Performance Issues

#### Flow is Slow

**Symptoms:** Flow takes too long to complete

**Solutions:**

| Issue | Solution |
|-------|----------|
| Too many API calls | Use Batch operation |
| Querying all records | Add MAXRESULTS, use filters |
| Sequential processing | Use parallel branches where possible |
| Large responses | Query only needed fields |

**Batch example for creating multiple customers:**
```json
{
  "BatchItemRequest": [
    {"bId": "1", "operation": "create", "Customer": {"DisplayName": "Customer 1"}},
    {"bId": "2", "operation": "create", "Customer": {"DisplayName": "Customer 2"}},
    {"bId": "3", "operation": "create", "Customer": {"DisplayName": "Customer 3"}}
  ]
}
```

#### Rate Limiting (429 Errors)

**Symptoms:** Requests fail with too many requests error

**QuickBooks limits:** 500 requests per minute

**Solutions:**
1. Add delays between actions (1-2 seconds)
2. Use Batch operations
3. Use CDC instead of querying all records
4. Implement exponential backoff

**Delay pattern:**
```
Action: CreateOrUpdateCustomer
   ↓
Action: Delay - 2 seconds
   ↓
Action: CreateOrUpdateCustomer
```

---

### Sandbox vs Production Issues

#### Works in Sandbox, Fails in Production

**Checklist:**

| Check | Action |
|-------|--------|
| Host URL | Change from `sandbox-quickbooks.api.intuit.com` to `quickbooks.api.intuit.com` |
| Credentials | Use Production Client ID and Secret |
| Connection | Create new connection with production auth |
| Company ID | Use production company's realmId |
| Test data | IDs from sandbox don't exist in production |

#### Data from Wrong Company

**Symptoms:** Seeing sandbox data in production or vice versa

**Cause:** Using wrong realmId or connection

**Solution:**
1. Verify which connection the flow uses
2. Check the realmId in each action
3. Create separate connections for sandbox and production
4. Name connections clearly: "QBO Sandbox" vs "QBO Production"

---

### Common Error Codes Reference

| Code | Name | Common Cause | Quick Fix |
|------|------|--------------|-----------|
| 3100 | AuthorizationFault | Bad token or wrong company | Re-authorize |
| 3200 | ValidationFault | Missing/invalid data | Check required fields |
| 5010 | StaleObjectException | SyncToken mismatch | Fetch latest, retry |
| 6000 | DuplicateNameError | Name already exists | Use unique name |
| 6140 | InvalidIdError | Reference ID not found | Query to find correct ID |
| 6240 | ValidationFault | Business rule violation | Check QuickBooks rules |
| 500 | InternalServerError | Transient error | Retry after delay |
| 503 | ServiceUnavailable | QuickBooks down | Retry later |

---

### Debug Checklist

When something fails, check in order:

1. **Connection valid?**
   - Test tab → test GetCompanyInfo
   - If fails, recreate connection

2. **Correct Company ID?**
   - Verify realmId matches authorized company

3. **Correct environment?**
   - Sandbox host for sandbox credentials
   - Production host for production credentials

4. **Required fields present?**
   - Check entity documentation for required fields

5. **Valid references?**
   - Query to verify CustomerRef, ItemRef, etc. exist

6. **Correct data types?**
   - IDs as strings, amounts as numbers
   - Dates as YYYY-MM-DD

7. **Fresh SyncToken?**
   - For updates, get latest before updating

8. **Within API limits?**
   - Under 500 requests/minute
   - Under 30 operations per batch

---

## Quick Reference

### Finding IDs

| To find | Query |
|---------|-------|
| Customer ID | `SELECT Id, DisplayName FROM Customer WHERE DisplayName = 'Name'` |
| Vendor ID | `SELECT Id, DisplayName FROM Vendor WHERE DisplayName = 'Name'` |
| Item ID | `SELECT Id, Name FROM Item WHERE Name = 'Product'` |
| Account ID | `SELECT Id, Name FROM Account WHERE Name = 'Account Name'` |
| Invoice ID | `SELECT Id, DocNumber FROM Invoice WHERE DocNumber = '1001'` |

### Minimum Required Fields

| Entity | Required Fields |
|--------|-----------------|
| Customer | DisplayName |
| Vendor | DisplayName |
| Invoice | CustomerRef, Line |
| Bill | VendorRef, Line |
| Payment | CustomerRef, TotalAmt |
| Item | Name |
| Account | Name, AccountType |

### Line Item DetailTypes

| DetailType | Use For |
|------------|---------|
| SalesItemLineDetail | Invoice, Estimate, SalesReceipt lines |
| ItemBasedExpenseLineDetail | Bill, PurchaseOrder lines (items) |
| AccountBasedExpenseLineDetail | Bill, Purchase lines (expenses) |
| JournalEntryLineDetail | Journal Entry lines |
| DepositLineDetail | Deposit lines |

---

*Last updated: January 2026*
