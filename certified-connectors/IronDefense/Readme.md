
## IronDefense Connector
IronNet provides an external REST API for its flagship product, IronDefense. The
API allows users to query and modify alerts as well as ingest alert, event, and
IronDome notifications.



## Prerequisites
You will need the following to proceed:
* The URL where the IronDefense deployment is hosted
* User login credentials for IronVue with the permissions `Access IronAPI`, `View
  Alert`, `Edit Alert`, and `Manage Threat Intelligence Rules`


## Supported Operations
The connector supports the following operations:
* `CommentOnAlert`: Allows a client to comment on any given alert, with the option to send to IronDome (if enrolled).
* `GetAlertIronDomeInformation`: Allows a client to retrieve community IronDome correlation information for an alert. 
* `GetAlertNotifications`: Allows a client to retrieve alert notifications from IronDefense without pulling duplicate messages that have already been ingested.
* `GetAlerts`: Allows a client to retrieve IronDefense alerts in an environment. The response can be filtered based on the alert field parameters and limited to a given number of alerts. 
* `GetDomeNotifications`: Allows a client to retrieve dome notifications from IronDefense without pulling duplicate messages that have already been ingested.
* `GetEvent`: Allows a client to retrieve details for an IronDefense event including the event context.
* `GetEventNotifications`: Allows a client to retrieve event notifications from IronDefense without pulling duplicate messages that have already been ingested.
* `GetEvents`: Allows a client to retrieve IronDefense events for a particular IronDefense alert. Event context information is not included in these event objects.
* `Login`: Allows a client to login and retrieve a valid JSON Web Token (JWT) to use in subsequent calls.
* `RateAlert`: Allows a client to rate an alert as part of the review/triage process.
* `ReportObservedBadActivity`: Allows a client to submit a domain and/or IP of observed bad activity for Threat Intelligence Rule, event/alert creation and IronDome correlation.
* `SetAlertStatus`: Allows a client to change an alert's status to progress it through the review process.
* `UpdateEntityRecord`: Allows a client to send IP lease information from a source of entity information.

