# Resco Reports

Resco Reports provides you with a highly customizable tool for generating reports and documents based on your Dataverse / Dynamics data. This connector makes it possible to use the [Resco Mobile Report templates](https://docs.resco.net/wiki/Mobile_report) to automate document generation or any other back-office scenarios.

## Publisher: Resco

## Prerequisites

In order to use this connector, you will need the following:

* Dataverse / Dynamics organization with Woodford solution ([available here](https://www.resco.net/woodford-overview/)).
* A user account with schema access privilege or system administrator privilege.
* Some reports created with Resco [Report Designer](https://docs.resco.net/wiki/Report_Designer)

## Supported operations

### Generate a single row report

Generates a report for specific table row.

### Generate a Resco questionnaire report

Generates a report or an auto-report for specific Resco questionnaire.

### Generate a report on multiple rows

Generates a report for provided list of table rows.

### Generate a report on multiple rows (fetch XML)

Generates a report for a set of table rows defined by fetch XML query.

## Obtaining Credentials

This connector uses `OAuth2` authentication. When creating a new connection, you'll be required to specify your Dataverse environment (its resource URI) and give the consent for Resco Reports Azure app to access your data.
The connector will then use this connection to fetch the report definitions and all data necessary to generate the report document.

## Typical flow scenarios

This connector can follow many flow triggers. The most frequent scenarios are:
* `When a row is selected (Dataverse)` trigger, which allows to run it from Power Apps form via `Flow` command.
* `When an HTTP request is received` trigger, which can be used to run a flow from JavaScript button handler.
* `PowerApps` trigger, which allows to run a flow for manually entered arguments.

The output of this connector is a `Report content` which can be consumed as an input for various actions like filling the email attachment content, Note record document body or SharePoint document body. Report content output is supplemented by `File name` and `Content type` headers. In the case of HTTP request trigger, it can be used directly as a response.

## Known Issues and Limitations

There are currently no issues or limitations known.

For more information on parameters accepted/required by the connector's operations, please contact us at www.resco.net/contact-support.