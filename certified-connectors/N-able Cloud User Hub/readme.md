## Cloud User Hub
Cloud User Hub is a unified Microsoft CSP portal that lets you automate common tasks across all Microsoft 365 apps without switching consoles or customers.

## Publisher: N-able Cloud User Hub B.V.

## Prerequisites
You will need the following to proceed:
* A Cloud User Hub subscription https://www.n-able.com/products/cloud-user-hub/trial
* Configured solutions in Cloud User Hub for the specific operations you intend to use
* Configured roles in Cloud User Hub for the specific operations you intend to use

## Obtaining Credentials
You can login with any account that is configured to access Cloud User Hub. For automation scenarios we advise you to use a dedicated service account.

## Supported Operations
The connector supports the following operations:
* `Get locales`: Get a list of locales
* `Get users`: Retrieve a list of user objects from Cloud User Hub
* `Get user details`: Get user by Microsoft object identifier
* `Delete user`: Delete a user. When deleted, user resources are moved to a temporary container and can be restored within 30 days
* `User change password`: Update the passwordProfile of a user to reset their password
* `Get organizations`: Get all organizations you are authorized to view
* `Create user`: The request body contains the user to create. At a minimum, you must specify the required properties for the user
* `Get user license details`: Retrieve a list of license details objects for enterprise users
* `Get organization subscribed SKUs`: Get the list of commercial subscriptions that an organization has acquired
* `Assign a license to a user`: Add or remove licenses for the user. You can also enable and disable specific plans associated with a license
* `Get Cloud User Hub user groups`: Get a list of groups of all organizations you are authorized to view
* `Get a list of Cloud User Hub user group members`: Get a list of user group members of a specific group
* `Get Azure AD security groups`: Get a list of Azure AD security groups of all organizations you are authorized to view
* `List organization domains`: Retrieve a list of domain objects
* `Create Azure AD group`: Create an Azure AD group
* `Delete Cloud User Hub user group`: Delete a Cloud User Hub user group
* `Remove a member from a user group`: Remove a member from a Cloud User Hub user group
* `Add a member to a user group`: Add a member to a Cloud User Hub user group, the member should belong to the same organization as the group
* `Add a member to a group`: Add a member to a Microsoft 365 group or a security group through the members navigation property
* `Remove member`: Remove a member from a Microsoft group via the members navigation property
* `Get subscriptions`: Retrieves all subscriptions for the customer of a partner by their active directory tenant identifier
* `Update subscription quantity`: Update the quantity of a subscription
* `List Command Blocks`: Retrieve a list of available Command Blocks
* `Get commandblock jobs for all authorized organizations`: Get commandblock jobs for Command Blocks the calling user is authorized to view.
* `Get statistics on the Command Block jobs for all authorized organizations`: Get statistics on the Command Block jobs for Command Block the calling user is authorized to view.
* `Get commandblock jobs of a specific organization`: Get commandblock jobs for Command Blocks the calling user is authorized to view.
* `Get Command Block details`: Get the details of Command Block, provider and action validation depend on the the specific Command Block.
* `Add a Command Block job for a user`: Add a Command Block job, provider and action validation depend on the specific Command Block.
* `Add a Command Block job for a group`: Add a Command Block job, provider and action validation depend on the specific Command Block.
* `Add a Command Block job for an organization`: Add a Command Block job, provider and action validation depend on the specific Command Block.
* `Get details of a single user job`: Get details of a single user job, provider and action validation depend on the the specific Command Block.
* `Get Command Categories`: Get all available categories and subcategories.

## Known issues and limitations
There are no known issues or limitations specific to the Cloud User Hub connector.