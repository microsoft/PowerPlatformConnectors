# Veterans Affairs Providers

The United States Department of Veterans Affairs (VA) Provider Directory API delivers extensive information on VA healthcare providers, encompassing provider locations, specialties, office hours, and availability for new patients. Utilizing both real-time and historical data, this service ensures access to up-to-date details about VA healthcare facilities.

## Publisher: Richard Wilson

## Prerequisites

To use this connector, you must obtain an API key after registering on the VA services website. This API key will be used to authenticate requests to the VA Facilities API.

## Supported Operations

### List Locations

Retrieves a list of locations based on specified criteria, including provider locations, specialties, and office hours.

- **Inputs**:
  - `Id`: The logical ID of the resource. Once assigned, this value never changes. It is used to identify specific locations.
  - `Identifier`: Another form of identification for the location, distinct from the logical ID.
  - `Address`: The physical address of the location.
  - `Address City`: The city where the location is based.
  - `Address State`: The state where the location is situated.
  - `Address Postalcode`: The postal code of the location's area.
  - `Name`: A human-readable name for the location.
  - `Last Updated`: The last date when the location information was updated.
  - `Page`: Pagination option to specify the page number of the search result.
  - `Count`: Specifies the number of records returned in a single page, with a maximum of 100.
- **Outputs**:
  - Provides a JSON list of locations that match the search criteria.

### Get Location by ID

Retrieves detailed information about a specific location by its ID.

- **Inputs**:
  - `Id`: The unique identifier for the location. This is required to fetch specific location details.
- **Outputs**:
  - Returns detailed JSON formatted information about the location specified by the ID.

### List Organizations

Retrieves a list of organizations based on specified criteria, including their locations, names, and other identifying information.

- **Inputs**:
  - `Id`: The logical ID of the organization. Used to identify specific organizations.
  - `Identifier`: A unique identifier for the organization, different from the logical ID.
  - `Address`: The physical address of the organization.
  - `City`: The city where the organization is based.
  - `State`: The state where the organization is located.
  - `Postal Code`: The postal code for the organization's location.
  - `Name`: The human-readable name of the organization.
  - `Last Updated`: The last date when the organization's information was updated.
  - `Page`: Specifies the page number for pagination of the search results.
  - `Count`: Specifies the number of records to return in a single page, up to a maximum of 100.
- **Outputs**:
  - Outputs a JSON list of organizations that match the specified search criteria.

### Get Organization by ID

Retrieves detailed information about a specific organization by its ID.

- **Inputs**:
  - `Id`: The unique identifier for the organization. Required to fetch specific organization details.
- **Outputs**:
  - Provides detailed JSON formatted information about the specified organization.

### List Practitioners

Retrieves a list of practitioners based on specified criteria, including identifiers, names, and last update dates.

- **Inputs**:
  - `Id`: The logical ID for the practitioner. It is used to identify specific practitioners.
  - `Identifier`: A unique identifier for the practitioner, separate from the logical ID.
  - `Family Name`: The family name or surname of the practitioner.
  - `Given Name`: The given or first name of the practitioner.
  - `Name`: A name that could be either the given name or family name.
  - `Last Updated`: The last date when the practitioner's information was updated.
  - `Page`: For specifying the page number in the search results for pagination.
  - `Count`: The number of records to return on a single page, with a cap of 100.
- **Outputs**:
  - Returns a JSON list of practitioners matching the search criteria.

### Get Practitioner by ID

Retrieves detailed information about a specific practitioner by their unique ID.

- **Inputs**:
  - `Id`: The unique identifier for the practitioner, necessary to retrieve detailed information.
- **Outputs**:
  - Provides detailed information about the practitioner in JSON format.

### List Practitioner Roles

Retrieves a list of practitioner roles based on specified criteria, including identifiers and names, reflecting their roles and responsibilities.

- **Inputs**:
  - `Id`: The logical ID of the practitioner role. Identifies specific practitioner roles.
  - `Practitioner Identifier`: A unique identifier for a practitioner within a given system.
  - `Practitioner Name`: The name of the practitioner, which could be a given name, family name, or both.
  - `Last Updated`: Indicates the last date the practitioner role information was updated.
  - `Page`: The page number being requested for pagination purposes.
  - `Count`: The number of resources to return in a single page, up to a maximum of 100.
- **Outputs**:
  - Outputs a JSON list of practitioner roles that meet the specified criteria.

### Get Practitioner Role by ID

Retrieves detailed information about a specific practitioner role by its unique ID.

- **Inputs**:
  - `Id`: The unique identifier for the practitioner role, required for fetching detailed information.
- **Outputs**:
  - Returns detailed JSON formatted information about the specified practitioner role.

## Obtaining Credentials

To obtain credentials for accessing the VA Facilities API, you must register on the VA services website and request an API key. This key will be used in the header of your API requests for authentication.

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
