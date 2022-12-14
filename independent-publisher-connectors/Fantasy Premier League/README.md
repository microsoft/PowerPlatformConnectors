# Fantasy Premier League Independent Connector
The Fantasy Premier League independent connector allows you to gather information from your fantasy football(soccer) leagues, ratings, information on current fixtures, general information and more. This will allow you to gather information on your fantasy league and post information about your league, such as current standings, to your Microsoft Teams groupchat or channel.

## Publisher: Joe Unwin (FlowJoe)

## Pre-requisites
To use this connector, you need the following
1. A Microsoft Power Apps or Power Automate plan with custom connector feature
2. A Fantasy Premier League account (You do not need any API authentication, just access to getting an ID of a league)

## Obtaining Credentials

You do not need any API authentication credentials, you do need access to the Fantasy Premier League website to gather ID's to make requests.

**League ID:** This can be found by navigating to a league on a desktop device and copying the ID from the URL. Example; https://fantasy.premierleague.com/leagues/258156/standings/c You would copy the ID 258156 from the URL.

**Manager ID:** This can be found by navigating to the 'Points' section on a desktop device and copying the ID from the URL. Example; https://fantasy.premierleague.com/entry/583476/event/5 You would copy the ID 583476 from the URL.

**Event ID:** This is the current Game Week number, for example if it is game week five you would put '5' as the event ID. 

## Supported Operations
The connector supports the following operations:
* [Classic League Standings](/api/leagues-classic/{league_id}/standings)
	- The Classic Leage Standings endpoint allows you to retrieve information from your league, including player first and last names, team names, overall league position, last weeks league position, total points and game week points.

* [Manager/User History](/api/entry/{manger_id}/history/)
	- The Manager/User History endpoint allows you to gather current and past years history on managers.
	
* [Manager/User Basic Information](/api/entry/{manger_id})
	- The Manager/User History endpoint allows you to gather information such as team names, first and last name, leagues joined and more.
	
* [Game Week Live Data](/api/entry/{event_id}/live/)
	- The Game Week Live Data endpoint allows you to get current live gameweek information, such as most captained, top scoring player, average score and more.
	
* [Player Detailed Data](/api/element-summary/{element_id}/)
	- The Player Detailed Data endpoint returns a player’s detailed information divided into 3 sections; fixtures, current league history and previous years history.
	
* [Fixtures](/api/entry/fixtures/)
	- The Fixtures endpoint returns a JSON array which contains every fixture of the season.
	
* [General Information](/api/entry/fixtures/)
	- The General Information endpoint returns general information about the Fantasy Premier League.
	
## Known Issues and Limitations

## Frequently Asked Questions
