# D7Messaging

D7 offers a dependable and affordable cloud-based messaging platform that enables enterprises to engage with their customers through SMS, chat, and various social media platforms.

## Publisher: Signtaper Technologies FZCO

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Automate subscription
* A valid subscription in [D7API](https://app.d7networks.com/)
* D7API Authentication Token. 

## Supported Operations
The connector supports the following operations:
### `D7-Balance`: 
Get D7API account balance

No parameters are required. 

### `SMS-SendMessage`: 
Send SMS through D7 SMS Gateway.

Required parameters: content, originator, and recipients

### `NumberLookup`: 
D7's Number Lookup service to determine the current status of the number.

Required parameters: recipient

### `OTP-SendOTP`: 
Generate and dispatch an OTP (One-Time Password) to a mobile number.

Required parameters: originator, recipients, and content

### `OTP-ResendOTP`: 
Request a new OTP (One-Time Password) to be sent to a mobile number.

Required parameters: otp_id

### `OTP-VerifyOTP`: 
Verify the OTP (One-Time Password) sent to your mobile number.

Required parameters: otp_id, otp_code

### `Viber-SendMessage`: 
Send a message to a Viber number.

Required parameters: originator, label, recipients, and content

## Obtaining Credentials
#### Get D7SMS API_Username and API_Password
- [Register](https://app.d7networks.com/signup) for a D7API account 
- [Sign In](https://app.d7networks.com/signin) with your email and password and navigate to [API Tokens](https://app.d7networks.com/api-tokens)

Click on "Create Token" and provide a token "Name". 

On the next window, click on "Generate Token" and copy the Token

You're now ready to start using this integration.

## Known Issues and Limitations

To initiate Viber Messaging services, organizations need to register with Viber. For the registration process, please get in touch with support@d7networks.com.

## Deployment Instructions

Please use the instructions located [here](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate, Power Apps or Azure LogicApps.
