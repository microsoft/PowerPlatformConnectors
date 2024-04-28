# AlisQI

Integrate your QMS with your ERP, MES, PLM, instruments, sensors or website using AlisQI's connectivity tools.

## Publisher: AlisQI B.V.

## Prerequisites
In order to use this connector, you will need the following:

* An account with AlisQI.
* Once you've signed up and you have an application, get the API Key from the online portal; you will use this key to authenticate your requests from our AlisQI connector.

## Supported Operations
The connector supports the following operations:

* Get results or master data `/getResults` : Get (quality) results from analysis sets or master data from selection lists.

* Get all analysis sets or selection lists `/getAllSets`: Get a list of (all)  analysis sets or selection lists.

* Create or update results or master data `/storeResults`: Create or update quality results in analysis sets or master data in selection lists.

* Get descriptive statistics for analysis set fields `/getStatistics`: Get descriptive statistics for numeric analysis set fields of your choice.

* Get capability statistics for analysis set fields `/getSpecificationEvaluation`: Get in-depth statistics of specification evaluation and product and process capability for an analysis set's numeric fields.

* Get specification versions `/getSpecificationVersion`: Get (a) specification version(s) for an analysis set.

* Create a new specification version `/storeSpecificationVersion`: Create (a) new specification version(s) for an analysis sets.

* Get the structure of an analysis sets or selection list `/getSetDefinition`: Get the structural definition of an analysis sets or selection lists.

## Obtaining Credentials

* All API calls are subject to authentication. In order to invoke the AlisQI API endpoints, developers need to create an authentication token.

* The API supports the [Bearer authentication specification](https://datatracker.ietf.org/doc/html/rfc6750).

* Each token is connected to a single user. The Read, Insert and Update permission this user has for analysis sets (via its user group), also apply to API invocations. [More information on the user and permission management in AlisQI](https://help.alisqi.com/article/362-user-management).

* API authentication tokens can be created in the in-app [integration hub](https://help.alisqi.com/article/478-integration-hub). We recommend using dedicated tokens per integration scenario.

* Alternatively, if the Bearer authentication method is unavailable, requests can be authenticated using the accessToken URL parameter `&accessToken=abc...123`.

## Known Issues and Limitations

* Filters: Operations with a filter parameter expect a JSON string that represents the filter. Use the in-app [integration hub](https://help.alisqi.com/article/478-integration-hub) to build a filter and copy the JSON string.

* Dates: All dates and times are in UTC (formatted as `yyyy-mm-dd hh:mm:ss`), and all strings are encoded using UTF-8.

* Caching: The [getResults](https://alisqi.readme.io/reference/get-results) operation supports cache validation using ETag and Last-Modified headers. We recommend enabling caching on clients and making conditional requests to reduce latency, server load and energy usage.

* Usage limits: Previously we have seen performance degradation due to overloading the API. To safeguard stability of our platform usage limits apply to the API. These limits are designed to prevent undesired loads on our systems from misconfigured API integrations or abuse.
The rate limit defaults 30 calls per minute. This applies to both read and write operations.
Requests that hit the rate limit will have a status code of `429 Too many requests`.

All responses include the following headers with explicit numbers on the limits and remaining calls: 

* X-RateLimit-Limit  
* X-RateLimit-Remaining  
* X-RateLimit-Retry-After

To debug problems in your integration scenarios that are or can be caused by rate limits, please go through this checklist:

* Consult your integration specialist to inspect the API calls and return headers.
* Open your AlisQI integration hub to learn about the concrete rate limit applied to your installation.
* Inspect the API log in the AlisQI integration hub, to determine the number of requests per minute.
* Inspect the API request return headers, to see if they contain the `429 Too many requests` status code.
* Inspect individual API request return headers to assess the `X-RateLimit-* headers`.
* Assess whether you can improve the integration by reducing duplicate requests, or by combining multiple results in a single storeResults call. Feel free to reach out to AlisQI support to discuss the capabilities of our API regarding caching and processing multiple results.
* If your integration scenario is optimized but legitimately requires more calls per minutes, please reach out to AlisQI support to discuss possibilities to raise the limit.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps.