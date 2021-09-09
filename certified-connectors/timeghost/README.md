# timeghost

timeghost is a dedicated Microsoft 365 / Office 365 solution that integrates perfectly with your work environment. Generate time reports, invoices and other documents using the timeghost connector.

## Pre-requisites

To use this connector, you need to have a work, school or Microsoft personal account. You also need to accept the permissions and consent of timeghost by taking one of these two steps:

- [Teams](https://teams.microsoft.com/): Install the timeghost Teams app and start it, accept the permissions and consent.
- [Web](http://web.timeghost.io/): Open the timeghost web app and accept the permissions and consent.

You're now ready to start using this connector.

## Supported Operations

The connector supports the following operations:

- `Add Client` : Add a client to a workspace.
- `Add Custom Feed Event` : Add an event to the users feed. Entry is automatically deleted after 180 days.
- `Add Project` : Add a project to a workspace.
- `Add Task` : Add a task to a workspace.
- `Add Time` : Add a time to a project within a workspace.
- `Add Time Tag` : Add a time tag to a workspace.
- `Create Times Excel Report` : Returns an Excel file containing a report of the specified time.
- `Delete Item` : Deletes an item from a workspace.
- `Get Client Projects` : Get all projects of one client in one workspace.
- `Get Client Times` : Get all times of one client in one workspace.
- `Get Clients` : Get all clients from a workspace.
- `Get Project Tasks` : Get all tasks of one project in one workspace.
- `Get Project Times` : Get all times of one project in one workspace.
- `Get Projects` : Get all projects from a workspace.
- `Get Time Tags` : Get all time tags from a workspace.
- `Get Times` : Get all times from a workspace.
- `Update Client` : Updates a client in a workspace.
- `Update Custom Feed Event` : Updates a custom feed entry in a workspace.
- `Update Project` : Updates a project in a workspace.
- `Update Task` : Updates a task in a workspace.
- `Update Time` : Updates a time in a workspace.
- `Update Time Tag` : Updates a time tag in a workspace.
- `Client Trigger` : If a client is created, changed or deleted in timeghost, this trigger runs.
- `Project Trigger` : If a project is created, changed or deleted in timeghost, this trigger runs.
- `Tag Trigger` : If a tag is created, changed or deleted in timeghost, this trigger runs.
- `Task Trigger` : If a task is created, changed or deleted in timeghost, this trigger runs.
- `Time Trigger` : If a time is created, changed or deleted in timeghost, this trigger runs.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
