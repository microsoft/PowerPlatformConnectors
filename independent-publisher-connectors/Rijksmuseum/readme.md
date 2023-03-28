# Rijksmuseum
Rijksmuseum data services provide access to object metadata and user generated content from their museum collection.

## Publisher: Ashwin Ganesh Kumar

## Prerequisites
You need a Rijksmuseum account. You can make one for free [here](https://www.rijksmuseum.nl/nl/registreer?redirectUrl=https://www.rijksmuseum.nl/nl).

## Obtaining Credentials
For authorization an API key is required. How to get this is described in
their [website](https://data.rijksmuseum.nl/object-metadata/api/)

## Supported Operations

### Object metadata
Searching the collection of objects from Rijksmuseum. [More info](https://data.rijksmuseum.nl/object-metadata/api/)

### User-generated content
Gives access to sets of objects created by users in Rijksstudio. [More info](https://data.rijksmuseum.nl/user-generated-content/api/)


## Known Issues and Limitations
Although the API service is free, the number of calls is limited. The number of results that will be fetched cannot exceed 10,000.

## Deployment Instructions
Upload the connector and authorize by providing your API key. 