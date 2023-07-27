
## Blueink Connector
The Blueink Connector offers the possibility to create and send bundles, check the status of bundles, retreive the details of signers in your Blueink account, check packets statuses, and more in the Power Automate Platform

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Blueink API Subscription


## Building the connector 
When using it for the first time, the Blueink Connector expects you to create a connection using a Blueink API key. 

### Create Blueink API key
Click the `API` tab in the Blueink portal and click the `Add API Key` button in the Private API Keys section. Enter a name for the key and click `Submit`. The key will be displayed, click the `Copy` button to copy the key to the clipboard. You will paste this into the Power Automate connection dialog as your API key. The format must be 'Token <your key>'. Just pasting in the key will not work, it will need to be preceded by 'Token '.


## Supported Operations
The connector supports the following operations:
* `Manage Bundles`: Create and send Bundles, Check Bundle details, modify Bundles, and more
* `Manage Packets`: Check Packet details, update Packets, and more
* `Manage Persons`: Get Person details, update Persons, and more
* `Manage Templates`: Get Document Template details, update Document Templates, and more
* `Manage Webhooks`: Get Webhook details, update Webhooks, create Webhooks, and more


## Known issues and limitations
The Blueink API key must be preceded by 'Token ' in the connection dialog. Just pasting in the key will not work.
