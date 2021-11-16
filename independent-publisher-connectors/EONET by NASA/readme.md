# EONET by NASA
The Earth Observatory Natural Event Tracker (EONET) is a prototype web service from NASA with the goal of:

- Providing a curated source of continuously updated natural event metadata.
- Providing a service that links those natural events to thematically-related web service-enabled image sources (e.g., via WMS, WMTS, etc.).

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You can read more about EONET at [EONET Documentation](https://eonet.sci.gsfc.nasa.gov/what-is-eonet).

## Obtaining Credentials
To access any NASA and other Data.gov API, you must create an API key at [https://api.nasa.gov/#signUp](https://api.nasa.gov/#signUp).

## Supported Operations

### Get events
Retrieves event imagery.

### Get events by categories
Retrieves a list of categories filtered by event.

### Get events in GeoJSON format
Retrieves event imagery in the GeoJSON format.

### Get categories
​Retrieves a list of categories.

### Get layers
Retrieves a list of layers.

### Get Sources
Retrieves a list of sources.

## Known Issues and Limitations
There is an hourly rate limit of 1,000 requests per hour. If you exceed this limit, your key will be blocked for an hour.
