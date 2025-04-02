# Aranda Service Management Suite (ASMS)

Boost your company's productivity and transform your users' experience with Aranda Service Management Suite, the multi-tenant solution that allows you to manage, integrate, and automate business processes and services. Create, search, and update records stored in any workspace (project) of ASMS, including incidents, requirements, and changes, among others.

## Publisher: 

Aranda Software


## Pre-requisites

Please keep in mind the following pre-requisites to continue:

* A subscription and an instance of Aranda Service Management Suite (ASMS). Start here: https://arandasoft.com/
* An active Microsoft Power Automate subscription.
* 

## Supported Operations
|||
|:----------|:-----------|
|* `Add attachment`: |Attach a file to a specific case record in ASMS.|
|* `Add note`: |Add a note to a specific case record in ASMS.|
|* `Create case`: |Create a new case record in ASMS.|
|* `Get case`: |Gets a record for a ASMS case.|
|* `Search cases`:  |Retrieves a list of ASMS case records depending on the search criteria added in the query.|
|* `Update case`: |Updates a record for a ASMS case.|

## Obtaining Credentials

* To perform authentication, use the integration token. In the following link you will find information on how to obtain the [integration token](https://docs.arandasoft.com/asms-admin/pages/01-general/24-Tokens_integracion.html)

## Known Issues and Limitations

1. When using the action  <a href="#AddNote">Add note</a> the maximum allowed for each note is 254 characters.
2. When using the action  <a href="#searchTicket">Search cases</a> only the first 20 cases related to the added search criteria will be retrieved.


## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.