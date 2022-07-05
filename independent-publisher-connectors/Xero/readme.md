
## Xero Connector
Xero is an accounting software platform for small and medium sized businesses.

Using this API, you can authorise Power App or Power Automate to work with the Xero API sets.  At present we're implementing:-

* Xero OAuth 2.0 Client Authentication.  This mirrors the existing Xero definitions, so includes the retrieval of invoice information also.

We plan to add the following
* Accounting
* Assets
* Files
* Projects
* Payroll (AU)
* Payroll (UK)
* Payroll (NZ)
* Bankfeeds
* AppStore


## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Xero subscription or trial (a demo company account is sufficient). See https://developer.xero.com/documentation/getting-started-guide/
* A Xero developer account.  See https://developer.xero.com/ 

## Building the connector 
Xero API sets are secured using OAuth 2.0, so we need to authenticate the client first before our connector can securely access Xero.

### Set up an Xero application for your custom connector
We first need to register our connector as an application via the Xero developer account.  You can read more about this here (https://developer.xero.com/documentation/getting-started-guide/).

This will allow the connector to identify itself to Xero so that it can ask for permissions to access Xero data on behalf of the end user.

1. Create a new Xero application
From the Xero developer portal, create a New app.  Once created, make a note of the Client id and Client Secret 1, as you'll need these later.

Ensure that the Redirect URIs includes https://global.consent.azure-apim.net/redirect
  
At this point, we now have a valid Xero application that can be used to get permissions from end users and access Xero.  The next step for us is to create a custom connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Get started': authenticates our connection to Xero tenants
* `Invoices': returns all invoices for a single tenant
* `Refresh token': returns a new token

We plan to add all the API sets, as summarised above, if their is demand for them.
