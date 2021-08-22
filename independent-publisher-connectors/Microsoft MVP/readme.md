# Microsoft MVP
The MVP Award Program is a community - and you've always been there for each-other! 
A lot of you want to help us make this community better, including creating a better experience for submitting contributions. 
The MVP Connector will directly enable you to use your own ideas to create and enhance your own MVP experience.
Microsoft MVP connector, by an MVP for all MVPs :)

## Publisher
### Ahmad Najjar

## Prerequisites
You need to be memeber of the [MVP community](https://mvp.microsoft.com/en-us) and you need to subscribe for the [Microsoft MVP API](https://mvpapi.portal.azure-api.net/).

## Authentication
This connector uses OAuth to authenticate you against your Live ID (hotmail.com) account. 
To use Microsoft/Office365/Live OAUTH in Power Automate / Power Apps, you must create a application in [(App Registration)](https://portal.azure.com) in Azure.

Follow these steps to create the app registration:
* Sign in to the Azure portal.
* If you have access to multiple tenants, in the top menu, use the Directory + subscription filter  to select the tenant in which you want to register an application.
* Search for and select Azure Active Directory.
* Under Manage, select App registrations > New registration.
* Enter a display Name for your application. Users of your application might see the display name when they use the app, for example during sign-in. You can change the display name at any time and multiple app registrations can share the same name. The app registration's automatically generated Application (client) ID, not its display name, uniquely identifies your app within the identity platform.
* Specify who can use the application, sometimes called its sign-in audience. Select Accounts in any organizational directory and personal Microsoft accounts.
* Don't enter anything for Redirect URI (optional). We'll configure a redirect URI Later.
* Select Register to complete the initial app registration.
* When registration finishes, the Azure portal displays the app registration's Overview pane. You see the Application (client) ID. Also called the client ID, this value uniquely identifies your application in the Microsoft identity platform.
* To add a redirect URI click on Authentication and under web enter these two URIs:
** https://login.live.com/oauth20_desktop.srf
** https://global.consent.azure-apim.net/redirect
* Make sure that Live SDK support is on under Advanced settings, and click save
* Finally we need to add a Client Secret (Sometimes called an application password)
* In the Azure portal, in App registrations, select your application.
* Select Certificates & secrets > New client secret.
* Add a description for your client secret.
* Select an expiration for the secret or specify a custom lifetime.
* Client secret lifetime is limited to two years (24 months) or less. You can't specify a custom lifetime longer than 24 months.
* Microsoft recommends that you set an expiration value of less than 12 months.
* Select Add.
* Record the secret's value for use in your client application code. This secret value is never displayed again after you leave this page.

## Supported Operations
#### Create contribution
Creates a new Contribution item.

#### Update contribution
Updates a Contribution item.

#### Delete contribution
Deletes a Contribution item.

#### Get contribution areas
Gets a list of Contribution areas grouped by Award Names.

#### Get contributions
Gets all your contributions (supports pagination).

#### Get contribution types
Gets a list of Contribution Types.

#### Get contribution by Id
Gets a Contribution item by id.

#### Get my MVP profile
Gets the current logged on user profile summary.

#### Get MVP profile by Id
Gets a users public profile (by Id).

#### Get MVP profile image
Gets the MVP Profile Image.

#### Get online identities
Restricted to the current user.

#### Get online identities by nominations Id
Restricted to the current user.

#### Get all historical cycle contributions
Gets all of the historical cycle Contributions for the MVP (supports pagination).

#### Get current cycle contributions
Gets all the current cycle Contributions for the MVP.

#### Get sharing preferences (visibility)
Gets a list of Sharing Preference / Visibility Types for Contributions.

## API Documentation
Visit [Microsoft MVP API reference](https://mvpapi.portal.azure-api.net/docs/services/580eb8bfac2551138cf5da27/operations/580eb8bfac25510f0c09c1a5) page for further details.

## Known Issues and Limitations
* Operation "Create contribution"
* Operation "Update contribution"
Activity type must be provided twice (I'm working on making the user experience friendlier and easier)
Visit [MVP API Known Issues](https://mvpapi.portal.azure-api.net/issues) to learn more about the API's issues

#### Not all operations provided by the Microsoft MVP API are part of the first IP connector submission. I will keep adding/updating/supporting this connector based on your feedback/requests :)