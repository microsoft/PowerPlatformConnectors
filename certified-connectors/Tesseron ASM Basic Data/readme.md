## Tesseron Basic Data Connector
With Tesseron, we offer your company an innovative solution for cooperation with your customers, partners and suppliers.This connector allows you to use Tesseron Basic Data API to create, edit and search business partners and contacts.

## Publisher: Publisher's Name
Lutihle + Luithle GmbH

## Prerequisites
You will need the following to proceed:
* A Tesseron Instance
* A Tesseron licensed user
* An API Key (Service: Basic Data)

### Set up a Tesseron instance and user for your custom connector
Since Tesseron Rest API uses API keys to validate your user, you first need to contact your system administrator to create an API key for your user. After that is completed you can create your Tesseron connection.

1. Check user rights
With this connector you will be able to perform basic data actions within your Tesseron instance. Therefore you need to have the mandatory user rights in the needed entities(at least editor).

2. Apply for your API key
Currently, API keys can only be created by your system administrator. Therefore, request your API key from your system administrator.

3. Create a new connection
    - Provide your Tesseron instance URL
    - Enter your Tesseron API key

```

## Supported Operations
The connector supports the following operations:
* `Search Enterprise`: Receive an enterprise ID by providing a business partner ID
* `Get Enterprise`: Receive all enterprise information by providing a business partner ID
* `Get Enterprise by Parameter`: Receive all enterprise information with a query defined by parameters
* `Create Enterprise`: Create an enterprise in your Tesseron instance
* `Update Enterprise`: Update an enterprise in your Tesseron instance
* `Create Contact`: Create a contact for an existing enterprise in your Tesseron instance
* `Get Contact by Parameter`: Receive all contact information with a query defined by parameters

## Obtaining Credentials
Authentication is done via an API key. Please provide your API key and your instance URL als connection parameters.


## Known Issues and Limitations
* Sufficient user rights are mandatory to receive ticket information in specific areas
* If the ticket creation is triggered multiple time at once there may be an incomplete ticket creation


## Deployment Instructions
Since Tesseron Rest API uses API keys to validate your user, you first need to contact your system administrator to create an API key for your user. After that is completed you can create your Tesseron connection.

1. Check user rights
With this connector you will be able to perform ticket actions within your Tesseron instance. Therefore you need to have the mandatory user rights in the needed areas (at least editor).

2. Apply for your API key
Currently, API keys can only be created by your system administrator. Therefore, request your API key from your system administrator.

3. Create a new connection
    - Provide your Tesseron instance URL
    - Enter your Tesseron API key



