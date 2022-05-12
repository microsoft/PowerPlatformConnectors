# InEight Connector
InEight Connector provides an API to work with InEight objects.

## Publisher: InEight

## Prerequisites
You will need the following to proceed:
* Access to the InEight Cloud platform
* An Azure subscription

## Using the connector
The InEight cloud platform APIs are secured by Azure Active Directory (AD) so, you first need to set up a user account with the required permissions in the cloud platform. This allows InEight connectors to access the InEight cloud platform securely.

### User account:
A valid user account is required to create a connection and use the InEight connector. Work with administrator to set up a user account to access the InEight cloud platform.

### Tenant Prefix:
This is part of the environment url.

Example:
- Environment url: [customer].hds.ineight.com
- Tenant Prefix: [customer]

### Ocp-Apim Subscription Key:
Work with an administrator to get subscription key.

## Supported Operations
The connector supports the following operations:

* `DailyPlan_Get`: Allows user to export all approved daily plans from Project Suite into their internal System of Record.
* `DailyPlanStatus_Import`: Allows user to import confirmation from an external source, into Project Suite for all received daily plans.
* `TimeCard_Get`: Allows user to export all submitted time cards from Project Suite into their internal System of Record.
* `TimeCardConfirmation_Import`: Allows user to import confirmation for  all received time cards from an external source into Project Suite.
* `Crafts_Import`: Allows customers to maintain a complete list of crafts or individual records as a part of the master list.
* `Employees_Import`: Allows customers to maintain a complete master list of all employee resources for their entire organization.
* `Equipments_Import`: Allows customers to import a complete list of equipment master data that can be used on any given project.
* `EquipmentCategories_Import`: Allows an external system to import  a master list of Equipment Categories in the InEight cloud platform.
* `EquipmentTypes_Import`: Allows an external system to import  a master list of Equipment Types in the InEight cloud platform.
* `Roles_Import`: Allows customers to create roles in bulk with default permissions.
* `Trades_Import`: Allows customers to import a complete list of trades or individual trade records as maintenance of the master list.
* `Users_Import`: Allows customers to maintain information about the people that have login credentials and access to the InEight cloud platform from a third-party system.
* `UserRoles_Import`: Allows customers to maintain users roles in bulk.
