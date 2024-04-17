# Zoho Calendar

Zoho Calendar connector gives you the power to manage your calendar and perform repetitive actions such as create, edit, delete event by creating workflows using action trigger model.

## Publisher: Zoho Corporation

## Prerequisites

A verified Zoho Calendar account. Click [here](https://zoho.com/calendar) to create your first account.

## Supported Operations

The connector supports the following operations:

### Get calendar list

Gets the list of all calendars with the calendar UID, name, and timezone.

### Get calendar details

Retrieves the details of a particular calendar using the Calendar UID such as its name and timezone

### Get Event list

Gets the list of all the events with Event UID, Calendar UID, Title, start date, end date, between the specified date range  in  yyyy-MM-ddTHH:mm:sszzz format.

### Create new event

Adds a new event to the user's calendar.
 	 
### Delete event

Deletes an event from the user's calendar.

### Get event

Gets the details of a particular event such as the Event Title, start date, end date, etc., based on the calendar UID and event UID

### Search Events

Gets the list of events from the specified calendar UID based on the date range in yyyy-MM-ddTHH:mm:sszzz format.

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


