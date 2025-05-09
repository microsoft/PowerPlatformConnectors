# TALXIS Data Feed
TALXIS Data Feed is a collection of different data sources. You can get company's business data, format address, or get public holidays for example. Use this connector to improve your data quality.

## Publisher: TALXIS
This connector is managed and published by [NETWORG](https://www.networg.com/) under the product name [TALXIS](https://www.talxis.com/).

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* TALXIS license

You will not be able to use this connector without TALXIS license. Contact NETWORG if you wish to get this license.

## Supported Operations
### Get Company Logo
Returns company logo if available. Supported jurisdiction codes: CZ, SK.

### Get Company
Returns company detail using company number.

### Get Company Finace Report
Returns company finance data. Extra API key is required for this.

### Get Week Number From Date
Returns week number based on provided criteria.

### Geocode Address
Returns geocoded address.

### Get Google Maps API Key
Returns new short lived key.

### Get Salutation
Returns salutation.

### Get Holidays
Returns list of holidays for a given state and year

## Obtaining Credentials
Users will be asked to provide their credentials when using this connector. Standard OAuth 2.0 implicit grant flow is used through `TALXIS - Data Feed - Flow` Enterprise Application registration.
This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access TALXIS Data Feed API on behalf of the end user. You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios).

## Known Issues and Limitations
No limitations.

## Deployment Instructions
This connector is fully managed and part of the certified connector's catalogue. If you wish to use it, find it in the list of the available connectors. However, it's required to approve `TALXIS - Data Feed - Flow` Enterprise Application registration.