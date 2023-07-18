# BKK Futár
Get and search planned and real-time information from the BKK FUTÁR system - public transportation of Budapest - regarding journeys, vehicles, stops and many more!

## Publisher
### Fördős András

## Prerequisites
In order to be able to use the connector, you must have a valid API key. You can obtain one at the following link: [https://opendata.bkk.hu/keys](https://opendata.bkk.hu/keys). Registration is completely free.

## Supported Operations
### Search alerts
Search and list detailed information about various alerts and disturbances, based on time or free text search capabilities.

### Get bicycle rental stations
List all bike rental stations and detailes about them, such as location information or currently available number of bikes.

### Get arrivals and departures for a stop
List planned and actual arrival and departure times for given stop, based on additional time or location filters.

### Get schedule for a stop
List schedules for a selected stop, including times, details and directions.

### Get stops for a location
List all nearby stops and their details for a selected location based on coordinates. If no coordinates are given, returns all stops.

### Get vehicles for a stop
List all vehicles and their details, which are on route containing the selected stop.

## Usage and disclaimer

Endpoints are served by a BKK-hosted server park. BKK reserves the right to limit keys for users where they notice a significantly higher load than deemed normal. If you would like to use the service with a higher throughput for business purposes or have special data integration needs please contact their customer service: bkk@bkk.hu

## Limitations

This connector only provides access to a subset of the available endpoints of the BKK Futár service and only makes a subset of input and output parameters available. Reach out and let us connect in case you want to bring in more and make them pat of this connector.