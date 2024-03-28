# SignHost

Signhost the workflow signing service by Entrust is a digital signing and identification solution, empowering all types of businesses with seamless and secure online transactions.
 
## Publisher
Entrust Netherlands B.V.

## Prerequisites

You will need a SignHost subscription in order to use this connector. You can generate an APIKey from the Signhost portal

## Supported Operations

### Get transaction details
Returns current transaction details. Please do not use this action for active polling.</br>
### Delete transaction 
Delete a transaction by a transaction id. When a transaction is not in an end-state (such as fully signed) the transaction will be cancelled and cleaned. A cancelled transaction can be told to send an e-mail notification to the awaiting signers that the transaction was cancelled. The status of the transaction will be set to cancelled. When a transaction is in an end-state the transaction the transaction will be cleaned. The status of the transaction will remain the same but we will clean any uploaded documents and sensitive data as soon as possible.</br>
### Download PDF
Returns the (signed) document(s).</br>
### Download receipt
Returns the receipt when the transaction is successfully signed.</br>
### Create transaction
Creates a new transaction.</br>
### Add file
Add a file to the transaction. </br>
### Start transaction
Starts a transaction with the same {transactionId}. </br>

## Obtaining Credentials
You can generate a usertoken on the settings page in the SignHost portal. Please make sure to use the 'APIKey ' prefix in front of your usertoken.  

## Known Issues and Limitations
Currently not all of the features are supported. We welcome you to reach out to us with any requests or inquiries.

## Deployment Instructions
Please follow the instructions in [Create a custom connector from an OpenAPI definition](https://learn.microsoft.com/en-us/connectors/custom-connectors/define-openapi-definition).