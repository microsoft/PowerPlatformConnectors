# Airly

Airly provides accurate, hyper-local data about air pollution in your community. This is the first step towards understanding the health risks and making improvements.

## Publisher: Tomasz Poszytek

## Prerequisites

You have to create an account at Airly. You can sign up for free [here](https://developer.airly.org/en/login). After you register, you will be then able to obtain your apiKey [here](https://developer.airly.org/en/docs#general.authentication).

## API Limits

Depending on the plan you have, there are different API lmits. Please get familiar with them by navigating [here](https://airly.org/en/pricing/airly-api/). Default rate limit per apikey is **100 API requests per day** for all users. In case of exceeding the limit API request will return `HTTP 429 - Too Many Requests`. Also, each API response contains additional HTTP headers that inform about current API key limits and their usage. Header names begin with `X-RateLimit-Limit-` and `X-RateLimit-Remaining-`.

## Coordinates

In several action you are able to provide cooridantes of the point in which neighbourhood you would like to obtain data. The coordinates (both as query parameters and as returned payload fields) are according to **WGS 84 standard**.

## Time

All the time values that occur in the API (both as query parameters and as returned payload fields) are **ISO8601 timestamps** according to **UTC timezone** e.g. `2018-08-24T08:24:48.652Z`.

## Supported Operations

### Get installation by ID

Operation returns single installation metadata, given by **InstallationId**.

### Get nearest installations

Endpoint returns list of installations which are closest to a given point, sorted by distance to that point.

### Get measurements for installation

Endpoint returns measurements for concrete installation given by **InstallationId**. The operation returns unified payload format, which contains following fields:

1. **current** - single element of type AveragedValues; contains averaged measurements for the last 60 minutes (moving average); for installations other than Airly (e.g. GIOŚ) could represent data up to 3 hours old, due to delays in third party data arrival,
2. **history** - list of elements of type AveragedValues; contains a list of historical hourly averaged measurements for the last 24 full hours,
3. **forecast** - list of elements of type AveragedValues; contains a list of anticipated future hourly averaged measurements for the next 24 full hours.

### Get nearest measurements

Endpoint returns measurements for an installation closest to a given location. The operation returns unified payload format, which contains following fields:

1. **current** - single element of type AveragedValues; contains averaged measurements for the last 60 minutes (moving average); for installations other than Airly (e.g. GIOŚ) could represent data up to 3 hours old, due to delays in third party data arrival,
2. **history** - list of elements of type AveragedValues; contains a list of historical hourly averaged measurements for the last 24 full hours,
3. **forecast** - list of elements of type AveragedValues; contains a list of anticipated future hourly averaged measurements for the next 24 full hours.

### Get measurements for a point

Endpoint returns measurements for any geographical location. Measurement values are interpolated by averaging measurements from nearby sensors (up to 1,5km away from the given point). The returned value is a weighted average, with the weight inversely proportional to the distance from the sensor to the given point. The operation returns unified payload format, which contains following fields:

1. **current** - single element of type AveragedValues; contains averaged measurements for the last 60 minutes (moving average); for installations other than Airly (e.g. GIOŚ) could represent data up to 3 hours old, due to delays in third party data arrival,
2. **history** - list of elements of type AveragedValues; contains a list of historical hourly averaged measurements for the last 24 full hours,
3. **forecast** - list of elements of type AveragedValues; contains a list of anticipated future hourly averaged measurements for the next 24 full hours.

### Get available indexes

Endpoint returns a list of all the index types supported in the API along with lists of levels defined per each index type.

### Get available measurements

Endpoint returns list of all the measurement types supported in the API along with their names and units.

## Known Issues and Limitations

No issues and limitations are known at this time.

## Getting Started

You can visit [Airly API documentation](https://developer.airly.org/en/docs) to get even more information about the data returned by the actions (endpoints). Especially get familiar with possible error responses and their schemas ([here](https://developer.airly.org/en/docs#general.errors)), as you might want to build support for them as well, especially the `301 Moved Permanently` response, that contains ID of the installation which replaced current installation and a link inside `Location` header to obtain its details. Read more [here](https://developer.airly.org/en/docs#concepts.installations).

## Disclaimer

Despite that I put a lot of effort to prepare connector as best as possible, issues may occur. If you face any issue, please let me know immediately!