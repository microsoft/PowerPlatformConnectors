# Exact Online - Time & Billing
Exact Online covers both accounting and CRM, providing firm foundations on which to build a strong company.
This connector contains the methods that can be used with the Exact Online module "Time & Billing".

## Publisher: Indocs

## Prerequisites
You need an [Exact Online account](https://www.exact.com/try).

## Obtaining Credentials
For authorization a client ID and client secret are required.
The [Exact Online knowledgebase](https://support.exactonline.com/community/s/knowledge-base#All-All-DNO-Content-oauth-eol-oauth-devstep1) describes how to create / get these.

## Supported Operations

### Divisions
Returns only divisions that are accessible to the signed-in user, as configured in the user card under 'Companies: Access rights'. Accountants will only see divisions that belong to a single license (either their own or a client's), being the license that owns the division specified in the URI.Please note that divisions returned are only those which the user has granted permission to.Recommended alternative that is not limited to accessible divisions: /api/v1/{division}/system/AllDivisionsRecommended alternative that is not limited to a single license: /api/v1/{division}/system/Divisions [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=HRMDivisions)

### EmploymentInternalRates
Use this endpoint to retrieve internal rates of employees. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ProjectEmploymentInternalRates)

### HourCostTypes
This endpoint enables users to retrieve up to date active Hour and Cost types. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourCostTypes)

### HourEntryActivitiesByProject
This endpoint enables users to retrieve a list of WBS Activities and its parent Deliverable based on the project ID provided. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourEntryActivitiesByProject)

### HourEntryRecentAccounts
Use this endpoint to read a list of accounts that is used by an employee to create hour entries. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourEntryRecentAccounts)

### HourEntryRecentAccountsByProject
This endpoint enables users to retrieve a list of Accounts used by an employee for hour entries based on the Project Id provided. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourEntryRecentAccountsByProject)

### HourEntryRecentHourTypes
Use this endpoint to read a list of items that is used by an employee to create hour entries. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourEntryRecentHourTypes)

### HourEntryRecentHourTypesByProject
This endpoint enables users to retrieve a list of Items used by an employee for hour entries based on the Project Id provided. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourEntryRecentHourTypesByProject)

### HourEntryRecentProjects
Use this endpoint to read and retrieve projects that employees have used for entering hour entries order by most recently. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourEntryRecentProjects)

### HoursByDate
Use this endpoint to read , filter and display all hour entries by employee based on the Date provided. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHoursByDate)

### HoursById
This endpoint enables users to retrieve an hour entry's information for an employee. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHoursById)

### HourTypes
This endpoint enables users to retrieve up to date active Hour types. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourTypes)

### HourTypesByDate
This endpoint enables users to retrieve active Hour types based on date provided. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourTypesByDate)

### HourTypesByProjectAndDate
Use this endpoint to read active hour types by providing project id and a reference check date.Note: Employee Id parameter is optional. Hour types listed will be based on a hierachy with project employee hour type restriction, project hour type restriction and employee restriction by providing this additional parameter. For this function to work correctly, you must supply all parameters. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectHourTypesByProjectAndDate)

### Me
This end point retrieves information about the current user. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SystemSystemMe)

### ProjectRestrictionRebillings
This endpoint enables users to restrict which cost types the project allows rebilling to customer. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ProjectProjectRestrictionRebillings)

### ProjectTimeCostTransactions
The TimeCostTransactions sync api returns both time and cost transactions. To filter out time or cost transactions you need to filter only on property Type. The API does not allow to filter on these fields. Filtering need to be done after receiving the records.
The sync api's have the goal to keep the data between Exact Online and a 3rd party application the same.
The sync api's are all based on row versioning and because of that it is guaranteed to be unique. Every time an existing record is changed or a new record is inserted, the row versioning value is higher than the highest available value at that time. When retrieving records via these api's also a timestamp value is returned. The highest timestamp value of the records returned should be stored on client side. Next time records are retrieved, the timestamp value stored on client side should be provided as parameter. The api will then return only the new and changed records. Using this method is more reliable than using modified date, since it can happen that multiple records have the same modified date and therefore same record can be returned more than once. This will not happen when using timestamp.
The sync api's are also developed to give best performance when retrieving records. Because of performance and the intended purpose of the api's, only the timestamp field is allowed as parameter.
The single and bulk api’s are designed for a different purpose. They provide ability to retrieve specific record or a set of records which meet certain conditions.
In case the division is moved to another database in Exact Online the timestamp values will be reset. Therefore, after a division is moved all data needs to be synchronized again in order to get the new timestamp values. To see if a division was moved, the /api/v1/{division}/system/Divisions can be used. The property DivisionMoveDate indicated at which date a division was moved and this date can be used to determine if it is needed to synchronize all data again.
The API has two important key fields, the Timestamp and the ID. The ID should be used to uniquely identify the record and will never change. The Timestamp is used to get new or changed records in an efficient way and will change for every change made to the record.
The timestamp value returned has no relation with actual date or time. As such it cannot be converted to a date\time value. The timestamp is a rowversion value.
When you use the sync or delete api for the first time for a particular division, filter on timestamp greater than 1.
 [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=SyncProjectTimeCostTransactions)

### RecentCostsByNumberOfWeeks
Use this endpoint to read cost entries in the last specified number of ISO weeks.For time and billing project users to gather insight on the cost based on status by the user per week ordered by the most recent date, in turn aid in generate powerful reports. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectRecentCostsByNumberOfWeeks)

### RecentHours
This endpoint enables users to retrieve hour entries in the last 4 ISO weeks, including the current week. The list is ordered by most recent date first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectRecentHours)

### RecentHoursByNumberOfWeeks
Use this endpoint to read hour entries in the last specified number of ISO weeks.For time and billing project users to gather insight on the amount of hours based on status by the user per week ordered by the most recent date, in turn aid in generate powerful reports. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectRecentHoursByNumberOfWeeks)

### TimeAndBillingAccountDetails
This endpoint enables users to retrieve the account details. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingAccountDetails)

### TimeAndBillingAccountDetailsByID
This endpoint enables users to retrieve the account details. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingAccountDetailsByID)

### TimeAndBillingActivitiesAndExpenses
Use this endpoint to retrieve a list of Activities, Expenses and its parent Deliverable. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingActivitiesAndExpenses)

### TimeAndBillingEntryAccounts
This endpoint enables users to retrieve currently active account details. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryAccounts)

### TimeAndBillingEntryAccountsByDate
This endpoint enables users to retrieve all accounts still active as of the date provided as the parameter. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryAccountsByDate)

### TimeAndBillingEntryAccountsByProjectAndDate
This endpoint enables users to retrieve the account related to the project ID as of the date provided as the parameters.For this function to work correctly, you must supply all parameters. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryAccountsByProjectAndDate)

### TimeAndBillingEntryProjects
This endpoint enables users to retrieve project code and project description based on the project ID provided. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryProjects)

### TimeAndBillingEntryProjectsByAccountAndDate
This endpoint enables users to retrieve a list of projects allowed for an employee based on the Account and Date provided.For this function to work correctly, you must supply all parameters. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryProjectsByAccountAndDate)

### TimeAndBillingEntryProjectsByDate
Use this endpoint to read a list of projects allowed for an employee based on the Date provided.Note: For getting the list, it is mandatory to supply check date. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryProjectsByDate)

### TimeAndBillingEntryRecentAccounts
This endpoint enables users to retrieve a list of Accounts used by an employee for hour and cost entries. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryRecentAccounts)

### TimeAndBillingEntryRecentActivitiesAndExpenses
This endpoint enables users to retrieve a list of Activites and Expenses together with its corresponding parent Deliverable used by an employee for hour and cost entries. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryRecentActivitiesAndExpenses)

### TimeAndBillingEntryRecentHourCostTypes
This endpoint enables users to retrieve a list of Items used by an employee for hour and cost entries. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryRecentHourCostTypes)

### TimeAndBillingEntryRecentProjects
This endpoint enables users to retrieve a list of Projects used by an employee for hour and cost entries. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingEntryRecentProjects)

### TimeAndBillingItemDetails
This endpoint enables users to retrieve a list of Items with details. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingItemDetails)

### TimeAndBillingItemDetailsByID
Use this endpoint to read an Item used in hour and cost entries with details based on the Item Id provided. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingItemDetailsByID)

### TimeAndBillingProjectDetails
This endpoint enables users to retrieve a list of Projects with details. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingProjectDetails)

### TimeAndBillingProjectDetailsByID
Use this endpoint to read and retrieve time and billing project details based on the project ID provided. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingProjectDetailsByID)

### TimeAndBillingRecentProjects
This endpoint enables users to retrieve a list of Projects used by an employee for hour and cost entries. The list is ordered by the most recently used first. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ReadProjectTimeAndBillingRecentProjects)

### TimeCorrections
Use this endpoint to create, read, update and delete time correction to correct final time entries that already invoiced. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ProjectTimeCorrections)

### TimeTransactions
Use this endpoint to create, read, update and delete time transaction of the user.Note: For creating a time transactions, it is mandatory to supply item, project and the quantity of hour. [More info](https://start.exactonline.nl/docs/HlpRestAPIResourcesDetails.aspx?name=ProjectTimeTransactions)

## Known Issues and Limitations
- The number of API calls is limited to 60 per minute and 5000 per day, per company.

Check the [API Limits](https://support.exactonline.com/community/s/knowledge-base#All-All-DNO-Simulation-gen-apilimits) page for more details.
