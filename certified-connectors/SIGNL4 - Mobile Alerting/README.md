# SIGNL4: Mobile Alerting and Incident Response

## Publisher: Derdack

When critical systems fail or incidents happen, SIGNL4 bridges the ‘last mile’ to your staff, engineers, IT admins and workers ‘in the field’. It adds real-time mobile alerting to your services, systems, and processes in no time.
SIGNL4 notifies through persistent mobile push, SMS text and voice calls with acknowledgement, tracking and escalation. Integrated duty and shift scheduling ensure the right people are alerted at the right time.
Through convenient interfaces like email and webhooks, SIGNL4 adds mobile alerting and incident response capabilities to Power Automate, IT, IoT and other technical systems.
SIGNL4 thus provides for an up to 10x faster and effective response to critical alerts, major incidents, and urgent service requests.

## Prerequisites

You need an SIGNL4 account. If you no not already have one you can get one at [https://connect.signl4.com/](https://connect.signl4.com/). You can also directly download the mobile app from the [Google Play Store](https://play.google.com/store/apps/details?id=com.derdack.signl4) or from the [Apple App Store](https://itunes.apple.com/us/app/signl4/id1100283480).

## Supported Operations

The SIGNL4 connector for Power Automate encapsulates the [SIGNL4 REST API](https://connect.signl4.com/api/docs/index.html?urls.primaryName=SIGNL4%20API%20V2).
It provides the following functionalities:

- Send alerts to SIGNL4 teams
- Acknowledge, close and annotate alerts
- Set users on-duty and off-duty
- Manage duty schedules
- Manage users
- Manage categories
- And more ...

### AlertsTrigger

Triggers a new alert for your team. All team members on duty will receive alert notifications.

### AlertsAcknowledgeAll

This method confirms all unhandled alerts your team currently has by a specific user.

### AlertsAcknowledgeMultiple

This method confirms all alerts provided.

### AlertsCloseAll

This method closes all acknowledged alerts your team currently has.

### AlertsCloseMultiple

This method closes all alerts provided.

### AlertsGetPaged

Gets alerts paged.

### AlertsGetReport

Returns information about the occurred alert volume in different time periods as well as information about the response behaviour (amount of confirmed and unhandled alerts) of the team members.

### AlertsUndoAcknowledgeMultiple

This method tries to undo the acknowledgement of multiple alerts via a queue. The operation is handled in the background.

### AlertsUndoCloseMultiple

This method tries to undo multiple alert closes. The operation is handled in the background.

### AlertsGet

Gets an alert by id.

### AlertsAcknowledge

Acknowledge an alert.

### AlertsAnnotate

Annotates an alert by given Annotation Info.

### AlertsGetAttachmentInfo

Get attachments of an alert by id.

### AlertsGetAttachment

Gets a specified attachment of a specified alert.

### AlertsClose

Close an alert.

### AlertsGetDetails

Get details about an alert.

### AlertsEscalateToTeam

Escalate an alert manually to a team.

### AlertsGetNotifications

Get notifications of all users by alert id.

### AlertsGetTimeline

Get alert timeline entries of an alert by id.

### AlertsUndoAck

This method tries to undo an alert acknowledgement.

### AlertsUndoClose

This method tries to undo an alert close.

### CalloutsGetTemplates

Get all callout templates for your subscription.

### CalloutsCreateTemplate

Create new callout templates for your subscription.

### CalloutsUpdateTemplate

Update existing callout template for your subscription.

### CalloutsDeleteTemplate

Delete existing callout template from your subscription.

### CategoriesGetAll

Get all categories.

### CategoriesGetImageNames

Gets the names of all alert category images. You can get the image by going to account.signl4.com/images/alerts/categoryImageName.svg.

### CategoriesGetAllOfTeam

Sample Request: GET /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7

### CategoriesCreate

Sample Request: POST /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7 { "name": "Water", "imageName": "water.svg", "color": "#0000cc", "keywordMatching": "Any", "keywords": [ { "value": "H2O" }, { "value": "Water" } ] }

### CategoriesGetAllMetrics

Sample Request: GET /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7/metrics

### CategoriesGet

Sample Request: GET /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7/c0054336-cd89-4abf-882d-ba69b5adb25e

### CategoriesUpdate

Sample Request: PUT /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7/c0054336-cd89-4abf-882d-ba69b5adb25e { "name": "Water-Updated", "imageName": "water.svg", "color": "#0000cc", "keywordMatching": "All", "keywords": [ { "value": "H2O" }, { "value": "Water" }, { "value": "Wet" } ] }

### CategoriesDelete

Sample Request: DELETE /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7/c0054336-cd89-4abf-882d-ba69b5adb25e

### CategoriesGetMetrics

Sample Request: GET /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7/c0054336-cd89-4abf-882d-ba69b5adb25e/metrics

### CategoriesGetSubscription

Sample Request: GET /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7/c0054336-cd89-4abf-882d-ba69b5adb25e/subscriptions { }

### CategoriesSetSubscription

Sample Request: POST /categories/cbb70402-1359-477f-ac92-0171cf2b5ff7/c0054336-cd89-4abf-882d-ba69b5adb25e/subscriptions { }

### DutiesUserPunchIn

The specified user will be punched in to duty.

### DutiesUserPunchInAsManager

The specified user will be punched in to duty as a manager.

### DutiesUserPunchOut

The specified user will be punched out from duty.

### DutiesGetDutyAssistantInfo

Get duty assistant info for teams.

### EventDistributionDelete

Delete an event distribution.

### EventsGetOverviewPaged

Get overview event paged. If there are more results, you also get a continuation token which you can add to the event filter.

### EventsGetDetails

Get details of an event.

### EventsGetOverview

Get overview event by id.

### EventsGetParameters

Get parameters of an event by id.

### EventsSend

Send event.

### EventSourcesGet

Get event sources from one or more teams.

### EventSourcesCreate

Create event source.

### EventSourcesGetById

Get event source by id.

### EventSourcesUpdate

Update an event source.

### EventSourcesDelete

Delete an event source.

### PrepaidGetBalance

Get your subscription's current prepaid balance.

### PrepaidGetSettings

Get your subscription's current prepaid settings.

### PrepaidUpdateSettings

Update your subscription's current prepaid settings.

### PrepaidGetTransactions

Get your subscription's prepaid transactions.

### PrepaidUpdateSettings55

Update a subscription's current prepaid settings.

### RemoteActionsGetActions

Sample Request: GET /RemoteActions?externalId=12345

### RemoteActionsCreate

Sample Request: POST /remoteActions { "teamId": "cbb70402-1359-477f-ac92-0171cf2b5ff7", "name": "MyAction", "enabled": true, }

### RemoteActionsGetAvailableDefinitions

Retrieve all available definition.

### RemoteActionsGetDefinition

Retrieve a definition.

### RemoteActionsGetJobsPaged

Sample Request: POST /remoteActions/journal/paged?maxResults=100 { "statusCode": "Executed" }

### RemoteActionsGetJob

Sample Request: GET /remoteActions/journal/{jobId}

### RemoteActionsGet

Sample Request: GET /RemoteActions/{actionId}

### RemoteActionsUpdate

Sample Request: PUT /remoteActions/{actionId} { "teamId": "cbb70402-1359-477f-ac92-0171cf2b5ff7", "name": "MyAction", "enabled": true, }

### RemoteActionsDelete

Sample Request: DELETE /eaRemoteActions/{actionId}

### RemoteActionsCreateJob

Sample Request: POST /remoteActions/{actionId}/jobs { "remoteActionPin" : "1234", "parameters": [ { "name": "Param1", "value": "Value 1" } ] }

### SchedulesGetPlanned

Returns information about all planned schedules.

### ScriptsGetAll

Returns all script instances in the subscription.

### ScriptsCreate

Creates a new script instance of the script specified in the request body.

### ScriptsGetInfo

Gets the script instance specified by the passed instance id.

### ScriptsUpdateConfig

Updates the specified script instance, typically used for updating the configuration of a script.

### ScriptsDelete

Gets the script instance specified by the passed instance id.

### ScriptsUpdateCustomData

Updates the specified script instance.

### ScriptsDisable

Disables a given script instance.

### ScriptsEnable

Enables a script instance.

### ScriptsGetAvailable

Returns all available inventory scripts which can be added to a SIGNL4 subscription.

### ScriptsGetAll76

Returns all inventory scripts.

### ScriptsGet

Gets the script specified by the passed script id.

### SubscriptionsGetAll

Get infos of all available/managed subscriptions.

### SubscriptionsGetInfo

Get infos of a specific subscription.

### SubscriptionsGetChannelPriceInfo

Returns the subscription's channel price information.

### SubscriptionsGetFeatures

Returns the features of a specified subscription.

### SubscriptionsGetVoiceNumberLicense

Gets a subscription's voice number licenses.

### SubscriptionsGetPrepaidBalance

Get a subscription's current prepaid balance.

### PrepaidGetSettings84

Get a subscription's current prepaid settings.

### SubscriptionsGetPrepaidTransactions

Get a subscription's prepaid transactions.

### SubscriptionsUpdateProfile

Updates a subscriptions profile.

### SubscriptionsGetAllTeamsInfo

Get infos for all teams of the subscription.

### SubscriptionsGetUserLicenses

Gets a subscription's user licenses.

### TeamsGetAllInfo

Get infos of all teams.

### TeamsGetAlertSettings

Gets alert settings of a specific team.

### TeamsGetDutySettings

Get team duty settings for multiple teams.

### TeamsInviteUser

Invite users to a team.

### TeamsGetTeamMemberships

Get Team memberships from a user.

### TeamsGetAllPublicInformation

Get public information about all teams in a subscription.

### TeamsGetAllUsers

Getting all users of specified teams.

### TeamsGetInfo

Gets infos of a specific team.

### TeamsGetAlertReportsInfo

Get information about downloadable alert reports.

### TeamsGetAlertReport

Returns Alert Report.

### TeamsGetAlertSettings99

Gets alert settings of a specific team.

### TeamsSetAlertSettings

Sets alert settings of a specific team.

### TeamsGetAllAlertPatterns

Get all alerting patterns from team.

### TeamsCreateAlertPattern

Create a new alerting pattern.

### TeamsValidateAlertPattern

Validate a time slots of a pattern against all existing time slots of other patterns.

### TeamsUpdateAlertPattern

Update an existing alerting pattern.

### TeamsDeleteAlertPattern

Delete a specific alerting pattern.

### TeamsGetDutyReport

Download duty report with a specific fileName.

### TeamsGetEventSources

Gets event sources of a specific team.

### TeamsGetHolidays

Get holidays for team.

### TeamsDeleteHolidays

Delete holidays.

### TeamsSaveRepeatingHolidays

Save repeating holidays for a team.

### TeamsSaveHolidays

Save holidays for a team.

### TeamsCopyHolidays

Copy holidays from one team to another.

### TeamsGetImage

Gets image of a specified team.

### TeamsUploadImage

Uploaded a profile image for a specified team.

### TeamsDeleteImage

Deletes current team image.

### TeamsGetMemberships

Get team memberships by team.

### TeamsResendInvite

Sends invite email again if an invite exists.

### TeamsAddUser

Add user to a team.

### TeamsUpdateUserMembership

Updates the user's team membership. You can move the user to another team within the subscription and/or change the user's role.

### TeamsRemoveUser

Removes a user or invitation from a team.

### TeamsGetPrivacySettings

Get Team privacy settings for a team.

### TeamsUpdatePrivacySettings

Update privacy settings for a team.

### TeamsUpdateProfile

Updates team profile of a team.

### DutiesCreateOrUpdate

Create/Update given duty schedule.

### DutiesDeleteInRange

Delete duty schedules in range.

### DutiesSaveMultiple

Save multiple schedules. It is possible to override existing schedules if you wish.

### DutiesDelete

Delete a specific duty.

### DutiesGetInfo

Returns information of the duty schedule with the specified Id.

### TeamsGetSetupProgress

Gets setup progress of a specific team.

### UsersGettAll

Returns a list of user objects with details such as their email address and duty information. Only users you have access to will be returned.

### UsersGet

Returns a user object with details such as his email address and duty information.

### UsersDelete

Deletes user account.

### UsersUpdatePassword

Updates the password of a user.

### UsersCheckPermissions

Checks if a user has the provided permission.

### UsersDeleteContactAddress

Delete a contact address from a user.

### UsersGetDutyStatus

Returns a object with duty information.

### UsersGetImage

Get personal user image.

### UsersUploadImage

Uploaded a profile image for a specified user.

### UsersDeleteImage

Delete personal user image.

### UsersUpdateLocaleInfo

Update locale info (e.g. language, time zone) for user.

### UsersSetLocation

Set location of an user.

### UsersGetNotificationProfile

Get notification profiles for user.

### UsersSetNotificationProfile

Set notification profiles for user.

### UsersSetPhoneNumber

Set the phone number of a user. If another phone number is saved, it will be overwritten.

### UsersDeletePhoneNumber

Delete the phone number of a user.

### UsersValidatePhoneNumber

Validates a previous created phone number for a user.

### UsersUpdateProfile

Updates user profile of an user.

### UsersSendEventSourceInfoEmail

Sends a mail to a specified user that contains information of all event sources of the user's team.

### UsersGetSetupProgress

Gets setup progress of a specific user.

### UsersGetAllTeams

Returns a list of team objects with details such as their name. Only teams the user is member of will be returned.

### WebhooksGetAll

Returns a collection of defined outbound webhooks in the system.

### WebhooksCreate

Creates a new outbound webhook that will be notified when certain events occur.

### WebhooksGet

Returns information of the webhook specified by the passed id.

### WebhooksUpdate

Updates the specified webhook.

### WebhooksDelete

Deletes the specified webhook so that it will no longer be notified.

### WebhooksDisable

Ability to enable a webHook.

### WebhooksEnable

Ability to disable a webHook.

## Obtaining Credentials

In order to use the connector you need your SIGNL4 API key. In the SIGNL4 web portal you get your API key under Integrations -> API Key. You can then use your API key to authenticate your SIGNL4 connector in Power Automate.

## Getting Started

You can find a detailed description of the SIGNL4 API functions [here](https://connect.signl4.com/api/docs/index.html?urls.primaryName=SIGNL4%20API%20V2).
After adding the SIGNL4 connector to your Power Automate flow you need to obtain the SIGNL4 API key. Then you can use the API functions accordingly.
If you should have any further questions please do not hesitate to contact the SIGNL4 team.

The easiest way to start is to use the EventsSend action to send an alert:
- First get Your SIGNL4 team secret / integration secret from your SIGNL4 web portal.
- When you have the team secret / integration secret you can use this one in the function EventsSend to trigger an event that can generate an alert for your team. You just need to enter any Title and Message. You can find a description of the other parameters on the [inbound webhook documentation page](https://connect.signl4.com/webhook/docs/index.html).
- That's it and you now trigger an event and receive an alert in SIGNL4.

## Known Issues and Limitations

The SIGNL4 connector for Power Automate encapsulates the whole SIGNL4 REST API 2.0. Some functionalities depend on your [SIGNL4 plan](https://www.signl4.com/pricing/).

## Frequently Asked Questions

You can find the SIGNL4 FAQ and online help [here](https://signl4.zendesk.com/hc/en-us). Also there is a comprehensive [video library](https://vimeo.com/showcase/signl4) available.

## Deployment Instructions

Please use these [instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate.
