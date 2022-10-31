# Tribal Connector

Tribal Edge modules, such as Tribal Admissions, provide staff and students the tools they need to succeed in the world of education. Connect to Tribal Edge modules using the Tribal Connector to manage information, create clearance checks for applications, and much more.

## Publisher: Tribal

We provide the expertise, software and services required to underpin student success.

## Prerequisites

1. Sign in to create a connection using your Tribal account to define the following:
- Environment such as Live, Test or Development.
- Region such as APAC or EMEA.
- Edge Tenant ID as supplied by Tribal.

2. On sign in, you must enable the following permissions:
- Events Connector Endpoint
- Connect to the web hooks

## Documentation

[EMEA Documentation](https://help.tribaledge.com/emea/edge/EdgeEducation.htm)  
[APAC Documentation](https://help.tribaledge.com/apac/edge/EdgeEducation.htm)

## Supported Operations

**Entity** refers to one of the available business entities in Tribal like such as Person, Schools, Desks, Applications etc.

### Triggers

- `When an entity is created`: Triggers anytime an entity is created within a Tribal Edge module.

- `When an entity is updated`: Triggers anytime an entity is updated within a Tribal Edge module.

- `When an entity is deleted`: Triggers anytime an entity is deleted within a Tribal Edge module.

- `When an event is triggered on an entity`: Use this trigger for custom event types like Status changed, Cancel interview, Request visa and so on.

- `When a long running operation is performed on an entity`: Use this trigger as a consecutive trigger on flow where the first trigger is one of the above.

### Actions

- `Create an entity`: Creates an entity within a Tribal Edge module

- `Create a child entity`: Creates an entity which is a child of another entity within a Tribal Edge module

- `Read an entity`: Reads an entity within a Tribal Edge module

- `Read a child entity`: Reads an entity which is a child of another entity within a Tribal Edge module

- `Read entity collection`: Reads a collection of entities within a Tribal Edge module. Additional filtering is available for the records using Open Data Protocol syntax.  

- `Read child entity collection`:  Reads a collection of entities which are children of another entity within a Tribal Edge module. Additional filtering is available for the records using Open Data Protocol syntax.

- `Update an entity`: Updates an entity within a Tribal Edge module

- `Update a child entity`: Updates an entity which is a child of another entity within a Tribal Edge module

- `Delete an entity`: Deletes an entity within a Tribal Edge module

- `Delete a child entity`: Deletes an entity which is a child of another entity within a Tribal Edge module

## Known Issues and Limitations

- Opening an editted flow can sometimes show the entity as a custom value instead of the description from the drop down. This is purely cosmetic and doesn't effect the behaviour of the flow.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps