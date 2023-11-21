# MoreApp Forms Connector

MoreApp is a tool to automate work processes. Save time with Digital Work Orders, Inspections, Reports, and more!

## Publisher

MoreApp Forms

## Prerequisites

- A MoreApp account on the Branch plan

## Supported Operations

### New Submission

Trigger when a new submission has been filled in.

### Task Fulfilled

Trigger when a task has been fulfilled.

### Download File

Download a submission file.

### Download Report

Download a submission report.

### Create Task

Create a new task.

## Obtaining credentials

An API key is needed in order to use the integration. Check our [API authentication documentation](https://docs.moreapp.com/docs/developer-docs/b6b6c2d4906e9-authentication) for help on creating an API key.

## Known issues and limitations

A user can't make arbitrary API calls, only the actions described above are available.

## Deployment

Deploying this custom connector can be done using the `paconn` command. Check [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) for installation and usage docs.

After loggin in, follow [these](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli#update-an-existing-custom-connector) instructions to update your existing connector
