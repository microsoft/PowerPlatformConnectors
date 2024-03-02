# Nimflow Connector
Nimflow is a next-generation task orchestrator.  Using this API, you can dispatch actions, send responses and  subscribe to events.

## Publisher: Nimflow LLC

## Prerequisites
You will need a Nimflow account, a business unit and to create a Digital Worker. 

## Supported Operations
The connector supports the following operations:

### Dispatch action
Dispatch an action for a Context, identified with a Reference.

### Send responses
Send responses for a Task in a Context, identified with a Reference

### When task created
Trigger on Task Created events

### When task updated
Trigger on Task Updated events

### When task archived
Trigger on Task Archived events

### When milestone reached
Trigger on Milestone Reached events

### When milestone cleared
Trigger on Milestone Cleared events

## Obtaining Credentials
Login to your [Nimflow](https://studio.nimflow.com), select a Business Unit and create a Digital Worker. Then complete the connection parameters with the corresponding data. For the Base URL, please complete with the protocol and the host without the last slash bar (e.g.: https://api.nimflow.com). If you use https://api.nimflow.com, you will also need to complete the Subscription Key parameter to authenticate. Please request the subscription key to [support](mailto:support@nimflow.com).

## Known Issues and Limitations
None.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps