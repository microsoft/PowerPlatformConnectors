## Africa's Talking Airtime

Africa's Talking Airtime offers a power API that allows you to distribute pinless airtime to mobile subscribers, receiving discounts for each transaction you make. This connector exposes operations that enable one to send airtime to mobile subscribers as well as to watch for notifications indicating whether the delivery was successful or failed.

## Pre-requisites

In order to use this connector, you will need the following:

* An account with Africa's Talking; you can sign up [here](https://account.africastalking.com/auth/register?next=%2Fauth%2Fsignup).
* A live production application; you can create one after registering.

Once you've signed up and you've created a live application, generate a new API Key for that application; you will use this key to authenticate your requests from this connector.

You will also need to contact us to activate the airtime product for your application; do so by sending an email to airtime@africastalking.com.

## Authentication.

This connector uses `API Key` authentication (see steps above on how to obtain one). When creating a new connector (in Power Apps/Logic Apps), you'll be required to provide an API Key.

## Supported Operations

This connector supports the following operations:

* `Send Airtime`: Send airtime to multiple recipients.
* `Find transaction status`: Find a particular airtime transaction's status.

It also supports the following triggers:

* `When you receive an Airtime Status Notification`: Triggered when you receive an delivery status notification for an initial request to send airtime.

For more information on parameters accepted/required by the connector's operations, please refer to our [documentation](https://developers.africastalking.com/docs/airtime/overview).
