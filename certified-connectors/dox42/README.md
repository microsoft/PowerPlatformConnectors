## dox42 connector
dox42 provides a powerful and very extensive REST API. Using dox42 you can automatically generate documents with data from all sources, like Dataverse, SharePoint or others. dox42 uses Microsoft Entra ID for authentication.


## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* Microsoft Entra ID
* a dox42 license (dox42 Online or dox42 Server + dox42 Word/Excel/PowerPoint Add-ins)
* dox42 Server Designer application (Version 1.0.1.4 or later)

## Building the connector 
Since dox42 uses Microsoft Entra ID authentication, we first need to set up a few things in Microsoft Entra ID so that our connectors can securely generate your documents with dox42.  After that is completed, you can test the connector.

### Set up an Microsoft Entra ID application for your custom connector
We first need to register dox42 as an application in Microsoft Entra ID. This will allow the connector to identify itself to Microsoft Entra ID so that it can ask for permissions to access your data on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Microsoft Entra ID application
This Microsoft Entra ID application will be used to identify the connector to dox42.  This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Microsoft Entra ID application to use the dox42 connector:
This step will ensure that your application can successfully retrieve an access token to invoke dox42 on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, make sure that the necessary permissions like delegated SharePoint permissions and for example “Dynamics CRM user_impersonation” are added.
	- Follow the instructions in chapter 6 of the dox42 Microsoft Entra ID Documentation: https://www.dox42.com/Download/dox42_DynamicsCRM_Documentation_EN.pdf 
   

### At this point, we now have a valid Microsoft Entra ID application that can be used to get permissions from end users and access dox42 document generation.  The next step for us is to modify the premium connector.

- Enter your Azure Active Directory Information in the Security tab of the connector. Follow the Security information in the dox42 Power Automate Documentation for further information: https://www.dox42.com/Download/dox42_MSPowerAutomate_LogicApps.pdf 

### For calling the dox42 document generation, insert a query string from the dox42 Server Designer application, when designing your flow.



## Supported Operations

The connector supports the following operations:

* `dox42_Call_GET`: Make an HTTP GET request to the dox42 service
* `dox42_Call_POST`: Make an HTTP POST request to the dox42 service