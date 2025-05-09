# Focusmate

Focusmate is a virtual co-working platform that helps you get things done by providing an accountability partner for 25 or 50-minute sessions dedicated to each attendee's focus area. This connector will allow you to retrieve your user information, your Focusmate sessions, and public information on your session partners.

## Publisher: Phil Cole

## Prerequisites

You have to create an account at Focusmate. You can sign up for free [here](https://focusmate.com). After you register, you will be then able to obtain your apiKey at the [Focusmate settings page](https://www.focusmate.com/profile/edit-p).

## Dates & Times

All the date and time values that occur in the API (both as query parameters and as returned payload fields) are **ISO8601 timestamps** according to **UTC timezone** e.g. `2018-08-24T08:24:48.652Z`.

## Supported Operations

### Get Profile

Gets the profile data for the calling user.

### Get Partner Profile

Gets the profile data for the given user id. Only publicly available data is returned.

### Sessions

Returns a list of sessions for the calling user.

* Sessions will be sorted in reverse chronological order (newest first)
* The start parameter must be less than or equal than the end parameter
* The time range provided must not exceed 1 year in length
* Datetime parameters must be in ISO 8601 format and must specify an offset (or 'Z' for UTC)
* All sessions partially within the given range will be returned
* The 10-minute gap (5-minute for 25-minute sessions) after a session ends does not count as part of that session
* Sessions whose start time matches the start parameter will be included
* Sessions whose end time matches the end parameter will be included
* Returned date-time values will always be in UTC
* The calling user will always be the first user in the users array. Note that a number of fields are only available for the calling user
* Canceled sessions are omitted

## Known Issues and Limitations

No issues and limitations are known at this time.

## Getting Started

You can visit [Focusmate API documentation](https://apidocs.focusmate.com/) to get even more information about the data returned by the actions (endpoints).

## Disclaimer

This connector is provided on a best-effort basis. If you face any issues, please let me know immediately!
