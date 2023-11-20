# GSA Per Diem

Provides access to the U.S. General Services Administration's per diem rate data, which includes lodging, meal, and incidental expense rates for federal travelers. This connector for Power Automate enables users to integrate these per diem rates into their workflows and applications. For a detailed understanding of how the GSA Per Diem API works, and for additional endpoint details, refer to the official [GSA Per Diem API documentation](https://open.gsa.gov/api/perdiem/).

## Publisher: Richard Wilson

## Prerequisites

Before you can start using the GSA Per Diem API, you'll need to obtain a free API key. This key will be used to authenticate your requests to the API. Obtaining the key is a straightforward process, which can be done through the GSA Per Diem API website. Once you have the key, you'll be able to make API requests to access the per diem rate data.

## Supported Operations

### Get Per Diem Rates by City, State, and Year

Retrieves specific per diem rates based on the city, state, and fiscal year.

- **Inputs**:
  - `City`: The name of the city.
  - `State Abbreviation`: The two-letter abbreviation of the state.
  - `Year`: The fiscal year for which rates are requested.
- **Outputs**:
  - `Rates`: Detailed per diem rate information for the specified location and year.
  - `Errors`: Any errors encountered during the request.

### Get Per Diem Rates for All Counties

Provides comprehensive per diem rates across all counties and cities within a specified state for a given fiscal year.

- **Inputs**:
  - `State Abbreviation`: The two-letter abbreviation of the state.
  - `Year`: The fiscal year for which rates are requested.
- **Outputs**:
  - `Rates`: An array of per diem rate information for all counties and cities within the specified state and year.
  - `Errors`: Any errors encountered during the request.

### Get Per Diem Rates by ZIP Code and Year

Fetches per diem rates for a specific geographic area identified by ZIP code for a given fiscal year.

- **Inputs**:
  - `Zip`: ZIP code of the area.
  - `Year`: The fiscal year for which rates are requested.
- **Outputs**:
  - `Rates`: Per diem rate information based on the specified ZIP code and fiscal year.
  - `Errors`: Any errors encountered during the request.

### Lodging Rates for the Continental US by Year

Offers detailed lodging rate information for various locations within the Continental United States for the specified fiscal year.

- **Inputs**:
  - `Year`: The fiscal year for which rates are requested.
- **Outputs**:
  - An array of lodging rates for each month, along with overall meal rates, city, state, and county details.

### Mapping of ZIP Code to Destination-IDs and State Locations

Provides a mapping of ZIP codes to their corresponding Destination-IDs and state locations for a given fiscal year.

- **Inputs**:
  - `Year`: The fiscal year for which the mapping is requested.
- **Outputs**:
  - A list of ZIP codes with their corresponding Destination-IDs and state locations for the specified fiscal year.

## Obtaining Credentials

To access the GSA Per Diem API, an API key is required. You can obtain this key from the [GSA Per Diem API website](https://open.gsa.gov/api/perdiem/).

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
