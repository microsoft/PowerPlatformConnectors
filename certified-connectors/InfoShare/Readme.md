
## Kendox InfoShare Connector
Kendox InfoShare provides a powerful and very extensive REST API. Using this API, you can create and manage documents and processes in InfoShare. Together with the Kendox InfoShare Power Automate Connector you can streamline content processes across a widespread application landscape and limit information silos.

## Pre-requisites
You will need the following to proceed:
* A license to use this connector purchasable at support@kendox.com.
* Kendox InfoShare with the WCF service endpoints online reachable.
* Kendox InfoShare login credentials.

## Supported Operations
The connector supports the following operations:

### General
* `Logon`: Logon to InfoShare server
* `Logon with hashed password`: Logon to InfoShare
* `Logoff`: Logon from InfoShare Server

### Document
* `Create document`: Create the document in InfoShare
* `Update document`: Update the document in InfoShare
* `Get document properties`: Get the properties of the document from InfoShare
* `Get file content`: Get the content of the document from InfoShare
* `Get document`: Returns document informations from InfoShare

### Process
* `Close current task and assign user to next task`: Close a process task and assign user to next task in InfoShare
* `Update process`: Update a process in InfoShare
* `Get process properties`: Get the properties of the process from InfoShare
* `Close an open process`: Close a process in InfoShare
* `Close task and continue process`: Close a process task in InfoShare
* `Get process`: Returns a list of process informations from InfoShare

### Search
* `Document Search`: Search documents in InfoShare
* `List processes: Search processes in InfoShare

