# Coupa (Independent Publisher)

| | |
|---|---|
| **Publisher** | NovaGL |
| **Website** | [coupa.com](https://www.coupa.com) |
| **Privacy Policy** | [coupa.com/privacy-policy](https://www.coupa.com/privacy-policy) |
| **Categories** | Commerce, Finance & Accounting |
| **Version** | 1.3.0 |

## Overview

This connector provides read and action access to the **Coupa REST API**
for Power Automate flows and Power Apps. It covers the full P2P lifecycle — requisitions,
purchase orders, invoices, receipts, contracts, suppliers, sourcing, inventory, and more.

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
core.sourcing.read core.sourcing.write core.inventory.read
core.budget.read core.approval.read
```
Add `core.purchase_order.write` only if using issue/close/cancel/reopen/release actions.
Add `core.order_pad.read` and `core.order_pad.write` for Order Lists endpoints.
Add `core.supplier.write` for supplier update operations.
Add `core.invoice.write` for invoice write/action operations (submit, void, dispute, etc.).

## Supported Operations (135 total)

| Operation ID | Method | Path | Description |
|---|---|---|---|
| `GetAccounts` | GET | `/accounts` | Get Accounts |
| `GetAccountById` | GET | `/accounts/{id}` | Get Account by ID |
| `GetAddresses` | GET | `/addresses` | Get Addresses |
| `GetAddressById` | GET | `/addresses/{id}` | Get Address by ID |
| `GetApprovals` | GET | `/approvals` | Get Approvals |
| `GetApprovalById` | GET | `/approvals/{id}` | Get Approval by ID |
| `GetBudgetLines` | GET | `/budget_lines` | Get Budget Lines |
| `GetBudgetLineById` | GET | `/budget_lines/{id}` | Get Budget Line by ID |
| `GetCommodities` | GET | `/commodities` | Get Commodities |
| `GetCommodityById` | GET | `/commodities/{id}` | Get Commodity by ID |
| `GetContracts` | GET | `/contracts` | Get Contracts |
| `Querycontractterms` | GET | `/contracts/{contract_id}/contract_terms` | Query contract terms |
| `Createcontractterm` | POST | `/contracts/{contract_id}/contract_terms` | Create contract term |
| `Showcontractterm` | GET | `/contracts/{contract_id}/contract_terms/{id}` | Show contract term |
| `Updatecontractterm` | PATCH | `/contracts/{contract_id}/contract_terms/{id}` | Update contract term |
| `GetContractById` | GET | `/contracts/{id}` | Get Contract by ID |
| `Manuallyaddanapproverforacontract` | PUT | `/contracts/{id}/add_approver` | Manually add an approver for a contract |
| `Movesthecontracttothecompletedstate.` | PUT | `/contracts/{id}/complete` | Moves the contract to the completed state. |
| `Createandpublishacontract` | POST | `/contracts/{id}/create_published` | Create and publish a contract |
| `PutRemoveanapproverwhowasmanuallyadded1` | PUT | `/contracts/{id}/remove_approval` | Remove an approver who was manually added |
| `Submitsthecontractforapproval.` | PUT | `/contracts/{id}/submit_for_approval` | Submits the contract for approval. |
| `Get_DataSources` | GET | `/data_file_sources` | Query Data Sources |
| `GetExchangeRates` | GET | `/exchange_rates` | Get Exchange Rates |
| `GetExchangeRateById` | GET | `/exchange_rates/{id}` | Get Exchange Rate by ID |
| `IntegrationRun_query` | GET | `/integration_runs` | Query Integration Runs |
| `IntegrationID_Query` | GET | `/integration_runs/{id}` | Query Integration by ID |
| `GetInventory` | GET | `/inventory` | Get Inventory |
| `GetInventoryById` | GET | `/inventory/{id}` | Get Inventory by ID |
| `GetInventoryTransactions` | GET | `/inventory_transactions` | Get Inventory Transactions |
| `GetInventoryTransactionById` | GET | `/inventory_transactions/{id}` | Get Inventory Transaction by ID |
| `GetInvoices` | GET | `/invoices` | Get Invoices |
| `DeleteInvoices` | DELETE | `/invoices/{id}` | Delete Invoice in New status |
| `GetInvoiceById` | GET | `/invoices/{id}` | Get Invoice by ID |
| `PatchInvoice` | PATCH | `/invoices/{id}` | Update invoice |
| `PutInvoice` | PUT | `/invoices/{id}` | Update invoice |
| `AbandonInvoices` | PUT | `/invoices/{id}/abandon` | Abandon invoice |
| `UpdateInvoicesAddApprover` | PUT | `/invoices/{id}/add_approver` | Manually add an approver for an invoice |
| `GetInvoiceAttachments` | GET | `/invoices/{id}/attachments` | Get Invoice Attachments |
| `Bypass_ApprovalsInvoices` | PUT | `/invoices/{id}/bypass_approvals` | Bypass approvals |
| `UpdateInvoicesBypassCurrentApproval` | PUT | `/invoices/{id}/bypass_current_approval` | Bypass current approval |
| `GetInvoiceComments` | GET | `/invoices/{id}/comments` | Get Invoice Comments |
| `DisputeInvoices` | PUT | `/invoices/{id}/dispute` | Dispute invoice |
| `UpdateInvoicesFlipToAdvanceShipNotice` | PUT | `/invoices/{id}/flip_to_advance_ship_notice` | Flip invoice to Advance Ship Notice |
| `UpdateInvoicesImageScan` | PUT | `/invoices/{id}/image_scan` | Update image scan attachment |
| `UpdateInvoicesRemoveApproval` | PUT | `/invoices/{id}/remove_approval` | Remove an approver who was manually added |
| `Restart_ApprovalsInvoices` | PUT | `/invoices/{id}/restart_approvals` | Restart approvals |
| `Retrieve_Downloadclearancedocument` | GET | `/invoices/{id}/retrieve_clearance_document` | Retrieve/Download clearance document |
| `Retrieve_Downloadimagescan` | GET | `/invoices/{id}/retrieve_image_scan` | Retrieve/Download image scan |
| `GetInvoicesRetrieveLegalInvoicePdf` | GET | `/invoices/{id}/retrieve_legal_invoice_pdf` | Retrieve/Download legal invoice pdf |
| `Revalidate_TolerancesInvoices` | PUT | `/invoices/{id}/revalidate_tolerances` | Revalidate tolerances |
| `SubmitInvoices` | PUT | `/invoices/{id}/submit` | Submit draft invoice for approval |
| `UpdateInvoicesUpdateLineAccounts` | PUT | `/invoices/{id}/update_line_accounts` | Update line accounts |
| `VoidInvoices` | PUT | `/invoices/{id}/void` | Void an approved invoice |
| `UpdateInvoicesWithdrawDispute` | PUT | `/invoices/{id}/withdraw_dispute` | Withdraw dispute |
| `GetItems` | GET | `/items` | Get Items |
| `GetItemById` | GET | `/items/{id}` | Get Item by ID |
| `Querysupplieritems` | GET | `/items/{item_id}/supplier_items` | Query supplier items |
| `Createsupplieritem` | POST | `/items/{item_id}/supplier_items` | Create supplier item |
| `Showsupplieritem` | GET | `/items/{item_id}/supplier_items/{id}` | Show supplier item |
| `Updatesupplieritem` | PATCH | `/items/{item_id}/supplier_items/{id}` | Update supplier item |
| `GetLookupValues` | GET | `/lookup_values` | Get Lookup Values |
| `GetLookupValueById` | GET | `/lookup_values/{id}` | Get Lookup Value by ID |
| `GetLookups` | GET | `/lookups` | Get Lookups |
| `GetLookupById` | GET | `/lookups/{id}` | Get Lookup by ID |
| `GetOrderPads` | GET | `/order_pads` | Get Order Lists |
| `CreateOrderPad` | POST | `/order_pads` | Create Order List |
| `GetOrderPadById` | GET | `/order_pads/{id}` | Get Order List by ID |
| `UpdateOrderPad` | PUT | `/order_pads/{id}` | Update Order List |
| `GetOrderPadLines` | GET | `/order_pads/{order_pad_id}/order_pad_lines` | Get Order List Lines |
| `CreateOrderPadLine` | POST | `/order_pads/{order_pad_id}/order_pad_lines` | Create Order List Line |
| `GetOrderPadLineById` | GET | `/order_pads/{order_pad_id}/order_pad_lines/{id}` | Get Order List Line by ID |
| `UpdateOrderPadLine` | PUT | `/order_pads/{order_pad_id}/order_pad_lines/{id}` | Update Order List Line |
| `GetPaymentTerms` | GET | `/payment_terms` | Get Payment Terms |
| `GetPaymentTermById` | GET | `/payment_terms/{id}` | Get Payment Term by ID |
| `GetPurchaseOrderChanges` | GET | `/purchase_order_changes` | Get Purchase Order Changes |
| `GetPurchaseOrderChangeById` | GET | `/purchase_order_changes/{id}` | Get Purchase Order Change by ID |
| `Manuallyaddapproverforanorderheaderchange` | PUT | `/purchase_order_changes/{id}/add_approver` | Manually add approver for an order header change |
| `Removeanapproverwhowasmanuallyadded` | PUT | `/purchase_order_changes/{id}/remove_approval` | Remove an approver who was manually added |
| `SubmitPurchaseOrderforApproval` | PUT | `/purchase_order_changes/{id}/submit_for_approval` | Submit Purchase Order for Approval |
| `GetPurchaseOrderLines` | GET | `/purchase_order_lines` | Get PO Lines |
| `GetPurchaseOrderLineById` | GET | `/purchase_order_lines/{id}` | Get PO Line by ID |
| `Reopenforinvoicing` | PUT | `/purchase_order_lines/{id}/reopen_for_invoicing` | Reopen for invoicing |
| `Reopenforreceiving` | PUT | `/purchase_order_lines/{id}/reopen_for_receiving` | Reopen for receiving |
| `Softcloseforinvoicing` | PUT | `/purchase_order_lines/{id}/soft_close_for_invoicing` | Soft close for invoicing |
| `Softcloseforreceiving` | PUT | `/purchase_order_lines/{id}/soft_close_for_receiving` | Soft close for receiving |
| `GetPurchaseOrders` | GET | `/purchase_orders` | Get Purchase Orders |
| `GetPurchaseOrderById` | GET | `/purchase_orders/{id}` | Get Purchase Order by ID |
| `GetPurchaseOrderAttachments` | GET | `/purchase_orders/{id}/attachments` | Get Purchase Order Attachments |
| `CancelPurchaseOrder` | PUT | `/purchase_orders/{id}/cancel` | Cancel Purchase Order |
| `ClosePurchaseOrder` | PUT | `/purchase_orders/{id}/close` | Close Purchase Order |
| `GetPurchaseOrderComments` | GET | `/purchase_orders/{id}/comments` | Get Purchase Order Comments |
| `Comments_PO_Create` | POST | `/purchase_orders/{id}/comments` | Comments - Add Purchase Order Comment |
| `IssuePurchaseOrder` | PUT | `/purchase_orders/{id}/issue` | Issue Purchase Order |
| `Releasepurchaseorderonbuyerhold` | PUT | `/purchase_orders/{id}/release_from_buyer_hold` | Release purchase order on buyer hold |
| `ReopenPurchaseOrder` | PUT | `/purchase_orders/{id}/reopen` | Reopen Purchase Order |
| `GetQuoteRequests` | GET | `/quote_requests` | Get Quote Requests |
| `CreateQuoteRequest` | POST | `/quote_requests` | Create Quote Request |
| `GetQuoteRequestById` | GET | `/quote_requests/{id}` | Get Quote Request by ID |
| `UpdateQuoteRequest` | PUT | `/quote_requests/{id}` | Update Quote Request |
| `GetQuoteResponses` | GET | `/quote_requests/{quote_request_id}/quote_responses` | Get Quote Responses for Event |
| `GetAllQuoteResponsesForEvent` | GET | `/quote_requests/{quote_request_id}/quote_responses/all` | Get All Quote Responses for Event (including drafts) |
| `GetAllQuoteResponses` | GET | `/quote_responses` | Get Quote Responses (All Events) |
| `GetAllQuoteResponsesIncludingDrafts` | GET | `/quote_responses/all` | Get All Quote Responses Including Drafts (All Events) |
| `GetQuoteResponseById` | GET | `/quote_responses/{id}` | Get Quote Response by ID |
| `RemoveQuoteResponseAward` | DELETE | `/quote_responses/{id}/award` | Remove Award from Quote Response |
| `AwardQuoteResponse` | POST | `/quote_responses/{id}/award` | Award Quote Response |
| `GetReceiptRequests` | GET | `/receipt_requests` | List Receipt Requests |
| `GetReceiptRequestById` | GET | `/receipt_requests/{id}` | Get Receipt Request by ID |
| `GetReceivingTransactions` | GET | `/receiving_transactions` | Get Receiving Transactions |
| `GetReceivingTransactionById` | GET | `/receiving_transactions/{id}` | Get Receiving Transaction by ID |
| `GetRequisitions` | GET | `/requisitions` | Get Requisitions |
| `Add_To_Cart_FreeText` | POST | `/requisitions/add_to_cart` | Add To Cart (Free Text) |
| `Sendcurrentcartforuser` | GET | `/requisitions/current_cart` | Send current cart for user |
| `QueryMyRequistions` | GET | `/requisitions/mine` | Requistions- Query My Requistions |
| `Createarequisitionandsubmit` | POST | `/requisitions/submit_for_approval` | Create a requisition and submit |
| `GetRequisitionById` | GET | `/requisitions/{id}` | Get Requisition by ID |
| `Manuallyaddanapproverforarequisition` | PUT | `/requisitions/{id}/add_approver` | Manually add an approver for a requisition |
| `PutRemoveanapproverwhowasmanuallyadded` | PUT | `/requisitions/{id}/remove_approval` | Remove an approver who was manually added |
| `Updaterequisitionandsubmitforapproval` | PUT | `/requisitions/{id}/update_and_submit_for_approval` | Update requisition and submit for approval |
| `GetSupplierInformation` | GET | `/supplier_information` | Get Supplier Information |
| `GetSupplierInformationById` | GET | `/supplier_information/{id}` | Get Supplier Information by ID |
| `GetSupplierItems` | GET | `/supplier_items` | Get Supplier Items |
| `Advancedsupplieritemsearch` | GET | `/supplier_items/search` | Advanced supplier item search |
| `GetSupplierItemById` | GET | `/supplier_items/{id}` | Get Supplier Item by ID |
| `GetSuppliers` | GET | `/suppliers` | Get Suppliers |
| `GetSupplierById` | GET | `/suppliers/{id}` | Get Supplier by ID |
| `UpdateSupplier` | PUT | `/suppliers/{id}` | Update Supplier |
| `GetUOMs` | GET | `/uoms` | Get UOMs |
| `GetUOMById` | GET | `/uoms/{id}` | Get UOM by ID |
| `GetUserGroupMemberships` | GET | `/user_group_memberships` | Get User Group Memberships |
| `GetUserGroupMembershipById` | GET | `/user_group_memberships/{id}` | Get User Group Membership by ID |
| `GetUserGroups` | GET | `/user_groups` | Get User Groups |
| `GetUserGroupById` | GET | `/user_groups/{id}` | Get User Group by ID |
| `GetUsers` | GET | `/users` | Get Users |
| `GetUserById` | GET | `/users/{id}` | Get User by ID |

> **Invoice Lines:** `/invoice_lines` does not exist as a standalone Coupa endpoint.
> Use `GetInvoices` or `GetInvoiceById` with `return_object=shallow` to get embedded invoice line data.

> **Inventory Transactions:** `/inventory_transactions` has no standalone line endpoint.
> Use `GetInventoryTransactions` with date range filters for incremental polling.

## Dynamic Host

All API calls route to `https://{instance}.coupahost.com/api` at runtime via the
`dynamichosturl` policy. One connector, any tenant.

## Incremental Polling

All collection (`GET` list) endpoints support incremental polling via:
- `updated-at[gt]` — return only records updated after a given datetime (ISO 8601)
- `updated-at[lt]` — upper bound for the update window
- `created-at[gt]` — return only records created after a given datetime

These parameters are marked as `x-ms-trigger: batch`, making them usable as
Power Automate scheduled trigger sources. Use `limit` (max 50) and `offset` to page through results.

## Pagination

`limit` is capped at **50 records per request**. To retrieve all records:
1. Call with `limit=50&offset=0`
2. If 50 records returned, repeat with `offset=50`, `offset=100`, etc.
3. Stop when fewer than 50 records are returned.

For ongoing sync, use `updated-at[gt]` with your last-run timestamp instead of full re-pagination.

## Known Issues and Limitations

- **Receipts**: Availability depends on tenant config — confirm with your Coupa admin
- **Matching Allocations**: Only populated if 3-way match is enabled on the tenant
- **PO Actions** (issue/close/cancel/reopen/release): Require `core.purchase_order.write` scope
- **Invoice Actions** (submit/void/dispute/bypass etc.): Require `core.invoice.write` scope
- **Order Lists** (create/update and line create/update): Require `core.order_pad.write` scope
- **Suppliers Update**: Requires `core.supplier.write` scope
- **Supplier Information (SIM)**: Requires SIM API permissions on your Coupa API key/app
- **Exchange Rates**: Only available if multi-currency is enabled
- **Inventory**: Availability depends on tenant configuration
- **Schema depth**: Nested objects (e.g. invoice charges, supplier sites, legal entities) are returned
  as summary objects with key fields only. Use `return_object=shallow` or `fields` parameter for full depth via raw API if needed.
- Both `PATCH` and `PUT` are available for invoice updates: use `PATCH` for partial field updates, `PUT` for full object replacement.

## Version History

| Version | Summary |
|---|---|
| 1.3.0 | 135 operations; added incremental polling (`x-ms-trigger: batch`) to all collection endpoints; added supplier part number filter to `/purchase_orders`; consolidated duplicate PO comments path; fixed schema 512-node limit across 6 definitions; refactored to global `$ref` parameters and responses (47% file size reduction); fixed enum type mismatches; updated operation count |
| 1.2.0 | Added Order Lists (`order_pads`) and Order List Lines endpoints, supplier update, and Supplier Information GET endpoints; updated auth scope guidance to include `offline_access`, `core.order_pad.*`, and `core.supplier.write` |
| 1.1.9 | 62 paths; added requisition_lines, matching_allocations, payment_terms, exchange_rates, supplier_items, users, receipts, sub-resources; fixed 7 v118 bugs |
| 1.1.8 | Dynamic host; PO actions, PO changes, PO lines |
