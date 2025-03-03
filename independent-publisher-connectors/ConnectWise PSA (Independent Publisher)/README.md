# ConnectWise PSA (Independent Publisher)
ConnectWise is a leading IT management software that provides solutions for managed service providers (MSPs) and technology professionals. This connector aims to streamline access to ConnectWise APIs, enabling users to integrate and automate key business processes such as ticketing, company and contact details.  As we build more endpoints to the connector, we can then allow this connector to have more functionality.

## Publisher: Chris Howell, Opal Business Solutions

## Prerequisites
- API Member Public and Private Key
- Knowledge of the Company Code
- ClientID from ConnectWise Developer

## Supported Operations
- **Get Tickets:** Retrieve a list of service tickets.
- **Create Ticket:** Add a new service ticket.
- **Update Ticket:** Modify an existing service ticket.
- **Get Companies:** Retrieve company details.
- **Get Contacts:** Retrieve contact details.

## Operation 1
Get Tickets allows for the retrieving of data from the service tickets

## Operation 2
Create Tickets allows for the creation of a new service ticket

## Operation 3
Update Ticket allows for the updating of an existing service ticket

## Operation 4
Get Companies allows for the retrieving of data from the company

## Operation 5
Get Contacts allows for the retrieving of data from the contact

## Obtaining Credentials
API Key is retrieved through ConnectWise Setup, Members and an API Member needs to be created.  Saving the Public and Private Key is important
Company Code is required, which you can obtain when logging into ConnectWise
ClientID is obtained through (https://developer.connectwise.com/)
Once the API Key is obtained, and the Company Code is documented, you will need to convert the following to Base64
  Convert: Company Code+Public Key:Private Key to Base64

## Known Issues and Limitations
Currently there are no limitations, and issues outside any limitations from the ConnectWise API side.






