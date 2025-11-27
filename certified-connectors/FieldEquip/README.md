# FieldEquip

## Publisher
BPM Geeks, LLC DBA Bursys

## Publisher ID
fieldequip1748394731074

## Overview

FieldEquip enables real-time integration of critical operational data between client systems and the FieldEquip platform, ensuring a seamless integration, improved visibility, and efficient field service management by connecting technicians, support teams, customers, to boost productivity, manage mobile worker time reporting, track rental equipment, reduce equipment downtime, cut costs, and enhance customer satisfaction.

## Prerequistes

To successfully run the connector, you would need following information to proceed.

1. API Key for Authentication, can be requested from FieldEquip team
2. URL for the origin, where the requests would be sent, can be requested from FieldEquip team
3. Body to be passed as an advanced parameter, it supports single objects or can be passed together as an array.

## Authentication

This connector uses API Key authentication. The API key must be included in the headers of each request. This key is issued by the FieldEquip support team.


## Supported Operations

1. Create Customers: Inserts field service management related information into the FieldEquip platform for customer records.
2. Update Customers: Updates field service management related information into the FieldEquip platform for customer records.
3. Create Work Orders: Inserts field service management related information into the FieldEquip platform for work order records.
4. Update Work Orders: Updates field service management related information into the FieldEquip platform for work order records.
5. Create Items: Inserts field service management related information into the FieldEquip platform for item records.
6. Update Items: Updates field service management related information into the FieldEquip platform for item records.
7. Create Inventory: Inserts field service management related information into the FieldEquip platform for inventory records.
8. Create Item Adjustment: Inserts field service management related information into the FieldEquip platform for inventory record adjustments for an item.
9. Create Users: Inserts field service management related information into the FieldEquip platform for user records.
10. Update Users: Update field service management related information into the FieldEquip platform for user records.

## Deployment Instructions

1. Import the connector via Power Automate's Custom Connector interface.
2. Provide the API key, base URL, which can be requested from the FieldEquip team.
3. Test connectivity using a sample operation such as "Create Customers".
