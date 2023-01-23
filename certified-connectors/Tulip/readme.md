### Tulip Connector

The Tulip Connector allows you to integrate with the data in your Tulip Instance. Enxtending your Tulip frontline operations applications into your power automate, Teams and M365 workflows.

## Prerequisites
You will need the following to proceed:
* Active Tulip instance and subscription (tulip.co)
* Tulip API bot credentials for your Tulip Account with access to the appropriate scopes. ( Tulip Account or Workspace owner can provide credentials)

## Setting up the connector
On initial use, you will need to enter your Tulip Bot Credentials and Tulip Instance URL. This will provide you with the connection between Power Automate and your Tulip Instance. See [Adding an API](https://support.tulip.co/docs/how-to-use-the-table-api-1#:~:text=in%20a%20table.-,Adding,-an%20API) to see how to create a bot in your Tulip instance.
Then you will be able to configure the connector within your Power Automate flow.
note: if / where using the create record, you will need to create with a unique id

## Supported Operations
The connector supports the following operations:
* `Get Record`:  Returns a record from a Tulip Table based on a provided record id
* `Create Record`: Creates a new record in a Tulip Table. Note: requires a unique id to be defined. You can use the expression rand(100000000, 99999999) for example.
* `Put Record`: Updates a record in a Tulip Table based on provided id and values to update

## Known Issues & Limitation
* Files Type - The connector is currently limited to strings, numbers, integers and booleans. It will not handle images, videos and files.
* Individual Records - Only handling individual table records. Likely to be expanded in the future.
* Duplication of Record ID in Update Record Function - The update record requires two ids, these must be identical. This is due to it being required by the Tulip API in both the path and the body. We are looking for a fix and will update here when it is implemented.

### For support see [support.tulip.co](support.tulip.co) or [support@tulip.co](mailto:support@tulip.co)

### This connector is provided for use by Tulip customers to leverage the Tulip API in the Microsoft environment. Usage of the connector is to [Tulip Privacy Policy](https://tulip.co/legal/privacy-policy/) and [API License Agreement](https://tulip.co/legal/api-license-agreement/)
