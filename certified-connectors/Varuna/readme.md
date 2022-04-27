## Varuna Power Automate Connector

Varuna is an online platform to develop software with which brands can manage their sales, service and logistics processes, and provide all services needed for the complete digitalization of these processes. Connect to Varuna to manage your business. You can create, delete, update and use your specific documents in Varuna to automate your business.

## Publisher: Univera Computer Systems Industry and Trade Inc.

## Prerequisites

You will need the following to proceed:

* A registered tenant account for Varuna Platform
* An ApiKey from Varuna Platform
* A Tenant Id from Varuna Platform
* A Microsoft Power Apps or Power Automate plan to use Varuna Connector;

## Known Issues and Limitations

* This connector uses Varuna API.
* You can make your CRUD operations on Varuna API with this connector.
* Varuna Connector uses Azure Functions as a proxy between Power Automate and Varuna API. In case Azure Functions are not working, this connector would not work too.
* There is no validation mechanism when you are trying to connect to the Varuna connector with the wrong Tenant Id and ApiKey credentials. If you connect with the wrong credentials, you cannot use any action and trigger. Please be sure that you connected with the correct Tenant Id and ApiKey.
* Varuna provides a multi-tenant API. So, different tenants may receive different responses for the same actions. This is the normal behavior of the connector.

## Varuna Subscription

Navigate to https://www.varunasolution.com/ to get more details about Varuna Subscription.

### Obtaining Credentials

Authorization is required on all Varuna APIs. Once you have completed the Varuna registration process, please contact your account manager or *info@varunasolution.com* to obtain your API credentials (ApiKey & Tenant Id). These credentials will be used on the Varuna connector authorization.

## Supported Operations

The connector supports the following operations:

* `Create a Document`: Create a document with document type and related body parameters
* `Get a Document`: Get a specific document via a document type and a document Id
* `Update a Document`: Update a document with document type and document Id
* `Delete a Document`: Delete a specific document with document type and document Id
* `Get All Documents for a Document Type`: Get All Documents for a document type
* `Webhook Trigger Subscription`: Create a subscription to get notification for a specific document and event (Create, Delete, Update)

## Deployment Instructions

* You can download *apiProperties.json* and *apiDefinition.swagger.json* files to create the custom connector. Varuna connector does not use the Code feature of Power Automate Custom Connectors. So, you only need these 2 files. After you downloaded these files you can create your own custom connector for Varuna API. You must use paconn CLI to create a custom connector because you need to add *apiProperties.json*  to the connector files. You can follow the steps [here](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli "https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli") to create a custom connector using CLI.
