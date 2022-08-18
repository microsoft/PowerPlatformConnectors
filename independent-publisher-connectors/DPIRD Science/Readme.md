
## DPIRD Science Connector for Western Australia
This API provides the ‘back end’ to the [Rainfall to Date](https://www.agric.wa.gov.au/climate-weather/rainfall-date-tool-western-australia/), [Potential Yield](https://www.agric.wa.gov.au/climate-weather/potential-yield-tool/), and [Soil Water](https://www.agric.wa.gov.au/climate-weather/soil-water-tool/#?station=10115&soil=3&groundCover=1) tools already available on the DPIRD website. These online climate forecasting tools assist grain growers to optimize their crop potential.

* The [Soil Water tool](https://www.agric.wa.gov.au/climate-weather/soil-water-tool/#?station=10115&soil=3&groundCover=1) models how rainfall moves through the layers of different soil types to identify how much water is available for plant roots to take up.
* The [Rainfall to Date](https://www.agric.wa.gov.au/climate-weather/rainfall-date-tool-western-australia/) tool allow growers to select the beginning and end of the season to better determine available soil water for crop use.
* The [Potential Yield calculator](https://www.agric.wa.gov.au/climate-weather/potential-yield-tool/) combines weather data from the department and Bureau of Meteorology’s weather stations with historical decile finishes to produce the maximum yield crop potential.

You can see what information is available from the API, and how you can filter and search for specific information, by exploring the [interactive home page](https://www.agric.wa.gov.au/science-api-20).

## Prerequisites
You will need the following to proceed
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power platform CLI tools
* An [API key](https://www.agric.wa.gov.au/form/dpird-api-registration) from DPIRD in accordance with [terms and conditions](https://www.agric.wa.gov.au/apis/api-terms-and-conditions)

## Obtaining Credentials
To create a connection from this connector, you need to register for an API key, which will be added to a custom HTTP header called 'api-key'. The custom header you send will allow access to all of the public available API's (provided the API Key is valid).

If you would like an API key, please fill out the [DPIRD API Registration Form](https://www.agric.wa.gov.au/form/dpird-api-registration)

## Publisher: Paul Culmsee

## Building the connector 
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json

## Supported Operations
The connector supports the following operations
### Get Stations 
Returns a list of stations that can be used with the Science API

### Get Station (Single Station) 
Get the metadata for a specific station that can be used with the Science API.

### Get Stations Nearby 
Returns a list of stations that can be used with the Science API

### Get Potential Yield 
Returns the potential yield values as calculated using the French & Schultz potential yield model. 
#### The potential yield endpoint uses seasonal rainfall and decile finishes, calculated from historical data, to calculate the maximum wheat yield possible in the absence of any other constraints.
* Potential yield can be used as a tool in the seasonal decision making process.
* Estimates of potential wheat yield are obtained using the French & Schultz potential yield model:
* Yield (tonnes/ha) = WUE * (stored soil water + growing season rainfall - evaporation)
* WUE is the abbreviation for water use efficiency and growing season rainfall is usually calculated from April to October, inclusive.
* Stored soil water at the start of the growing season is estimated as one third of summer rainfall.
* More information on the model available at the [Potential Yield Tool on the DPIRD website](https://www.agric.wa.gov.au/climate-weather/potential-yield-tool)

### Get Soil Water
Returns the amount of water in the soil depending on the rainfall received and the type of soil.

### Get Yellow Spot
Run the Moin or Chris Yellow Spot model.

### Get Rainfall (Single Station)
Returns the rainfall to date, historical rainfall data and projected rainfall data, depending on already received rainfall.

## Notes
Many of the actions created allow for a comma delimited set of parameters. 

## Known Issues and Limitations
* The API Key is provided free of charge but is non-transferable and must only be used by the individual or entity to which the API Key has been issued (the API User).

* An API User may be subject to a limit on the number of free API calls it is permitted to make per day or per month. DPIRD will notify the API User through its Nominated User of any limits applicable to an API User’s DPIRD User Account.

* Whilst the Department of Primary Industries and Regional Development make every effort to provide accurate data in a timely fashion, we are not able to guarantee the availability of data at all times.

* Due to remoteness and harsh environments the weather stations may be submitted to, there are times when we may not receive any data. In future we will attempt to derive some of the missing data.