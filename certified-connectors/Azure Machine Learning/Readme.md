# Azure Machine Learning
Azure Machine Learning provides a comprehensive REST CLI for interacting with Azure Machine Learning Workspaces. This is an attempt to make the use of these APIs easier by exposing these as a connector on the Logic Apps and Power Platforms. This also enables users to leverage these platforms to perform extensive automation tasks without going through the hassle of setting up the CLI by providing UI elements for the same.

## Publisher: Microsoft ​

## Prerequisites
You will need the following to proceed:

* A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools

## Supported Operations

### BatchEndpointsList
List BatchEndpoints in an Azure ML workspace

### JobsCreateOrUpdate
Create or update jobs

Other supported operations can be found [here](https://github.com/Azure/azure-rest-api-specs/tree/main/specification/machinelearningservices/resource-manager/Microsoft.MachineLearningServices/stable/2022-05-01)

## Obtaining Credentials
The connector uses OAuth for authentication. For details on its configuration have a look at the [Deployment Instructions](#deployment-instructions).

## Known Issues and Limitations
No known issues or limitations

## Deployment Instructions
1. Create an Azure AD application
This can be done using [Azure Portal] (https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).  Once created, note down the value of Application (Client) ID.  You will need this later.

2. Configure (Update) your Azure AD application to access the Azure Active Directory API
This step will ensure that your application can successfully retrieve an access token to invoke Azure Active Directory rest APIs on behalf of your users.  To do this, follow the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis).
    - For redirect URI, use "https://global.consent.azure-apim.net/redirect"
    - For the credentials, use a client secret (and not certificates).  Remember to note the secret down, you will need this later and it is shown only once.
    - For API permissions, make sure "Azure Active Directory" and "user_impersonation" are added.
   
At this point, we now have a valid Azure AD application that can be used to get permissions from end users and access Azure Active Directory.  The next step for us is to create a custom connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```