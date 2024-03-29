## Tesseron Ticket Connector
With Tesseron, we offer your company an innovative solution for cooperation with your customers, partners and suppliers.This connector allows you to use Tesseron Ticket API to create, edit and search Tickets.

## Publisher: Publisher's Name
Lutihle + Luithle GmbH


## Prerequisites
You will need the following to proceed:
* A Tesseron Instance
* A Tesseron licensed user
* An API Key (Service: Ticket)


## Supported Operations
The connector supports the following operations:
* `Create Ticket`: Create a ticket in your Tesseron instance
* `Create Ticket Position`: Add a new position to your ticket (info, end report, reopen)
* `Get Ticket`: Receive all ticket information with a ticket or reference number
* `Search Ticket`: Receive a ticket number by providing a reference number
* `Search Ticket by Parameter`: Receive ticket information by providing a search query


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