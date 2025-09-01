# WMATA

The Washington Metropolitan Area Transit Authority (WMATA) connector provides access to various services of WMATA, including real-time data on bus and train predictions, incidents, elevator outages, GTFS data, and more. This connector facilitates seamless integration with WMATA's data to enhance applications with accurate transit information.

## Publisher: Richard Wilson, Daniel Cox

## Prerequisites

To use this connector, you must obtain an API key by registering on the WMATA Developer Portal. This API key will be used to authenticate requests to the WMATA API.

## Supported Operations

### Get next bus arrival times

Retrieve the next bus arrival times at a specified stop.

- **Inputs**:

  | Name   | Description                     | Example   |
  |--------|---------------------------------|------------|
  | StopID | 7-digit regional stop ID.       | `1001195`  |

- **Outputs**:

  Returns the stop name and a list of bus arrival predictions including direction, minutes to arrival, route ID, and vehicle ID.

### Get bus positions

Retrieve the positions of buses for the given route, with an optional search radius.

- **Inputs**:

  | Name   | Description                                               | Example |
  |--------|-----------------------------------------------------------|---------|
  | RouteID | Base bus route, e.g., `70`, `10A`.                        | `70`    |
  | Lat    | Center point latitude, required if Longitude and Radius are specified. | `38.8951` |
  | Lon    | Center point longitude, required if Latitude and Radius are specified. | `-77.0364` |
  | Radius | Radius (meters) to include in the search area.            | `1000`  |

- **Outputs**:

  Returns a list of bus positions including last update time, schedule deviation, direction, latitude, longitude, and trip details.

### Get bus route details

Retrieve a set of ordered latitude/longitude points along a route variant along with the list of stops served.

- **Inputs**:

  | Name    | Description                                 | Example |
  |---------|---------------------------------------------|---------|
  | RouteID | Bus route variant, e.g., `70`, `10A`, `10Av1`. | `70`    |
  | Date    | Date in YYYY-MM-DD format for which to retrieve route and stop information. Defaults to today's date if not specified. | `2024-08-14` |

- **Outputs**:

  Returns route details including direction information, route shape, and stop details.

### Get bus routes

Retrieve a list of bus routes and their variants.

- **Outputs**:

  Returns a list of bus routes including route ID, name, and line description.

### Get bus route schedule

Retrieve the bus schedule for a specific route and date.

- **Inputs**:

  | Name    | Description                          | Example |
  |---------|--------------------------------------|---------|
  | RouteID | Bus route identifier.                | `70`    |
  | Date    | Date in YYYY-MM-DD format. Defaults to today if not specified. | `2024-08-14` |

- **Outputs**:

  Returns the schedule for the specified route, including trip ID, start time, end time, and stops along the route.

### Get bus stop schedule

Retrieve the bus schedule for a specific stop and date.

- **Inputs**:

  | Name    | Description                          | Example |
  |---------|--------------------------------------|---------|
  | StopID  | Bus stop identifier.                 | `1001195`|
  | Date    | Date in YYYY-MM-DD format. Defaults to today if not specified. | `2024-08-14` |

- **Outputs**:

  Returns the schedule for the specified stop, including route ID, trip ID, start time, and end time.

### Get bus stops

Retrieve a list of bus stops for a specific route.

- **Inputs**:

  | Name    | Description                          | Example |
  |---------|--------------------------------------|---------|
  | RouteID | Bus route identifier.                | `70`    |

- **Outputs**:

  Returns a list of stops along the specified route, including stop ID, name, latitude, and longitude.

### Get next train arrival times

Retrieve the next train arrival information for one or more stations.

- **Inputs**:

  | Name         | Description                                   | Example |
  |--------------|-----------------------------------------------|---------|
  | StationCodes | Comma-separated list of station codes. Use `All` for all predictions. | `B03`  |

- **Outputs**:

  Returns a list of trains arriving at the station, including details like train cars, destination, track group, line, arrival station code, and minutes to arrival.

### Get rail lines

Retrieve information about all rail lines.

- **Outputs**:

  Returns a list of rail lines including line code, display name, and station codes.

### Get rail station list

Retrieve a list of station locations and their address information based on a given LineCode. If no LineCode is provided, all stations are returned.

- **Inputs**:

  | Name     | Description                                                                                   | Example |
  |----------|-----------------------------------------------------------------------------------------------|---------|
  | LineCode | Two-letter line code abbreviation: RD - Red, YL - Yellow, GR - Green, BL - Blue, OR - Orange, SV - Silver. | `RD`    |

- **Outputs**:

  Returns a list of stations including address, latitude, longitude, station codes, and line codes. 

### Get rail station parking information

Retrieve parking information at a station based on a given StationCode.

- **Inputs**:

  | Name        | Description                            | Example |
  |-------------|----------------------------------------|---------|
  | StationCode | Station code for which to retrieve parking information. | `A01`   |

- **Outputs**:

  Returns parking details including total parking spots, rider and non-rider costs, and short-term parking information.

### Get path between rail stations

Retrieve a set of ordered stations and distances between two stations on the same line.

- **Inputs**:

  | Name           | Description                     | Example |
  |----------------|---------------------------------|---------|
  | FromStationCode | Station code for the origin station. | `A01`   |
  | ToStationCode  | Station code for the destination station. | `B02`   |

- **Outputs**:

  Returns path details including station sequence, distance between stations, and line information.

### Get rail station entrances

Retrieve a list of nearby station entrances based on latitude, longitude, and radius.

- **Inputs**:

  | Name   | Description                                      | Example   |
  |--------|--------------------------------------------------|-----------|
  | Lat    | Center point latitude.                           | `38.8951` |
  | Lon    | Center point longitude.                          | `-77.0364`|
  | Radius | Radius (meters) to include in the search area.   | `500`     |

- **Outputs**:

  Returns a list of station entrances including name, latitude, longitude, and associated station codes.

### Get rail station information

Retrieve station location and address information based on a given StationCode.

- **Inputs**:

  | Name        | Description                            | Example |
  |-------------|----------------------------------------|---------|
  | StationCode | Station code for which to retrieve information. | `A01`   |

- **Outputs**:

  Returns station information including address, latitude, longitude, station codes, and line codes.

### Get rail station times

Retrieve opening and scheduled first/last train times for a given StationCode.

- **Inputs**:

  | Name        | Description                            | Example |
  |-------------|----------------------------------------|---------|
  | StationCode | Station code for which to retrieve times. | `A01`   |

- **Outputs**:

  Returns station opening times and first/last train times.

### Get rail station to rail station information

Retrieve travel time, distance, and fare information between two stations.

- **Inputs**:

  | Name           | Description                     | Example |
  |----------------|---------------------------------|---------|
  | FromStationCode | Station code for the origin station. | `A01`   |
  | ToStationCode  | Station code for the destination station. | `B02`   |

- **Outputs**:

  Returns travel time, distance, and fare information between the two stations.

### Get live train positions

Retrieve uniquely identifiable trains in service and the track circuits they currently occupy.

- **Inputs**:

  | Name        | Description                            | Example |
  |-------------|----------------------------------------|---------|
  | contentType | Response format (json or xml).          | `json`  |

- **Outputs**:

  Returns a list of train positions including train ID, train number, car count, circuit ID, and line code.

### Get standard train routes

Retrieve an ordered list of mostly revenue (and some lead) track circuits, arranged by line and track number.

- **Inputs**:

  | Name        | Description                            | Example |
  |-------------|----------------------------------------|---------|
  | contentType | Response format (json or xml).          | `json`  |

- **Outputs**:

  Returns a list of standard routes including line code and track circuits.

### Get track circuits

Retrieve a list of all track circuits, including those on pocket tracks and crossovers.

- **Inputs**:

  | Name        | Description                            | Example |
  |-------------|----------------------------------------|---------|
  | contentType | Response format (json or xml).          | `json`  |

- **Outputs**:

  Returns a list of track circuits including circuit ID, track number, and neighboring circuits.

### Get bus incidents

Retrieve reported bus incidents or delays for a given route.

- **Inputs**:

  | Name  | Description                                | Example |
  |-------|--------------------------------------------|---------|
  | Route | Base bus route.                            | `C2`    |

- **Outputs**:

  Returns a list of bus incidents including the date updated, incident description, incident type, and routes affected.

### Get elevator and escalator outages

Retrieve a list of reported elevator and escalator outages at a given station.

- **Inputs**:

  | Name        | Description                            | Example |
  |-------------|----------------------------------------|---------|
  | StationCode | Station code for which to retrieve outages. | `A01`   |

- **Outputs**:

  Returns a list of elevator and escalator incidents including date out of service, estimated return to service, location description, and unit type.

### Get rail incidents

Retrieve reported rail incidents, including significant disruptions and delays to normal service.

- **Outputs**:

  Returns a list of rail incidents including date updated, incident description, incident type, and lines affected.

### Get bus GTFS static

Retrieve the GTFS static data file for WMATA bus.

- **Outputs**:

  Returns the GTFS static data file as a ZIP.

### Get bus GTFS RT alerts

Retrieve the GTFS RT service alerts feed for WMATA bus.

- **Outputs**:

  Returns the GTFS RT service alerts feed as a binary file.

### Get bus GTFS RT trip updates

Retrieve the GTFS RT trip updates feed for WMATA bus.

- **Outputs**:

  Returns the GTFS RT trip updates feed as a binary file.

### Get bus GTFS RT vehicle positions

Retrieve the GTFS RT vehicle positions feed for WMATA bus.

- **Outputs**:

  Returns the GTFS RT vehicle positions feed as a binary file.

### Get rail GTFS static

Retrieve the GTFS static data file for WMATA rail, updated daily.

- **Outputs**:

  Returns the GTFS static data file as a ZIP.

### Get rail GTFS RT alerts

Retrieve the GTFS RT service alerts feed for WMATA rail.

- **Outputs**:

  Returns the GTFS RT service alerts feed as a binary file.

### Get rail GTFS RT trip updates

Retrieve the GTFS RT trip updates feed for WMATA rail.

- **Outputs**:

  Returns the GTFS RT trip updates feed as a binary file.

### Get rail GTFS RT vehicle positions

Retrieve the GTFS RT vehicle positions feed for WMATA rail.

- **Outputs**:

  Returns the GTFS RT vehicle positions feed as a binary file.

### Get rail and bus combined GTFS static

Retrieve the GTFS static data file for WMATA rail and bus combined, updated daily.

- **Outputs**:

  Returns the combined GTFS static data file as a ZIP.

## Obtaining Credentials

To obtain credentials for accessing the WMATA API, you must register on the WMATA Developer Portal and [request an API key](https://developer.wmata.com/signup). This key will be used in the header of your API requests for authentication.

After completing the sign-in process, follow these steps to obtain your API key:

1. **Create a New Subscription**:
   - Navigate to the "Products" tab in the WMATA Developer Portal.
   - Select a subscription level based on your needs. For most users, the 'Default Tier' will be appropriate.
   - Create a new subscription under the selected product.

2. **Access Your API Key**:
   - Once your subscription is created, it will be available in your Profile area of the Developer Portal.
   - Navigate to your Profile, where you will find your API key ready for use in your API requests.

This API key must be included in the header of all your API requests to authenticate with the WMATA services.

## Known Issues and Limitations

Currently, no known issues or limitations exist. Please refer to the WMATA Developer Portal for the latest updates.
