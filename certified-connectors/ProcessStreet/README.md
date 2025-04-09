## Process Street Connector
Process Street is a modern process management platform for teams.
You can use this connector to run workflows, find and update workflow runs, and trigger actions when a workflow is run or changes.

## Publisher: Process Street

## Prerequisites
You will need a Process Street account (you can login or sign up [here](https://app.process.st/login)).

## Supported Operations

The connector supports the following operations:

### Triggers

* `When a task is checked, unchecked or ready`: A trigger that will fire each time a task is completed, uncompleted or both, or becomes ready in Process Street
* `When a workflow is run`: Triggers when a workflow is run
* `When a workflow run is completed`: Triggers when a workflow run is completed

### Actions

#### Workflow Runs

* `Run a workflow`: Starts a workflow run based on a workflow
* `Find workflow runs`: Retrieves the most recent 200 active workflow runs that match the query
* `Update a workflow run`: Updates a workflow run

#### Form Fields

* `Get workflow run form fields`: Retrieves all the form fields of workflow run
* `Update workflow run form fields`: Updates the workflow run form fields

## Obtaining Credentials
The connector uses an API Key as its authentication method.
You can create an API Key [here](https://app.process.st/organizations/manage/integrations),
in the integrations tab of your organization settings. Keep in mind that only Process Street admins can create and manage API keys.

## Known Issues and Limitations
There are currently no known issues in the Process Street connector.

## Deployment Instructions
In order to deploy this connector as a custom connector you just need
to download the `apiDefinition.swagger.json` file. Then you should visit https://us.flow.microsoft.com/en-us/ and create
a new connector in `Data > Custom Connectors`. Use the "Import from OpenAPI file" option,
selecting the previously downloaded file. That should be all.
