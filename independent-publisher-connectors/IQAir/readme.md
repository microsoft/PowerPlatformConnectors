# IQAir - Airvisual

IQAir provides the world's largest free real-time air quality and pollution information platform from over 100.000 locations and cities. Get access to historical, current or even forecasted data through a single source!

## Publisher: Fördős András

## Prerequisites

IQAir provides various subscription models, out of which the free tier already allows you to access its vast amount of data. Sign up at the following [LINK](https://www.iqair.com/auth/signup)

## Obtaining Credentials

Once signed up, you can create one (or more) API Keys under your profile. This key is required to use the API.

## Supported Operations

### List countries
Request a list of available and supported countries by the API.

### List states
Request a list of available and supported states within a given country by the API.

### List cities
Request a list of available and supported cities within a given state (and country) by the API.

### Get data by coordinates
Request up-to-date data regarding the nearest city to your defined coordinates.

### Get data by city
Request up-to-date data regarding the chosen city within a state and country combination.

## Known Issues and Limitations

The current version of the connector only supports a subset of the API endpoints, mostly from the community plan. Contact me if you need to bring in any of the other ones!

The API itself has some limitations based on your subscription plan, for example calls per minute or per month: [LINK](https://www.iqair.com/commercial/air-quality-monitors/airvisual-platform/api)
