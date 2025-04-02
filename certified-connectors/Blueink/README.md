
## Blueink Connector
The Blueink Connector allows you to integrate Blueink's eSignature functionality into Power Automate workflows. With this connector you can send documents for eSignature, check the status of signing transactions, retreive the details of signers in your Blueink account, check statuses, and more.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Blueink subscription which includes API access (typically an API plan or an Enterprise plan)

## Building the connector 
On first use, the Blueink Connector expects you to create a connection using a Blueink API key. 

### Create Blueink API key
Click the `API` tab in the Blueink portal and click the `Add API Key` button in the Private API Keys section. Enter a name for the key and click `Submit`. The key will be displayed, click the `Copy` button to copy the key to the clipboard. You will paste this into the Power Automate connection dialog as your API key. The format must be 'Token <your key>'. Just pasting in the key will not work, it will need to be preceded by 'Token '.

## Terminology
Blueink uses the terms Bundle and Packet. A Bundle represents a signing transaction, and consists of a group of documents sent to 1 or more signers. It is roughly equivalent to the terms "Envelope" or "Signing Request" used by other platforms. A Packet represents a recipient of a Bundle, and also holds the status of their signing transaction and other configuration data.

## Supported Operations
The connector supports the following operations:
* `Manage Bundles`: Create and send Bundles, Check Bundle details, modify Bundles, and more
* `Manage Packets`: Check Packet (aka Signer) details, update Packets, and more
* `Manage Persons`: Get Person details, update Persons, and more
* `Manage Templates`: Get Document Template details, update Document Templates, and more
* `Manage Webhooks`: Get Webhook details, update Webhooks, create Webhooks, and more


## Known issues and limitations
The Blueink API key must be preceded by 'Token ' in the connection dialog. Just pasting in the key will not work.

## Deployment Instructions
Please use [these instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.
