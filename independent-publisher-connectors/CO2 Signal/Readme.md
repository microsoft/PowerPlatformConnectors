# CO2 Signal Connector
Get the most recent carbon intensity numbers for any country on the map. CO2 Signal API allows you to get access to information about

* where the electricity in a specific region comes from
* how it was produced
* how much carbon was emitted to produce it

## Pre-requisites
CO2 Signal uses API keys to allow access to the API. You can get an API key [here](https://co2signal.com/).
CO2 Signal expects the API key to be included as a header in all requests to the server: 
auth-token: myapitoken

## Publisher: Paul Culmsee

## Supported Operations
The connector supports the following operations:

### Get available zones
Get all Countries and zones available.

### Get latest by country code
Get the list of network stations.

### Get latest by geographic coordinate
Get a single network station record by its ID.

## Known Issues and Limitations
The API has a rate limit of 30 requests per hour and max 1 request per second. the latter limit means Power Automate users may see 429 errors. 

This API is free for non-commercial use. [Reach out](mailto://hello@electricitymap.org) to Co2 Signal if you plan to commercialise it.

Always update this connector via paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json. 
The custom connector UI will report and error with the "Remote Auth Reader" policy because the UI does not allow an empty value. 
If you save the connector via this UI, it writes an invalid value to the policy which will stop the Get Zones endpoint from working.

One of the endpoints in this connector calls a different API - the Electricity Market API. Electricitymarket owns the CO2 Signal API and recommend this API in their documentation.

## Frequently Asked Questions
This API is free for non-commercial use. Reach out to us if you plan to commercialise it.


