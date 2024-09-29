# Airtable
Airtable is a versatile collaboration platform that combines the familiar layout of a spreadsheet with the power of a database. It allows users to organize information into tables, with each table containing rows and columns similar to a spreadsheet. The platform offers various field types to store different types of data, such as text, numbers, dates, and attachments, and users can customize these fields to suit their specific needs. One of Airtable's key features is its diverse range of views, including grid, calendar, kanban, and gallery views, which enable users to visualize and interact with their data in different ways. Airtable also offers blocks, which are modular apps that extend its functionality, allowing users to add features like charts, calendars, and maps to their bases. Additionally, Airtable supports real-time collaboration, enabling multiple users to work on the same base simultaneously, comment on records, tag collaborators, and track changes. With its flexibility, ease of use, and robust set of features, Airtable is a popular choice for managing various types of information and workflows.

## Publisher: [Bitcot](https://www.bitcot.com/)
## Publisher Email: dev@bitcot.com
## Prerequisites
You need to have an Airtable account. You can [sign up](https://airtable.com/signup) for free.
## Supported Operations
### Create Workspace Base
Create a base inside particular workspace that contains mutiple table with diffrent category and formatting row data.Inside particular workspace you can create multiple base.

### Retrieve List Of Bases
Get a list of all base that created inside particular workspace. 

### Create Table
Create a table inside your base. you can create multiple table with mutiple columns inside particular base.

### Retrieve List Of Tables
Get a list of all table that created inside particular base.

### Create a Record
Create a record in a table. You can create single and multiple records at same time.

### Retrieve Single Row Record
Get a single row record from table.

### Retrieve All Rows Records
Get a list of all rows record from table.

### Update Single Row Record
Update single row data by selecting particular one or more column in a table.

### Update Multiple Row Record
Update multiple row data by selecting particular one or more column in a table.

### Delete Single Record
Delete single row record from table.

### Delete Multiples Records
Delete multiples rows records from table.

### Create New Column
Create new column into a table

### Update Column
Create existing column into a table

## Obtaining Credentials
The API key is all you need to call any of the APIs. You can access [API key](https://support.airtable.com/hc/en-us/articles/219046777-How-do-I-get-my-API-key-) from [accounts page](https://airtable.com/account).

## Getting Started
Visit Airtable [REST API](https://airtable.com/api) documentations page for further details. You may need to sign in to Airtable to view.

## Known Issues and Limitations
This connector cannot list bases as it require Airtable Metadata API. Further development is in plan for the API.

The response from the API is different for each table structure. You may need to use "Parse JSON" action to use the response to other action in Power Automate. It applies to "List Records", "Retrieve a Record", and "Update a Record" operation.

## Deployment Instructions
Inport the swagger file in target PowerApps environment.