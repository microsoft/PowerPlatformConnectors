# Google Maps Routes (Independent Publisher)

Google Maps provides powerful location and routing services. This connector uses the [Google Maps Routes API v2](https://developers.google.com/maps/documentation/routes) to calculate travel distance and estimated travel time between an origin and a destination.

## Publisher: Remsey Mailjard
---

## Prerequisites

To use this connector, you will need:
*   A valid [Google Cloud](https://cloud.google.com/) project.
*   An API key with the **Routes API** enabled.

## Obtaining Credentials

1.  Go to the [Google Cloud Console](https://console.cloud.google.com/).
2.  Create a new project or select an existing one.
3.  Navigate to **APIs & Services > Library**.
4.  Search for and select the **Routes API**, then click **Enable**.
5.  Go to **APIs & Services > Credentials**.
6.  Click **Create Credentials > API key**.
7.  Copy the generated API key. It is strongly recommended to secure your API key by restricting its usage to the Routes API only.
8.  Use this API key when creating the connection in the Power Platform. The connector uses an API key in the `X-Goog-Api-Key` header for authentication.

---

## Supported Action

### Get distance and travel time

Calculates the travel distance and estimated travel time between an origin and a destination. The results can include distance in meters, duration in seconds (with and without traffic), and human-readable text values, among other details controlled by the FieldMask.

#### Input Parameters

| Parameter | Description | Required |
| --- | --- | :---: |
| **Origin Address** | The full street address for the origin (e.g., `Damrak 1, Amsterdam`). | Yes |
| **Destination Address** | The full street address for the destination (e.g., `Spoorlaan 5, Tilburg`). | Yes |
| **Travel Mode** | The desired mode of transportation. Defaults to `DRIVE`. | No |
| **Units** | The unit system to use for display text. Defaults to `METRIC`. | No |
| **Traffic Model** | Specifies how live traffic is used to predict travel times. Only applies in `Driving` mode. Defaults to `BEST_GUESS`. | No |
| **X-Goog-FieldMask** | A comma-separated list of fields to include in the response. Use `*` to return all fields. Defaults to `*`. | No |
| **Origin Latitude** | The latitude of the origin. Can be used instead of an address. | No |
| **Origin Longitude** | The longitude of the origin. Can be used instead of an address. | No |
| **Destination Latitude** | The latitude of the destination. Can be used instead of an address. | No |
| **Destination Longitude** | The longitude of the destination. Can be used instead of an address. | No |
| **Departure Time** | The planned departure time in UTC format (e.g., `2025-09-20T14:00:00Z`). Cannot be used with `Arrival Time`. | No |
| **Arrival Time** | The desired arrival time in UTC format (e.g., `2025-09-20T16:00:00Z`). Cannot be used with `Departure Time`. | No |
| **Routing Preferences** | Determines how the route is calculated (e.g., with or without live traffic). Defaults to `TRAFFIC_AWARE_OPTIMAL`. | No |
| **Language Code** | The language code for localized output (e.g., `nl-NL`). Defaults to `en-US`. | No |
| **Return Alternative Routes** | Set to `true` to receive alternative routes for comparison. | No |
| **Route Modifiers** | Specify whether to avoid tolls, highways, or ferries. | No |
| **Extra Computations** | Request additional information like toll costs. Requires a matching `X-Goog-FieldMask`. | No |

#### Output Properties

| Property | Description | Example |
| --- | --- | --- |
| **distanceMeters** | The total travel distance in meters. | `115321` |
| **duration** | The estimated travel time in seconds, including current traffic. | `"4980s"` |
| **staticDuration** | The travel time in seconds without considering traffic conditions. | `"4620s"` |
| **description** | A short summary of the route. | `"Fastest route via A2"` |
| **localizedValues.distance.text** | The distance in a human-readable format, based on the selected units. | `"115 km"` |
| **localizedValues.duration.text** | The travel time in a human-readable format. | `"1 hour 23 mins"` |

---

## Usage Example in Power Automate

This example shows how to calculate the travel time between Amsterdam and Tilburg.

1.  **Trigger:** Start with a manual trigger (`Manually trigger a flow`).
2.  **Add Action:** Add the **Get distance and travel time** action from the Google Maps Routes connector.
3.  **Fill Parameters:**
    *   **Origin Address:** `Damrak 1, Amsterdam`
    *   **Destination Address:** `Spoorlaan 5, Tilburg`
    *   **Language Code:** `en-US`
4.  **Use Results:** Add a 'Compose' action to view the results. You can use dynamic content from the previous step.
    *   **Inputs:** `The estimated travel time is: @{body('Get_distance_and_travel_time')?['routes']?[0]?['localizedValues']?['duration']?['text']}`

When you run the flow, the 'Compose' action will contain the text "The estimated travel time is: 1 hour 23 mins" (or a similar value depending on traffic).

---

## Advanced Details

### `X-Goog-FieldMask` Presets

Use the `X-Goog-FieldMask` header to request only the data you need. This improves performance and reduces payload size.

| Preset Label | Value |
| --- | --- |
| Everything (all fields) | `*` |
| Distance only | `routes.distanceMeters` |
| Duration (traffic-aware) | `routes.duration` |
| Duration (no traffic) | `routes.staticDuration` |
| Distance (localized text) | `routes.localizedValues.distance.text` |
| Duration (localized text) | `routes.localizedValues.duration.text` |
| Minimal (fast) | `routes.distanceMeters,routes.duration` |
| Compare traffic impact | `routes.duration,routes.staticDuration` |
| Time + Distance + Summary | `routes.duration,routes.distanceMeters,routes.description` |
| Full (rich details) | `routes.duration,routes.staticDuration,...,routes.description,routes.warnings` |

> **Note:** If you use `Extra Computations` to request data like `TOLLS`, you must also add the corresponding fields (e.g., `routes.travelAdvisory.tollInfo`) to your FieldMask to receive the data.

### Error Handling

| HTTP Status | Meaning | Typical Causes | Recommended Handling |
| :---: | --- | --- | --- |
| `400` | Bad Request | Invalid/missing parameters, malformed FieldMask. | Validate inputs before calling the action, log error. |
| `403` | Forbidden | Invalid/expired API key, Routes API not enabled. | Check API key and ensure Routes API is enabled in your Google Cloud project. |
| `429` | Too Many Requests | Quota exceeded or rate limited by the API. | Use a retry policy in the action's settings (e.g., exponential backoff). |
| `5xx` | Server Error | Transient issues with the Google Maps Platform. | Use a retry policy in the action's settings. |

### Best Practices

*   **Use a minimal FieldMask:** Start with `routes.distanceMeters,routes.duration,routes.localizedValues` and only add fields as needed.
*   **Configure Retries:** In the action's settings in Power Automate, configure a retry policy for `429` and `5xx` errors.
*   **Validate Inputs:** Ensure addresses or coordinates are valid before calling the connector to prevent `400` errors.

### Known Issues and Limitations

*   The connector only supports a single origin and destination per call. Multi-stop itineraries require chaining multiple calls.
*   While the underlying API supports Place IDs, this connector's schema is optimized for **address** and **latitude/longitude** inputs.
*   Google Maps Platform quotas and billing apply. Ensure you monitor your usage in the Google Cloud Console.