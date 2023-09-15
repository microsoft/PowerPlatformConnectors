## Phone Verification API
Phone Verification service aims to determine validity, reachability and fraud risk of phone numbers entered into forms by users or auto dialers so that brands can ensure their customer records are accurate, improve conversion rates for customer outreach, and guard against bad actors.

### Use Cases of this connector
- Validating Contact Phone numbers
- Mining more data on your Contact
- Understanding if you should use the SMS channel with the Contact
- Fraud Prevention 
And many more!

## Pre-requisites
You will need the following to proceed:
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- [tyntec API Key](http://my.tyntec.com/api-settings)

## Supported requests
- **Verify Phone Number** using tyntec Phone Verification API [reference](https://api.tyntec.com/reference/verification/current.html#phone-verification-api)
    - To make a successful request, please, populate the followings fields:
        - *Phone Numberr* - The Phone Number you want to check

## Interpreting received values
- **Validity Check**: check if the phone number is valid according to the numbering plan for that country.
- **Active Status**: check if the phone number has been assigned to a subscriber by an operator.
- **Number Type**: determine the phone number type to decide which technology to use to communicate.
- **Mobile Operator**: identify the name and the country of the home network operator for the subscriber.
- **Outreach Status**: check if an SMS can be delivered to the subscriber behind the phone number.
- **Fraud Risk**: determine the likelihood of the phone number being used by a bad actor.

## How to get API key 
Please [sign up for a free account](https://www.tyntec.com/create-account). In your account, select the [API settings](http://my.tyntec.com/api-settings) and copy your API key.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

