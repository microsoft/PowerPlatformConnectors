# Clockify

Clockify is a popular, easy to use, free time tracking software solution. This connector helps to connect your Clockify account to the Power Platform so you can pull data like Workspaces, Projects, Tasks and TimeEntries.

## Change Notes 

- 21 Oct 2024 : Added support for pagination for workspace users and user time entries.

- 28 Oct 2023 : Added support for the new v1 version of clockify. Old Version deprecated but the existing flows will continue to run.

## Publisher: Dr Adrian Colquhoun (Strategik)

## Prerequisites

To use this connector, you need the following

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- A Clockify account. Signup here : https://clockify.me/

## Obtaining Credentials
- Configure API Key here : https://clockify.me/user/settings


### Supported Actions
Clockify API supports more than 15 resource actions. Currently this connector supports only the operations to fetch data for Users, Clients, Workspaces, Projects and User Timesheet Entries

#### 1. Workspaces
- List Workspaces : Retrieves List of Workspaces in your Clockify Account

#### 2. Clients
- List Clients  : Retrieves List of Clients for a given workspace Id

#### 3. Projects
- List Projects : Retrieves List of Projects for a given workspace Id

#### 4. Users
- List All Users :  Retrieves List of All Users for a given workspace Id

#### 5. Timesheet
- List User Time Entries :  Retrieves List of timesheet entries based on Workspace Id and User Id

## Other Actions :
- Webhooks & Reports will be available in the upcoming updates. Stay Tuned


## Known Issues and Limitations
There are no known issues at time of publishing.

## Rate Limits
There's a rate limit of 10 requests per second. If you get over the limit, you'll get "Too many requests" error.


## Deployment Instructions
Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).