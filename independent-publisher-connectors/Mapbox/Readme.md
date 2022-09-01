# Mapbox
Mapbox allows you to access its navigation, location search and static map generation API services to create interative/static maps in your application.

## Publisher: Simone Lin

## Prerequisite
You need to have a Mapbox [account](https://account.mapbox.com/auth/signin/?route-to=%22https%3A%2F%2Faccount.mapbox.com%2F%22) to create an API access token. ([Pricing](https://www.mapbox.com/pricing))

## Supported Operations
### [Forward Geocoding](https://docs.mapbox.com/api/search/geocoding/#forward-geocoding)
This action allows you to look up a single location by name and will return you the geographic coordinates (latitude and longitude) of the location
### [Get Static Map Image](https://docs.mapbox.com/api/maps/static-images/)
This action outputs a static map image of a location in base64, the location is specified by its longitude and latitude. The static map image does not have interactivity or controls.
### [Get Route To Location](https://docs.mapbox.com/api/navigation/directions/)
This action calculates and returns optimal route to reach a specified location from a given location. It supports 4 different routing profiles: `driving-traffic`, `driving`, `walking` and `cycling`. Default routing profile is set to `driving`.

## Obtaining Credentials
Create an account on [Mapbox](https://account.mapbox.com/auth/signin/?route-to=%22https%3A%2F%2Faccount.mapbox.com%2F%22), Log in and navigate into your account, you can get the default access token for [mapbox api](https://docs.mapbox.com/api/overview/) at the bottom of the account page.

## Known Issues and Limitations
* [Mapbox Search API](https://docs.mapbox.com/api/search/search/) is currently in private beta for worldwide coverage. I will update the connector with this action once it is published.
* Mapbox API is free for a limited amount of requests per month, be sure to consult the [APIs Pricing page](https://www.mapbox.com/pricing)
* The query parameter `geometries` in the **Get Route To Location** action currently only supports `polyline` value.