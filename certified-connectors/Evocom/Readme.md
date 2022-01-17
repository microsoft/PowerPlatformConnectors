## Evocom Connector

The Evocom Connector builds the bridge between the easiest solution for collaborative processes and the Microsoft Power Platform (and Azure Logic Apps). According to the motto "first digitize, then automate", companies transform manual processes into digital processes in order to further integrate and automate them afterwards.

## Prerequisites

You will need the following to proceed:

- An Evocom Productivity subscription
- A Microsoft Power Apps or Power Automate plan

## Set up an Evocom account

https://productivity.evocom.net/

## Supported Operations

The connector supports the following operations:

### Actions

- `Create task`: This operation is used to create a new task/approval/request in you task manager
- `Edit task`: This operation is used to edit a task, which wasn´t created by a process instance
- `Get process`: This operation retrieves process by ID
- `Get processes`: This operation retrieves all processes to which you are entitled
- `Get task`: This operation retrieves task by ID
- `Get tasks`: This operation retrieves all tasks to which you are entitled
- `Get teams`: This operation retrieves all teams
- `Get template`: This operation retrieves process template by ID
- `Get template steps`: This operation retrieves the list of steps defined in template
- `Get templates`: This operation retrieves all process templates
- `Start process`: This operation is used to start a process. Fields are dynamic from the start form of the process template.
- `Get user`: This operation retrieves user by EP login (email) or Azure user id

### Trigger

- `New process`: Triggers when a new process is started.
- `Changed process`: Triggers when process steps change or process is completed.
- `New task`: Triggers when a new task is created.
- `Changed task`: Triggers when something has changed in a task.
- `New team`: Triggers when a new team is created.
- `Changed team`: Triggers when something has changed in a team.

## Known issues and limitations

- You cannot use files in actions and as dynamic content in Power Automate.
