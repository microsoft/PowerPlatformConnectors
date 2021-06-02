## Africa's Talking Payments

Africa's Talking Payments provides a powerful API that allows you to charge and send payments to more than 300 million mobile account holders across Africa. This connectors exposes a couple of operations that allow you to seamlessly receive payments from payment providers, as well as initiate payments going out to your customers.

## Pre-requisites

In order to use this connector, you will need the following:

* An account with Africa's Talking; you can sign up [here](https://account.africastalking.com/auth/register?next=%2Fauth%2Fsignup).
* An application (sandbox or production). By default, a sandbox app is created for you when you sign up but you can create a production application afterwards if you need to use a live product.
* A payments product. If using a sandbox application, you can go ahead and create one under [`Payments > Products`](https://account.africastalking.com/apps/sandbox/payments/products). For a prod application, you'll have to request for a product first.

Once you've signed up and you have an application, generate a new API Key for that application; you will use this key to authenticate your requests from our Payments connector.

## Authentication.

This connector uses `API Key` authentication (see steps above on how to obtain one). When creating a new connector (in Power Apps/Logic Apps), you'll be required to provide an API Key.

You'll also be required to select an environment to use with this connector, either `sandbox` or `production`. Note that when using the sandbox environment, the username field to be used in all operations should be `sandbox`; otherwise, the username will be your prod application username.

## Supported Operations

This connector supports the following operations:

* `Mobile Checkout`: Initiate a Customer to Business (C2B) payments on a mobile subscriber’s device.
* `Mobile B2C (Business to Consumer)`: Send payments to mobile subscribers from your Payment Wallet.
* `Mobile B2B (Business to Business)`: Send payments to businesses e.g banks from your Payment Wallet.
* `Fetch Wallet Balance`: Fetch your wallet balance.
* `Wallet Transfer`: Transfer money from one Payment Product to another Payment Product hosted on Africa’s Talking.
* `Fetch Wallet Transactions`: Fetch your wallet transactions.
* `Top Up Stash`: Move money from a Payment Product to an Africa’s Talking application stash.
* `Fetch Product Transactions`: Fetch transactions of a particular payment product.

For more information on parameters accepted/required by the connector's operations, please refer to our [documentation](https://build.at-labs.io/docs/payments%2Foverview).
