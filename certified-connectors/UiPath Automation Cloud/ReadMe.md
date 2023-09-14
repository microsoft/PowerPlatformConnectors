## Author:
UiPath - [UiPath Website](https://uipath.com)

## Contact:
UiPath Support Team - [UiPath Support Website](https://www.uipath.com/support)

## Version: 
2.0 - Update Release

## Changes: 
Changes to previous *UiPath Orchestrator version 1.0*:

- Introduction of new global app registration feature for improved security. 
    The app needs now  admin consent to be used.
    Due to this change, existing workflows created with version 1 cannot be migrated.
- Two new operations *Get Job* and *Get Queue Item*.
    This will allow you to optimze your workflow execution.
- Minor UI enhancements.

## Description
This is a Power Automate Connector for UiPath Cloud Orchestrator.

## Supported Operations
The connector supports the following operations:
- [Run Job](https://docs.uipath.com/orchestrator/docs/about-jobs)
Run a job in UiPath Orchestrator.
- [Get Job](https://docs.uipath.com/orchestrator/automation-cloud/latest/user-guide/job-states)
Utilize the "Get Job" function to acquire specifics related to a task initiated through the "Run Job" feature. You may assess for statuses such as "Successful, Running or Faulted". In workflows, you may pair the "Do Until" and "Initialize and Set Variable" features with the "Get Job" function to monitor the "State" property of the preceding "Run Job" phase.
- [Add Queue Items](https://docs.uipath.com/orchestrator/docs/about-queues-and-transactions)
Add a queue item to a specific queue.
- [Get Queue Item](https://docs.uipath.com/orchestrator/automation-cloud/latest/user-guide/queue-item-statuses)
Obtain information about an item in the queue created during a prior step. For instance, you can verify its status, such as "Successful" or "Failed". In your workflow, you can employ the "Get Queue Item" functionality, coupled with "Do Until" and "Initialize and Set Variable", to inspect the "Status" attribute of the preceding "Add Queue Items" phase.

## Getting Help or Providing Feedback
If you have any issues, requests for functionality, or have general feedback, please reach out to UiPath Support Team - [UiPath Support Website](https://www.uipath.com/support).

## Pre-requisites
Please make sure you have an automation cloud account. [Sign up](https://www.uipath.com/developers/studio-download) for a trial.
You will need an UiPath administrator to register your Power Automate integration under *UiPath -> External applications -> App registrations*, before you can successfully connect.

## Using the Connector
1. Select UiPath Connector as a step in your Power Automate Flow.
2. Enter your UiPath Cloud Organization ID and Tenant ID in the text fields.
3. Sign into UiPath platform with your credentials (Make sure your app has been registered by an admin upfront).
4. Once successfully authenticated you can select folders, processes and queues in the form.
5. Begin using the connector in your environment to build apps and flows!

## Limitations
For security reasons you need to reauthenticate your connections after 90 days.