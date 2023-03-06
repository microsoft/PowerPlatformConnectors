# Storm Glass Global Weather
Weather forecasts & historical data from the world’s most trusted meteorological institutions in one single API. The Storm Glass API provides high-resolution forecasts for up to 10 days ahead as well as historical data. Marine data including tide is available for all oceans and seas world wide.

## Publisher: Paul Culmsee, Seven Sigma Business Solutions

## Prerequisites
* Usage of this API is subject to [Storm Glass Terms and Conditions](https://stormglass.io/terms-and-conditions/)

## Obtaining Credentials
* You will need to sign up to a [Storm Glass plan](https://stormglass.io/pricing/) to obtain an API key to access the API.

## Supported Operations
### Weather Point Request
The weather request is used to fetch weather data for a point. To get marine data you include a coordinate at sea in your request, and to get data for land and lakes - simply send in a coordinate located on land or on a lake.
### Bio Point Request
The bio request is used to fetch bio related data for a point.
### Tide Extremes Point Request
Retrieve information about high and low tide for a single coordinate. If nothing is specified, the returned values will be in relative to Mean Sea Level - MSL.
### Tide Sea-level Point Request
Retrieve the sea level given in meters hour by hour for a single coordinate. If nothing is specified the returned values will be in relative to Mean Sea Level - MSL.
### Get Tide Stations
The Tide Stations List Requests is used to list all available stations.
### Get Tide Stations for Area
The Tide Stations Area Request will list all available tide stations within a defined geographic area.
### Astronomy Point Request
Retrieve sunrise, sunset, moonrise, moonset and moon phase for a single coordinate.
### Solar Data Point Request
The solar request is used to fetch solar related data for a point
### Elevation Point Request
The elevation request is used to fetch elevation data (bathymetry for oceans and topography for land) for a point. The Storm Glass API provides elevation data globally.

## Known Issues and Limitations
* The free plan is not for commercial use and is limited to 10 requests per day
* The schema is dynamic in that a global network of weather and climate service providers cover different parts of the globe. This means your flows might need some post-processing work 