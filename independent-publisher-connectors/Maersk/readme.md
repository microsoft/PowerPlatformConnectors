# Maersk API (Independent Publisher)

The Maersk API provides access to real-time shipping and logistics data including vessels, ports, carrier locations, schedules, and deadlines. This connector allows you to build solutions that use Maersk’s reference data and scheduling endpoints to visualize shipping activity, track vessel schedules, and analyze global trade routes.

## Publisher: Dan Romano, IDR Consultants

> This connector is published and maintained by an independent publisher and is not affiliated with Maersk.

## Prerequisites

You will need a valid [Maersk Developer API key](https://developer.maersk.com/) (Consumer-Key) to use this connector. Once signed in, generate a key from your developer dashboard.

## Obtaining Credentials

1. Visit [Maersk Developer Portal](https://developer.maersk.com/)
2. Register or sign in to your account.
3. Subscribe to the APIs you plan to use.
4. Copy your `Consumer-Key` and use it when creating a connection in Power Platform.

## Supported Operations

The connector supports the following operations:

### Reference Data

- **Get Locations**: Retrieve a list of cities, countries, and port codes used across the Maersk network.
- **Get Carrier Location by ID**: Retrieve detailed metadata for a specific location using `carrierGeoID`.
- **Get Vessels**: Returns reference data about vessels, including IMO numbers, names, and flags.

### Schedules

- **Get Active Ports**: Get a list of active ports by providing carrier codes and optional filters like `UNLocationCode` or `cityName`.
- **Get Schedules by Port**: Returns vessel call schedules for a port using optional parameters like `UNLocationCode`, `cityName`, or `carrierGeoID`.
- **Get Schedules by Vessel**: Returns vessel schedules by providing either `vesselIMONumber` or `carrierVesselCode`.

### Deadlines

- **Get Shipment Deadlines**: Retrieve commercial deadlines per terminal for a specific vessel voyage, based on ISO country code, port of load, and voyage information.

## Known Issues and Limitations

- The API requires a `Consumer-Key` to be passed in the request header.
- `collectionFormat: multi` is not supported in Swagger 2.0 and has been adjusted to support comma-separated strings.
- The `/deadlines` endpoint does not return results unless valid vessel and port combinations are used.
- At least one of `vesselIMONumber` or `carrierVesselCode` is required for `/vessel-schedules`.
- For `/port-calls`, at least one of `UNLocationCode`, `cityName`, or `carrierGeoID` is required.









