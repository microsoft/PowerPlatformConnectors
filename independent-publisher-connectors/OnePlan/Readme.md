# OnePlan Connector
OnePlan offers an AI-enabled Strategic Portfolio, Financial, Resource and Work Management Platform that fits the needs of every organization. OnePlan connects with Microsoft Project, Microsoft Planner, Azure DevOps, Jira, Smartsheet and more for a complete view into all work across the enterprise.

# Publisher: OnePlan AI

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Automate subscription
* A valid subscription in [OnePlan](https://oneplan.ai/)
* OnePlan [Authentication Key](https://support.oneplan.ai/hc/en-us/articles/14388035357709-OnePlan-Integration-settings). 


## Getting Started

Create a new OnePlan Group. Go to Configuration page (gear icon on the top-right) and request to enable advanced settings. Once Advanced Settings is enabled, go to Integrations page, and add a new authentication key. Use that key to create the OnePlan Power Automate connector. Contact customer support for assistance.


## Supported Operations
### 'Get Plan': 
Description: Get an individual plan from your OnePlan Group based on the Plan Id. Includes built-in fields and custom fields for the plan.

Parameters: Plan Id

Returns: WorkPlan

### 'Update Plan': 
Description: Updates a plan using Plan ID. Needs Plan ID and the fields that need to be updated. Returns the updated plan to the action.

Parameters: Plan ID, WorkPlan Body

Returns: Updated WorkPlan


### 'Stamp Mapping':
Description: This action will stamp mapping information onto a plan. This will let the plan know about its connection to the external system.

Parameters: Plan ID, Flow ID, External System ID, External System Url

Returns: Success Message

### 'Get Mapped Plan Item Id'
Description: This action will use the flow id and the external item id to return plan id that it belongs to. Useful for when you want to find the plan for an external item.

Parameters: Flow ID, External Item Id

Returns: Plan Id

### 'Step Update': 
Description: Allows you to automatically update process steps for a workplan, changing the stage the workplan is currently in.

Parameters: WorkPlan Id, Step Name, Step Id

Returns: Success Message

### 'Step Approve': 
Description: For gated steps, allows you to approve a process step automatically.

Parameters: Plan ID, state

Returns: Success Message

## Supported Triggers

### 'Step Change Trigger': 
Trigger Action for when step changes

Required parameters: None

### 'Plan Update Trigger': 
Trigger Action for when Plan gets updated

Required parameters: None

## Obtaining Credentials
Navigate to my.oneplan.ai, setup a trial group and follow the onboarding process. For any inquires reach out to [OnePlan Support](https://support.oneplan.ai/hc/en-us/requests/new)

## Known Issues and Limitations
The connector does not support Work Plan actions at the moment.

## Deployment Instructions
Create a custom connector with the given api definition file, validate, and in the Test section add a new connection. To get the key go to your OnePlan group's config page. Navigate to Integrations from left bar and add a new authentication key. Copy the same key as the value for your connector to connect to your OnePlan environment.
