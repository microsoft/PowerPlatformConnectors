# 3e powered by EXPOCAD Connector
This connector provides Power Automate and Logic Apps access to live EXPOCAD 3e exposition data. It supports webhook-based triggers for floorplan and exhibitor activity, plus actions for reading and managing booths, exhibitors, event metadata, financial data, booth classes, rate plans, pavilions, and show-in-show records.

## Publisher: A.C.T. / EXPOCAD

## Prerequisites
Requires that your event(s) be set up in 3e and accessible by an api user.  You will be prompted for an event id when adding a trigger.

## Supported Triggers

Webhook triggers subscribe Power Automate to EXPOCAD 3e events. Each trigger requires a client identifier and, when applicable, an event identifier. The callback URL is supplied automatically by the Power Platform.

- When a booth is rented
- When a booth is unrented
- When a booth is held
- When a booth is unheld
- When a booth is deleted
- When a booth is undeleted
- When a booth number is changed
- When a booth is created
- When a booth is moved
- When a booth is combined
- When a booth is uncombined
- When an exhibitor is added
- When an exhibitor is updated
- When an exhibitor is deleted
- When an exhibitor contract is generated
- When an exhibitor pending contract is submitted
- When an exhibitor contract is signed
- When an exhibitor contract is voided
- When an exhibitor order is submitted
- When an exhibitor payment is attempted
- When an exhibitor deposit is paid
- When an exhibitor document is submitted
- When an exhibitor document is approved
- When an exhibitor document is rejected
- When an exhibitor logo is uploaded
- When exhibitor directory info is added
- When exhibitor directory info is modified
- When exhibitor directory info is deleted
- When an exhibitor email is changed
- When an exhibitor is moved
- When a booth display override is changed
- When a child exhibitor is added
- When a child exhibitor is removed
- When an exhibitor category is added
- When an exhibitor category is deleted
- When an exhibitor ID is changed
- When a booth class is assigned
- When a booth class is removed
- When a booth is resized
- When a show in show is assigned
- When a show in show is cleared

Webhook payload fields vary by trigger and can include fields such as `NotificationType`, `eventIdentifier`, `boothNumber`, `customerId`, `exhibitorName`, `otherBooths`, `oldValue`, `misc`, `timestamp`, and `callId`.

## Supported Actions

### Webhook Management

- Unsubscribe from a webhook by `webhookId`

### Booths

- Get a specific booth
- Get all booths
- Get all available booths
- Get all rented booths
- Rent a booth
- Unrent a booth
- Hold a booth
- Unhold a booth
- Convert a rented booth to a held booth
- Convert a held booth to a rented booth
- Combine booths
- Uncombine a booth
- Delete booths
- Undelete booths
- Change a booth number
- Apply a booth class
- Remove a booth class
- Set a booth display-name override
- Reset a booth display-name override
- Add a child exhibitor to a booth
- Remove a child exhibitor from a booth

### Booth Classes

- Get a booth class
- Get all booth classes
- Add a booth class
- Update a booth class
- Delete a booth class

### Events

- Get all events
- Get event statistics
- Get event information

### Exhibitors

- Get an exhibitor
- Get all exhibitors
- Add an exhibitor
- Update an exhibitor
- Delete an exhibitor
- Get exhibitor contacts
- Get all contacts for an exhibitor
- Add an exhibitor contact
- Update an exhibitor contact
- Delete an exhibitor contact

### Financials

- Get booth financial information
- Get transactions, with optional date and field filters
- Get invoice details
- Get all invoices
- Get requestable item master list
- Get assigned request items
- Get payment type master list
- Get payments

### Pavilions

- Get all pavilions

### Rate Plans

- Get the default rate plan
- Set the default rate plan
- Get all rate plans
- Add a rate plan

### Show-In-Shows

- Get all show-in-shows


## Obtaining Credentials
You may obtain your clientId from EXPOCAD support.
You will need to enter an api user's email as username and apiKey as password when making a connection. You can create an api user in the users and teams section of 3e.

## Authentication

This connector uses basic authentication. Keep authorization codes and credentials private. If credentials are compromised, contact EXPOCAD developer support.

## Known Issues and Limitations
- Triggers are not always received in the order they fired in.  A timestamp property is available on all trigger payloads, allowing for scenarios where the order of operations is significant.
- The connector is designed for event-driven EXPOCAD 3e scenarios and must be paired with valid EXPOCAD client and event identifiers.
- Write operations such as rent, hold, combine, delete, update, and default-rate-plan changes modify live event data and should be used with care.
- Please contact [developer@expocad.com](mailto:developer@expocad.com) for production access.

## Deployment Instructions

1. Import the connector to Power Automate or Logic Apps.
2. Provide your EXPOCAD credentials when prompted.
3. Choose the trigger or action needed for the flow.

## Terms

- [Privacy Policy](https://new.expocad.com/terms/privacy)
- [Terms of Use](https://new.expocad.com/terms/terms)
