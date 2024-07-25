
### NOTE
> This is a *sample* connector.  The connector is provided here with the intent to illustrate requirements to the connectors created to link your ERP system with Copilot for Finance application.

# Title
Copilot for Finance provides the ability for customers to integrate the Microsoft Outlook-addin with any backend systems that supports OAuth by using the integration of Power Platform Custom Connectors. Copilot for Finance connector should provide a REST API of your ERP system.  Using this API, Copilot for Finance Outlook-addin will be able to read and use data from your ERP system.

## Publisher
Microsoft Corporation ​

## Prerequisites
You will need the following to proceed:
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- Rest API service implemented with Copilot for finance API endpoints

## Supported Operations
The connector supports the following operations.
### Contacts
Provides list of ERP contacts based on the emails or phone numbers.

### Create contact
Provides an opportunity to create a new contact in the ERP.

### Update contact
Provides an opportunity to update the contact in the ERP.

### Customers
Provides a list of customers.

### Customer's account statement
Provides a customer account statement in pdf.

### Customer's activities
Provides a list of customer activities.

### Customer's aged balances
Provides an information about customer's aged balance reports.

### Customer's invoice documents
Provides customer's invoice document/s in pdf/zip format.

### Customer invoices
Provides list of outstanding invoices for the customer.

### List of invoice statuses
Provides a list of possible invoice statuses for the backend ERP.

### Update invoice status
Provides an opportunity to status and promised to pay date for an invoice.

### Supported version
Returns the latest supported version of the API.

## Obtaining Credentials
Custom connector needs to use OAuth2.0 authentication. More details could be found in the [documentation](https://learn.microsoft.com/en-us/copilot/finance/get-started/custom%20connectors/define-openapi-definition#review-authentication-type)

## Known Issues and Limitations
None.

## Deployment Instructions
Follow those [instructions](https://learn.microsoft.com/en-us/copilot/finance/get-started/custom%20connectors/define-openapi-definition).

## Documentation
More details could be found in the [documentation](https://learn.microsoft.com/en-us/copilot/finance/get-started/custom%20connectors/overview-custom-connectors).
