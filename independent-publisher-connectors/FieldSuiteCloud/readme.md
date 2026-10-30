# Field Suite Cloud
Field Suite Cloud is a field-service management platform for HVAC, plumbing, electrical, appliance repair, and other trade businesses. It manages customers, work orders, technicians/crew, and invoices through a mobile app backed by a cloud-hosted REST API. This connector exposes the Field Suite Cloud API so you can automate field-service workflows in Power Automate, Power Apps, and Azure Logic Apps.

## Publisher: SoftAIDev

## Prerequisites
- A Field Suite Cloud account with an active tenant.
- The email and password for that account, used to obtain a bearer token (see Obtaining Credentials).

## Supported Operations

### List Customers
Returns a list of customers for the authenticated tenant. Supports `page` and `limit` (max 100) query parameters.

### Create Customer
Creates a new customer record. Requires `contactName`; accepts company name, email, phone, address, city, state, zip, and notes.

### Get Customer
Returns a single customer by numeric ID.

### Update Customer
Updates an existing customer record by ID.

### List Work Orders
Returns a list of work orders for the authenticated tenant. Supports an optional `status` filter (0 = open, 1 = in progress, 2 = completed).

### Create Work Order
Creates a new work order. Requires `jobNumber` and `title`; accepts description, client ID, address, priority, and scheduled start/end.

### Get Work Order
Returns a single work order by numeric ID.

### Update Work Order
Updates an existing work order, including assignment, schedule, priority, status, and actual start/end times.

### Update Work Order Status
Changes the status of a work order (0 = open, 1 = in progress, 2 = completed). Can optionally auto-generate an invoice via `auto_invoice`.

### List Invoices
Returns a list of invoices for the authenticated tenant.

### Create Invoice
Creates a new invoice. Requires `invoiceNumber` and `clientId`; accepts job ID, amount, tax amount, due date, and notes.

### Get Invoice
Returns a single invoice by numeric ID.

### Mark Invoice Paid
Marks an invoice as paid by ID.

### List Technicians
Returns a list of technicians / crew members for the authenticated tenant.

### Create Technician
Creates a new technician record. Requires `name`; accepts email, phone, role, skills, and hourly rate.

## Supported Triggers
None in v1. The backend does not yet expose webhooks or reliable incremental polling.

## Obtaining Credentials
The connector authenticates with a bearer token (JWT).

1. Obtain a token by calling `POST https://pantherhive-api.hotwodi4.workers.dev/api/auth/login` with your Field Suite Cloud account email and password in the JSON body (`{"email": "...", "password": "..."}`). The response contains the JWT.
2. When creating the connection in Power Automate, enter the token in the Authorization field in the form `Bearer <token>`.

## Known Issues and Limitations
- No triggers are available in v1.
- The bearer token expires with the underlying Field Suite Cloud session; when it expires, update the connection with a fresh token.
- `limit` on List Customers is capped at 100 records per request.
- No triggers are available in v1.
- The bearer token expires with the underlying Field Suite Cloud session; when it expires, update the connection with a fresh token.
- `limit` on List Customers is capped at 100 records per request.

## Deployment Instructions
Please use [these instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
