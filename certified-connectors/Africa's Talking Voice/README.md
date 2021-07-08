## Africa's Talking Voice

Africa's Talking Voice offers a powerful API to build dynamic, scalable, fully-featured voice applications that reside entirely in the cloud without the need to purchase and maintain expensive voice equipment. This connector offers an operation to initiate a call to multiple recipients, providing a couple of voice actions that specify how the call should be handled.

## Pre-requisites

In order to use this connector, you will need the following:

* An account with Africa's Talking; you can sign up [here](https://account.africastalking.com/auth/register?next=%2Fauth%2Fsignup).
* A live production application; you can create one after registering.
* A live phone number on this application; you can request for one by sending an email to voice@africastalking.com

Once you've signed up and you've created a live application, generate a new API Key for that application; you will use this key to authenticate your requests from this connector.

## Authentication.

This connector uses `API Key` authentication (see steps above on how to obtain one). When creating a new connector (in Power Apps/Logic Apps), you'll be required to provide an API Key.

## Supported Operations

This connector supports the following operations:

* `Call`: Make an outbound call to multiple recipients. For now, this operation only supports two voice *actionTypes*: `Say` and `Play`. 

For more information on parameters accepted/required by the connector's operations, please refer to our [documentation](https://developers.africastalking.com/docs/voice/overview).
