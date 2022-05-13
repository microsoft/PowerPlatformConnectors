# Dynatrace Connector

Dynatrace provides best-in-class observability through an open, AI-powered platform. This connector exposes the Problems API endpoints. For more details on the Problems API, check out the documentation here: https://www.dynatrace.com/support/help/dynatrace-api/environment-api/problems-v2


## Prerequisites
You will need the following to use the connector:

* A Dynatrace tenant: https://www.dynatrace.com/trial/

* An API key: https://www.dynatrace.com/support/help/dynatrace-api/basics/dynatrace-api-authentication
    * Make sure you have Problems turned on for the API key scope to be able to read your data properly 

* To get meaningful data, you will need to have an app running on your Dynatrace tenant. There are example applications and workshops for launching applications with Azure on your tenant here: https://learn.alliances.dynatracelabs.com/azure/

## Supported Actions
* `GET problems` – Lists problems observed within the specified timeframe
* `GET problems by problem ID` - Gets the properties of the specified problem
* `GET problem comments by problem ID` - Gets all comments on the specified problem
* `GET problem comment by problem ID` - Gets the specified comment on a problem
