# Facebook Connector

## Prerequisites

1. Create a Facebook app `https://developers.facebook.com/docs/apps/#scenario`.
2. Add 'Facebook Login' product to your app.
3. Replace 'facebook_app_id' in apiProperties.json with your Facebook app id.

    ```json
      "clientId": "facebook_app_id"
    ```

4. Use CLI tool `paconn` to create a new custom connector.
5. On Facebook developer dashboard update 'Valid OAuth Redirect URIs' under Products >> Facebook Login >> Settings. You can find the redirect URI on security page of your custom connector.

## Supported Triggers

As part of this sample following triggers are supported:

* `When there is a new post on my timeline`: Triggers a new flow when there is a new post on your timeline. It is possible for this trigger to not activate for all posts since detection of a post must pass several privacy checks including a person's privacy settings on Facebook.

## Supported Actions

As part of this sample following actions are supported:

* `Get my feed`: Get the feed from your timeline.

* `Get page feed`: Get posts from the feed of a specified page.

* `Post to my page`: Post a message to a page that you own.

* `Get user timeline`: Get posts from a user's timeline.
