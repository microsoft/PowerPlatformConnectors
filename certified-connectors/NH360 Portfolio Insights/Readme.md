### NOTE
The NH360 Portfolio Insights connector allows users to interact with initiatives, projects, epics, etc. and data from NH360 Portfolio Insights using Microsoft Power Automate or Power Apps.

## NH360 Portfolio Insights Connector
NH360 Portfolio Insights Connector enables you to continuously connect, align and orchestrate all investments with strategies to drive business agility more effectively.
It connects disparate silos across the enterprise and provides 360 degrees of insight to help ensure that all investments are derived from – and continuously align with – strategic priorities.

## Publisher:
The North Highland Company

## Prerequisites
You will need the following to proceed:
* A NH360 Portfolio Insights subscription
* A NH360 Portfolio Insights site which allows connection requests from the connector
* User with access rights to the NH360 Portfolio Insights site

## Supported Operations
The connector supports the following operations:
* `Create entity`: Create a new entity in the NH360 Portfolio Insights site.
* `Get entity`: Returns properties of a selected entity.
* `Get entity -strict`: Returns the properties of a selected entity, provided in the Data property of the response. If an error occurs, it is returned without retry attempts.
* `List all entities`: Returns a list with all Entities from the NH360 Portfolio Insights site.
* `List all entities -strict`: Returns a list of all entities from the NH360 Portfolio Insights site. The values are included in the Data property, and in case of an error, no retries are attempted —only the error message is returned.
* `List entity types`: Returns a list with all Entity Types from the NH360 Portfolio Insights site.
* `Get entity field value`: Returns a specific Entity Field value from an entity.
* `Get entity fields values`: Returns a list with all Entity Fields and their values for a certain entity.
* `Get entity field values odata -strict`: Returns a list with all Entity Fields and their values for a certain entity. The fields are returned within the `Data` property of the response. If the OData query is malformed, an error is returned without any retry attempts.
* `Get entity fields`: Returns a list with an entity’s field names and their unique identifiers.
* `Get option set values`: Returns a list with all available values of a selected Option Set
* `List all options sets`: Returns a list with all options sets from the NH360 Portfolio Insights site.
* `Update entity field value`: Update the value of an Entity Field. For fields that use Option Sets, the Value should be set using the Option Set Value unique identifier.
* `Add resource to entity`: Assign a resource to an entity.
* `Get resources assigned to entity`: Returns all the name and unique identifier of all resources assigned to a selected entity.
* `Get financial custom field value`: Returns the value of a financial custom field.
* `Update financial custom field`: Update the values of financial custom fields.
* `Add a new entry to the entity history log`: Add a log entry to the selected entity's history.
* `List entity history entries`: Retrieve a list containing of all history entries for the selected entity.
* `Get the entity's stage validation status`: Indicates whether the selected entity passes all validations for the selected stage transition.
* `Execute Stage Transition`: Executes an entity lifecycle stage transition.


## Supported Triggers:
The connector supports the following triggers:
* `On Entity Created`: Executed when a new entity is created in the NH360 Portfolio Insights site.
* `On Entity Deleted`: Executed when one or more entities are deleted from the NH360 Portfolio Insights site.
* `On Entity Updated`: Executed when entity fields are updated.
* `On Financial Values Change`: Executed when a financial value is changed.
* `On Resource Assignment Created`: Executed when one or more resources are added to an entity’s resource plan.
* `On Resource Assignment Removed`: Executed when one or more resources are deleted.
* `On Resource Assignment Updated`: Executed when one or more resources are re-assigned.
* `On Milestone Created`: Executed when a milestone is created in the NH360 Portfolio Insights site.
* `On Milestone Deleted`: Executed when a milestone is deleted in the NH360 Portfolio Insights site.
* `On Milestone Updated`: Executed when a milestone is edited in the NH360 Portfolio Insights site.
* `On Relationship Created`: Executed each time a new relationship is created in the NH360 Portfolio Insights site.
* `On Relationship Deleted`: Executed each time a new relationship is deleted in the NH360 Portfolio Insights site.
* `On Relationship Updated`: Executed each time a new relationship is edited in the NH360 Portfolio Insights site.
* `On Process Stage Transition`: Executed when an entity changes its workflow stage.
* `On Change Request Created`: Executed when a new Change Request is created.
* `On Change Request Deleted`: Executed when a “Change request” is deleted.
* `On Change Request Status Changed`: Executed when a Change Request status is changed. Status values: Draft = 0, Pending = 1, Approved = 2, Rejected = 3, None = 4.
* `On Change Request Updated`: Executed when a Change Request is updated.
* `On Actuals Approval Workflow Started`: Executed when Actuals Approval workflow starts
* `On Actuals Period Status Changed`: Executed when the status of an Actuals Period has been changed. Status values: Any = 0, Opened = 1, Locked = 2, Archived = 3, Waiting For Approval = 4, Open For Edit = 5, Processing = 6, Not Open = 7.

## Get access to NH360 Portfolio Insights
Contact us to start using NH360 Portfolio Insights: [Contact Us](https://www.northhighland.com/contact-us)

## Known Issues and Limitations
No known issues or limitations

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
