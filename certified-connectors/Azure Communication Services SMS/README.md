# Azure Communication Services SMS

The Azure Communication Services SMS connector enables users to send SMS text messages as well as list all phone numbers in your Azure Communication Services resource.

## Publisher: Microsoft

## Prerequisites

You will need the following to proceed:

* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* An Azure Communication Services resource

## Supported Operations

### Send SMS (deprecated)

Sends SMS using the phone numbers in your Azure Communication Services (deprecated).

### Send SMS

Sends SMS using the phone numbers in your Azure Communication Services.

## Obtaining Credentials

1. Set up an Azure Communication Services resource
   
   - [Quickstart doc](https://docs.microsoft.com/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp).

2. Obtain credentials
   
   - Option 1: Connection using Connection String Authentication
     
       You can create a new connection using an [Azure Communication Services resource connection string](https://docs.microsoft.com/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp#access-your-connection-strings-and-service-endpoints).
   
   - Option 2: Connection using Service Principal (Azure AD application) Authentication
     
       You can also set up a service principal, create a registered application from [the Azure CLI](https://learn.microsoft.com/azure/communication-services/quickstarts/identity/service-principal-from-cli) or [through the Azure portal](https://learn.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal). Then, the endpoint and credentials can be used to authenticate.

## Known Issues and Limitations

No known issues and limitations at this time.

## Deployment Instructions

Run the following commands and follow the prompts:

```paconn
paconn create --api-def .\apiDefinition.swagger.json --api-prop .\apiProperties.json --script .\script.csx --icon .\icon.png
```