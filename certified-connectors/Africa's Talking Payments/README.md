## Africa's Talking Payments

Africa's Talking Payments provides a powerful API that allows you to charge and send payments to more than 300 million mobile, bank and card account holders across Africa. This connectors exposes a couple of operations that allow you to seamlessly receive payments from payment providers, as well as initiate payments going out to your customers.

## Pre-requisites

In order to use this connector, you will need the following:

* An account with Africa's Talking; you can sign up [here](https://account.africastalking.com/auth/register?next=%2Fauth%2Fsignup).
* A live prod application; you can create one after registering.
* A payments product; you can request for one after you've registered.

Once you've signed up and you've created a live application, generate a new API Key for that application; you will use this key to authenticate your requests from our Payments connector.

## Authentication.

This connector uses `API Key` authentication (see steps above on how to obtain one). When creating a new connector (in Power Apps/Logic Apps), you'll be required to provide an API Key.

## Supported Operations

This connector supports the following operations:

* `Mobile Checkout`: Initiate a Customer to Business (C2B) payments on a mobile subscriber’s device.
* `Mobile B2C (Business to Consumer)`: Send payments to mobile subscribers from your Payment Wallet.
* `Mobile B2B (Business to Business)`: Send payments to businesses e.g banks from your Payment Wallet.
* `Fetch Wallet Balance`: Fetch your wallet balance.
* `Wallet Transfer`: Transfer money from one Payment Product to another Payment Product hosted on Africa’s Talking.
* `Card Checkout`: Collect money into your Payment Wallet by initiating transactions that deduct money from a customers Debit or Credit Card.
* `Card Checkout Validate`: Validate card checkout charge requests.
* `Bank Checkout`: Collect money into your payment wallet by initiating transactions that deduct money from a customers bank account.
* `Bank Checkout Validate`: Validate bank checkout charge requests.
* `Fetch Wallet Transactions`: Fetch your wallet transactions.
* `Bank Transfer`: Move money from your Payment Wallet to bank accounts.
* `Top Up Stash`: Move money from a Payment Product to an Africa’s Talking application stash.
* `Fetch Product Transactions`: Fetch transactions of a particular payment product.
* `Find Transaction`: Find a particular payment transaction.

For more information on parameters accepted/required by the connector's operations, please refer to our [documentation](https://build.at-labs.io/docs/payments%2Foverview).
