# About

Asite is an open construction platform that enables organizations with comprehensive range of solutions connect dispersed teams across the lifecycle of capital assets by collaborate, plan, design, and build with seamless information sharing across the entire supply chain which helps capital project owners stay at the forefront of innovation, maintaining a golden thread of information.

The Asite connector helps to build a connection between two systems for file exchange. The connector collaborates both systems by uploading and downloading files based on triggers.

# Pre-requisites

- An active Asite CDE Subscription
- An active Microsoft Power Automate subscription
- An active workflow configuration in Asite platform, configured workflow trigger with the type "Microsoft Flow"

# Actions

Following are the internal and external actions used during connection flow to share a file

| **Name** | **Trigger / Action Name** | **API EndPoint** | **Description** | **Visibility** |
| -------- | ------------------------- | ---------------- | --------------- | -------------- |
| Select Project Name || /workspaceList | It will list out all the Asite's projects where you have access. | Internal |
| Select Folder Name || /folderAndFileList | It will list out all the accessible folders based on your access from the selected Project. | Internal |
| When an Asite workflow event is triggered | Trigger | /asitePullDataWebhook | To upload file with metadata and download file on defined folder based on triggered workflow. | Important |
| Get file content | Action | /downloadFileByUrl | Retrieves the file content from Asite | Important |
| List of configured Triggers from Asite Platform | | /triggerList | To display list of configured Triggers from Asite Platform | Internal |
| Delete configured trigger || /deleteAsitePullDataWebhook/{id} | To Delete configured trigger | Internal |
| Get Dynamic Schema based on project and folder | | /getFolderAttributes | Get Dynamic Schema based on project and folder | Internal |
| Set file metadata | Action | /saveMetadataForUpload | Retrieves standard and custom metadata | Important |
| Create file | Action | /uploadFileFromExternalSystem | Upload a file in Asite project folder. | Important |
| Get dynamic schema based on project Id and trigger Id || /getFolderAttributesFromTrigger | Get dynamic schema based on project Id and trigger Id | Internal |
| Select Folder Name || /getSubFolderList | List out all the accessible folders based on your access from the above selected Project. | Internal |

# Reference Link:

Please refer help document for authorize connector and configure flow. [Click here](https://adoddleqa2ak.asite.com/adoddle%20online%20help/Asite_Integration_via_Microsoft_Power_Automate.htm)