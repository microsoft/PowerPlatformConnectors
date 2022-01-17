## Africa's Talking SMS

Africa's Talking SMS provides a powerful API that allows you to effectively send text communication to your customers when they need it. This connectors exposes a couple of operations that allow you to send SMS and retrieve messages in your inbox from our APIs.

## Pre-requisites

In order to use this connector, you will need the following:

* An account with Africa's Talking; you can sign up [here](https://account.africastalking.com/auth/register?next=%2Fauth%2Fsignup).
* A live production application; you can create one after registering.

Once you've signed up and you've created a live application, generate a new API Key for that application; you will use this key to authenticate your requests from our SMS connector.

## Authentication.

This connector uses `API Key` authentication (see steps above on how to obtain one). When creating a new connector (in Power Apps/Logic Apps), you'll be required to provide an API Key.

## Supported Operations

This connector supports the following operations:

* `Send SMS`: Send a text message to multiple mobile recipients.
* `Fetch Inbox`: Fetch messages from your application's inbox.

It also supports the following triggers:

* `When you receive a Delivery Report for a sent message`: Triggered when you receive a Delivery Report for a message initially sent.
* `When you receive a message in your inbox`: Triggered whenever a message is sent to any of your registered shortcodes.
* `When a number opts out of receiving your bulk messages`: Triggered whenever a user opts out of receiving messages from your alphanumeric sender ID.

For more information on parameters accepted/required by the connector's operations, please refer to our [documentation](https://developers.africastalking.com/docs/sms/overview).
