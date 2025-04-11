# Proposal - Finance Agents SAP BTP Connector

The Finance Agents SAP BTP Connector enables integration between the Microsoft Copilot for Finance Outlook Add-in and any SAP ERP system, using the SAP Business Technology Platform (BTP) as a middleware layer. The connector exposes a REST API on BTP, which in turn communicates with the underlying ERP system via its native APIs. This setup allows the Copilot for Finance add-in to securely access and interact with ERP data through a Power Platform custom connector, using OAuth 2.0 authentication.

## Prerequisites
You will need the following to proceed:
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- The Finance Agents SAP BTP Connector MTAR package to deploy on SAP BTP
- An SAP ERP system: S/4HANA (On-premise / Private / Public Cloud) or SAP ECC 6.0

## Supported Operations
The connector supports the following operations.

### Contacts
- List contacts: Retrieves ERP contacts based on email or phone number
- Create contact: Creates a new contact in the ERP system
- Update contact: Updates an existing contact in the ERP system

### Customers
- List customers: Retrieves a list of customers from the ERP system
- Account statement: Provides a customer's account statement in PDF format
- Activities: Retrieves a list of customer-related activities
- Aged balances: Returns aging balance reports for a customer
- Invoice documents: Downloads invoice documents in PDF or ZIP format
- Outstanding invoices: Lists outstanding invoices for a customer

### Invoices
- Invoice statuses: Returns possible statuses for customer invoices
- Update invoice status: Allows updating the status and promise-to-pay date of an invoiceProvides a list of customers.

## Obtaining Credentials
Custom connector needs to use OAuth2.0 authentication.

## Known Issues and Limitations
None.
