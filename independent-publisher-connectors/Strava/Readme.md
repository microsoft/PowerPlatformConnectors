# Strava

Strava is a service for tracking human exercise which incorporates social network features. It is mostly used for cycling and running using GPS data

## Publisher: Richard Wierenga | Sogeti

## Prerequisites

You need to have a Strava account. You can [sign up](https://www.strava.com/register) for free.

## Supported Operations

### Get athlete stats

Returns the activity stats of an athlete. Only includes data from activities set to Everyone visibility.

### Get Authenticated Athlete

Returns the currently authenticated athlete. Tokens with profile:read_all scope will receive a detailed athlete representation; all others will receive a summary representation.

### List Athlete Activities

Returns the activities of an athlete for a specific identifier. Requires activity:read. Only Me activities will be filtered out unless requested by a token with activity:read_all.

### List Athlete Clubs

Returns a list of the clubs whose membership includes the authenticated athlete..

### Get Segment

Returns the specified segment. read_all scope required in order to retrieve athlete-specific segment information, or to retrieve private segments.

### List Starred Segments

List of the authenticated athlete's starred segments. Private segments are filtered out unless requested by a token with read_all scope.

### Get Activity

Returns the given activity that is owned by the authenticated athlete. Requires activity:read for Everyone and Followers activities. Requires activity:read_all for Only Me activities.

### List Activity Laps

Returns the laps of an activity identified by an identifier. Requires activity:read for Everyone and Followers activities. Requires activity:read_all for Only Me activities.

### List Activity Comments

Returns the comments on the given activity. Requires activity:read for Everyone and Followers activities. Requires activity:read_all for Only Me activities.

### List Activity Kudoers

Returns the athletes who kudoed an activity identified by an identifier. Requires activity:read for Everyone and Followers activities. Requires activity:read_all for Only Me activities.

### Get Club

Returns a given club using its identifier.

### List Club Members

Returns a list of the athletes who are members of a given club.

### List Club Administrators

Returns a list of the administrators of a given club.

### List Club Activities

Retrieve recent activities from members of a specific club. The authenticated athlete must belong to the requested club in order to hit this endpoint. Pagination is supported. Athlete profile visibility is respected for all activities.

## Obtaining Credentials

[Sign up](https://www.strava.com/register) and/or [Login](https://www.strava.com/login).
Go to the [Strava Api settings](https://www.strava.com/settings/api) page and create an app.

You should see the “My API Application” page now. Here is what everything means:

- Category: The category you chose for your application
- Club: Will show if you have a club associated with your application
- Client ID: Your application ID
- Client Secret: Your client secret (please keep this confidential)
- Authorization token: Your authorization token which will change every six hours (please keep this confidential)
- Your Refresh token: The token you will use to get a new authorization token (please keep this confidential)
- Rate limits: Your current rate limit
- Authorization Callback Domain: When building your app, change “Authorization Callback Domain” to localhost or any domain. When taking your app live, change “Authorization Callback Domain” to a real domain.

From the following page you can grab your client ID and client secret. The callback domain should be the following: "global.consent.azure-apim.net".

When your finished your api application page should look somehting like this:
![api settings](https://imgur.com/a/AwaOUu8)

For more information see the following page: [Link](https://developers.strava.com/docs/getting-started/#account)


## Known Issues and Limitations

No issues and limitations are known at this time.
