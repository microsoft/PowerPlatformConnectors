## Polaris PSA Connctor

Use Polaris PSA connector to connect to your Polaris tenant and create update project/ task / resource assignments in Polaris.

## Prerequisites
In order to use this connector, you will need following:

* An account with Polaris PSA. You can request a trial here: https://www.replicon.com/polaris-psa/

## Supported Operations

### BulkGetProjectDetails3
Use this operation to get detils of Project from Polaris

### CreateProjectOrApplyModifications
Use this operation to create or update a project in Polaris. Create/ Update is done based on target parameter provided.

### UserListServiceGetData
Get Users present in Polaris.

### GetDescendantTaskDetails
Get Hierarchy of tasks present inside a Project / task

### CreateTaskHierarchyOrApplyModifications
Create a task / task hierarchy in Polaris

### Move Task
Update Hierarchy of a Task in Polaris

### TaskListServiceGetData
Get task list for a project from Polaris

### GraphQL
Perform GraphQL operations (such as Resource Assignment) related to Projects in Polaris

### GetTimesheetSummary
Get summary of time entered in a timesheet in Polaris.

### BulkGetTimeEnteredSummary
Get summary of time entered by all users against a project/ task in Polaris

### WebhookSubscriptionsRestAPI
Subscribe to the Timesheet Events in Polaris to get notification about timesheet events in Polaris.

### GetMyTenantEndpointDetails
Get tenant end point details of the connected instance.

## Obtaining Credentials
To use Polaris PSA connector, you will need an account with Project Administrator access in Polaris. You will be taken to Polaris sign in process to complete connection.

## Useful Links
* [Polaris PSA](https://www.replicon.com/polaris-psa/ "Polaris PSA")
* [Polaris Support] (https://www.replicon.com/polaris/contact-support/ "Polaris Support")

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps

Connect with Polaris Support [here](https://www.replicon.com/polaris/contact-support/) for Client Id and Client Secret for authorization

## Support

For any questions please contact Polaris Support [here](https://www.replicon.com/polaris/contact-support/)