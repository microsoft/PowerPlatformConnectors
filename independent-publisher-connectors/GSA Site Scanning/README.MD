# GSA Site Scanning

The GSA Site Scanning API provides detailed insights into the health, compliance, and performance of US federal websites through three main operations. These operations are designed to analyze website data thoroughly, retrieve website information in bulk or individually, and offer granular details for specific URLs. For a detailed understanding of how the GSA Site Scanning API works, and for additional endpoint details, refer to the official [GSA Site Scanning API documentation](https://open.gsa.gov/api/site-scanning-api/).

## Publisher: Richard Wilson

## Prerequisites

Before you can start using the GSA Site Scanning API, you'll need to obtain a free API key. This key will be used to authenticate your requests to the API. Obtaining the key is a straightforward process, which can be done through the GSA Site Scanning API website. Once you have the key, you'll be able to make API requests to access the site scanning data.

## Supported Operations

### Perform Website Analysis

Performs an in-depth analysis of websites, assessing various aspects such as compliance, performance, and accessibility.

- **Inputs**:
  - **Target URL Domain**: Specifies the starting domain (including TLD) for the scan.
  - **Final URL Domain**: The domain (including TLD) where the scan ends after following redirects.
  - **Final URL Live**: Indicates if the final URL is actively serving content.
  - **Target URL Redirects**: Whether the target URL initiates any redirects.
  - **Target URL Agency Owner**: The agency owning or operating the website.
  - **Target URL Bureau Owner**: The bureau owning or operating the website.
  - **Scan Status**: The status of the website scan (e.g., completed, timeout, dns_resolution_error).
  - **DAP Detected at Final URL**: Detects if the Digital Analytics Program (DAP) is present at the final URL.
- **Outputs**:
  - Comprehensive analysis results, including status, performance metrics, and compliance with digital standards.

### Retrieve Website Information

Fetches detailed information about websites, offering insights into target and final URLs, ownership, scan status, and more.

- **Inputs**:
  - **Target URL Domain**: The starting point domain for the scan.
  - **Final URL Domain**: The endpoint domain after redirects.
  - **Final URL Live**: Indicates if content is successfully served at the final URL.
  - **Target URL Redirects**: Whether the target URL is redirected to another URL.
  - **Target URL Agency Owner**: Identifies the agency managing the website.
  - **Target URL Bureau Owner**: Identifies the bureau managing the website.
  - **Scan Status**: Reflects the outcome of the website scan.
  - **DAP Detected at Final URL**: Shows if the Digital Analytics Program (DAP) is found at the final URL.
  - **Limit**: Specifies the number of results per page.
  - **Page**: Determines the page number of results to display.
- **Outputs**:
  - Paginated and detailed information on websites, including analytics and compliance data.

### Retrieve Website Information by URL

Provides specific information about a website based on its URL, including details on redirects, CMS, and analytics presence.

- **Inputs**:
  - **Website URL**: The exact URL of the website to analyze.
- **Outputs**:
  - Detailed data about the website, including analytics detection, compliance status, and technical specifications.

## Obtaining Credentials

To access the GSA Site Scanning API, an API key is required. You can obtain this key from the [GSA Site Scanning API website](https://open.gsa.gov/api/site-scanning-api/).

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
