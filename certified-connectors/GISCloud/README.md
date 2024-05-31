# GIS Cloud
GIS Cloud is a SaaS platform that provides best in class web and mobile map rendering coupled with a powerful forms engine to facilitate all your collaborative mapping needs. The GIS Cloud Map Connector provides a subset of the extensive and powerful REST API that allows automations to drive most of the common use cases for a Map project. This connector will aloow you to list, update and interogate GIS Cloud Maps, Layers, Users and to also create Features and Files. With this subset a user can automate many data driven activities such as creating new features for inspection, creating reports, exporting data for dashboarding etc...

## Publisher: HandyGeo Services

## Prerequisites
### You will need the following to proceed:

- A GIS Cloud user account with a Map Editor subscription
- A GIS Cloud API key generated under the My Account menu
- The Power platform CLI tools
- A Microsoft Power Apps or Power Automate plan with custom connector feature

A full reference for the GIS Cloud REST API can be found at: https://docs.giscloud.com/rest

## Supported Operations
### [ACTION]: Get Maps
List accessable maps

### [ACTION]: Get Map
Get specified map details

### [ACTION]: Delete Map
Delete specified map project

### [ACTION]: Update Map
Update specified map project

### [ACTION]: Delete Map Cache
Clears the server cache of map tiles for a specified map

### [ACTION]: Render Map
Renders an image of the map project

### [ACTION]: Get Layers
List accessable layers

### [ACTION]: Get Layer
Get specified layer details

### [ACTION]: Delete Layer
Delete specified layer

### [ACTION]: Update Layer
Update specified layer

### [ACTION]: Render Map
Renders an image of the map layer

### [ACTION]: Get Layer Columns
Describe the attributes and data types of a layer

### [ACTION]: Get Features
List all features on a layer

### [ACTION]: Get Feature
Get specified feature on a layer

### [ACTION]: Create Feature
Create new feature on a specified layer

### [ACTION]: Delete Feature
Delete specified feature on a layer

### [ACTION]: Update Feature
Update specified feature on a layer

### [ACTION]: Download Media
Get specified file from cloud storage

### [ACTION]: Upload Media
Upload specified file to cloud storage

### [ACTION]: Delete Media
Delete specified file from cloud storage

### [ACTION]: Get Current User
Get current GIS Cloud user

### [ACTION]: Get User
Get specified GIS Cloud user

## Obtaining Credentials
The GIS Cloud REST API is secured via an API key. Most REST endpoints will not function without a valid API key.

To obtain a valid API key navigate to https://manager.giscloud.com and log in with your GIS Cloud account.

In the top right, click on your user name and select "My Account" from the dropdown menu.

In the popup modal, select the "API Access" tab. Enter a descriptive name for the key in "New API key description" box and click "Add key". A new modal will present your new API Key which you must copy and store securely as it will never be shown again.

For additional reference please see: https://docs.giscloud.com/rest#creating-an-api-key

After that is completed, you can create and test the connector.

## Known Issues and Limitations
### Authentication compatibility for PowerApps
To aid compatability with the PCF map component published by HandyGeo Services and to avoid prompting PowerApps users for API keys which is not user friendly we have created the connector to require the API key to be specified with every request.

### Dynamic data schema for Feature endpoints
Due to geographic features in GIS Cloud originating from user uploaded data the schema is always dataset specific. As such the schema for payloads and responses from all Feature endpoints are always dynamic and require support for dynamic schemas in the Office 365 product being used. This is generally well supported by PowerAutomate but can present challenges in PowerApps.

### Endpoint Pagination
GIS Cloud endpoints are limited to a maximum of 5,000 records being returned from a single request. While the REST API supports pagination, currently there is no way to automatically include this in the connector behaviour.

### REST API Rate Limiting for Service Protection
As should be expected with any production REST API service, if an excessive number of requests to the GIS Cloud API are made in a short amount of time the response will be a 5xx code. While a specific rate limit is not explicitly defined, the REST API will support a limited parallel loop rate (i.e. approx 20) but high rates are likely to result in significant failures. Failed responses can be managed with a retry policy but excessive rates will cause issues.

## Deployment Instructions
Obtain or create a suitable `settings.json` file and run the following commands and follow the prompts:

```paconn create --settings settings.json```