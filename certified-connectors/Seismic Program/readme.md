# Seismic Program connector

The Seismic Program connector provides a set of actions and triggers to interact with programs, tasks and requests.

## Publisher: Seismic

## Prerequisites

You need a Seismic Premium user account for the tenant.

## Supported Operations

### Get comments

Get a list of comments on a single node (program, task, request).

### Get a comment

Get a single comment on a single node (program, task, request).

### Create a comment

Create a comment on a single node (program, task, request).

### Update a comment

Update an existing comment on a single node (program, task, request).

### Delete a comment

Delete a comment on a single node (program, task, request).

### Get programs

Get a list on programs in a tenant.

### Get a program

Get a single program in a tenant.

### Create a program

Create a program in a tenant.

### Update a program

Update an existing program in a tenant.

### Delete programs

Delete a list of programs in a tenant.

### Delete a program

Delete a program in a tenant.

### Get requests

Get a list of requests in a tenant.

### Get a request

Get a single request in a tenant.

### Create a request

Create a request in a tenant.

### Update a request

Update a single request in a tenant.

### Delete a request

Delete a single request in a tenant.

### Delete requests

Delete a list of requests in a tenant.

### Get tasks

Get a list of tasks in a tenant.

### Get a task

Get a single task in a tenant.

### Create a task

Create a task in a tenant.

### Update a task

Update a single task in a tenant.

### Delete a task

Delete a single task in a tenant.

### Delete tasks

Delete a list of tasks in a tenant.

### Get status schemas

Get a list of status schemas.

### Get status schema

Get a single status schema

### Trigger: When a program is created

A trigger that uses the ProgramCreate webhook to return program data after it has been created

### Trigger: When a program is updated

A trigger that uses the ProgramUpdate webhook to return program data after it has been updated

### Trigger: When a program is deleted

A trigger that uses the ProgramDelete webhook to return program data after it has been deleted

### Trigger: When a task is created

A trigger that uses the PlannerTaskCreate webhook to return task data after it has been created

### Trigger: When a task is updated

A trigger that uses the PlannerTaskUpdate webhook to return task data after it has been updated

### Trigger: When a task is deleted

A trigger that uses the PlannerTaskDelete webhook to return task data after it has been deleted

### Trigger: When a request is created

A trigger that uses the PlannerRequestCreate webhook to return request data after it has been created

### Trigger: When a request is updated

A trigger that uses the PlannerRequestUpdate webhook to return request data after it has been updated

### Trigger: When a request is deleted

A trigger that uses the PlannerRequestDelete webhook to return request data after it has been deleted

## Obtaining Credentials

You need a Seismic Premium user account for the tenant. Contact your Seismic system administrator if you do not have sufficient permissions.


## Getting Started

The Seismic Programs connector includes the following actions. Each action corresponds to an API endpoint. Refer to the article for each corresponding API endpoint for further information on the fields and properties associated with the action.

- [Get comments](https://developer.seismic.com/seismicsoftware/reference/programs-get-comments)
- [Get a comment](https://developer.seismic.com/seismicsoftware/reference/programs-get-comment)
- [Create a comment](https://developer.seismic.com/seismicsoftware/reference/programs-create-comment)
- [Update a comment](https://developer.seismic.com/seismicsoftware/reference/programs-update-comment)
- [Delete a comment](https://developer.seismic.com/seismicsoftware/reference/programs-delete-comment)
- [Get programs](https://developer.seismic.com/seismicsoftware/reference/get-programs)
- [Get a program](https://developer.seismic.com/seismicsoftware/reference/get-program)
- [Create a program](https://developer.seismic.com/seismicsoftware/reference/create-program)
- [Update a program](https://developer.seismic.com/seismicsoftware/reference/update-program)
- [Delete programs](https://developer.seismic.com/seismicsoftware/reference/delete-programs)
- [Delete a program](https://developer.seismic.com/seismicsoftware/reference/delete-program)
- [Get requests](https://developer.seismic.com/seismicsoftware/reference/programs-get-requests)
- [Get a request](https://developer.seismic.com/seismicsoftware/reference/programs-get-request)
- [Create a request](https://developer.seismic.com/seismicsoftware/reference/programs-create-request)
- [Update a request](https://developer.seismic.com/seismicsoftware/reference/programs-update-request)
- [Delete requests](https://developer.seismic.com/seismicsoftware/reference/programs-delete-requests)
- [Delete a request](https://developer.seismic.com/seismicsoftware/reference/programs-delete-request)
- [Get tasks](https://developer.seismic.com/seismicsoftware/reference/programs-get-tasks)
- [Get a task](https://developer.seismic.com/seismicsoftware/reference/programs-get-task)
- [Create a task](https://developer.seismic.com/seismicsoftware/reference/programs-create-task)
- [Update a task](https://developer.seismic.com/seismicsoftware/reference/programs-update-task)
- [Delete tasks](https://developer.seismic.com/seismicsoftware/reference/programs-delete-tasks)
- [Delete a task](https://developer.seismic.com/seismicsoftware/reference/programs-delete-task)
- [Trigger: program create](https://developer.seismic.com/seismicsoftware/docs/programcreatev1)
- [Trigger: program update](https://developer.seismic.com/seismicsoftware/docs/programupdatev1)
- [Trigger: program delete](https://developer.seismic.com/seismicsoftware/docs/programdeletev1)
- [Trigger: task create](https://developer.seismic.com/seismicsoftware/docs/plannertaskcreatev1)
- [Trigger: task update](https://developer.seismic.com/seismicsoftware/docs/plannertaskupdatev1)
- [Trigger: task delete](https://developer.seismic.com/seismicsoftware/docs/plannertaskdeletev1)
- [Trigger: request create](https://developer.seismic.com/seismicsoftware/docs/plannerrequestcreatev1)
- [Trigger: request update](https://developer.seismic.com/seismicsoftware/docs/plannerrequestupdatev1)
- [Trigger: request delete](https://developer.seismic.com/seismicsoftware/docs/plannerrequestdeletev1)


## Known Issues and Limitations

No issues and limitations are known at this time. All APIs operate in accordance with Seismic API policy, including Rate Limits. Please refer to the [Seismic Developer portal](https://developer.seismic.com/) for API specifications, restrictions, and standards.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.