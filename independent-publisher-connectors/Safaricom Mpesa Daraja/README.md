# Safaricom Mpesa Daraja
The Connector provides API endpoints for accessing M-Pesa services, including B2C, C2B, and B2B.

# Overview

The daraja api connector allows low code developers to access safaricom mpesa services and intergrate them to their flows,power app or any othe service in power platform.

# Requirements

    - A Microsoft Azure account. If you don't have one, you can create a free trial account.
    - An existing Azure Active Directory tenant or the ability to create one.
    - Microsoft power platform.

# Getting started
# Registering an App in Microsoft Azure

This guide will walk you through the steps to register an app in Microsoft Azure. This is necessary to enable authentication and authorization for your application with Azure Active Directory.

# Steps to Register an App

    - Sign in to the Azure portal using your Azure account credentials.
    - In the left-hand menu, click on "Azure Active Directory."
    - Click on "App registrations" and then click the "+ New registration" button.
    - Enter a name for your app, choose the supported account types, and enter a redirect URI. The redirect URI is the URL where the user will be redirected after successfully authenticating with Azure. If you are not sure what to enter, the redirect url is available as a placeholder on the authentication page when building your connector.
    - Click the "Register" button.
    - After the app is registered, you will be taken to the app overview page. Here, you can view the app's details and configure various settings.
    - To enable authentication, click on "Authentication" in the left-hand menu.
    - In the "Implicit grant" section, check the box next to "Access tokens" and "ID tokens" and click the "Save" button.
    - Next, click on "Certificates & secrets" in the left-hand menu.
    - Click on "+ New client secret" and enter a description for the secret.
    - Set the expiration for the secret (e.g. 1 year).
    - Click the "Add" button.
    - After the secret is generated, copy the value and save it in a secure location. This secret will be used by your application to authenticate with Azure.


# Authors

    John Muchiri jeanfrancaiseharicot@gmail.com
    Bernard Karaba bkaraba14@gmail.com

## Source
[Safaricom Daraja API docs](https://developer.safaricom.co.ke/)


## Prerequisites
This API uses Oauth Authetication.


## Known Issues and Limitations
There are no known issues at this time.

## Supported Operations

###  Authorization
Gives you a time bound access token to call allowed APIs.

###  M-Pesa Express
Merchant initiated online payments.

###  Customer To Business (C2B)
Register URL for Validation/Confirmation and Simulate transaction.

### Business To Customer (B2C)
Transact between an M-Pesa short code to a phone number registered on M-Pesa.


###  Transaction Status
Check the status of a transaction.

### Account Balance
Enquire the balance on an M-Pesa BuyGoods (Till Number).

###  Reversals
Reverses an M-Pesa transaction.

