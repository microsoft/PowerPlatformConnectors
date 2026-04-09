# FDIC (Independent Publisher)
The Federal Deposit Insurance Corporation (FDIC) provides access to financial institution data, including bank financials, institution details, failure history, and location data.

## Publisher: Dan Romano (swolcat)
Required. For Independent-Publisher-Connector, it should be a first and last name of an individual or it can be a company name. ​If there is more than one publisher, please separate the names with a comma.​

## Prerequisites

No authentication is required. The FDIC API is publicly accessible.

## Supported Operations

### Get Institutions

Retrieve a list of FDIC-insured institutions with filtering options such as name, state, asset size, and status. Retrieve detailed information for a specific institution using its FDIC certificate number.

### Get Financial Data

Retrieve financial metrics for institutions including assets, liabilities, income, and capital ratios. Retrieve financial data for a specific institution over time.

### Get Failures

Retrieve a list of failed financial institutions with failure dates, acquiring institutions, and loss estimates. Retrieve detailed information about a specific bank failure.

### Get Locations

Retrieve branch and office location data for FDIC-insured institutions. Retrieve all branch locations associated with a specific institution.

### Get Summary Statistics

Retrieve aggregated statistics across institutions, such as total assets, deposits, and counts.

### Get Historical Institution Data

Retrieve historical snapshots of institution data across reporting periods.

## Obtaining Credentials

Required. Explain the authentication method and how to get the credentials.​

## Getting Started

No authentication is required. The FDIC API is publicly accessible.

1.) Download the connector files.
2.) Import the connector into your Power Platform environment.
3.) Create a connection (no authentication required).
4.) Use the connector in Power Apps or Power Automate

## Known Issues and Limitations

- The FDIC API may return large datasets; pagination and filtering are recommended for performance.
- Some endpoints may have inconsistent field availability depending on reporting period.
- Response schemas are normalized for Power Platform compatibility; raw API responses may differ slightly.

## Frequently Asked Questions

### Does this connector support real-time data?

Data availability depends on FDIC reporting cycles and may not reflect real-time changes.

### Are all FDIC datasets included?

This connector focuses on the most commonly used datasets (institutions, financials, failures, locations, and summaries).

## Deployment Instructions

1.) Download the connector files (apiDefinition.swagger.json, apiProperties.json, and readme.md)
2.) Import the connector into your Power Platform environment
3.) Create a connection (no authentication required)
4.) Use the connector in Power Apps or Power Automate