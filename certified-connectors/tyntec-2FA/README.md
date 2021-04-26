## Tyntec Authentication

With tyntec 2FA you can add an extra layer of security by triggering two-factor authentication (2FA) with SMS and Voice directly from your PowerAutomate and PowerApps.

2FA connector is built around feature-rich APIs, that help you verify new users, reduce fake accounts and spam, build user trust, and minimize account takeovers with channels that don’t require smartphones, apps or data connection.

### Use Cases of this connector
-   Identify contacts before sensitive actions
-   Secure your flows
-   Verify new contacts in your Dynamics

And many more!

## Pre-requisites
You will need the following to proceed:
-   A Microsoft Power Apps or Power Automate plan with custom connector feature
-   [tyntec API Key](http://my.tyntec.com/api-settings)

## Supported requests
-  **[Send One-Time Password](https://api.tyntec.com/reference/#authentication-2fa-otp-service-sends-otp)**  * Creates [One-Time Password entity](https://api.tyntec.com/reference/#authentication-schemas) and deliveres it using specified channel.*
    - Parameters:
        -   Phone Number - Contact that should recieve the  One-time Password
        -   Text - In case you want to overwrite the application template
        -   Pin Lenght - Length of the auto generated PIN
        -   Delivery Channel - The delivery channel, either SMS, VOICE, or AUTO
        -   Application ID - The applicationId of the application you would like to use. If not specified, uses default one.
        -   Language - Specifies the local language in ISO 639-1
        -   Country - The ISO 3166-1 alpha-2 code of the destination number
        -   OTP Code - The OTP code to be delivered instead of auto generated one
        -   Sender - Sender name for OTP delivery via SMS (if you want to override the application template one)
        -   Caller - Caller id for OTP delivery via Voice (if you want to override the application template one)

-  **[Verify One-Time Password](https://api.tyntec.com/reference/#authentication-2fa-otp-service-otp-validate)** *Verifies the One-time Password*
    - Parameters:
        -   One-time Password ID - your One-time Password ID
        -   One-Time Password Code - Input One-time password code that you want to verify
-  **[Get One-Time Password Status](https://api.tyntec.com/reference/#authentication-2fa-otp-service-otp-status)** *Gets the [One-Time Password entity](https://api.tyntec.com/reference/#authentication-schemas)*
    - Parameters:
        -   One-time Password ID - your One-time Password ID
-  **[Resend One-Time Password](https://api.tyntec.com/reference/#authentication-2fa-otp-service-resend-an-otp)** 
    - Parameters:
        -   One-time Password ID - your One-time Password ID
        -   Delivery Channel - The delivery channel, either SMS, VOICE, or AUTO
        -   Sender - Sender name for OTP delivery via SMS (if you want to override the application template one)
        -   Caller - Caller id for OTP delivery via Voice (if you want to override the application template one)

-  **[Delete One-Time Password](https://api.tyntec.com/reference/#authentication-2fa-otp-service-delete-otp)** *Deletes the [One-Time Password entity](https://api.tyntec.com/reference/#authentication-schemas)*

## How to get API key 
Please [sign up for a free account](https://www.tyntec.com/create-account). In your account, select the [API settings](http://my.tyntec.com/api-settings) and copy your API key.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

