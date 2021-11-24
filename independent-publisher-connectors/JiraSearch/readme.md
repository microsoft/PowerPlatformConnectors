# Jira Search Api
Jira Software is part of a family of products designed to help teams of all types manage work. Originally, Jira was designed as a bug and issue tracker. But today, Jira has evolved into a powerful work management tool for all kinds of use cases, from requirements and test case management to agile software development. This connector provides access to the v2 Jira issue search API which is not exposed using the built-in JIRA connector.  

## Publisher: Paul Culmsee

## Prerequisites
This connector uses Basic Auth and the account requires sufficient permission to access the projects/issues within the scope of search. See Permissions overview for details (https://support.atlassian.com/jira-core-cloud/docs/how-do-jira-permissions-work/) 

## Supported Operations

### Searches for issues using JQL
Search Jira using JQL language

## Obtaining Credentials
This connector uses basic auth. Contact your JIRA administrator to get a suitable account set up. ​

## Known Issues and Limitations
Pay attention to the hostname parameter. You just specify the name of your Jira instance (ie mycompany). You do NOT use a fully qualified domain name (eg mycompany.atlassian.net) 

This connector has only been tested and used for Jira Cloud. It is not designed to be used for on-premises deployments

All listed issues with the Microsoft supplied Jira connector apply. Consult the Known Issues section at https://docs.microsoft.com/en-us/connectors/jira/

## Frequently Asked Questions

## Deployment Instructions
After downloading the connector, edit the client ID and tenant ID in apiProperties.json to your app registration and tenant. 
Please use paconn CLI to deploy.
