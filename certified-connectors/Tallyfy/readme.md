
# Tallyfy

Tallyfy is beautiful workflow software that helps you create, track, and scale any repeatable process. It helps you eliminate the use of flowcharts, forms and emails by digitizing manual business processes and approval workflows. This connector offers the ability to integrate your other applications with actions in Tallyfy.

## Pre-requisites

To use this connector, all you need to have is an account on [Tallyfy](https://go.tallyfy.com/). You will be prompted to login upon creation of your first Flow.

For further information - please visit the [Tallyfy integrations](https://tallyfy.com/integrations/).

## API documentation

https://go.tallyfy.com/api/

You will need an access token. This is found under the `integrations` tab in the `settings` page of your Tallyfy account.

## Supported Operations

The connector supports the following operations:

* ```Get_Organization_Users```: Get organization's users.
* ```Get_Guests```: Get guests in an organization.
* ```Get_User_Organizations```: Get user's organizations.
* ```Get_User_Tasks```: Get a member's tasks.
* ```Invite_User_To_Organization```: Invite a new member to your organization.
* ```Create_Task```: Create a task.
* ```Get_Checklists```: Get Blueprints.
* ```Create_Run```: Launch a process using a blueprint.
* ```Completed_One_Off_Task```: Complete one off task.
* ```Reopen_One_Off_Task```: Reopen one off task.
* ```Completed_Process_Task```: Complete process task.
* ```Reopen_Process_Task```: Reopen process task.
* ```Comment_Task```: Add comment / Report issue / Resolve issue on a tasks.
* ```Edit_Task_Deadline```: Edit deadline of a task.
* ```Remove_Guest```: Remove guest of a task.
* ```Remove_Assignee```: Remove assignee of a task.
* ```Edit_Step_Type```: Edit step type.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

## Further Support

For further support, checkout our [support documentation](https://support.tallyfy.com/) or [contact us](https://tallyfy.com/contact-us/)
