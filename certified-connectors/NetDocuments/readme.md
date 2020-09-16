
# NetDcouments
NetDocuments is a cloud content management service for businesses of all sizes to securely create, store, manage and share their document work anywhere, anytime.


## Pre-requisites
To use this integration, you need to have an account with [NetDocuments](https://www.netdocuments.com).

	a. Access to NetDocuments Developer Portal
	b. NetDocuments Client ID 
	c. NetDocuments Client Secret
	d. A "Redirect URI" set for "https://global.consent.azure-apim.net/redirect"

###### Get the Client ID and Client Secret
If you do not have access to the Developer Portal to create a client id and secret, please open a ticket with API Support.

## API documentation
https://support.netdocuments.com/hc/en-us/articles/205219850-API-Documentation

## Supported Operations
The connector supports the following operations:
* ```GetUserInfo```: Get user info
* ```GetUserCabinets```: Get list of user cabinets 
* ```NewVersion```: Create a new version of a document
* ```GetDocInfo```: Get document profile
* ```RenameDocument```: Rename document
* ```GetDocContent```: Get document content
* ```DeleteDoc```: Delete document
* ```UpdateDocument```: Update document
* ```CreateFolder```: Create folder
* ```GetFldContent```: Get folder items
* ```FileFolder```: File or unfile document to folder
* ```DeleteFolder```: Delete folder
* ```RenameFolder```: Rename folder
* ```FollowFolder```: Follow folder
* ```FollowDocument```: Follow document
* ```SearchCab```: Search cabinet
* ```GetCurrentUserInfo```: Get current user info
* ```CheckinDoc```: Check in document
* ```CheckOutDoc```: Check out document
* ```CreateDocument```: Create document
* ```LockDocumentVersion```: Lock document version
* ```GetDocumentVersions```: Get document versions
* ```CreateSecuredLink```: Create secured link
* ```GetDocHistory```: Document history
* ```CreateWorkspaceParentChild```: Get or create a workspace (with parent and child attributes)
* ```CreateWorkspaceSingle```: Get or create a workspace (with a single attribute)
* ```GetWorkspaceInformation```: Get workspace information
* ```CreateChildEntry```: Create or update a child lookup table entry
* ```GetChildEntry```: Get a child lookup table entry
* ```DeleteChildEntry```: Delete a child lookup table entry
* ```CreateEntry```: Create or update a lookup table entry
* ```GetLookupEntry```: Get a lookup table entry or search for child table entries
* ```DeleteLookupEntry```: Delete a lookup table entry
* ```SearchLookupEntries```: Search for lookup table entries
* ```SearchCabinets```: Search one or more cabinets
* ```RefreshWorkspace```: Refresh a workspace
* ```LockDocument```: Lock document
* ```UnockDocument```: Unlock document
* ```GetRepositoryLog```: Get repository log
* ```GetRepositoryInformation```: Get repository information
* ```GetRepositoryUsers```: Get repository users
* ```GetRepositoryGroups```: Get repository groups
* ```CreateRepositoryGroup```: Create repository group
* ```DeleteRepositoryGroup```: Delete repository group
* ```CreateUser```: Create user
* ```AddOrRemoveUserRepository```: Add user to or remove user from repository
* ```CreateCollabSpace```: Create CollabSpace
* ```GetCabinetSettings```: Get cabinet settings
* ```GetCabinetInformation```: Get cabinet information
* ```GetCabinetCustomAttributes```: Get cabinet custom attributes
* ```GetCabinetDefaultAccess```: Get unhidden cabinet groups access rights
* ```AddToOrRemoveGroupFromCabinet```: Add to or remove group from cabinet
* ```GetCabinetGroups```: Get cabinet groups
* ```CreateCabinetExternalGroup```: Create a cabinet external group
* ```SearchCabinetModifyACLs```: Search a cabinet and modify ACLs
* ```GetContainerContents```: Get container contents
* ```GetGroupInformation```: Get group information
* ```GetGroupMembership```: Get group membership
* ```AddOrRemoveGroupMember```: Add or remove group member


## Further Support
For further support, please contact NetDcouments Support https://support.netdocuments.com
