# Fortnox

Fortnox offers a cloud-based platform that enables businesses and accounting firms to manage their finances and administration efficiently. The platform is a market leader in Sweden. It also provides customers with access to financial services and business insurance.

## Publisher: Mathias Törnblom

## Prerequisites

- Active Fortnox license
- Fortnox Developer Account
- API client credentials (Client ID, Client Secret)
- A company in Fortnox with API access enabled

## Supported Operations

### Accounts
- **Get Account**: Retrieve details of a specific account by account number.

### Customers
- **List Customers**: Retrieve a paginated list of customers. Supports filtering (e.g., active/inactive).
- **Get Customer**: Get details of a customer by customer number.
- **Update Customer**: Update specific fields for a customer.
- **Delete Customer**: Permanently delete a customer by customer number.

### Orders
- **Create Order**: Create a new order with support for multiple rows and delivery settings.

### Invoices
- **Create Invoice**: Generate a new invoice.
- **List Invoices**: Get a list of existing invoices.
- **Preview Invoice**: Fetch a PDF preview of an invoice.
- **Cancel Invoice**: Cancel an invoice using its document number.

## Obtaining Credentials

Fortnox uses **OAuth 2.0** for authentication.

1. Go to [developer.fortnox.se](https://developer.fortnox.se).
2. Register your integration to get a **Client ID** and **Client Secret**.
3. Use the authorization code flow to obtain an **access token**.
4. Ensure the user has granted access to the application during authentication.

Tokens must be included in the `Authorization: Bearer <token>` header.

## Getting Started

1. Export the Swagger file provided for this connector.
2. In **Power Apps** or **Power Automate**, go to **Data > Custom Connectors**.
3. Select **+ New custom connector > Import an OpenAPI file**.
4. Upload the Swagger (OpenAPI) file and follow the wizard.
5. Configure OAuth 2.0 settings under the **Security** tab.
6. Test the connection using your Fortnox credentials and start building flows or apps.

## Known Issues and Limitations

- Pagination is required for large datasets (e.g., customers, invoices).
- Rate limits apply based on your Fortnox subscription plan.
- Some advanced fields are only visible when editing the connector in expert mode.
- You must handle token refresh manually unless using a premium connector setup.

## Frequently Asked Questions

### How do I test the connector?

Use Power Automate’s **Test** tab inside the custom connector to run operations like "List Customers" or "Get Account".

### Can I use this connector in Power BI?

Not directly. Use Power Automate to fetch data and push it to a dataset in Power BI.

## Deployment Instructions

1. Open **Power Apps** or **Power Automate**.
2. Go to **Custom Connectors > New Custom Connector**.
3. Select **Import an OpenAPI file**, upload the Fortnox Swagger file.
4. Set up OAuth 2.0 with your Client ID and Secret.
5. Define the **authorization URL**, **token URL**, and **refresh URL** from Fortnox.
6. Save and test the connector.
7. Add it to your flows or apps like any built-in connector.
