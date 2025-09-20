<<<<<<< HEAD
# Google Maps Routes

The Google Maps Routes connector allows you to calculate travel information between two locations. It uses the Google Maps Routes API to return the total travel distance and estimated travel duration for a specified mode of transport. This connector is designed for one-to-one route calculations, supporting a single origin and a single destination per request.
=======
# Google Maps Routes Connector
The Google Maps Routes Connector allows you to calculate travel information between two locations.  
It uses the **Google Maps Routes API to return:  

- Total travel distance (in meters)  
- Estimated travel duration (in seconds)  

This connector supports a **single origin–destination pair** per request, making it easy to integrate route calculations into your apps and workflows

This connector is designed for **one-to-one route calculations**.  
Each request supports a **single origin address** and a **single destination address**, and returns the total travel distance and duration for that route.  

Typical use cases include:  
- Estimating travel distance and duration in workflows  
- Enriching business data with route information  
- Integrating location-based logic into apps and automations  
>>>>>>> 1594a9d16961f80568b76c5962cb68cf11b6b664

## Publisher: Remsey Mailjard (Skills4-IT)

## Prerequisites
<<<<<<< HEAD

To use this connector, you will need a Google Cloud project with the Routes API enabled and a valid API key.

1.  A **Google Cloud project** with the [Routes API enabled](https://console.cloud.google.com/marketplace/product/google/routes.googleapis.com). You can enable the API in the Google Cloud Console under *APIs & Services > Library*.
2.  A valid **API key**. You can create one on the [Google Cloud Console Credentials page](https://console.cloud.google.com/apis/credentials). It is recommended to restrict the key to the Routes API for security.

## How to get credentials

This connector authenticates with an API key (via the `X-Goog-Api-Key` header).

1.  Navigate to the [Google Cloud Console Credentials page](https://console.cloud.google.com/apis/credentials).
2.  Copy your API key.
3.  When creating a new connection in Power Automate or Power Apps, paste the key into the "API Key" field.

## Get started with your connector

After creating a connection, you can use the connector's action in your flows.

**Example Flow: Calculate Biking Distance**

1.  Add the **Get Distance and Travel Time** action.
2.  Set **Origin Address** to `Spui 70, Den Haag`.
3.  Set **Destination Address** to `Damrak 1, Amsterdam`.
4.  Set **Travel Mode** to `BICYCLE`.
5.  Use the resulting `Display Distance` (e.g., "64.2 km") in a subsequent step.

## Known issues and limitations

*   **Single Route Only:** Each request supports only a single origin–destination pair. Calculating routes for multiple waypoints or creating distance matrices is not supported.
*   **Real-time Traffic:** The connector returns estimated durations. Real-time traffic conditions are handled by Google's backend logic.

## Supported Operations

The connector supports the following operation:

### Get Distance and Travel Time

Calculates a route between one origin and one destination and returns the total travel distance and the estimated travel time.

#### Input Parameters

| Name | Type | Required | Description |
| --- | --- | --- | --- |
| Origin Address | `string` | Yes | The full street address or place where the route starts. |
| Destination Address | `string` | Yes | The full street address or place where the route ends. |
| Travel Mode | `string` | No | The mode of travel. Options: `DRIVE` (default), `BICYCLE`, `WALK`, `TWO_WHEELER`, `TRANSIT`. |
| Units | `string` | No | The unit system for the distance output. Options: `METRIC` (default) or `IMPERIAL`. |

#### Output Parameters

| Name | Type | Description |
| --- | --- | --- |
| Travel distance in meters | `integer` | The total travel distance in meters (e.g., 75400). |
| Travel duration (seconds) | `string` | The total travel duration in seconds, formatted as a string (e.g., "3461s"). |
| Display Distance | `string` | The human-readable distance value (e.g., "75.4 km"). |
| Duration travel time | `string` | The human-readable time needed to travel (e.g., "58 mins"). |
=======
To use this connector, you will need:

- A **Google Cloud project** with the [Routes API enabled](https://console.cloud.google.com/marketplace/product/google/routes.googleapis.com).  
  You can enable the API in the **Google Cloud Console** under *APIs & Services* > *Library*.  
- A valid **API key**, created in the [Google Cloud Console Credentials page](https://console.cloud.google.com/apis/credentials).  


## Obtaining Credentials
This connector authenticates with an API key passed via the `X-Goog-Api-Key` header.  
Store the key securely in the connector connection settings.

## Supported Operations
The connector supports the following operations:
* `Calculate Distance and Travel Time`: Calculates a route between one origin and one destination and returns the total travel distance (in meters) and the estimated travel time (in seconds).


## Known Issues and Limitations
- There are currently no known issues with this connector.  
- Each request supports only a **single origin–destination pair**. Calculating multiple routes or distance matrices is not supported.

>>>>>>> 1594a9d16961f80568b76c5962cb68cf11b6b664
