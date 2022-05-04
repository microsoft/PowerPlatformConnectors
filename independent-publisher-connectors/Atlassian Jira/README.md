# Jira Community Connector
Increase the connectivity between the Power Platform and Atlassian Jira with added actions to manage, create and delete items using the JIRA v3 API. [Atlassian Jira API](https://developer.atlassian.com/cloud/jira/platform/rest/v3/intro/)


## Pre-requisites
To use this connector, you need the following
1. A Microsoft Power Apps or Power Automate plan with custom connector feature
2. A cloud Jira instance (e.g. instance.atlassian.net )
3. Your user email address and an API key with the correct piveledges  
4. Ensure that you have the correct permissions on JIRA for the appropriate actions you want to use. For example, if you want to depete a project, when generating your API key, you need to ensure that your account has the Jira permissions to do this.

###### How to get an API key
* [If you do not have a Jira instance, sign up here](https://www.atlassian.com/software/jira).
* [Log in to the manage profile section](https://id.atlassian.com/manage-profile/security/api-tokens) with your account details and click 'Create API Token'. 

## API documentation
[Jira Rest API Documentation](https://developer.atlassian.com/cloud/jira/platform/rest/v3/intro/)

## Supported Operations
The connector supports the following operations:
* [Issues](https://developer.atlassian.com/cloud/jira/platform/rest/v3/api-group-issues/#api-group-issues)
	- Edit Issue

* [Projects](https://developer.atlassian.com/cloud/jira/platform/rest/v3/api-group-projects/#api-group-projects) 
	- Delete Project
	- Update Project

* [Project Categories](https://developer.atlassian.com/cloud/jira/platform/rest/v3/api-group-project-categories/#api-group-project-categories)
	- Get All Project Categories
	- Create Project Category
	- Remove Project Category

* [Tasks](https://developer.atlassian.com/cloud/jira/platform/rest/v3/api-group-tasks/#api-group-tasks) 
	- Cancel Task
	- Get Task

* [Users](https://developer.atlassian.com/cloud/jira/platform/rest/v3/api-group-users/#api-group-users) 
	- Get User
