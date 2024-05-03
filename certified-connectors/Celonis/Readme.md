Skip to:

Skip to Jira Navigation
Skip to Side Navigation
Skip to Main Content
###CONFIDENTIAL Tool### Welcome to P&E Jira Cloud, enjoy the cloud experience. Refer https://confluence.celonis.com/display/DKB/Jira+Knowledge+Base for any questions on Cloud.




P&E Jira

Your work

Projects

Filters

Dashboards

Teams

Plans

Apps

Create
Search

9+




Intelligence APIs
Software project
MENU
PLANNING
DEVELOPMENT
OPERATIONS
You're in a company-managed project
Learn more


Projects
Intelligence APIs
Intelligence APIs | Scrum Board
Intelligence APIs - Sprint #43
Main goal is to release SAPI-1.5.0 to production Other goals are: - Tech Design for Gateway - Analysis of API usage
0 days remaining





Complete sprint

As you type to search or apply filters, the board updates with issues to match.
Search this board
Filter by assignee













Epic

Label

Type

Quick filters
GROUP BY

None

Insights

View settings
IAPI-850
IAPI-961
IAPI-956
IAPI-955
IAPI-909
IAPI-912
IAPI-957
IAPI-949
IAPI-786
IAPI-935
IAPI-833
IAPI-870
IAPI-906
IAPI-826
IAPI-941
IAPI-864
IAPI-633
IAPI-602
IAPI-893

IAPI-780

IAPI-909


[PUB/SUB] Generate OpenAPI v2 specs of Subscription Management for MS Power Platform Marketplace

Attach

Create subtask

Link issue


Add design

General
Test Assessment
Description

Please generate the updated openAPI v2 specs including the subscription lifecycle management end-points.

Also, please, commit the attached updates to the Microsoft readme file:

Readme (5).md

cc: @Emilio Alves Fulgencio

Attachments
1


Open Readme (5).md
Readme (5).md
Readme(5).md
21 Mar 2024, 03:10 pm


Activity
Show:

All

Comments

History

Work log

Salesforce Comments

Newest first

Add a comment…
Pro tip: press
M
to comment

In Progress

Actions
Your pinned fields
Labels


None
Sprint


Intelligence APIs - Sprint #43

+1
Development

1 branch

1 commit
1 minute ago

1 pull request
MERGED
Releases
Assignee



Emilio Alves Fulgencio
Fix versions


None
Details
Reporter



Ioanna Aristeidopoulou
Change Owner


None
Start date


None
Datadog Incident


None
Does this functionality need Release Notes?


None
Priority



No Prio Set
Parent
NEW


Epic icon
IAPI-780 [Post-GA] Trigger Subscription
Zoom discussions
Open Zoom discussions
More fields
Story Points


None
Original estimate


0m
Time tracking


No time logged
Components


None
Affects versions


None
Due date


None
Automation

Rule executions
Salesforce

Associations
Created 15 March 2024 at 09:56
Updated 3 days ago
Configure

Readme (5).md
text · 9 KB

# Title
The **Celonis Intelligence API** allow Celonis customers to embed actionable process intelligence into third-party platforms where end-users may benefit from Celonis process insights. In this manner, it would bring Celonis intelligence like process KPIs or process inefficiencies closer to the end-users and benefit operational users without current access to Celonis by providing valuable insights for better decision-making while executing their day-to-day activities.
Apart from the data query capability, Intelligence API also supports pushing data to third-party platforms by allowing them to subscribe to business triggers.
## Publisher: Publisher's Name
[Celonis](https://www.celonis.com/) ​
## Prerequisites
1.  A team in Celonis Platform.
2.  A  [Knowledge Model](https://docs.celonis.com/en/knowledge-model.html)  available in a client's team.
3.  Process-based Celonis Subscription (contact your account team to verify).
## Supported Operations
The current API methods allow consumers to interact with Knowledge Models defined in Celonis Platform by providing the following functionalities:
### Knowledge Model Discovery Methods
####   [Get a list of Knowledge Models (KM)](https://developer.celonis.com/openapi/reference/operation/getKnowledgeModel/)
####   [Get a list of KM Records](https://developer.celonis.com/openapi/reference/operation/getRecords/)
####   [Get a KM Record Schema Details](https://developer.celonis.com/openapi/reference/operation/getRecordDetails/)
####   [Get a list of KM Filters](https://developer.celonis.com/openapi/reference/operation/getFilters/)
### Knowledge Model Data Retrieval Methods
####   [Get a list of KM Record data](https://developer.celonis.com/openapi/reference/operation/getRecordDataResult/)
### Subscription Lifecycle Management Methods
####   [Get a list of Subscriptions](https://developer.celonis.com/openapi/reference/operation/getSubscriptions/)
####   [Get a list of KM Triggers](https://developer.celonis.com/openapi/reference/operation/getTriggers/)
####   [Create a Subscription to a KM  Trigger](https://developer.celonis.com/openapi/reference/operation/createSubscription/)
####   [Update a Subscription](https://developer.celonis.com/openapi/reference/operation/updateSubscription/)
####   [Pause a Subscription](https://developer.celonis.com/openapi/reference/operation/pauseSubscription/)
####   [Resume a Subscription](https://developer.celonis.com/openapi/reference/operation/resumeSubscription/)
####   [Terminate a Subscription](https://developer.celonis.com/openapi/reference/operation/unsubscribe/)
## Obtaining Credentials
### Authentication
Each request to the API must be authenticated with a Celonis API key. There are two ways of doing this:
#### Using a User API key
You can find out how to create an user API key by following our  [User API Keys](https://docs.celonis.com/en/user-profile.html)  guide.
The Celonis API uses Bearer Token Authentication for verifying consumer access. The credentials must be sent in an Authorization header in the HTTP request. Credentials sent in the URL or body of the request will be ignored.
To authenticate using Bearer Token Authentication:
1.  Create the token in Celonis:    `MDg5MGVkNDktNjMwZC00ODdiLTkyNGItMjNmMzMxNjRmM2IwOkhNUVRMUis4SGh6NHhBY21Vck9GaWdkem5rYzBrb3p0N056WUM0bGlqczMM`
2.  Include the string in the HTTP Authorization header formatted like this:  
    `Authorization:  **Bearer** MDg5MGVkNDktNjMwZC00ODdiLTkyNGItMjNmMzMxNjRmM2IwOkhNUVRMUis4SGh6NHhBY21Vck9GaWdkem5rYzBrb3p0N056WUM0bGlqczMM`
#### Using an Application API key
You can find out how to create an AppKey by following our  [Application API Keys](https://docs.celonis.com/en/application-keys.html)  guide.
To authenticate using AppKey Authentication:
1.  Create the AppKey in Celonis:  
    `MzgyZDEzYjItNjI1MS00NTIwLTk1YTItY2ZjYzMzZTllOTNmOkE3a1dvYnpYQ0c3aUtUdTNRNC9UNzFLUXZmY0E2ZjVXUUROajFoN1R5UzIr`
2.  Include the string in the HTTP Authorization header formatted like this:  
    `Authorization:  **AppKey** MzgyZDEzYjItNjI1MS00NTIwLTk1YTItY2ZjYzMzZTllOTNmOkE3a1dvYnpYQ0c3aUtUdTNRNC9UNzFLUXZmY0E2ZjVXUUROajFoN1R5UzIr`
### Authorization
You must  [set the right permissions](https://docs.celonis.com/en/studio-spaces.html#UUID-c98ebc9f-fda1-c4fd-8182-8569193bc1cd_id_StudioSpaces-SpacePermissions)  and ensure the  [User API Key or the Application API Key](https://developer.celonis.com/intelligence-api/getting-started/#authentication)  leveraged for authorization purposes has access to the Celonis  [Studio package](https://docs.celonis.com/en/studio.html#UUID-54a37cea-24d8-8459-8668-a517455a4e19_id_Studio-Workingwithpackages)  containing the Knowledge Model(s) you would like to access through Intelligence APIs.
You can grant access permissions by following these steps:
-   Go to the Studio package.
-   Click on the three dots and select  `Permissions`  from the pop-up menu.
-   Search for the User (in case you are using a  [Bearer token](https://developer.celonis.com/intelligence-api/getting-started/#using-a-user-api-key)  ) or AppKey (in case you are using an  [AppKey](https://developer.celonis.com/intelligence-api/getting-started/#using-an-application-api-keystrong-user-independent-one)  ) and grant at least  `USE PACKAGE`  rights.
## Getting Started
The base URL for the Celonis API is  `https://<team>.<cluster>.celonis.cloud/intelligence/api`
To find the team and the cluster, please check the URL you use to access the Celonis Platform and retrieve the team and cluster from it.
The Celonis Intelligence API is a JSON API and its endpoints will always return a JSON response, no matter the success of the request.
The current API methods allow consumers to interact with Knowledge Models defined in Celonis Platform by providing the following functionalities:
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
#### Table 1. Default request rate limits
| **Limit** | **Default Values** |
|--|--|
| Max number of requests/day | `6000` requests/day |
| Max number of requests/second | `20` requests/second |
| Max number of allowed fields per request in the Knowledge Model | `200` fields/request |
| Max number of records per request returned when calling the  `/data`  endpoint | `50` records/request |
| Total maximum number of records that can be retrieved through the  `/data`  endpoint | First `5.000` records per filtered/sorted table |
Subscription to Trigger is also enforcing rate limiting. This means for a team, only a certain number of subscriptions can be created. In addition, no matter how many subscriptions are created, there is a maximum number of events that can be emitted from the API to the third parties consumers.
If a client reaches the maximum number of subscriptions, they will need to delete an existing subscription in order to create a new one.
If a client reaches the maximum number of events emitted from the API, the rest of data produced by Celonis Platform will be discarded.
If the client is approaching their daily quota, they will be informed via emails to your admin account. The first email will be sent when 80% of the quota is exceeded, letting your admin account know that you're over that percentage. The second email will be sent as soon as you exceed 100% of the quota.
Currently, the API has the following default limits:
#### Table 2. Default event rate limits
| **Limit** | **Default Values** |
|--|--|
| Max number of subscriptions/team | `10` |
| Max number of events/day | `100,000` |
Please consult your account team to verify your team's limits.
## Frequently Asked Questions
Please refer to the  [Frequently Asked Questions](https://developer.celonis.com/intelligence-api/faq/) section in Celonis Developer Portal.
## Deployment Instructions
