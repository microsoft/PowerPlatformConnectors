
### NOTE
> This is a connector of the National Bank of Greece,providing access to ERP Bank Connection API.


## Prerequisites
In order to use this connector you will need to perform the below actions:
* While on Power Apps Custom Connector Security tab, "Refresh URL" must be filled in with the Token URL.
* Client-ID, Client-Secret for the connector connection to be filled in on the Security tab of the Connector.
* User Credentials are necessary to connect to the National Bank of Greece using OAuth2.
* Redirect URL https://global.consent.azure-apim.net/redirect must be filled  in  "Oauth Redirect uri(s)" of your Organizations environment in NBG developers portal (https://developer.nbg.gr/organizations).

### Deploying the sample
Run the following commands and follow the prompts after sucessfull login:

```paconn
paconn login
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
1.  View their account details, balances and statements
2.  View their card details, balances and statements
3.  Send money to accounts



