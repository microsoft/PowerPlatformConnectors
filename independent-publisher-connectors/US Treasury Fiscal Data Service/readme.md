# U.S. Treasury Fiscal Data Service Connector

The **U.S. Treasury Fiscal Data Service** provides comprehensive and detailed financial data on the U.S. government's fiscal operations. This connector enables seamless integration with various datasets, allowing users to access up-to-date information on topics such as public debt, exchange rates, financial reports, interest expenses, and more.

## Publisher: [Your Name]

## Prerequisites

To use this connector, you will need:

- A **Microsoft Power Apps** or **Power Automate** plan that includes custom connector capabilities.
- No authentication is required. The API is publicly available.

## Supported Operations

This connector supports the following operations:

### Get Debt to the Penny
Retrieve the total **public debt outstanding**, including **debt held by the public** and **intragovernmental holdings**.

### Get Treasury Reporting Rates of Exchange
Access the **exchange rates of foreign currencies** as reported by the U.S. Treasury.

### Get Monthly Statement of the Public Debt (MSPD)
Obtain detailed information on the **composition of the public debt**, categorized by security type and holder.

### Get Financial Report of the U.S. Government
Fetch the **statements of net cost**, providing insights into the **government's financial position and operations**.

### Get Interest Expense on the Public Debt Outstanding
Retrieve data on the **interest expenses incurred on the public debt**, broken down by various categories.

### Get U.S. Treasury-Owned Gold
Access information on the **U.S. Treasury's gold reserves**, including **quantities and book values**.

### Get Record-Setting Treasury Auctions
Obtain data on **record-setting Treasury auctions**, detailing **security types, terms, and associated metrics**.

## Obtaining Credentials

No authentication is required to use this connector. The API is publicly accessible.

## Getting Started

1. **Import the U.S. Treasury Fiscal Data Service connector** into your Power Apps or Power Automate environment.
2. **Create a new connection** (no credentials needed).
3. **Utilize the available operations** to integrate **U.S. Treasury fiscal data** into your applications or workflows.

## Known Issues and Limitations

- **Data Latency**: Some datasets may have a **publication delay** due to processing times. Always refer to the `record_date` field to ensure data currency.
- **Rate Limits**: The API enforces **rate limits** to prevent abuse. Refer to the [API documentation](https://fiscaldata.treasury.gov/api-documentation) for details on **request limits**.
- **Data Availability**: Certain datasets may not be **updated in real-time**. Users should verify the `record_date` and `effective_date` fields to ensure they are working with the most recent data.

## Frequently Asked Questions (FAQ)

### How often is the data updated?
Update frequencies vary by dataset. Some are updated **daily**, while others are **monthly or quarterly**. Refer to the [dataset page](https://fiscaldata.treasury.gov/datasets/) for specific update schedules.

### What should I do if I encounter issues accessing data?
Ensure that the **API endpoint is correct** and that you are **formatting requests properly**. If issues persist, check the [Treasury Fiscal Data Service API status](https://fiscaldata.treasury.gov/status/) or contact their support team.

### Is there a data model available for the datasets?
Yes, and there is also a Fiscal Data Registry that provides **metadata** and **data dictionaries** detailing government-wide financial data elements - specifically those data elements commonly used across multiple agencies.. You can access it [here](https://fiscal.treasury.gov/data-registry/).

## Deployment Instructions

To deploy this connector as a **custom connector** within your **Power Platform** environment: import the connector file, configure the operations, and create a connection. No API key is required.
