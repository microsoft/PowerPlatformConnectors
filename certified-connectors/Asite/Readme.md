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
| When a workflow is triggered on file(s) | Trigger | /asitePullDataWebhook | This operation triggers a flow when a file is uploaded/updated on the project. The trigger is fired to include sub-folders based on the workflow configuration. | Important |
| Get file content | Action | /downloadFileByUrl | Retrieves the file content from Asite | Important |
| List of configured Triggers from Asite Platform | | /triggerList | To display list of configured Triggers from Asite Platform | Internal |
| Delete configured trigger || /deleteAsitePullDataWebhook/{id}/{nameOfClass} | To Delete configured trigger | Internal |
| Get Dynamic Schema based on project and folder | | /getFolderAttributes | Get Dynamic Schema based on project and folder | Internal |
| Set file metadata | Action | /saveMetadataForUpload | Retrieves standard and custom metadata | Important |
| Create file | Action | /uploadFileFromExternalSystem | Upload a file in Asite project folder. | Important |
| Get dynamic schema based on project Id and trigger Id || /getFolderAttributesFromTrigger | Get dynamic schema based on project Id and trigger Id | Internal |
| Select Folder Name || /getSubFolderList | List out all the accessible folders based on your access from the above selected Project. | Internal |
| When a workflow is triggered on App(s) | Trigger | /asitePullAppFormDataWebhook | This operation triggers a flow when a form is created/updated on the project. Configure separate flows for each App. | Important |
| Get dynamic schema based on project Id and trigger Id | Action | /getFormAttributesFromTrigger | Get dynamic schema based on project Id and trigger Id | Internal |

# Reference Link:

Please refer help document for authorize connector and configure flow. [Click here](https://adoddleqa2ak.asite.com/adoddle%20online%20help/Asite_Integration_via_Microsoft_Power_Automate.htm)