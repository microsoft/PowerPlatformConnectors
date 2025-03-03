# Fuxsy-AutodeskFusionManage
The Fuxsy-Autodesk Fusion Manage (PLM) Connector is a custom integration solution developed by Fuxsy s.r.o. It bridges Autodesk Fusion Manage (PLM) and Microsoft Power Platform, two powerful low-code process platforms.
This connector enhances Autodesk PLM's capabilities by leveraging features provided by Microsoft Power Platform, including:
* MS Power Automate: Automate processes and functions in Autodesk PLM such as PDF publishing and task scheduling.
* MS Power Apps: Create your own custom applications integrated with Autodesk PLM.
* MS PowerBi: Access Autodesk PLM data within a comprehensive reporting system.
* MS Sharepoint: Manage Autodesk PLM data and processes within SharePoint.

## Publisher: Fuxsy s.r.o.

## Prerequisites
You must be administrator of tenat to be able integrate Autodesk Fusion Managage 360 tenant within MS Power Platform. 
Integration doesnt work for Participant access. It works only for Professional & Enterprise license.

##Supported Operatins
In order to get access to both, free & paid functions you will have to meet all of the Prerequisite described above and have licenses from us.

Free PLM Connector functions
### Get - Workspace list
Retrieve all workspaces available in your Autodesk Fusion Manage Tenant

### Get - Item list:
Retrieve a list of all items in specified workspace.

### Get - Item Detail
Retrieve all fields, sections, and additional information for a specified item in defined workspace.

### Get - Report Details
Retrieve details from defined report.

### Get - Search Items
Search for items based on specified values.

Paid PLM Connector functions
### Get Groups & Users & Permissions
Get all informations from Users, Groups and Permissions

### Get Workpsace items, Views
Get all items in worskpace also within custom views

### Get - Search in Tenant or Workspace
Posibility to seach any object based on any string in whole Fusion Manage Tenant

### Get - Item logs, Detai, Fields, Values, Worflow
Get all informations about Item it self, Fileds, Values, Seccions and workflow also within history.

### Post - Item Create Row
Posibility to create Item in defined workspace. For more informations go below in frequently asked Questions.

### Put -  Item Update Details
Funcion for update existing item within whole or filed update.

### Post - Item Managed Items Create
Posibility to create new Item within link in Manage tab

### Post - Item Relationship Item Create
Posibility to create new Item within link in Relationship tab

### Post - Item Transition
Posibility to change state of item based on configured workflow.

### Get - Item Grid details
Get all informations from Grid tab from item

### Post - Item Grid Create
Post informations in to Item Grid tab to create new row within fields
### Put - Item Grid Update
Update fileds or row on item in Grid tab

### Post - Attachment Create
Create new Attachment as file for item 

### Post - Attachment Create Version
Create new version file of existing Attachment on Item 

### Post - Attachment Folder Create
Create new folder for Attachment for defined Item

### Post - Script Create New
Postibilities to create new script.
Upcate existing script is not available in Fusion over API

### Get - All Reports
Get list of all reports available in tenant

### Get - Reports details
Get all details of specified report

### Get - Item Print View
Get details of created Pring View for specific Item.
 
### Get - Picklis & Picklist Values
Get all picklist and values available in Fusin Manage tenant

## Obtaining Credentials
1) Obtaining Autodesk Forge app connector 
To be able integrate your Fusion Manage tenant with third-party apps, you need to generate a special set of integration credentials as part of Autodesk's standard security requirements.
For more details, visit the Autodesk Forge documentation: https://aps.autodesk.com/en/docs/oauth/v2/tutorials/create-app/

2) Enabling integration of ClientID for your Fusion Manage Tenant
Once you have generated ClientID & ClientSecret inside of Autodesk Forge App, you must add these credentials to your Fusion Manage in order to enable tenant integration.
More information can be found here: https://help.autodesk.com/view/PLM/ENU/?guid=FLC_RestAPI_v3_API_2_legged_Tutorial_html

3) Obtaining a Fuxsy PLM Connector Subscription key
Send us an email at: support@fuxsy.eu
	- Subject : Fuxsy PLM Connector - Subscription key
	- Email body:
			- Tenant name : "YourTenantName.autodeskplm360.net"
			- Email address : Email address of the user in Autodesk Fusion == > "This email address will be used for API communication. This user must have adequate perrmissions in Fusion Manage to perform required API functions"
      Autodesk Forge app registration:
			- ClientID : Unique Autodesk Forge Client ID ==> which was generated inside on of the steps above. 
			- ClietnSecret: Unique Autodesk Forge API secret 

## Getting Started

## Known Issues and Limitations
All Fusion Manage BOM funcios are not included in actual version

## Frequently Asked Questions

### How to structure Body content for create Item in Fusion Manage
Post - Item Create: gains you the ability to create a new item inside of chosen workspace.

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
