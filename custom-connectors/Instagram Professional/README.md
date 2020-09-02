# Instagram Professional Connector

### NOTE
> This is a *sample* connector.  The connector is provided here with the intent to illustrate certain features and functionality around connectors.

## Prerequisites

1. Create a Facebook app at `https://developers.facebook.com/docs/apps/#scenario`.
2. Add the 'Facebook Login' product to your app.
3. Within the Facebook developer experience, locate the Facebook app id. Replace 'facebook_app_id' in apiProperties.json with your Facebook app id.

    ```json
      "clientId": "facebook_app_id"
    ```
4. Connect your Instagram Professional account to a Facebook Page. Theuser should have enough permissions to perform tasks on that page.
5. Use the `paconn` [CLI tool](https://docs.microsoft.com/connectors/custom-connectors/paconn-cli) to create a new custom connector.
6. On the Facebook developer dashboard, update 'Valid OAuth Redirect URIs' under Products >> Facebook Login >> Settings with the redirect URI on security page of your custom connector.

## Known Limitations

This connector only supports scenarios with Instagram Professional accounts. 

## Supported Actions

The following actions are supported by the connector:

* `Get my accounts`: This operation gets the Instagram Professional user accounts.

* `Get my profile`: This operation returns detailed information about a User.

* `Get my recent media`: This operation returns recent media posted by the logged in User.

* `Get comments for media`: This operation returns a list of comments on a media object. It will return the 50 latest comments.
