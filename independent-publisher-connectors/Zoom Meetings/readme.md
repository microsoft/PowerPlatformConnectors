# Zoom Connector
Zoom is widely used for Virtual Meetings. Using this Connector you can Create Meetings, Get Meetings and Meeting Details (Individually). In some scenarios we want to perform certain actions Ex: Add Meetings into Excel and many more. In this process Zoom Connector will gets the work done.

## Prerequisites
You will need the following to proceed:
•	A Microsoft Power Apps or Power Automate plan with custom connector feature
•	A Zoom Account

## Supported Operations
Currently there are two actions:
* `Create Meeting` : This Action will help to create Meeting with few inputs required for the Meeting. You will get a Response which contains all the Meeting Details Ex: Meeting URL, Meeting ID
* `Get Meetings` : This Action will get the Meetings that you have in Zoom Platform. The Response you will get will contain all the Meeting details for every Meeting that you created.
* `Meeting Details` : This Action will help the get the details for a specific Meeting. The Response will contain all the Meeting details that you requested. 

## The API Documentation
This is the official documentation for Zoom API: https://marketplace.zoom.us/docs/api-reference/introduction

## Building a Connector
Building Connector requires the following:
* Login to Zoom Marketplace https://marketplace.zoom.us/
* Create an App.
* Note down the Client Id and Client Secret.
* Add the Redirect URL in the Zoom Marketplace Website.

## Known Issues and Limitations
No issues and limitations are known at this time
