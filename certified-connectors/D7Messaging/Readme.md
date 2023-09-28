# D7Messaging

D7 offers a dependable and affordable cloud-based messaging platform that enables enterprises to engage with their customers through SMS, chat, and various social media platforms.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Automate subscription
* A valid subscription in [D7API](https://app.d7networks.com/)
* D7API Authentication Token. 

## Get D7SMS API_Username and API_Password
- [Register](https://app.d7networks.com/signup) for a D7API account 
- [Sign In](https://app.d7networks.com/signin) with your email and password and navigate to [API Tokens](https://app.d7networks.com/api-tokens)

Click on "Create Token" and provide a token "Name". 

On the next window, click on "Generate Token" and copy the Token

You're now ready to start using this integration.

## Supported Operations
The connector supports the following operations:
* `D7-Balance`: Get D7API account balance

	No parameters are required. 
* `SMS-SendMessage`: Send SMS through D7 SMS Gateway.

	Required parameters: content, originator, and recipients
* `NumberLookup`: D7's Number Lookup service to determine the current status of the number.

	Required parameters: recipient
* `OTP-SendOTP`: Generate and dispatch an OTP (One-Time Password) to a mobile number.

	Required parameters: originator, recipients, and content
* `OTP-ResendOTP`: Request a new OTP (One-Time Password) to be sent to a mobile number.

	Required parameters: otp_id
* `OTP-VerifyOTP`: Verify the OTP (One-Time Password) sent to your mobile number.

	Required parameters: otp_id, otp_code
* `Viber-SendMessage`: Send a message to a Viber number.

	Required parameters: originator, label, recipients, and content

## Support and documentation: 
You can find the detailed documentation [here:](https://d7networks.com/docs)

For all support requests and general queries, you can contact support@d7networks.com
or visit [contact-us](https://d7networks.com/contact/)

Also, you can avail of the live chat available on our website [d7networks.com](https://d7networks.com/) or you can text us via WhatsApp at +971566816452
