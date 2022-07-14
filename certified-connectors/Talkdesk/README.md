# Talkdesk Connector
The Talkdesk connector provides a better way for organizations to intelligently unlock the promise and potential of great customer experience with end-to-end cloud solutions. Connect to other external systems already supported by Microsoft(within Power Apps and Power Automate) to be able to execute actions in Talkdesk like create an agent, create a callback, get a user by email, etc. or trigger your own flows when a contact is created/updated, note is created, inbound call starts, etc. in Talkdesk.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The [Power platform CLI tools](https://docs.microsoft.com/connectors/custom-connectors/paconn-cli)
* Access to a Talkdesk account and a Talkdesk Client Id and Client Secret. If you dont have access, please contact our sales team(https://talkdesk.com).
 

### Set up an endpoint in Talkdesk for clients to access the instance
We need to create an OAuth application endpoint in the Talkdesk instance. This will allow for external client applications to access our Talkdesk instance. 

#### NOTE
> Role required: admin

You can follow the steps below:

1. In your Talkdesk instance, Navigate to Builder > OAuth Clients and then click New OAuth Client.
2. Fill the form with the following details:
	 - For OAuth client name, use a unique name that identifies the application that you require OAuth access for. 
	 - For Grant type check Refresh token and Authorization code
	 - In Redirect URI's please add "https://global.consent.azure-apim.net/redirect"
	 - Add the following scopes: `contacts:read agents-bulk:write callback:write notifications:write industries-activity:write users:read presence-user:read account-custom-status:read ccaas-user-status:write webhooks-trigger:write webhooks-schema:read account:read presence-account:read`
	 - Client Id and Client Secret will be automatically generated when you click the Save button
	 - Save this `Client ID` to be used in apiProperties.json file in later steps and save this `Client Secret` to use it in later steps while deploying the connector.

### Update Talkdesk instance details and Client ID
#### Update instance details
1. Go to your Talkdesk instance, copy the account name. 
2. Replace `YourTalkdeskAccount` in 
    - apiProperties.json file - under authorizationURL, tokenURL and refreshURL
    - apiDefinitions.swagger.json file - authorizationURL and tokenURL.  

#### Update Client ID
Replace `YourTalkdeskClientId` in apiProperties.json file by using the Client ID value noted before when creating the OAuth client in Talkdesk. 
	 
## Deploying the connector
Run the following commands and follow the prompts:

```paconn
paconn create -e [Power Platform Environment GUID] --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json] --icon [Path to icon.png] --secret [The OAuth2 client secret for the connector]
```

## Supported Actions
The connector supports the following actions:
* `Get contact by Id`: Get contact by Id from Talkdesk
* `Create a new agent`: Create a new agent in Talkdesk
* `Create a callback`: Create a new callback in Talkdesk
* `Create a workspace notification`: Create a new workspace notification in Talkdesk
* `Get user by email`: Get user by email from Talkdesk
* `Get account configured custom status used by agents`: Get account configured custom status used by agents in Talkdesk
* `Get user presence`: Get user presence in Talkdesk
* `Update user status`: Update user status in Talkdesk. This action has to be used after the SubscribePresenceForAllUsers action
* `Unsubscribe webhook`: Unsubscribe webhook when trigger is deleted
* `Suscribe presence for all users`: Subscribe to the presence of all users from the authenticated account. This actions has to be used before UpdateUserStatus action

## Supported Triggers
The connector supports the following triggers:
* `When a contact is created`: When a contact is created in Talkdesk
* `When a contact is updated`: When a contact is updated in Talkdesk
* `When a note is created`: When a note is created in Talkdesk
* `When agent logs in`: When agent logs in Talkdesk
* `When agent logs out`: When agent logs out Talkdesk
* `When outbound call ends`: When outbound call ends in Talkdesk
* `When inbound call reaches contact center`: When inbound call reaches contact center in Talkdesk
* `When inbound call ends`: When inbound call ends in Talkdesk
* `When inbound call starts`: When inbound call starts in Talkdesk

