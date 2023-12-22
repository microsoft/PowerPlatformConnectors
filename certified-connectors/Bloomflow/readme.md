# Bloomflow
Bloomflow is the innovation management platform empowering large enterprises to thrive in today’s rapidly changing business landscape.  Leverage this connector to streamline collaboration and enhance productivity through efficient automated workflows.
​
## Publisher: Bloomflow
​
## Prerequisites
You need to be a Bloomflow eterprise user and your organization needs to have access to the [Blooflow Public API](https://docs.bloomflow.com/public/public-api.html) package. 
To know more about Bloomflow and our services, get in touch with us at [hello@bloomflow.com](hello@bloomflow.com).
​
## Supported Operations
The connector supports the following operations:
> Note: An item refers to a partner or a project on Blooflow.
​
* `Create Item`: Create an item object.
* `Create Document`: Add a document to an item.
* `Create Note`: Create a note linked to an item.
* `Create Application`: Create a new application for the given ApplicationForm. Application will be available in the ATS (Application portal).
* `Get item by ID`: Retrieve a specific item by id.
* `Get reference data`: Get reference data to create an item. Contains all valid values for typology, fundraising_stage, nb_employees and geographic_markets.
* `List Items`: Retrieve a list of items. Result can be filtered by *term*, *tags*, *labels*, *sources* and by *updated date*.
* `List Documents`: List all documents linked to an item.
* `List Notes`: List all notes linked to an item.
* `Update Item`: Update an item object.
​
​
​
## Supported Triggers
The connector supports the following triggers:
* `New item created`: A partner or a project is created.
* `Item property changed`: A property (name, logo, pitch, custom fields etc.) of an item is added, deleted or updated.
* `Item association changed`: An object linked (labels, tags, sources, workflows etc.) to an item is added, deleted or updated. Note: Object available in this version- workflow.
* `Workflow property changed`: A property (current step, current state, tasks etc.) is changed on a workflow. Note: Property available in this version- current step.
* `New Application created`: An application is created in the ATS (Application Portal).
​
​
## Obtaining Credentials
Bloomflow APIs use API key authorization to ensure that client requests access data securely. With API key auth, you send a key-value pair to the API in the request headers for every call.
Request header must have a **x-bflow-api-key** field containing the API key
When you are ready to work with the public API ask your credentials by contacting us at customer@bloomflow.com.
​
## Known Issues and Limitations
* Currently the `Workflow property changed` trigger only lets you track changes on Workflow Steps. This scope is likely to be expanded to cover changes in other workflow elements such as State, Groups and Tasks.
* Currently the `New Application created` trigger gives access to limited information on incoming applications. 
​
## Common errors and remedies
Here are some issues you could face while using the connector:
* `Error 400`: Appears when you make a bad request, it means that you wrongly enter the parameters. Check how to configure them correctly with our [documentation](https://docs.bloomflow.com/public/public-api.html).
* `Error 401`: Appears when you are unauthorized to access the requested information.
* `Error 403`: Appears when you wrongly enter your API Key.
​
If you have any questions about our APIs, check our [documentation](https://docs.bloomflow.com/public/public-api.html) or feel free to [contact us](hello@bloomflow.com). 
​
## Deployment Instructions
Please use the instructions on [Microsoft Power Platform Connectors CLI](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate or Power Apps.