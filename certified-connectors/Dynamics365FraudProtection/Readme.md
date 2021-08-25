# Title

Microsoft Dynamics 365 Fraud Protection provides merchants the capability to obtain risk assessment on fraudulent activity associated with e-commerce purchases , fraudulent account sign ups and sign in attempts in their online store.

## Prerequisites

To use this connector, you need to have Dynamics 365 Fraud Protection provisioned. For instructions on how to do this, see [Dynamics 365 Fraud Procection](https://docs.microsoft.com/dynamics365/fraud-protection/).

## Supported operations

The connector supports the following operations:

- `Account Creation`: Sends information and context about an incoming new Account Creation attempt, and gets a response that contains potential risk level and rule decisions for Account Creation.
- `Account Creation Status`: Sends an update of the status of Account Creation, for example, if Account Creation has been canceled. This is a data ingestion event only.
- `Account Login`: Sends information and context about an incoming new Account Login attempt and gets a response that contains potential risk level and rule decisions for the Account Login.
- `Account Login Status`: Sends an update of the status of Account Login, for example, if Account Login has been canceled. This is a data ingestion event only.
- `Account Update`: Sends updates or creates user account information, for example, Add Payment Instrument, Add Address, or any other user attribute. This is a data ingestion event only.
- `Purchase`: Sends information and context about an incoming new purchase transaction. The response contains a decision to either approve or reject the purchase transaction based on your rules.
- `Purchase Status`: Sends an update of the status of a purchase, for example, if the purchase has been canceled. This is a data ingestion event only.
- `Bank Event`: Sends information if a purchase transaction sent to the bank was approved or rejected for bank authorization or bank charge/settlement. This is a data ingestion event only.
- `Refund`: Sends information about a previous purchase transaction being refunded. This is a data ingestion event only. Most merchants send these events using bulk data upload.
- `Chargeback`: Sends information about a previous purchase that the customer disputed with their bank as fraud. This is a data ingestion event only. Most merchants send these events using bulk data upload.
- `Purchase Update Account`: Sends updates or creates user account information, for example, Add Payment Instrument, Add Address, or any other user attribute. This is a data ingestion event only.
- `Label`: Sends an update of the label. This is a data ingestion event only.
- `Custom Assessment`: Sends a structure of your own choosing, triggered by conditions of your own choosing for rule evaluation and gets a response that contains a decision for the event.

## Obtaining credentials

This connector supports both user account and service principal authentication. Typically, this connector is called from Power Automate, so you will need to configure authentication. For more information, see [Dynamics 365 Fraud Protection Samples](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/tree/master/power%20apps%20connector).

## Host URL

This connector requires host URL. To find the host URL, follow these steps:

1. Go to [https://dfp.microsoft.com](https://dfp.microsoft.com).
2. Select **Integration (preview)**.
3. Set the host URL with an API endpoint value.

For more information, see [Dynamics 365 Fraud Protection Samples](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/tree/master/power%20apps%20connector).

## Getting started

For more information about how to get started, see [Dynamics 365 Fraud Protection Samples](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/tree/master/power%20apps%20connector).
