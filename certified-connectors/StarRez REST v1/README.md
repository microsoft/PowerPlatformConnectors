# StarRez REST v1
The StarRez resident and property management platform is the perfect tool to help your community thrive. 

With the StarRez REST API, you can automate regular tasks, import and export data and streamline your business processes.

# Publisher
StarRez, Inc.

# Prerequisites
To use this integration, you will need a StarRez account.

# Obtaining Credentials
To configure the custom connector, you will need to provide a username, password and base URL. 
Please contact the StarCare team at https://support.starrez.com or starcare@starrez.com to obtain these credentials. 
You can then create a connection that links the StarRez custom connector to your StarRez instance. 

# Supported Operations

## Triggers
None at this stage.

## Actions
The connector supports the following operations:

* `Delete`: Deletes a record from the specified table.

### Booking
* `Get Booking`: Retrieves one or more records from the Booking table.
* `Create Booking`: Creates a new record in the Booking table.
* `Update Booking`: Updates an existing record in the Booking table.

### Entry
* `Get Entry`: Retrieves one or more records from the Entry table.
* `Create Entry`: Creates a new record in the Entry table.
* `Update Entry`: Updates an existing record in the Entry table.

### Entry Address
* `Get Entry Address`: Retrieves one or more records from the Entry Address table.
* `Update Entry Address`: Updates an existing record in the Entry Address table.

### Entry Application
* `Get Entry Application`: Retrieves one or more records from the Entry Application table.
* `Create Entry Application`: Creates a new record in the Entry Application table.
* `Update Entry Application`: Updates an existing record in the Entry Application table.

### Entry Custom Field
* `Get Entry Custom Field`: Retrieves one or more records from the Entry Custom Field table.
* `Update Entry Custom Field`: Updates an existing record in the Entry Custom Field table.

### Entry Detail
* `Get Entry Detail`: Retrieves one or more records from the Entry Detail table.
* `Update Entry Detail`: Updates an existing record in the Entry Detail table.

### Entry Enrollment
* `Get Entry Enrollment`: Retrieves one or more records from the Entry Enrollment table.
* `Create Entry Enrollment`: Creates a new record in the Entry Enrollment table.
* `Update Entry Enrollment`: Updates an existing record in the Entry Enrollment table.

### Room Location
* `Get Room Location`: Retrieves one or more records from the Room Location table.

### Room Space
* `Get Room Space`: Retrieves one or more records from the Room Space table.
* `Create Room Space`: Creates a new record in the Room Space table.

### Room Space Maintenance
* `Get Room Space Maintenance`: Retrieves one or more records from the Room Space Maintenance table.
* `Create Room Space Maintenance`: Creates a new record in the Room Space Maintenance table.
* `Update Room Space Maintenance`: Updates an existing record in the Room Space Maintenance table.

### Term
* `Get Term`: Retrieves one or more records from the Term table.

### Term Session
* `Get Term Session`: Retrieves one or more records from the Term Session table.

### Transaction
* `Get Transaction`: Retrieves one or more records from the Transaction table.
* `Create Transaction`: Creates a new record in the Transaction table.

# Known issues and limitations

The current version of the StarRez REST custom connector does not include all API functionality. 
At present, operations are available for 12 of the most used tables. 
Other operations such as ListAttachments and GetPhoto are not currently supported.
	
`Get` actions allow for basic filtering of records in the relevant table. Records are matched based on the fields supplied. 
Complex queries cannot be created using this version of the connector. If more complex queries are required, the HttpRequest connector should be used instead.