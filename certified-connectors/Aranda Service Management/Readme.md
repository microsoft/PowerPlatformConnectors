## Aranda Service Management Suite (ASMS)

Boost your company's productivity and transform your users' experience with Aranda Service Management Suite, the multi-tenant solution that allows you to manage, integrate, and automate business processes and services. Create, search, and update records stored in any workspace (project) of ASMS, including incidents, requirements, and changes, among others.

This connector is available in the following products and regions:

|Service|	Class|	Regions|
|:----------|:-------:|:---|
|Power Automate|Premium|All Power Automate regions except the following: <br> - US Government (GCC) <br> - US Government (GCC High)	<br> - China Cloud operated by 21Vianet	<br> - US Department of Defense (DoD)|



|**Contact**||
|:--------- |:-------|
|name|Aranda Software|
|url|https://arandasoft.com/|
|email|soporte@arandasoft.com|


|**Connector Metadata**||
|:----------|:-------|
|Categories|IT Operations|
|Privacy policy|https://arandasoft.com/politica-de-privacidad/|
|Publisher||
|Website|https://arandasoft.com/|


## Known Issues and Limitations

1. When using the action  <a href="#AddNote">Add note</a> the maximum allowed for each note is 254 characters.
2. When using the action  <a href="#searchTicket">Search cases</a> only the first 20 cases related to the added search criteria will be retrieved.


## Pre-requisites

Please keep in mind the following pre-requisites to continue:

* A subscription and an instance of Aranda Service Management Suite (ASMS). Start here: https://arandasoft.com/

## Getting credentials

* To perform authentication, use the integration token. In the following link you will find information on how to obtain the [integration token](https://docs.arandasoft.com/asms-admin/pages/01-general/24-Tokens_integracion.html)

## Actions
|||
|:----------|:-----------|
|* `Add attachment`: |Attach a file to a specific case record in ASMS.|
|* `Add note`: |Add a note to a specific case record in ASMS.|
|* `Create case`: |Create a new case record in ASMS.|
|* `Get case`: |Gets a record for a ASMS case.|
|* `Search cases`:  |Retrieves a list of ASMS case records depending on the search criteria added in the query.|
|* `Update case`: |Updates a record for a ASMS case.|

<br>

### Get case

Operation ID: GetCase

Gets a record for a ASMS case.

#### **Parameters**

|Name	|Key	|Required|	Type|	Description|
|----------|:-------------:|------|:---|:---|
|Case Code|ticketId|	true|	string|Case Code |

#### **Returns**

|Name	|Key	|	Type|	Description|
|----------|:-------------|----------|:------|
|Additional Fields|additionalFields|array|Case additional fields, depending on the case retrieved|
|Applicant ID|applicantId|integer|Applicant ID|
|Author ID|authorId|integer|Author ID|
|Author|authorName|string|Author name (e.g "John Doe")|
|Case Code|idByProject|string|Case Code (e.g "REQ-51-12-123")| 
|Case ID|id|integer|System ID of the attachment record for which to retrieve the metadata|
|Case Type ID|itemTypeId|integer|Case type ID|
|Case Type|itemTypeName|string|Case type (e.g "Service Call")|
|Category ID|categoryId|integer|Category ID|
|Category|categoryName|string|Category name (e.g "Vacation Request")|
|CI ID|ciId|integer|Configuration item ID|
|CI|ciName|string|Main configuration item name|
|Client ID|customerId|integer|Client ID|
|Client|customerName|string|Client name (e.g "Jane Doe")|
|Closed Date|closedDate|string|Closed date|
|Closed|isClosed|boolean|Closed|
|Company ID|companyId|integer|Company ID|
|Company|companyName|string|Company name (e.g "ACME Inc.")|
|Costs|cost|number|Costs|
|Description|description|string|Detailed description of the case|
|Estimated Cost|estimatedCost|number|Estimated cost|
|Estimated Date|estimatedDate|string|Estimated date|
|Final date|finalDate|string|Final date (only for Task records)|
|Group ID|groupId|integer|Responsible group ID|
|Group|groupName|string|Responsible group name (e.g "ACME HR Team")|
|Impact ID|impactId|integer|Impact ID|
|Impact|impactName|string|Impact name (e.g "Low")|
|Interface ID|interfaceId|string|Interface ID|
|Model ID|modelId|integer|Model ID|
|Model|modelName|string|Model name (e.g "Standard HR Request")|
|OLA ID|olaId|integer|OLA ID|
|OLA|olaName|string|OLA name|
|Organizational Area ID|unitId|integer|Organizational area ID|
|Organizational Area|unitName|string|Organizational area name|
|Price|price|number|Price|
|Priority ID|priorityId|integer|Priority ID|
|Priority|priorityName|string|Priority name (e.g "Medium")|
|Progress|currentProgress|integer|Progress|
|Project ID|projectId|integer|Project ID|
|Project|projectName|string|Project name (e.g "Human Resources")|
|Provider ID|providerId|integer|Provider ID|
|Provider|providerName|string|Provider name|
|Real Cost|realCost|number|Real cost|
|Real Date|realDate|string|Real date|
|Reason ID|reasonId|integer|Reason ID|
|Reason|reasonName|string|Reason name (e.g "Closed successfully")|
|Registry Date|openedDate|string|Registry date|
|Registry Type ID|registryTypeId|integer|Registry type ID|
|Registry Type|registryTypeName|string|Source of the case (e.g "Web Portal")|
|Responsible ID|responsibleId|integer|Responsible ID|
|Responsible|responsibleName|string|Responsible name (e.g "Richard Smith")|
|Risk ID|riskId|integer|Risk ID|
|Risk|riskName|string|Risk name (only for Change/Release records)|
|Service ID|serviceId|integer|Service ID|
|Service|serviceName|string|Service name (e.g "Employee Services")|
|SLA ID|slaId|integer|SLA ID|
|SLA|slaName|string|SLA name (e.g "Standard Agreement")|
|Solution|commentary|string|Case solution|
|Status ID|stateId|integer|Status ID|
|Status|stateName|string|Status name (e.g "Closed")|
|Subject|subject|string|Case subject|
|UC ID|ucId|integer|UC ID|
|UC|ucName|string|UC name|
|Urgency ID|urgencyId|integer|Urgency ID|
|Urgency|urgencyName|string|Urgency name (e.g "High")|

<br>

### Create case

Operation ID: CreateCase

Create a new case record in ASMS.

#### **Parameters**

|Name	|Key	|Required|	Type|	Description|
|----------|:-------------:|:------:|:------|:-------------:|
|Additional Fields|additionalFields|--|array|Case additional fields, depending on the case to create|
|Applicant ID|applicantId|--|integer|Applicant ID|
|Case Type ID|itemTypeId|true|integer|Case type ID|
|Category ID|categoryId|true|integer|Category ID|
|CI ID|ciId|--|integer|Configuration item ID|
|Client ID|customerId|--|integer|Client ID|
|Company ID|companyId|--|integer|Company ID|
|Description|description|--|string|Detailed description of the case|
|Group ID|groupId|--|integer|Responsible group ID|
|Impact ID|impactId|--|integer|Impact ID|
|Organizational Area ID|unitId|--|integer|Organizational area ID|
|Project ID|projectId|true|integer|Project ID|
|Provider ID|providerId|--|integer|Provider ID|
|Reason ID|reasonId|--|integer|Reason ID|
|Registry Type ID|registryTypeId|--|integer|Registry type ID|
|Responsible ID|responsibleId|--|integer|Responsible ID|
|Service ID|serviceId|true|integer|Service ID|
|Status ID|stateId|--|integer|Status ID|
|Subject|subject|true|string|Case subject|
|Urgency ID|urgencyId|--|integer|Urgency ID|

***--Note:  The parameters required to create a new case depend on the configuration settings in Aranda Service Management Suite (Administrator), according to the project and type of case to create.*** 

#### **Returns**

|Name	|Key	|	Type|	Description|
|----------|:-------------:|----------|:------|
|Case Code|idByProject|string|Case code (e.g "INC-51-12-123")|
|Case ID|id|integer|Code of the record for which metadata will be attached.|


### Update Case

Operation ID: UpdateCase

Updates a record for a ASMS case.

#### **Parameters**

|Name	|Key|Required|Type|Description|
|-------|:--|:-------|:---|:----------|
|Additional Fields|additionalFields|--|array|Case additional fields, depending on the case to update|
|Applicant ID|applicantId|--|integer|Applicant ID|
|Case code|id|true|string|Case code (e.g 1271)|
|Category ID|categoryId|--|integer|Category ID|
|CI ID|ciId|--|integer|Configuration item ID|
|Client ID|customerId|--|integer|Client ID|
|Company ID|companyId|--|integer|Company ID|
|Description|description|--|string|Detailed description of the case|
|Group ID|groupId|--|integer|Responsible group ID|
|Impact ID|impactId|--|integer|Impact ID|
|Organizational Area ID|unitId|--|integer|Organizational area ID|
|Provider ID|providerId|--|integer|Provider ID|
|Reason ID|reasonId|--|integer|Reason ID|
|Registry Type ID|registryTypeId|--|integer|Registry type ID|
|Responsible ID|responsibleId|--|integer|Responsible ID|
|Service ID|serviceId|--|integer|Service ID|
|Status ID|stateId|--|integer|Status ID|
|Subject|subject|--|string|Case subject|
|Urgency ID|urgencyId|--|integer|Urgency ID|

****Note: The parameters required to update a case record depend on the configuration settings in Aranda Service Management Suite (Administrator), according to the project and type of case to create.*** 

#### **Returns**

|Name	|Key	|	Type|	Description|
|----------|:-------------:|----------|:------|
|Result|result|boolean|Specifies the success of the case update action.|

<div id="searchTicket"/>

### Search cases

Operation ID: SearchTickets

Retrieves a list of ASMS case records depending on the search criteria added in the query.

#### **Parameters**

|Name	|Key	|Required|	Type|	Description|
|----------|:-------------|------:|:---|:---|
|Filters|filters|true|array|Array of search criteria|
|Relation between filters|logicOperator|true|string|Specifies the relation between search criteria|


##### **Search criteria (filters)**


|Name	|Key	|Required|	Type|Description|
|-------|:------|:-------|:-----|:----------|
|Field to filter|field|true|string|Case field to filter (e.g "stateName")|
|Search operator|operator|true|string|Search operator (e.g "EqualTo")|
|Value to find|value|true|string|Case value to find (e.g "Closed")|


#### **Returns**

Returns an array with the details of the registered cases that match the search criteria.

|Name	|Key	|	Type|	Description|
|----------|:-------------|----------|:------|
|Applicant ID|applicantId|integer|Applicant ID|
|Author ID|authorId|integer|Author ID|
|Author|authorName|string|Author name (e.g "John Doe")|
|Case Code|idByProject|string|Case Code (e.g "REQ-51-12-123")| 
|Case ID|id|integer|System ID of the attachment record for which to retrieve the metadata|
|Case Type ID|itemTypeId|integer|Case type ID|
|Case Type|itemTypeName|string|Case type (e.g "Service Call")|
|Category ID|categoryId|integer|Category ID|
|Category|categoryName|string|Category name (e.g "Vacation Request")|
|Client ID|customerId|integer|Client ID|
|Client|customerName|string|Client name (e.g "Jane Doe")|
|Company ID|companyId|integer|Company ID|
|Company|companyName|string|Company name (e.g "ACME Inc.")|
|Description|description|string|Detailed description of the case|
|Group ID|groupId|integer|Responsible group ID|
|Group|groupName|string|Responsible group name (e.g "ACME HR Team")|
|Impact ID|impactId|integer|Impact ID|
|Impact|impactName|string|Impact name (e.g "Low")|
|Model ID|modelId|integer|Model ID|
|Model|modelName|string|Model name (e.g "Standard HR Request")|
|Project ID|projectId|integer|Project ID|
|Project|projectName|string|Project name (e.g Human Resources)|
|Reason ID|reasonId|integer|Reason ID|
|Reason|reasonName|string|Reason name (e.g "Closed successfully")|
|Responsible ID|responsibleId|integer|Responsible ID|
|Responsible|responsibleName|string|Responsible name (e.g "Richard Smith")|
|Service ID|serviceId|integer|Service ID|
|Service|serviceName|string|Service name (e.g "Employee Services")|
|SLA ID|slaId|integer|SLA ID|
|SLA|slaName|string|SLA name (e.g "Standard Agreement")|
|Status ID|stateId|integer|Status ID|
|Status|stateName|string|Status name (e.g Closed)|
|Subject|subject|string|Case subject|

<br>

### Add Attachment

Operation ID: AttachFile

Attach a file to a specific case record in ASMS.

#### **Parameters**

|Name	|Key	|Required|	Type|	Description|
|:----------|:-------------|:------|:---|:---|
|Case ID|ticketId|true|string|Code of the record for which metadata will be attached.|
|File Name|fileName|true|string|File name (e.g "Vacation_Approval_Form.docx")|
|File|file|true|byte|File|

****Nota: The maximum file size to attach depends on the configuration settings in Aranda Service Management Suite (Administrator), according to the project.*** 

#### **Returns**

The results of this operation are dynamic.


<div id="AddNote"/>

<br>

### Add note

Operation ID: AttachNote

Add a note to a specific case record in ASMS.

#### **Parameters**

|Name	|Key	|Required|Type|	Description|
|-------|:------|:------|:---|:---|
|Case ID|ticketId|true|string|Code of the record for which metadata will be attached.| 
|Note|message|true|string|Note to be attached to the record.|

#### **Returns**

The results of this operation are dynamic.