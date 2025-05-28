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
3. Company ID, for inserting or retrieving information from the relevant company, can be requested from FieldEquip team

## Authentication

This connector uses API Key authentication. The API key must be included in the headers of each request. This key is issued by the FieldEquip support team.


## Supported Operations

1. Create Records: Updates or inserts field service management related information into the FieldEquip platform, currently supporting only customer records.
2. Get Records: To retrieve field service management related information into the FieldEquip platform, currently supporting only customer records.

## Deployment Instructions

1. Import the connector via Power Automate's Custom Connector interface.
2. Provide the API key, base URL, and Company ID.
3. Test connectivity using a sample operation such as "Get Records".
