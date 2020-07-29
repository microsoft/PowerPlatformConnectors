
﻿
## GoFileRoom Connector
GoFileRoom is a cloud-based document management and workflow solution. With the GoFileRoom connector, you can perform document, user management, and FirmFlow actions.


## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
* Active GoFileRoom login
* API Key for your GoFileRoom database


## API documentation
<<https://developerportal.thomsonreuters.com/>> 


## Supported Operations
The connector supports the following operations:
* `Copy Document`: Copies the documents
* `Create Document`: Creates a document record with indexes
* `Create Group`: Creates permission group
* `Create Users`: Creates GoFileRoom and ClientFlow users into the System
* `Delete Document`: Deletes a document
* `Delete User`: Delete GoFileRoom and ClientFlow users from the System
* `Document Reindex`: Reindexes the documents
* `Document Search`: Loads all the documents the user has access to in the specified drawer.
* `Get Child Indexes`: Gets a list of child index values based on the list ID, which is provided when the parent list value is selected.
* `Get Document`: Downloads the document
* `Get Document History`: Gets the history of the document
* `Get Document Indexes`: Retrieves the document's indexes
* `Get Document Status`: Get the Status of Documents
* `Get Drawer Indexes`: Retrieves a list of document indexes that are set up for the drawer
* `Get Drawers`: Fetches the drawers accessible for the Logged in User
* `Get Dynamic Rules for Index`: Gets the list of dynamic rules configured for the specified index
* `Get FirmFlow Data by Deliverable`: Gets the Tracking Report by Deliverable data
* `Get Group Document Security`: Retrieve group document security
* `Get Group Permissions`: Gets permissions of groups
* `Get Groups`: Fetches the groups available for user and group management
* `Get Licenses`: Retrieves licensing counts
* `Get List of Index Values`: Gets all the entries for a list type index
* `Get List of Reports`: Fetches the reports available for user management
* `Get LookupList Values`: Fetches the values from the LookupList
* `Get Password Policy`: Fetches the Password policy for the firm
* `Get Upload Locations`: Fetches the upload locations for the user
* `Get User Document Security`: Fetches user document security per login
* `Get User Info`: Fetches the GFR User information
* `Get User Permissions`: Gets permissions of user
* `Get Users`: Fetches the GFR users for the Logged in User
* `Index LookupList Find`: This method is designed for use with autocomplete fields.
* `Login`: Logs a user into the application
* `Logout`: Logout of application
* `Merge PDF's`: Merge PDF files together
* `Modify Group`: Modify group permissions
* `Modify User`: Modify user permissions
* `Publish Document Status`: Publish or Unpublish documents from ClientFlow.
* `Set Group Document Security`: Assigns document security to a group
* `Set Group Permissions`: Assigns permission to group
* `Set User Document Security`: Assigns user document security permissions
* `Set User Permissions`: Assigns permission to user
* `Taxsort Document`: Taxsort PDF document
* `Validate Indexes`: Checks whether the indexes provided are valid.
* `Validate Token`: Validate Token status



## How to get credentials
<<In order to have a valid GoFileRoom login you or your firm must have a GoFileRoom database. If you are an Administrator of GoFileRoom logins can be created by navigating to Administration | Manage Users and Groups | Users. If your firm does not have GoFileRoom and is interested in the application please refer to: https://tax.thomsonreuters.com/us/en/cs-professional-suite/gofileroom>> 


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

﻿