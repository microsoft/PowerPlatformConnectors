# Huddle
Huddle provides document collaboration & client engagement portals for Enterprise and Government clients and partners. This connector helps to automate your workflows for document storage, retrieval, task assignment, and customized notifications. 


## Pre-requisites
You will need the following to proceed:
* A [Huddle](https://huddle.com) account
* A Client ID for your OAuth App provided by Huddle Support
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power platform CLI tools

## Supported Operations
The connector supports the following operations:
* ```Poll a folder for its most recent upload```: Triggers whenever a new file is added to a specified folder.
* ```Poll a workspace for new approval requests```: Triggers whenever a new approval request is added to a specified Workspace.
* ```Upload a file```: Uploads a file to the target folder. Includes automatic file type detection, with a broad set of types supported.
* ```Upload a text file```: Accepts raw input and adds to your account as a text file.
* ```Get file contents```: Retrieves a file by ID.
* ```Delete a folder```: Deletes a folder by ID.
* ```Create a new folder```: Creates a new folder or subfolder in the target folder.
* ```Create a task/To-Do```: Creates a task/to-do in the target workspace.
* ```Mark task as complete```: Marks a task as "Complete" by ID.


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps


## Further Support
For further support, checkout our [support documentation](https://huddle.zendesk.com/hc/en-us) or [contact us](https://www.huddle.com/about/contact/)
