# GeoDB
The GeoDB API focuses on getting global city and region data. Filter cities by name prefix, country, location, time-zone, and even minimum population. Sort cities by name, country code, elevation, and population - or any combination of these. Get all country regions and all cities in a region.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
Please review the [API documentation](https://wirefreethought.github.io/geodb-cities-api-docs/).

## Obtaining Credentials
You will need to have a RapidAPI account and subscribe to the GeoDB service before receiving an API key.

## Supported Operations
### Find administrative divisions
Find administrative divisions, filtering by optional criteria. If no criteria are set, you will get back all known divisions.
### Find divisions near division
Find administrative divisions near the given origin division, filtering by optional criteria. If no criteria are set, you will get back all known divisions.
### Find cities near division
Find cities near the given administrative division, filtering by optional criteria. If no criteria are set, you will get back all known cities.
### Find cities
Find cities, filtering by optional criteria. If no criteria are set, you will get back all known cities.
### Find cities near city
Find cities near the given origin city, filtering by optional criteria. If no criteria are set, you will get back all known cities.
### Find divisions near location
Find administrative divisions near the given location, filtering by optional criteria. If no criteria are set, you will get back all known divisions.
### Find cities near location
Find cities near the given location, filtering by optional criteria. If no criteria are set, you will get back all known cities.
### Get administrative division details
Get the details for a specific administrative division, including location coordinates, population, and elevation above sea-level (if available).
### Get city details
Get the details for a specific city, including location coordinates, population, and elevation above sea-level (if available).
### Get city distance
Get distance from the given city.
### Get city date-time
Get city date-time.
### Get city admin region
Get the details for the containing populated place (e.g., its county or other administrative division), including location coordinates, population, and elevation above sea-level (if available).
### Get city time
Get city time.
### Find countries
Find countries, filtering by optional criteria. If no criteria are set, you will get back all known countries.
### Get country details
Get the details for a specific country, including number of regions.
### Find country regions
Get all regions in a specific country. These could be states, provinces, districts, or otherwise major political divisions.
### Get region details
Get the details of a specific country region, including number of cities.
### Find country region administrative divisions
Get the administrative divisions in a specific country region. The country and region info is omitted in the response.
### Find country region cities
Get the cities in a specific country region. The country and region info is omitted in the response.
### Find currencies
Find currencies, filtering by optional criteria. If no criteria are set, you will get back all known currencies.
### Get languages
Get all supported languages.
### Get locales
Get all known locales.
### Get time-zones
Get all known time-zones.
### Get time-zone
Get time-zone.
### Get time-zone date-time
Get time-zone date-time.
### Get time-zone time
Get time-zone time.

## Known Issues and Limitations
There are no known issues at this time.
