# Toggl Plan

Easy-to-use visual planning tool for project management. Plan project steps on a colorful timeline and have a clear overview of when things need to be done. Use boards view to follow up on task statuses. Change of plans? The simple drag-and-drop interface makes flexible planning easy.

## Publisher: Daniel Laskewitz | Sogeti

## Prerequisites

- A paid or trial Toggl Plan account
- A registered application

## Getting Started

You will need to gather either OAuth credentials to use with this connector.

## Obtaining Credentials

A Toggl Plan account (trial or paid) is needed to register an application. You can start a trial [here](https://toggl.com/plan/). After that you can go to the [developer page](https://developers.plan.toggl.com/index.html) and register your application there. From there you can grab the app key and secret.

## Supported Operations

### User Profiles

#### Get profile information

Returns the profile information of the logged in user.

#### Update profile

Updates the profile of the logged in user.

### Members

#### Add a new member

Creates a member that has not yet been linked with an existing user.

#### Update member not yet linked with existing user

Updates a member that has not yet been linked with an existing user.

#### Fetch list of members

Fetches the list of members belonging to a workspace.

#### Get member

Gets a member belonging to a workspace by ID.

#### Update member

Updates a member belonging to a workspace by ID.

#### Remove member

Removes a member from a workspace by ID.

### Tasks

#### Fetch list of tasks

Fetches a list of tasks belonging to a workspace. Possible filters are `since`, `until`, `users`, `projects`, `tasks`, `groups` and/or `groups`.

#### Add a new task

Adds a new task to a workspace.

#### Get a task

Gets a task from a workspace by ID.

#### Update a task

Updates a task from a workspace by ID.

#### Remove a task

Removes a task from a workspace by ID.

### Milestones

#### Fetch list of milestones

Fetches a list of milestones belonging to a workspace. Possible filters are `since`, `until`, `projects` and/or `tags`.

#### Add a new milestone

Adds a new milestone to a workspace.

#### Get a milestone

Gets a milestone from a workspace by ID.

#### Update a milestone

Updates a milestone from a workspace by ID.

#### Remove a milestone

Removes a milestone from a workspace by ID.

### Projects

#### Fetch list of projects

Fetches a list of projects belonging to a workspace.

#### Add a new project

Adds a new project to a workspace.

#### Get a project

Gets a project from a workspace by ID.

#### Update a project

Updates a milestone from a workspace by ID.

#### Remove a project

Removes a milestone from a workspace by ID.

### Groups

#### Fetch list of groups

Fetches a list of groups belonging to a workspace.

#### Add a new group

Adds a new group to a workspace.

#### Get a group

Gets a group from a workspace by ID.

#### Update a group

Updates a group from a workspace by ID.

#### Remove a group

Removes a group from a workspace by ID.

#### Add a new user to group

Adds a new group membership to a workspace.

#### Remove a group membership

Removes a group membership from a workspace by ID.

## Known Issues and Limitations

No issues and limitations are known at this time.

## Frequently Asked Questions

### How do I obtain a client ID and client Secret?

See the [obtaining credentials section](#obtaining-credentials).
