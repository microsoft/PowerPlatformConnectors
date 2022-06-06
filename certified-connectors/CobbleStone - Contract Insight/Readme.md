# Overview
Cobblestone’s Contract Insight has the ability to send and receive information to other web services using an Application Programming Interface or API. 
This guide assumes the reader has some familiarity with topics such as APIs, JSON formatting, and Swagger, at a minimum.  

# Authorization (OAUTH 2.0)

- Contract Insight offers two methods of authorization for access: 
	- Authorization Code: https://oauth.net/2/grant-types/authorization-code/ 
	- Implicit Grant: https://oauth.net/2/grant-types/implicit/
		
## Creating an API Client

### From a Web Browser

Within their core system, a system administrator can navigate to the following to configure credentials for use on the API*:
- For SaaS Clients: https://yourCompanyName.cobblestone.software/core/MyAPI.aspx
- For Deployed Clients (typically): https://yourCompanyName/core/MyAPI.aspx

> (NOTE: This is under the "My" top menu item and then select "My API Clients")

> This allows a system admin to create OAuth 2.0 credentials (Client ID and Client Secret) for use with the REST API

From Contract Insight

To begin, you will open Contract Insight and go under My > My API Clients…

![image](images\01.jpg)

Create a new API Client entry by clicking Add API Client and give it a unique name. The name is only internal and can be anything meaningful to its purpose.
We recommend using a different API key to organize things into small, manageable, and purposeful tasks.

![image](images\02.jpg)

This will generate a key and only admin personnel have access to the page. It is bound the the person who generated the key and they are the only one who can see it. You can share they key but only the logged in user can see it.

![image](images\03.jpg)

At this point we are done setting up the API Client entry.

## Supported endpoints
- Get: 				Pull list of records for a given entity with specific criteria, order by tag, group by tag and number of records. Note: There is no limit on number of records to be pulled but the user has to consider that larger results will be slower to process.
- Add: 				Add a record (limited to one record per call) to specified entity. The response will be the system generated ID.
- Update:			Update record(s) for a specified entity/area with specific criteria and a TriggerWorkflow as flag to run workflow after the update request succeeds or not. Maximum number of records that can be updated is 500 per call.
- Schema:			Lists columns, data types, and any restrictions or flags (NULL, Length, Primary Key, etc.) in a table.
- List of entities:	Returns the list of names and three flagged properties indicating whether it can be edited, updated, or viewed/queried.