# Mailform
The Mailform Connector for Power Automate is designed to make it easy to print and mail documents, letters and invoices, directly from your Power Apps or Power Automate flows.

## Publisher: Mailform, Inc.

Published by Mailform, the easiest way to print and mail documents, letters and invoices online. Send FedEx Standard Overnight, USPS Priority, Express, Certified & First Class Mail letters and postcards.
  
## Prerequisites

You will need the following to proceed:
* A Mailform account with a valid, non-zero credit account (to pay for your print and mail jobs). Sign up for free at [Mailform](https://www.mailform.io)

## Supported Operations

### Print and mail a letter or postcard

This action takes a PDF (for a letter) or a PDF, PNG or JPEG (for a postcard) as well as the address information and delivery service and then creates an order that prints and mails the document according to the parameters passed.


### Retrieve order details

This action takes an order identifier and returns the order details, including any tracking number associated with the order.

### Retrieve user details

This action retrieves the user's name, email address, return addresses and credit balance.

## Obtaining Credentials

Users need to authenticate using a Mailform API key obtained from their Mailform account.

To generate an API key, first sign into your [Mailform account](https://www.mailform.io), and then visit your [Mailform API Access](https://www.mailform.io/webapp/account.html#/api) page. Click the ***New API Token*** button to create a new API token. Save the token value: it won't be shown again.

## Known Issues and Limitations

None

## Deployment Instructions

To deploy this connector:

- Access your Power Automate environment.
- Navigate to 'Custom Connectors'.
- Import the provided connector definition.
- Enter the required authentication details.
- Test the connector to ensure functionality.