## Meisterplan Connector

Whether your work is traditional, agile or hybrid, Meisterplan helps portfolio and resource managers manage people across teams and initiatives. Our fully-featured REST API allows you to keep your business data synchronised and consistent across the many tools you use to achieve your planning goals.

## Publisher
This connector is published by [Meisterplan](https://meisterplan.com/).

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Meisterplan user with the following Integration Rights:
  * *Access Meisterplan APIs and Connect External Applications*
* The Power platform CLI tools

## Supported Operations
The connector supports reading and/or updating the following Meisterplan entities:

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

## Building the connector

You can download the API definitions file `swagger.json` from our [system](https://api.us.meisterplan.com/swagger.json).
You will need an API-token to connect to the REST-API. For details on how to create an API-token, [see our documentation](https://help.meisterplan.com/hc/en-us/articles/360028700752-REST-API-Manage-API-Tokens).

## Obtaining Credentials

This connector uses oAuth to access your Meisterplan system. Simply log in with your Meisterplan account and grant access to the Power Automate Connector.

If you're building the connector yourself, please use an API token as described [here](https://help.meisterplan.com/hc/en-us/articles/360028700752-REST-API-Manage-API-Tokens).

## Known Issues and Limitations

The Meisterplan Connector uses the Meisterplan REST API. A description including status codes can be found in the REST API [documentation](https://api.us.meisterplan.com/docs/api.html). Additional information can be found in the [Help Center](https://help.meisterplan.com/hc/en-us/articles/360011962979-REST-API-Overview).

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.
