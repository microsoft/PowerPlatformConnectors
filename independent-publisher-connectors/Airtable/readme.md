# Airtable
Airtable is a cloud based spreadsheet-like service.

## Publisher: Woong Choi

## Prerequisites
You need to have an Airtable account. You can [sign up](https://airtable.com/signup) for free.

## Supported Operations
### List Records
List records in the table.

### Create a Record
Create a record in a table.

### Retrieve a Record
Get a record details in a table.

### Delete a Record
Delete a record in a table.

### Update a Record
Update a record in a table.

## Obtaining Credentials
The API key is all you need to call any of the APIs. You can access [API key](https://support.airtable.com/hc/en-us/articles/219046777-How-do-I-get-my-API-key-) from [accounts page](https://airtable.com/account).

## Getting Started
Visit Airtable [REST API](https://airtable.com/api) documentations page for further details. You may need to sign in to Airtable to view.

## Known Issues and Limitations
This connector cannot list bases as it require Airtable Metadata API. Further development is in plan for the API.

The response from the API is different for each table structure. You may need to use "Parse JSON" action to use the response to other action in Power Automate. It applies to "List Records", "Retrieve a Record", and "Update a Record" operation.

## Deployment Instructions
Inport the swagger file in target PowerApps environment.