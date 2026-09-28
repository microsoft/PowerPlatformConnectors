# FieldEquip 

## Publisher

BPM Geeks, LLC DBA Bursys

## Overview

The FieldEquip connector provides seamless, real-time integration between client systems and the FieldEquip platform. It enables frictionless data flow across technicians, support teams, customers, and field operations.

By leveraging FieldEquip's no-code automation environment, organizations can:
Streamline field service workflows

- Improve operational visibility
- Reduce equipment downtime
- Lower operational costs
- Enhance customer satisfaction

The platform supports critical workflows such as mobile time reporting, rental equipment tracking, inventory and item management, and automated updates; helping teams operate more efficiently and with greater accuracy.

## Connection Setup

To configure this connector, you will need the following:

1. **API Key**
Used for authentication. This can be requested from the FieldEquip support team.

2. **Origin URL**
The base URL where requests will be sent. Provided by FieldEquip upon onboarding.

3. **Request Body**
Passed as an advanced parameter. Supports both:
    - A single JSON object
    - An array containing multiple objects

Ensure your body structure matches the schema defined for each operation.

## Getting Started with FieldEquip Connector

Below is the list of supported operations currently available in the connector:

### Customer Operations
- **Create Customer**: Inserts customer data into the FieldEquip platform.
- **Update Customer**: Updates existing customer records.

### Work Order Operations
- **Create Work Order**: Creates new work order records.
- **Update Work Order**: Updates existing work orders.

### Item & Inventory Operations
- **Create Item**: Adds new item records.
- **Update Item**: Updates item details.
- **Create Inventory**: Creates inventory records for items.
- **Create Item Adjustment**: Adjusts inventory quantities for specific items.

### User Operations
- **Create User**: Adds user records into FieldEquip.
- **Update User**: Updates existing user information.

### Location Operations
- **Create Location**: Inserts location data into the FieldEquip platform.
- **Update Location**: Updates existing location details.

## Troubleshooting
- **401 Unauthorized**: Check that the API key or origin URL is valid and associated with the correct tenant/environment.
- **400 Bad Request**: Ensure your request body matches the required schema.
- **Timeout errors**: Reduce payload size or break large updates into smaller batches.
