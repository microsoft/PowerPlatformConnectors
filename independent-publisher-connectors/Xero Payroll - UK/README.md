# Xero Payroll - UK
The Xero accounting software uses a single unified ledger, which allows users to work in the same set of books regardless of location or operating system. Its features include automatic bank feeds, invoicing, accounts payable, expense claims, fixed asset depreciation, purchase orders, bank reconciliations, and standard business and management reporting. The Payroll API exposes payroll related functions of the payroll Xero application and can be used for a variety of purposes such as syncing employee details, importing timesheets etc.

## Publisher: Hitachi Solutions

## Prerequisites
To use this connector, you need the following

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- A Xero account with either the demo company or a paid subscription tenant
- A Xero developer account with a configured OAuth 2.0 application

- UK Xero organisations (including the sample Demo company) must have concluded the [payroll set up steps](https://central.xero.com/s/topic/0TO1N0000017kmIWAQ/payroll-employees#business) before the Payroll API can be used.

## Obtaining Credentials
After signing in at [developer.xero.com](https://developer.xero.com/), navigate to the My Apps section and create a new app. Give it a descriptive name and choose Web App as the integration type. Enter your company URL and for the OAuth 2.0 redirect URIs field, use `https://global.consent.azure-apim.net/redirect`. Agree to the terms and conditions and create the app. Copy the client ID created and then generate and copy the client secret - these will be used when configuring the custom connector.

## Getting Started
Follow the guide provided by Xero on [developer.xero.com](https://developer.xero.com/documentation/getting-started/getting-started-guide).

## API Documentation
[Xero Payroll API - UK](https://developer.xero.com/documentation/payroll-api-uk/overview)

- [Employees](https://developer.xero.com/documentation/payroll-api-uk/employees)

- [Reimbursements](https://developer.xero.com/documentation/payroll-api-uk/reimbursements)

- [Timesheets](https://developer.xero.com/documentation/payroll-api-uk/timesheets)

## Supported Operations
This connector supports the following operations:

### Action: Retrieve a employee
Allows you to retrieve details of a payroll employee in a Xero organisation.
### Action: Add a employee
Allows you to add a payroll employee in a Xero organisation.
### Action: Update a employee
Allows you to update a payroll employee in a Xero organisation.
### Action: Retrieve employees
Allows you to retrieve payroll employees in a Xero organisation.
### Action: Retrieve timesheets
Allows you to retrieve payroll timesheets in a Xero organisation
### Action: Retrieve a timesheet
Allows you to retrieve a payroll timesheets in a Xero organisation
### Action: Delete a timesheet
Allows you to delete a payroll Timesheet in a Xero organisation
### Action: Approve a timesheet
Allows you to add a payroll timesheet in a Xero organisation
### Action: Add a timesheet lines to payroll timesheet
Allows you to add timesheet Lines to payroll timesheets in a Xero organisation
### Action: Delete a timesheet line
Allows you to delete a timesheet line of a payroll timesheet in a Xero organisation
### Action: Update a timesheet line
Allows you to update a timesheet line of a payroll timesheet in a Xero organisation
### Action: Retrieve reimbursements
Allows you to retrieve all reimbursements in a Xero organisation
### Action: Add a reimbursement
Allows you to add a reimbursement in a Xero organisation
### Action: Retrieve a payroll reimbursement
Allows you to retrieve details of a reimbursement in a Xero organisation

## Known Issues and Limitations
There are no known issues at time of publishing.

## Deployment Instructions
Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).
