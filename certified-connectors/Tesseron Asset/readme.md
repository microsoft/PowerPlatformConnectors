## Tesseron Asset Connector
With Tesseron, we offer your company an innovative solution for cooperation with your customers, partners and suppliers.This connector allows you to use Tesseron Asset API to create and manage assets.

## Publisher: Publisher's Name
Lutihle + Luithle GmbH


## Prerequisites
You will need the following to proceed:
* A Tesseron Instance
* A Tesseron licensed user
* An API Key (Service: Asset)


## Supported Operations
The connector supports the following operations:
* `Add Asset`: Create assets in your tesseron instance
* `Update Asset`: Update your tesseron assets with the newest data
* `Get asset template configuration`: Receive the configuration of the requested asset template
* `Search asset`: Get a list of assets with the specified parameters
* `Get asset information`: Receive asset details (fields, values and options)


## Obtaining Credentials
Authentication is done via an API key. Please provide your API key and your instance URL als connection parameters.


## Known Issues and Limitations
* Sufficient user rights are mandatory to receive asset information


## Deployment Instructions
Since Tesseron Rest API uses API keys to validate your user, you first need to contact your system administrator to create an API key for your user. After that is completed you can create your Tesseron connection.

1. Check user rights
With this connector you will be able to perform asset actions within your Tesseron instance. Therefore you need to have the mandatory user rights.

2. Apply for your API key
Currently, API keys can only be created by your system administrator. Therefore, request your API key from your system administrator.

3. Create a new connection
    - Provide your Tesseron instance URL
    - Enter your Tesseron API key