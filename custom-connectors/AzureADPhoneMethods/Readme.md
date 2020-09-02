
## Azure AD phone methods Connector
With this connector you can manage phone authentication methods to Azure Active Directory. This methods can be used for Azure MFA and Self Service Password Reset



## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature

## Building the connector 
Since the Azure AD authentication methods APIs are secured by Azure Active Directory (AD), we first need to set up a few things in Azure AD so that our connectors can securely access the phone methods. After that is completed, you can create and test the sample connector.

Check out [this blog post](https://janbakker.tech/prepopulate-phone-methods-using-a-custom-connector-in-power-automate/) for a step-by-step tutorial.

### Set up an Azure AD application for your custom connector
We first need to register our connector as an application in Azure AD.  This will allow the connector to identify itself to Azure AD so that it can ask for permissions to access Key Vault data on behalf of the end user.  You can read more about this [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/authentication-scenarios) and follow the steps below:

1. Create an Azure AD application
This Azure AD application will be used to identify the connector to the Microsoft Graph API.  This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access the Azure Key Vault API
This step will ensure that your application can successfully retrieve an access token to invoke Azure Key Vault on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use “https://global.consent.azure-apim.net/redirect”
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, Graph API **delegated** permissions **UserAuthenticationMethod.ReadWrite.All** needs to be added to the app registration. Don't forget to grant admin access.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Azure AD authentication methods.  The next step for us is to create a custom connector.

### Create the custom connector
You can install the connector using the paconn CLI tool, or manually upload the file to Power Automate.

#### Using paconn
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

#### Manual upload
Use the [Power Automate Portal](https://flow.microsoft.com) to create a new custom connector. Use **Import an OpenAPI file** and select the apiDefinition.swagger.json file.

### Connector Security setup

Use the following values for the the Oath 2.0 security page:

* `Authentication type` : OAuth 2.0
* `Client id`: the application ID from the app registration
* `Client secret` the secret from the app registration
* `Tenant ID`: common
* `Resource URL` : https://graph.microsoft.com


## Supported Operations
The connector supports the following operations:
* `List phoneMethods`: Retrieve a list of phone authentication method objects. This will return up to three objects, as a user can have up to three phones usable for authentication.
* `Create phoneAuthenticationMethod`: Add a new phone authentication method. A user may only have one phone of each type, captured in the phoneType property. This means, for example, adding a mobile phone to a user with a preexisting mobile phone will fail. Additionally, a user must always have a mobile phone before adding an alternateMobile phone.  Adding a phone number makes it available for use in both Azure multi-factor authentication (MFA) and self-service password reset (SSPR), if enabled.  Additionally, if a user is enabled by policy to use SMS sign-in and a mobile number is added, the system will attempt to register the number for use in that system.
* `Enable Sms SignIn`: Enable SMS sign-in for an existing mobile phone number. To be successfully enabled:  The phone must have \"phoneType\": \"mobile\". The phone must be unique in the SMS sign-in system (no one else can also be using that number). The user must be enabled for SMS sign-in in the authentication methods policy.
* `Disable Sms SignIn`: Disable SMS sign-in for an existing mobile phone number.  Note: The number will no longer be available for SMS sign-in, which can prevent your user from signing in.
* `Get phoneMethod`: Retrieve a single phoneAuthenticationMethod object.
* `Delete phoneAuthenticationMethod`: Delete a user's phone authentication method.  Note: This removes the phone number from the user and they will no longer be able to use the number for authentication, whether via SMS or voice calls.  Remember that a user cannot have an alternateMobile number without a mobile number. If you want to remove a mobile number from a user that also has an alternateMobile number, first update the mobile number to the new number, then delete the alternateMobile number.  If the phone number is the user's default Azure multi-factor authentication (MFA) authentication method, it cannot be deleted. Have the user change their default authentication method, and then delete the number.
* `Update phoneAuthenticationMethod`: Update the phone number associated with a phone authentication method.  You can't change a phone's type. To change a phone's type, add a new number of the desired type and then delete the object with the original type.  If a user is enabled by policy to use SMS to sign in and the mobile number is changed, the system will attempt to register the number for use in that system.

## Terminology


* `UserID` : the ID or UPN of the Azure AD user
* `PhoneType` : the type of the phone number. `mobile`,`AlternateMobile`, or `office`
* `PhonemethodID` : this is the global ID for the different methods:
    * 3179e48a-750b-4051-897c-87b9720928f7 (mobile)
    * b6332ec1-7057-4abe-9331-3d72feddfe41 (alternate mobile)
    * e37fc753-ff3b-4958-9484-eaa9425c82bc (office)
