## Cognite Data Fusion Connector
Connect to your Cognite Data Fusion project using the Cognite Data Fusion API endpoints found at https://api-docs.cognite.com/

Documentation: https://docs.cognite.com/
Developer: https://developer.cognite.com/

## Cognite

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* A Cognite Data Fusion Project
* OAuth2 configured with your Cognite Data Fusion Project (see below)

## Supported Operations
* List time series
* Create time series
* Retrieve time series
* Filter time series
* Aggregate time series
* Search time series
* Update time series
* Delete time series
* Insert data points
* Retrieve data points
* Retrieve latest data point
* Delete data points
* Create or update spaces
* List spaces defined in the project
* Retrieve spaces by their space-ids
* Delete spaces
* Create or update data models
* List data models defined in the project
* Filter data models
* Retrieve data models by their external ids
* Delete data models
* Create or update views
* List views defined in the project
* Retrieve views by their external ids
* Delete views
* Create or update containers
* List containers defined in the project
* Retrieve containers by their external ids
* Delete containers
* Delete indexes from containers
* Delete constraints from containers
* Create or update nodes or edges
* Filter nodes or edges
* Retrieve nodes or edges by their external ids
* Search for nodes or edges
* Aggregate data across nodes or edges
* Delete nodes or edges
* Query nodes or edges
* Sync nodes or edges
* Query_GraphQL

### List time series
List time series. Use `nextCursor` to paginate through the results.

### Create time series
Creates one or more time series.

### Retrieve time series
Retrieves one or more time series by ID or external ID. The response returns the time series in the same order as in the request.

### Filter time series
Retrieves a list of time series that match the given criteria.

### Aggregate time series
The aggregation API allows you to compute aggregated results from a set of time series, such as
getting the number of time series in a project or checking what assets the different time series
in your project are associated with (along with the number of time series for each asset).
By specifying `filter` and/or `advancedFilter`, the aggregation will take place only over those
time series that match the filters. `filter` and `advancedFilter` behave the same way as in the
`list` endpoint.


### Search time series
Fulltext search for time series based on result relevance. Primarily meant
for human-centric use cases, not for programs, since matching and
order may change over time. Additional filters can also be
specified. This operation does not support pagination.

### Update time series
Updates one or more time series. Fields outside of the request remain unchanged.

For primitive fields (those whose type is string, number, or boolean), use `"set": value`
to update the value; use `"setNull": true` to set the field to null.

For JSON array fields (for example `securityCategories`), use `"set": [value1, value2]` to
update the value; use `"add": [value1, value2]` to add values; use
`"remove": [value1, value2]` to remove values.

### Delete time series
Deletes the time series with the specified IDs and their data points.

### Insert data points
Insert data points into a time series. You can do this for multiple time series.
If you insert a data point with a timestamp that already exists, it will be overwritten with the new value.

### Retrieve data points
Retrieves a list of data points from multiple time series in a project.
This operation supports aggregation and pagination.
Learn more about [aggregation](<https://docs.cognite.com/dev/concepts/aggregation/>).

Note: when `start` isn't specified in the top level and for an individual item, it will default to epoch 0, which is 1 January, 1970, thus
excluding potential existent data points before 1970. `start` needs to be specified as a negative number to get data points before 1970.

### Retrieve latest data point
Retrieves the latest data point in one or more time series. Note that the latest data point
in a time series is the one with the highest timestamp, which is not necessarily the one
that was ingested most recently.


### Delete data points
Delete data points from time series.

### Create or update spaces
Add or update (upsert) spaces. For unchanged space specifications, the operation completes without making any changes.  We will not update the ```lastUpdatedTime``` value for spaces that remain unchanged.

### List spaces defined in the project
List spaces defined in the current project.

### Retrieve spaces by their space-ids
Retrieve up to 100 spaces by specifying their space-ids.

### Delete spaces
Delete one or more spaces. Currently limited to 100 spaces at a time.


If an existing data model references a space, you cannot delete that space. Nodes, edges and other data types that are part of a space will no longer be available. 

### Create or update data models
Add or update (upsert) data models. For unchanged data model specifications, the operation completes without  making any changes.  We will not update the ```lastUpdatedTime``` value for models that remain unchanged.

### List data models defined in the project
List data models defined in the project. You can filter the returned models by the specified space.

### Filter data models
List data models in a project when they match a given filter.

### Retrieve data models by their external ids
Retrieve up to 100 data models by their external ids. Views can be auto-expanded when the ```InlineViews``` query parameter is set.

### Delete data models
Delete one or more data models.  Currently limited to 100 models at a time.  This does not delete the views,  nor the containers they reference.

### Create or update views
Add or update (upsert) views. For unchanged view specifications, the operation completes without making any changes.  We will not update the ```lastUpdatedTime``` value for views that remain unchanged.

### List views defined in the project
List of views defined in the current project. You can filter the list by specifying a space.

### Retrieve views by their external ids
Retrieve up to 100 views by their external ids.

### Delete views
Delete one or more views.  Currently limited to 100 views at a time.  The service cannot delete a view  referenced by a data model.

### Create or update containers
Add or update (upsert) containers. For unchanged container specifications, the operation completes without  making any changes.  We will not update the ```lastUpdatedTime``` value for containers that remain unchanged.

### List containers defined in the project
List of containers defined in the current project. You can filter the list by specifying a space.

### Retrieve containers by their external ids
Retrieve up to 100 containers by their specified external ids.

### Delete containers
Delete one or more containers. Currently limited to 100 containers at a time. You cannot delete a container when  one or more data model(s) or view(s) references it.

### Delete indexes from containers
Delete one or more container indexes. Currently limited to 10 indexes at a time.

### Delete constraints from containers
Delete one or more container constraints. Currently limited to 10 constraints at a time.

### Create or update nodes/edges
Create or update nodes and edges in a transaction. The ```items``` field of the payload is an array of objects
where each object describes a node or an edge to create, patch or replace. The ```instanceType``` field of
each object must be ```node``` or ```edge``` and determines how the rest of the object is interpreted.

This operation is currently limited to 1000 nodes and/or edges at a time.

Individual nodes and edges are uniquely identified by their externalId and space.

### Creating new instances
When there is no node or edge with the given externalId in the given space, a node will be created and the
properties provided for each of the containers or views in the ```sources``` array will be populated for the
node/edge. Nodes can also be created implicitly when an edge between them is created (if
```autoCreateStartNodes``` and/or ``` autoCreateEndNodes``` is set), or when a direct relation
property is set, the target node does not exist and ```autoCreateDirectRelations``` is set.

To add a node or edge, the user must have capabilities to access (write to) both the view(s) referenced in
```sources``` and the container(s) underlying these views, as well as any directly referenced containers.

### Filter nodes/edges
Filter the instances - nodes and edges - in a project.

### Retrieve nodes/edges by their external ids
Retrieve up to 1000 nodes or edges by their external ids.

### Search for nodes/edges
Search text fields in views for nodes or edge(s). The service will return up to 1000 results. This operation   orders the results by relevance, across the specified spaces.

### Aggregate data across nodes/edges
Aggregate data for nodes or edges in a project. You can use an optional query or filter specification to limit  the result.

### Delete nodes/edges
Delete nodes and edges in a transaction. Limited to 1000 nodes/edges at a time.


When a node is selected for deletion, all connected incoming and outgoing edges that point to or from it are also deleted. However, please note that the operation might fail if the node has a high number of edge connections. If this is the case, consider deleting the edges connected to the node before deleting the node itself. 

### Query nodes/edges
The Data Modelling API exposes an advanced query interface. The query interface supports parameterization, 
recursive edge traversal, chaining of result sets, and granular property selection.

A query is composed of a with section defining result set expressions that describe the input to the query, 
a set of optional parameter placeholders if the query is parameterized, and then the select section that defines 
which properties are to be returned back as part of the result.

Imagine you have a data set with airplanes and airports, represented as two sets of nodes with edges between them
indicating in which airports the airplanes land. Here is an example of a query which fetches a specific airplane as 
well as the airports it lands in:

```yaml
with:
    airplanes:
        nodes:
            filter:
                equals:
                    property: ["node", "externalId"]
                    value: {"parameter": "airplaneExternalId"}
        limit: 1
    lands_in_airports:
        edges:
            from: airplanes
            maxDistance: 1
            direction: outwards
            filter:
                equals:
                    property: ["edge", "type"]
                    value: ["aviation", "lands-in"]
    airports:
        nodes:
            from: lands_in_airports
parameters:
    airplaneExternalId: myFavouriteAirplane
select:
    airplanes: {}
    airports: {}
```

### Sync nodes/edges
Subscribe to changes for nodes and edges in a project, matching a supplied filter.  This endpoint will always  return a ```NextCursor```.  The sync specification mirrors the query interface, but sorting is not currently  supported.

## Obtaining Credentials
Since Cognitedata APIs are secured by Azure Active Directory (AD), we first need to set up a few thing in Azure AD so that our connectors can securely access the Cognite API.  After that is completed, you can create and test the sample connector.

### Set up an Azure AD application for your custom connector
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access Cognite data on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This Azure AD application will be used to identify the connector to the Cognite API.  This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access the Cognitedata API
This step will ensure that your application can successfully retrieve an access token to invoke Cognitedata API on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, make sure “Cognitedata API: <cluster>” and “user_impersonation” are added.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Cognitedata API.  The next step for us is to create a custom connector.

## Known Issues and Limitations
This is a partial connector containing only the Time Series and Data Modeling endpoints to the Cognite Data Fusion API. 

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```
### NOTE
> Make sure that the PLACEHOLDER variables are updated in apiProperties.json
