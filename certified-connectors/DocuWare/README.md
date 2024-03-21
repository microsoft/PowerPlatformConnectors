## DocuWare Power Automate Connector

DocuWare provides a powerful and extensive REST API. With DocuWare's Power Automate Connector, users can leverage many of the most common features of the DocuWare REST API to integrate DocuWare into their Power Automate flows.

## Prerequisites

You will need the following to proceed:
* A DocuWare Cloud subscription
* A Microsoft Power Apps or Power Automate account

## Setting Up The Connector

Setting up DocuWare's Power Automate Connector is easy. You must first generate a new API Key and then create a connection in Power Automate that let's you use that API Key with the DocuWare Power Automate connector.

### Creating a New API Key

Creating a new API Key is easy and can be done from within DocuWare.

Follow these steps to create a new API Key in DocuWare:

1. Log into your DocuWare account that you want to use with Power Automate
2. From the main menu, navigate to "Configurations"
3. From the "Configurations" page, go to the "Integrations" section and click on the icon for "Power Automate API Keys"
   - If you do not see the "Power Automate API Keys" icon, then it is possible your user does not have the correct permissions. Contact your DocuWare administrator to resolve this.
4. Click the "Create API Key" button
5. Fill out the "Name" and "Description" (optional) values, and click "Create"
   - After pressing "Create", there might be a slight delay as the configuration process creates your key.
   - Also, be sure to save your API Key. This will be your only opportunity to view the full API Key. If you lose this key, you will need to create another one.

At this point, you now have a valid API Key that can be used in Power Automate to create a connection to DocuWare. This connection can then be used with the DocuWare Power Automate Connector.

### Create the Power Automate Connection

Creating a connection in Power Automate is easy and can be done when you are creating your flows.

Follow these steps to create a new connection in Power Automate:

1. Login to Power Automate
2. Create a new flow or open up an existing flow
3. Add an activity from the DocuWare Power Automate connector to your flow
   - Click "+ New Step" to add a new step to your flow
   - When presented with "Choose an operation", select "All" from the category selection, and then type "DocuWare" in the search field.
   - Select the "DocuWare" connector and then choose from the list of available actions.
4. When the action is added to the flow, you will be prompted to create a new connection.
5. Enter a connection name ("Connection Name") and the API Key ("API Key") that you received from DocuWare in the previous section and click "Create".

From this point forward, all activities that are added from the DocuWare connector will use this connection. You can create additional connections by clicking on the "..." in the top right corner of an activity, and then clicking "+ Add new connection" at the bottom of the menu that pops up.

## Supported Operations

The DocuWare Power Automate connector supports the following operations:

* Store to DocuWare
  * Import to Document Tray - Import a new document into a document tray.
  * Store to File Cabinet - Stores a new document to a file cabinet.
* Get from DocuWare
  * Download a Document - Downloads a document from a file cabinet or document tray.
  * Download a File - Downloads a file/section of a document stored in a file cabinet or document tray.
  * Get Document Information - Gets information about a document.
  * Get Dialog Fields - Gets a list of fields for a dialog.
  * Get Dialogs - Gets a list of dialogs for a file cabinet or document tray.
  * Get File Cabinet Fields - Gets a list of fields for a file cabinet.
  * Get File Cabinets and Document Trays - Gets a list of file cabinets and/or document trays.
  * Get Organization - Get the organization name.
  * Get Stamp Fields - Gets a list of fields for a stamp.
  * Get Stamps - Gets a list of stamps for a file cabinet or document tray.
  * Search for Documents - Search a file cabinet for documents matching the specified criteria.
* Altering Documents
  * Append a File to a Document - Appends a file/section to an existing document.
  * Delete a Document - Deletes a document from a file cabinet or document tray.
  * Delete a File - Deletes a file/section from an existing document.
  * Place a Stamp - Places a stamp on a document.
  * Replace a File - Replaces a file/section in an existing document.
  * Transfer Documents - Moves one or more documents from one file cabinet/document tray to another.
  * Update Index Fields - Updates the index fields of a document.

## Connector Documentation

For detailed documentation, please refer to [Power Automate Connector Documentation](https://go.docuware.com/iPaaSPowerAutomateDocumentation)

## Known Issues and Limitations

At this time, DocuWare's Power Automate connector is not supported in the new Power Automate Flow designer.
