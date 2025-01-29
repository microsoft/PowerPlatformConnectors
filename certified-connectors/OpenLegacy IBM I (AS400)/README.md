# OpenLegacy IBM i (AS400)
Build Flows that leverage your IBM i and other legacy systems. The OpenLegacy IBM i (AS400) connector provides bidirectional connectivity to your system’s data and business logic. You can now build Business Flows with information from your legacy environment through the OpenLegacy connector. This will enable you to utilize your legacy assets as digital components, providing you a Legacy Integration and Modernization Solution from Day 1. All OpenLegacy connectors provide access to various legacy systems through APIs deployed on OpenLegacy’s cloud or any other target deployment architecture you prefer- fully flexible, any on-prem to any cloud or digital platform.
## Publisher: OpenLegacy Technologies Inc. ​

## Prerequisites
* An API to your AS400 that is deployed on OpenLegacy Cloud or any other target.
* OpenLegacy Hub account.
* OpenLegacy Hub API keys.

## Supported Operations
The following operations were defined for the connector:
### GetAllProjects
(Internal) Gets a list of user's projects with contracts from the OpenLegacy Hub.
### GetMethodsForContract
(Internal) Gets a list of methods defined in the selected project.
### GetMethodOpenApiSpec
(Internal) Gets a Swagger 2 specification for the selected method.
### AS400Cobol
Performs an HTTP request to the user's API. Main logic is controlled by C# script.
During design-time displaying a list of projects filtered for this action only.
### AS400Rpg
Performs an HTTP request to the user's API. Main logic is controlled by C# script.
During design-time displaying a list of projects filtered for this action only.
### AS400DataQueue
Performs an HTTP request to the user's API. Main logic is controlled by C# script.
During design-time displaying a list of projects filtered for this action only.
### AS400Db2Queries
Performs an HTTP request to the user's API. Main logic is controlled by C# script.
During design-time displaying a list of projects filtered for this action only.
### AS400Db2Executables
Performs an HTTP request to the user's API. Main logic is controlled by C# script.
During design-time displaying a list of projects filtered for this action only.

## Obtaining Credentials
Register to OpenLegacy Hub at https://hub.openlegacy.com.
To find your API Keys go to upper right-side menu, click on API Keys and copy the Account API Key and the Run Time API Key.

## Getting Started
OpenLegacy has prepared a demo AS400 project for you to use that will connect to our Demo Environment.
To meet your specific project requirements, an OpenLegacy team will build and manage the API to your AS400 according to your requirements.
Register to OpenLegacy Hub. One of our Customer Success representatives will contact you for any questions.
Follow the instructions below to import the AS400 demo project. Click on Projects->Import from Solution Center.
In the Filter Data Field type “IBMi” and select IBMi COBOL Insurance Policies Demo. The Demo project will be imported.
Follow the instructions provided in this [video](https://youtu.be/u6VaH4yHO7E).
Contact OpenLegacy at https://www.openlegacy.com/company/support for any question.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.