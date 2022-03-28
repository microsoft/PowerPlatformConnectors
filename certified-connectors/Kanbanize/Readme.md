
## Kanbanize
An official connector working with the Kanbanize API.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* A Kanbanize subscription with access to API enabled

## Supported Actions
The connector supports the following actions:
* `Create Card`: Create a card in a board of your choice.
* `Update Card`: Update the properties of a card (Title, Description, Priority, etc.).
* `Move Card`: Move a card (Task or Initiative, if possible) to a column.
* `Create Subtask`: Creates a new subtask of a card or initiative.
* `Add Comment`: Adds a comment to a card or initiative.
* `Log Time`: Log time to a card. The time is logged in minutes.
* `Get Boards`: Get all boards from the account.
* `Get Columns`: Gets all columns for a specified board.
* `Get Lanes`: Gets all lanes for a specified board.
* `Get Types`: Gets all types within the account.
* `Delete Card`: Delete a card.
* `Block or Unblock Card`: Block, unblock or edit block reasons for a card.
* `Archive Card`: Move card to archive. The card must be not be blocked and it must be in column Done or in column Archive.
* `Get Card By ID`: Get card by its ID.
* `Get All Cards`: Get all cards from given board.
* `Get Workflows`: Get all workflows from a given board.
* `Delete Webhook`: Delete Webhook. Used internally.

## Supported Triggers
The connector supports the following triggers:
* `General Trigger`: Triggers for any action on the board. Use Control/Condition or Control/Switch to manage flow for different triggers. Here is partial list of supported event types: 
"Card created", "Card moved", "Card details changed", "Card deleted", "Card blocked"