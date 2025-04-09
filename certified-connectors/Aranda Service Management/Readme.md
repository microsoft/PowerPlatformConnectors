# Aranda Service Management Suite (ASMS)

Boost your company's productivity and transform your users' experience with Aranda Service Management Suite, the multi-tenant solution that allows you to manage, integrate, and automate business processes and services. Create, search, and update records stored in any workspace (project) of ASMS, including incidents, requirements, and changes, among others.

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
| `Create case`: |Create a new case record in ASMS.|
| `Create configuration item`: |Create a new configuration item record in Aranda CMDB.|
| `Download article attachment`: |Download an attachment related to a knowledge base (KB) article.|
| `Download case attachment`: |Download an attachment related to a case.|
| `Get case`: |Get a case record in ASMS.|
| `Get case history`: |Get the history details of a case record in ASMS.|
| `Get configuration item`: |Get a configuration item record in Aranda CMDB.|
| `Search articles`: |Retrieves a list of knowledge base (KB) articles based on the search criteria added in the query.|
| `Search cases`: |Retrieves a list of cases based on the search criteria added in the query.|
| `Search configuration items`: |Retrieves a list of configuration items based on the search criteria added in the query.|
| `Update case`: |Updates a record for an ASMS case.|
| `Update configuration item`: |Updates a record for a configuration item in Aranda CMDB.|


Date type properties are in ISO 8601 format.


## Obtaining Credentials

* To perform authentication, use the integration token. In the following link you will find information on how to obtain the [integration token](https://docs.arandasoft.com/asms-admin/pages/general/Tokens_integracion.html)

## Connector Documentation

For detailed documentation around the connector please refer to https://docs.arandasoft.com/connectors

## Known Issues and Limitations

1. When using the action  <a href="#functions">Search cases</a> only the first 50 cases related to the added search criteria will be retrieved.
2. When using the action  <a href="#functions">Search configuration items</a> only the first 50 configuration items related to the added search criteria will be retrieved.
3. When using the action  <a href="#functions">Search articles</a> only the first 20 cases related to the added search criteria will be retrieved.
4. When using the action  <a href="#functions">Create case</a>, the properties Service, Category, Groups, etc., can lose their values and leaving these properties as NULL, due to the nesting and dependency between the ticket properties. To reset the value of the ticket property click it again and select the proper value from the drop-down (if a nested value is required) or enter manually a proper identifier (such as the "Service ID" for the "Service" property)
5. When using the action <a href="#functions">Create case</a> allow updating the Risk if it is a required field in Aranda CMDB configuration.
6. When using the action <a href="#functions">Update configuration item</a>, the property "Content" of each knowledge base (KB) article can include HTML content (when the article has been created using HTML format). If your flow needs to display this content, it is recommended to use a further action which support HTML visualization or to display it in plain text (by removing HTML format)


## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.