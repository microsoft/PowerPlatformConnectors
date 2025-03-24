# Azure Communication Services MMS

The Azure Communication Services MMS connector allows to send MMS messages from the Azure Communication Services resources in your subscription.

## Publisher: Microsoft

## Prerequisites

You will need the following to procced:
- A Microsoft Power Apps or Power Automate plan with custom connector feature.
- An Azure subscription.
- An Azure Communication Services resource.
- An Azure Communication Services purchased phone number that is set up to receive MMS messages.

## Obtaining Credentials
1. Set up an Azure Communication Services resource - [Quickstart doc](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp).
2. Purchase a phone number in this resource with the "Send and receive SMS" feature selected.
3. [Access](https://docs.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-azp#access-your-connection-strings-and-service-endpoints) your connection strings and service endpoints.

## Known Issues and Limitations

Does not support Service principal (Azure AD application) Authentication at this time.

## Deployment Instructions

Run the following commands and follow the prompts:

```paconn
paconn create --api-def .\apiDefinition.swagger.json --api-prop .\apiProperties.json --script .\script.csx --icon .\icon.png
```