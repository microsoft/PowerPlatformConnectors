# BuildingMinds DigitalTwin Core Connector
BuildingMinds Core provides various REST APIs. With this API you can access the Digital Building Twin Masterdata API on the BuildingMinds Platform. A problem many customers have in the real estate sector is accurate information about their buildings. With the API customers and partners can use and create Apps which build on the Digital Building Twin.

## Publisher: BuildingMinds

## Prerequisites
To use the connector you need an account on the BuildindMinds Platform and user with appropriate authorisation. See below for getting access.

## Supported Operations
Required. Describe actions, triggers, and other endpoints.​
### Operation 1
Description of operation 1.

The connector supports the following operations:
* `GetPortfolios`: Get all portfolios 
* `GetSites`: Get all sites 
* `GetBuildings`: Get all buildings 
* `GetFloors`: Get all floors 
* `GetRoofs`: Get all roofs 
* `GetFacades`: Get all facades 
* `GetOutsideareas`: Get all outside areas  
* `GetSubareas`: Get all sub areas 
* `GetLands`: Get all lands 
* `GetSpaces`: Get all spaces 
* `GetPortfolioById`: Gets a portfolio with the given ID 
* `GetSiteById`: Gets a site with the given ID 
* `GetBuildingById`: Gets a buildings with the given ID 
* `GetFloorById`: Gets a floor with the given ID 
* `GetRoofById`: Gets a roof with the given ID 
* `GetFacadeById`: Gets a facade with the given ID 
* `GetOutsideareaById`: Gets an outside aread with the given ID 
* `GetSubareaById`: Gets a suba area with the given ID 
* `GetLandById`: Gets a land with the given ID 
* `GetSpaceById`: Gets a  space with the given ID 
* `CheckForChildrenOnPortfolio`: Gets a site with the given ID 
* `CheckForChildrenOnSite`: Gets the information about the existence of children on a site
* `CheckForChildrenOnBuilding`: Gets the information about the existence of children on a building
* `CheckForChildrenOnFloor`:Gets the information about the existence of children on a floor
* `CheckForChildrenOnRoof`: Gets the information about the existence of children on a roof
* `CheckForChildrenOnFacade`: Gets the information about the existence of children on a facade
* `CheckForChildrenOnOutsidearea`: Gets the information about the existence of children on an outside area
* `CheckForChildrenOnSubarea`: Gets the information about the existence of children on a sub area
* `CheckForChildrenOnLand`: Gets the information about the existence of children on a land
* `CheckForChildrenOnSpace`: Gets the information about the existence of children on a space
* `GetAssociatedSpacesForSpace`:  Get the list of associated entities
* `GetUnassociatedSpaces`: Get the list of not associated entities

## Obtaining Credentials
Please contact us under: ​https://buildingminds.com/services/customer-support or support@buildingminds.com

## Known Issues and Limitations
No known issues.

## Deployment Instructions
1. Clone the PowerPlatformConnectors GitHub repository
2. Open a terminal, then change to the `Docurain` directory
3. Run `paconn login`, then follow the authentication steps
4. Once authenticated, run `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json`
5. Select the target environment for your connector