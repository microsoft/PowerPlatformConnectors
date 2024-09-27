# Miro
Miro (formerly RealtimeBoard) is an intuitive visual collaboration and whiteboarding platform for cross-functional teams.

## Publisher: Michał Romiszewski

## Prerequisites
You will need a free or paid [Miro](https://miro.com/) account in order to use this connector.

## Obtaining Credentials
Miro apps need authorization to access content on Miro boards. Authorization is an explicit consent that a user grants to a specific app, which in turn, provides access to specific Miro user data or team data. 
To get a **Client ID** and **Client Secret**, you will need to create your app in Miro following steps from the [Getting started guide](https://developers.miro.com/docs/getting-started):
1. Get developer team to create app [Get Dev Team](https://miro.com/app/dashboard/?createDevTeam=1).
2. On the [Your apps](https://miro.com/app/settings/user-profile/apps) page, review the Terms and Conditions, select the *I agree to the Terms and Conditions* and click *Create new app*.
3. On the *Create new app window*, in the *App Name* box, enter the name and *Click Create app*.
4. Select required scopes:
    - `boards:read` - Read boards you have access to.
    - `boards:write` - Modify boards you have access to.
5. Click *Install app and get OAuth token*.
6. On the *Install app to get OAuth token* window, Select a team, click *Install & authorize* button. After confirmation, click the *Close* button.
7. Fill in the *Redirect URI for the OAuth2.0* field:
    - `https://global.consent.azure-apim.net/redirect`
8. You can get the **Client ID** and **Client Secret** from the *App Credentials* section.

## Supported Operations
The connector supports the following operations:
### Boards
- `Get boards`: Get all boards available to the user.
- `Create a board`: Create a new board.
- `Get a board`: Get a specific board by its unique identifier.
- `Update a board`: Update a specific board by its unique identifier.
- `Copy a board`: Create a copy of an existing board.
- `Share a board`: Share an existing board with users.
### Widgets
- `Get widgets`: Get all widgets.
- `Create a card widget`: Create a new card widget.
- `Get a widget`: Get a specific single widget by its unique identifier.
- `Update a card widget`: Update a specific card widget by its unique identifier.
- `Delete a widget`: Delete a specific widget by its unique identifier.

## Known Issues and Limitations
There are no known issues at this time.