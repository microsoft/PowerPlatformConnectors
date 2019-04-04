# Facebook Connector

## Prerequisites

1. Create a Facebook app `https://developers.facebook.com/docs/apps/#scenario`.
2. Add 'Facebook Login' product to your app.
3. Replace 'facebook_app_id' in apiProperties.json with your Facebook app id.

    ```json
      "clientId": "facebook_app_id"
    ```

4. Use CLI tool `paconn` to create a new custom connector.
5. On Facebook developer dashboard update 'Valid OAuth Redirect URIs' under Products >> Facebook Login >> Settings.
  You can find the redirect URI on security page of your custom connector.

## Supported Actions

As part of this sample following actions are supported:

* `Get my Pages`: Lists all the pages that you own.

* `Get feeds from my timeline`: Gets feed of posts from your timeline.

* `Get page access token`: Get a page access token for a page you own.

* `Get Page Feeds`: Gets feed of posts from a page

* `Post to my page`: Post a message to a page that you own.
   This operation requires page access token, which can be retrieved by `Get page access token` action.
