# GSA Analytics.usa.gov

Provides access to data from Analytics.usa.gov, which is a unified Google Analytics account for U.S. federal government agencies. This connector for Power Automate enables users to integrate web analytics data into their workflows and applications. For a detailed understanding of how the GSA Analytics.usa.gov API works, and for additional endpoint details, refer to the official [GSA Analytics.usa.gov API documentation](https://open.gsa.gov/api/dap/).

## Publisher: Richard Wilson

## Prerequisites

Before you can start using the GSA Analytics.usa.gov API, you'll need to obtain a free API key. This key will be used to authenticate your requests to the API. Obtaining the key is a straightforward process, which can be done through the GSA Analytics.usa.gov API website. Once you have the key, you'll be able to make API requests to access the analytics data.

## Supported Operations

### Get Report Data

Retrieves data from a specific report in the DAP.

- **Inputs**:
  - `report_name`: The name of the report.
  - `limit`: The number of records to return (optional).
  - `page`: The page number to return (optional).
  - `after`: Limit the results to data with dates on or after the specified date (optional).
  - `before`: Limit the results to data with dates on or before the specified date (optional).
- **Outputs**:
  - `Reports`: Detailed report data for the specified report.
  - `Errors`: Any errors encountered during the request.

### Get Agency Report Data

Retrieves data from a specific report for a specific agency in the DAP.

- **Inputs**:
  - `agency_name`: The name of the agency.
  - `report_name`: The name of the report.
  - `limit`: The number of records to return (optional).
  - `page`: The page number to return (optional).
  - `after`: Limit the results to data with dates on or after the specified date (optional).
  - `before`: Limit the results to data with dates on or before the specified date (optional).
- **Outputs**:
  - `Reports`: Detailed report data for the specified agency and report.
  - `Errors`: Any errors encountered during the request.

### Get Domain Report Data

Retrieves data from a specific report for a specific domain in the DAP.

- **Inputs**:
  - `domain`: The domain name.
  - `report_name`: The name of the report.
  - `limit`: The number of records to return (optional).
  - `page`: The page number to return (optional).
  - `after`: Limit the results to data with dates on or after the specified date (optional).
  - `before`: Limit the results to data with dates on or before the specified date (optional).
- **Outputs**:
  - `Reports`: Detailed report data for the specified domain and report.
  - `Errors`: Any errors encountered during the request.

## Obtaining Credentials

To access the GSA Analytics.usa.gov API, an API key is required. You can obtain this key from the [GSA Analytics.usa.gov API website](https://open.gsa.gov/api/dap/).

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
