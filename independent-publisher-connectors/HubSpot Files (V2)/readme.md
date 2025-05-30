# HubSpot Files V2

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must have an account with [HubSpot](https://app.hubspot.com/signup-hubspot/crm) and be a Super Admin.

## Obtaining Credentials
Once you are logged in to your account, go to Settings -> Account Setup -> Integrations -> Private Apps. You will need to create a private app and assign it only the scopes you will use actions for. Once your private app is created, you will have an access token in the Auth section of the private app.

## Supported Operations
### Delete file for GDRP
Deletes a file for GDRP.
### Get file
Retrieve a file by ID.
### Delete file
Delete file by ID.
### Update file
Replaces existing file data with new file data. Can be used to change image content without having to upload a new file and update all references.
### Update file properties
Update properties of file by ID.
### Upload file
Upload a single file with content specified in request body.
### Generate link
Generates signed URL that allows temporary access to a private file.
### Import file asynchronously
Asynchronously imports the file at the given URL into the file manager.
### Get file by path
Retrieve the file by the path address.
### Search files
Search through files in the file manager. Does not display hidden or archived files.
### Check import status
Check the status of requested import.
### Get folder
Get folder by ID.
### Delete folder
Delete folder by ID.
### Create folder
Creates a folder.
### Search folders
Search for folders. Does not contain hidden or archived folders.
### Check folder update
Check status of folder update. Folder updates happen asynchronously.
### Update folder properties
Update properties of folder by given ID. This action happens asynchronously and will update all of the folder's children as well.

## Known Issues and Limitations
There are no known issues at this time.
