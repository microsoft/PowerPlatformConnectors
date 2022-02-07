# Tikit [Sample] Connector

Tikit provides a basic ticket management system. Using this API, you can create tickets and files associated with it.Very often, you may want to leverage the business with tickets.This is the solution for it.

# Prerequisites

You will need the following prerequisites to proceed:

* A Microsoft Power Apps or Power Automate plan with custom connector feature.
* An API Key to use with custom connector.
* The Power Platform CLI tools.

# Building the tools

Since Tikit is a ticket management system and is secured by api key do first of all you need an api key to interact with it.You will find more information about this [here](https://tikit/azurewebsites.net) and follow the steps below:

1. Get your API Key by registering yourself on [official](https://tikit.azurewebsites.net) website as an user.
2. Setup power automate and power apps for use with the tikit api.

At this point you will be able to access the connector with the api key previously acquired by the website.

# Supported operations

The connector supports the following operations:

* File: This let you add files to the previously added tickets.
    * AddFile: Add file to the specified ticket.
    * GetFile: Get file with a specified file id.
    * GetFiles: Get all the files with the help of apikey previously acquired.
    * UpdateFile: Update the ticket file with the help of unique fileId.

* Ticket: This lets you add tickets in the database.
    * GetTickets: Gets all the tickets.
    * AddTicket: Add ticket to the database.
    * UpdateTicket: Update the ticket with the help of unique ticket id.
    * GetTicket: Get ticket with the help of unique id of the ticket.