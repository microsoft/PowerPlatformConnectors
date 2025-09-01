# Title
ActivityInfo provides a no code relational database builder for data collection and management in the social sector. 
This connector will allow users to create automations in their ActivityInfo databases from Power Automate. 
This will allow Power Automate users to create workflows that perform an action in Power Automate (e.g. send an email) 
in response to an event triggered in their ActivityInfo database (e.g. when a caseworker adds a new record).

## Publisher: Publisher's Name
BeDataDriven B.V.

## Prerequisites
* A valid Microsoft Power Automate subscription.
* A valid ActivityInfo account with access to an active database.
* An ActivityIfo database you own or have "Manage Automations" permission granted.
* An API token from ActivityInfo with Read and Write scope.

## Supported Operations

### GetForms
Get a list of forms in an ActivityInfo database (internal action).

### GetAddRecordSchema
Get schema of event data when a record is added (internal action).

### GetEditRecordSchema
Get schema of event data when a record is edited (internal action).

### GetDeleteRecordSchema
Get schema of event data when a record is deleted (internal action).

### DeleteAutomaton
Deletes an automation (internal action).

### AddRecordTrigger
Notifications of add records events.

### EditRecordTrigger
Notifications of edit records events.

### DeleteRecordTrigger
Notifications of delete records events.

## Obtaining Credentials
This connector requires an API token from ActivityInfo. Log into [ActivityInfo](https://www.activityinfo.org) and visit your account settings to
create a token with Read and Write scope.

## Getting Started
Log into your Power Automate account. Create a new flow and search for ActivityInfo when prompted for a trigger.

## Known Issues and Limitations
* Filters can only be specified in ActivityInfo and not in Power Automate.
* To use a filter, you will have to [update the automation definition in ActivityInfo](https://www.activityinfo.org/support/webinars/2024-05-15-introduction-to-automations.html).
* Any changes made to an automation created in Power Automate will not reflect in Power Automate.
* Any edits made in Power Automate will result in loss of any edits previously made in ActivityInfo for the automation.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.
