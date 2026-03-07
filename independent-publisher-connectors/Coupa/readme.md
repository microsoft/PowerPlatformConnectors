# Coupa (Independent Publisher)

| | |
|---|---|
| **Publisher** | NovaGL |
| **Website** | [coupa.com](https://www.coupa.com) |
| **Privacy Policy** | [coupa.com/privacy-policy](https://www.coupa.com/privacy-policy) |
| **Categories** | Commerce, Finance & Accounting |
| **Version** | 12 |

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

**Optional scopes:**
```
core.purchase_order.read core.invoice.read core.requisition.read
core.contract.read core.supplier.read core.user.read core.common.read
core.sourcing.read core.sourcing.write
```
Add `core.purchase_order.write` only if using issue/close/cancel/reopen actions.
Add `core.order_pad.read` and `core.order_pad.write` for Order Lists endpoints.
Add `core.supplier.write` for supplier update operations.

## Supported Operations (78 total)

| Operation ID | Method | Path | Description |
|---|---|---|---|
| `GetAccounts` | GET | `/accounts` |  |
| `GetAccountById` | GET | `/accounts/{id}` |  |
| `GetAddresses` | GET | `/addresses` |  |
| `GetAddressById` | GET | `/addresses/{id}` |  |
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
| `GetMatchingAllocations` | GET | `/matching_allocations` | Query matching allocations — links PO lines, invoice lines and receipts (3-way match). |
| `GetMatchingAllocationById` | GET | `/matching_allocations/{id}` | Get a single matching allocation. |
| `GetOrderPads` | GET | `/order_pads` | Get Order Lists. |
| `CreateOrderPad` | POST | `/order_pads` | Create an Order List. |
| `GetOrderPadById` | GET | `/order_pads/{id}` | Get an Order List by ID. |
| `UpdateOrderPad` | PUT | `/order_pads/{id}` | Update an Order List. |
| `GetOrderPadLines` | GET | `/order_pads/{id}/order_pad_lines` | Get Order List Lines. |
| `CreateOrderPadLine` | POST | `/order_pads/{id}/order_pad_lines` | Create an Order List Line. |
| `GetOrderPadLineById` | GET | `/order_pads/{id}/order_pad_lines/{line_id}` | Get an Order List Line by ID. |
| `UpdateOrderPadLine` | PUT | `/order_pads/{id}/order_pad_lines/{line_id}` | Update an Order List Line. |
| `GetPaymentTerms` | GET | `/payment_terms` | Query payment terms reference data. |
| `GetPaymentTermById` | GET | `/payment_terms/{id}` | Get a single payment term. |
| `GetPurchaseOrderChanges` | GET | `/purchase_order_changes` |  |
| `GetPurchaseOrderChangeById` | GET | `/purchase_order_changes/{id}` |  |
| `GetPurchaseOrders` | GET | `/purchase_orders` |  |
| `GetPurchaseOrderById` | GET | `/purchase_orders/{id}` |  |
| `GetPurchaseOrderAttachments` | GET | `/purchase_orders/{id}/attachments` | Get attachments for a Purchase Order. |
| `CancelPurchaseOrder` | PUT | `/purchase_orders/{id}/cancel` |  |
| `ClosePurchaseOrder` | PUT | `/purchase_orders/{id}/close` |  |
| `GetPurchaseOrderComments` | GET | `/purchase_orders/{id}/comments` | Get comments for a Purchase Order. |
| `IssuePurchaseOrder` | PUT | `/purchase_orders/{id}/issue` |  |
| `ReopenPurchaseOrder` | PUT | `/purchase_orders/{id}/reopen` |  |
| `GetQuoteRequests` | GET | `/quote_requests` | Get sourcing events. Requires `core.sourcing.read`. |
| `CreateQuoteRequest` | POST | `/quote_requests` | Create a sourcing event. Requires `core.sourcing.write`. |
| `GetQuoteRequestById` | GET | `/quote_requests/{id}` | Get a sourcing event by ID. Requires `core.sourcing.read`. |
| `UpdateQuoteRequest` | PUT | `/quote_requests/{id}` | Update a sourcing event. Requires `core.sourcing.write`. |
| `GetQuoteResponses` | GET | `/quote_requests/{quote_request_id}/quote_responses` | Get most recent submitted responses per supplier for an event. Requires `core.sourcing.read`. |
| `GetAllQuoteResponsesForEvent` | GET | `/quote_requests/{quote_request_id}/quote_responses/all` | Get all responses for an event including drafts. Requires `core.sourcing.read`. |
| `GetAllQuoteResponses` | GET | `/quote_responses` | Get most recent submitted responses across all events. Requires `core.sourcing.read`. |
| `GetAllQuoteResponsesIncludingDrafts` | GET | `/quote_responses/all` | Get all responses across all events including drafts. Requires `core.sourcing.read`. |
| `GetQuoteResponseById` | GET | `/quote_responses/{id}` | Get a specific quote response. Requires `core.sourcing.read`. |
| `AwardQuoteResponse` | POST | `/quote_responses/{id}/award` | Award a supplier response. Requires `core.sourcing.write`. |
| `RemoveQuoteResponseAward` | DELETE | `/quote_responses/{id}/award` | Remove an award from a response. Requires `core.sourcing.write`. |
| `GetReceiptRequests` | GET | `/receipt_requests` | Query receipt requests (tenant config dependent). |
| `GetReceiptRequestById` | GET | `/receipt_requests/{id}` | Get a single receipt request. |
| `GetReceivingTransactions` | GET | `/receiving_transactions` |  |
| `GetReceivingTransactionById` | GET | `/receiving_transactions/{id}` |  |
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
