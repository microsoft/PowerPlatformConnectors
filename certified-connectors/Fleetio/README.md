# Fleetio

Fleetio is a Fleet maintenance software for fleets of all sizes

This connector allows you to extract data from the [Fleetio API](https://developer.fleetio.com/)

## Prerequisites

- An active Fleetio Account
- An API Key and Account Token

## Supported Operations

This connector supports the following endpoints from the Fleetio API

- `v2/issues`
- `v2/service_entries`
- `v1/submitted_inspection_forms`
- `v1/fuel_entries`
- `v1/vehicles`
- `v1/expense_entries`
- `v2/contacts`
- `v1/parts`
- `v1/purchase_orders`
- `v1/vehicle_assignments`

## Obtaining Credentials

We provide a guide on our developer portal, that you can find [here](https://developer.fleetio.com/docs/overview/quick-start)

## Known Issues and Limitations

Depending on how much data you have in your Fleetio Account, some endpoints may take longer than others to sync as this connector support full refresh only

## Deployment Instructions

Please use [these instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector