# RiskIQ Digital Footprint

RiskIQ Digital Footprint for Microsoft enables security teams to take control of their attack surface, reducing their risk and creating a better defense. The RiskIQ Digital Footprint connector for Microsoft will automatically make your external asset inventory including asset metadata available to your team for automated operations. Use this data to build reports, trigger alerts or aid in the identification of vulnerabilities or exposures against your assets.

## Pre-requisites

You will need the following to proceed:  
* A Microsoft Power Apps or Power Automate plan with custom connector feature  
* An Azure subscription  
* The Power platform CLI tools  
* [RiskIQ API credentials](https://api.riskiq.net/api/concepts.html)  

## Supported Operations

The connector supports the following operations:
* `Get assets by type` :  Retrieve the asset of the specified type and name from Global Inventory.  
* `Get asset by ID` : Retrieve the asset of the specified UUID from Global Inventory.  
* `Get connected assets by type` : Retrieve the set of assets which are connected to the requested asset.  
* `Get assets from recent dataset by search id or name` : Search Global Inventory recent dataset for a set of assets that match the criteria.  
* `Get task by ID` : Retrieve the status of an asynchronous global inventory update task.  
* `Get the list of confirmed assets added or removed by type` : Retrieve the list of confirmed assets that have been added or removed from inventory over the given time period. Retrieve the list of asset detail changes in inventory over the given time period.   
* `Get the count of confirmed assets added or removed` : Retrieve summary describing counts of confirmed assets that have been added or removed from inventory over the given time period.  
* `Get the list of newly opened ports` : Retrieve the list of newly opened ports hits.  
* `Get the list of brands` : Retrieve the list of brands defined for a workspace.  
* `Get the list of organizations` : Retrieve the list of organizations defined for a workspace.  
* `Get the list of tags` : Retrieve the list of tags defined for a workspace.  
* `Get the list of saved searches` : Retrieve the list of saved searches for a workspace.  
* `Request to search the list of assets by type` : Bulk retrieve a set of assets by name and type.  
* `Request to get the assets from the recent dataset that match the criteria` : Search Global Inventory recent dataset for a set of assets that match the criteria.  
* `Request to get the assets from the recent dataset that match the criteria` : Search Global Inventory historical dataset for a set of assets that match the criteria.  
* `Add the assets to global inventory` : Add one or more assets and a set of properties.  
* `Update the assets to global inventory` : Update one or more properties on a set of assets.  
* `Update the assets to global inventory using historical search` : Update one or more properties on a set of assets. This will use historical search if updating via a query, otherwise it works the same as /update.  
* `Cancel the task for global inventory update` : Cancel further processing of an asynchronous Global Inventory update task.  
 
## How to get credentials

Register for a test API key at [RiskIQ Security Intelligence Services](https://api.riskiq.net/api/concepts.html) or contact your account representative (support@riskiq.com) to identify your existing customer keys.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps. Additionally, you can leverage this connector within Logic Apps.