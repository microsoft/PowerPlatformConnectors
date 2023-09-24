# Title
The **Celonis Intelligence API** allow Celonis customers to embed actionable process intelligence into third-party platforms where end-users may benefit from Celonis process insights. In this manner, we would bring Celonis intelligence like process KPIs or process inefficiencies closer to our end-users and benefit operational users without current access to Celonis by providing valuable insights for better decision-making while executing their day-to-day activities.
## Publisher: Publisher's Name
[Celonis](https://www.celonis.com/) ​
## Prerequisites
1.  A team in Celonis EMS.
2.  A  [Knowledge Model](https://docs.celonis.com/en/knowledge-model.html)  available in your team.
3.  Premium EMS Subscription or the new process-based EMS Subscription (contact your account team to verify).
## Supported Operations
The current API methods allow consumers to interact with Knowledge Models defined in the EMS by providing the following functionalities:
### Knowledge Model Discovery Methods
####   [List of Knowledge Models (KM)](https://developer.celonis.com/openapi/reference/operation/getKnowledgeModel/)
####   [Get a list of KM Records](https://developer.celonis.com/openapi/reference/operation/getRecords/)
####   [Get KM Record Schema Details](https://developer.celonis.com/openapi/reference/operation/getRecordDetails/)
####   [Get a list of KM Filters](https://developer.celonis.com/openapi/reference/operation/getFilters/)
### Knowledge Model Data Retrieval Methods
####   [Get a list of KM Record data](https://developer.celonis.com/openapi/reference/operation/getRecordDataResult/)
## Obtaining Credentials
### Authentication
Each request to the API must be authenticated with a Celonis EMS API key. There are two ways of doing this:
#### Using a User API key
You can find out how to create an user API key by following our  [User API Keys](https://docs.celonis.com/en/user-profile.html)  guide.

The Celonis API uses Bearer Token Authentication for verifying consumer access. The credentials must be sent in an Authorization header in the HTTP request. Credentials sent in the URL or body of the request will be ignored.

To authenticate using Bearer Token Authentication:

1.  Create the token in the EMS:    `MDg5MGVkNDktNjMwZC00ODdiLTkyNGItMjNmMzMxNjRmM2IwOkhNUVRMUis4SGh6NHhBY21Vck9GaWdkem5rYzBrb3p0N056WUM0bGlqczMM`
2.  Include the string in the HTTP Authorization header formatted like this:  
    `Authorization:  **Bearer** MDg5MGVkNDktNjMwZC00ODdiLTkyNGItMjNmMzMxNjRmM2IwOkhNUVRMUis4SGh6NHhBY21Vck9GaWdkem5rYzBrb3p0N056WUM0bGlqczMM`

#### Using an Application API key
You can find out how to create an AppKey by following our  [Application API Keys](https://docs.celonis.com/en/application-keys.html)  guide.

To authenticate using AppKey Authentication:

1.  Create the AppKey in the EMS:  
`MzgyZDEzYjItNjI1MS00NTIwLTk1YTItY2ZjYzMzZTllOTNmOkE3a1dvYnpYQ0c3aUtUdTNRNC9UNzFLUXZmY0E2ZjVXUUROajFoN1R5UzIr`
2.  Include the string in the HTTP Authorization header formatted like this:  
    `Authorization:  **AppKey** MzgyZDEzYjItNjI1MS00NTIwLTk1YTItY2ZjYzMzZTllOTNmOkE3a1dvYnpYQ0c3aUtUdTNRNC9UNzFLUXZmY0E2ZjVXUUROajFoN1R5UzIr`

### Authorization
You must  [set the right permissions](https://docs.celonis.com/en/studio-spaces.html#UUID-c98ebc9f-fda1-c4fd-8182-8569193bc1cd_id_StudioSpaces-SpacePermissions)  and ensure the  [User API Key or the Application API Key](https://developer.celonis.com/intelligence-api/getting-started/#authentication)  leveraged for authorization purposes has access to the Celonis  [EMS Studio package](https://docs.celonis.com/en/studio.html#UUID-54a37cea-24d8-8459-8668-a517455a4e19_id_Studio-Workingwithpackages)  containing the Knowledge Model(s) you would like to access through Intelligence APIs.

You can grant access permissions by following these steps:

-   Go to the Studio package.
-   Click on the three dots and select  `Permissions`  from the pop-up menu.
-   Search for the User (in case you are using a  [Bearer token](https://developer.celonis.com/intelligence-api/getting-started/#using-a-user-api-key)  ) or AppKey (in case you are using an  [AppKey](https://developer.celonis.com/intelligence-api/getting-started/#using-an-application-api-keystrong-user-independent-one)  ) and grant at least  `USE PACKAGE`  rights.

## Getting Started
The base URL for the Celonis API is  `https://<team>.<cluster>.celonis.cloud/intelligence/api`

To find the team and the cluster, please check the URL you use to access the Celonis EMS and retrieve the team and cluster from it.

The Celonis Intelligence API is a JSON API and its endpoints will always return a JSON response, no matter the success of the request.

The current API methods allow consumers to interact with Knowledge Models defined in the EMS by providing the following functionalities:

-   List of available Knowledge Models and their details
-   List of records and their details
-   List of filters
-   Data for a specific record.

Details about the Celonis [Intelligence API Features](https://developer.celonis.com/intelligence-api/features/) at Celonis developer portal.

## Known Issues and Limitations

### Rate limiting
The Intelligence API was not built to bulk export RAW data but to make the calculated results and insights from process mining available to 3rd party platforms and applications. That’s why the Celonis API enforces rate limiting. This means that only a certain number of requests are allowed per day and a certain number of records can be retrieved in each call. Celonis reserves the right to adjust the rate limits at any time to guarantee high-quality service for all clients.

In case a client repeatedly exceeds the rate limits or engages in behavior that is deemed to be suspicious, Celonis reserves the right to temporarily or permanently limit or suspend access to the API for that client.

When a client exceeds the number of requests per day, the Celonis API will return a 429 response (too many requests) including an HTTP header (`x-ratelimit-reset`) which indicates the time (in seconds) that the client needs to wait before a new request can be processed.

The following HTTP headers are also returned as part of each call:

-   `x-ratelimit-limit`  : Represents the max number of requests that the client can perform in the current time window.
-   `x-ratelimit-remaining`  : Number of remaining requests in the current time window.

Currently, the API has the following default limits:

#### Table 1. Default rate limits

| **Limit** | **Default Values** |
|--|--|
| Max number of requests/day | 6000 requests/day |
| Max number of requests/second | 20 requests/second |
| Max number of allowed fields per request in the EMS Knowledge Model | 200 fields/request |
| Max number of records per request returned when calling the  `/data`  endpoint | 50 records/request |
| Total maximum number of records that can be retrieved through the  `/data`  endpoint | First 1000 records per filtered/sorted table |

Please consult your account team to verify your team's limits.

## Frequently Asked Questions
Please refer to the  [Frequently Asked Questions](https://developer.celonis.com/intelligence-api/faq/) section in Celonis Developer Portal.
## Deployment Instructions
Required. Add instructions on how to deploy this connector as custom connector. 
(To be added by @Patrick or @Mike)