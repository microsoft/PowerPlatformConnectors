# UiPath Automation Cloud
This connector allows you to connect and run automated workflows in UiPath Automation Cloud. UiPath is a leading provider of Robotic Process Automation technology. Its Orchestrator service provides a web-based platform for managing, deploying, scheduling, monitoring, and automating processes. It is designed to run business processes on multiple robots in a consistent and efficient manner, making it easier to scale automation projects of any size.

## Publisher: UiPath Inc.
[UiPath Website](https://uipath.com)

## Prerequisites
- If you do not already have a UiPath Automation Cloud account, you will need to register for one. [Sign up](https://www.uipath.com/developers/studio-download) for a trial. Once registered, an UiPath administrator can then invite you to their Automation Cloud project. 
- Before connecting with Automation Cloud, your admin must register Power Automate integration in the *UiPath -> External applications -> App registrations* section.

## Supported Operations
The UiPath Orchestrator Connector supports operations that allow users to run jobs, get job statuses, add queue items to specific queues, and get queue item information. The connector enables the user to monitor the status of each of these operations using UiPath Orchestrator features, such as the "Do Until" and "Initialize and Set Variable" functions.

### Run Job
[Run Jobs](https://docs.uipath.com/orchestrator/docs/about-jobs) in UiPath Orchestrator.

### Get Job
Utilize the [Get Job](https://docs.uipath.com/orchestrator/automation-cloud/latest/user-guide/job-states) function to acquire specifics related to a task initiated through the "Run Job" feature. You may assess for statuses such as "Successful, Running or Faulted". In workflows, you may pair the "Do Until" and "Initialize and Set Variable" features with the "Get Job" function to monitor the "State" property of the preceding "Run Job" phase.

### Add Queue Items
[Add a Queue Item](https://docs.uipath.com/orchestrator/docs/about-queues-and-transactions) to a specific queue.

### Get Queue Item
Obtain information about a [Queue Item](https://docs.uipath.com/orchestrator/automation-cloud/latest/user-guide/queue-item-statuses) created during a prior step. For instance, you can verify its status, such as "Successful" or "Failed". In your workflow, you can employ the "Get Queue Item" functionality, coupled with "Do Until" and "Initialize and Set Variable", to inspect the "Status" attribute of the preceding "Add Queue Items" phase.

## Obtaining Credentials
To get credentials for Automation Cloud, you need to register for an account on the [Automation Cloud website](https://www.uipath.com/developers/studio-download), or you can reach out to your UiPath Automation Cloud administrator. After registering, you will be provided with a username and password that will enable you to log in and view the services available to you.

## Getting Started
1. Add the UiPath Automation Cloud Connector as a step in your Power Automate Flow. 
2. Enter your Organization ID and Tenant ID in the provided text fields (Ask your admin if you are not sure where to get this from or check FAQ below). 
3. Log in using your credentials (Ensure that the Power Automate Connector has been registered by an admin in advance). 
4. After successful authentication, you can choose folders, processes, and queues from the form. 
5. You can now start using the Connector in your environment to create apps and flows!

## Known Issues and Limitations
This connector does not work with OnPremsies deplyoments.
For security reasons you need to reauthenticate your connections after 90 days.

## Frequently Asked Questions
### Why is the UiPath Automation Cloud sign in process not working
If you have working credetials, please make sure the Power Automate Connector has been registered for your UiPath Automation Cloud organization before you connect. Contact your admin for further help.
### Does this connector work for OnPremise deployments
No, this connector works only with UiPath Automation Cloud.
### Why is process breaking after 90 days
You will have to reauthenticate after 90 days for security reasons.
### Where do I get Organization ID and Tenant ID from
If you browse to your Orchestrator instance, you can check the URL https://cloud.uipath.com/{Organization ID}/{Tenant ID}.

## Deployment Instructions
There is no manual deployment process required as both the PowerAutomate and UiPath platforms handle all aspects of deployment, app registration and privilege management.