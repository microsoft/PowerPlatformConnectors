# Title
Zellis HCM contains operations to allow you to easily send and receive employee information between Zellis HCM and your other systems. 

## Publisher: Publisher's Name
Zellis 

## Prerequisites
To use the connector, you will need credentials for the Zellis Intelligence Platform API suite and Notification Hub. Details on how to do this are available on the Zellis Customer Help Centre.   

## Supported Operations

### Entity Trigger
Trigger for webhook

### Validate Notification
To validate incoming webhook notification

### Create Zellis record
Create Zellis standard record available from list, e.g. Worker Attendance in Zellis Intelligence Platform.

### Partially amend Zellis record
Partially amend Zellis standard Zellis record available from list, e.g. Worker, via its WorkerNumber.

### Search Zellis records
Retrieve a list of Zellis standard records, e.g. Workers, that matches your search criteria.

### Update Zellis record
Update Zellis standard record available from list, e.g. Fixed Payment in Zellis Intelligence Platform.

## Obtaining Credentials
1) Please use [these instructions](https://learn.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app) to get the client id & secret. Use https://global.consent.azure-apim.net/redirect as redirect URI.
2) Get ZIP oData Server Client Id, Zellis Domain, ZIP Read as well as Write Context, Notification Hub Domain, Notification Hub Context, APIM Subscription Key and HMAC Signature Key from Zellis IT Team
3) Form Scope as api://oData-server-client-id/.default
4) Form Resource URI as api://oData-server-client-id

## Getting Started
For detailed documentation around the connector please refer to the Zellis Customer Help Centre.

## Known Issues and Limitations
Pagination does not function correctly for the Search Zellis Records action.

## Frequently Asked Questions

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
