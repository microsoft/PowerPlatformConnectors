## Varuna Power Automate Connector
Varuna is an online platform to develop software with which brands can manage their sales, service and logistics processes, and to provide all services needed for the complete digitalization of these processes. Connect to Varuna to manage your business. You can create, delete, update and use your specific documents in Varuna to automate your business.

## Publisher: Univera Computer Systems Industry and Trade Inc.

## Prerequisites
You will need the following to proceed:
* A registered tenant account for Varuna Platform   
* An ApiKey from Varuna Platform
* A Tenant Id from Varuna Platform
* A Microsoft Power Apps or Power Automate plan to use Varuna Connector;

## Varuna Subscription 
Navigate to https://www.varunasolution.com/ to get more details about Varuna Subscription.

### Obtaining Credentials
Authorization is required on all Varuna APIs. Once you have completed the Varuna registration process, please contact your account manager or info@varunasolution.com to obtain your API credentials (ApiKey & Tenant Id). These credentials will then be used on the Varuna connector authorization.


## Supported Operations
The connector supports the following operations:
* `Create a Document`: Create a document with document type and related body parameters
* `Get a Document`: Get a specific document via a document type and a document Id
* `Update a Document`: Update a document with document type and document Id
* `Delete a Document`: Delete a specific document with document type and document Id
* `Get All Documents for a Document Type`: Get All Documents for a document type
* `Webhook Trigger Subscription`: Create a subscription to get notification for a specific document and event (Create, Delete, Update)