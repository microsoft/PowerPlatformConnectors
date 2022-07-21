# iManage Work

iManage is the industry-leading provider of document and email management solutions for knowledge workers. iManage platform organizes and secures the information in documents and emails, so professionals can search for what they need, act on it, and collaborate more effectively.

## Pre-requisites

This connector is available for all iManage Work customers connecting to the cloudimanage.com or imanage.work endpoints. You will need the iManage Work URL specific to your environment and the login credentials for the account under which the connector can execute actions.

## Supported Operations

The connector supports following operations:

### Document

- `Upload document`: Uploads a file as a new document to the specified folder.
- `Download document`: Downloads a version of a specific document, based on the document ID.
- `Get document profile`: Gets the document properties associated with a specific document ID.
- `Update document profile`: Performs updates on the document profile.
- `Update current or create new document version`: Updates the specified version of a document, or creates a new version. This action can update the profile and content of a document.

### Workspace

- `Create workspace`: Creates a new workspace based on the selected template.
- `Update workspace`: Updates information of the specified workspace.
- `Get workspace profile`: Gets the workspace profile, and optionally the list of allowed operations for the workspace.
- `Search for workspaces`: Searches and returns workspaces that match the specified criteria within a library.

### Library

- `Get libraries`: Gets a list of libraries to which the user has access.
- `Get classes`: Gets the list of document classes available in a library that match the specified criteria.
- `Get subclasses`: Gets all the library subclasses that match the specified criteria.
- `Get workspace templates`: Gets list of templates available in a library.
- `Search for folders`: Searches and returns folders that match the specified criteria within a library.

### User

- `Get user details`: Gets information about a user from the specified library.

### Group

- `Get group members`: Get details of users that belong to the specified group and match the specified criteria.

### Common Operations

- `Get trustees`: Returns a list of trustees with certain permissions for a document, folder or workspace.
- `Edit name-value pairs`: Creates, updates or deletes name-value pair properties of a document, folder or workspace.
