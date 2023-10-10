# PPM Express

PPM Express is an Online Project Portfolio Management Software that was designed to easily integrate and orchestrate your existing projects in Microsoft Planner, Azure DevOps, Jira Software, Monday.com, and Microsoft Project Online to get a bigger picture of your projects and product portfolio management in one system.

## Prerequisites

To use PPM Express with Power Apps/Power Automate and configure the integration, you will need an Enterprise PPM Express plan. To use the PPM Express API, a User license should be assigned to your account in PPM Express [PPM Express User](https://help.ppm.express/89495-ppm-express-settings/people-management?from_search=88585429#section-0) (not a Team Member). Users can request a trial tenant at: [here](https://ppm.express/trial/).

## Obtaining Credentials

An API token should be used for authentication.

To locate your API Key in your PPM Express account, please follow these steps:

1. Log into your PPM Express app account.
2. Click on the gear icon in the right upper corner of the page and select Automation.
3. Click on the New Token button under the Personal access tokens section.
Provide the name for your token, and select the expiration date and scopes.
4. Click on the Generate Token button.
5. Make sure to copy the token to your clipboard. The token is not stored and you will not be able to see it again after this window is closed.

For more details follow the [instructions](https://help.ppm.express/public-api/1177935-automation-api-settings-in-ppm-express) on our help site.

## Creating the connection

1. Log into Microsoft Power Automate or Power Apps.
2. On the left side panel click on the Dataverse menu item and then select Connections.
3. At the top of the page click New Connection and select the PPM Express connection.
4. You will be prompted to input three parameters: Tenant Region, Tenant Name, API Key. Region and tenant can be checked and copied at the URL: `https://app.ppm.express/{region}/@{tenant}`.
5. In the Tenant Region section select your Data Center location (United States/Europe).  
6. In the Tenant Name section type the name of your PPM Express tenant.
7. In the API Key section, use the API key created in your PPM Express Automation settings and click Create.
8. The connection will be created and can be used within Power Automate.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

## Supported Operations

The connector supports the following operations:

- `Get a Challenge`: Gets an existing Challenge by Id
- `Update a Challenge`: Updates an existing Challenge
- `Get Challenges List`: Gets all Challenges
- `Create a Challenge`: Creates new Challenge

- `Get fields`: Gets fields information for specified entity(e.g. Project, Idea, Resource etc)

- `Get an Idea`: Gets an existing Idea by Id
- `Update an Idea`: Updates an existing Idea
- `Get Ideas List`: Gets All Ideas or for the specified challenge
- `Create an Idea`: Creates a new Idea for the specified challenge
- `Update an Idea Stage`: Updates an Idea Stage

- `Get current user`: Gets current user and token information

- `Get a Project`: Gets a Project Info by Id
- `Update a Project`: Updates the specified Project fields
- `Get Projects List`: Gets a list of all projects
- `Create a Project`: Creates a new Project

- `Get a Key Date`: Gets a Key Date Info by Id
- `Update a Key Date`: Updates a Key Date
- `Get Key Dates List`: Gets a List of all Key Dates for the specified project
- `Create a Key Date`: Creates a new Key Date

- `Get a Task`: Gets a Task by Id and by Project
- `Update a Task`: Updates a Task
- `Get Tasks List`: Gets a List of all Tasks for the specified Project
- `Create a Task`: Creates a Task for the specified Project

- `Get a Resource`: Gets a Resource by Id
- `Update a Resource`: Updates a Resource
- `Get Resources List`: Gets a List of all Resources
- `Create a Resource`: Creates a new Resource

## Supported Triggers

The connector supports the following triggers:

`Webhook Trigger`: Triggers for any action (Create, Update, Delete) in PPM Express performed for the entity.
Here is the list of supported entities:
"Project", "Idea", "Task", "Key Date", "Resource". PPM Express lets you use up to 50 webhooks subscriptions.

## Our public API documentation

API documentation can be found [here](https://api-us.ppm.express/index.html).

## Known Issues and Limitations

- Currently, there is no dynamic schema support for the entity attributes in operations. Therefore it is not possible to provide names and descriptions for all fields. Fields for the entities (Project, Task, Idea, KeyDate) must be specified manually as JSON Object. For example:

```json
{
      "Name": "Sample Project",
      "Description": "TBD",
      "Manager": [
        {
          "id": "5d52f1b8-85d7-475e-a6e3-6f2ace2f37d0",
          "fullName": "Person 1",
          "imageId": null
        }
      ],
      "Custom Field": 123
    }
```

- Further API development is in PPM Express roadmap.

## Further Support

For any questions please feel free to [contact-us](https://ppm.express/contact-us/).
