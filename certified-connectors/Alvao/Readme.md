# ALVAO

ALVAO provides a powerful and very extensive REST API. Using this API, you can automate processes related to Asset management objects and Service Desk tickets.

## Publisher

ALVAO s.r.o.

## Prerequisites

You will need the following to proceed:

* Be ALVAO customer (SaaS or On-prem) with REST API available from the internet.

## Supported Operations

The connector supports the following operations:

### Create ticket

Creates a new ticket.

### Get ticket

Returns fields of a ticket.

### Update ticket

Updates fields of a ticket.

### Create record in ticket log

Creates a new record in a ticket log.

### Assign ticket to solver or solver team

Assigns a ticket to a solver or a solver team.

### Change ticket state

Transits a ticket in a ticket status.

### Move ticket to another service

Moves a ticket to a service.

### Create object

Creates a new object from specified template under specified object.

### Get object

Returns object with its properties.

### Get objects

Returns objects with its properties according to OData parameters.

### Update object properties

Update object properties.

### Move object

Moves an object to a specified destination.

### Get users

Returns users and their properties according to OData parameters.

### Import objects from CSV

Imports objects provided in CSV format. Similar functionality to ImportUtil utility.

## Obtaining Credentials

1. Create an application account in ALVAO WebApp and give it the necessary permissions.
2. Use the credentials of this account to connect to the ALVAO connector.
3. Set the REST API Host URL, e.g. <https://alvao.contoso.com/AlvaoRestApi>.

## Getting Started

Optional. How to get started with your connector.

## Known Issues and Limitations

Some actions or triggers may require entering an ID of an entity like service, person etc. All these IDs can be found in URLs whne editing them in the Administration part of ALVAO WebApp.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
