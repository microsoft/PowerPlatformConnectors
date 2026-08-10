# Tomorrow.io Weather

[Tomorrow.io](https://www.tomorrow.io/) is an enterprise-grade weather intelligence service. This connector brings the Tomorrow.io Weather API into Power Automate, Power Apps, Logic Apps, and Copilot Studio, so you can pull hyper-local weather data into your flows and apps.

The connector exposes six actions. Two run on any Tomorrow.io API key (usage-capped); the other four require a paid Tomorrow.io plan and return HTTP 403 on a free key.

## Actions

| Action | Plan | Endpoint |
|---|---|---|
| Get timeline | Free (usage-capped) | `POST /v4/timelines` |
| Get on-demand weather events | Free (usage-capped) | `POST /v4/events` |
| Get historical weather (paid plan) | Paid | `POST /v4/historical` |
| Get climate normals (paid plan) | Paid | `POST /v4/historical/normals` |
| Get weather on route (paid plan) | Paid | `POST /v4/route` |
| Get on-demand events on routes (paid plan) | Paid | `POST /v4/events-timeline/routes` |

## Publisher

Adam Recanati (Independent Publisher)

## Prerequisites

You need a Tomorrow.io API key. Create a free or paid key in the [Tomorrow.io developer console](https://app.tomorrow.io/development/keys).

## Obtaining credentials

1. Sign up at [tomorrow.io](https://www.tomorrow.io/weather-api/).
2. Open the [developer console](https://app.tomorrow.io/development/keys) and copy your API key.
3. When you create a connection, paste the key into the **API Key** field. It is sent as the `apikey` query-string parameter on every request.

## Supported operations

### Get timeline
The flexible Timelines feed: request any data-layer fields over any time range — past, present, or future — at the timestep you choose. Works on a free key (usage-capped).

### Get on-demand weather events
Evaluate active and upcoming weather events (severe-weather advisories and custom insights) for a location, on demand. Works on a free key (usage-capped).

### Get historical weather (paid plan)
Observed historical weather over an arbitrary past date range. Data lags roughly six days behind now.

### Get climate normals (paid plan)
Long-term climate averages for a location across a date range (data aggregated from 2000–2020).

### Get weather on route (paid plan)
Forecast conditions at each waypoint along a route.

### Get on-demand events on routes (paid plan)
Monitor weather-based insights along a route — evaluated separately at the origin, the destination, and en route.

## Known issues and limitations

- The four **(paid plan)** actions return HTTP 403 on a free API key. Use *Get timeline* and *Get on-demand weather events* on a free key.
- The two free actions are usage-capped; heavy use requires a paid plan.
- *Get historical weather* covers dates up to roughly six days before now; for more recent data use *Get timeline*.
- Location for *Get on-demand weather events* and the route actions is a GeoJSON geometry in `[longitude, latitude]` order, not a `"lat,lon"` string.

## Frequently asked questions

**Which actions work on a free API key?**
*Get timeline* and *Get on-demand weather events* (both usage-capped). The rest need a paid Tomorrow.io plan.

**Where do I get an API key?**
From the [Tomorrow.io developer console](https://app.tomorrow.io/development/keys).

## Support

For issues with this connector, contact the publisher at adam.recanati@tomorrow.io. For Tomorrow.io API questions, see the [API documentation](https://docs.tomorrow.io/reference/) or [Tomorrow.io support](https://www.tomorrow.io/).
