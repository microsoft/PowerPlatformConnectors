
# Flixcheck

Flixcheck provides a powerful and very extensive REST API. Using this API, you can create and manage checks. This connector exposes a subset of these APIs as operations in PowerAutomate.


## Publisher: Flixcheck GmbH

Flixcheck is a no code app platform where users can build web apps and native apps.


## Prerequisites

You will need the following to proceed:

* Register at https://app.flixcheck.com/register


## Supported Operations

The connector supports the following operations:

### Subscribe to Check opened Event

Subscribe to Check opened Event via Webhook trigger

### Subscribe to Check completely finished Event

Subscribe to Check completely finished Event via Webhook trigger

### Subscribe to Check delivered Event

Subscribe to Check delivered Event via Webhook trigger

### Subscribe to Check created Event

Subscribe to Check created Event via Webhook trigger

### Get Check by ID

Get Check by checkId via Action

### Get Checks from Folder

Get Checks filtered by folderId via Action

### Create Check

Create Check via Action

### Get Template by ID

Get Template by templateId via Action

### Get Templates from all Folders

Get Templates from all Folders via Action

### Get Folders

Get Folders by Type via Action


## Obtaining Credentials

You authenticate via OAuth2.


## Known Issues and Limitations

-


## Deployment Instructions

1. Clone the PowerPlatformConnectors GitHub repository
2. Open a terminal, then change to the `flixcheck` directory, found in `certified-connectors` of the cloned repository
3. Run `paconn login`, then follow the authentication steps
4. Once authenticated, run `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret $SECRET`
5. Select the target environment for your connector
6. Create a new flow in Power Automate, or a new Power App, using the connector. When prompted, create a new connection with [OAuth2](https://app.flixcheck.com/portal/settings/connections). 