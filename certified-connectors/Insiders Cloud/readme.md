# Insiders Cloud connector
The Insiders Cloud is used to automate document processing. This Powerautomate app enables customers of the Insiders Cloud to analyze their documents.

## Publisher: Insiders Technologies GmbH

## Prerequisites
- Username and password of your Cloud api user
- A deployed subsystem in the Insiders Cloud

## Supported Operations
### `Analyze Document`
Analyzes a document for a specific subsystem and category and returns its fields.

## Obtaining Credentials
Credentials for a admin user can be acquired of a consultant of Insiders or an Insiders Partner. 
With the credentials, new users can be create in the User Management in the cloud platform. 

## Known Issues and Limitations
There are no known issues at this time.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector should you wish to do so in Microsoft Power Automate and Power Apps.

Also take a look at the description of task-meta-data in the swagger.json and format the field in the connector accordingly.
