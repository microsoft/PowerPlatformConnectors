# Title
The Partner Center API helps Cloud Solution Provider (CSP) partners integrate their existing CRM or billing software with the Microsoft systems that manage customer accounts, place orders, manage subscriptions, and handle support requests.

## Publisher: Publisher's Name
Oleksii Skirko | Innoware

## Prerequisites
You will need the following to proceed:

- A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
- Access to Microsoft Partner Center with appropriate permissions to call API functions
- The Power platform CLI tools
- AAD Application

Since the APIs used by the connector are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD for connector to securely access them. After this setup, you can create and test the connector. 

### Azure AD Application
Since the connector uses OAuth as authentication type, we first need to register an application in Azure AD. This application will be used to get the authorization token required to invoke rest APIs used by the connector on user's behalf. You can read more about this [here](https://docs.microsoft.com/en-us/partner/develop/api-authentication#application-and-user-access). Please use **Application and user access section** in this link. For redirect URI, use "https://global.consent.azure-apim.net/redirect". For the credentials, use a client secret (and not certificates). **Remember to note the secret down**, you will need this later and it is shown only once.

## Supported Operations
Required. Describe actions, triggers, and other endpoints.​
### Operation 1
Description of operation 1.

### Operation 2
Description of operation 2.

## Obtaining Credentials
Required. Explain the authentication method and how to get the credentials.​

## Getting Started
Optional. How to get started with your connector.

## Known Issues and Limitations
Required. Include any known issues and limitations a user may encounter.

## Frequently Asked Questions
Optional. Include frequently asked questions by your customer.
### Question 1
Answer to question 1
### Question 2
Answer to question 2

## Deployment Instructions
Optional. Add instructions on how to deploy this connector as custom connector.
