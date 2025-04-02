## Tesseron Invoice Connector
With Tesseron, we offer your company an innovative solution for cooperation with your customers, partners and suppliers.This connector allows you to use Tesseron Invoice API to create position notes and activity recordings.

## Publisher: Publisher's Name
Lutihle + Luithle GmbH


## Prerequisites
You will need the following to proceed:
* A Tesseron Instance
* A Tesseron licensed user
* An API Key (Service: Invoice)


## Supported Operations
The connector supports the following operations:
* `Create Invoice Position Note`: Create position notes with or without tickets
* `Create Activity Recording`: Add a new activity recordings to your profile
* 'Get Service Assignments (Dispatcher)': Get all service assignments


## Obtaining Credentials
Authentication is done via an API key. Please provide your API key and your instance URL als connection parameters.


## Known Issues and Limitations
* Sufficient user rights are mandatory to receive ticket information in specific areas
* If the ticket creation is triggered multiple times at once there may be an incomplete ticket creation


## Deployment Instructions
Since Tesseron Rest API uses API keys to validate your user, you first need to contact your system administrator to create an API key for your user. After that is completed you can create your Tesseron connection.

1. Check user rights
With this connector you will be able to perform ticket actions within your Tesseron instance. Therefore you need to have the mandatory user rights in the needed areas (at least editor).

2. Apply for your API key
Currently, API keys can only be created by your system administrator. Therefore, request your API key from your system administrator.

3. Create a new connection
    - Provide your Tesseron instance URL
    - Enter your Tesseron API key