# AccuWeather
AccuWeather is a service that provides commercial weather forecasting services worldwide.

## Publisher
### Ahmad Najjar

## Prerequisites
You need to have an AccuWeather [developer account](https://developer.accuweather.com/) or a [subscription package](https://developer.accuweather.com/packages).

## Supported Operations
### [Locations Operations](https://developer.accuweather.com/accuweather-locations-api/apis)
#### Get Admin Area List
Returns basic information about administrative areas in the specified country.

#### Get Country List
Returns basic information about all countries within a specified region.

#### Get Region List
Returns basic information about all regions.

#### Get Top Cities List
Returns information for the top 50, 100, or 150 cities, worldwide.

#### Autocomplete search (Cities)
Returns basic information about locations matching an autocomplete of the search text.

#### Geoposition Search (Latitude and Longitude)
Returns information about a specific location, by GeoPosition (Latitude and Longitude).

### [Forecasts Operations](https://developer.accuweather.com/accuweather-forecast-api/apis)
#### Get Daily Forecasts
Returns daily forecast data for a specific location.

#### Get Hourly Forecasts
Returns forecast data for the next hour(s) for a specific location.

### [Current Conditions Operations](https://developer.accuweather.com/accuweather-current-conditions-api/apis)
#### Get Current Conditions
Returns current conditions data for a specific location. Current conditions searches require a location key. Please use the Locations API to obtain the location key for your desired location. By default, a truncated version of the current conditions data is returned. The full object can be obtained by passing "details=true" into the url string.

#### Get Current Conditions for Top Cities
Returns current conditions data for the top 50, 100, or 150 cities worldwide, based on rank.

#### Get Historical Current Conditions (past 24 hours)
Returns historical current conditions data for a specific location. Current conditions searches require a location key. Please use the Locations API to obtain the location key for your desired location. By default, a truncated version of the current conditions data is returned. The full object can be obtained by passing "details=true" into the url string.

#### Get Historical Current Conditions (past 6 hours)
Returns historical current conditions data for a specific location. Current conditions searches require a location key. Please use the Locations API to obtain the location key for your desired location. By default, a truncated version of the current conditions data is returned. The full object can be obtained by passing "details=true" into the url string.

### [Indices Operations](https://developer.accuweather.com/accuweather-indices-api/apis)
#### Get Daily Index Values for a Group of Indices
Returns daily index data for a specific group of indices, by location key. Not all daily indices' groups data are available for retrieval, check (https://developer.accuweather.com/list-available-daily-indices)

#### Get Daily Index Values for a Specific Index
Returns daily index data for a specific index, by location key. Not all daily indices data are available for retrieval, check (https://developer.accuweather.com/list-available-daily-indices)

#### Get Daily Index Values for for All Indices
Returns daily index data for all indices, by location key.

#### Get List of Daily Indices
Returns metadata for all daily indices.

#### Get List of Index Groups
Returns metadata for all index groups.

#### Get List of Indices in a Specific Group
Returns metadata for all indices in a specified group.

#### Search by location key
Returns information about a specific location, by location key.

#### Search location by IP address
Returns information about a specific location, by IP Address.

### [Weather Alarms](https://developer.accuweather.com/accuweather-weather-alarms-api/apis)
#### Get one day of weather alarms
Returns 1 day of weather alarms for a specific location.
#### Get five days of weather alarms
Returns 5 days of weather alarms for a specific location.
#### Get ten days of weather alarms
Returns 10 days of weather alarms for a specific location.
#### Get fifteen day of weather alarms
Returns 15 days of weather alarms for a specific location.

### [Translations](https://developer.accuweather.com/accuweather-translations-api/apis)
#### List all languages
Returns metadata for all languages.
#### List the available translation groups
Lists groups of phrases that are available for translation.
#### List the translations for a specific group
Returns all translated phrases for a specific group, in the desired language.

### [Alerts](https://developer.accuweather.com/accuweather-alerts-api/apis)
#### Get alerts by location key
Returns alert data for a specific location. Alert searches require a location key. Please use the Locations API to obtain the location key for your desired location. By default, a truncated version of the alert data is returned. The full object can be obtained by passing details=true.

### [Imagery](https://developer.accuweather.com/accuweather-imagery-api/apis)
#### Get radar and satellite imagery
Returns links to radar and satellite images by location key. Imagery searches require a location key. Please use the Locations API to obtain a location key for your desired location. Radar images are not available for all locations worldwide at this time. If radar is not available for your desired location, satellite images are still provided.

## API Documentation
Visit [AccuWeather APIs reference](https://developer.accuweather.com/apis) page for further details.

## Known Issues and Limitations
* Operation "Get Daily Index Values for a Group of Indices"
* Operation "Get Daily Index Values for a Specific Index"
Not all daily indices/groups' indices data are available for retrieval, check (https://developer.accuweather.com/list-available-daily-indices)

#### Not all operations provided by AccuWeather are part of the first IP connector submission. I will keep adding/updating/supporting this connector based on your feedback/requests :)
