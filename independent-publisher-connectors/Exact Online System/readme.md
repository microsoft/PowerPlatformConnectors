# Exact Online - System
Exact Online covers both accounting and CRM, providing firm foundations on which to build a strong company.
This connector contains the System service as described in the [reference documentation](https://start.exactonline.nl/docs/HlpRestAPIResources.aspx?SourceAction=10).

## Publisher: Indocs

## Prerequisites
You need an [Exact Online account](https://www.exact.com/try).

## Obtaining Credentials
For authorization a client ID and client secret are required.
The [Exact Online knowledgebase](https://support.exactonline.com/community/s/knowledge-base#All-All-DNO-Content-oauth-eol-oauth-devstep1) describes how to create / get these.

## Supported Operations

### AccountantInfo
This end point shows info related to the accountant of the customer. If the customer is an accountant himself, the service will show the info of the customer. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SystemSystemAccountantInfo)

### AllDivisions
Returns all (non-deleted) divisions of a single license, being the license that owns the division specified in the URI. Typically this will be the signed-in user's license, but for an accountant it can be a client's license instead. Most users will see all divisions that are in the relevant license (including divisions they do not have access rights to), but if the license is not user's (i.e. accountant in client's division), or if the user has limited rights ('view user'), then only divisions that are accessible to the user will be returned.Please note that divisions returned are only those which the user has granted permission to. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SystemSystemAllDivisions)

### AvailableFeatures
This end point shows all features that are available in the current licence. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SystemSystemAvailableFeatures)

### Divisions
Returns only divisions that are accessible to the signed-in user, as configured in the user card under 'Companies: Access rights'. Accountants will see both their own divisions and those belonging to their clients. Please note that divisions returned are only those which the user has granted permission to. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SystemSystemDivisions)

### GetMostRecentlyUsedDivisions
The end point retrieves the top 5 most recently used companies. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SystemSystemGetMostRecentlyUsedDivisions)

### Me
This end point retrieves information about the current user. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SystemSystemMe)

## Known Issues and Limitations
- The number of API calls is limited to 60 per minute and 5000 per day, per company.

Check the [API Limits](https://support.exactonline.com/community/s/knowledge-base#All-All-DNO-Simulation-gen-apilimits) page for more details.
