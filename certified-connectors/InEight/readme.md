# InEight Connector
InEight Connector provides an API to work with InEight objects.

## Publisher: InEight

## Prerequisites
You will need the following to proceed:
* Access to the InEight Cloud platform

## Obtaining Credentials:

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
​
* `DailyPlan_Get`: Gets all approved daily plans.
* `DailyPlanStatus_Import`: Sends confirmation to all received daily plans.
* `TimeCard_Get`: Gets all submitted time cards.
* `TimeCardConfirmation_Import`: Sends confirmation to all received time cards.
* `Crafts_Import`: Allows customers to provide and maintain a complete list of crafts or individual records as maintenance of the master list.
* `Employees_Import`: Allows customers to provide and maintain a complete list of all employee resources for their entire organization.
* `Equipments_Import`: Allows customers to provide and maintain a complete list of equipment that can be used on any given project.
* `EquipmentCategories_Import`: Allows an external system to maintain a master list of Equipment Categories in the InEight cloud platform.
* `EquipmentTypes_Import`: Allows an external system to maintain a master list of Equipment Types in the InEight cloud platform.
* `Roles_Import`: Allows customers to create roles in bulk for any user with default permissions.
* `Trades_Import`: Allows customers to provide a complete list of trades or individual records as maintenance of the master list.
* `Users_Import`: Allows customers to create and maintain information about the people that have login credentials and access to the InEight cloud platform from a third-party system.
* `UserRoles_Import`: Allows customers to assign roles to the users to setup access to the InEight cloud platform.