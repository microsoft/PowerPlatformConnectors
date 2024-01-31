
# ALVAO

ALVAO provides a powerful and very extensive REST API. Using this API, you can automate processes related to Asset management objects and Service Desk tickets.

## Prerequisites

You will need the following to proceed:

* Be ALVAO customer (SaaS or On-prem) with REST API available from the internet.

## Authentication

Basic authentication.

_Note: Microsoft Entra ID authentication is also available, but only for the use as a custom connector._

### Set up a connection

1. Set the Username and Password of an application account created in ALVAO.
2. Set the REST API Host URL, e.g. <https://alvao.contoso.com/AlvaoRestApi>.

## Supported Operations

The connector supports the following operations:

* `Create ticket`: Creates a new ticket
* `Get ticket`: Returns fields of a ticket
* `Update ticket`: Updates fields of a ticket
* `Create record in ticket log`: Creates a new record in a ticket log
* `Assign ticket to solver or solver team`: Assigns a ticket to a solver or a solver team
* `Change ticket state`: Transits a ticket in a ticket status
* `Move ticket to another service`: Moves a ticket to a service
* `Create object`: Creates a new object from specified template under specified object
* `Get object`: Returns object with its properties
* `Get objects`: Returns objects with its properties according to OData parameters
* `Update object properties`: Update object properties
* `Move object`: Moves an object to a specified destination
* `Get users`: Returns users and their properties according to OData parameters
* `Import objects from CSV`: Imports objects provided in CSV format. Similar functionality to ImportUtil utility

## Supported Triggers

The connector supports the following triggers:

* `When a ticket transitions to a status`: This operation is triggered when a ticket transitions to a certain status
* `When a ticket field value is changed`: This operation is triggered when a certain ticket field value is changed
* `When an object is created`: This operation is triggered when an object of a certain kind is created
* `When an object is moved`: This operation is triggered when an object of a certain kind is moved to a different position in the object tree
* `When a value of an object property is changed`: This operation is triggered when a value of a certain object property is changed

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further Support

For further support, you can contact us at <support@alvao.com>.
