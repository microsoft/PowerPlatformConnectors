# NASA FIRMS

NASA FIRMS uses satellite observations from the MODIS and VIIRS instruments to detect active fires and thermal anomalies and deliver this information in near real-time to decision makers.

## Publisher: Fördős András

## Prerequisites

No prerequisites exists apart from signing up to obtain an API key (called MAP_KEY).

## Obtaining Credentials

To obtain an API key (called MAP_KEY), simply register through [https://firms.modaps.eosdis.nasa.gov/api/map_key/](https://firms.modaps.eosdis.nasa.gov/api/map_key/). You will receive your API Key in the sign up email, that you can already begin to use.

## Supported Operations

### Name Parser
Name parsing is the process of splitting a person's name into its individual components, such as first name, middle name, last name, suffix, salutation, and title.

## Known Issues and Limitations

Current version of the connector only supports a subset of the API endpoints. Additonally, the supported operations don't fully map with the available inputs to the API endpoints. Contact me if you see a need to bring in any of the other ones or extend the existing ones!

The underlying API itself has some limitations (such as max number of calls within time frame, or data availability per sensor): [https://firms.modaps.eosdis.nasa.gov/content/academy/data_api/firms_api_use.html](https://firms.modaps.eosdis.nasa.gov/content/academy/data_api/firms_api_use.html)
