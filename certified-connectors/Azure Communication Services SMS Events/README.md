# Azure Communication Services SMS Events

The Azure Communication Services SMS Events connector enables the processing of SMS messages sent to Azure Communication Services phone numbers.

## Publisher: Microsoft

## Prerequisites

You will need the following to proceed:

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- An Azure subscription with the Event Grid resource provider registered
- An Azure Communication Services resource
- An Azure Communication Services purchased phone number that is set up to receive SMS messages

## Obtaining Credentials

1. Create an Azure Communication Services resource - [Quickstart doc](https://docs.microsoft.com/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp).
2. Purchase a phone number in this resource with the "Send and receive SMS" feature selected.
3. [Register an Event Grid resource provider](https://learn.microsoft.com/azure/communication-services/quickstarts/sms/handle-sms-events#register-an-event-grid-resource-provider) for the subscription the Azure Communication Services resource is contained in.
4. Create an Azure Active Directory (AAD) user or Service Principal that can be used for the connector connection.
   - Option 1: Connection using AAD account - [Add a new user](https://learn.microsoft.com/azure/active-directory/fundamentals/add-users-azure-active-directory#add-a-new-user)
   - Option 2: Connection using Service Principal (AAD application) Authentication - You can also set up a service principal, create a registered application from [the Azure CLI](https://learn.microsoft.com/azure/communication-services/quickstarts/identity/service-principal-from-cli) or [through the Azure portal](https://learn.microsoft.com/azure/active-directory/develop/howto-create-service-principal-portal). The credentials can then be used to authenticate.
5. Ensure the AAD user or service principal has the following permissions assigned to them:

   - Microsoft.Communication/CommunicationServices/Read
   - Microsoft.Resources/subscriptions/resourceGroups/read
   - Microsoft.EventGrid/eventSubscriptions/write
   - Microsoft.EventGrid/eventSubscriptions/read
   - Microsoft.EventGrid/eventSubscriptions/delete

    This can be achieved by creating a custom role or assigning the user to the following built-in roles:

   - Reader - At the Azure Communication Services resource level 
   - EventGrid EventSubscription Contributor - At the resource group level

## Get started with your connector

This connector contains a single trigger **When an SMS message is received**. By default it will run when a message is sent to any of your purchased phone numbers, but it can also be configured to only run based on the "To" and "From" phone numbers of the SMS message.

## Deployment Instructions

Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --script script.csx --icon icon.png
```

## Known issues and limitations

* Trigger temporarily disabled on update if "Event Subscription Name" set - If a value was assigned to the "Event Subscription Name" field when creating the trigger, any updates to the trigger (for example, adding a "To" phone number) can take up to 10 minutes to be applied. During this time the trigger will not function.
* Event Subscription not deleted if Connection deleted before Flow.

## Common errors and remedies

No known common errors.

## FAQ

No FAQ