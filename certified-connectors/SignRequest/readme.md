
## SignRequest
SignRequest is a secure and legally binding e-signature service that enables you and your partners to sign contracts and other documents fast and without the hassle of having to print, sign and scan.


## Pre-requisites
A free SignRequest account is required. Create one on [https://signrequest.com](https://signrequest.com).


## API documentation
The regular [REST API](https://signrequest.com/api/v1/docs/) is also used for the Flow/LogicApps/PowerApps connector. You can refer to the fields on the [SignRequest Quick Create endpoint](https://signrequest.com/api/v1/docs/#tag/signrequest-quick-create).


## Supported Operations
- You can create signature requests based on templates or files provided by the connector (from POST form data, base64 encoded file content or from urls where SignRequest can download the document to sign).
- Flows can also be triggered based on status changes of SignRequests, viewed/signed etc.


## How to get credentials
Only a SignRequest account is required as OAuth2 is used to authenticate.


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps

