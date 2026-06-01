### NOTE
The NH360 Portfolio Insights connector allows users to interact with initiatives, projects, epics, etc. and data from NH360 Portfolio Insights using Microsoft Power Automate or Power Apps.

## NH360 Portfolio Insights Connector
NH360 Portfolio Insights connector enables you to continuously connect, align and orchestrate all investments with strategies to drive business agility more effectively. It connects disparate silos across the enterprise and provides 360 degrees of insight to help ensure that all investments are derived from – and continuously align with – strategic priorities. Connect to NH360 Portfolio Insights to plan, manage, and adjust your initiatives, projects, epics and more.

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
* `Create entity -strict`: Create a new entity in the NH360 Portfolio Insights site. Always returns HTTP 200. If an error occurs, it is included in the response's error list without any retry attempts.
* `Delete entity`: Deletes an existing entity.
* `Get entity`: Returns properties of the selected entity.
* `Get entity -strict`: Returns properties of the selected entity. Always returns HTTP 200. If an error occurs, it is included in the response's error list without any retry attempts.
* `List all entities`: Returns a list with all entities from the NH360 Portfolio Insights site.
* `List all entities -strict`: Returns a list with all entities from the NH360 Portfolio Insights site. Always returns HTTP 200. If an error occurs, it is included in the response's error list without any retry attempts.
* `List entity types`: Returns a list with all entity types from the NH360 Portfolio Insights site.
* `Get entity field value`: Returns the value of a specific entity field for the selected entity.
* `Get entity fields values`: Returns a list with all entity fields and their values for the selected entity.
* `Get entity field values odata -strict`: Returns a list with all entity fields and their values for the selected entity. The fields are returned within the `Data` property of the response. Always returns HTTP 200. If the OData query is malformed, the error is included in the response's error list without any retry attempts.
* `Get entity fields`: Returns a list with all fields in the NH360 Portfolio Insights site, including their unique identifiers.
* `Update entity field value`: Update the value of a single entity field. For fields that use option sets, the value should be set using the option set value's unique identifier.
* `Update entity field value -strict`: Update the value of a single entity field. For fields that use option sets, the value should be set using the option set value's unique identifier. Always returns HTTP 200. If an error occurs, it is included in the response's error list without any retry attempts.
* `Update entity fields values`: Update the values of multiple entity fields. For fields that use option sets, each value should be set using the option set value's unique identifier.
* `Update entity fields values -strict`: Update the values of multiple entity fields. For fields that use option sets, each value should be set using the option set value's unique identifier. Always returns HTTP 200. If an error occurs, it is included in the response's error list without any retry attempts.
* `Get option set values`: Returns a list with all available values of the selected option set.
* `List all option sets`: Returns a list with all option sets from the NH360 Portfolio Insights site.
* `Add resource to entity`: Assign a resource to an entity.
* `Get resources assigned to entity`: Returns all the names and unique identifiers of all resources assigned to the selected entity.
* `Create entity relationship`: Create a new relationship between two entities.
* `Get financial custom field value`: Returns the value of a financial custom field for the selected entity.
* `Update financial custom field`: Update the values of a single financial custom field for the selected entity. For fields that use option sets, the value should be set using the option set value's unique identifier.
* `Update financial custom fields values`: Update the values of multiple financial custom fields for the selected entity. For fields that use option sets, each value should be set using the option set value's unique identifier.
* `Add a new entry to the entity's history log`: Add an entry to the entity history of the selected entity.
* `List entity history entries`: Retrieve a list of all entries in the entity history of the selected entity.
* `Get the entity's stage validation status`: Returns the stage validation status for the selected entity.
* `Execute Stage Transition`: Executes an entity stage transition.
* `Get lifecycle approval requests by approver`: Returns all lifecycle approval requests for which the current user is an approver.
* `Get lifecycle approval requests by entity`: Returns all lifecycle approval requests for the selected entity.
* `Get lifecycle approval request details`: Retrieves detailed information for a specific lifecycle approval request.
* `Set lifecycle approval response`: Updates the response of the selected lifecycle approval request.
* `Read milestones of an entity`: Returns the milestones for the selected entity.
* `Create milestone`: Creates a new milestone for an entity.
* `Read milestone`: Reads the properties of the selected milestone.
* `Update milestone`: Updates the properties of the selected milestone.
* `Read milestone field value`: Reads the value of a single milestone field for the selected milestone.
* `Read milestone field values`: Reads the values of multiple milestone fields for the selected milestone.
* `Update milestone field value`: Updates the value of a single milestone field for the selected milestone.
* `Update milestone field values`: Updates the values of multiple milestone fields for the selected milestone.


## Supported Triggers:
The connector supports the following triggers:
* `On Entity Created`: Executed when a new entity is created in the NH360 Portfolio Insights site.
* `On Entity Updated`: Executed when entity fields are updated.
* `On Entity Deleted`: Executed when one or more entities are deleted from the NH360 Portfolio Insights site.
* `On Financial Values Change`: Executed when a financial value is changed.
* `On Process Stage Transition`: Executed when an entity changes its workflow stage.
* `On Resource Assignment Created`: Executed when one or more resources are assigned to an entity's resource plan.
* `On Resource Assignment Removed`: Executed when one or more resource assignments are deleted from an entity's resource plan.
* `On Resource Assignment Updated`: Executed when one or more resources are reassigned.
* `On Milestone Created`: Executed when a milestone is created in the NH360 Portfolio Insights site.
* `On Milestone Updated`: Executed when a milestone is edited in the NH360 Portfolio Insights site.
* `On Milestone Deleted`: Executed when a milestone is deleted from the NH360 Portfolio Insights site.
* `On Relationship Created`: Executed each time a new relationship is created in the NH360 Portfolio Insights site.
* `On Relationship Updated`: Executed each time a relationship is edited in the NH360 Portfolio Insights site.
* `On Relationship Deleted`: Executed each time a relationship is deleted from the NH360 Portfolio Insights site.
* `On Change Request Created`: Executed when a new change request is created.
* `On Change Request Updated`: Executed when a change request is updated.
* `On Change Request Deleted`: Executed when a change request is deleted.
* `On Change Request Status Changed`: Executed when a change request status is changed. Status values: Draft = 0, Pending = 1, Approved = 2, Rejected = 3, None = 4.
* `On Actuals Approval Workflow Started`: Executed when actuals approval workflow starts. (Not supported in NH360 Portfolio Insights 7.)
* `On Actuals Period Status Changed`: Executed when the status of an actuals period has been changed. Status values: Any = 0, Opened = 1, Locked = 2, Archived = 3, Waiting For Approval = 4, Open For Edit = 5, Processing = 6, Not Open = 7. (Not supported in NH360 Portfolio Insights 7.)
* `On Lifecycle Approval Request Created`: Executed when a new lifecycle approval request is created for an approver.
* `On Lifecycle Approval Request Changed`: Executed when a lifecycle approval request status changes.

## Get access to NH360 Portfolio Insights
Contact us to start using NH360 Portfolio Insights: [Contact Us](https://www.northhighland.com/contact-us)

## Known Issues and Limitations
No known issues or limitations

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
