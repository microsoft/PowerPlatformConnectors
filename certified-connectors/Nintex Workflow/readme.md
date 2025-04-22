# Nintex Workflow – Start a Workflow Connector
The Nintex Workflow – Start a Workflow connector allows you to trigger workflows in Nintex Workflow from Microsoft Power Automate or Copilot Studio. This enables seamless automation across various connected platforms, integrating Microsoft tools with enterprise-grade workflow solutions from Nintex.

## Publisher: Nintex

## Prerequisites
To use this connector, you must have:

1. A valid Nintex Workflow tenant.
2. A published workflow in Nintex Workflow that is configured with a component workflow start event.

## Supported Operations
The connector provides the following operation:

### Start Workflow
Start a workflow in Nintex Workflow that is configured with a component workflow start event. 

## Known Issues and Limitations
- If a triggered Nintex workflow instance is terminated or have failed, it will not be able to resume the Power Automate or Copilot flow and will remain in progress.
- The action currently lists up to 10 published workflows (this will be increased in the future).

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps, and use [these documentation](https://developer.nintex.com/docs/nw-api-docs/getting-started) to setup authentication in Nintex Workflow.

## Support
For support, please contact Nintex at https://www.nintex.com/support/ or refer to our API documentation.