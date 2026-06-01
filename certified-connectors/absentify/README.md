# absentify

absentify is a free tool specifically designed for the Microsoft 365 environment to efficiently manage your team's or employees' absences. It provides a comprehensive overview of all time off and the reasons given, eliminating the need for cumbersome Excel sheets. absentify offers the easiest way to approve vacation requests and holidays, fully integrated with Microsoft Teams and Microsoft 365.

## Publisher: BrainCore Solutions GmbH

## Prerequisites

To use this connector, you need a work, school, or personal Microsoft account. You must also accept the permissions and consent for absentify by following one of these steps:

- [Teams](https://teams.microsoft.com/): Install the absentify Teams app, launch it, and accept the permissions and consent.
- [Web](http://app.absentify.com/): Open the absentify web app and accept the permissions and consent.

Additionally, you need the Essentials Plan or higher to use the triggers of this connector.

## Supported Operations

The connector supports the following operations:

### Triggers

#### `When a leave request is created`
This trigger is activated when a new request is created in your company.

#### `When a leave request status changes`
This trigger is activated when the status of an existing request changes.

### API Operations (Plus Plan required)

The following operations require the absentify Plus plan:

#### Members
- `[Plus] Get all members` - Retrieve all members with optional filters for departments, status, employment dates, and more.
- `[Plus] Invite a member` - Invite a new member to the workspace.
- `[Plus] Get member by ID` - Retrieve a specific member by their unique ID.
- `[Plus] Update a member` - Update an existing member's information.
- `[Plus] Delete a member` - Delete a member permanently.
- `[Plus] Get member by Microsoft ID` - Retrieve a member by their Microsoft user ID.
- `[Plus] Get member by email` - Retrieve a member by their email address.
- `[Plus] Get member by custom ID` - Retrieve a member by their custom ID.
- `[Plus] Update member approvers` - Update the approvers for a specific member.
- `[Plus] Update member allowance` - Update a member's allowance for a specific year.
- `[Plus] Add member schedule` - Add a new work schedule for a member.

#### Departments
- `[Plus] Get all departments` - Retrieve all departments in the workspace.
- `[Plus] Create a department` - Create a new department.
- `[Plus] Update a department` - Update an existing department.
- `[Plus] Delete a department` - Delete a department.

#### Leave Types
- `[Plus] Get all leave types` - Retrieve all leave types configured in the workspace.
- `[Plus] Create a leave type` - Create a new leave type.
- `[Plus] Update a leave type` - Update an existing leave type.
- `[Plus] Delete a leave type` - Delete a leave type.

#### Requests
- `[Plus] Get all requests` - Retrieve all leave requests with optional filters.
- `[Plus] Create a request` - Create a new leave request for a member.
- `[Plus] Get request by ID` - Retrieve a specific leave request by its ID.
- `[Plus] Update a request` - Update the status of a leave request (approve, decline, or cancel).
- `[Plus] Delete a request` - Delete a leave request permanently.

#### Public Holidays
- `[Plus] Get all public holiday calendars` - Retrieve all public holiday calendars.
- `[Plus] Create a public holiday calendar` - Create a new public holiday calendar.
- `[Plus] Get public holiday calendar by ID` - Retrieve a specific calendar with all its holidays.
- `[Plus] Update a public holiday calendar` - Update an existing calendar.
- `[Plus] Delete a public holiday calendar` - Delete a calendar.

#### Workspace
- `[Plus] Get workspace settings` - Retrieve the workspace settings and configuration.

#### Absences
- `[Plus] Get absences` - Retrieve absences for a specific date range.

## Obtaining Credentials

To use this connector, you need a work, school, or personal Microsoft account, which you can log in with directly.

## Getting Started

Install absentify through one of the following methods:

- [Teams](https://teams.microsoft.com/): Install the absentify Teams app, launch it, and accept the permissions and consent.
- [Web](http://app.absentify.com/): Open the absentify web app and accept the permissions and consent.

To fully configure your account, follow this [quickstart guide](https://support.absentify.com/article/57-quickstart). Ensure you are on the Essentials Plan or higher for triggers, or Plus Plan for API operations.

You're now ready to get started.

## Known Issues and Limitations

- In the Essentials Plan, you can only create three triggers. After the third trigger, you will receive an error message and need to delete an existing trigger before creating a new one.
- API operations (marked with [Plus]) require the absentify Plus plan. If you attempt to use these operations without the Plus plan, you will receive an error.

## Deployment Instructions

Please follow [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
