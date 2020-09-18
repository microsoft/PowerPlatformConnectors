## Tyntec Portability Check

Adds the ability to check phone numbers against the tyntec number portability database to validate phone numbers. It can be used to increase conversion, improve phone number data mining, and reduce fraudulent SIM swaps attempts.


### Use Cases of this connector
- Validating Contact Phone numbers
- Mining more data on your Contact
- Understanding if you should use the SMS channel with the Contact
- Fraud Prevention (detect SIM swaps)
And many more!


## Pre-requisites
You will need the following to proceed:
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- [tyntec API Key](http://my.tyntec.com/api-settings)

## Supported requests
- **CheckPhoneNumber** using tyntec Portability Check API [reference](https://api.tyntec.com/reference/#number-information-global-number-portability)
    - To make a successful request, please, populate the followings fields:
        - *msisdn* - The Phone Number you want to check

## Interpreting received values
- "requestId": The ID of your request
- "msisdn": The phone number of interest, given in the international format
- "ported": An indication of the porting status (True/False)
- "errorCode": The reason for an unsuccessful attempt (if 'ffff' then the phone number doesn't exist)
- "mcc": A representative MCC (Mobile Country Code) of the operator
- "mnc": A representative MNC (Mobile Network Code) of the operator
- "ttId": The respective tyntec ID of the network
- "operator": The operator's name
- "country": Country of its operator 
- "timeZone": Timezone of the given Phone number
- "technology": The technology used by the operator's network. Possible values are: GSM, MVNO GSM, GSM/CDMA, Satellite, CDMA, iDen, iDen/GSM, Pager, Fixed
- "synchronous": true

## How to get API key 
Please [sign up for a free account](https://www.tyntec.com/create-account). In your account, select the [API settings](http://my.tyntec.com/api-settings) and copy your API key.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

