Spring Global is a leading provider of field sales tools for the Consumer Packaged Goods (CPG) industry. We enable users to serve with ease, monitor & control field execution, and leverage a single app with a 360° of the customer. We deliver a proven, flexible, enterprise-grade SaaS solution and our development team is continuously working on innovations to improve and expand the capabilities of our full-stack solution.
Current vertion of connector contains a trigger to handle surver execution events and methods get relevant information about survey execution and users

## Prerequisites

To use the Spring Global connector, you need to have trial or full license for Spring Global Survey. So, you must have portal URL  and credentials (login and password) to create and configure a survey.
To configure the connector you should have Web API URL and API Key.

## How to get credentials
Contact our support team (sustaining.br@springglobal.com) for getting trial license and additional support.

## Getting started with your connector
1. Create a survey in the Spring Global Survey portal. Publish it.
2. Create a Power Automeate Flow
   * The flow starts with trigger "On Survey Execution" of Spring Global connector 
     Use WebAPI Url and API key to configure connection
   * Use "Get Survey Execution Data" to get details by survey execution id
3. Fill the survey as a user
4. Look the flow execution

## Supported Operations
The current version of connector supports the following operations:
* webhook trigger that fire on new surver execution
* method to get survey execution details
* method to get user details 

## Deployment instructions  
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
Note: the connector contains code (script.csx file), so use --script (or -x) key to upload script file as well.