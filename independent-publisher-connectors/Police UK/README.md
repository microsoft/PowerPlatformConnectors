# Police UK
The UK Police connector provides access to crime, neighbourhood, 
force, and stop and search data from the official UK Police API 
at data.police.uk. Data covers England, Wales and Northern Ireland.

## Publisher: Josh Bray

## Prerequisites
No authentication is required. The API is free and publicly accessible.

## Supported Operations

### Forces
- **List all forces** - Returns all police forces available via the API
- **Get specific force** - Returns details for a specific police force
- **Get force senior officers** - Returns senior officers for a force

### Crime
- **Get street-level crimes** - Returns crimes within 1 mile of a location
- **List crime categories** - Returns valid crime category slugs
- **Get last updated date** - Returns when crime data was last updated

### Neighbourhoods
- **List neighbourhoods for a force** - Returns all neighbourhoods for a force
- **Get specific neighbourhood** - Returns details for a neighbourhood
- **Get neighbourhood team** - Returns team members for a neighbourhood
- **Locate neighbourhood** - Returns the neighbourhood for a coordinate

### Stop and Search
- **Get stop and searches by force** - Returns stop and searches by force

## Obtaining Credentials
No credentials or API key are required.

## Known Issues and Limitations
- Location data returned is anonymised and approximate, not exact crime locations
- Scotland is only covered by British Transport Police, so crime levels may appear lower than reality
- Data is updated monthly, not in real time
- The Police API call limit operates using a 'leaky bucket' algorithm as a controller. This allows for infrequent bursts of calls, and allows you to continue to make an unlimited amount of calls over time.
The current rate limit is 15 requests per second with a burst of 30. So, on average you must make fewer than 15 requests each second, but you can make up to 30 in a single second.
If you exceed the limit stated above, the API will return a HTTP 429 (Too Many Requests) response code.
