# NBG ERP Solution 
This is a connector of the National Bank of Greece,providing access to ERP Bank Connection API.
Using this API, clients are provided with full control of their NBG finance in one place and increase their satisfaction. 
Connect to a user’s bank account or card and perform money transfers

## Publisher: National Bank of Greece


## Prerequisites
In order to use this connector you will need to perform the below actions:
1. While on Power Apps Custom Connector Security tab, "Refresh URL" must be filled in with the Token URL.
2. Client-ID, Client-Secret for the connector connection to be filled in on the Security tab of the Connector.
3. User Credentials are necessary to connect to the National Bank of Greece using OAuth2.
4. Redirect URL https://global.consent.azure-apim.net/redirect must be filled  in  "Oauth Redirect uri(s)" of your Organizations environment in NBG developers portal (https://developer.nbg.gr/organizations).

## Supported Operations
The connector supports the following operations:
1.  View their account details, balances and statements
2.  View their card details, balances and statements
3.  Send money to accounts or pay cards.

### NBG User Accounts
It returns all the user's accounts and their details.
The account should belong to the user (userId) who is asking the question. The account should be 11 digits, the format that the NBG uses to describe an account.

### NBG User Cards
Fetch information for user's cards or get details for a specific card.

### Transfer Process
Perform a transfer of an amount to an account and calculate relative expenses, or execute a payment to an NBG card

## Obtaining Credentials
Use your National Bank of Greece (NBG) e-banking credentials to connect using Oauth2 Authentication. Make sure that you have access to necessary Client Id and Client Secret values.

## Known Issues and Limitations
This connector is not able to transfer money from bank accounts outside of National Bank of Greece accounts.
Make sure that you are referring to your NBG account or cards when using this connector, to get details of existing accounts or cards.

### Deployment Instructions
Run the following commands and follow the prompts after successful Power Apps login:


1. paconn login
2. paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>.
3. Login to Power Apps to create a new connection.







