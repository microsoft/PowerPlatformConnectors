# Tribal Connector

Tribal Edge modules, such as Tribal Admissions, provide staff and students the tools they need to succeed in the world of education. Connect to Tribal Edge modules using the Tribal Connector to manage information, create clearance checks for applications, and much more.

## Prerequisites
1. Sign in to create a connection using your Tribal account to define the following:
- Environment such as pre-production or production.
- Region such as APAC or EMEA.
- Edge Tenant ID as supplied by Tribal.
- Service Name such as Admissions.

2. On sign in, you must enable the following permissions:
- Events Connector Endpoint
- Connect to the web hooks

## Documentation
[EMEA Documentation](https://help.tribaledge.com/emea/edge/EdgeEducation.htm)  
[APAC Documentation](https://help.tribaledge.com/apac/edge/EdgeEducation.htm)

## Supported Operations
**Entity** refers to one of the available business entities in Tribal like such as Person, Schools, Desks, Applications etc.

### Triggers

`When an entity is created`

`When an entity is updated`

`When an entity is deleted`

`When an event is triggered on an entity`  
Use this trigger for custom event types like Status changed, Cancel interview, Request visa and so on.

`When a long running operation is performed on an entity`   
Use this trigger as a consecutive trigger on flow where the first trigger is one of the above.

### Actions

`Create an entity`

`Create a child entity`

`Update a child entity`

`Update an entity`

`Delete a child entity`

`Delete an entity`

`Read a child entity`

`Read an entity`

`Read child entity collection`  
Additional filtering is available for the records using Open Data Protocol syntax.

`Read parent entity collection`  
Additional filtering is available for the records using Open Data Protocol syntax.

## Deployment instructions
Deploy the Tribal Connector as a custom connector in Microsoft Power Automate and Power Apps as detailed in [Microsoft Power Platform Connectors CLI](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli)

## Support
For support, contact us at servicedesk@tribalgroup.com.