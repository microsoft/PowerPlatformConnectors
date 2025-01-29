# Tribal - SITS
Streamline the day-to-day administration of student management to enhance student experience.

## Publisher: Tribal
We provide the expertise, software and services required to underpin student success.

## Prerequisites

### Documentation
[EMEA Documentation](https://help.tribaledge.com/emea/edge/EdgeEducation.htm)
[APAC Documentation](https://help.tribaledge.com/apac/edge/EdgeEducation.htm)

## Supported Operations
**Record** refers to one of the available business records in Tribal like such as Person, Schools, Desks, Applications etc.

### Triggers
- `When an event happens`: Triggers anytime an event on an entity happens within Tribal SITS.

### Actions
- `Create an entity`: Creates an entity within Tribal Maytas.
- `Read record`: Reads a record within Tribal SITS.
- `Read records`: Reads many records within Tribal SITS.
- `Update record`: Update a record in Tribal SITS.
- `Delete record`: Delete a record in Tribal SITS.
- `Send an HTTP request`: Performs a custom request on a relative path for Tribal SITS.

## Obtaining Credentials
1. Sign in to create a connection using your Tribal account to define the following:
- Environment such as Live, Test or Development.
- Region such as APAC or EMEA.
- Edge Tenant ID as supplied by Tribal.

2. On sign in, you must enable the following permissions:
- Events Connector Endpoint
- Connect to the web hooks

## Known Issues and Limitations
None

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps