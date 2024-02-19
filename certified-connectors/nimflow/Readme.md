# Nimflow Connector
Nimflow is a next-generation task orchestrator.  Using this API, you can dispatch actions, send responses and  subscribe to events.

## Publisher: Nimflow

## Prerequisites
You will need the following to proceed:
* A Nimflow business unit subscription

## Building the connector 
We first need to set up a few thing in Nimflow so that our connectors can securely access the Business Unit.  After that is completed, you can create and test the sample connector.


## Supported Operations
The connector supports the following operations:
* `Dispatch action`: Dispatch an action for a Context, identified with a Reference
* `Send responses`: Send responses for a Task in a Context, identified with a Reference
* `When task created`: Trigger on Task Created events 
* `When task updated`: Trigger on Task Updated events
* `When task archived`: Trigger on Task Archived events
* `When milestone reached`: Trigger on Milestone Reached events
* `When milestone cleared`: Trigger on Milestone Cleared events

## Getting Started

Complete the Connection parameters with the created Digital Worker for the Business Unit. For the Base URL, please complete with the protocol and the host without the last slash bar (e.g.: https://api.nimflow.com). If you use https://api.nimflow.com, you will also need to complete the Subscription Key parameter to authenticate. Please request the subscription key to [info](mailto:info@nimflow.com).

## Known Limitations

The API has restrictions in terms of the size of the Request and the Response body and the response time which is detailed [here](https://www.nimflow.com/docs/api/#request-limitations)

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps