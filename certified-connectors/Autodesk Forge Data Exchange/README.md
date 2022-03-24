# Autodesk Forge® Data Exchange

Unleash your productivity and streamline collaboration by connecting design and make data between Revit and the tools of your choice.

## Publisher: Autodesk Inc.

## Prerequisites
You will need the following to proceed:

* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An [Autodesk Account](https://accounts.autodesk.com/)
* Access to [Autodesk Construction Cloud (ACC)](https://construction.autodesk.com/)

## Supported Operations

### Get details about a Data Exchange :
This operation gets details about a Data Exchange such as its Autodesk Docs project name, author of the file, and the last modified time, etc.

### Get properties of a Data Exchange :
This operation gets the properties of a Data Exchange. You can specify to get all the properties or only the latest properties.

## Get details of a Data Exchange property :
This operation gets details of a property associated with a Data Exchange.

## Supported Triggers

### When a Data Exchange is created :
This operation triggers a flow when a Data Exchange is created.

### When a Data Exchange is updated :
This operation triggers a flow when a Data Exchange is updated.

## Known Issues and Limitations
- The Revit element parameter values are represented in metric irrespective of the units specified on the source model.
- There is no localization support for parameter names currently. The built-in parameters will be available in English while the shared/project parameters will be available in the language used for their creation.
- Parameters of type Enumeration will currently only return the numeric value and not the string value.
- The **Get properties of a Data Exchange** action can return a maximum of 1024 dynamic parameter names. For exchanges that contain more than 1024 unique parameters, the list will be truncated to contain the top 1024 parameters.
- The **Get properties of a Data Exchange** action when used with the Latest properties option will not return records if only Type parameters have been changed. If the corresponding instance parameters have also been changed then both type parameters as well as instance parameters will be returned correctly.
- The **Get properties of a Data Exchange** action returns all the possible parameters within the selected Exchange. In some cases, there may be different parameters that have the same user-visible name. 
- Reference Parameters require an additional look up via the **Get details of a Data Exchange property** action to extract one or more fields from the referenced asset.


## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
