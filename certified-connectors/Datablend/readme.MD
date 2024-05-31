# Datablend Connector
Datablend is a powerful iPaaS solution with a focus on serving the needs of CFOs, controllers, and financial planning and analysis (budgeting) professionals as they look to streamline processes, speed up closes, and increase confidence in reporting.

## Publisher: Datablend

## Prerequisites
You will need the following to proceed:
* A Datablend account for OAuth2 Logins

## Supported Operations
The connector supports the following operations:
1. Search for a specified group the user belongs to
   * JSON result displays a list of groups based on user's login information
2. Search Workflows available by group for the user
   * JSON result displays workflows available for a group by a user
3. Execute workflow
   * JSON Result from a who selects a workflow ID and passes it to this endpoint which will execute the workflow in the API
4. Search Executed Workflows
   * JSON result from a User who will look for the executed workflow based on the ID used to execute it. User will have to query this endpoint until the results come back as either failed or successful.
5. Get Query results of Executed Workflow 
   * JSON Result from a User who will select the QUERY ID returned from the previous step.

## Obtaining Credentials
Reach out to sales@datablend.com or visit our website for more details.

## Known Issues and Limitations
This is only set up to explore existing workflows for a customer. In order to create a new Workflow, you need to do so in Datablend.
The Query results are returned as a Base64 CSV file, so you have to use "Compose" in Power Automate with the following:
base64ToString(outputs('Get_Query_Execution_By_Id')?['body']['$content'])

* All default values for the parameters are set except the IDs of the group, workflow, or query

## Deployment Instructions
* Obtain credentials from Datablend
