# Mastodon Connector

This connector lets you interact with the [Mastodon microblogging](https://joinmastodon.org/) platform.

Similar to how blogging is the act of publishing updates to a website, microblogging is the act of publishing small updates to a stream of updates on your profile. You can publish text posts and optionally attach media such as pictures, audio, video, or polls. Mastodon lets you follow friends and discover new ones. Unlike other microblogging platforms, Mastodon is decentralized, interoperable and open source.

## Publisher: Hagen Deike

## Prerequisites

### Creating the Mastodon Application credentials

There is no unified API at Mastodon. Instead, you have to set up access in your profile. The URL is then formed from the domain of the Mastodon server on which you have registered your profile.

1. **Open** your **Mastodon** accounts website e.g. `mastodon.example`
2. Go to your settings with **Edit profile** 
3. Navigate to the menu **Development**
4. Click the button **New application**
5. Fill in the field **Application name**
6. Fill in the field **Redirect URI** and enter *`https://global.consent.azure-apim.net/redirect`*
7. Choose **read, write & follow** for the scope and save
8. Open the newly created application and memorize *Client key & Client secret* for the further process

![Application Settings](/custom-connectors/Mastodon/images/ApplicationSettings.png)

## Supported Operations

### Timelines

- `ViewPublicTimeline`: View public timeline
- `ViewHashtagTimeline`: View public statuses containing the given hashtag.
- `ViewHomeTimeline`: View statuses from followed users.

### Statuses

- `PostNewStatus`: Publish a status in your timeline.

## Known Issues and Limitations

Future improvements:

- No Triggers have been implemented yet.
- Options for *PostNewStatus* are still missing.

## Deployment Instructions

Please use [these instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/define-openapi-definition) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps

1. Go to the newly created connector and **edit** it
2. Define the **Host** parameter. This is the URL of your Mastodon server where you created the application in your profile e.g. `mastodon.example`
3. The field **Basis-Url** should be set to */api/v1*
4. *Optional: you can replace the icon and color with your own settings*
5. Select the authentication type **OAuth2** / **Generic Oauth2** for login
6. Fill in the field **Client ID** as found in your Mastodon profil
7. Fill in the field **Client secret** as found in your Mastodon profil
8. Fill in the field **Authorization URL** as followed `https://<mastodon.example>/oauth/authorize`
9. Fill in the field **Token URL** as followed `https://<mastodon.example>/oauth/token`
10. Fill in the field **Refresh URL** as followed `https://<mastodon.example>/oauth/token`
11. Fill in the field **Scope** as followed `read, write, follow`

Create, login and test the new mastodon connector.
