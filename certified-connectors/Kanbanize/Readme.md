
## Kanbanize
An official connector working with the Kanbanize API.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* A Kanbanize subscription with access to API enabled

## Supported Operations
The connector supports the following operations:
* `Create Card`: Create a card in a board of your chocie.
* `Update Card`: Update the properties of a card (Title, Description, Priority, etc.).
* `Move Card`: Move a card (Task or Initiative, if possible) to a column.
* `Create Subtask`: Creates a new subtask of a card or initiative.
* `Add Comment`: Adds a comment to a card or initiative.
* `Log Time`: Log time to a card. The time is logged in minutes.
* `Get Boards`: Get all boards from the account.
* `Get Columns`: Gets all columns for a specified board.
* `Get Lanes`: Gets all lanes for a specified board.
* `Get Types`: Gets all types within the account.