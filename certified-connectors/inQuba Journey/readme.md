
## inQuba Journey Power Automate Connector
The inQuba Journey connector allows you to seamlessly integrate with the inQuba Journey API. This will allow you to use existing connectors in power automate to transfer data to inQuba and track your customer journeys.

## Publisher: InQuba Customer Intelligence (Pty) Ltd


## Prerequisites
You will need the following to proceed:
* A registered inQuba Journey Instance deployed
* An Azure subscription
* A Microsoft Power Apps or Power Automate plan with custom connector feature;

## Sign Up for an InQuba Subscription 
Navigate to https://azuremarketplace.microsoft.com/en-us/marketplace/apps/inquba.inqubajcv1?tab=Overview to sign up through the Azure Market Place.

### Obtaining Credentials
Authorization is required on all inQuba APIs. Once you have completed the inQuba registration process, please contact your account manager or support@inquba.com to obtain your API credentials. These credentials will then be used on the Journey connector.

## Known Issues and Limitations
The create event and create transaction API currently only support single event and transaction payloads per request. For multiple events and transactions it is recommended to initiate many iterated requests.

## Supported Operations
The connector supports the following operations:
* `Acquire Access Token`: Acquire a valid access token to be used with the inQuba API
* `Create Event`: Publish an event record to the Events API in order to trigger communications
* `Create Transaction`: Publish a transaction record to the Transactions API and track your customer journeys

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector should you wish to do so in Microsoft Power Automate and Power Apps



