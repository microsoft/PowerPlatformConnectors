## Meisterplan Connector

Whether your work is traditional, agile or hybrid, Meisterplan helps portfolio and resource managers manage people across teams and initiatives. Our fully-featured REST API allows you to keep your business data synchronised and consistent across the many tools you use to achieve your planning goals.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Meisterplan user with the following Integration Rights:
  * *Access Meisterplan APIs and Connect External Applications*
* The Power platform CLI tools

## Building the connector

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.
You can download the API definitions file `swagger.json` from our [system](https://api.us.meisterplan.com/swagger.json).
You will need an API-token to connect to the REST-API. For details on how to create an API-token, [see our documentation](https://help.meisterplan.com/hc/en-us/articles/360028700752-REST-API-Manage-API-Tokens).


## Supported Operations
The connector supports updating the following entities in Meisterplan
- Actual Time Worked
- Allocations
- Business Goals
- Calendars
- Financials
- Milestones & Milestone Dependencies
- OBS
- Portfolios (read only)
- Priorities
- Programs
- Projects
- Resources
- Role Capacities
- Roles
- Scenarios (read only)
You can find the complete REST API documentation [here](https://api.us.meisterplan.com/).

## Known Issues and Limitations
---
* This connector does not yet support triggers.
* Webhooks for the Meisterplan REST-API are coming soon.
