
### Fuxsy-AutodeskFusionManage (PLM) connector
The Fuxsy-Autodesk Fusion Manage (PLM) Connector is a custom integration solution developed by Fuxsy s.r.o. It bridges Autodesk Fusion Manage (PLM) and Microsoft Power Platform, two powerful low-code process platforms.
This connector enhances Autodesk PLM's capabilities by leveraging features provided by Microsoft Power Platform, including:
* MS Power Automate: Automate processes and functions in Autodesk PLM such as PDF publishing and task scheduling.
* MS Power Apps: Create your own custom applications integrated with Autodesk PLM.
* MS PowerBi: Access Autodesk PLM data within a comprehensive reporting system.
* MS Sharepoint: Manage Autodesk PLM data and processes within SharePoint.
* Additional Features: Numerous other features are available to enhance your PLM processes.

## Publisher: Fuxsy s.r.o.
* For more information, visit: www.fuxsy.eu

## Prerequisites
### Obtaining Autodesk Forge app connector 
To be able integrate your Fusion Manage tenant with third-party apps, you need to generate a special set of integration credentials as part of Autodesk's standard security requirements.
For more details, visit the Autodesk Forge documentation: https://aps.autodesk.com/en/docs/oauth/v2/tutorials/create-app/

### Enabling integration of ClientID for your Fusion Manage Tenant
Once you have generated ClientID & ClientSecret inside of Autodesk Forge App, you must add these credentials to your Fusion Manage in order to enable tenant integration.
More information can be found here: https://help.autodesk.com/view/PLM/ENU/?guid=FLC_RestAPI_v3_API_2_legged_Tutorial_html

### Obtaining a Fuxsy PLM Connector Subscription key
Last but not least is to contact us to get your own Subscription Key which is a must have for using any of our Connector integration functions.
Free Funcions:
* We offer a free trial with access to a limited number of functions.
* For obtain free 

Paid Funcions:
* To get more details about all of our connector functions let us know at: support@fuxsy.eu

How to get a Fuxsy PLM Connector subscription key?

Send us an email at: support@fuxsy.eu
	- Subject : Fuxsy PLM Connector - Subscription key
	- Email body:
			- Tenant name : "YourTenantName.autodeskplm360.net"
			- Email address : Email address of the user in Autodesk Fusion == > "This email address will be used for API communication. This user must have adequate perrmissions in Fusion Manage to perform required API functions"
      Autodesk Forge app registration:
			- ClientID : Unique Autodesk Forge Client ID ==> which was generated inside on of the steps above. 
			- ClietnSecret: Unique Autodesk Forge API secret 
 
## Supported Operations
In order to get access to both, free & paid functions you will have to meet all of the Prerequisite described above.

### Free PLM Connector functions
The connector supports the following operations:
* Get - Workspace list: Retrieve all workspaces available in your Autodesk Fusion.
* Get - Item list: Retrieve a list of all items within specified workspace.
* Get - Item Detail: Retrieve all fields, sections, and additional information for a specified item in a defined workspace.
* Get - Report Details: Retrieve details from defined report.
* Get - Search Items: Search for items based on specified values.

### Paid PLM Connector functions 1.1 version
* The connector supports all functions available in the Free PLM Connnector
 
* Post - Item Create: gains you the ability to create a new item inside of chosen workspace.
CUSTOM_LOOKUP_NAME - picklist custom ID
INTEGER - array number of desired element inside of the picklist
WSID - Workspace ID
example:
{
  "sections": [
    {
      "link": "/api/v3/workspaces/WSID/sections/SECTION_ID",
      "fields": [
        {
          "__self__": "/api/v3/workspaces/WSID/views/1/fields/FIELD_CUSTOM_ID",
          "value": "string"
        },
        {
          "__self__": "/api/v3/workspaces/WSID/views/1/fields/FIELD_CUSTOM_ID",
          "value": "string"
        },
        {
          "__self__": "/api/v3/workspaces/WSID/views/1/fields/FIELD_CUSTOM_ID",
          "value": {
            "link": "/api/v3/lookups/CUSTOM_LOOKUP_NAME/options/INTEGER"
          }
        }
      ]
    },
    {
      "link": "/api/v3/workspaces/WSID/sections/SECTION_ID",
      "fields": [
        {
          "__self__": "/api/v3/workspaces/WSID/views/1/fields/FIELD_CUSTOM_ID",
          "value": {
            "link": "/api/v3/workspaces/WSID/items/DMSID"
          }
        },
        {
          "__self__": "/api/v3/workspaces/WSID/views/1/fields/FIELD_CUSTOM_ID",
          "value": "string"
        }
      ]
    }
  ]
}
* Post - Item Create Row
* Put -  Item Update Details
* Post - Item Managed Items Create
* Post - Item Relationship Item Create
* Post - Item Transition
* Post - Attachment Create
* Post - Attachment Create Version
* Post - Attachment Folder Create
* Post - Script Create New
