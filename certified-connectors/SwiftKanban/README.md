# SwiftKanban

SwiftKanban is a Visual Work Management Tool for helping users manage their work effectively by leveraging the simple yet powerful principles of the Kanban Methodology. Use SwiftKanban connector to add/modify cards with the data from rest of your ecosystem.

## Pre-requisites

You will need the following to proceed:

- A [SwiftKanban](https://login.swiftkanban.com/) account
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- The Power platform CLI tools

## Getting started

To use this connector, an API key is required. This can be found by logging into your SwiftKanban account and navigating to Profile -> Authentication token.

## Supported operations

The connector supports the following operations: 

- `Add comment to a card`: This operation is used to add a comment to an existing SwiftKanban card.
- `Create a card`: This operation is used to add a card in the backlog or the custom column as defined in the board policies.
- `Update a card`: This operation is used to update a specific card for for a given card ID.
- `Get card by card ID`: This operation is used to retrieve a specific card details for a given card ID.
- `Get all accessible boards`: This operation is used to retrieve a list of all accessible projects for your SwiftKanban instance.
- `Get team members details in a board`: This operation is used to retrieve a detailed list of all users associated with a SwiftKanban board.
- `Get all the card types available in a board`: This operation is used to retrieve a list of cardTypes for your SwiftKanban board.
- `Archive a card`: This operation is used to archive a specific card for a given card ID.
- `Discard or abort a card`: This operation is used to discard or abort a specific card for a given card ID.
- `Get details of queues and sub-queues`: This operation is used to fetch lane details along with queues and subqueues.
- `Move a card to backlog`: This operation is used to move a card to backlog.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further Support

Check out our [documentation](https://www.digite.com/knowledge-base/swiftkanban/) or reach us at [https://www.digite.com/services/customer-support](https://www.digite.com/services/customer-support). 