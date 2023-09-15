# iManage Work

iManage is the industry-leading provider of document and email management solutions for knowledge workers. iManage platform organizes and secures the information in documents and emails, so professionals can search for what they need, act on it, and collaborate more effectively. The iManage Work connector enables users to automate repetitive tasks and approval processes while they keep all their files secure in the iManage Work environment.

## Publisher: iManage LLC

## Prerequisites

The connector is available for all iManage Work customers connecting to cloudimanage.com. First, the Power Automate application will need to be enabled by an administrator of your iManage Work environment. Once enabled, you will need the specific URL for your environment and a login account and password under which the connector can execute actions. For more information, see the FAQ.

## Supported Triggers

### For a selected document

This trigger allows you to start a workflow for a selected document in an iManage library.

## Supported Operations

### Upload document

Uploads a file as a new document to the specified folder.

### Download document

Downloads a version of a specific document, based on the document ID.

### Get document profile

Gets the document properties associated with a specific document ID.

### Update document profile

Performs updates on the document profile.

### Update current or create new document version

Updates the specified version of a document, or creates a new version. This action can update the profile and content of a document.

### Create workspace

Creates a new workspace based on the selected template.

### Update workspace

Updates information of the specified workspace.

### Get workspace profile

Gets the workspace properties associated with a specific workspace ID.

### Search for workspaces

Searches and returns workspaces that match the specified criteria within a library.

### Get libraries

Gets a list of libraries to which the user has access.

### Get classes

Gets the list of document classes available in a library that match the specified criteria.

### Get subclasses

Gets all the library subclasses that match the specified criteria.

### Get workspace templates

Gets a list of workspace templates available in a library.

### Search for folders

Searches and returns folders that match the specified criteria within a library.

### Get user details

Gets information about a user from the specified library.

### Get group members

Get details of users that belong to the specified group and match the specified criteria.

### Get trustees

Gets a list of trustees with certain permissions for a document, folder or workspace. A trustee can be a user or group.

### Edit name-value pairs

Creates, updates or deletes name-value pair properties of a document, folder or workspace.

### Update default security

Updates the default security of a document, folder or workspace.

### Update permissions

Updates permissions of an object.

### Copy document

Copies a document to the specified folder.

### Move document

Moves a document to the specified folder.

### Add document reference

Adds document reference or document shortcut in another folder.

### Get core extended metadata properties of a document

Gets core extended metadata properties of a document.

### Set core extended metadata properties on a document

Sets core extended metadata properties on a document.

### Search core extended metadata taxonomy node values

Searches for core extended metadata taxonomy node values matching the specified search criteria.

### Delete document reference

Deletes the document reference from the specified folder.

### Get permissions

Gets user or group access permission properties of a document, folder or workspace. Response properties can vary with user or group permissions.

### Promote document version

Promotes an existing version of a document to the latest version. A new version is created from a specified Journal ID or Version and then promoted to latest.

### Get document versions

Gets profiles for all versions the current user has access to for a specified document.

### Update workflow state

Updates the state of a workflow run. This action can only be used in a workflow that has been created to start with a trigger provided by iManage.

### Add document history entry

Adds an entry as an event in the document's history.

### Copy permissions

Copies the access permissions, and optionally the default security, from one iManage Work object to another. 'Copy Type' gives you options for how to handle the combination of source and destination permissions. 'Overwrite' updates the target object's users, groups, and permission levels to exactly match the source object's. 'Merge' keeps all the users and groups on the destination object and combines them with the source, but in case of a conflict of permission level on an individual or group: 'Merge (Pessimistic)' takes the lower permission, while 'Merge (Optimistic)' takes the higher permission for the conflicting user or group.

## Obtaining Credentials

If you are an existing iManage Work user, provide your iManage Work credentials to sign in. Otherwise, contact your System Administrator for assistance.

## Known Issues and Limitations

For a list of known issues and limitations, visit https://docs.imanage.com/power-automate/index.html.

## Frequently Asked Questions

For a list of Frequently Asked Questions, visit https://docs.imanage.com/power-automate/index.html.

## Deployment Instructions

To deploy this connector as custom connector in Microsoft Power Automate and Power Apps, visit https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli
