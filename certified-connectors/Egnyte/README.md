
# Egnyte

Egnyte is a content collaboration platform that keeps your files safe, synced, and easy to share. Connect to Egnyte to manage your files. You can perform various actions such as upload, move, download, and delete files in Egnyte.

## Pre-requisites

To use the connector, you need to be an Egnyte user.

## Supported Actions

The connector supports the following actions:

* `Create group`: Create a group in Egnyte. https://developers.egnyte.com/docs/read/Group_Management#Create-a-Group
* `Get group details`: Retrieve group info using ID. https://developers.egnyte.com/docs/read/Group_Management#Show-Single-Group
* `Get list of all groups`: Retrieve the list of the groups. https://developers.egnyte.com/docs/read/Group_Management#List-Groups
* `Get user details`: Retrieve the information about the user by ID. Learn more: https://developers.egnyte.com/docs/read/User_Management_API_Documentation#Get-Single-User
* `Get list of all users`: Retrieve the information about the users. Learn more: https://developers.egnyte.com/docs/read/User_Management_API_Documentation#Get-User-List
* `Update user by ID`: Update the single user. Learn more: https://developers.egnyte.com/docs/read/User_Management_API_Documentation#Update-Single-User
* `Create folder`: Create a folder in Egnyte. https://developers.egnyte.com/docs/File_System_Management_API_Documentation#Create-a-Folder
* `Delete file by path`: Delete a file using a path. https://developers.egnyte.com/docs/File_System_Management_API_Documentation#Delete-a-File-or-Folder
* `Delete folder by path`: Delete a folder using a path. https://developers.egnyte.com/docs/File_System_Management_API_Documentation#Delete-a-File-or-Folder
* `Delete folder by ID`: Delete a folder using an ID. https://developers.egnyte.com/docs/File_System_Management_API_Documentation#Delete-a-File-or-Folder
* `Delete file by ID`: Delete a file using an ID. https://developers.egnyte.com/docs/File_System_Management_API_Documentation#Delete-a-File-or-Folder
* `Copy file by path`: Copy a file by using a path. https://developers.egnyte.com/docs/File_System_Management_API_Documentation#Copy-File-or-Folder
* `Copy folder by path`: Copy a folder by using a path. https://developers.egnyte.com/docs/File_System_Management_API_Documentation#Copy-File-or-Folder
* `Full group update`: Overwrite all of the attributes of a group. https://developers.egnyte.com/docs/read/Group_Management#Full-Update-to-a-Group
* `Partial group update`: Update specific attributes of a group. https://developers.egnyte.com/docs/read/Group_Management#Partial-Update-to-a-Group
* `Delete group`: Delete a group by ID. https://developers.egnyte.com/docs/read/Group_Management#Delete-a-Group
* `Copy file by ID`: Copy a file by using an ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Copy-File-or-Folder
* `Copy folder by ID`: Copy a folder using an ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Copy-File-or-Folder
* `Move file by path`: Move a file in Egnyte using path. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Move-File-or-Folder
* `Move folder by path`: Move a folder in Egnyte using path. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Move-File-or-Folder
* `Move file by ID`: Move a file in Egnyte using file ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Move-File-or-Folder
* `Move folder by ID`: Move a folder using ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Move-File-or-Folder
* `Share file`: Share a file in Egnyte. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#Create-a-Link
* `Share folder`: Share a folder in Egnyte. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#Create-a-Link
* `File info by path`: Retrieve file info using path. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation
* `Folder info by path`: Retrieve folder info using path. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation
* `File info by ID`: Retrieve file info using file ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation
* `Folder info by ID`: Retrieve folder info using ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation
* `Lock file by path`: Lock file using path. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Lock-a-File
* `Unlock file by path`: Unlock file using path. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Unlock-a-File
* `Lock file by ID`: Lock file using ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Lock-a-File
* `Unlock file by ID`: Unlock file using ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Unlock-a-File
* `Get file content by path`: Retrieves the file contents from Egnyte using path. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Download-File
* `Get file content by ID`: Retrieves the file content from Egnyte using ID. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Download-File
* `Create file`: Uploads a file to Egnyte. https://developers.egnyte.com/docs/read/File_System_Management_API_Documentation#Upload-a-File
* `Set metadata by file ID`: Write metadata to a file by using file ID. https://developers.egnyte.com/docs/Metadata_API#Set-Values-for-a-Namespace
* `Set metadata by folder ID`: Write metadata to a folder by using folder ID. https://developers.egnyte.com/docs/Metadata_API#Set-Values-for-a-Namespace
* `Get all namespaces`: Get a list all custom metadata namespaces in the domain. https://developers.egnyte.com/docs/Metadata_API#Get-All-Namespaces
* `Create namespace`: Create a namespace in Egnyte. https://developers.egnyte.com/docs/Metadata_API#Create-Namespace
* `Update namespace attributes`: Updates namespace attributes. https://developers.egnyte.com/docs/Metadata_API#Update-Namespace-Attributes
* `Update namespace keys`: Update the key of a custom metadata for a domain. https://developers.egnyte.com/docs/Metadata_API#Update-Namespace-Keys
* `Get namespace`: List all custom metadata keys that have been created in a given namespace. https://developers.egnyte.com/docs/Metadata_API#Get-Namespace
* `Delete namespace`: Delete a specified namespace. https://developers.egnyte.com/docs/Metadata_API#Delete-Namespace
* `Get all projects`: List all project folders in the domain. https://developers.egnyte.com/docs/read/Project_Folder_API#Get-All-Projects
* `Mark folder as project`: Mark an existing folder as a project. https://developers.egnyte.com/docs/read/Project_Folder_API#Mark-Folder-as-Project
* `Create project from template`: Create a new project from a project folder template. https://developers.egnyte.com/docs/read/Project_Folder_API#Create-From-Template
* `Get project by ID`: Retrieve a project using ID. https://developers.egnyte.com/docs/read/Project_Folder_API#Find-Project-by-ID
* `Update project by ID`: Updates specified properties of a project identified by its ID. https://developers.egnyte.com/docs/read/Project_Folder_API#Update-Project
* `Delete project by ID`: Remove a project based on its Id. https://developers.egnyte.com/docs/read/Project_Folder_API#Remove-Project
* `Get project by root folder ID`: Retrieve a project based on its root folder ID. https://developers.egnyte.com/docs/read/Project_Folder_API#Find-Project-by-Root-Folder
* `Clean up project by ID`: This endpoint initiates a cleanup in a project, and optionally delete or disable specified users. https://developers.egnyte.com/docs/read/Project_Folder_API#Cleanup%20Project 
* `Create metadata key`: Add a metadata key to an existing namespace. https://developers.egnyte.com/docs/Metadata_API#Create-Metadata-Key
* `Delete metadata key`: Delete a metadata key from an existing namespace. https://developers.egnyte.com/docs/Metadata_API#Delete-Metadata-Key
* `Get metadata by file ID`: Get metadata from a file by using file ID. https://developers.egnyte.com/docs/Metadata_API#Get-Values-for-a-Namespace
* `Get metadata by folder ID`: Get metadata from a folder by using folder ID. https://developers.egnyte.com/docs/Metadata_API#Get-Values-for-a-Namespace
* `Search metadata`: Find items with a specific metadata field or value. https://developers.egnyte.com/docs/Metadata_API#Search-Metadata
* `Get effective permissions`: Get effective permissions for a user. https://developers.egnyte.com/docs/read/Egnyte_Permissions_API#Get-Effective-Permission-for-a-User
* `Set folder permissions`: Set permissions for a folder. https://developers.egnyte.com/docs/read/Egnyte_Permissions_API#Set-Folder-Permissions
* `Get folder permissions`: Get permissions for a folder. https://developers.egnyte.com/docs/read/Egnyte_Permissions_API#Get-Folder-Permissions
* `Deep link by ID`: Create a deep link for file or folder by ID. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#Deep-Links-to-Files-and-Folders
* `Deep link by path`: Create a deep link for file or folder by path. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#Deep-Links-to-Files-and-Folders
* `List all links`: Lists all links except for deleted links. Please note, that if the user executing this method is not an admin, then only links created by the user will be listed. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#List-Links
* `List all links V2`: Lists all links. Please note, that if the user executing this method is not an admin, then only links created by the user will be listed. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#List-Links%20v2
* `Show link details`: View the details of a link. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#Show-Link-Details
* `Create link`: Create a link. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#Create-a-Link
* `Delete link`: Delete a link. https://developers.egnyte.com/docs/read/Egnyte_Link_API_Documentation#Delete-a-Link



## Supported Triggers

The connector supports the following triggers:

* `When a file is locked`: This operation triggers a flow when a file is locked
* `When a file is unlocked`: This operation triggers a flow when a file is unlocked
* `When a file is updated`: This operation triggers a flow when a file is updated in specific Folder
* `When a file is created v2`: This operation triggers a flow when a file creates in specific Folder
* `When a share link is created`: This operation triggers a flow when a share link is created
* `When a share link is deleted`: This operation triggers a flow when a share link is deleted
* `When the file or folder permission changes`: This operation triggers a flow when a file or a folder permissions changes
* `When the file or folder metadata changes`: This operation triggers a flow when a file or a folder metadata changes
* `When a project folder is created`: This operation triggers a flow when a project folder is created or when an existing folder is marked as a project folder
* `When a folder is unmarked as a project`: This operation triggers a flow when a folder is unmarked as a project
* `When a project is updated`: This operation triggers a flow when a project is updated
* `When a workflow is created`: This operation triggers a flow when a workflow is created
* `When a workflow is completed`: This operation triggers a flow when a workflow is completed
* `When an approval type workflow is approved`: This operation triggers a flow when an approval type workflow is approved
* `When an approval type workflow is rejected`: This operation triggers a flow when an approval type workflow is rejected
* `When a group is created`: This operation triggers a flow when a group is created
* `When a group is updated`: This operation triggers a flow when a group is updated
* `When a group is deleted`: This operation triggers a flow when a group is deleted
* `When a file is created (properties)`: This operation triggers a flow when a new file is created in a folder.
* `When a folder is created (properties)`: This operation triggers a flow when a sub-folder is created in a folder.
* `When a file is deleted (properties)`: This operation triggers a flow when a file is deleted in a folder.
* `When a folder is deleted (properties)`: This operation triggers a flow when a sub-folder is deleted in a folder.
* `When a file is renamed (properties)`: This operation triggers a flow when a file is renamed in a folder.
* `When a folder is renamed (properties)`: This operation triggers a flow when a sub-folder is renamed in a folder.
* `When a file is moved (properties)`: This operation triggers a flow when a file is moved in a folder.
* `When a folder is moved (properties)`: This operation triggers a flow when a sub-folder is moved in a folder.
* `When a file is copied (properties)`: This operation triggers a flow when a file is copied in a folder.
* `When a folder is copied (properties)`: This operation triggers a flow when a sub-folder is copied in a folder.