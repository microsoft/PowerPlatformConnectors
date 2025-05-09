## Spinpanel connector
Spinpanel connector allows you to integrate the powerful multi-tier and multi-tenant Microsoft Cloud automation capabilities of Spinpanel as operations in Microsoft Power Automate and Power Apps. 

## Prerequisites
You will need the following to proceed:
* A Spinpanel Cloud Essentials, Cloud Business or Cloud Enterprise subscription
* Configured solutions in Spinpanel for the specific operations you intend to use
* Configured roles in Spinpanel for the specific operations you intend to use

## How to get credentials
You can login with any account that is configured to access Spinpanel. For automation scenarios we advise you to use a dedicated service account.

## Supported Operations
The connector supports the following operations:
* `Get locales`: Get a list of locales
* `Get users`: Retrieve a list of user objects from Spinpanel
* `Get user details`: Get user by Microsoft object identifier
* `Delete user`: Delete a user. When deleted, user resources are moved to a temporary container and can be restored within 30 days
* `User change password`: Update the passwordProfile of a user to reset their password
* `Get organizations`: Get all organizations you are authorized to view
* `Create user`: The request body contains the user to create. At a minimum, you must specify the required properties for the user
* `Get user license details`: Retrieve a list of license details objects for enterprise users
* `Get organization subscribed SKUs`: Get the list of commercial subscriptions that an organization has acquired
* `Assign a license to a user`: Add or remove licenses for the user. You can also enable and disable specific plans associated with a license
* `Get spinpanel user groups`: Get a list of groups of all organizations you are authorized to view
* `Get a list of Spinpanel user group members`: Get a list of user group members of a specific group
* `Get Azure AD security groups`: Get a list of Azure AD security groups of all organizations you are authorized to view
* `List organization domains`: Retrieve a list of domain objects
* `Create Azure AD group`: Create an Azure AD group
* `Delete Spinpanel user group`: Delete a Spinpanel user group
* `Remove a member from a user group`: Remove a member from a Spinpanel user group
* `Add a member to a user group`: Add a member to a Spinpanel user group, the member should belong to the same organization as the group
* `Add a member to a group`: Add a member to a Microsoft 365 group or a security group through the members navigation property
* `Remove member`: Remove a member from a Microsoft group via the members navigation property
* `Get subscriptions`: Retrieves all subscriptions for the customer of a partner by their active directory tenant identifier
* `Update subscription quantity`: Update the quantity of a subscription



