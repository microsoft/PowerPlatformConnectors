## Create Asset Oxmaint
The Create Asset connector (Oxmaint) allows users to add new assets with relevant metadata and properties for efficient tracking and management. Designed for CMMS, it simplifies the creation of assets, supporting maintenance planning and work order management.


## Publisher: Oxmaint Inc.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature.
* Oxmaint Subscription Account


## Supported Operations
The connector supports the following operations:

### Create Asset
Allows users to create a new Asset within the Oxmaint system. Which will help him to do operations in oxmaint like maintenance, inspection etc.
 
## Obtaining Credentials
* Visit https://oxmaint.com
* Then Sign up for Oxmaint.
* Fill the necessary details Like: Name, Business Email, Password (Email and password will be required for useing the connector). 


## Getting Started
These are the parameters that are required for connector:-

* `Master Email`: User's unique Oxmaint email for asset management access.
* `Password`: Provided by Oxmaint support or user for secure login.
* `Asset Type`: Categorizes the kind of asset (e.g.: Machine, Asset).
* `Asset Name`: Specific name or identifier of the asset.
* `Asset Number`: Unique identifier or serial number for the asset.
* `Asset Category`: Broader category that groups the asset (e.g.: fleet, equipment etc.).
* `Asset Model`: Manufacturer model name or number for the asset.
* `Odometer`: Total distance or usage the asset has covered.
* `Odometer Unit`: Unit of measurement for odometer reading (e.g.: Hours, Miles, KM).
* `Status`: Current operational condition of the asset (e.g.: "Available", "In Maintenance", "Maintenance Required", "Breakdown", "Discontinue", "Deployed", "Idle").
* `Asset Operator`: Name of the person or entity responsible for operating the asset.

## Known Issues and Limitations
* The connector is designed to work with Oxmaint's CMMS system and not compatible with other.
* User can Add only one asset with same asset number.
