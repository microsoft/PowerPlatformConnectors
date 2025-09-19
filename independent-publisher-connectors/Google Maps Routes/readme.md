# Google Maps Routes Connector
The Google Maps Routes Connector allows you to calculate travel information between two locations.  
It uses the **Google Maps Routes API to return:  

- Total travel distance (in meters)  
- Estimated travel duration (in seconds)  

This connector supports a **single origin–destination pair** per request, making it easy to integrate route calculations into your apps and workflows

This connector is designed for **one-to-one route calculations**.  
Each request supports a **single origin address** and a **single destination address**, and returns the total travel distance and duration for that route.  


## Publisher: Remsey Mailjard (Skills4-IT)

## Prerequisites
To use this connector, you will need:

- A **Google Cloud project** with the [Routes API enabled](https://console.cloud.google.com/marketplace/product/google/routes.googleapis.com).  
  You can enable the API in the **Google Cloud Console** under *APIs & Services* > *Library*.  
- A valid **API key**, created in the [Google Cloud Console Credentials page](https://console.cloud.google.com/apis/credentials).  


## Obtaining Credentials
This connector authenticates with an API key passed via the `X-Goog-Api-Key` header.  
Store the key securely in the connector connection settings.

## Supported Operations

### Calculate Distance and Travel Time
Calculates a route between one origin and one destination.  
The response includes the total travel distance (in meters) and the estimated travel time (in seconds).

## Known Issues and Limitations
- There are currently no known issues with this connector.  
- Each request supports only a **single origin–destination pair**. Calculating multiple routes or distance matrices is not supported.


## Versioning
- **1.0** – Initial release: distance (meters) + duration (seconds).

## Support
- Publisher: **Remsey Mailjard** – Skills4-IT  
- Website: https://www.skills4-it.nl  
- Email: remsey@skills4-it.nl
