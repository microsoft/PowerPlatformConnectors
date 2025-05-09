The Azure IoT Central V3 connector can be used to connect your Azure IoT Central V3 application to your workflows. You can use the connector to run workflows when a rule is fired. You can use it to get the latest information from your IoT devices such as telemetry data and properties. And you can also use it to update your IoT devices by sending commands, and updating properites.
 
## Prerequisites
 
- You must be a user in an Azure IoT Central V3 Pay-As-You-Go application to use this connector.
- You must use an Azure Active Directory work or school account.
 
## How to get credentials
 
To add this connector to your workflow, you must first sign into the Azure Active Directory work or school account that is a user in the Azure IoT Central application you want to connect to.

## Release notes

### September 2021
- Azure IoT Central V3 connector has now been updated to align with our generally available [1.0 REST API](https://docs.microsoft.com/en-us/rest/api/iotcentral/) surface. All of the connector actions now support our [DTDLv2 format](https://github.com/Azure/opendigitaltwins-dtdl/blob/master/DTDL/v2/dtdlv2.md) and support for DTDLv1 based models is being deprecated.
- The actions to `Run a device command`, `Get/Update device properties`, and `Get device telemetry value` have been updated to add support to the
[latest device template features](https://docs.microsoft.com/en-us/azure/iot-central/core/concepts-device-templates) in Azure IoT Central. These actions now
support both root and component capabilities.
- A new set of device module actions has been added. These actions allow the creation of workflows that interact with Commands, Properties, Telemetries
of both device modules and module components.
- Actions to `Get/Create/Update/Delete a device` have been updated to align with the latest Azure IoT Central [REST API](https://docs.microsoft.com/en-us/rest/api/iotcentral/1.0/devices).
- The actions `Get device cloud properties` and `Update device cloud properties` are now part of `Get device properties` and `Update device properties`.
The new actions enable workflows to interact with both Device Properties and Cloud Properties simultaneously.

> **Deprecation notice:** by the end of CY21, the actions marked as deprecated in the list below will no longer be available when building new workflows.
Existing workflows referencing these actions will continue to work until March 2022. We encourage you to update your workflows to take advantage of the newly announced actions that are now supported within Azure IoT Central.

## Known issues and limitations
 
- To use this connector in Microsoft Power Automate, you must have signed into the IoT Central application at least once. Otherwise the application won't appear in the Application dropdown
- Microsoft personal accounts (such as @hotmail.com, @live.com, @outlook.com domains) are not supported to sign into the IoT Central connector. You must use an Azure Active Directory work or school account.
