# Microsoft Partner Center
The Partner Center API helps Cloud Solution Provider (CSP) partners integrate their existing CRM or billing software with the Microsoft systems that manage customer accounts, place orders, manage subscriptions, and handle support requests.

## Publisher: Oleksii Skirko | Innoware

## Prerequisites
You will need the following to proceed:

- A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
- Access to Microsoft Partner Center with appropriate permissions to call API functions
- The Power platform CLI tools
- AAD Application

Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD for connector to securely access them. After this setup, you can create and test the connector. 

### Azure AD Application
Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD. This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf. You can read more about this [here](https://docs.microsoft.com/en-us/partner/develop/api-authentication#application-and-user-access). Please use **Application and user access section** in this link. For redirect URI, use "https://global.consent.azure-apim.net/redirect". For the credentials, use a client secret (and not certificates). **Remember to note the secret down**, you will need this later and it is shown only once.

### Install paconn
- Install Python 3.5+ from [https://www.python.org/downloads](Python downloads). Select the Download link on any version of Python greater than Python 3.5. For Linux and macOS X, follow the appropriate link on the page. You can also install using an OS-specific package manager of your choice.
- Run the installer to begin installation and be sure to check the box 'Add Python X.X to PATH'.
- Make sure the installation path is in the PATH variable by running:
`python --version`
- After python is installed, install `paconn` by running:
`pip install paconn`\
If you get errors saying 'Access is denied', consider using the `--user` option or running the command as an Administrator (Windows).
You can find more details regarding `paconn` by following this [link](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) 

## Supported Operations
The connector currently supports the following operations:​
### [Get Invoices](https://docs.microsoft.com/en-us/partner-center/develop/get-a-collection-of-invoices)
Retrieves a collection of the partner's invoices.

### [Get Customers](https://docs.microsoft.com/en-us/partner-center/develop/get-a-list-of-customers)
Gets a collection of resources that represents all of a partner's customers.

### [Get Subscriptions](https://docs.microsoft.com/en-us/partner-center/develop/get-all-of-a-customer-s-subscriptions)
Gets a collection of a customer's subscriptions.

### [Get customer orders](https://docs.microsoft.com/en-us/partner-center/develop/get-all-of-a-customer-s-orders)
Gets a collection of all the orders for a specified customer.

### [Get customer details](https://docs.microsoft.com/en-us/partner-center/develop/get-a-customer-by-id)
Gets customer's details from Business and Company profiles

## Deployment Instructions
Add your Application ID to clientId property of `apiProperties.json` file.\
Run the following commands and follow the prompts:\
`paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>`

