# WorkMobile

WorkMobile is an award-winning No Code toolkit that allows businesses to deploy end-to-end field based mobile solutions. It allows customers to build bespoke forms and business processes in minutes. This connector allows you to connect your WorkMobile account and send data to and from other applications supported by PowerAutomate.

## Publisher: eSAY Solutions Ltd

The WorkMobile Connector allows you to integrate with the data in your WorkMobile account. 

## Prerequisites
You will need the following to proceed:

* A valid WorkMobile account 
* A current APIToken 

You should be familiar with WorkMobile and it's concepts. More information may be found here [WorkMobile](https://www.workmobilesolutions.com/)

## Setting up the connector
Once you have created an API token you will be able to connect your WorkMobile account to Power Automate

## Supported Operations

### Actions
* ```Create a Job```: Raise a new job, including specifying user-defined attributes
* ```Allocate a Job```: Assign a previously raised job to a new user
* ```Unallocate the Job```: Unassign a job from the currently assigned user
* ```Job Broadcast```: Send the job to a group of users for any to choose to do
* ```Retrieve a list of forms```: Get a list of all the forms that you login has access to
* ```Retrieve a report in a specified format```: Get the report for a row in a specific form. PDF and Office formats are available 
* ```Retrieve Mobile Users```: Get a list of mobile users visible to your login
* ```Create a new Mobile User```: Create a new user to collect data
* ```Update the attributes associated with the mobile user```: Update the users details
* ```User groups```: Get a list of user groups

### Triggers
* ```When dats is submitted for a form```: Notify your PowerAutomate flow with data when new records arrive in a form

## Obtaining Credentials
Credentials may be obtained for signing up to a WorkMobile account here - https://www.workmobilesolutions.com/. Once an account is activated you can create an API token using the following steps

* Click add a new portal user
* Enter a username and other details as required (we suggest PowerAutomateAPIUser or similar)
* Add an email address - this isn't validated - you may use PowerautomateAPIUser@yourdomain.com
* Dont forget to click the active toggle on the top right of the Login Edit panel
* Click Save
* From the list find the user you have just created and click to edit the user
* In the Security details section - you will find an API Token heading.
* Click Generate, fill in the Description box and click update. Press OK to confirm each step
* Click copy at the end of the API Token box.

Use this Token key in any of your connectors. 

## Known Issues & Limitation
No known issues.

In the event of any problems please contact wmsupport@workmobileforms.com or visit https://helpcentre.esayworkmobile.co.uk/en

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps