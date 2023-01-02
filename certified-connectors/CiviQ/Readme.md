# CiviQ

## Description
The CIVIQ connector allows the connection and integration with the CIVIQ OpenConsult platform. 
The CIVIQ OpenConsult is an online tool that allows the users to create public consultation surveys, targeted for cities and communities.

Consultations, Submissions and Surveys can be searched for based on some criteria or directly using a Consultation ID which uniquely identifies an consultation in the CIVIQ system. 
When using the consultation ID the entire data will be returned for that single object.
The API will return data that match the search criteria.

CiviQ API is part of the corporate data set and requires a corporate data set API key for access.


## Pre-Requisites
The access to the backend API is protected through an authorization key (api_key), and it's used to integrate data from diferent data model 
objects (Consultations, Submissions, Surveys, etc...)
API key could be taken from API configuration page (admin/config/civiq/api)


### Deploying the Connector
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```


## Supported Operations
The connector supports the following operations:
* `Get consultations`: Get consultations based on some criterias.
* `Get consultation by ID`: Get a specified consultation based on ID.
* `Get consultation submissions by ID`: Get a list of consultation submissions based on consultation ID.
* `Get surveys`: Get surveys based on some criterias.
* `Get survey by ID`: Get a specified survey based on ID.
* `Get survey submissions by ID`: Get a list of survey submissions based on survey ID.
* `Get applications`: Get applications based on some criterias.
* `Get application by ID`: Get a specified application based on ID.
* `Get application submissions by ID`: Get a list of application submissions based on application ID.
* `Get files by UUID`: Get a list of files based on UUID.


## Supported Triggers
The connector supports the following triggers:
* `Submission Submitted`: Event triggers when consultation submissions are submitted.
* `Submission Updated`: Event triggers when consultation submissions are updated.
* `Submission Approved`: Event triggers when consultation submissions are approved.
* `Consultation Opened`: Event triggers when consultation is opened.
* `Consultation Closed`: Event triggers when consultation is closed.
* `Webform Submission Submitted`: Event triggers when a user submits a submission in a webform.
* `Webform Opened`: Event triggers when a webform is opened.
* `Webform Closed`: Event triggers when a webform is closed.
* `Delete webhook`: Event triggers to delete a webhook.