# Electricity Maps Connector

You can use Electricity Maps API to get access to information about
where the electricity in a specific area comes from,
how much carbon was emitted to produce it.
Furthermore, you can use this API to get historical data, live data and forecasts.

## Source
[Electricity Maps API docs](https://static.electricitymaps.com/api/docs/index.html)

## Publisher: Vitalii Sorokin

## Prerequisites
This API uses an API key.
The API key should be included as a header in all requests. Don't have an API key [Contact Electricity Maps](https://www.electricitymaps.com/#contactform) or [sign up](https://api-portal.electricitymaps.com) for a free trial product or subscribe for a product for your needs.

## Getting Started
To create a connection you need to enter the Base URL of the  subscribed product and corresponding API Key. You can find these values in your profile at [Electricity Map API Portal](https://api-portal.electricitymaps.com).

## Known Issues and Limitations
There are no known issues at this time.

## Supported Operations

### Get Carbon Intensity Forecast
Retrieves the forecasted carbon intensity (in gCO2eq/kWh) of an area. It can either be queried by zone identifier or by geolocation.

### Get Carbon Intensity History
Retrieves the last 24h of carbon intensity (in gCO2eq/kWh) of an area. It can either be queried by zone identifier or by geolocation. The resolution is 60 minutes.

### Get Live Carbon Intensity
Retrieves the last known carbon intensity (in gCO2eq/kWh) of electricity consumed in an area. It can either be queried by zone identifier or by geolocation.

### Get Power Breakdown History
Retrieves the last 24h of power consumption and production breakdown of an area, which represents the physical origin of electricity broken down by production type. It can either be queried by zone identifier or by geolocation. The resolution is 60 minutes.

### Get Live Power Breakdown
Retrieves the last known data about the origin of electricity in an area.
`Power Production` (in MW) represents the electricity produced in the zone, broken down by production type
`Power Consumption` (in MW) represents the electricity consumed in the zone, after taking into account imports and exports, and broken down by production type.
`Power Export` and `Power Import` (in MW) represent the physical electricity flows at the zone border
`Renewable Percentage` and `FossilFree Percentage` refers to the % of the power consumption breakdown coming from renewables or fossil-free power plants (renewables and nuclear) It can either be queried by zone identifier or by geolocation.

### Get Power Consumption Forecast
Retrieves the forecasted power consumption breakdown of an area, which represents the physical origin of electricity broken down by production type. It can either be queried by zone identifier or by geolocation.

### Get Power Production Forecast
Retrieves the forecasted power production breakdown of an area by production type. It can either be queried by zone identifier or by geolocation.

### Get Available Zones
Returns all zones available.

### Check API Health
 Can be used to automatically verify that the Electricity Maps API is up.