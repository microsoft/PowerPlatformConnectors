## Process Street Connector
Process Street is a modern process management platform for building, sharing, and automating your core processes. 
You can perform various actions such as starting, finding, and updating a workflow run in Process Street. 
Or trigger actions when a workflow starts or task status changes.

## Publisher: Process Street

## Prerequisites
You will need a Process Street account (you can login or sign up [here](https://app.process.st/login)).

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
* `List Form Field Values by Task`: Returns the form field (and their values) that belong to a certain task of a workflow run
* `Update Form Field Values`: Allows to set a value for form fields of a workflow run (replacing their values if present)

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
