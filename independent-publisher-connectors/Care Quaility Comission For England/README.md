# Title: Care Quality Comission Of England.

A connector to the API for the indenpendent regulator of health and social care in England.

## Publisher: Martyn Lesbirel

## Prerequisites
There are no prerequisites for the connector the connector.​

## Supported Operations

The connector supports a number of operations matching onto the various HTTP calls that can be made to the API. The following provides a description of each one. More full descriptions can be found a the CQC API site.​

### Locations
Retrieve a list of locations. Locations are sites where a Provider operates. Parameters may be added which filter the response so that it will only include organisations with attributes matching the required value(s). 

### Location Inspection Areas
Retrieve the list of Inspection Areas which have been inspected at this location excluding any that may have been globally retired (covering services no longer inspected by CQC). Although an Inspection area may be Superseded globally for the purpose of new inspections, it will remain active (along with the ratings for that area) at a given location until it has been re-inspected under the superseding inspection area(s).

### Providers
Retrieve the list of providers. Providers are organisations that carry out health and social care services, and are regulated by CQC. Filters may be added so that the response will only include organisations with attributes matching the required value(s). 

### Provider Locations
Retrieve the list of locations for this provider. Locations are sites where a Provider operates. Some Providers will have only one location, whereas others may have many.

### Provider Inspection Areas
Retrieve the list of Inspection Areas which have been inspected at this organisation excluding any that may have been globally retired (covering services no longer inspected by CQC). Although an Inspection area may be Superseded globally for the purpose of new inspections, it will remain active (along with the ratings for that area) at a given organisation until it has been re-inspected under the superseding inspection area(s).

### Changes
The Changes service provides a mechanism of staying informed about updates to providers and locations, to allow users to refresh their cached data if necessary. Changes for two organisation types are available: provider or location. Consumers must specify a start and end date range, see the help for more information. This service allows identification of which organisations have changed over time. For successive calls to this service, the startTime parameter should be set to the endTime of the previous call. The times correspond to the time that this service itself acquires the data and is not, for example, the time of data entry onto a source system.

### Inspection Areas
Get the global taxonomy of all CQC Inspection Areas including their current status for new inspections.

### Main Report
Retrieve a PDF file or its text content.

### Report And Related Doc
Retrieve a main inspection report and a document related to it.
This is the Related Document Type found from the relatedDocuments response object. e.g. "Use%20of%20Resources". When this parameter is absent, the main report for the inspection is returned.

## Obtaining Credentials
No credentials are required.

However the CQC does request that a named partner code be added to all requests. A default one has been set in the connector however if you are a CQC Syndication partner and value for this code has been supplied to you each operation should be modified so that the 'partnerCode' parameter passes the correct value.

## Known Issues and Limitations

### CQC Documentation Unclear on operations
The CQC appear to have duplicated API descriptions and examples on their web sites. I've reied to clarify this in the connedtor as far as possible.

### Multiple Values are not available
While using the HTTP API directly it allows multiple values to be searched at once such as follows 

`https://anypoint.mulesoft.com/mocking/api/v1/sources/exchange/assets/4d36bd23-127d-4acf-8903-ba292ea615d4/cqc-syndication-1/1.0.45/m/locations?region=North+West&region=South`

Where the region query parameter is repeated twice for each value. The MS Custom Connector adds the values as a list to the one query parameter causing the CQC API to fail.

These issues will be raised with CQC to seek answers and the connector updated as required.
