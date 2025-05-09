# Lansweeper App For Sentinel
The Lansweeper connector for Sentinel allow users to retrieve the list of sites-ids, and asset details from the Lansweeper platform for the given site-id and IP address or MAC address by using a valid GraphQL query and valid Lansweeper API Authorization Token.  

## Publisher
Lansweeper

## Pre-requisites
You will need the following to proceed:  
- An Azure subscription
- A Microsoft Power Apps or Power Automate or Logic Apps plan with custom connector feature
- The Power platform CLI tools
- Lansweeper API credentials

## Supported Operations
The connector supports the following operations:
- `Get Asset details using GraphQL:` Retrieve the list of authorized sites and the asset details from Lansweeper platform using IP address or MAC address.  

## How to get Authorization Token for Lansweeper API connection

Refer Personal Application to get [Lansweeper API Authorization Token](HTTPS://DOCS.LANSWEEPER.COM/DOCS/API/AUTHENTICATE#PERSONAL-APPLICATION)

## Known Issues and Limitations
Currently there are no known issues. The only limitation is that the connector offers a single api which uses GraphQL, hence the user must use different GraphQL queries for performing different operations using the same api provided by this connector.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps. Additionally, you can leverage this connector within Logic Apps.
