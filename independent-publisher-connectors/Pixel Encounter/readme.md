# Pixel Encounter

Simply a web application and an API which generates randomly generated pixel monsters in SVG format. It can be used to create profile pictures or enhance any of your graphic interfaces.

## Publisher: Fördős András

## Prerequisites

Pixel Encounter is an open API and requires no purchase or subscription.

## Obtaining Credentials

There are no credentials needed for this API.

## Supported Operations

### Get a random monster (JSON)
Get a random pixel monster with its JSON representation.

### Get a monster by ID (JSON)
Get a monster by ID with its JSON representation.

### List monsters
Get a paged list with SVG monsters

### Get a random SVG monster (JSON)
Get an SVG monster with random pattern with JSON representation.

## Known Issues and Limitations

Current version of the connector only supports a subset of the API endpoints. Contact me if you see a need to bring in any of the other ones!

The API itself has some limitations:
* maximum 2 requests/second tied to the IP address
* maximum 10 000 requests/30 days tied to the IP address
* no support for CORS.
