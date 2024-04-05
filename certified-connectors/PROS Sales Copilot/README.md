# PROS AI for Sales Copilot Connector

Unlock the full potential of your sales team with the seamless integration of PROS AI and Microsoft Copilot for Sales. Our innovative connector is designed to streamline your sales processes and supercharge your decision-making capabilities, giving you a competitive edge in today's fast-paced business landscape.

## Publisher: PROS

## Prerequisites

You will need the following to proceed:

- A PROS Smart Configure Price Quote (CPQ) for Dynamics 365 license
- A PROS AI for Sales Copilot license
- You will need to work with your PROS contact to set up your PROS AI for Sales Copilot instance with an OAuth 2.0 Client
- A Microsoft Power Apps or Power Automate license
- A Dynamics 365 Customer Engagement license
- The Power Platform CLI tools

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.

## Obtaining Credentials

An OAuth 2.0 Client can be obtained via [PROS Support](https://pros.com/customer-experience/customer-support/).

### Connector Security setup

Use the following values for the the connector security page:

- `Authentication type` : OAuth 2.0
- `Identity Provider` : Generic OAuth 2

  The following are provided through your PROS contact:

  - `Client ID`
  - `Client Secret`
  - `Authorization URL`
  - `Token URL`
  - `Refresh URL`
  - `Scope`

- `Redirect URL` : Provide to your PROS contact after you have completed the Security setup page.

## Supported Operations

The connector supports the following operations:

### `GET testconnection`

Test the connection.

### `GET getrelatedrecords`

Get records related to CRM records from non-CRM applications.

## Getting Started

Contact [PROS Support](https://pros.com/customer-experience/customer-support/) for more information.

## Known Issues and Limitations

A user must have a PROS Smart Configure Price tenant in order to use this connector

## Frequently Asked Questions

### What is PROS Smart Configure Price Quote (CPQ) for Dynamics 365?

PROS Smart CPQ for Dynamics 365 is a cloud-based, enterprise-class CPQ solution that enables sales teams to quickly and accurately create quotes for all types of products and services. It is a native Dynamics 365 application that provides a seamless user experience and leverages Dynamics 365 data and security.

### What is PROS AI for Sales Copilot?

Unlock the full potential of your sales team with the seamless integration of PROS AI and Microsoft Copilot for Sales. Our innovative connector is designed to streamline your sales processes and supercharge your decision-making capabilities, giving you a competitive edge in today's fast-paced business landscape.

### What is the PROS AI for Sales Copilot Connector?

The PROS AI for Sales Copilot Connector is a custom connector that enables users to integrate PROS AI with Microsoft Power Automate and Power Apps.

### What's the authentication type?

The PROS AI for Sales Copilot Connector uses OAuth 2.0 authentication.

### How do I get an OAuth 2.0 Client?

Contact [PROS Support](https://pros.com/customer-experience/customer-support/) for more information.

### How do I get a PROS AI for Sales Copilot license?

Contact [PROS Support](https://pros.com/customer-experience/customer-support/) for more information.

### How do I get a PROS Smart CPQ for Dynamics 365 license?

Contact [PROS Support](https://pros.com/customer-experience/customer-support/) for more information.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Apps.

## Support

For further support, please contact ConnectSupport@pros.com.
