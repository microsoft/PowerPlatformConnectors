# Aranda Service Management Suite (ASMS)

Boost your company's productivity and transform your users' experience with Aranda Service Management Suite, the multi-tenant solution that allows you to manage, integrate, and automate business processes and services. Create, search, and update records stored in any workspace (project) of ASMS, including incidents, requests, changes, tasks, and assets, among others.

## Publisher: 

Aranda Software


## Pre-requisites

Please keep in mind the following pre-requisites to continue:

* A subscription and an instance of Aranda Service Management Suite (ASMS). Start here: https://arandasoft.com/
* An active Microsoft Power Automate subscription with Premium connector capabilities.


<div id="functions"/>

## Supported Operations
|||
|:----------|:-----------|
| `Add attachment to case`: |Attach a file to a specific case record in ASMS.|
| `Add note to case`: |Add a note to a specific case record in ASMS.|
| `Create case`: |Create a new case record in ASMS (deprecated).|
| `Create case (V2)`: |Create a new case record in ASMS (Additional fields are added as dynamic fields).|
| `Create configuration item`: |Create a new configuration item record in Aranda CMDB (deprecated).|
| `Create configuration item (V2)`: |Create a new configuration item record in Aranda CMDB (Additional fields are added as dynamic fields).|
| `Create task`: |Create a new task record in ASMS, associated with a case.|
| `Download article attachment`: |Download an attachment related to a knowledge base (KB) article.|
| `Download case attachment`: |Download an attachment related to a case.|
| `Get case`: |Get a case record in ASMS.|
| `Get case history`: |Get the history details of a case record in ASMS.|
| `Get configuration item`: |Get a configuration item record in Aranda CMDB.|
| `Get task`: |Get a task record in ASMS.|
| `Search articles`: |Retrieves a list of knowledge base (KB) articles based on the search criteria added in the query.|
| `Search cases`: |Retrieves a list of cases based on the search criteria added in the query.|
| `Search configuration items`: |Retrieves a list of configuration items based on the search criteria added in the query.|
| `Search Task`: |Retrieves a list of tasks, related to a search criterion.|
| `Update case`: |Updates a record for an ASMS case (deprecated).|
| `Update case (V2)`: |Updates a record for an ASMS case (Additional fields are added as dynamic fields).|
| `Update configuration item`: |Updates a record for a configuration item in Aranda CMDB (deprecated).|
| `Update configuration item (V2)`: |Updates a record for a configuration item in Aranda CMDB (Additional fields are added as dynamic fields).|
| `Update task`: |Updates a record for an ASMS task.|


Date type properties are in ISO 8601 format.


## Obtaining Credentials

* To perform authentication, use the integration token. In the following link you will find information on how to obtain the [integration token](https://docs.arandasoft.com/asms-admin/en/pages/general/Tokens_integracion.html)

## Connector Documentation

For detailed documentation around the connector please refer to https://docs.arandasoft.com/connectors/en/

## Known Issues and Limitations

1. When using the action  <a href="#search-cases">Search cases</a> only the first 50 cases related to the added search criteria will be retrieved.
2. When using the action  <a href="#search-configuration-items">Search configuration items</a> only the first 50 configuration items related to the added search criteria will be retrieved.
3. When using the action  <a href="#search-articles">Search articles</a> only the first 20 articles related to the added search criteria will be retrieved.
4. When using the action  <a href="#search-tasks">Search tasks</a> only the first 50 tasks related to the added search criteria will be retrieved.
5. When using the action  <a href="#create-case">Create case</a>, the properties Service, Category, Groups, etc., can lose their values and leaving these properties as NULL, due to the nesting and dependency between the ticket properties. To reset the value of the ticket property click it again and select the proper value from the drop-down (if a nested value is required) or enter manually a proper identifier (such as the "Service ID" for the "Service" property)
6. When using the action <a href="#create-configuration-item">Create configuration item</a> it is allowed to update the Risk if it is a required field in Aranda CMDB configuration.
7. The property dropdowns will only show 100 items. If you need to add an item that is not in the list, you must add its identifier in the system. For additional fields, you must add the name displayed in the specialist or administrator console.
8. To add or modify any basic or additional date-type field of cases, task or CIs, it is necessary to transform or convert the date using the **built-in tools** of Power Automate.


## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.