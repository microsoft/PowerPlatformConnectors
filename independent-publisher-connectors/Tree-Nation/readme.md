# Tree-Nation
The simplest way for citizens and companies to plant trees around the world and offset their CO2 emissions. Tree-Nation is on a mission to plant 1 trillion trees by 2050.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to create an account with [Tree-Nation](https://tree-nation.com/) and request access to the API.

## Obtaining Credentials
Tree-Nation will email you an API token to use when your API access has been approved.

## Supported Operations
### Project list
If the parameter status=active is passed, only active projects (those where a tree can be planted) will be listed.
### Project details
Retrieve the details about a project.
### Sites list
Retrieve the available planting sites in a project.
### Species list
Retrieve the available species in a project. Only those species with stock > 0 can be used for planting.
### Species details
Retrieve the details about a species.
### Forest details
Retrieve a user forest details.
### Forest tree count
Retrieve a user forest tree count (by slug). Period is optional and can be one of: day, week, month, quarter, year. If period is not passed, the all time value will be returned.
### Plant
Plant a tree.
### Create user
Create a new user.
### Tree template details
Retrieve the details about the tree template of a planter.
### Update tree template
Modify the message of a tree template.
### Buy credit
Buy credit.

## Known Issues and Limitations
There are no known issues at this time.
