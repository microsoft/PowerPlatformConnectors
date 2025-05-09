# Zoom Connector
Zoom is widely used for Virtual Meetings. Using this Connector you can Create Meetings, Get Meetings and Fetch Meeting Details (Individually). In some scenarios we want to perform certain actions Ex: Add Meetings into Excel. For this process Zoom Meetings Connector will gets the work done.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Zoom Account

## Supported Operations
Currently there are three actions:
* `Create Meeting` : This Action will help to create Meeting with few inputs required for the Meeting. You will get a Response which contains all the Meeting Details Ex: Meeting URL, Meeting ID
* `Get Meetings` : This Action will get the Meetings that you have in Zoom Platform. The Response you will get will contain all the Meeting details for every Meeting that you created.
* `Meeting Details` : This Action will help the get the details for a specific Meeting. The Response will contain all the Meeting details that you requested. 

## The API Documentation
This is the official documentation for Zoom API: https://marketplace.zoom.us/docs/api-reference/introduction

## Building a Connector
<b>Creating App:</b>
* Go to Zoom Developer Portal https://marketplace.zoom.us/.
* Sign In with your Account.
* Then to Develop Tab and Build App.
* Choose App as OAuth to create.
* Enter Name and select Account-level app. Turn off the toggle of publishing your app.

<b>Getting Credentials:</b>
* In "My Dashboard" go to "Created Apps".
* Click on App Name.
* Copy Client ID and Client Secret and Add this in the Security tab of the Connector.
* In "Redirect URL for OAuth" https://us.flow.microsoft.com/en-us/.
* In "Add Allow List" insert https://us.flow.microsoft.com/en-us/ and https://global.consent.azure-apim.net/redirect.
* In Information Tab Add Contact Details, Short Description and Long Description.
* `Note` : Scopes in Marketplace and Security Tab must be Same.
* The App uses admin read and write. So you need the following scope to be included meeting:write:admin, meeting:read:admin, meeting:master.
* `Note` : There is small issue while creating a connection in https://us.flow.microsoft.com/en-us/.
* There is an Error Message "Invalid Scope". 
* For fixing this Simply Sign Out(https://1drv.ms/u/s!An7eWraK_9KraqxPrDbgQUQf_hw?e=UhHW1B) Zoom Account and Sign In Again.
* That's it you are ready to go.




## Known Issues and Limitations
No issues and limitations are known at this time
