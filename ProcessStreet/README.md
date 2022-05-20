## Process Street Connector
Process Street is a modern process management platform for building, sharing, and automating your core processes. 
You can perform various actions such as starting, finding, and updating a workflow run in Process Street. 
Or trigger actions when a workflow starts or task status changes.

## Prerequisites
You will need the following to proceed:
* A Process Street account (you can login or sign up [here](https://app.process.st/login))
* An API Key. You can create an API Key [here](https://app.process.st/organizations/manage/integrations), 
* in the integrations tab of your organization settings. (Only admins can create and manage API keys)

## Supported Operations

The connector supports the following operations:

### Triggers

* `Task Checked/Unchecked`: Fires when a task is completed, uncompleted or both
* `Workflow Run Created`: Fires when a workflow is run
* `Workflow Run Completed`: Fires when a workflow run is completed

### Actions


#### Workflow Runs

* `Create Workflow Run`: Creates a new run based on a workflow id
* `Find Workflow Run`: Searches for existing workflow runs by name
* `Update Workflow Run`: Updates an existing workflow run

#### Form Fields

* `List Form Field Values`: Returns a batch of form fields (and their values) for a certain workflow run
* `Update Multiple Form Field Values`: Allows to set a value for form fields of a workflow run (replacing their values if present)
