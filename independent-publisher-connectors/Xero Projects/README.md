# Xero Projects
The Xero accounting software uses a single unified ledger, which allows users to work in the same set of books regardless of location or operating system. Its features include automatic bank feeds, invoicing, accounts payable, expense claims, fixed asset depreciation, purchase orders, bank reconciliations, and standard business and management reporting. Xero Projects allows businesses to track time and costs on projects/jobs and report on profitability.

## Publisher: Hitachi Solutions

## Prerequisites
To use this connector, you need the following

- A Microsoft Power Apps or Power Automate plan with custom connector feature

- A Xero account with either the demo company or a paid subscription tenant

- A Xero developer account with a configured OAuth 2.0 application


## Obtaining Credentials
After signing in at [developer.xero.com](https://developer.xero.com/), navigate to the My Apps section and create a new app. Give it a descriptive name and choose Web App as the integration type. Enter your company URL and for the OAuth 2.0 redirect URIs field, use `https://global.consent.azure-apim.net/redirect`. Agree to the terms and conditions and create the app. Copy the client ID created and then generate and copy the client secret - these will be used when configuring the custom connector.

## Getting Started
Follow the guide provided by Xero on [developer.xero.com](https://developer.xero.com/documentation/getting-started/getting-started-guide).

## API Documentation
[Xero Projects API](https://developer.xero.com/documentation/projects/overview-projects)

- [Projects](https://developer.xero.com/documentation/projects/projects)

- [Tasks](https://developer.xero.com/documentation/projects/tasks)

## Supported Operations
This connector supports the following operations:

### Action: Retrieve a project
Allows you to retrieve a project.
### Action: Retrieve projects
Allows you to retrieve a list of projects.
### Action: Create a project
Allows you to create a new project.
### Action: Update a project
Allows you to update a project.
### Action: Update a project status
Allows you to update a project status.
### Action: Retrieve a task
Allows you to retrieve a task.
### Action: Retrieve tasks
Allows you to retrieve a list of tasks.
### Action: Create a task
Allows you to create a task.
### Action: Update a task
Allows you to update a task.
### Action: Delete a task
Allows you to delete a task.

## Known Issues and Limitations
There are no known issues at time of publishing.

## Frequently Asked Questions


## Deployment Instructions
Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).
