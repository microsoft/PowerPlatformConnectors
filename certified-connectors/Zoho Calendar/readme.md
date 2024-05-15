# Zoho Calendar

Zoho Calendar connector gives you the power to manage your calendar and perform repetitive actions such as create, edit, delete event by creating workflows using action trigger model.

## Publisher: Zoho Corporation

## Prerequisites

A verified Zoho Calendar account. Click [here](https://zoho.com/calendar) to create your first account.

## Supported Operations

The connector supports the following operations:

### Get calendar list

this endpoint gets the list of all calendars with the Calendar unique ID, Name, and timezone. For example, the user wants to see all their calendars before scheduling a meeting. They can enter the prompt as get my zoho calendars, and the operation retrieves their calendars with their details.

### Get calendar details

this endpoint retrieves the details of a particular calendar using the Calendar unique ID. The operation takes the calendar unique ID as the input and fetches details about the specific calendar such as its name and timezone.\n For example, you are unsure of a calendar timezone before scheduling a meeting with someone. You can enter the prompt as "Can you tell me the timezone for the calendar def789-ghi012?" And the operation retrieves the timezone information about that specific calendar.

### Get Event list

this endpoint gets you the list of all the events with Event unique ID, Title, start time, end time based on the specified date range in  yyyy-MM-ddTHH:mm:sszzz  or yyyy-MM-dd format using the Calendar unique ID. The user gives the Calendar unique id, start and end dates as inputs in either yyyy-MM-ddTHH:mm:sszzz (including time) or yyyy-MM-dd (date only) format. The operation would return a list of events that fall within the specified date range that includes details such as the Event unique ID, Calendar unique ID, start time, end time etc. \n For example, you can have the operation show you all your  events this week. You can enter the prompt as "Show me all my events from today (2024-05-14) to Sunday (2024-05-19) for the calendar def789-ghi012 and the operation would retrieve all the events of calendar def789-ghi012 you have in that week.

### Create new event

Adds a new event to the user's calendar.
 	 
### Delete event

Deletes an event from the user's calendar.

### Get event

this endpoint gets the details of a particular event such as the Event unique ID, Title, start time, end time based on the Calendar unique ID and Event unique ID. The user enters the Calendar unique ID and Event unique ID as inputs. Based on the provided details, the operation would retrieve details about the event such as the Event unique ID, Title, start time, end time based. \n For example, the user wants to check their meeting schedule.  Users can enter the prompt as Get my event detils of event event-ijk012 for the Calendar cal-ghi012, and the operation retrieves the details regarding that event.

### Search Events

this endpoint gets the list of events from the specified Calendar unique ID based on the specified date range in yyyy-MM-ddTHH:mm:sszzz format. The user enters the alendar unique ID and the date range in yyyy-MM-ddTHH:mm:sszzz format as inputs. Based on the provided date range, the operation returns only those that fall within the specified timeframe. \n For example, you can have the operation get events from your calendar def789-ghi012. You can enter the prompt as Get all events of calendar def789-ghi012 for the next week. and the operation would retrieve all the matched events of calendar def789-ghi012 scheduled for the next week.

### Update Event

Updates an existing event in a user's calendar.	 	 	 

## Supported Triggers

The connector supports the following operations:
 	 	 	 	 	 
### New event notification

When an event is added to a calendar

### Update event notification

When an event is updated to a calendar

### Delete event notification

When an event is deleted in a calendar

## Obtaining Credentials

Zoho Calendar API uses OAuth2.0 to authenticate and hence no secondary authentication is required once the account has been set up.
	
## Known Issues and Limitations
Select the respective domain of your Zoho Calendar account. Failing to do so will result in an authentication error.

## Frequently Asked Questions

https://help.zoho.com/portal/en/kb/calendar/faqs

## Deployment Instructions
Refer the documentation [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.


