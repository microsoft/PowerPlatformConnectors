# Iconfirm Connector
Iconfirm offers a suite of tools and functionality to help organizations comply with the requirements set by GDPR with additional features to ease the day to day documentation and insight for your organization.

## Iconfirm features
* Citizen/Data subject rights management
* Consents/Confirmations
* Documentation
* Systems-/Vendor management
* Data processing agreement generator (Auto generated)
* Risk assessments
* Resource bank
* Incident handling
* Reports

For more information about what we offer, have a look at our [website](https://www.iconfirm.eu/en/) or request a [demon](https://info.iconfirm.eu/en/demonstration-iconfirm).

## Publisher: Iconfirm AS

## Prerequisites
You will need the following to proceed:
* An Iconfirm subscription
* An API user configured on your Iconfirm subscription


## Obtaining Credentials
In order to get your Api-key for authorizing the connector to communicate with Iconfirm, you would need to follow these simple steps:
* 1: Sign in to your Iconfirm account
* 2: Navigate to `Access control` and select `Users`
* 3: Press `Register` and select `Register new API user`
* 4: Fill in a recognizable name for the user account and an optional description, then hit `Save`.
* 5: Hit `Generate new API key`, ensure to copy and save this somewhere safe, as the API key is only generated and displayed once and overwritten if generated again.
* 6: Ready to go, the above API key can now be used with our connector in the configuration steps when setting up your app/flow

## Supported operations
The connector supports the following operations:

Verify login/Healthcheck
* `Verify authentication`: Checks if the supplied connection ApiKey is authenticated and ready to go.

Risk Scenarios
* `Find risk scenarios`: Lists all registered risk scenarios (Can also include default risk scenarios suggested by Iconfirm).
* `Get risk scenario by Id`: Returns a single risk scenario.
* `Get risk scenarios by mitigating action Id`: Returns a list of all risk scenarios linked to the given mitigating action.

Mitigating Actions
* `Find mitigating actions`: Lists all registered mitigating actions (Can also include default mitigating actions suggested by Iconfirm).
* `Get mitigating action by Id`: Returns a single mitigating action.
* `Get mitigating actions by risk scenario Id`: Returns a list of all mitigating actions linked to the given risk scenario.

Tasks
* `Find tasks`: Returns a list of registered tasks.
* `Get tasks by Id`: Returns a single task.
* `Get tasks by Uuid`: Returns a single task.
* `Update status`: Updates the status for a single task.

Records
* `Find records`: Returns a list of records matching the filter criteria defined.
* `Get record status`: Returns the status for a single record.
* `Get changes`: Returns a list of changes within a given time period.

### Additional operations for controllers
Records
* `Register record`: Registers a new record.
* `Update citizen`: Updates the registered data of the record to ensure data is always up to date.
* `Send reminder`: Sends out a reminder to the data subject registered on the given record.
* `Revoke record`: Revokes the record.

Citizen Tasks
* `Find tasks`: Returns a list of registered tasks.
* `Get changes`: Returns all status changes for tasks within the given period.
* `Update status`: Updates the status for a single task.

Processing Definitions
* `Find processing definitions`: Returns a list of registered processing definitions.
* `Get processing definition by Id`: Returns a single processing definition.
* `Get processing definition by Tag`: Returns processing definitions matching the defined tag. 

System/Recipients
* `Find systems`: Returns a list of all registered systems/recipients.
* `Get system by Id`: Returns a single system/recipient.
* `Get system by Uuid`: Returns a single system/recipient.
* `Get system by Tag`: Returns a list of systems/recipients matching the defined tag.
* `Register system`: Registeres a new system/recipient.
* `Update system`: Updated the given system/recipient.
* `Get DPA fulfillment`: Returns a report of fields missing on the system/recipient in order to automatically generate a Data Processing Agreement.
* `Generate DPA`: Generates a Data Processing Agreement with data combined from your details combined with the system/recipient in the given language.
* `Get archiving settings by Id`: Returns the archiving settings defined for the system/recipient.
* `Get risk assessment by Id`: Returns all risk assessments made on the system/recipient.

Processes
* `Find processes`: Returns a list of all registered processes.
* `Get process by Id`: Returns a single process.
* `Register process`: Registeres a new process.
* `Update process`: Updated the given process.
* `Get risk assessment by Id`: Returns all risk assessments made on the process.

Risk assessments
* `Find risk assessments`: Returns a list of all registered risk assessments.
* `Get risk assessment by Id`: Returns a single risk assessment.

Incidents
* `Find incidents`: Returns a list of all registered incidents.
* `Get incident by Id`: Returns a single incident.
* `Register incident`: Registeres a new incident.
* `Update incident`: Updated the given incident.

Dpia
* `Find dpias`: Returns a list of all registered dpias .
* `Get dpia by Id`: Returns a single dpia.
* `Register dpia`: Registeres a new dpia.
* `Update dpia`: Updated the given dpia.


### Additional operations for global systems
Customers
* `Find customers`: Returns a list of all registered customers.
* `Get customer by Id`: Returns a single customer.


## Known Issues and Limitations
* All operations listed as 'additional operations' will require your organization to be within the above mentioned context 'Controller' or 'Global System' 
* All register/update operations needs to contain all information for the object in question, all non-included parameters will be overwritten
* Not all elements of the solution is available through our API and this Connector for requests or suggestions for additional operations and functionality please contact Iconfirm support.

## Help, Feedback and Additional documentation
For additional information, feedback or help, have a look at our [website](https://www.iconfirm.eu/en/) and/or contact support through there.
For additional information, feedback or help, have a look at our [website](https://www.iconfirm.eu/en/) and/or contact support through there.
