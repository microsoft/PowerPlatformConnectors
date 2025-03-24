# eformity

The eformity connector allows you to automate the way you create and send your documents from your eformity.net environment. You could retrieve variables from an external scource, pass those variables to the document generator to be used inside your document, and then send the complete document with dynamic data via an email for example.


## Publisher

eformity document solutions b.v.
https://eformity.nl


## Prerequisites

To be able to use the eformity connector, you will need:

- an eformity.net subscription.
- an eformity API key.


## How to get an eformity.net subscription

If you want to learn more about us, what we do, and what we can help you with, please contact us at [support@eformity.nl](mailto:support@eformity.nl).


## How to retrieve an API key

To retrieve an API key, go to your eformity environment (<https://[subscription].eformity.net>) and follow these steps:

1. Go to System Management > Users
2. Select and open a user
3. Under API keys, click the Edit-button
4. In the API key overview, click the Add-button
5. Enter a name and also *ensure* `Active` is enabled
6. Click the Create-button
7. Copy the API key


## Supported Operations

The eformity connector supports the following operations:

##### `Create a document:`  Start a task for creating a document
This operation starts a task for creating a document on the eformity.net server. A task ID will be returned. When the task is finished, you can use the task ID to download the document.

##### `Retrieve task status:`  Retrieve the current status of a task
This operation retrieves the current status of a task, and tells you if the task has been completed, is still busy or if the task had failed to complete.

##### `Download a document:`  Download a document from a completed task
This operation downloads a document using the task ID retrieved from the Start task operation.


## Known Issues and Limitations

If you encounter an issue with the eformity connector, please contact us at [support@eformity.nl](mailto:support@eformity.nl).


## Frequently Asked Questions

##### How do I download a document?
To download a document, you first have to create and complete a `Create a document`-operation. This operation will return a task ID. With this ID, you can perform a Download document task. This operation will return an object with your document in Base64 format.

##### How do I insert variables in the document I want to generate?
To insert variables in your document, you specify them when creating a `Create a document`-operation. Variables are passed in XML format. Here is an example body on how to pass a variable named 'projectName' with the value 'BlueOrange':

`<variables><projectName>BlueOrange</projectName></variables>`
