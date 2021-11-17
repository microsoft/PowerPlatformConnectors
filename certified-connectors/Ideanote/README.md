# Ideanote
Ideanote is the next-generation innovation software that has everything you need to collect, develop, prioritize and act on more of the right ideas. This connector exposes actions that enable you to invite and update users as well as list missions, ideas and comments. The connector also makes it possible with triggers to subscribe to various important events.

## Publisher: Ideanote ApS

## Prerequisites
An Ideanote account. Create a new account at https://ideanote.io/.

## Supported Operations

### Supported Triggers

* `NewComment`: When a comment is created
* `NewIdea`: When an idea is created
* `NewUser`: When a user is created
* `NewLike`: When an idea or comment is liked
* `FinishRating`: When someone rates an idea
* `UpdatedIdeaPhase`: When the phase of an idea changes
* `UpdatedIdeaStatus`: When the status of an idea changes

### Supported Actions

* `CreateUser`: Create user
* `UpdateUser`: Update user
* `ListMissions`: List all missions
* `ListIdeas`: List all ideas
* `ListIdeasForMission`: List all ideas for a mission
* `ListCommentsForIdea`: List all comments for an idea

## Obtaining Credentials
Please follow these steps to obtain credentials:

1. Login to Ideanote
2. Go to settings/integrations and click `Power Automate`
3. Copy the `API Token` from the dialog

## Known Issues and Limitations
- It is not currently possible to create new missions, ideas or comments.
- It is not currently possible to access dynamic data using the `NewIdea` trigger.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
