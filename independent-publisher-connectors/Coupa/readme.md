# Coupa (Independent Publisher)

| | |
|---|---|
| **Publisher** | NovaGL |
| **Website** | [coupa.com](https://www.coupa.com) |
| **Privacy Policy** | [coupa.com/privacy-policy](https://www.coupa.com/privacy-policy) |
| **Categories** | Commerce, Finance & Accounting |
| **Version** | 1.1.9 |

## Overview

This connector provides read and action access to the **Coupa REST API**
for Power Automate flows and Power Apps. It covers the full P2P lifecycle — requisitions,
purchase orders, invoices, receipts, contracts, suppliers, matching allocations and more.

A **single connector works across all Coupa tenants** (test, UAT, production) via dynamic host —
you enter the instance name once at connection time.

## Prerequisites

1. A Coupa instance on `coupahost.com`
2. An OAuth2 app registered in Coupa: **Setup → OAuth2/OpenID Connect Clients**
   - Grant type: `Authorization Code`
   - Redirect URI: `https://global.consent.azure-apim.net/redirect`

## Credentials

| Field | Where to get it |
|---|---|
| **Instance Name** | URL prefix — e.g. `mycompany-test` from `mycompany-test.coupahost.com` |
| **Client ID** | Coupa → Setup → OAuth2/OpenID Connect Clients |
| **Client Secret** | Generated when you create the OAuth2 app |
| **Scopes** | Space-separated — see recommended set below |

**Required scopes:**
```
offline_access
```

**Recommended scopes:**
```
core.purchase_order.read core.invoice.read core.requisition.read
core.contract.read core.supplier.read core.user.read core.common.read
```
Add `core.purchase_order.write` only if using issue/close/cancel/reopen actions.
Add `core.order_pad.read` and `core.order_pad.write` for Order Lists endpoints.
Add `core.supplier.write` for supplier update operations.

## Supported Operations (67 total)

| Operation ID | Method | Path | Description |
|---|---|---|---|
| `GetAccounts` | GET | `/accounts` |  |
| `GetAccountById` | GET | `/accounts/{id}` |  |
| `GetAddresses` | GET | `/addresses` |  |
| `GetAddressById` | GET | `/addresses/{id}` |  |
| `GetApprovalChains` | GET | `/approval_chains` |  |
| `GetApprovalChainById` | GET | `/approval_chains/{id}` |  |
| `GetApprovals` | GET | `/approvals` |  |
| `GetApprovalById` | GET | `/approvals/{id}` |  |
| `GetBudgetLines` | GET | `/budget_lines` |  |
| `GetBudgetLineById` | GET | `/budget_lines/{id}` |  |
| `GetCommodities` | GET | `/commodities` |  |
| `GetCommodityById` | GET | `/commodities/{id}` |  |
| `GetContracts` | GET | `/contracts` |  |
| `GetContractById` | GET | `/contracts/{id}` |  |
| `GetExchangeRates` | GET | `/exchange_rates` | Query FX exchange rates for multi-currency normalisation. |
| `GetExchangeRateById` | GET | `/exchange_rates/{id}` | Get a single exchange rate. |
| `GetInvoices` | GET | `/invoices` | Query Invoice headers. Use return_object=shallow to embed invoice-lines in … |
| `GetInvoiceById` | GET | `/invoices/{id}` | Get a single Invoice by ID. Use return_object=shallow to include embedded i… |
| `GetInvoiceAttachments` | GET | `/invoices/{id}/attachments` | Get attachments for an Invoice. |
| `GetInvoiceComments` | GET | `/invoices/{id}/comments` | Get comments for an Invoice. |
| `GetItems` | GET | `/items` |  |
| `GetItemById` | GET | `/items/{id}` |  |
| `GetLookupValues` | GET | `/lookup_values` |  |
| `GetLookupValueById` | GET | `/lookup_values/{id}` |  |
| `GetLookups` | GET | `/lookups` |  |
| `GetLookupById` | GET | `/lookups/{id}` |  |
| `GetMatchingAllocations` | GET | `/matching_allocations` | Query matching allocations — links PO lines, invoice lines and receipts (3-… |
| `GetMatchingAllocationById` | GET | `/matching_allocations/{id}` | Get a single matching allocation. |
| `GetPaymentTerms` | GET | `/payment_terms` | Query payment terms reference data. |
| `GetPaymentTermById` | GET | `/payment_terms/{id}` | Get a single payment term. |
| `GetPurchaseOrderChanges` | GET | `/purchase_order_changes` |  |
| `GetPurchaseOrderChangeById` | GET | `/purchase_order_changes/{id}` |  |
| `GetPurchaseOrderLines` | GET | `/purchase_order_lines` |  |
| `GetPurchaseOrderLineById` | GET | `/purchase_order_lines/{id}` |  |
| `GetPurchaseOrders` | GET | `/purchase_orders` |  |
| `GetPurchaseOrderById` | GET | `/purchase_orders/{id}` |  |
| `GetPurchaseOrderAttachments` | GET | `/purchase_orders/{id}/attachments` | Get attachments for a Purchase Order. |
| `CancelPurchaseOrder` | PUT | `/purchase_orders/{id}/cancel` |  |
| `ClosePurchaseOrder` | PUT | `/purchase_orders/{id}/close` |  |
| `GetPurchaseOrderComments` | GET | `/purchase_orders/{id}/comments` | Get comments for a Purchase Order. |
| `IssuePurchaseOrder` | PUT | `/purchase_orders/{id}/issue` |  |
| `ReopenPurchaseOrder` | PUT | `/purchase_orders/{id}/reopen` |  |
| `GetReceipts` | GET | `/receipts` | Query receipt headers (tenant config dependent). |
| `GetReceiptById` | GET | `/receipts/{id}` | Get a single receipt. |
| `GetReceivingTransactions` | GET | `/receiving_transactions` |  |
| `GetReceivingTransactionById` | GET | `/receiving_transactions/{id}` |  |
| `GetRequisitionLines` | GET | `/requisition_lines` | Query requisition lines — confirmed standalone Coupa endpoint. |
| `GetRequisitionLineById` | GET | `/requisition_lines/{id}` | Get a single requisition line. |
| `GetRequisitions` | GET | `/requisitions` |  |
| `GetRequisitionById` | GET | `/requisitions/{id}` |  |
| `GetSupplierItems` | GET | `/supplier_items` | Query supplier items with contracted pricing and part numbers. |
| `GetSupplierItemById` | GET | `/supplier_items/{id}` | Get a single supplier item. |
| `GetSupplierInformation` | GET | `/supplier_information` | Query Supplier Information (SIM) records. |
| `GetSupplierInformationById` | GET | `/supplier_information/{id}` | Get a single Supplier Information (SIM) record by ID. |
| `GetSuppliers` | GET | `/suppliers` |  |
| `GetSupplierById` | GET | `/suppliers/{id}` |  |
| `UpdateSupplier` | PUT | `/suppliers/{id}` | Update supplier profile fields such as payment term. |
| `GetUOMs` | GET | `/uoms` |  |
| `GetUOMById` | GET | `/uoms/{id}` |  |
| `GetUserGroupMemberships` | GET | `/user_group_memberships` |  |
| `GetUserGroupMembershipById` | GET | `/user_group_memberships/{id}` |  |
| `GetUserGroups` | GET | `/user_groups` |  |
| `GetUserGroupById` | GET | `/user_groups/{id}` |  |
| `GetUsers` | GET | `/users` | Query Coupa users. |
| `GetUserById` | GET | `/users/{id}` | Get a single user by ID. |


> **Invoice Lines:** `/invoice_lines` does not exist as a standalone Coupa endpoint.
> Use `GetInvoices` or `GetInvoiceById` with `return_object=shallow` to get embedded invoice line data.

## Dynamic Host

All API calls route to `https://{instance}.coupahost.com/api` at runtime via the
`dynamichosturl` policy. One connector, any tenant.

## Known Issues and Limitations

- **Pagination**: `limit` is capped at 50 records per request; use `offset` to retrieve additional pages

- **Receipts**: Availability depends on tenant config — confirm with your Coupa admin
- **Matching Allocations**: Only populated if 3-way match is enabled on the tenant
- **PO Actions** (issue/close/cancel/reopen): Require `core.purchase_order.write` scope
- **Order Lists** (create/update and line create/update): Require `core.order_pad.write` scope
- **Suppliers Update**: Requires `core.supplier.write` scope
- **Supplier Information (SIM)**: Requires SIM API permissions on your Coupa API key/app
- **Exchange Rates**: Only available if multi-currency is enabled
- Most operations are read-only `GET`; write operations are PO actions (`PUT`), Order Lists/Order List Lines create/update (`POST`/`PUT`), and Supplier update (`PUT`).

## Version History

| Version | Summary |
|---|---|
| 1.2.0 | Added Order Lists (`order_pads`) and Order List Lines endpoints, supplier update, and Supplier Information GET endpoints; updated auth scope guidance to include `offline_access`, `core.order_pad.*`, and `core.supplier.write` |
| 1.1.9 | 62 paths; added requisition_lines, matching_allocations, payment_terms, exchange_rates, supplier_items, users, receipts, sub-resources; fixed 7 v118 bugs |
| 1.1.8 | Dynamic host; PO actions, PO changes, PO lines |
