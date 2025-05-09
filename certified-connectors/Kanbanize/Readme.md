
## Businessmap
Businessmap is the most flexible software platform for outcomes-driven enterprise agility. The unmatched functionality consolidates multiple tools into one, enabling affordable deployment at scale, visibility across all projects/portfolios, and alignment on goals, to deliver quality work faster. The Businessmap connector can be used to reflect changes in external tools (e.g. issue trackers, email, time tracking tools, etc.) inside your Kanban cards. This is an official connector working with the [Businessmap API](https://demo.kanbanize.com/openapi/).


## Publisher: Businessmap

## Prerequisites
* A Microsoft Power Apps or Power Automate plan
* To use this connector, you will need a Businessmap account and user with permissions to access the API. [Sign-Up here](https://businessmap.io/sign-up) and create and account to try it. Visit our [Knowledge Base](https://knowledgebase.businessmap.io/hc/en-us) to find answers to your questions regarding Businessmap.


## Supported Actions
The connector supports the following actions:

* `Create Card`: Create a card in a board of your choice. To set custom fields, stickers and tags use in combination with "Update Card".
* `Get All Cards`: Get all cards from a selected board. The response contains up to 200 cards. For more than 200 cards, please use paging.
* `Get Card by Custom ID`: Find card by custom card ID. If multiple cards are found, only the first one will be returned. Use ‘Board’ parameter to filter search by boards.
* `Get Card by ID`: Get card details by its internal ID.
* `Move Card`: Move a card to new column, lane, workflow or board.
* `Update Card`: Update the properties of a card (Title, Description, Priority, etc.). If getting a timeout or API limit reached errors, try breaking down the setting of custom fields, stickers and tags into separate actions.
* `Add Comment`: Add a comment to a card or initiative.
* `Archive or Unarchive Card`: Add card to archive, or extract it from archive.
* `Block Card`: Block a card.
* `Create Subtask`: Create a new subtask of a card or initiative.
* `Discard or Restore Card`: Discard a card or restore a previously discarded card.
* `Download Attachment`: Download an attachment from a card. File content is returned as base64 encoded string.
* `Get Card Attachments`: Get a list with all attachments for a specified card.
* `Get {Object} Name`: Get the name of an object based on its ID. Helper function used when you want to get the name of the board, workflow, column etc. based on its ID.
* `Link Cards`: Link a card to an existing card.
* `Log Time`: Log time to a card.
* `Set Custom Field Value`: Set the value of a custom field.
* `Set or Unset Stickers`: Add sticker(s) to a card or remove assigned sticker(s) from a card
* `Set or Unset Tags`: Add tag(s) to a card or remove assigned tag(s) from a card
* `Unblock Card`: Unblock a card.
* `Unlink Cards`: Remove a link between two cards.
* `Upload Attachment`: Upload an attachment to a card.
* `Get Block Reasons`: Get all block reasons within the account. Can filter by board. Used internally.
* `Get Boards`: Get all boards within the account. Used internally.
* `Get Columns`: Get all columns for a specified board. Used internally.
* `Get Custom Fields`: Get all custom fields within the account. Used internally.
* `Get Custom Field Values`: Get all values for a specified custom field. Used internally.
* `Get Lanes`: Get all lanes for a specified board. Used internally.
* `Get Stickers`: Get all stickers within the account. Can filter by board. Used internally.
* `Get Tags`: Get all tags within the account. Can filter by board. Used internally.
* `Get Templates`: Get all templates within the account. Used internally.
* `Get Types`: Get all types within the account. Used internally.
* `Get Users`: Get all users for a board. Used internally.
* `Get Workflows`: Get all workflows for a specified board. Used internally.
* `Delete Webhook`: Delete Webhook. Used internally.

## Supported Triggers
The connector supports the following triggers:

* `General Trigger`: Triggers for any action on the board. Use Control/Condition or Control/Switch to manage flow for different triggers. Here is partial list of supported event types: 
"Card created", "Card moved", "Card details changed", "Card deleted", "Card blocked"


## Obtaining Credentials
A Businessmap account is needed to use the connector. You can register [here](https://businessmap.io/sign-up). You will find your API key within your account details.

## Known Issues and Limitations
Certain limits are applied to ensure the product and our APIs are running as smoothly and efficiently as possible. The exact API limits depend on your service plan.


## Frequently Asked Questions
Visit our [FAQ page](https://businessmap.io/faq) to read the most popular questions or contact our [customer support](https://businessmap.io/customer-support) with any specific request.


## Deployment Instructions
Please use the instructions on [Microsoft Power Platform Connectors CLI](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate or Power Apps.