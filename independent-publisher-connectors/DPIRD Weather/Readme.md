
## DPIRD Weather Connector for Western Australia
The Department of Primary Industries and Regional Development's (DPIRD) Agriculture and Food branch has implemented and manages over 185 weather stations throughout Western Australia. These weather stations provide timely, relevant and local weather data to assist growers and regional communities make more informed decisions.

The data includes air temperature, humidity, rainfall, wind speed wind direction, and most stations also measure incoming solar radiation to calculate evaporation and evapotranspiration (ETo). Averages, totals, minimum and maximum values are calculated and the data is summarised into hourly, daily, monthly and yearly data.

You can see what information is available from the API, and how you can filter and search for specific information, by exploring the [interactive home page](https://www.agric.wa.gov.au/weather-api-20).

## Prerequisites
You will need the following to proceed
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power platform CLI tools
* An [API key](https://www.agric.wa.gov.au/form/dpird-api-registration) from DPIRD in accordance with [terms and conditions](https://www.agric.wa.gov.au/apis/api-terms-and-conditions)

## Authentication
To create a connection from this connector, you need to register for an API key, which will be added to a custom HTTP header called 'api-key'. The custom header you send will allow access to all of the public available API's (provided the API Key is valid).

If you would like an API key, please fill out the [DPIRD API Registration Form](https://www.agric.wa.gov.au/form/dpird-api-registration)

## Publisher: Paul Culmsee

## Building the connector 
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json

## Supported Operations
The connector supports the following operations
### Get Weather Stations 
Returns a list of stations and their metadata.

### Get Weather Stations Availability 
Gets the availability metadata for weather stations

### Get Weather Stations Nearby 
Returns a list of nearby stations, sorted by distance. Use paging to control the number of stations returned. Returns 5 stations by default. Sorted by distance from location ascending.

### Get Weather Station (Single Station) 
Get the metadata for a specific station

### Get Weather Station Availability (Single Station)
Get Weather Station Availability (Single Station)

### Get Weather Stations Bulletins
Returns daily 9am bulletins for a list of stations. The daily 9am bulletin is a summary where every field has been summarised over the 24 hour period to 9am.

### Get Weather Stations Rainfall
Get the rainfall weather stations have received over various time periods up to the current time.

### Get Weather Station Rainfall (Single Station)
Get the rainfall weather station has received over various time periods up to the current time.

### Get Weather Stations Extreme Conditions 
Get the amount of time weather stations have had extreme conditions for, over various time periods. The extreme conditions indicated are: frost conditions - where temperature is less than or equal to 2°C; heat conditions - where temperature is greater than or equal to 30°C; and erosion conditions - where wind is greater than or equal to 28.8km/h.

### Get Stations Extreme Events 
Find extreme values for weather station properties.

### Get Stations Latest Data 
Get the latest data for a series of stations. Returns all active stations by default.

### Get Weather Station Bulletins (Single Station) 
Returns daily 9am bulletins for a station. The daily 9am bulletin is a summary where every field has been summarised over the 24 hour period to 9am.

### Get Station Latest Data 
Get the latest data for a station.

### Get Station Minute Data 
Returns minute recordings for a station over a maximum 24 hour period.

### Get Stations 15min Summary
Returns a set of quarter-hourly summaries for a list of stations. Note that the 15min summary is only available from the beginning of the previous year to the current date.

### Get Stations 30min Summary 
Returns a set of half-hourly summaries for a list of stations. Note that the 30min summary is only available from the beginning of the previous year to the current date.

### Get Stations Hourly Summary 
Returns a set of hourly summaries for a list of stations.

### Get Stations Daily Summary 
Returns a set of daily summaries for a list of stations. The 24 hour period each field is summarised over varies between 24 hours to 9am (eg rainfall, minimum temperature), 24 hours from 9am (eg maximum temperature) and 24 hours from 12am (eg wind, solar)

### Get Stations Monthly Summary 
Returns a set of monthly summaries for a list of stations. The monthly period each field is summarised over is a monthly composite of the same 24 hour period used by the field in the daily summary. For example, rainfall, which uses the 24 hours to 9am daily period starts at 9am on the last day of the previous month and concludes at 9am on the last day of the month.

### Get Stations Yearly Summary 
Returns a set of annual summaries for a list of stations. The yearly period each field is summarised over is a yearly composite of the same 24 hour period used by the field in the daily summary. For example, rainfall, which uses the 24 hours to 9am daily period starts at 9am on the last day of the previous year and concludes at 9am on the last day of the year.

### Get Stations 15min Summary Timeseries
Returns a set of quarter-hourly time-series summaries for a list of stations. Useful for charts. Note that the 15min summary is only available from the beginning of the previous year to the current date.

### Get Stations 30min Summary Timeseries 
Returns a set of half-hourly time-series summaries for a list of stations. Useful for charts.

### Get Stations Hourly Summary Timeseries
Returns a set of hourly time-series summaries for a list of stations. Useful for charts. 

### Get Stations Daily Summary Timeseries
Returns a set of daily time-series summaries for a list of stations. Useful for charts.

### Get Stations Monthly Summary Timeseries
Returns a set of monthly time-series summaries for a list of stations. Useful for charts.

### Get Stations Yearly Summary Timeseries 
Returns a set of yearly time-series summaries for a list of stations. Useful for charts.

### Get Single Station 15min Summary
Returns a set of quarter-hourly summaries for a specific station. Note that the 30min summary is only available from the beginning of the previous year to the current date.

### Get Single Station 30min Summary
Returns a set of half-hourly summaries for a specific station. Note that the 15min summary is only available from the beginning of the previous year to the current date.

### Get Single Station Hourly Summary
Returns a set of hourly summaries for a specific station.

### Get Single Station Daily Summary
Returns a set of daily summaries for a specific station. The 24 hour period each field is summarised over varies between 24 hours to 9am (eg rainfall, minimum temperature), 24 hours from 9am (eg maximum temperature) and 24 hours from 12am (eg wind, solar).

### Get Single Station Monthly Summary
Returns a set of monthly summaries for a specific station. The monthly period each field is summarised over is a monthly composite of the same 24 hour period used by the field in the daily summary. For example, rainfall, which uses the 24 hours to 9am daily period starts at 9am on the last day of the previous month and concludes at 9am on the last day of the month.

### Get Single Station Yearly Summary
Returns a set of yearly summaries for a specific station. he yearly period each field is summarised over is a yearly composite of the same 24 hour period used by the field in the daily summary. For example, rainfall, which uses the 24 hours to 9am daily period starts at 9am on the last day of the previous year and concludes at 9am on the last day of the year.

## Known Issues and Limitations
* The API Key is provided free of charge but is non-transferable and must only be used by the individual or entity to which the API Key has been issued (the API User).

* An API User may be subject to a limit on the number of free API calls it is permitted to make per day or per month. DPIRD will notify the API User through its Nominated User of any limits applicable to an API User’s DPIRD User Account.

* Whilst the Department of Primary Industries and Regional Development make every effort to provide accurate data in a timely fashion, we are not able to guarantee the availability of data at all times.

* Due to remoteness and harsh environments the weather stations may be submitted to, there are times when we may not receive any data. In future we will attempt to derive some of the missing data.