## Cloud Commander
Cloud Commander is a unified Microsoft CSP portal that lets you automate common tasks across all Microsoft 365 apps without switching consoles or customers.

## Publisher: N-Able Technologies Ltd.

## Prerequisites
You will need the following to proceed:
* A Cloud Commander subscription https://www.n-able.com/products/cloud-commander.
* Configured and customers with an approved status in Cloud Commander for the specific operations you intend to use.
* Configured roles in Cloud Commander for the specific operations you intend to use.

## Supported Operations
The connector supports the following operations:
* `List organizations`: List all organizations you are authorized to view.
* `List users`: Retrieve a list of user objects from Cloud Commander.
* `Create user`: The request body contains the user to create. At a minimum, you must specify the required properties for the user.
* `Get user details`: Get user by Microsoft object identifier.
* `Delete user`: Delete a user. When deleted, user resources are moved to a temporary container and can be restored within 30 days.
* `Update user`: Update the user.
* `Get user license details`: Retrieve a list of license details objects for enterprise users.
* `Get organization subscribed SKUs`: Get the list of commercial subscriptions that an organization has acquired.
* `Assign a license to a user`: Add or remove licenses for the user. You can also enable and disable specific plans associated with a license.
* `Get Cloud Commander groups`: Get a list of Cloud Commander groups of all organizations you are authorized to view.
* `Get a list of Cloud Commander group membe`: Get a list of Cloud Commander group members of a specific group.
* `Get Microsoft Entra ID security groups`: Get a list of Microsoft Entra ID security groups of all organizations you are authorized to view.
* `Create Microsoft Entra ID group`: Create an Microsoft Entra ID group.
* `List organization domains`: Retrieve a list of domain objects.
* `Delete Cloud Commander group`: Delete a Cloud Commander group.
* `Remove a member from a Cloud Commander group`: Remove a member to a Cloud Commander group.
* `Add a member to a Cloud Commander group`: Add a member to a Cloud Commander group, the member should belong to the same organization as the group.
* `Add a member to a Microsoft Entra ID group`: Add a member to a Microsoft 365 group or a security group through the members navigation property.
* `Remove member from a Microsoft Entra ID group`: Remove a member from a group via the members navigation property.
* `Get Partner Center subscriptions`: Retrieves all subscriptions for the customer of a partner by their active directory tenant identifier.
* `Update Partner Center subscription quantity`: Update the quantity of a subscription.
* `List Command Blocks`: Retrieve a list of available Command Blocks.
* `Get commandblock jobs for all authorized organizations`: Get commandblock jobs for Command Blocks the calling user is authorized to view.
* `Get statistics on the Command Block jobs for all authorized organizations`: Get statistics on the Command Block jobs for Command Block the calling user is authorized to view.
* `Get commandblock jobs of a specific organization`: Get commandblock jobs for Command Blocks the calling user is authorized to view.
* `Get Command Block details`: Get the details of Command Block, provider and action validation depend on the specific Command Block.
* `Add a Command Block job for a user`: Add a Command Block job, provider and action validation depend on the specific Command Block.
* `Add a Command Block job for a device`: Add a Command Block job, provider and action validation depend on the specific Command Block.
* `Add a Command Block job for a group`: Add a Command Block job, provider and action validation depend on the specific Command Block.
* `Add a Command Block job for an organization`: Add a Command Block job, provider and action validation depend on the specific Command Block.
* `Get details of a single user job`: Get details of a single user job, provider and action validation depend on the specific Command Block.
* `Get Command Categories`: Get all available categories and subcategories.
* `Get locales`: Get a list of locales.
* `Get manager`: Get manager of the user.
* `Assign manager`: Assign manager to the user.
* `Convert to shared mailbox`: Convert user mailbox to shared mailbox.
* `Set mailbox forwarding address`: Set mailbox forwarding address.
* `Set OneDrive site permissions`: Set OneDrive site permissions.

## Obtaining Credentials
You can login with any account that is configured to access Cloud Commander. For automation scenarios we advise you to use a dedicated service account.

## Known issues and limitations
There are no known issues or limitations specific to the Cloud Commander connector.

## Deployment instructions
Please refer to [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.