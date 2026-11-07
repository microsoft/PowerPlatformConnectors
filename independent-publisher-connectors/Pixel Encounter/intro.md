# Pixel Encounter

Simply a web application and an API which generates randomly generated pixel monsters in SVG format. It can be used to create profile pictures or enhance any of your graphic interfaces.

## Publisher: Fördős András

## Prerequisites

Pixel Encounter is an open API and requires no purchase or subscription.

## Obtaining Credentials

There are no credentials needed for this API.

## Deprecation

**This connector is deprecated.** The underlying Pixel Encounter service (pixelencounter.com) is no longer available, so the connector's operations no longer return data.

If you are using it in your flows or apps, please migrate away from it. As an alternative for generating avatars, consider the **DiceBear** independent publisher connector (based on dicebear.com), which offers a range of styles including pixel art. Reach out if you need help migrating.

## Supported Operations

### Get a random monster (JSON) (deprecated)
Get a random pixel monster with its JSON representation.

### Get a monster by ID (JSON) (deprecated)
Get a monster by ID with its JSON representation.

### List monsters (deprecated)
Get a paged list with SVG monsters

### Get a random SVG monster (JSON) (deprecated)
Get an SVG monster with random pattern with JSON representation.

## Known Issues and Limitations

This connector is deprecated and its operations are non-functional, because the underlying Pixel Encounter service is no longer available (see [Deprecation](#deprecation) above).

The connector only ever supported a subset of the API endpoints.

The API previously had some limitations:
* maximum 2 requests/second tied to the IP address
* maximum 10 000 requests/30 days tied to the IP address
* no support for CORS.
