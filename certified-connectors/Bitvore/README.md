# Bitvore Entity API

## Description

The corporate entities, companies or organizations, that the articles the Corporate News API returns are persisted in the Bitvore Knowledge Graph. The Entity API provides access to the information about those corporate entities.

![Entity Information](https://s3.amazonaws.com/static.assets.bitvore.com/api-docs/entity.png)

Companies can be searched for based on some criteria or directly using a Bitvore ID which uniquely identifies an entity in the Bitvore system. When using the Bitvore ID the entire company profile will be returned for that single company. If one is unsure what the Bitvore ID of a company is a search can be performed using different profile properties such as name, ticker, industry, etc. The API will return all companies that match the search criteria. Only summary information is returned but it should be enough information to identify the exact company being searched for. Once the Bitvore ID is found news can be retrieved for the company using the Corporate News API.

![Entity Lookup](https://s3.amazonaws.com/static.assets.bitvore.com/api-docs/entity-lookup.png)

The Bitvore ID for a company never changes so it is recommended that clients save the Bitvore IDs for future use instead of calling the Entity API each time if possible.

Each company has a monitoring state field which can have the value “active” or “inactive”. Active companies are being monitored by Bitvore so news is being currently collected. An inactive company will need to be activated in order to start news collection. If you see an inactive company you would like activated please contact customer support.

The Entity API is part of the corporate data set and requires a corporate data set API key for access. 

API Documentation can be found [here](https://developer.bitvore.com/docs/overview/entity-api)

## Pre-Requisites

In order to utilize the Bitvore Entity API, you need to register for an API Key by contacting the [Products Team](mailto:products@bitvore.com).

## Deploying the Connector

Run the following command and follow the prompts. 

```sh
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```