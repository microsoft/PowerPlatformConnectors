# OpenNEM Connector
The OpenNEM project aims to make the wealth of public National Electricity Market (NEM) data more accessible to a wider audience.

We hope that improved access will facilitate better public understanding of the market, improve energy literacy and help facilitate a more informed national discussion on Australia’s energy transition in the long term interests of consumers.

By providing a clear window on the data, we hope to address the information asymmetry between stakeholders and improve the productivity of those engaged in energy market discussions.. 

## Publisher: Paul Culmsee

## Pre-requisites

## Supported Operations
The connector supports the following operations:

### Get Networks
Get the list of NEM networks and regions each network is made up of.

### Get Regions for Network
Get the list of regions for a network.

### Get Fueltech Mix By Network
Return fueltech proportion of demand for a network

### Get Emission Factor Per Network Region
Return emission factor for each network region

### Get Power Network Region By Fueltech
Return power network region by fueltech

### Get Interconnector Flow Network
Return interconnector flow network details

### Get Stations
Get the list of ??? stations.

### Get Station by Code
Get a single ??? station record by its code.

### Get Energy by Station
Get energy output for a station (list of facilities) over a period

### Get Power by Station
Get the power outputs for a station

### Get Facilities
Get the list of facilities.

### Get Facilities by Code
Get a single facility record by its code.

### Get Weather Stations
Get the list of list of weather stations.

### Get Weather Station by Code
Get a single weather station record by its code.

### Get Weather Station Observations
Get a weather observations from a station.




## Obtaining Credentials 
This connector uses oAuth with Application.Read.All on Delegated scope 

## Known Issues and Limitations
Required. Include any known issues and limitations a user may encounter.

## Frequently Asked Questions
### Why have you not added the other application endpoints?
My intent here was to write flows that would look for application registrations with soon to be expiring client secrets. If you would like see other endpoints added, let me know or submit changes to this connector via guthub. 

### How to I create a client ID and secret?
Register an application in Azure AD and grant it delegated Application.Read.All access to the Microsoft Graph Service (https://docs.microsoft.com/en-us/graph/api/resources/application?view=graph-rest-1.0).

## Deployment Instructions
Create a new application registration in Azure AD and enable API access to the Microsoft Graph with Application.Read.All for Delegated permissions.
After downloading the connector, edit the client ID and tenant ID in apiProperties.json to your app registration and tenant. 
Please use paconn CLI to deploy.


