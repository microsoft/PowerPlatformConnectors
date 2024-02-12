# Azure Function Apps Connector for Application Registrations
Azure Function Apps provides a powerful and very extensive REST API.
The intention is for a published solution on AppSource to have cutomers create an AAD App Registration, and have them send the credentials securely within the app (Dynamics 365). 
Note that this is just the Custom Connector, App Registration / Function App is not included. 

## Publisher: Flores.nl

## Pre-requisites
1. You have created an AAD protected Azure Function App https://learn.microsoft.com/en-us/connectors/custom-connectors/create-custom-connector-aad-protected-azure-functions
2. You have created an AAD App Registration for the Custom Connector, that gains access to the AAD protected Azure Function Apps App Registration. 
2. The application registration associated to this connector requires the delegated permission: user_impersonation. To access your function app.

## Supported Operations
The connector supports the following operations:

### POST
Post data to your Function Apps, the following properties can be send: