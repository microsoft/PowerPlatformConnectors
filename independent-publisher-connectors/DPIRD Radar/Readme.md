
## DPIRD Radar Connector for Western Australia
The Department of Primary Industries and Regional Development's (DPIRD) allows you to integrate the RADAR estimated rainfall data within your applications.

You can see what information is available from the API, and how you can filter and search for specific information, by exploring the [interactive home page](https://api.dpird.wa.gov.au/v2/radar/openapi/).

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
### Get Radars 
Returns a list of radars and their metadata.

### Get Nearby Radar 
Get nearest radar points to a latitude and longitude

### Get Radar 
Get the metadata for a specific radar

### Get Radar Rainfall
Get the rainfall for a specific radar

### Get Radar Daily Summaries 
Get the daily summaries for a latitude and longitude based on the closest radar point

### Get Radar Monthly Summaries 
Get the monthly summaries for a latitude and longitude based on the closest radar point

## Notes
Many of the actions created allow for a comma delimited set of parameters. 

## Known Issues and Limitations
* The API Key is provided free of charge but is non-transferable and must only be used by the individual or entity to which the API Key has been issued (the API User).

* An API User may be subject to a limit on the number of free API calls it is permitted to make per day or per month. DPIRD will notify the API User through its Nominated User of any limits applicable to an API User’s DPIRD User Account.

* Whilst the Department of Primary Industries and Regional Development make every effort to provide accurate data in a timely fashion, we are not able to guarantee the availability of data at all times.

* Due to remoteness and harsh environments the weather stations may be submitted to, there are times when we may not receive any data. In future we will attempt to derive some of the missing data.