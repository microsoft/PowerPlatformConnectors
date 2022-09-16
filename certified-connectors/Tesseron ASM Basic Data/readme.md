## Tesseron ASM Ticket Connector
With Tesseron ASM, we offer your company an innovative solution for cooperation with your customers, partners and suppliers.This connector allows you to use Tesseron ASM Ticket API to create, edit and search Tickets.

## Publisher: Publisher's Name
Lutihle + Luithle GmbH

## Prerequisites
You will need the following to proceed:
* A Tesseron ASM Instance
* A Tesseron ASM licensed user
* An API Key (Service: Ticket)

### Set up a Tesseron ASM instance and user for your custom connector
Since Tesseron ASM Rest API uses API keys to validate your user, you first need to contact your system administrator to create an API key for your user. After that is completed you can create your Tesseron ASM connection.

1. Check user rights
With this connector you will be able to perform basic data actions within your Tesseron ASM instance. Therefore you need to have the mandatory user rights in the needed entities(at least editor).

2. Apply for your API key
Currently, API keys can only be created by your system administrator. Therefore, request your API key from your system administrator.

3. Create a new connection
    - Provide your Tesseron ASM instance URL
    - Enter your Tesseron ASM API key

```

## Supported Operations
The connector supports the following operations:
* `Search Enterprise`: Receive an enterprise ID by providing a business partner ID
* `Get Enterprise`: Receive all enterprise information by providing a business partner ID
* `Get Enterprise by Parameter`: Receive all enterprise information with a query defined by parameters
* `Create Enterprise`: Create an enterprise in your Tesseron ASM instance
* `Update Enterprise`: Update an enterprise in your Tesseron ASM instance
* `Create Contact`: Create a contact for an existing enterprise in your Tesseron ASM instance
* `Get Contact by Parameter`: Receive all contact information with a query defined by parameters

## Obtaining Credentials
Authentication is done via an API key. Please provide your API key and your instance URL als connection parameters.


## Known Issues and Limitations
* Sufficient user rights are mandatory to receive ticket information in specific areas
* If the ticket creation is triggered multiple time at once there may be an incomplete ticket creation


## Deployment Instructions
Since Tesseron ASM Rest API uses API keys to validate your user, you first need to contact your system administrator to create an API key for your user. After that is completed you can create your Tesseron ASM connection.

1. Check user rights
With this connector you will be able to perform ticket actions within your Tesseron ASM instance. Therefore you need to have the mandatory user rights in the needed areas (at least editor).

2. Apply for your API key
Currently, API keys can only be created by your system administrator. Therefore, request your API key from your system administrator.

3. Create a new connection
    - Provide your Tesseron ASM instance URL
    - Enter your Tesseron ASM API key



