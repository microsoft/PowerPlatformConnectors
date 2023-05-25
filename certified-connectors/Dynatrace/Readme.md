# Dynatrace Connector
Dynatrace is a leader in cloud observability, using AI and automation to monitor and optimize application performance, development, security, user experience, and more. Dynatrace’s Davis AI delivers precise root cause analysis of your environments’ most complex problems. Using the Dynatrace connector, you can utilize the insights from Dynatrace’s Problems, Security Problems, Events, and Metrics API to analyze your environment data to take any number of actions with the available connectors in Microsoft’s library. Learn more about Dynatrace’s APIs [here.]( https://www.dynatrace.com/support/help/dynatrace-api/environment-api)

## Publisher: Dynatrace

## Prerequisites
You will need the following to use the connector:

* [A Dynatrace tenant](https://www.dynatrace.com/trial/)

* [An API key](https://www.dynatrace.com/support/help/dynatrace-api/basics/dynatrace-api-authentication) with the following scopes enabled:
    * Read problems
    * Write problems
    * Read events
    * Ingest events
    * Read entites
    * Read security problems

* To get meaningful data, you will need to have an app running on your Dynatrace tenant. There are example applications and workshops for launching applications with Azure on your tenant [here](https://learn.alliances.dynatracelabs.com/azure/)

## Supported Operations
### `GET problems`
Lists problems observed within the specified timeframe
### `GET problem by problem ID` 
Gets the properties of the specified problem
### `GET problem comments by problem ID` 
Gets all comments on the specified problem
### `GET problem comment by problem ID` 
Gets the specified comment on a problem
### `POST problem comment`
Adds a new comment on the specified problem
### `GET events`
Lists events within the specified timeframe
### `GET entities`
Gets the information about monitored entites
### `GET entity by ID`
Gets the properties of the specified monitored entity
### `POST event ingest`
Ingests a custom event
### `GET security problems`
Lists all security Problems
### `Get Security Problems By ID`
Get parameters of a security problems


## Getting Started
You will need to enter your tenant URL and API key to start using the connector. Follow the instructions above to get both items.
The tenant URL you input should be in the format:

    * abc123.dynatrace.com

The API key should be entered in the format:

    * Api-Token dt1234.ABCDEFGH

## Known Issues and Limitations
A user must have a Dynatrace Tenant in order to use this connector

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.