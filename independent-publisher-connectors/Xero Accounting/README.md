# Xero Accounting
The Xero accounting software uses a single unified ledger, which allows users to work in the same set of books regardless of location or operating system. Its features include automatic bank feeds, invoicing, accounts payable, expense claims, fixed asset depreciation, purchase orders, bank reconciliations, and standard business and management reporting.

## Publisher: Hitachi Solutions

## Prerequisites
To use this connector, you need the following

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- A Xero account with either the demo company or a paid subscription tenant
- A Xero developer account with a configured OAuth 2.0 application

## Obtaining Credentials
After signing in at [developer.xero.com](https://developer.xero.com/), navigate to the My Apps section and create a new app. Give it a descriptive name and choose Web App as the integration type. Enter your company URL and for the OAuth 2.0 redirect URIs field, use `https://global.consent.azure-apim.net/redirect`. Agree to the terms and conditions and create the app. Copy the client ID created and then generate and copy the client secret - these will be used when configuring the custom connector. If you are going to build triggers for your Xero tenant, that configuration is on the Webhooks tab in this app. Since Xero allows for triggers based on contacts and invoices but only allows one URL per app, best practice is to create a second app to trigger the second object changes.

## Getting Started
Follow the guide provided by Xero on [developer.xero.com](https://developer.xero.com/documentation/getting-started/getting-started-guide). In order to use Xero webhooks as triggers in Power Automate, follow this [guide](Getting Started with Xero App Webhooks.md).

## API Documentation
[Xero Accounting API](https://developer.xero.com/documentation/api/api-overview)

- [Bank Transfers](https://developer.xero.com/documentation/api/bank-transfers)
- [Contacts](https://developer.xero.com/documentation/api/contacts)
- [Invoices](https://developer.xero.com/documentation/api/invoices)
- [Payments](https://developer.xero.com/documentation/api/payments)
- [Purchase Orders](https://developer.xero.com/documentation/api/purchase-orders)

## Supported Operations
This connector supports the following operations:

### Action: Create a contact
Add a contact in a Xero organisation.

### Action: Get a contact
Retrieve a contact in a Xero organisation.

### Action: Get contacts
Retrieve a list of contacts in a Xero organisation.

### Action: Update a contact
Update a contact in a Xero organisation.

### Action: Get sales invoices
Retrieve sales invoices in a Xero organisation.

### Action: Get a sales invoice
Retrieve a sales invoice in a Xero organisation.

### Action: Create a payment
Apply a payment to approved AR and AP invoices.

### Action: Get payments
Retrieve either a list of payments for invoices and credit notes.

### Action: Get a payment
Retrieve either a payment for invoices and credit notes.

### Action: Create a purchase order
Add a purchase order in a Xero organisation.

### Action: Get a purchase order
Allows you to retrieve a purchase order.

### Action: Get purchase orders
Allows you to retrieve purchase orders.

### Action: Create a bank transfer
Create a bank transfer in a Xero organisation.

### Action: Get a bank transfer
Retrieve a bank transfer in a Xero organisation.

### Action: Get bank transfers
Retrieve a list of bank transfers in a Xero organisation.


## Known Issues and Limitations
There are no known issues at time of publishing.

## Frequently Asked Questions


## Deployment Instructions
Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).
