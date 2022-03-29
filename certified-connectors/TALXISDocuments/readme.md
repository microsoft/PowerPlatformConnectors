## TALXIS Documents Connector
TALXIS Documents helps you with creation of documents from templates. You can can fill out template Excel file with liquid tags.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* TALXIS Documents license

You will not be able to use this connector without TALXIS Documents license. Contact NETWORG if you wish to get this license.

### Set up Azure AD application
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access TALXIS Documents API on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Approve 'TALXIS - Documents - Flow' Azure AD application
This Azure AD application will be used to identify the connector against your Azure AD. [Approve the application registration in your tenant.](https://login.microsoftonline.com/common/adminconsent?client_id=9e11c855-6c8f-46b1-8608-ba2ce87ee92d)