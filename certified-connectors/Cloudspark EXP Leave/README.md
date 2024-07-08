# Cloudspark Labs Leave
The Cloudspark Labs Leave Connector provides users with the ability to generate Adaptive Cards to send via Email or Microsoft Teams. The Cards can be used to Approve and Reject time off requests made in the Cloudspark Leave App. 

## Publisher: Cloudspark Technologies

## Prerequisites
In order to use this connector, you will need to have an active version of the Cloudspark MS Teams Leave application. For more info, please get in touch with [support@cloudsparklabs.com](support@cloudsparklabs.com).

## Supported Operations
* `Approve a Leave Request`: Approve a time off request submitted by one of your users.
* `Reject a Leave Request`: Reject a time off request submitted by one of your users.
* `Create an Email Request ACE`: Create an adaptive card detailing a time off request to be sent via email.
* `Create a Teams Request ACE`: Create an adaptive card detailing a time off request to be sent via Microsoft Teams.
* `Create a Summary ACE`: Once a time off request has been approved/rejected, create an adaptive card with a summary of the request to be sent via email to the original requester.

It also supports the following trigger:

* `When a Leave request is submitted`: When a user submits a time off request, the connector triggers the workflow and passes in information about the request made.  


## Authentication and Credentials

This connector uses `API Key` authentication. When creating a new connector in a Logic App, a customer will be required to provide an API Key. To obtain an API Key, please send us an [email](support@cloudsparklabs.com) and we will verify that you are an active customer of the Leave app.

## Known Issues and Limitations
As this connector is exclusive to the Cloudspark Leave application, you will not be able to use it unless you are registered with Cloudspark Labs. Please [get in touch](support@cloudsparklabs.com) if you are having trouble using the connector.  

## Deployment Instructions
Install the connector from the logic apps connector store, and proceed to add the Leave Submitted Trigger into your logic app workflow. Create a new connection with the API Key provided to you, and then enter your TenantID. Once the the trigger is set up, you can configure and customize the workflow using the rest of the connector's actions! 