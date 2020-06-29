## dotdigital Engagement Cloud Connector

The dotdigital Engagement Cloud platform is an omnichannel marketing solution for B2C, B2B, and NFP marketers. It allows you to import data, build segments and triggers, and create relevant marketing campaigns that engage your customers on their favorite channels.

The connector uses our powerful and easy to use REST API, allowing you to send email campaigns, SMS messages and enroll contacts to your Engagement Cloud automation programs etc.

## Pre-requisites

You need the following to proceed:

- A Microsoft Power Apps, Azure Logic Apps or Power Automate plan with custom connector feature

- [Microsoft Power Platform Connectors CLI](https://github.com/microsoft/PowerPlatformConnectors/tree/master/tools/paconn-cli)

- API authentication details for your dotdigital Engagement Cloud account

## Setup

### Get your API account username and password

Please follow the steps as below:

1. Sign in to your Engagement Cloud account.

2. Go to Access > API users.

3. Create a new API user and ensure that the user status is set to 'Enabled'.

4. Make a note of the username and password for creating a connection for the connector.

## Supported Operations

| Operation Name | Description |
| ----------------------------- | -------------------------------------------------------------------------------------------- |
| **Create address book** | Creates an address book in Engagement Cloud |
| **Create contact** | Add new or existing contacts to an address book in Engagement Cloud |
| **Send email campaign** | Send an email campaign |
| **Send transaction email using a triggered campaign** | Send a transaction email using a triggered campaign |
| **Program enrolment** | Enroll contacts to an automation program |
| **Send SMS message** | Sends a SMS message |

## Support and documentation

For all general queries or if you need any assistance, please contact support@dotdigital.com or visit [Contact us](https://dotdigital.com/contact-us/)