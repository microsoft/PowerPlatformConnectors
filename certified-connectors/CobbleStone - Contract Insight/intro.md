The CobbleStone Contract Insight connector allows you to connect our software with many popular web and data services. The basic out of the box (OOB) endpoints will allow you to review schema table names and whether they can be searched, edited, and updated. You can pull Entity information on table names, and query, add, and update Customer record data.

CobbleStone Software provides leading contract management software (CLM), vendor management, eSourcing, and procurement software for organizations around the world. CobbleStone Contract Insight software automates and streamlines the entire source-to-contract lifecycle for legal, procurement, sales, risk, and supplier management professionals in numerous industries around the world. CobbleStone combines a user-friendly interface with robust features, backed by VISDOM® artificial intelligence and machine learning. CobbleStone is the trusted contract lifecycle management software provider for thousands of users with unparalleled industry experience.


## Prerequisites

Requires existing license to CobbleStone Software (Contact Insight).

Authorization (OAUTH 2.0)

- CobbleStone Software offers two methods of authorization for access: 
	- Authorization Code: https://oauth.net/2/grant-types/authorization-code/ 
	- Implicit Grant: https://oauth.net/2/grant-types/implicit/
		
Credentials can be setup/generated through your CobbleStone Software system.

## How to get credentials

To create the API Client credentials through your CobbleStone Software system, use the following steps:

Within your CobbleStone Software system, a system administrator can navigate to the following page to setup/configure credentials:
- For SaaS Clients: https://yourCompanyName.cobblestone.software/core/MyAPI.aspx
- For Deployed Clients (typically): https://yourCompanyName/core/MyAPI.aspx

> (NOTE: This is under the "My" top menu item and then select "My API Clients")

> This allows a system admin to create OAuth 2.0 credentials (Client ID and Client Secret) for use with the connector.

From logging into your CobbleStone Software system:

To begin, you will open CobbleStone Software and go under My > My API Clients…

![image](images\01.jpg)

Create a new API Client entry by clicking Add API Client and give it a unique name. The name is only internal and can be anything meaningful to its purpose.
We recommend using a different API key to organize things into small, manageable, and purposeful tasks.

![image](images\02.jpg)

This will generate a key and only admin personnel have access to the page. It is bound the the person who generated the key and they are the only one who can see it. You can share they key but only the logged in user can see it.

![image](images\03.jpg)

At this point we are done setting up the API Client entry. This Client ID and Client Securet are used for the connector to communicate with your CobbleStone Software system.

## Get started with your connector

### Setting up the connector

Before you can use the conector you need to fill some configurations as follows and illustrated in image below.

![image](images\OAuth2.jpg)

- Base URL would be typically as follow:
	- For SaaS Clients: https://yourCompanyName.cobblestone.software/API2/
	- For Deployed Clients (typically): https://yourCompanyName/{API_Folder}/

- Client ID
	Client Id is obtained from the How To get credentials point above.
- Client Secret
	Client Secret obtained from How To get credentials point above.
- Authorization Url
	Your base url illustrated above with /Authorization at the end.
- Token Url
	Your base url illustrated above with /oauth/token at the end.
- Refresh Url
	Your base url illustrated above with /oauth/token at the end.
- Scopes
	This can be left empty because it is not implemented in our API yet.

### Entity List

This step would pull the list of entities that API is allowed to work with.
- Input: No input for this step.

![image](images\EntityList.jpg)

- Output
	The response is a collection of objects that have the following properties:
	- EntityName: table name in the database
	- EntityNameDisplay: title of table
	- AllowAdd: allows you to use this table in Add Endpoint 
	- AllowEdit: allows you to use this table in Update Endpoint 
	- AllowView: allows you to use this table in Get Endpoint 
	- EntityNameForAPI: this is the value you use in URL segment  

### Schema
- Input: Name of entity to be selected from drop down list.

![image](images\Schema.jpg)

- Output
	The response is a collection of objects that have the following properties:
	- Name: name in database 
    - IsPrimaryKey: Boolean that determines if it is a primary key 
    - IsIdentity: Boolean to determine if it is auto increment 
    - IsNullable: Boolean that allows a NULL value 
    - DBType: Data types for field
    - MaxLength: Max number of characters in the field (NULL for number field) 
    - DataPrecision: Max number of decimal places (NULL when it isn’t a number)
    - DataScale: Max number of floating points (NULL when it isn’t a number) 
    - HasDefaultValue: Boolean 
    - IsComputedColumn: Boolean that indicates the value is a concatenation or calculation 
    - IsPassword: Boolean indicates the field stores a password 

### Get (a.k.a Select) Endpoint

- There is a nested object that need to be sent with this request in order to fulfill a pull request, which are the following:

	- Entity name: Name of entity to be selected from drop down list.
	- Fields: an array of fields (Name: which is the field name in database and it is mandatory, Alias: which is similar to AS alias in SQL and it is optional), NOTE: field name here doesnt exist as drop down list, the user must be aware of those names by using schema endpoint (for example through our API swagger page).
	  ![image](images\GetRecords_Entities.jpg)
	  In the above sample i am requesting two columns with different aliases so the response will limited to two columns set.	  
	  ![image](images\GetRecords_EmptyCollection.jpg)
	  In the above example I did switch to regular input and deleted everything so the Fields collection will be empty, in this scenario it will similar to ( SELECT * ) in SQL world meaning pull all columns.
	- Where Clause: it is a nested object that we created a GUI tool to handle generating it and it is shipped with our APIs.
	  To get into that tool web page you need to add \Web to the end of your API base url and you will end to the login page
	  ![image](images\Logins.jpg)
	  The username and password here client_id and client_secret you generated to use for the connector's connection.
	  Build your criteria as illustrated in the image below.
	  ![image](images\QueryBuilder.jpg)
	  After you are done with criterias click Edit Filter button below to get the following dialog.
	  ![image](images\QueryBuilder_Json.jpg)
	  Copy that json and bring it back to where clause input in power automate and add it as an expression ( json() ) illustrated in the following image (NOTE: dont pasteas text ot it wont be parsed when it is sent back to endpoint).
	  ![image](images\GetRecords_Where.jpg)
	- Order by tag: fill the column you want to order by, add more than one to order by more than one field, then at Order By Tag Direction chose the 	direction or leave it for the default value of ASC (Ascending).
	  NOTE: never use same input for more than one field comma seperated and always make sure of the column spelling from schema endpoint using our API swagger page.
	- Order by tag: fill the column you want to group by, add more than one to group by more than one field.
	  NOTE: never use same input for more than one field comma seperated and always make sure of the column spelling from schema endpoint using our API swagger page.
	- Start Index: corresponds to the simplest form of OFFSET function in SQL. It is set to zero if we need to fetch records starting with the first one. In other words, you may skip first N records in the resulting set.
	- Length: corresponds to the simplest form of FETCH function in SQL. In other words, you can take N records from resulting set starting with the record number specified by StartIndex attribute.

	 Response is an array of objects where each object represents a row.
	 ![image](images\GetRecords_Response.jpg)

### Update Endpoint

- Request object contains of array of Fields that needs to be updated, same where clause explained in Get endpoint above and a boolean flag for Triggering workflow.
	- Entity name: Name of entity to be selected from drop down list.
	- Fields: array of objects known in API as Tuples, contains Name and Value pair where Name is the field name in database and value is the new value.
	NOTE: field names have to be spelled correctly and if user isnt sure then use our API swagger page.
	- Trigger Workflow: tells the API to fire any workflow associated with the current entity, default value is false.

	![image](images\GetRecords_Response.jpg)

	Response will be an object containing a message, number of affected rows successfully and number of failed to affect rows.

### Add (a.k.a Insert) Endpoint

- Request object contains only Tuples which is similar to the one illustrated in Update endpoint and those Name/Value pairs will be the new fields for the new record.

	![image](images\Insert.jpg)

	Response will be an object containing a message and the newly auto generated Id.

## Known issues and limitations

Currently the connector does not support sending or receiving binary files.

The "Update" endpoint is limited to a max of 500 affected rows per hit. If more then 500 records need to be updated, they need to be processed in multiple hits.

## Common errors and remedies

- The following are the common error our client may encounter:
	- 400 Bad Request
	- 401 Unauthorized
	- 405 Method Not Allowed
	- 415 Unsupported Media Type

	The above error codes will have the following Json response:
		{
    		"LogId": (string) unique identifier of the error (GUID format),
    		"Message": (string) error desciption,
    		"Exception": (object) similar to System.Exception in .NET
		}

	We provide an ad-hoc report within ContractInsight to list all errors logged via APIs and client would be able to search error via LogId property.
