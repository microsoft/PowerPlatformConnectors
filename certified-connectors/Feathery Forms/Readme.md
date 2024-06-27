# Feathery Forms Connector
Feathery is the most powerful form and workflow automation platform. Build pixel-perfect forms, set up powerful logic, and connect to hundreds of different systems.

## Publisher: Feathery

## Prerequisites
You will need the following to proceed:
* A [Feathery account](https://feathery.io)
* A Microsoft Power Apps or Power Automate plan

## Supported Operations
The connector supports the following triggers.

### FormCompletion
When a Feathery form submission is completed

### DataReceived
When any data is received from a Feathery form submission (e.g. a partial submission)

## Getting Started
[Sign up](https://app.feathery.io) for a Feathery account. Then, 
follow our [quickstart guide](https://docs.feathery.io/platform/quickstart) to get started.

## Known Issues and Limitations
None

## Frequently Asked Questions
### Why isn't my submission triggering `FormCompletion`?
Once an existing submission triggers the `FormCompletion` event, it won't be able to trigger it again.

### When does the `DataReceived` event trigger?
Anytime a submission receives user data -- when a step is submitted, integration is connected, data is set programmatically, etc.

## Deployment Instructions
1. Authorize your connector with your Feathery API key. This can be found in the account settings of your Feathery dashboard. 
2. Specify the name of the form that you want to track events for. You may have multiple connectors track the same form.
