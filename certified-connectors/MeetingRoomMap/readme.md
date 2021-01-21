# MeetingRoomMap

Search and display images for locations.
Use admin site https://www.meetingroommap.net to upload your own images/floorplans. Then map your meeting rooms and people from Azure AD as well as other custom locations to these floorplans.
Use this connector to search and display the mapped locations. Operations exists for each of the location types: Meeting rooms, people and custom locations.

## Pre-requisites

You will need the following to proceed:

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- A subscription to MeetingRoomMap. Go to https://www.meetingroommap.net to read more and sign up. Free trials are available.

## Supported Operations

Operations for meeting rooms:

- `GetMeetingRoomDetails`: Get details for a specific meetingroom.
- `GetMeetingRoomImage`: Get image for a specific meeting room.
- `GetRooms`: Get all rooms (defined in Azure AD as meeting rooms).
- `GetRoomsByImageName`: Get all meeting rooms mapped to a specific image.
- `RoomLists`: Get meeting rooms lists as defined in Azure AD.
- `RoomsByListAddress`: Get all meeting rooms from specific list name.
- `NextMeetings`: Get the next meetings from Outlook for the current user including meeting room image details.
- `SearchMeetingRooms`: Get rooms by name search (from Azure AD meeting rooms).

Operations for people (office locations):

- `GetOfficeLocationImage`: Get image of specific office location.
- `GetOfficeLocations`: Get all office locations including people located at each office location.
- `GetOfficeLocationsByImage`: Get all office locations mapped to a specific image.
- `GetRoomWithPersonsDetails`: Get office location details including list of people for that office location.
- `SearchCoworkers`: Search coworker by name/email and return location info.

Operations for custom locations:

- `GetCategories`: Get all custom location categories defined.
- `GetCustomLocationImage`: Get image for specific custom location.
- `GetCustomLocations`: Get a list of all custom locations defined.
- `GetCustomLocationsByImageName`: Get all custom locations for a given image.
- `LocationDetails`: Get location details including image url by the location id.
- `SearchLocations`: Search custom locations by name.

Operations for images/floorplans:

- `Images`: Get list of all floorplans/images uploaded.

## How to get credentials

The connector needs authentication with an Azure AD account. Please log in with an existing user or system account.
