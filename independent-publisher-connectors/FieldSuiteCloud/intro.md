# Field Suite Cloud

## Proposal

Field Suite Cloud is a field-service management platform for HVAC, plumbing, electrical, appliance repair, and other trade businesses. It manages customers, work orders, technicians/crew, invoices, and estimates through a Flutter app and a Cloudflare-hosted REST API (PantherHive).

This Independent Publisher connector exposes Field Suite Cloud's REST API to Microsoft Power Automate, Power Apps, and Azure Logic Apps so users can automate field-service workflows alongside Microsoft 365, Dynamics, and other connectors.

## Publisher

SoftAIDev

## Contact

- Email: support@softaidev.com
- Support: https://softaidev.pages.dev/field-suite-cloud-support
- Documentation: https://softaidev.pages.dev/field-suite-cloud-docs

## Authentication

Bearer JWT token obtained from `POST /api/auth/login` on the Field Suite Cloud API. The token is passed in the `Authorization` header as `Bearer <token>`.

## Planned actions (15)

- ListCustomers
- CreateCustomer
- GetCustomer
- UpdateCustomer
- ListWorkOrders
- CreateWorkOrder
- GetWorkOrder
- UpdateWorkOrder
- UpdateWorkOrderStatus
- ListInvoices
- CreateInvoice
- GetInvoice
- MarkInvoicePaid
- ListTechnicians
- CreateTechnician

## Triggers

No real-time triggers in v1. The backend does not yet expose webhooks or reliable incremental polling. Triggers may be added in a future revision.

## API host

`https://pantherhive-api.hotwodi4.workers.dev`

## Status

Connector is built, tested against a live tenant, and ready for Microsoft validation. Looking for collaborators or feedback before submitting full artifacts.
