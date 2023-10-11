# Tribal - Maytas
Streamline the day-to-day administration of training, reduce administration costs and improving efficiency, manage courses, monitor learner success rates.

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

## Supported Operations
**Entity** refers to one of the available business entities in Tribal Maytas like such as Trainees, Assessors or Employers.

### Triggers
- `When an event happens`: Triggers anytime an event on an entity happens within Tribal Maytas.

### Actions
- `Create an entity`: Creates an entity within Tribal Maytas.
- `Read an entity`: Reads an entity within Tribal Maytas.
- `Read entity collection`: Reads a collection of entities within Tribal Maytas. Additional filtering is available for the records using Open Data Protocol syntax.
- `Update an entity`: Update one or more properties on an entity within a Tribal Maytas.
- `HTTP Request`: Performs a custom request on a relative path for Tribal Maytas.

## Known Issues and Limitations
- HTTP Request Patch http verb is not working.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps