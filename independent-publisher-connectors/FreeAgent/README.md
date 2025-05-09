# FreeAgent

FreeAgent is a cloud based accounting system. It brings everything together, from invoice and expense management to project management and sales tax. FreeAgent made specifically for freelancers, small business owners and their accountants. This Connector helps you to retrieve and update data in your FreeAgent Account. This connectors supports only core operations dealing with Invoice, Recurring Invoice and Contacts.

 
## Publisher: Nirmal

## Prerequisites

To use this connector, you need the following

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- You need to have an FreeAgent account. You can [sign up](https://signup.freeagent.com/signup) for free.
- A FreeAgent developer account with a configured OAuth 2.0 application. https://dev.freeagent.com/

## Obtaining Credentials
- Create a New Developer Account here : https://dev.freeagent.com/signup

- Create a New App
- Update the App OAuth Redirect URI's to "	
https://global.consent.azure-apim.net/redirect"
- Get the OAuth identifier (ClientID),OAuth secret (Client Secret)
- Deploy the connector using the oAuth Credentials.



### Supported Operations
Freeagent API has more than 40 operations. Currentlt this connector the most important operations related to Invoice, Contact and Recurring Invoices.

#### 1. Contact
- List all contacts : Retrieves list of Contacts 
- Create Contact : Creates a New Contact
- Delete Contact : Delete a Contact based on Contact Url
- Update Contact : Update a Contact based on Contact Url
- Get a single contact : Retrieve a Contact by Contact Url.


####  2. Invoice
- Create Invoice : Creates a New Invoice
- List Invoices : List all Invoices
- Delete Invoice : Delete a Invoice
- Show Invoice : Retrieve a Invoice based on Invoice Url
- Update Invoice : Update a Invoice based on Invoice Url

#### 3. Invoice Flag

- Mark invoice as sent : Update a Invoice with Status Sent
- Mark invoice as cancelled : Update a Invoice Status as Cancelled.
- Mark invoice as scheduled : Update a Invoice as Scheduled
- Mark invoice as draft : Update a Invoice as Draft


#### 4. Recurring Invoice
- List all recurring invoices : Retrieves the list of recurring invoices
- Show Recurring Invoice : Retrieve details about a recurring invoice

Note : The API doesn't support creating/updating/deleting Recurring Invoices.



## Known Issues and Limitations
There are no known issues at time of publishing.

## Rate Limits
FreeAgent API has some usage rate limits. Please refer the "Rate Limits" Section in this page https://dev.freeagent.com/docs/introduction


## Deployment Instructions
Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).

