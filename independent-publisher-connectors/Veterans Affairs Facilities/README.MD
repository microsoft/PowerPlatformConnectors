# Veterans Affairs Facilities

The United States Department of Veterans Affairs (VA) Facilities service offers comprehensive details on VA health, benefits, cemeteries, and Vet Centers, including location, contact information, and services available. Data is sourced from both real-time and historical records, ensuring users have access to the latest facility updates.

## Publisher: Richard Wilson

## Prerequisites

To use this connector, you must obtain an API key after registering on the VA services website. This API key will be used to authenticate requests to the VA Facilities API.

## Supported Operations

### List facilities

Query facilities with a combination of optional search parameters.

- **Inputs**:

  | Name               | Description                                                                                           | Example                      |
  |--------------------|-------------------------------------------------------------------------------------------------------|------------------------------|
  | `Facility IDs`     | Comma separated list of facility IDs for the search.                                                  | `vha_688,vha_644`            |
  | `Zip Code`         | Zip code for searching facilities, considering only the first five digits.                            | `80301`                      |
  | `State`            | State for facility search, typically a two-character code.                                            | `CO`                         |
  | `Latitude`         | Latitude for facility search in WGS84 coordinates.                                                    | `56.7`                       |
  | `Longitude`        | Longitude for facility search in WGS84 coordinates.                                                   | `-123.4`                     |
  | `Radius`           | Radial distance from specified point to filter facility search, in miles.                             | `75`                         |
  | `Bounding Box`     | Bounding box coordinates for facility search within a geographic area.                                | `-105.4,39.4,-104.5,40.1`    |
  | `VISN`             | VISN (Veterans Integrated Service Networks) code for facility search.                                 | `20`                         |
  | `Facility Type`    | Type of facility location to filter search results.                                                   | `health`                     |
  | `Services Offered` | A comma separated list of services to filter facilities by the services they offer.                   | `primaryCare,mentalHealthCare` |
  | `Include Mobile`   | Flag to include or exclude mobile facilities in the search results.                                   | `True`                       |
  | `Page Number`      | Page number of results to return in a paginated response.                                             | `1`                          |
  | `Results Per Page` | Number of results per page in a paginated response.                                                   | `10`                         |

- **Outputs**:

  Returns a list of VA facilities matching the search criteria, including facility details such as name, location, and services offered.

### Get a facility by ID

Returns detailed information about a specific facility, identified by its unique ID.

- **Inputs**:

  | Name          | Description                                                                               | Example  |
  |---------------|-------------------------------------------------------------------------------------------|----------|
  | `Facility ID` | The unique identifier of the facility, following the format `<prefix>_<station>`.         | `vha_688`|

- **Outputs**:

  Detailed information about the specified VA facility, including services available, location details, and contact information.

### List services for a specific facility

Queries services offered by a facility, with optional parameters to refine results. Supports independent or combined use of `serviceIds` and `serviceType` for filtering.

- **Inputs**:

  | Name          | Description                                                                                           | Example         |
  |---------------|-------------------------------------------------------------------------------------------------------|-----------------|
  | `Facility ID` | Unique identifier of the facility, adhering to the format `<prefix>_<station>`.                       | `vha_688`       |
  | `Service IDs` | A comma separated list of service IDs to filter the search, usable with or without the `serviceType` parameter. | `covid19Vaccine` |
  | `Service Type`| Service type to filter the search, usable with or without the `serviceIds` parameter.                 | `health`        |

- **Outputs**:

  A list of services provided by the specified facility, including detailed descriptions and availability.

### Get details for a specific service at a facility

Retrieves detailed information about a specific service offered by a facility, identified by both facility ID and service ID.

- **Inputs**:

  | Name         | Description                                                                            | Example           |
  |--------------|----------------------------------------------------------------------------------------|-------------------|
  | `Facility ID`| The facility's unique identifier, structured as `<prefix>_<station>`.                  | `vha_688`         |
  | `Service ID` | The unique identifier for the specific service within the facility.                    | `covid19Vaccine`  |

- **Outputs**:

  Detailed information about the specified service at the given facility, including contact information, service hours, and appointment scheduling details.

### List all facility IDs

Provides a bulk download option for all facility IDs, with the capability to filter by facility type for more targeted results.

- **Inputs**:

  | Name          | Description                                          | Example        |
  |---------------|------------------------------------------------------|----------------|
  | `Facility Type`| Filter for searching by facility type, such as health, cemetery, benefits, or vet center. | `health` |

- **Outputs**:

  A list of all facility IDs, optionally filtered by facility type.

### List all nearby facilities

Retrieves all facilities within a specified drive time from a given location, with optional filtering by services.

- **Inputs**:

  | Name              | Description                                                                                          | Example     |
  |-------------------|------------------------------------------------------------------------------------------------------|-------------|
  | `Latitude`        | Latitude from which drive time is calculated.                                                        | `38.8977`   |
  | `Longitude`       | Longitude from which drive time is calculated.                                                       | `-77.0365`  |
  | `Drive Time`      | Maximum drive time in minutes from the location to include facilities.                               | `60`        |
  | `Services Filter` | An optional comma separated list of service to filter by.                                           | `primaryCare,mentalHealthCare` |
  | `Page Number`     | The page number of results to return.                                                                | `1`         |
  | `Results Per Page`| The number of results to return per page.                                                            | `10`        |

- **Outputs**:

  A list of nearby VA health facilities within the specified drive time from the given location, optionally filtered by services offered.

## Obtaining Credentials

To obtain credentials for accessing the VA Facilities API, you must register on the VA services website and request an API key. This key will be used in the header of your API requests for authentication.

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
