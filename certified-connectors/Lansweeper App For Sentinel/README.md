# Lansweeper App For Sentinel
The Lansweeper App for MSFT Sentinel allows SOC team members to leverage the Lansweeper capabilities of IT Asset discovery and identification, allowing it to be automated via the Microsoft Logic App.This integration implements the investigative actions for the Lansweeper app on the MS Sentinel Platform. 

It will allow end-users to implement any use cases on the Lansweeper Platform that are possible using a combination of the below-mentioned actions.

**Authentication:** Create a Logic app with a custom connector. Authenticate the connection with Lansweeper APIs using the API Key from Logic app.  
**List Authorized Sites:** Retrieve the list of the authorized sites.  
**Hunt IP:** Get the asset details from the Lansweeper platform for the given Site ID and IP address.  
**Hunt MAC:** Provide the asset details from the Lansweeper platform for the given Site ID and MAC address  

## Pre-requisites
You will need the following to proceed:  
- An Azure subscription
- A Microsoft Power Apps or Power Automate or Logic Apps plan with custom connector feature
- The Power platform CLI tools
- Lansweeper API credentials

## Supported Operations
The connector supports the following operations:
- `Get Asset details using Graph-QL:` Retrieve the list of authorized sites and the asset details from Lansweeper platform using IP address or MAC address.  

## How to get Authorization Token for Lansweeper API connection

Refer Personal Application to get [Lansweeper API Authorization Token](HTTPS://DOCS.LANSWEEPER.COM/DOCS/API/AUTHENTICATE#PERSONAL-APPLICATION)

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps. Additionally, you can leverage this connector within Logic Apps.