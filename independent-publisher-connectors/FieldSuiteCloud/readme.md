# Field Suite Cloud — Power Automate Independent Publisher Connector

This folder contains the OpenAPI definition and supporting assets needed to submit Field Suite Cloud as an Independent Publisher connector on the Microsoft Power Platform.

## What is included

- `openapi.yaml` — OpenAPI 3.0.1 spec describing the public Field Suite Cloud API surface.
- `assets/icon-*.png` — Connector icon in the sizes required by Microsoft.
- `README.md` — This file.
- `SUBMISSION.md` — Step-by-step Independent Publisher submission checklist.
- `SAMPLE-FLOWS.md` — Example Power Automate flows for certification screenshots.

## Connector capabilities

### Actions

| Action | Operation | Description |
|---|---|---|
| List Customers | `ListCustomers` | Returns customers in the tenant. |
| Get Customer | `GetCustomer` | Returns one customer by ID. |
| Create Customer | `CreateCustomer` | Adds a new customer. |
| Update Customer | `UpdateCustomer` | Updates an existing customer. |
| List Work Orders | `ListWorkOrders` | Returns work orders for the tenant. |
| Get Work Order | `GetWorkOrder` | Returns one work order by ID. |
| Create Work Order | `CreateWorkOrder` | Creates a new work order. |
| Update Work Order | `UpdateWorkOrder` | Edits a work order. |
| Update Work Order Status | `UpdateWorkOrderStatus` | Changes work order status. |
| List Invoices | `ListInvoices` | Returns invoices for the tenant. |
| Get Invoice | `GetInvoice` | Returns one invoice by ID. |
| Create Invoice | `CreateInvoice` | Creates a new invoice. |
| Mark Invoice Paid | `MarkInvoicePaid` | Marks an invoice as paid. |
| List Technicians | `ListTechnicians` | Returns crew members / technicians. |
| Create Technician | `CreateTechnician` | Adds a new technician. |

### Authentication

The connector uses **Bearer token (JWT)** auth. A user must already be authenticated to Field Suite Cloud. The same `Authorization: Bearer <token>` header used by the Flutter app and other connectors is sent on every request.

## API host

The OpenAPI spec points at the production Field Suite Cloud backend:

```
https://pantherhive-api.hotwodi4.workers.dev
```

All endpoints are under `/api/*` and are already live and used by the Field Suite Cloud Flutter application.

## Branding

| Field | Value |
|---|---|
| Connector name | Field Suite Cloud |
| Description | Connect Field Suite Cloud work orders, customers, technicians, and invoices with Power Automate. |
| Color | `#2563EB` |
| Icon | See `assets/icon-*.png` |

## Public pages required for submission

Microsoft requires the following live URLs. Update this table before submitting.

| Page | URL | Status |
|---|---|---|
| Privacy policy | https://softaidev.pages.dev/privacy-policy | Exists |
| Terms of use | https://softaidev.pages.dev/terms-of-service | Exists |
| Support | https://softaidev.pages.dev/field-suite-cloud-support | Exists |
| Documentation | https://softaidev.pages.dev/field-suite-cloud-docs | Exists |

## Triggers — future release

Power Automate triggers such as "When a work order is created" require either a polling endpoint with `since` semantics or a webhook subscription flow. Those are **not** included in the v1 connector.

Recommended v2 work:

1. Add `GET /api/jobs?since=<timestamp>` to the PantherHive backend.
2. Add `GET /api/clients?since=<timestamp>`.
3. Add `GET /api/invoices?since=<timestamp>`.
4. Add webhook subscription endpoints (`POST /webhook/subscriptions`, `DELETE /webhook/subscriptions/{id}`) if you prefer push triggers.
5. Update `openapi.yaml` with the new trigger operations and re-submit the connector.

## How to import for internal testing

1. Go to https://make.powerautomate.com.
2. Select **Data** > **Custom connectors** > **New custom connector** > **Import OpenAPI file**.
3. Upload `openapi.yaml`.
4. Set the **Host** to `pantherhive-api.hotwodi4.workers.dev`.
5. On the **Security** tab, confirm **Authentication type** is set to **API key** or **OAuth 2.0** as appropriate. For this v1 connector, use the bearer token scheme defined in the OpenAPI file.
6. Test the actions in a new flow.

## Next step for certification

Follow `SUBMISSION.md` to submit as an Independent Publisher connector.
