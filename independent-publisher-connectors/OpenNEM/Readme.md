# OpenNEM Connector
The OpenNEM project aims to make the wealth of public National Electricity Market (NEM) data more accessible to a wider audience.

We hope that improved access will facilitate better public understanding of the market, improve energy literacy and help facilitate a more informed national discussion on Australia’s energy transition in the long term interests of consumers. By providing a clear window on the data, we hope to address the information asymmetry between stakeholders and improve the productivity of those engaged in energy market discussions.. 

Project homepage at https://opennem.org.au

Currently supports:

Australian NEM: https://www.nemweb.com.au/
West Australia Energy Market: http://data.wa.aemo.com.au/

## Publisher: Paul Culmsee

## Pre-requisites

## Supported Operations
The connector supports the following operations:

### Get Networks
Get the list of NEM networks and regions each network is made up of.

### Get Network Stations
Get the list of network stations.

### Get Network Station by ID
Get a single network station record by its ID.

### Get a Single Station by Code
Get a single network station record by its code.

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

### Get Network Regions
Get the list of regions for a network.

### Get Fuel Technologies
Get Fuel Technoligies used in the network.

### Get Intervals
Get Time Intervals.

### Get Periods
Get Reporting Time Periods.

### Get Units
Get Units.

### Get Power by Station
Get the power outputs for a station

### Get Energy by Station
Get energy output for a station (list of facilities) over a period

### Get Interconnector Flow Network
Return interconnector flow network details

### Get Power Network Region By Fueltech
Return power network region by fueltech

### Get Emission Factor Per Network Region
Return emission factor for each network region

### Get Fueltech Mix By Network
Return fueltech proportion of demand for a network

### Get Price History By Network and Network Region
Returns network and network region price info for interval which defaults to network interval size

## Known Issues and Limitations
OpenNEM is an open source effort. The API and schema is not fully documented so the descriptions and summary for actions are sometimes brief.

## Frequently Asked Questions

