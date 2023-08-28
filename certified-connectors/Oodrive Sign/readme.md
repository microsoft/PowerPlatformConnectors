
# Oodrive Sign Connector
The Sign solution from Oodrive helps organizations to easily manage, prepare, sign and share agreements. This connector connects to your Oodrive Sign environment and offers functionalities as you can retrieve in the web solution. 

## Publisher: Oodrive
Oodrive is Europe's first trusted collaboration suite. Join more than one million people using Oodrive to collaborate, communicate and make business flow with transparent tools to ensure security, sovereignity, and compliance.

## Prerequisites
To use this connector in Microsoft Power Automate, your organization must be owner of an Oodrive Sign environment. You must then have the API key that belong to your environment. It can be retrieved by contacting Oodrive Sign support service, or by contacting your IT service administrator. 

The triggers included in this connector will register the webhook for you. No additional data will be required. 
## Supported Operations

Oodrive Sign's connector supports many actions and triggers to manage agreements easily. 

Below is the list of actions that you can use. 

### __Get a list of all contracts__
Easily retrieve all contracts bound to your Oodrive Sign's environment.
You can filter the results using differents advanced parameters. See the documentation for more information. 

### __Get a contract's status__
For a specific contract, retrieve its status. 

### __Create a contract and send it for signature__
This action is an "all in one" operation. It allows you to create a contract in your environment, add recipients, documents, and send it for signature. 

### __Get a specific contract__
Retrieve metadata from a specific contract by specifying its unique identifier, such as its status, name, whether it is closed or not, etc. 

### __Download a contract's signed document__
Retrieve a file in PDF format from the contract's signed document. 

### __List contract's documents__
Retrieve a list of all documents bound to a specific contract.

### __Upload a document and attach it to a contract__
Upload a binary document to your environment and attach it to an existing contract.

### __Download the contract's documents__
Retrieve a file in PDF format which contains all the documents bound to the specified contract. 

### __List a contract's recipients__
Retrieve a list of all recipients bound to the specified contract.

### __Add a recipient to a contract__
Attach an existing recipient (that means: a recipient already saved in your environment) to the specified contract. 

### __Upload an attachment and add it to a contract__
Upload a document as attachment in your Oodrive Sign's environment, then attach it to the specified contract. 

### __Send a contract to be signed__
Send the specified contract and all its documents to be signed to all recipients. 

### __Vadidate a contract__
Countersign the specified contract. 

### __Withdraw a contract__
Withdraw a specified contract. That means, the specified contract will have the status ABANDONED even if it was sent for signature. 

### __Stop a contract's transaction__
Stop the transaction of the specified contract, so that you can modify contract metadata, documents bound to it and the signatories metadata.

### __Get signatures of a contract__
Retrieve a list of all signatures information from the specified contract. 

### __Resend notification email to a recipient__
Send a notification email to a specific recipient for contract. 

### __Resend notification email to all recipients__
Send a notification email to all recipients for contract. 

### __Download a contract's proof file__
Retrieve the proof file of the specified contract. 

### __Delete a recipient for a contract__
Delete a recipient from the specified contract. 

### __Create a Bundle__
Create a new Bundle with at least one existing contract.

### __List a bundle's contracts__
Retrieve a list of contract metadata bound to the specified bundle. 

### __Add a contract to a bundle__
Add the specified existing contract to the specified Bundle. 

### __Send a bundle for signature__
Send the specified bundle for signature. That means, all the contracts in the bundle will be sent for signature to their respective recipients. 

### __Withdraw a bundle__
Withdraw all contract from the specified bundle. 

### __Stop a bundle transaction__
Stop the transaction of all contracts embeded in the specified bundle. 

### __Validate contracts in a bundle__
Countersign all contracts embeded in the specified bundle.

### __List all perimeters__
Retrieve a list of all perimeters bound to your Oodrive Sign's environment.

### __List all recipients__
Retrieve a list of all recipients bound to your Oodrive Sign's environment. 

### __Get a specific recipient__
Retrieve a specific recipient form your Oodrive Sign's environment, in the saved list of recipient. 

### __When a contract's status change__
This operation will be triggered whenever a contract's status, from you Oodrive Sign's environment, has changed. The target contracts can be filtered. More information in the documentation. 

## Obtaining Credentials

An API key is required to authenticate to Oodrive Sign's service. The API key is given in the welcoming e-mail that you received when you subscribe to a plan of Oodrive Sign service. 

## Connector Documentation
For detailed documentation around the connector please refer to your Oodrive Sign environment API specifications. 

## Known Issues and Limitations
For issues with Oodrive Sign and/or the connector, please contact the [support service](https://www.oodrive.com/support/). 

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.