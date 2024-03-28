# DocuWare Power Automate Connector

DocuWare provides a powerful and extensive REST API. With DocuWare's Power Automate Connector, users can leverage many of the most common features of the DocuWare REST API to integrate DocuWare into their Power Automate flows.

## Publisher: DocuWare GmbH

## Prerequisites

You will need the following to proceed:
* A DocuWare Cloud subscription
* A Microsoft Power Apps or Power Automate account

## Supported Operations

The DocuWare Power Automate connector supports the following actions:

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

### Import to Document Tray
* Import a new document into a document tray.

### Store to File Cabinet
* Stores a new document to a file cabinet.

### Download a Document
* Downloads a document from a file cabinet or document tray.

### Download a File
* Downloads a file/section of a document stored in a file cabinet or document tray.

### Get Document Information
* Gets information about a document.

### Get Dialog Fields
* Gets a list of fields for a dialog.

### Get Dialogs
* Gets a list of dialogs for a file cabinet or document tray.

### Get File Cabinet Fields
* Gets a list of fields for a file cabinet.

### Get File Cabinets and Document Trays
* Gets a list of file cabinets and/or document trays.

### Get Organization
* Get the organization name.

### Get Stamp Fields
* Gets a list of fields for a stamp.

### Get Stamps
* Gets a list of stamps for a file cabinet or document tray.

### Search for Documents
* Search a file cabinet for documents matching the specified criteria.

### Append a File to a Document
* Appends a file/section to an existing document.

### Delete a Document
* Deletes a document from a file cabinet or document tray.

### Delete a File
* Deletes a file/section from an existing document.

### Place a Stamp
* Places a stamp on a document.

### Replace a File
* Replaces a file/section in an existing document.

### Transfer Documents
* Moves one or more documents from one file cabinet/document tray to another.

### Update Index Fields
* Updates the index fields of a document.

## Obtaining Credentials
The authentication method is an API key.
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

## Known Issues and Limitations

At this time, DocuWare's Power Automate connector is not supported in the new Power Automate Flow designer.

## Deployment Instructions
To deploy this connector as a custom connector in Power Automate, follow these instructions:

1. Open the Power Automate portal.
2. Navigate to the "Custom connectors" section.
3. Click on the "New custom connector" button.
4. Choose the option to "Import an OpenAPI file".
5. Provide a name and description for the custom connector.
6. Click on the "Import" button and select the OpenAPI file for the DocuWare connector.
7. Review and update the connector details, such as icon, color, and authentication settings.
8. Save the custom connector.
9. Test the connector by creating a new flow and adding actions from the DocuWare connector.
10. Share the custom connector with other users or teams if needed.
