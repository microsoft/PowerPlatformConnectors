# Dime.Scheduler

Dime.Scheduler, a Dime Software product, is a resource and project planning solution for the Microsoft Dynamics product suite.

## Publisher: Dime Software

Dime Software builds powerful and integrated business applications for Microsoft Dynamics and Microsoft 365. More than 100 resellers worldwide trust us to provide integrated solutions for complex business processes.

## Prerequisites

In order to use the connector, you'll need:

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- A running instance of Dime.Scheduler on-premise or in the cloud, and must be accessible by the app that uses this connector.
- An API key that you can obtain on [Dime.Scheduler Connect](https://connect.dimescheduler.com).

## Supported operations

The custom connector provides all the capabilities that the Dime.Scheduler SDK and the Dime.Scheduler Azure Functions expose.
The following entities can be managed through the custom connector:

| Entity       | Create/Update | Delete |
| ------------ | ------------- | ------ |
| Action URI   | ✅            | ✅     |
| Appointment  | ✅            | ✅     |
| Assignment   | ✅            | ✅     |
| Caption      | ✅            | ✅     |
| Category     | ✅            | ✅     |
| Time marker  | ✅            | ✅     |
| Pin          | ✅            | ✅     |
| Filter Group | ✅            | ✅     |
| Filter Value | ✅            | ✅     |
| Job          | ✅            | ✅     |
| Notification | ✅            | ✅     |
| Resource     | ✅            | ✅     |
| Task         | ✅            | ✅     |

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

- A Forms user account from your Dime.Scheduler instance.
- An API key that you can obtain on [Dime.Scheduler Connect](https://connect.dimescheduler.com).

## Known Issues and Limitations

The Dime.Scheduler instance must be accessible by Dime.Scheduler Connect (i.e. the web application needs to be exposed to the Internet).

## Deployment Instructions

Use the CLI to deploy a custom connector:

```cmd
paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json --icon icon.png
```

To establish a connection, you'll need to provide the following parameters:

| Parameter          | Value                                                                                                                                                                                                                                            |
| ------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Dime.Scheduler URI | This needs to be the base URI of your Dime.Scheduler instance, that is publicly accessible.                                                                                                                                                      |
| E-mail address     | The e-mail address of the Dime.Scheduler user that has sufficient rights to insert data into the solution.                                                                                                                                       |
| Password           | The password of said user                                                                                                                                                                                                                        |
| API Key            | This is the key of the subscription that allows you to use Dime.Scheduler's connect hub. You can register [here](https://connect.dimescheduler.com). Once enrolled, navigate to your user profile and copy the primary key of your subscription. |
