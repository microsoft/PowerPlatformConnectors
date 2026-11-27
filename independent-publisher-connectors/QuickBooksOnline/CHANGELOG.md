# Changelog

All notable changes to the QuickBooks Online connector will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-01-18

### Added
- Initial release of QuickBooks Online independent publisher connector
- 60+ operations covering QuickBooks Online API v3
- **Customer & Vendor Management**
  - Create, read, update operations for customers and vendors
- **Invoice Operations**
  - Create, read, update, send, PDF download, void, and delete invoices
- **Bill Operations**
  - Full bill lifecycle management including payments
- **Payment Operations**
  - Customer payments and bill payments with invoice linking
- **Estimates & Sales Receipts**
  - Quote management and cash sale transactions
- **Credit Memos & Refunds**
  - Customer credits and refund receipts
- **Purchase Operations**
  - Expenses, checks, credit card charges, and purchase orders
- **Product & Service Items**
  - Item management for inventory and services
- **Chart of Accounts**
  - Account creation and management
- **Banking Operations**
  - Journal entries, deposits, and transfers
- **Employee Management**
  - Basic employee record management
- **Financial Reports** (15+ reports)
  - Profit & Loss (with detail variant)
  - Balance Sheet
  - Cash Flow Statement
  - Trial Balance
  - General Ledger
  - Aged Receivables (summary and detail)
  - Aged Payables (summary and detail)
  - Customer Balance (summary and detail)
  - Vendor Balance (summary and detail)
  - Transaction List
  - And more
- **Advanced Operations**
  - Query: SQL-like query capability for all entities
  - CDC (Change Data Capture): Incremental sync support
  - Batch: Execute up to 30 operations in one request
- **Company Information**
  - Retrieve company info and preferences
- **Comprehensive Documentation**
  - Detailed README with setup instructions
  - 1,981-line user guide with examples and troubleshooting
  - Common patterns and workflows
  - Error handling guide

### Technical Details
- OAuth 2.0 authentication with automatic token refresh
- Support for both sandbox and production environments
- Proper handling of QuickBooks SyncToken for optimistic concurrency
- Sparse update support for partial record updates
- RealmId (Company ID) parameter for multi-company support

### Documentation
- Setup guide with Intuit Developer Portal instructions
- OAuth configuration steps
- RealmId documentation with multiple discovery methods
- Operation reference with examples
- Troubleshooting guide with common errors
- Best practices for API usage

[1.0.0]: https://github.com/microsoft/PowerPlatformConnectors/tree/dev/independent-publisher-connectors/QuickBooksOnline
