# Dime.Scheduler (on-prem)

Dime.Scheduler, a Dime Software product, is a resource and project planning solution for the Microsoft Dynamics product suite and the Power Platform.

## Publisher: Dime Software

Dime Software builds powerful and integrated business applications for Microsoft Dynamics and Microsoft 365. Hundreds of resellers worldwide trust us to provide integrated solutions for complex business processes.

## Prerequisites

In order to use the connector, you'll need an activate Dime.Scheduler tenant and an API key which you can generate inside the application.

## Supported operations

The following entities can be managed through the connector:

| Entity       | Create/Update | Delete |
| ------------ | ------------- | ------ |
| Action URI   | ✅             | ✅      |
| Appointment  | ✅             | ✅      |
| Assignment   | ✅             | ✅      |
| Caption      | ✅             | ✅      |
| Category     | ✅             | ✅      |
| Container    | ✅             | ✅      |
| Time marker  | ✅             | ✅      |
| Pin          | ✅             | ✅      |
| Filter Group | ✅             | ✅      |
| Filter Value | ✅             | ✅      |
| Job          | ✅             | ✅      |
| Notification | ✅             | ✅      |
| Resource     | ✅             | ✅      |
| Task         | ✅             | ✅      |

The following operations are supported to manipulate certain properties of the entities:

- **Appointment**
  - Category
  - Time marker
  - Importance
  - Locked
  - Planning Quantity
  - URL
- **Task**
  - Container
  - Filter Value
  - Locked
- **Resource**
  - Container
  - Filter Value
  - Locked
  - Calendar
  - Capacity
  - Certificate
  - Filter Value
  - GPS Tracking
  - URL

## Obtaining Credentials

- A Dime.Scheduler API key

## Known Issues and Limitations

Be advised that this connector only targets Dime.Scheduler cloud. For the on-premises connector, refer to the Dime.Scheduler (on-prem) connector.

## Deployment Instructions

Use the CLI to deploy a custom connector:

```cmd
paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json --icon icon.png
```

To establish a connection, you'll need to provide the following parameters:

| Parameter   | Value                                                                                    |
| ----------- | ---------------------------------------------------------------------------------------- |
| Environment | Select production for your live instance, or use the sandbox to test your functionality. |
| API key     | Your Dime.Scheduler API key.                                                             |
