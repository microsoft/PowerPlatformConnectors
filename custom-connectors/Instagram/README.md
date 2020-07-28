# Instagram Connector

## Prerequisites

1. Create a Facebook app `https://developers.facebook.com/docs/apps/#scenario`.
2. Add 'Facebook Login' product to your app.
3. Replace 'facebook_app_id' in apiProperties.json with your Facebook app id.

    ```json
      "clientId": "facebook_app_id"
    ```
4. Connect Instagram professional account to a Facebook Page. User should have enough permissions to perform tasks on that page.
5. Use CLI tool `paconn` to create a new custom connector.
6. On Facebook developer dashboard update 'Valid OAuth Redirect URIs' under Products >> Facebook Login >> Settings. You can find the redirect URI on security page of your custom connector.

## Supported Actions

Following actions are supported by connector:

* `Get my accounts`: Get Instagram user accounts.

* `Get my profile`: This operation returns detailed information about a User.

* `Get my recent media`: This operation returns recent media posted by the logged in user.

* `Get comments for media`: This operation returns a list of comments on a media object. It will return latest 50 comments.
