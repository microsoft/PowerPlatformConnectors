# Nederlandse Spoorwegen
Nederlandse Spoorwegen (NS) is the main railway operator within the Netherlands. 

## Publisher: Miguel Verweij

## Prerequisites
You need a Nederlandse Spoorwegen account. You can get one for free [here](https://apiportal.ns.nl/signin).

## Obtaining Credentials
For authorization an API key is required. How to get this is described in NS's developer [Starter's Guide](https://apiportal.ns.nl/startersguide).
The product to choose for this connection is [Ns-App](https://apiportal.ns.nl/products/NsApp).

## Supported Operations

### Arrivals
List of arrivals for a specific station. [More info](https://apiportal.ns.nl/docs/services/reisinformatie-api/operations/getArrivals?)

### Departures
List of departures for a specific station. [More info](https://apiportal.ns.nl/docs/services/reisinformatie-api/operations/getDepartures?)

### Stations
List of stations. [More info](https://apiportal.ns.nl/docs/services/reisinformatie-api/operations/getStations?)

### Station Disruptions
List of disruptions relevant for the current station. [More info](https://apiportal.ns.nl/docs/services/reisinformatie-api/operations/getStationDisruptions_v3?)

## Deployment Instructions
Upload the connector and authorize by providing your API key. 

## Known Issues and Limitations
Although the API service is free, the number of calls is limited. NS always has the option to adjust the limit if desired.
