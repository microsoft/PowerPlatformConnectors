## TALXIS Data Feed Connector
TALXIS Data Feed is a collection of different data sources. You can get company's business data, format address, or get public holidays for example. Use this connector to improve your data quality.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* TALXIS license

You will not be able to use this connector without TALXIS license. Contact NETWORG if you wish to get this license.

### Set up Azure AD application
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access TALXIS Data Feed API on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Approve 'TALXIS - Data Feed - Flow' Azure AD application
This Azure AD application will be used to identify the connector against your Azure AD. [Approve the application registration in your tenant.](https://login.microsoftonline.com/common/adminconsent?client_id=28d529aa-b85e-4469-9cf3-937bea582555)