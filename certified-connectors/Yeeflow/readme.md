

## Yeeflow 
Yeeflow is a no-code application development platform that empowers you to build enterprise-class applications that run on mobile, tablet, and web. Create custom forms, configure workflows, build informative dashboard, and get your app up and running in minutes.

Yeeflow connector allows you to access and operate your data in Yeeflow.  And you can be notified when an item has been created, changed, deleted in Yeeflow and then making appropriate actions in other systems.


## Pre-requisites
First of all, you need to have an account in [Yeeflow](https://www.yeeflow.com/).


## API documentation
The API documentation can be found here [https://developer.yeeflow.com/api/](https://developer.yeeflow.com/api/)


## Supported Operations

 - **Add an item to a list:** This action will create an item in selected list.
 - **Get an item by ID:** This action can be used to get an item of selected list by item ID.
 - **Update an item by ID:** This action will update an item in selected list.
 - **Delete an item:** This action will delete an item in selected list.
 - **Get fields of a list:** This action will get the field definitions of a list.
 - **Query items:** This action can be used to query items of selected list.  The detailed description of the API can be found here [Query list items](https://developer.yeeflow.com/api/#tag/Lists/paths/~1lists~1{appID}~1{listID}~1items~1query/post)
 - **Add file to item:** Adds a new file to a field of the specified list item.
 - **Add file to library:** Uploads a file to a library.
 - **Upload file:** Upload temporary file. The returned file object can be used to create a list item, initiate a process, and submit it to the agent. (Temporary files that are not referenced will be removed after a certain period of time.)
 - **Get file properties:** Get file properties array from a file field.
 - **Get file content:** Get file content (Bytes) by file id.
 - **Get library file:** Get file content (Bytes) from library.
 - **Get definition of an Agent:** Get the definition of an AI agent by agent ID.
 - **Run an Agent:** Execute an AI agent by Agent ID and required input parameters.
 - **Start a workflow:** Start a workflow by key and variables.  ApplicantID is optional and is used when submitting an application on behalf of a specified user.

 ## Supported Triggers
 - **When an item is created:** Trigger when an item is created in selected list.
 - **When an item is created or modified:** Trigger when an item is created or modified in selected list.
 - **When an item is deleted:** Trigger when an item is deleted in selected list.
 - **When an item is modified:** Trigger when an item is modified in selected list.