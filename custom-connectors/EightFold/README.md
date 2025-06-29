# EightFold
Eightfold is a talent intelligence platform used by companies for recruiting, talent management, and workforce planning. 

## Publisher: HR Copilot

## Prerequisites
> To access the EightFold connector you need username and password. More documentation can be found here https://apidocs.eightfold.ai/docs/oauth-configuration

## Supported Operations

### GetExistingUsers
Get created users at Benifex platform by employee IDs with pagination support.

### ATS Position 
* `ATS Position Get`
- *Inputs:* `Body` `employeatsPositionId` `systemId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given an ats position id, it returns all attributes of the ats position.

* `ATS Position Update`
- *Inputs:* `Body` `employeatsPositionId``systemId`
- *HTTP Verb:* `PUT`
- *Description:* To update the existing Ats Position in the System, include the ats position id in the params.

* `ATS Position Patch`
- *Inputs:* `Body` `employeatsPositionId` `systemId`
- *HTTP Verb:* `PATCH`
- *Description:* Given an ats position id and data that needs to be patched, it patches the ats position.

* `ATS Position List`
- *Inputs:* `Body` `systemId` 
- *HTTP Verb:* `GET`
- *Description:* Search ats positions using query parameters and get the complete ats position objects.

* `ATS Position Create`
- *Inputs:* `Body` `systemId` 
- *HTTP Verb:* `POST`
- *Description:* To add a new ats position in the system, send the position information in the payload. ats position id will be sent back in case of a successful response.


### Position 
* `Position Get`
- *Inputs:* `Body` `PositionId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a position id, it returns all attributes of the position

* `Position Update`
- *Inputs:*  `Body` `PositionId`
- *HTTP Verb:* `PUT`
- *Description:* To update the existing Position in the System, include the positionId in the params. Please Note: Update will overwrite existing fields. If empty fields are sent during an update, those fields will be wiped out.

* `Position Patch`
- *Inputs:*  `Body` `PositionId`
- *HTTP Verb:* `PATCH`
- *Description:* Given a position id and data that needs to be patched, it patches the position. 

* `Position List`
- *Inputs:*  `Body` `start` `limit` `atsJobId` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search positions using query parameters and get the complete position objects.

* `Position Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* To add a new position in the system, send the position information in the payload. positionId will be sent back in case of a successful response

* `Position List Matched Candidates`
- *Inputs:*  `Body` `PositionId` `start` `limit` `atsJobId` `filterQuery` `lead` `validatedSkills` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns all the matching candidates for a position based on the filter query params provided in the API request. 

* `Position List Applicants`
- *Inputs:*  `Body` `PositionId` `start` `limit` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns all the matching applicants for a position

* `Position Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch positions for a given list of position ids (limit 100)

### Async API Status
* `Gives status of transaction`
- *Inputs:*  `Body` `transactionId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a transaction id, it returns the status and encoded entity id of the transaction, status can be PASS, IN PROGRESS or FAIL in the past 3 days

### Profile 
* `Profile Get`
- *Inputs:*  `Body` `profileId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a profile id of a candidate, it returns all attributes of the profile

* `Profile Update`
- *Inputs:*  `Body` `profileId` `refreshClaimedProfile`
- *HTTP Verb:* `PUT`
- *Description:* To update the existing Profile(Candidate/Employee) in the System, include the profileId in the params. Please Note: Update will overwrite existing fields. If empty fields are sent during an update, those fields will be wiped out

* `Profile Patch`
- *Inputs:*  `Body` `profileId` `refreshClaimedProfile` 
- *HTTP Verb:* `PATCH`
- *Description:* Given profile id and data that needs to be patched, it patches the profile. Note: All required body params in the schema are not enforced in PATCH request, unlike path params.

* `Profile List`
- *Inputs:*  `Body` `profileId` `atsCandidateId` `start` `limit` `filterQuery` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search profiles using query parameters and get the complete profile objects

*`Profile Create`
- *Inputs:*  `Body` `internalGroupAffiliation`
- *HTTP Verb:* `POST`
- *Description:* This API creates a profile with the given data in the payload.

* `Profile List Matched Positions`
- *Inputs:*  `Body` `profileId` `start` `limit` `filterQuery` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns all the matching positions for a profile

* `Creates or advances an application stage`
- *Inputs:*  `Body` `profileId`
- *HTTP Verb:* `POST`
- *Description:* Create or advance application stage for a given profile and position

* `Profile Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch profiles for a given list of profile ids (limit 100)

* `GET attachment`
- *Inputs:*  `Body` `profileId` `attachmentId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given the key for an attachment, it fetches the attachment in octet-stream format

### Careerhub
* `Suggest Skills Get`
- *Inputs:*  `Body` `profileSection` `attachmentId` `term` `userEmail` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* For a given profile id, fetch suggested skills for a particular profile section.

* `Career Planner Role Get`
- *Inputs:*  `Body` `roleSection` `userEmail` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns roles for a specified section. Possible values of sections are preferred-roles and recommended-roles

* `Career Planner Role Patch`
- *Inputs:*  `Body` `roleSection` `userEmail` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Patch preferred role for a user. Note: All required body params in the schema are not enforced in PATCH request, unlike path params.

* `Career Planner Skill Gap Analysis Get`
- *Inputs:*  `Body` `roleSection` `userEmail` `include` `exclude` `keyList`
- *HTTP Verb:* `PATCH`
- *Description:* Skill gap analysis data which includes present skills and recommeded skills for the provided role and current user profile

* `Career Planner Recommeded Course Get`
- *Inputs:*  `Body` `role` `userEmail`
- *HTTP Verb:* `GET`
- *Description:* Return recommended course for the current user and selected role pair.

### Career Navigator
* `Career Navigator Search`
- *Inputs:*  `Body` `userEmail` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns career navigator recommended career paths.

### Profile Feedback
* `Profile Feedback Get`
- *Inputs:*  `Body` `profileFeedbackId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a profile feedback id of a candidate, it returns all attributes of the profile feedback.

* `Profile Feedback Batch Fetch`
- *Inputs:*  `Body` `profileFeedbackId` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch profile feedbacks for a given list of feedback ids (limit 100)

* `Profile Feedback List`
- *Inputs:*  `Body` `profileFeedbackId` `start` `limit` `feedbackType` `profileId` `positionId` `requesterEmail` `reviewerEmail` `status` `include` `exclude` `keyList`
- *HTTP Verb:* `GEST`
- *Description:* Search profile feedbacks using specific filter parameters and get all attributes of profile feedbacks objects

### Profile Note
* `Profile Note Get`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Given a profile tag id of a candidate, it returns all attributes of the profile note.

* `Profile Note Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch profile notes for a given list of note ids (limit 100)

* `Profile Note List`
- *Inputs:*  `Body` `start` `limit` `profileId` `queryLimit` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Search profile notes using specific filter parameters and get the complete profile note objects.

### Profile Tag
* `Profile Tag Get`
- *Inputs:*  `Body` `profileTagId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a profile tag id of a candidate, it returns all attributes of the profile tag

* `Profile Tag Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch profile tags for a given list of tag ids (limit 100)

* `Profile Tag List`
- *Inputs:*  `Body` `start` `limit` `profileId` `queryLimit` `creatorEmail` `positionId` `include` `exclude` `keyList`
- *HTTP Verb:* `GETS`
- *Description:* Search profile tags using specific filter parameters and get all profile tag objects.

### Profile Application 
* `Profile Application Get`
- *Inputs:*  `Body` `profileApplicationId` `include` `exclude` `keyList`
- *HTTP Verb:* `GETS`
- *Description:* Given a profile application id, it returns all attributes of the profile application

* `Profile Application Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch profile applications for a given list of applications ids (limit 100)

* `Profile Application List`
- *Inputs:*  `Body` `start` `limit` `profileId` `queryLimit` `applicationId` `creatorEmail` `atsJobId` `applicationStatus` `stage` `positionId`  `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search profile applications using specific filter parameters and get all attributes of profile applications.

### User Message
* `User Message Get`
- *Inputs:*  `Body` `userMessageId` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Given a user message id, it returns all attributes of the user message.

* `User Message Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Batch fetch user messages for a given list of user message ids (limit 100)

* `User Message List`
- *Inputs:*  `Body` `start` `limit` `profileId` `queryLimit` `userId` `emailTo` `emailFrom` `userCampaignId` `templateId` `createdAt` `creatorEmail` `positionId` `replyTime` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search user messages using specific filter parameters and get all attributes of entities

### Email Delivery Feedback 
* `Email Delivery Feedback Get`
- *Inputs:*  `Body` `emailDeliveryFeedbackId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given the id of email delivery feedback, it returns all attributes of the entity

* `Email Delivery Feedback Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch email delivery feedbacks for a given list of entity ids (limit 100)

* `Email Delivery Feedback List`
- *Inputs:*  `Body``start` `limit` `queryLimit` `email` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search Email Delivery Feedbacks using specific filter parameters and get all attributes of the entity objects.

### ATS Candidate
* `ATS Candidate Get`
- *Inputs:*  `Body` `atsCandidateId` `systemId``include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given an ats candidate id of a candidate, it returns the ats candidate object.

* `ATS Candidate Update`
- *Inputs:*  `Body` `atsCandidateId` `systemId` `refreshClaimedProfile`
- *HTTP Verb:* `PUT`
- *Description:* To update the existing ats candidate, include the ats_candidate_id in the params. Please Note: Update will overwrite existing fields. If empty fields are sent during an update, those fields will be wiped out.

* `ATS Candidate Patch`
- *Inputs:*  `Body` `atsCandidateId` `systemId` `refreshClaimedProfile`
- *HTTP Verb:* `PATCH`
- *Description:* Given an ats candidate id and data that needs to be patched, it patches the ats candidate. Note: All required body params in the schema are not enforced in PATCH request, unlike path params.

* `ATS Candidate List`
- *Inputs:*  `Body` `systemId` `start` `limit` `filterQuery` `query` `email` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search ats candidates using query parameters and get the complete ats candidate objects.

* `ATS Candidate Create`
- *Inputs:*  `Body` `systemId`
- *HTTP Verb:* `POST`
- *Description:* To add a new ats candidate in the system, send the candidate information in the payload. ats candidate id will be sent back in case of a successful response

### User Campaign 
* `User Campaign Get`
- *Inputs:*  `Body` `userCampaignId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a campaign id, it returns all attributes of the user campaign.

* `User Campaign Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch user campaigns for a given list of campaign ids (limit 100)

* `User Campaigns List`
- *Inputs:*  `Body` `start` `limit` `queryLimit` `userId` `createdAt` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search user campaigns using specific filter parameters and get all attributes of entities.

### Course 
* `Course Get`
- *Inputs:*  `Body` `courseId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a course id of a course, it returns the course object

* `Patches courses`
- *Inputs:*  `Body` `courseId` `include` `exclude` `keyList`
- *HTTP Verb:* `PATCH`
- *Description:* Given an course id and data that needs to be patched, it patches the course.

* `Course List`
- *Inputs:*  `Body` `start` `limit` `query` `courseType` `recommendedFor` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search courses using query parameters and get the complete course object. recommendedFor feature does not support query addition. Note: It may take some time for the newly deleted courses to disappear from LIST call, as there may be indexing delays. You can still use the GET BY ID call right after deletion of course to confirm the deletion of the course.

* `Course Create`
- *Inputs:*  `Body` 
- *HTTP Verb:* `POST`
- *Description:* Does creation for a course.

### User Calendar Event
* `User Calendar Event Get`
- *Inputs:*  `Body` `userCalendarEventId`
- *HTTP Verb:* `GET`
- *Description:* Given the id of User Calendar Event, it returns all attributes of the entity

* `User Calendar Event Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch user calendar events for a given list of entity ids (limit 100)

* `User Calendar Event List`
- *Inputs:*  `Body` `start` `limit` `profileId` `creatorUserId` `queryLimit` `meetingTime` `assigneeUserId` `candidateEmail` `positionId` `status` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search User Calendar Events using specific filter parameters and get all attributes of the entity objects

### Event Candidate Activity 
* `Event Candidate Activity Get`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given an event activity id of a candidate, it returns all attributes of the event activity

* `Event Candidate Activity Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch event candidate activity for a given list of entity ids (limit 100)

* `Event Candidate Activity List`
- *Inputs:*  `Body` `plannedEventId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search event candidate activities using specific filter parameters and get all attributes of the entities 

### Event
* `Planned Event List`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* A get call for listing planned events data

* `Planned Event Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch planned events for a given list of planned event ids (limit 100)

* `Planned Event Get`
- *Inputs:*  `Body` `plannedEventId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a planned event id, it returns all attributes of the planned event

### Insights
* `Profile Insights List`
- *Inputs:*  `Body` `limit` `query` `distributionFieldsLimit` `positionMatchedFor` `fields` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns profile insights

* `Talent Traits Create`
- *Inputs:*  `Body` `query` `includeModels` `filterLocation` `filterSeniority` `filterTitle` `filterCompany`
- *HTTP Verb:* `POST`
- *Description:* Post a query with different filters for talent traits. All the values in the same filter is treated as an OR operation, while all the values in a different filter is treated as an AND operation.

*  `Employee Diversity Insights Overview`
- *Inputs:*  `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns Diversity insights of employees of an organization

### HRIS Employee
* `HRIS Employee Get` 
- *Inputs:*  `Body` `systemId` `uniqueIdentifier` `include``exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given an email id or employee id of an employee, it returns the HRIS employee information

* `HRIS Employees List`
- *Inputs:*  `Body` `start` `limit` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* List call that returns all the employee profiles.

### Join Pool Create
* `Talent Pool Create`
- *Inputs:*  `Body` 
- *HTTP Verb:* `GET`
- *Description:* Allowing the candidate to join talent pool

### Employee Role 
* `Employee Role GET`
- *Inputs:*   `roleId` `include` `exclude` `keyList` 
- *HTTP Verb:* `GET`
- *Description:* Given a role Id of a role, it returns the employee role

* `Employee Role Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* Create an employee role in Eightfold

### JIE
* `JIE Role GET`
- *Inputs:*  `Body` `roleId` `include` `exclude` `keyList` 
- *HTTP Verb:* `POST`
- *Description:* Given an Id of a role, it returns the JIE role for the same

* `JIE Role Update`
- *Inputs:*  `Body` `roleId` `include` `exclude` `keyList` 
- *HTTP Verb:* `PUT`
- *Description:* Update a role in Eightfold

* `JIE Role Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* Create a role in Eightfold

* `JIE Role List`
- *Inputs:*  `Body` `start` `limit` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* This endpoint returns a list of roles. If a new role is created, it takes some time to show up in the list call, you can fetch that role using get endpoints right after the creation of role

* `Get Succession Plan`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* This endpoint returns succession plan information for group id of a current user.

### User 
* `User Get`
- *Inputs:*  `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a user id of a user, it returns the user object

* `User Patch`
- *Inputs:*  `Body` `userId` `include` `exclude` `keyList`
- *HTTP Verb:* `PATCH`
- *Description:* Given a user id of a user with user data, it patches the user object. Note: All required body params in the schema are not enforced in PATCH request, unlike path params.

* `User List`
- *Inputs:*  `start` `limit` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Returns a list of users

* `User Create`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Create an User in Eightfold. email, firstName and lastName cannot be empty strings while creating an User. 

*  `User Batch Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch users for a given list of user ids (limit 100)
	
### Entity Changelog 
* `Entity Changelog list`
- *Inputs:*  `Body` `entityType` `start` `limit` `startTime` `endTime` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* This API fetches modified entity ids given a time frame. Maximum duration of a time window is limited to 24 hrs.

### Employee 
* `Employee Get`
* `Entity Changelog list`
- *Inputs:*  `profileId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a profile id of a employee, it returns all attributes of the employee

* `Employee Patch`
- *Inputs:*  `Body` `profileId`
- *HTTP Verb:* `PATCH`
- *Description:* Patches employee given its profile_id.

* `Employee Patch HRIS`
- *Inputs:*  `Body` `profileId` `employeeId`
- *HTTP Verb:* `PATCH`
- *Description:* Patches employee given its employee id.

* `Employee Create`
- *Inputs:*  `Body` 
- *HTTP Verb:* `POST`
- *Description:* Creates a new employee

* `Employees List`
- *Inputs:*   `start` `limit` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* List call that returns all the employee profiles.

* `Batch Employees Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch employees for a given list of profile ids

* `Recommended Mentors List`
- *Inputs:*  `profileid` `start` `limit` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a profile ID of an employee, return recommended mentors

* `Employees Match Score List`
- *Inputs:*  `start` `limit` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* List call that returns employees match scores against their current roles.

### Succession 
* `Succession Plan Get`
- *Inputs:*  `postionId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a position ID of the succession plan, return attributes of the plan

* `Succession Plan Patch`
- *Inputs:*  `Body` `profileId`
- *HTTP Verb:* `PATCH`
- *Description:* Update existing succession plan's metadata. 

* `Succession Plan Recommended Successor`
- *Inputs:*  `postionId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Get a list of recommended successors for the specified succession plan given a position ID.

* `Succession Plan List`
- *Inputs:*  `start` `limit` `incumbentIds` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GETS`
- *Description:* Search and get a list of succession plans based on the search parameters given

* `Succession Plan Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `GET`
- *Description:* Creates a new succession plan

* `Succession Plan Incumbent List`
- *Inputs:*  `positionId` `start` `limit` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Get a list of incumbents of the succession plan.

* `Succession Plan Successor Create`
- *Inputs:*  `Body` `positionId` `order` `profileId` `employeeId` `state`
- *HTTP Verb:* `POST`
- *Description:* An employee is added as a successor in the succession plan. The employee must be a valid and existing employee profile in Eightfold.

* `Succession Plan Successor Patch`
- *Inputs:*  `Body` `profileId` `positionId` `order` `profileId` `employeeId` `state`
- *HTTP Verb:* `PATCH`
- *Description:* Patch a successor in the succession plan. 

* `Succession Plan Caretaker Create`
- *Inputs:*  `Body` `positionId` `profileId` `employeeId`
- *HTTP Verb:* `POST`
- *Description:* Add an employee as a new caretaker to the succession plan. The employee must be a valid and existing employee profile on Eightfold.

### Demand 
* `Demand Get`
- *Inputs:*  `demandId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a demand id, it returns all attributes of the demand

* `Demand Patch`
- *Inputs:*  `Body` `demandId`
- *HTTP Verb:* `PATCH`
- *Description:* Patches demand given its id.

* `Demand Update` `demandId`
- *Inputs:*  `Body` 
- *HTTP Verb:* `PUT`
- *Description:* Updates demand given its id.

* `Demand Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* Create a new Demand

* `Demand List`
- *Inputs:*  `start` `limit` `query` `extDemandId` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search demands using query parameters and get the complete demand objects

* `Batch Demand Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch demands for a given list of demand ids

### Ext Demand 
* `External Demand Get`
- *Inputs:* `extDemandId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a external demand id, it returns all attributes of the external demand

* `External Demand Patch`
- *Inputs:*  `Body` `extDemandId` `include` `exclude` `keyList`
- *HTTP Verb:* `PATCH`
- *Description:* Patches external demand given its id.

* `External Demand Update`
- *Inputs:*  `Body` `extDemandId`
- *HTTP Verb:* `PUT`
- *Description:* Updates external demand given its id

* `External Demand Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* Create a new External Demand

* `External Demand List`
- *Inputs:* `start` `limit` `query` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search external demands using query parameters and get the complete external demand objects

* `External Demand Employee Application Update`
- *Inputs:*  `Body` `extDemandId`
- *HTTP Verb:* `POST`
- *Description:* Create or advance application stage of an employee using their employee ID

### Booking 
* `Booking Get`
- *Inputs:* `bookingId` `include` `exclude` `keyList`       
- *HTTP Verb:* `GET`
- *Description:* Given a Booking id, it returns all attributes of the Booking (Resource Management Entity)

* `Booking Patch`
- *Inputs:*  `Body` `bookingId`
- *HTTP Verb:* `PATCH`
- *Description:* Patches a booking given its id.

* `Booking Update`
- *Inputs:*  `Body` `bookingId`
- *HTTP Verb:* `PUT`
- *Description:* Updates a booking given its id.

* `Booking Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* Create a new Booking

* `Booking List`
- *Inputs:* `start` `limit` `employeeEmail` `demandId` `startDate` `endDate` `bookingType` `bookingStatus` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Search bookings using query parameters and get the complete booking objects.

* `Batch Bookings Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`  
- *HTTP Verb:* `POST`
- *Description:* Batch fetch bookings for a given list of booking ids

### Ext Booking 
* `Ext Booking Get`
- *Inputs:*  `extBookingId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given an external booking id, it returns all attributes of the external booking

* `External Booking Patch`
- *Inputs:*  `Body` `extBookingId`
- *HTTP Verb:* `PATCH`
- *Description:* Patches external booking given its id. 

* `External Booking Update`
- *Inputs:*  `Body` `extBookingId`
- *HTTP Verb:* `PUT`
- *Description:* Updates external booking given its id.

* `External Booking Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* Create a new External Booking

### Holiday 
* `Holiday Get`
- *Inputs:*  `holidayId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given a Holiday id, it returns all attributes of the Holiday (Resource Management Entity)

* `Holiday Patch`
- *Inputs:*  `Body` `holidayId`
- *HTTP Verb:* `PATCH`
- *Description:* Patches a holiday given its id. 

* `Holiday Update`
- *Inputs:*  `Body` `holidayId`
- *HTTP Verb:* `PUT`
- *Description:* Updates a holiday given its id.

* `Holiday Create`
- *Inputs:*  `Body`
- *HTTP Verb:* `POST`
- *Description:* Create a new Holiday

* `Holiday List`
- *Inputs:*  `start` `limit` `holidayType` `region` `startTime` `endTime` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search holidays using query parameters and get the complete holiday objects.

* `Batch Holidays Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch holidays for a given list of holiday ids

### Org Unit 
* `Org Unit Get`
- *Inputs:*  `orgUnitId` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Given an Org Unit id, it returns all attributes of the Org Unit

* `Org Unit List`
- *Inputs:* `start` `limit` `filterQuery` `query` `include` `exclude` `keyList`
- *HTTP Verb:* `GET`
- *Description:* Search org units using query parameters and get the complete org unit objects

* `Batch Org Unit Fetch`
- *Inputs:*  `Body` `include` `exclude` `keyList`
- *HTTP Verb:* `POST`
- *Description:* Batch fetch org units for a given list of org unit ids
	
## Obtaining Credentials
Make an Api call to the endpoint 'https://apiv2.eightfold-wu.ai/oauth/v1/authenticate'  with an API testing Tool with the body with "username", "password" and "grant_type".

## Known Issues and Limitations
"username" and "password" of eightfold platform are manadotory to estabalish connection with connectors.

## Deployment Instructions
Refer to "https://apidocs.eightfold.ai/docs/getting-started" 
