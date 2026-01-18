# QuickBooks Online (Independent Publisher)

> **Important:** This is an independent publisher connector created by Forceworks. It is not affiliated with, endorsed by, or supported by Intuit Inc. For official QuickBooks integrations, please visit the [Intuit App Store](https://quickbooks.intuit.com/app/apps/home/).

A comprehensive Power Platform connector for QuickBooks Online, providing full access to accounting data including customers, vendors, invoices, bills, payments, items, and financial reports.

## Publisher: Forceworks

**Website:** https://www.forceworks.com
**Support Contact:** connect@forceworks.com

## Prerequisites

1. A QuickBooks Online account (any tier: Simple Start, Essentials, Plus, or Advanced)
2. An Intuit Developer account at [developer.intuit.com](https://developer.intuit.com)
3. An app created in the Intuit Developer Portal

## Supported Operations

### Customers & Vendors
| Operation | Description |
|-----------|-------------|
| **CreateCustomer** | Create a new customer |
| **GetCustomer** | Retrieve a customer by ID |
| **UpdateCustomer** | Update an existing customer |
| **CreateVendor** | Create a new vendor |
| **GetVendor** | Retrieve a vendor by ID |
| **UpdateVendor** | Update an existing vendor |

### Invoices
| Operation | Description |
|-----------|-------------|
| **CreateInvoice** | Create a new invoice |
| **GetInvoice** | Retrieve an invoice by ID |
| **UpdateInvoice** | Update an existing invoice |
| **SendInvoice** | Email an invoice to the customer |
| **GetInvoicePDF** | Download invoice as PDF |
| **VoidInvoice** | Void an invoice |
| **DeleteInvoice** | Delete an invoice |

### Bills & Payments
| Operation | Description |
|-----------|-------------|
| **CreateBill** | Create a new bill (AP) |
| **GetBill** | Retrieve a bill by ID |
| **UpdateBill** | Update an existing bill |
| **DeleteBill** | Delete a bill |
| **CreatePayment** | Record a customer payment |
| **GetPayment** | Retrieve a payment by ID |
| **UpdatePayment** | Update a payment |
| **VoidPayment** | Void a payment |
| **DeletePayment** | Delete a payment |
| **CreateBillPayment** | Record a payment to a vendor |
| **GetBillPayment** | Retrieve a bill payment by ID |
| **UpdateBillPayment** | Update a bill payment |
| **DeleteBillPayment** | Delete a bill payment |

### Estimates & Sales Receipts
| Operation | Description |
|-----------|-------------|
| **CreateEstimate** | Create a new estimate/quote |
| **GetEstimate** | Retrieve an estimate by ID |
| **UpdateEstimate** | Update an estimate |
| **SendEstimate** | Email an estimate |
| **GetEstimatePDF** | Download estimate as PDF |
| **DeleteEstimate** | Delete an estimate |
| **CreateSalesReceipt** | Create a sales receipt |
| **GetSalesReceipt** | Retrieve a sales receipt by ID |
| **UpdateSalesReceipt** | Update a sales receipt |
| **SendSalesReceipt** | Email a sales receipt |
| **GetSalesReceiptPDF** | Download sales receipt as PDF |
| **VoidSalesReceipt** | Void a sales receipt |
| **DeleteSalesReceipt** | Delete a sales receipt |

### Credit Memos & Refunds
| Operation | Description |
|-----------|-------------|
| **CreateCreditMemo** | Create a customer credit memo |
| **GetCreditMemo** | Retrieve a credit memo by ID |
| **UpdateCreditMemo** | Update a credit memo |
| **VoidCreditMemo** | Void a credit memo |
| **DeleteCreditMemo** | Delete a credit memo |
| **CreateRefundReceipt** | Create a refund receipt |
| **GetRefundReceipt** | Retrieve a refund receipt by ID |
| **UpdateRefundReceipt** | Update a refund receipt |
| **DeleteRefundReceipt** | Delete a refund receipt |
| **CreateVendorCredit** | Create a vendor credit |
| **GetVendorCredit** | Retrieve a vendor credit by ID |
| **UpdateVendorCredit** | Update a vendor credit |
| **DeleteVendorCredit** | Delete a vendor credit |

### Purchases & Purchase Orders
| Operation | Description |
|-----------|-------------|
| **CreatePurchase** | Create an expense/check/credit card charge |
| **GetPurchase** | Retrieve a purchase by ID |
| **UpdatePurchase** | Update a purchase |
| **VoidPurchase** | Void a purchase |
| **DeletePurchase** | Delete a purchase |
| **CreatePurchaseOrder** | Create a purchase order |
| **GetPurchaseOrder** | Retrieve a purchase order by ID |
| **UpdatePurchaseOrder** | Update a purchase order |
| **SendPurchaseOrder** | Email a purchase order |
| **DeletePurchaseOrder** | Delete a purchase order |

### Products & Services
| Operation | Description |
|-----------|-------------|
| **CreateItem** | Create a product or service item |
| **GetItem** | Retrieve an item by ID |
| **UpdateItem** | Update an item |

### Chart of Accounts & Banking
| Operation | Description |
|-----------|-------------|
| **CreateAccount** | Create a new account |
| **GetAccount** | Retrieve an account by ID |
| **UpdateAccount** | Update an account |
| **CreateJournalEntry** | Create a journal entry |
| **GetJournalEntry** | Retrieve a journal entry by ID |
| **UpdateJournalEntry** | Update a journal entry |
| **DeleteJournalEntry** | Delete a journal entry |
| **CreateDeposit** | Create a bank deposit |
| **GetDeposit** | Retrieve a deposit by ID |
| **UpdateDeposit** | Update a deposit |
| **DeleteDeposit** | Delete a deposit |
| **CreateTransfer** | Create an account transfer |
| **GetTransfer** | Retrieve a transfer by ID |
| **UpdateTransfer** | Update a transfer |
| **DeleteTransfer** | Delete a transfer |

### Reference Data
| Operation | Description |
|-----------|-------------|
| **CreateEmployee** | Create an employee |
| **GetEmployee** | Retrieve an employee by ID |
| **UpdateEmployee** | Update an employee |
| **CreateClass** | Create a class |
| **GetClass** | Retrieve a class by ID |
| **UpdateClass** | Update a class |
| **CreateDepartment** | Create a department/location |
| **GetDepartment** | Retrieve a department by ID |
| **UpdateDepartment** | Update a department |
| **CreateTerm** | Create a payment term |
| **GetTerm** | Retrieve a term by ID |
| **UpdateTerm** | Update a term |
| **CreatePaymentMethod** | Create a payment method |
| **GetPaymentMethod** | Retrieve a payment method by ID |
| **UpdatePaymentMethod** | Update a payment method |
| **GetTaxCode** | Retrieve a tax code by ID |
| **GetTaxRate** | Retrieve a tax rate by ID |

### Attachments
| Operation | Description |
|-----------|-------------|
| **CreateAttachable** | Create attachment metadata |
| **GetAttachable** | Retrieve an attachable by ID |
| **UpdateAttachable** | Update an attachable |
| **DeleteAttachable** | Delete an attachable |

### Company & Settings
| Operation | Description |
|-----------|-------------|
| **GetCompanyInfo** | Get company information |
| **UpdateCompanyInfo** | Update company information |
| **GetPreferences** | Get company preferences |
| **UpdatePreferences** | Update preferences |
| **GetExchangeRate** | Get currency exchange rate |
| **GetBudget** | Get budget by ID |

### Financial Reports
| Operation | Description |
|-----------|-------------|
| **GetProfitAndLossReport** | Profit and Loss statement |
| **GetProfitAndLossDetailReport** | Detailed P&L |
| **GetBalanceSheetReport** | Balance Sheet |
| **GetCashFlowReport** | Cash Flow statement |
| **GetTrialBalanceReport** | Trial Balance |
| **GetGeneralLedgerReport** | General Ledger |
| **GetAgedReceivablesReport** | AR Aging Summary |
| **GetAgedReceivablesDetailReport** | AR Aging Detail |
| **GetAgedPayablesReport** | AP Aging Summary |
| **GetAgedPayablesDetailReport** | AP Aging Detail |
| **GetCustomerBalanceReport** | Customer Balance Summary |
| **GetCustomerBalanceDetailReport** | Customer Balance Detail |
| **GetVendorBalanceReport** | Vendor Balance Summary |
| **GetVendorBalanceDetailReport** | Vendor Balance Detail |
| **GetSalesByCustomerReport** | Sales by Customer |
| **GetSalesByProductReport** | Sales by Product/Service |
| **GetInventoryValuationReport** | Inventory Valuation |
| **GetTransactionListReport** | Transaction List |
| **GetAccountListReport** | Account List |
| **GetCustomerIncomeListReport** | Customer Income |
| **GetVendorExpensesReport** | Expenses by Vendor |

### Query & Sync Operations
| Operation | Description |
|-----------|-------------|
| **Query** | Execute SQL-like queries against any entity |
| **GetChangedEntities** | Change Data Capture (CDC) for sync |
| **BatchOperation** | Execute up to 30 operations in one request |

## Obtaining Credentials

### Step 1: Create an Intuit Developer Account
1. Go to [developer.intuit.com](https://developer.intuit.com)
2. Sign up for a free developer account

### Step 2: Create an App
1. Navigate to the Dashboard and click "Create an app"
2. Select "QuickBooks Online and Payments"
3. Enter your app name and select a scope

### Step 3: Configure OAuth Settings
1. Go to your app's Keys & OAuth section
2. Add the redirect URI: `https://global.consent.azure-apim.net/redirect`
3. Copy your **Client ID** and **Client Secret**

### Step 4: Get Your Company ID (RealmId)

After connecting via OAuth, you'll receive a `realmId` parameter. This is your Company ID and is required for all API calls.

**How to find your realmId:**
1. **During OAuth:** After authorizing, check the redirect URL for `realmId=` parameter
   - Example: `https://...?realmId=9341456161184198&...`
2. **In QuickBooks:** Go to Settings → Account and Settings, then check your browser's URL bar
   - The number after `/app/` is your realmId
3. **Store this value:** You'll need it for every API operation

**Example realmId:** `9341456161184198`

## Important Notes

### Refresh Tokens
- Access tokens expire after **1 hour**
- Refresh tokens expire after **100 days of inactivity**
- **Critical:** The refresh token changes with each refresh call - always store the latest token
- Power Automate handles token refresh automatically

### API Versioning
The connector uses `minorversion=70` by default. You can override this in the advanced parameters.

### Rate Limits
- Maximum 500 requests per minute per realm
- Batch operations count as 1 request

### API Pricing (2025)
Intuit now charges for some API calls under the App Partner Program:
- **Core API** (creates/updates): Free and unlimited
- **CorePlus API** (queries/reads): Metered based on usage tier

Use batch operations and CDC to optimize API usage.

### Query Syntax
The Query operation uses QuickBooks' SQL-like syntax:
```sql
SELECT * FROM Customer WHERE Active = true MAXRESULTS 100
SELECT * FROM Invoice WHERE TotalAmt > '1000' AND TxnDate > '2024-01-01'
SELECT * FROM Item WHERE Type = 'Service'
```

**Limitations:**
- No OR operator in WHERE clauses
- No GROUP BY or JOIN
- Use `%` for wildcards with LIKE
- Maximum 1000 results per query

### Change Data Capture (CDC)
Use CDC for efficient data synchronization:
- Specify entities: `Customer,Invoice,Payment`
- Lookback up to 30 days
- Returns all changed/deleted records since specified datetime

## Known Issues and Limitations

1. **PDF downloads** may require additional handling in Power Automate for binary content
2. **Attachable uploads** require multipart form data (use separate upload endpoint)
3. **Reports** have a maximum of 400,000 cells - reduce date range if exceeded
4. **Sandbox vs Production**: Uses same auth endpoint but different data hosts

## Deployment Instructions

### For Custom Connector in Power Automate:
1. Download the `apiDefinition.swagger.json` and `apiProperties.json`
2. Go to Power Automate > Data > Custom Connectors
3. Click "New custom connector" > "Import an OpenAPI file"
4. Upload the swagger file
5. Configure OAuth with your Client ID and Secret
6. Create and test the connector

### For Independent Publisher Certification:
1. Fork the [PowerPlatformConnectors](https://github.com/microsoft/PowerPlatformConnectors) repo
2. Create folder: `independent-publisher-connectors/QuickBooksOnline/`
3. Add files: `apiDefinition.swagger.json`, `apiProperties.json`, `readme.md`
4. Test all operations thoroughly
5. Submit a pull request with test screenshots

## Support

For connector issues: [Your support email/URL]
For QuickBooks API documentation: [developer.intuit.com](https://developer.intuit.com/app/developer/qbo/docs/get-started)
