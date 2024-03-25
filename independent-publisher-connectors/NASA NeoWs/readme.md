# NASA Near-Earth Objects
NeoWs (Near Earth Object Web Service) is a web service for near earth Asteroid information: search for Asteroids based on their closest approach date to Earth, lookup a specific Asteroid with its NASA JPL small body id, as well as browse the overall data-set.

## Publisher: Fördős András

## Prerequisites
You can read more about the NASA Image and Video Library at [here](https://api.nasa.gov).

## Obtaining Credentials
You do not need to authenticate in order to explore the NASA data. However, if you will be intensively using the APIs to, say, support a mobile application, then you should sign up for a NASA developer at [https://api.nasa.gov/#signUp](https://api.nasa.gov/#signUp). This particular connector enforces the usage of an API Key.

## Supported Operations

### Browse
Browse the overall Asteroid data-set.

### Feed
Retrieve a list of Asteroids based on their closest approach date to Earth.

### Lookup
Lookup a specific Asteroid based on its NASA JPL small body (SPK-ID) ID.

## Known Issues and Limitations
There is a generic hourly rate limit of 1,000 requests per hour, see [https://api.nasa.gov/](https://api.nasa.gov/). If you exceed this limit, your key will be blocked for an hour.

Additionally, the connector does not provide access to all endpoints and/or data of the udnderlyng service, although the intention is there. Get in touch, if you are missing somethig and let's discuss on how it could be enabled through this connector!
