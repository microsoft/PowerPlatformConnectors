## Replicon TBP

Use Replicon TBP connector to connect to your Replicon tenant and create update project/ task / resource assignments in Replicon.

## Prerequisites
In order to use this connector, you will need following:

* An account with Replicon. You can request a trial here: https://www.replicon.com/

## Supported Operations

### BulkGetProjectDetails3
Use this operation to get detils of Project from Replicon

### CreateProjectOrApplyModifications
Use this operation to create or update a project in Replicon. Create/ Update is done based on target parameter provided.

### UserListServiceGetData
Get Users present in Replicon.

### GetDescendantTaskDetails
Get Hierarchy of tasks present inside a project / task

### CreateTaskHierarchyOrApplyModifications
Create a task / task hierarchy in Replicon

### Move Task
Update Hierarchy of a Task in Polaris

### TaskListServiceGetData
Get task list for a project from Replicon

### GetTimesheetSummary
Get summary of time entered in a timesheet in Replicon.

### BulkGetTimeEnteredSummary
Get summary of time entered by all users against a project / task in Replicon

### GetMyTenantEndpointDetails
Get tenant end point details of the connected Replicon instance.

### WebhookSubscriptionsRestAPI
Subscribe to the Timesheet Events in Replicon to get notification about timesheet events in Replicon.

## Obtaining Credentials
To use Replicon connector, you will need an account with Project Administrator access in Replicon. You will be taken to Replicon sign in process to complete connection.

## Useful Links
* [Replicon](https://www.replicon.com/ "Replicon")
* [Replicon Support] (https://www.replicon.com/customerzone/contact-support/ "Replicon Support")

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps

Connect with Replicon Support [here](https://www.replicon.com/replicon/contact-support/) for Client Id and Client Secret for authorization

## Support

For any questions please contact Replicon Support [here](https://www.replicon.com/customerzone/contact-support/)