## PIPware Connector
This connector allows you to save KPI Target or KPI Actual data to your PIPware instance. This is more efficient than capturing the data into PIPware manually. Commonly used data sources are Excel or SQL, but any source returning data in the expected format can be used.

## Pre-requisites
You will need the following to proceed:
* A Power Platform premium plan
* The Power platform CLI tools (paconn)
* An activated login to a PIPware instance for testing purposes
* Your user account's API key for testing purposes
* Your PIPware user account will need the "KPIActual" permission in order to use the "Save KPI Actuals" step in this Connector.
* Your PIPware user account will need the "KPITarget" permission in order to use the "Save KPI Targets" step in this Connector.

## Building the connector 

### Deploying the sample
Follow the below instructions to deploy the connector to your Power Automate account to test out in some Flows:
* After making any changes to any of the json files run `paconn validate --api-def apiDefinition.swagger.json` and  `paconn validate -s settings.json` to make sure it passes Microsoft's requirements without errors or warnings
* To override an existing connector use `paconn update --api-def apiDefinition.swagger.json --api-prop apiProperties.json`. This is the only format that properly pushes both the swagger and properties file. The settings file is not necessary to push usually, and doesn't push the properties file.
* If you want to push it up to a brand new connector use `paconn create`. See the paconn documentation for details.
* To double check if you pushed it up correctly you can use `paconn download` in a different directory on your machine. 

## Supported Operations
The connector supports the following operations:
* `SaveKPIActuals`: Create or update KPI values in your PIPware instance
* `SaveKPITargets`: Create or update KPI targets in your PIPware instance
