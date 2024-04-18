## Cornerstone Learning vILT Connector
The custom connector streamlines vILT integration by leveraging customers' Azure AD tenant environments, ensuring data integrity and simplifying authentication. No external Graph API permissions are needed, enhancing security. This versatile connector facilitates low-code solutions for diverse scenarios. By incorporating this custom connector, the full potential of Microsoft Teams API in the case of MS Teams integration is unlocked seamlessly within customers' infrastructure.

## Prerequisites
Before proceeding, ensure you have the following:
* API Key Generated in Edge Framework via Azure Connector Tile
* Environment where your coporation resides (US, UK, FR)
* Your Corporation Identifier

## Steps to Set-Up
Follow these steps to set up the connector:
* Generate a new API Key through the Edge Integrations Tile. Make a note of the API key as it will only be displayed once.
* In your Logic Apps environment, select the Cornerstone Learning Connector Trigger for the desired workflow.
* You will be prompted for a connection where you'll need to:
	- Choose your environment from the dropdown list.
	- Add the API key generated in the tile
	- Input your corporation identifier provided by Cornerstone.

# Supported triggers
Cornerstone Learning vILT Connector supports the following operations:
1.  Create Instructor – This operation triggers a flow when a new instructor is added to the vILT vendor.
2.  Update Instructor – This operation triggers a flow when an instructor is updated in the vILT vendor.
3.  Create Session – This operation triggers a flow when a new schedule is created for a vILT session.
4.  Update Session – This operation triggers a flow when a schedule is modified for a vILT session.
5.  Delete Session – This operation triggers a flow when a schedule is deleted.
6.  Get Attendance – This operation triggers a flow when a user triggers the "Update Attendance" event for a vILT session in the roster page.
7.  Launch URL – This operation triggers a flow when an instructor or attendee try to launch a vILT schedule either through the "My calendar" or "My transcript" page.

# Supported Actions
1.  Respond to Create Instructor - This action returns an expected response for the Create Instructor Trigger.
2.  Respond to Update Instructor - This action returns an expected response for the Update Instructor Trigger. 
3.  Respond to Create Session - This action returns an expected response for the Create Session Trigger.
4.  Respond to Update Session - This action returns an expected response for the Update Session Trigger.
5.  Respond to Delete Session - This action returns an expected response for the Delete Session Trigger.
6.  Respond to Get Attendance - This action returns an expected response for the Get Attendance Trigger which includes the emails of the attendees os the session. This information will update the roster.
7.  Respond to Launch URL - This action returns an expected response for the Launch URL Trigger which includes the url for the vILT session.



