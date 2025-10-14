# Cornerstone Learning vILT Connector
The custom connector streamlines vILT integration by leveraging customers' Azure AD tenant environments, ensuring data integrity and simplifying authentication. No external Graph API permissions are needed, enhancing security. This versatile connector facilitates low-code solutions for diverse scenarios. By incorporating this custom connector, the full potential of Microsoft Teams API in the case of MS Teams integration is unlocked seamlessly within customers' infrastructure.

## Publisher: Cornerstone OnDemand
https://www.cornerstoneondemand.com/solutions/integrations-and-apis/

## Prerequisites
Before proceeding, ensure you have the following:
* API Key Generated in Edge Framework via Azure Connector Tile
* Environment where your coporation resides (US, UK, FR), visible in Azure Connector Tile.
* Your Corporation Identifier, visible in Azure Connector Tile.

## Supported Operations
The Cornerstone Learning vILT Connector provides comprehensive integration capabilities, featuring 7 triggers and 7 corresponding actions. These functionalities facilitate real-time event handling and data synchronization across virtual instructor-led training (vILT) platforms.
Each trigger initiates a flow based on specific events within the vILT environment:

### Create Instructor (Trigger)
Activates when a new instructor is added.
### Update Instructor (Trigger)
Activates when an existing instructor's details are updated.
### Create Session (Trigger)
Activates when a new vILT session is scheduled.
### Update Session (Trigger)
Activates when a scheduled vILT session is updated.
### Delete Session (Trigger)
Activates when a vILT session is cancelled and its schedule is removed.
### Get Attendance (Trigger)
Activates when the attendance for a vILT session is updated via the roster page.
### Launch URL (Trigger)
Activates when an instructor or attendee launches a vILT session through the "My Calendar" or "My Transcript" pages.

Corresponding actions provide structured responses to the triggers, ensuring smooth operation and data consistency.

### Respond to Create Instructor
Sends a predefined response upon instructor creation.
### Respond to Update Instructor
Sends a predefined response upon instructor update.
### Respond to Create Session
Sends a predefined response when a session is created.
### Respond to Update Session
Sends a predefined response when a session is updated.
### Respond to Delete Session
Sends a predefined response when a session is deleted.
### Respond to Get Attendance
Sends a response that includes the attendees' email addresses, updating the session's roster.
### Respond to Launch URL
Sends a response that includes the URL for accessing the vILT session.

## Obtaining Credentials
Authentication requires an API Key. When accessing any of the operations, you will be prompted to set up a connection by providing your API Key, environment details, and corporation identifier. You can obtain these credentials by selecting the "Generate Settings" button located within the IPaaS connector tile in your edge framework.

## Getting Started
Follow these steps to set up the connector:
* Generate the settings through the Edge Integrations Tile. A new API key, environment and corporation identifier will be available. Make a note of the API key as it will only be displayed once. 
* In your Logic Apps environment, select the Cornerstone Learning Connector Trigger for the desired workflow.
* You will be prompted for a connection where you'll need to:
	- Choose your environment from the dropdown list.
	- Add the API key generated in the tile
	- Input your corporation identifier provided by Cornerstone.

## Known Issues and Limitations
Extended options are not available.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector should you wish to do so in Microsoft Power Automate and Power Apps