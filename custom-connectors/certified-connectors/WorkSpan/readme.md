## WorkSpan Connector
[WorkSpan](https://app.workspan.com) is a collaborative platform for connecting with your partners and managing your partner programs. You can integrate your CRM, Partner Center and collaboration tools to WorkSpan.

## Pre-requisites

- Signup to [WorkSpan](https://app.workspan.com) using your business email
- Create an application user on WorkSpan

## Building the connector 

Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:

* `Bulk load`: Bulk Load data to WorkSpan
* `Receive Object Events`: Register to receive object events (updates, stage changes)
* `Receive Collaboration Events`: Register to receive events for tasks, comments, membership changes
