# Nintex Workflow – Start a Workflow Connector
The Nintex Workflow – Start a Workflow connector allows you to trigger workflows in Nintex Workflow from Microsoft Power Automate or Copilot Studio. This enables seamless automation across various connected platforms, integrating Microsoft tools with enterprise-grade workflow solutions from Nintex.

## Publisher: Nintex

## Prerequisites
To use this connector, you must have:

1. A valid Nintex Automation Cloud tenant.
2. A published workflow in Nintex Automation Cloud that is configured with a Webhook start event.

## Supported Operations
The connector provides the following operation:

### StartWorkflow
Triggers a Nintex Automation Cloud workflow that has been configured with a webhook start event. 

## Known Issues and Limitations
- Terminated or failed Nintex workflow or will not resume the Power Automate or Copilot flow, this is because the Nintex workflow cannot currently callback when it fail/terminated.
- The action can currently show up to 10 published workflows (Tthis will be increased in the future).

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps, and use [these documentation](https://developer.nintex.com/docs/nw-api-docs/getting-started) to setup authentication in Nintex Workflow.

## Support
For support, please contact Nintex at https://www.nintex.com/support/ or refer to our API documentation.