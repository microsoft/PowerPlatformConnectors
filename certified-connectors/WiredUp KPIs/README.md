# WiredUp KPIs

This connector allows you to save KPI Target or KPI Actual data to your WiredUp instance. This is more efficient than capturing the data into WiredUp manually. Commonly used data sources are Excel or SQL, but any source returning data in the expected format can be used.

## Publisher: PiPware Solutions

## Pre-requisites

In order to use this connector, you will need the following:

- A Power Platform premium plan
- The Power platform CLI tools (paconn)
- An activated login to a WiredUp instance
- Your user account's API key
- The name of your site if your WiredUp instance is multi-site
- Your WiredUp user account will need the "KPIActual" permission in order to use the "Save KPI Actuals" step in this Connector.
- Your WiredUp user account will need the "KPITarget" permission in order to use the "Save KPI Targets" step in this Connector.

## Supported Operations

The connector supports the following operations:

- `SaveKPIActuals`: Create or update KPI values in your WiredUp instance
- `SaveKPITargets`: Create or update KPI targets in your WiredUp instance

## Obtaining Credentials

WiredUp APIs use API key authorisation to ensure that client requests access data securely.
With API key auth, you send a key-value pair to the API in the request headers for every call.
Request header must have a `X-PIPWARE-ApiKey` field containing the API key.
A `X-PIPWARE-SiteName` must also be present with the name of the site you want to acces.
When you are ready to work with the public API ask your credentials by contacting us at customer@bloomflow.com. ​
WiredUp APIs use API key authorisation to ensure that client requests access data securely.
With API key auth, you send a key-value pair to the API in the request headers for every call.
Request header must have a `X-PIPWARE-ApiKey` field containing the API key.
A `X-PIPWARE-SiteName` must also be present with the name of the site you want to acces.
Contact support at support@wiredup.global to get your credentials.

## Known Issues and Limitations

- The `valueDate` and `targetDate` fields are required in the request body for the `SaveKPIActuals` and `SaveKPITargets` operations respectively. The date format must be in the ISO 8601 format, e.g. `2024-03-14T09:19:41.687Z`.

## Deployment Instructions

Please use the instructions on [Microsoft Power Platform Connectors CLI](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate or Power Apps.
