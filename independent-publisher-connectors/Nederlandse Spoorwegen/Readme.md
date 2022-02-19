# Nederlandse Spoorwegen

Nederlandse Spoorwegen (NS) is the main railway operator within the Netherlands.

## Publisher: Miguel Verweij

## Prerequisites

You need a Nederlandse Spoorwegen account. You can get one for free [here](https://apiportal.ns.nl/signin).

## Obtaining Credentials

For authorization an API key is required. How to get this is described in NS's developer [Starter's Guide](https://apiportal.ns.nl/startersguide).
The product to choose for this connection is [Ns-App](https://apiportal.ns.nl/products/NsApp).

## Supported Operations

The connector supports the following operations:
- `Departures`: List of departures for a specific station
- `Arrivals`: List of arrivals for a specific station
- `Journey details`: Returns information about a specific journey
- `Single trip`: Returns a single trip, based on the given parameters
- `Calamities`: List of calamities. This operation is deprecated. Use /api/v3/disruptions instead
- `International prices`: Returns price information for international trips
- `Domestic prices`: Returns price information for domestic trips
- `Stations`: List of stations
- `Trips`: Returns a travel advice for the given parameters
- `Station disruptions`: List of disruptions relevant for the current station
- `Single disruption`: Returns information on a single disruption/maintenance/calamity
- `Disruptions`: List of calamities/disruptions/maintenance

## Known Issues and Limitations

Although the API service is free, the number of calls is limited. NS always has the option to adjust the limit if desired.