
## Kendox InfoShare Connector
Kendox InfoShare provides a powerful and very extensive REST API. Using this API, you can create and manage documents and processes within InfoShare. Together with the Kendox InfoShare Power Automate Connector you can streamline content processes across a widespread application landscape, thus avoiding information silos.

## Pre-requisites
You will need the following to proceed:
* A license to use this connector available via sales365@kendox.com.
* Kendox InfoShare with the WCF service endpoints reachable online.
* Kendox InfoShare login credentials.

## Supported Operations
The connector supports the following operations:

### General
* `Logon`: Log on to InfoShare Server
* `Logon with hashed password`: Log on to InfoShare Server
* `Logoff`: Log on from InfoShare Server

### Document
* `Create document`: Create a document in InfoShare
* `Update document`: Update the document in InfoShare
* `Get document properties`: Get the properties of a document from InfoShare
* `Get file content`: Get the content of a file from InfoShare
* `Get file content converted`: Converts content of a file to another file format (e.g. tiff to pdf). Get file content including annotations and overlays  
* `Get document`: Returns document information (structure) from InfoShare

### Process
* `Create process`: Create and start a process 
* `Close current task and assign user to next task`: Close a process task and assign user to next task in InfoShare
* `Update process`: Update a process in InfoShare
* `Get process properties`: Get the properties of a process from InfoShare
* `Close an open process`: Close a process in InfoShare
* `Close task and continue process`: Close a process task in InfoShare
* `Get process`: Returns a list of process informations (structure) from InfoShare

### Search
* `Document Search`: Search for documents within InfoShare
* `List processes`: Search for processes within InfoShare

### User Tables
* `Get rows from a user table`: Gets some rows from a user table 
* `Insert rows into a user table`: Insert rows into a user table 
* `Create a user table`: Creates a user table 
* `Delete user table rows`: Deletes some rows from a user table 
