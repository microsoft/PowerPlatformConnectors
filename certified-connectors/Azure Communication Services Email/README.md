# Azure Communication Services Email

Azure Communication Services Email is a new primitive that facilitates high volume transactional, bulk and marketing emails on the Azure Communication Services platform and will enable Application-to-Person (A2P) use cases. You can read more about Email in Azure Communication Services [here](https://docs.microsoft.com/en-us/azure/communication-services/concepts/email/email-overview).

## Publisher: Microsoft

## Prerequisites

You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* An Azure Communication Services Email resource with a configured domain
* An Azure Communication Services resource connected with an Azure Email domain

## Supported Operations

### Send Email 

Sends an email message to one or more recipients.

### Get Email Message Status

Gets the status of an email message sent previously.

## Obtaining Credentials
1. Set up an Azure Communication Services resource
    - [Quickstart doc](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp)

2. Set up an Azure Commuincation Services Email resource
    1. Create an Email resource
        - [Quickstart doc](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/email/create-email-communication-resource)
    2. Configure a domain
        - [Add Azure Managed domains](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/email/add-azure-managed-domains)
        - OR [Add custom domains](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/email/add-custom-verified-domains)

3. Connect Email Domain with Communication Services resource
    - [Quickstart doc](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/email/connect-email-communication-resource)

4. Connection String Authentication
    - You can create a new connection using an [Azure Communication Services resource connection string](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp#access-your-connection-strings-and-service-endpoints).

## Known Issues and Limitations
Does not support Service principal (Azure AD application) Authentication at this time.

## Deployment Instructions

Run the following commands and follow the prompts:

```paconn
paconn create --api-def .\apiDefinition.swagger.json --api-prop .\apiProperties.json --script .\script.csx --icon .\icon.png
```

