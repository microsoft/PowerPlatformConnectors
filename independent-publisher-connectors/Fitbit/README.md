# Title
The Fitbit Web API is intended for a developer to start using the the Web API, along with providing additional content on implementing the endpoints. The co

## Publisher: Ashwin Ganesh Kumar

## Prerequisites
To use the Fitbit APIs, you need to have a Fitbit developer account. To create a developer account,

1. Go to https://accounts.fitbit.com/signup to register for a fitbit.com account. The email address must be valid to complete the verification process. An existing fitbit.com account can be used.
2. A verification email will be sent to the user requesting a response.Once the email address is verified, the user will be able to access https://dev.fitbit.com/apps to register new applications used to query the Web APIs.



## Supported Operations

### Get Activity Goals
Retrieves a user's current daily or weekly activity goals.

### Get Activity Log List
Retrieves a list of a user's activity log entries before or after a given day.

### Get Activity TCX
The Training Center XML (TCX) is a data exchange format that contains GPS, heart rate, and lap data. This endpoint retrieves the details of a user's location using GPS and heart rate data during a logged exercise

### Get Activity Time Series by Date
Retrieves the activity data for a given resource over a period of time by specifying a date and time period. The response will include only the daily summary values.

### Get Activity Type
Retrieves the details for a single activity from the Fitbit activities database in the format requested. If available, activity level details will display.

### Get Alarms
Retrieves the alarms enabled for a specific device. This endpoint is supported for trackers that support alarms.

### Get All Activity Types
Retrieves a list of all valid Fitbit public activities and the private, user-created activities from the Fitbit activities database in the format requested. If available, activity level details will display.

### Get AZM Time Series by Date
This endpoint returns the daily summary values over a period of time by specifying a date and time period.

### Get Badges
Retrieves a list of the user’s badges.

### Get Body Fat Log
Retrieves a list of all user's body fat log entries for a given date.

### Get Body Time Series by Date
Retrieves a list of all user's bmi, body fat, or weight for a given period.

### Get Breathing Rate Summary by Date
This endpoint returns average breathing rate data for a single date. Breathing Rate data applies specifically to a user’s “main sleep,” which is the longest single period of time during which they were asleep on a given date.

### Get Daily Activity Summary
Retrieves a summary and list of a user’s activities and activity log entries for a given day.

### Get Devices
Retrieves a list of Fitbit devices paired to a user's account.

### Get Favorite Activities
Retrieves a list of a user's favorite activities.

### Get Frequent Activities
Retrieves a list of a user's frequent activities.

### Get Friends Leaderboard
This endpoint returns the user's friend leaderboard in the format requested using units in the unit system which corresponds to the Accept-Language header provided. This scope does not provide access to the friend's Fitbit data. Those users will need to individually consent to share their data with your application.

### Get Heart Rate Time Series by Date
Retrieves the heart rate time series data over a period of time by specifying a date and time period. The response will include only the daily summary values.

### Get HRV Summary by Date
This endpoint returns the Heart Rate Variability (HRV) data for a single date. HRV data applies specifically to a user’s “main sleep,” which is the longest single period of time asleep on a given date.

### Get Lifetime Stats
Retrieves the user's activity statistics.

### Get Profile
Retrieves the user's profile data.

### Get Recent Activity Types
Retrieves a list of a user's recent activities types logged with some details of the last activity log of that type.

### Get Sleep Goal
Returns a user's current sleep goal.

### Get Sleep Log by Date
This endpoint returns a list of a user's sleep log entries for a given date. The data returned can include sleep periods that began on the previous date.

### Get Sleep Log by Date Range
This endpoint returns a list of a user's sleep log entries for a date range. The data returned for either date can include a sleep period that ended that date but began on the previous date.

### Get Sleep Log List
This endpoint returns a list of a user's sleep log entries before or after a given date, and specifying offset, limit and sort order. The data returned for different dates can include sleep periods that began on the previous date.

### Get VO2 Max Summary by Date
This endpoint returns the Cardio Fitness Score (also known as VO2 Max) data for a single date. VO2 Max values will be shown as a range if no run data is available or a single numeric value if the user uses a GPS for runs.

### Get Weight Log
Retrieves a list of all user's weight log entries for a given date.

### Get Weight Time Series by Date
Retrieves a list of all user's weight log entries for a given period.

## Obtaining Credentials

### Registering an Application
An application must be registered ([here](https://dev.fitbit.com/apps)) within the developer account prior to calling the Fitbit Web APIs for the first time. Each registered application is provided with a client ID and secret. These client credentials will need to be referenced by the application during user authorization.

## Known Issues and Limitations
No issues and limitations are known at this time.

## Deployment Instructions
Upload the connector and authorize by providing your **Client id** and **Client secret** from your registered application ([here](https://dev.fitbit.com/apps)).