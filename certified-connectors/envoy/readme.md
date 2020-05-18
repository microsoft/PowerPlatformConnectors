
### NOTE
> This is a Envoy connector.  The connector is provided here with the intent to illustrate certain features and functionality around the Envoy connector.

## Envoy Connector
Envoy provides an extensive REST API for our visitor management platform.  Using this API, you can create invites from a spreadsheet, move your visitor entry data into another system, and you would also be able to pull Envoy data into other Microsft Applications.  Very often, you may want to leverage your visitor data in your application, or in your process automation. 

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
* An Envoy subscription
* Be an Envoy Global Admin

## Building the connector 
Since Envoy APIs are secure and require authentication, we utilized OAuth to create a secure connection between the Envoy and Microsoft systems.  After that is completed, you can create and test the sample connector.

### Set up an Envoy account
https://envoy.com/trial/

## Supported Operations

Update the invite	
Patch request to update invite information
The connector supports the following operations:
* `addBlockListFilters`: Add blocklist filters
* `Create Invites`: This is the POST request to create an invite
* `getBlockListFilters`: GET request to retrieve block list filters.
* `getCompanyInformation`: Retrieve information around the company (GET)
* `getEmployeesList`: GET request to retrieve the employees list
* `getEntryInformation`: Retrieve Employee Information with filtering by ID and location
* `getFlowsInfo`: Get information around the flows from your location.
* `getInviteByID`: GET request to retrieve an invite by ID
* `getInviteInfo`: Retrieve invite information based on filtering.
* `getLocations`: Retrieve invite information based on filtering.




