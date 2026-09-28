# Coupa P2P Connector

**Publisher:** NovaGL  
**Version:** 1.8.15  
**Operations:** 152  
**Entities:** 33  

A Power Automate custom connector for the Coupa Procurement & Accounts Payable API. Covers purchase orders, invoices, requisitions, contracts, sourcing, suppliers, and more.

---

## Prerequisites

- A Coupa instance with API access enabled
- An OAuth2 client configured in Coupa under **Setup → OAuth2/OpenID Connect Clients**
- The client must use the **client_credentials** grant type
- Appropriate OAuth2 scopes granted to the client

---

## Authentication

This connector uses **No Authentication** with a policy rewrite — the confirmed community workaround for injecting a dynamic Bearer token in Power Automate custom connectors.

### How It Works

Every action exposes an **Access Token** header field. A connector policy rewrites this into the real `Authorization` header before the request reaches Coupa:

```
Flow passes:     Access-Token: Bearer eyJ...
Policy rewrites: Authorization: Bearer eyJ...
Coupa receives:  Authorization: Bearer eyJ...
```

This works because the platform only blocks adding `Authorization` as a parameter directly — it does not block a policy from setting it based on another header value.

### Getting a Bearer Token

Use the standard Power Automate **HTTP action** at the top of each flow to obtain a token from Coupa's OAuth2 endpoint:

```
Method:  POST
URI:     https://{instance}.{domain}/oauth2/token
Headers: Content-Type: application/x-www-form-urlencoded
Body:    grant_type=client_credentials
         &client_id={your-client-id}
         &client_secret={your-client-secret}
         &scope={your-scopes}
```

### Connection Setup

When creating a connection provide two fields:

| Field | Description |
|---|---|
| **Coupa Instance Name** | Subdomain only — e.g. `mycompany-test` (not the full URL) |
| **Coupa Domain** | `coupahost.com` (standard) or `coupacloud.com` |

### Flow Pattern

Every flow should follow this pattern:

**Step 1 — HTTP action** to get token (see above)

**Step 2 — Initialize Variable**

| Field | Value |
|---|---|
| Name | `token` |
| Type | String |
| Value | `concat('Bearer ', body('HTTP')?['access_token'])` |

**Step 3 — Connector action** — pass `@{variables('token')}` in the **Access Token** field of every connector action

### Token Expiry

Tokens expire after approximately 1 hour. Always fetch a fresh token at the start of each flow run — never cache tokens between runs.

### Recommended Scopes

Request only the scopes your flows need:

| Scope | Access |
|---|---|
| `core.purchase_order.read` | Read purchase orders |
| `core.purchase_order.write` | Create/update purchase orders |
| `core.invoice.read` | Read invoices |
| `core.invoice.write` | Create/update invoices |
| `core.requisition.read` | Read requisitions |
| `core.requisition.write` | Create/update requisitions |
| `core.contract.read` | Read contracts |
| `core.contract.write` | Create/update contracts |
| `core.supplier.read` | Read suppliers |
| `core.supplier.write` | Update suppliers |
| `core.user.read` | Read users |
| `core.common.read` | Read common data (accounts, addresses, etc.) |
| `core.sourcing.read` | Read sourcing events |
| `core.sourcing.write` | Create/update sourcing events |
| `core.inventory.read` | Read inventory |
| `core.budget.read` | Read budget lines |
| `core.approval.read` | Read approvals |
| `core.receiving.read` | Read receiving transactions |
| `core.receiving.write` | Create receiving transactions |
| `core.order_pad.read` | Read order lists |
| `core.order_pad.write` | Create/update order lists |

---

## Example Flow — Get the Most Recent Purchase Order

**Step 1 — HTTP** (get token)

| Field | Value |
|---|---|
| Method | `POST` |
| URI | `https://mycompany-test.coupahost.com/oauth2/token` |
| Headers | `Content-Type: application/x-www-form-urlencoded` |
| Body | `grant_type=client_credentials&client_id=xxx&client_secret=xxx&scope=core.purchase_order.read` |

**Step 2 — Initialize Variable**

| Field | Value |
|---|---|
| Name | `token` |
| Type | String |
| Value | `concat('Bearer ', body('HTTP')?['access_token'])` |

**Step 3 — Purchase Order: Get All**

| Field | Value |
|---|---|
| Access Token | `@{variables('token')}` |
| Limit | `1` |
| Order By | `created-at` |
| Direction | `desc` |

**Step 4 — Use the result**

```
first(body('Purchase_Order_Get_All'))
first(body('Purchase_Order_Get_All'))?['po-number']
first(body('Purchase_Order_Get_All'))?['id']
```

---

## Operations

### Account (2)
| Action | Description |
|---|---|
| Account: Get All | Retrieve all accounts |
| Account: Get by ID | Retrieve a single account by ID |

### Address (2)
| Action | Description |
|---|---|
| Address: Get All | Retrieve all addresses |
| Address: Get by ID | Retrieve a single address by ID |

### Approval (2)
| Action | Description |
|---|---|
| Approval: Get All | Retrieve all approvals |
| Approval: Get by ID | Retrieve a single approval by ID |

### Budget Line (2)
| Action | Description |
|---|---|
| Budget Line: Get All | Retrieve all budget lines |
| Budget Line: Get by ID | Retrieve a single budget line by ID |

### Commodity (2)
| Action | Description |
|---|---|
| Commodity: Get All | Retrieve all commodities |
| Commodity: Get by ID | Retrieve a single commodity by ID |

### Contract (13)
| Action | Description |
|---|---|
| Contract: Get All | Retrieve all contracts |
| Contract: Get by ID | Retrieve a single contract by ID |
| Contract: Get Terms | Retrieve all terms for a contract |
| Contract: Get Term by ID | Retrieve a specific contract term by ID |
| Contract: Create Term | Add a new term to a contract |
| Contract: Update Term | Update an existing contract term |
| Contract: Get Attachments | Retrieve all attachments on a contract |
| Contract: Post Attachment (Text or URL) | Add a text or URL attachment to a contract |
| Contract: Add Approver | Manually add an approver to a contract |
| Contract: Remove Approver | Remove a manually added approver from a contract |
| Contract: Submit for Approval | Submit a contract for approval |
| Contract: Complete | Move a contract to the completed state |
| Contract: Create and Publish | Create and publish a contract |

### Data Source (1)
| Action | Description |
|---|---|
| Data Source: Get All | Retrieve all data file sources |

### Exchange Rate (2)
| Action | Description |
|---|---|
| Exchange Rate: Get All | Retrieve all exchange rates |
| Exchange Rate: Get by ID | Retrieve a single exchange rate by ID |

### Integration Run (2)
| Action | Description |
|---|---|
| Integration Run: Get All | Retrieve all integration runs |
| Integration Run: Get by ID | Retrieve a single integration run by ID |

### Inventory (2)
| Action | Description |
|---|---|
| Inventory: Get All | Retrieve all inventory records |
| Inventory: Get by ID | Retrieve a single inventory record by ID |

### Inventory Transaction (2)
| Action | Description |
|---|---|
| Inventory Transaction: Get All | Retrieve all inventory transactions |
| Inventory Transaction: Get by ID | Retrieve a single inventory transaction by ID |

### Invoice (25)
| Action | Description |
|---|---|
| Invoice: Get All | Retrieve all invoices |
| Invoice: Get by ID | Retrieve a single invoice by ID |
| Invoice: Update (Partial) | Partially update an invoice (PATCH) |
| Invoice: Update (Full) | Fully update an invoice (PUT) |
| Invoice: Delete | Delete an invoice |
| Invoice: Get Attachments | Retrieve all attachments on an invoice |
| Invoice: Post Attachment (Text or URL) | Add a text or URL attachment to an invoice |
| Invoice: Get Comments | Retrieve all comments on an invoice |
| Invoice: Post Comment | Add a comment to an invoice |
| Invoice: Abandon | Abandon an invoice |
| Invoice: Add Approver | Manually add an approver to an invoice |
| Invoice: Bypass Approvals | Bypass the full approval workflow |
| Invoice: Bypass Current Approval | Bypass only the current approval step |
| Invoice: Dispute | Dispute an invoice |
| Invoice: Withdraw Dispute | Withdraw a dispute on an invoice |
| Invoice: Flip to Advance Ship Notice | Convert an invoice to an advance ship notice |
| Invoice: Remove Approver | Remove a manually added approver |
| Invoice: Restart Approvals | Restart the approval workflow |
| Invoice: Revalidate Tolerances | Revalidate tolerance checks |
| Invoice: Submit for Approval | Submit an invoice for approval |
| Invoice: Update Line Accounts | Update account coding on invoice lines |
| Invoice: Void | Void an invoice |
| Invoice: Download Clearance Doc | Download the clearance document |
| Invoice: Download Image Scan | Download the image scan |
| Invoice: Download Legal Invoice PDF | Download the legal invoice PDF |

### Item (2)
| Action | Description |
|---|---|
| Item: Get All | Retrieve all items |
| Item: Get by ID | Retrieve a single item by ID |

### Lookup (2)
| Action | Description |
|---|---|
| Lookup: Get All | Retrieve all lookups |
| Lookup: Get by ID | Retrieve a single lookup by ID |

### Lookup Value (4)
| Action | Description |
|---|---|
| Lookup Value: Get All | Retrieve all lookup values |
| Lookup Value: Get by ID | Retrieve a single lookup value by ID |
| Lookup Value: Create | Create a new lookup value |
| Lookup Value: Update | Update an existing lookup value |

### Order List (4)
| Action | Description |
|---|---|
| Order List: Get All | Retrieve all order lists |
| Order List: Get by ID | Retrieve a single order list by ID |
| Order List: Create | Create a new order list |
| Order List: Update | Update an existing order list |

### Order List Line (4)
| Action | Description |
|---|---|
| Order List Line: Get All | Retrieve all order list lines |
| Order List Line: Get by ID | Retrieve a single order list line by ID |
| Order List Line: Create | Create a new order list line |
| Order List Line: Update | Update an existing order list line |

### Payment Term (2)
| Action | Description |
|---|---|
| Payment Term: Get All | Retrieve all payment terms |
| Payment Term: Get by ID | Retrieve a single payment term by ID |

### PO Change (5)
| Action | Description |
|---|---|
| PO Change: Get All | Retrieve all purchase order changes |
| PO Change: Get by ID | Retrieve a single PO change by ID |
| PO Change: Add Approver | Manually add an approver to a PO change |
| PO Change: Remove Approver | Remove a manually added approver |
| PO Change: Submit for Approval | Submit a PO change for approval |

### PO Line (8)
| Action | Description |
|---|---|
| PO Line: Get All | Retrieve all purchase order lines |
| PO Line: Get by ID | Retrieve a single PO line by ID |
| PO Line: Get Attachments | Retrieve all attachments on a PO line |
| PO Line: Post Attachment (Text or URL) | Add a text or URL attachment to a PO line |
| PO Line: Reopen for Invoicing | Reopen a PO line for invoicing |
| PO Line: Reopen for Receiving | Reopen a PO line for receiving |
| PO Line: Soft Close for Invoicing | Soft close a PO line for invoicing |
| PO Line: Soft Close for Receiving | Soft close a PO line for receiving |

### Purchase Order (11)
| Action | Description |
|---|---|
| Purchase Order: Get All | Retrieve all purchase orders |
| Purchase Order: Get by ID | Retrieve a single purchase order by ID |
| Purchase Order: Get Attachments | Retrieve all attachments on a purchase order |
| Purchase Order: Post Attachment (Text or URL) | Add a text or URL attachment to a purchase order |
| Purchase Order: Get Comments | Retrieve all comments on a purchase order |
| Purchase Order: Post Comment | Add a comment to a purchase order |
| Purchase Order: Cancel | Cancel a purchase order |
| Purchase Order: Close | Close a purchase order |
| Purchase Order: Issue | Issue a purchase order to the supplier |
| Purchase Order: Release from Buyer Hold | Release a purchase order from buyer hold |
| Purchase Order: Reopen | Reopen a closed purchase order |

### Receipt Request (2)
| Action | Description |
|---|---|
| Receipt Request: Get All | Retrieve all receipt requests |
| Receipt Request: Get by ID | Retrieve a single receipt request by ID |

### Receiving Transaction (4)
| Action | Description |
|---|---|
| Receiving Transaction: Get All | Retrieve all receiving transactions |
| Receiving Transaction: Get by ID | Retrieve a single receiving transaction by ID |
| Receiving Transaction: Get Attachments | Retrieve all attachments on a receiving transaction |
| Receiving Transaction: Post Attachment (Text or URL) | Add a text or URL attachment to a receiving transaction |

### Requisition (13)
| Action | Description |
|---|---|
| Requisition: Get All | Retrieve all requisitions |
| Requisition: Get by ID | Retrieve a single requisition by ID |
| Requisition: Get Attachments | Retrieve all attachments on a requisition |
| Requisition: Post Attachment (Text or URL) | Add a text or URL attachment to a requisition |
| Requisition: Get Comments | Retrieve all comments on a requisition |
| Requisition: Post Comment | Add a comment to a requisition |
| Requisition: Get Mine | Retrieve requisitions belonging to the authenticated user |
| Requisition: Get Current Cart | Retrieve the current shopping cart |
| Requisition: Add to Cart (Free Text) | Add a free-text line item to the current cart |
| Requisition: Add Approver | Manually add an approver to a requisition |
| Requisition: Remove Approver | Remove a manually added approver |
| Requisition: Create and Submit | Create a requisition and submit for approval |
| Requisition: Update and Submit | Update a requisition and submit for approval |

### Requisition Line (2)
| Action | Description |
|---|---|
| Requisition Line: Get Attachments | Retrieve all attachments on a requisition line |
| Requisition Line: Post Attachment (Text or URL) | Add a text or URL attachment to a requisition line |

### Sourcing (13)
| Action | Description |
|---|---|
| Sourcing: Get Quote Requests | Retrieve all quote requests |
| Sourcing: Get Quote Request by ID | Retrieve a single quote request by ID |
| Sourcing: Create Quote Request | Create a new quote request |
| Sourcing: Update Quote Request | Update an existing quote request |
| Sourcing: Get Quote Request Attachments | Retrieve attachments on a quote request |
| Sourcing: Post Quote Request Attachment (Text or URL) | Add a text or URL attachment to a quote request |
| Sourcing: Get Quote Response by ID | Retrieve a single quote response by ID |
| Sourcing: Get All Quote Responses | Retrieve all quote responses across all events |
| Sourcing: Get All Quote Responses (incl. Drafts) | Retrieve all quote responses including drafts |
| Sourcing: Get Quote Responses for Event | Retrieve quote responses for a specific event |
| Sourcing: Get Quote Responses for Event (All) | Retrieve all quote responses for a specific event including drafts |
| Sourcing: Award Quote Response | Award a quote response to a supplier |
| Sourcing: Remove Quote Response Award | Remove an award from a quote response |

### Supplier (3)
| Action | Description |
|---|---|
| Supplier: Get All | Retrieve all suppliers |
| Supplier: Get by ID | Retrieve a single supplier by ID |
| Supplier: Update | Update an existing supplier |

### Supplier Information (2)
| Action | Description |
|---|---|
| Supplier Information: Get All | Retrieve all supplier information records |
| Supplier Information: Get by ID | Retrieve a single supplier information record by ID |

### Supplier Item (6)
| Action | Description |
|---|---|
| Supplier Item: Get All | Retrieve all supplier items |
| Supplier Item: Get by ID | Retrieve a single supplier item by ID |
| Supplier Item: Get by Item | Retrieve supplier items for a specific item |
| Supplier Item: Get by Item and ID | Retrieve a specific supplier item for a specific item |
| Supplier Item: Create | Create a new supplier item |
| Supplier Item: Update | Update an existing supplier item |

### UOM (2)
| Action | Description |
|---|---|
| UOM: Get All | Retrieve all units of measure |
| UOM: Get by ID | Retrieve a single unit of measure by ID |

### User (2)
| Action | Description |
|---|---|
| User: Get All | Retrieve all users |
| User: Get by ID | Retrieve a single user by ID |

### User Group (2)
| Action | Description |
|---|---|
| User Group: Get All | Retrieve all user groups |
| User Group: Get by ID | Retrieve a single user group by ID |

### User Group Membership (2)
| Action | Description |
|---|---|
| User Group Membership: Get All | Retrieve all user group memberships |
| User Group Membership: Get by ID | Retrieve a single user group membership by ID |

---

## Known Limitations

### File Uploads Not Supported
File attachment uploads (`multipart/form-data`) are not supported in Power Automate custom connectors. To upload files to Coupa attachments, use the standard **HTTP action** directly with multipart encoding.

### Token Management
The connector does not handle token refresh automatically. Tokens expire after approximately 1 hour. Always fetch a fresh token using the HTTP action at the start of each flow run.

### Numeric Fields Returned as Strings
Coupa returns monetary and quantity values (totals, prices, amounts) as strings rather than numbers — e.g. `"1250.00"` not `1250.00`. This is a known Coupa quirk. Account for this when using these values in calculations within your flow.

### Boolean Fields
Some Coupa boolean fields are returned as `"true"`/`"false"` strings rather than actual booleans. Use `equals(field, 'true')` rather than direct boolean comparison in flow conditions.

### Independent Publisher OAuth2 Limitation
Power Automate does not support the OAuth2 client credentials grant type for independent publisher connectors. The Access-Token header pattern used by this connector is the established community workaround.

---

## Filtering and Pagination

All **Get All** operations support these standard parameters:

| Parameter | Description |
|---|---|
| **Limit** | Maximum records to return (max 50 per call) |
| **Offset** | Records to skip — use for pagination |
| **Order By** | Field name to sort by e.g. `created-at` |
| **Direction** | Sort direction: `asc` or `desc` |
| **Updated After** | Return records updated after this ISO 8601 date — ideal for polling/delta flows |
| **Updated Before** | Return records updated before this ISO 8601 date |
| **Created After** | Return records created after this ISO 8601 date |
| **Return Object** | Response depth: `limited`, `shallow`, or omit for full |

### Pagination Pattern

```
Do Until (no more results):
  Purchase Order: Get All
    Limit:  50
    Offset: @{variables('offset')}
  
  Append to array variable: @{body('Purchase_Order_Get_All')}
  
  Increment offset by 50
  
  Condition: length(body('Purchase_Order_Get_All')) is less than 50
```

### Delta / Polling Pattern

```
Purchase Order: Get All
  Updated After: @{addHours(utcNow(), -1)}
  Order By:      updated-at
  Direction:     desc
```

---

## Version History

| Version | Changes |
|---|---|
| 1.8.15 | Added descriptions to 738 responses, 739 definition properties, and 1 missing parameter |
| 1.8.14 | Fixed ContractTerm.id type; cleaned up operationIds (CompleteContract, SubmitContractForApproval) |
| 1.8.13 | Added ContractTerm definition; fixed Contract Terms schemas |
| 1.8.12 | Removed Invoice image scan multipart op; full nested schema audit |
| 1.8.11 | Fixed 13 wrong response schemas on nested paths (Supplier Item, Order List Line, Sourcing, Contract Terms) |
| 1.8.10 | Removed undocumented supplier_items/search endpoint |
| 1.8.9 | Stripped all leaf type constraints from definitions — no more type validation on responses |
| 1.8.8 | Fixed 36 missing parameter summaries and descriptions |
| 1.8.7 | Removed all 9 multipart file upload operations (platform limitation) |
| 1.8.6 | Fixed non-ID integers and date-time nullable; comprehensive type audit |
| 1.8.5 | Fixed number (no format) and boolean type fields |
| 1.8.4 | Converted all number/double fields to string (Coupa returns monetary values as strings) |
| 1.8.3 | Fixed inline schemas; added missing definitions (InventoryRecord, InventoryTransaction, ReceiptRequest, DataFileSource); fixed attachment/comment $refs |
| 1.8.2 | Fixed 4 inline object schemas in integration runs, data sources, and requisition comments |
| 1.8.1 | Added descriptions to 27 operations missing them |
| 1.8.0 | Set all 162 operations to important visibility |
| 1.7.9 | Stripped all type constraints from definition properties |
| 1.7.8 | Removed all response schemas to prevent type validation errors |
| 1.7.7 | Fixed response type enforcement errors |
| 1.7.6 | Fixed parameter visibility — important/advanced/internal correctly applied |
| 1.7.5 | Fixed summary inconsistencies; moved Item supplier ops to Supplier Item group |
| 1.7.4 | Fixed Requisition: Get Mine and Add to Cart summaries |
| 1.7.3 | Replaced prefix abbreviations with full entity names in all summaries |
| 1.7.2 | Set Get All operations to important visibility |
| 1.7.1 | Removed all x-ms-trigger: batch flags — Get All ops were hidden as triggers |
| 1.7.0 | Full rebuild from v1.3.0 base — Access-Token auth, full summaries, operationIds, attachments, comments |
| 1.3.0 | Original base file |
