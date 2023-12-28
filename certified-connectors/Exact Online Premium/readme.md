# Exact Online Premium connector
The Exact Online Premium connector for Power Automate allows receiving notifications from Exact Online when a business entity record changes. For certain entities, a trigger can be created that responds to newly created or modified records. In addition, the connector can perform some custom formatting, reducing complexity for the user.

## Publisher: Exact Cloud Development Benelux
www.exact.com

## Prerequisites
In order to install the connector, you will need the following:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power Platform CLI tools

In addition, in order to use the connector with Exact Online, the following is required:
* An Exact Online Premium subscription

## Supported operations
### Triggers
The following triggers are available to the user:
* **When an *account* is created or changed**
* **When an *opportunity* is created or changed**
* **When a *project* is created or changed**
* **When a *sales order* is created or changed**

All triggers are set up for one company; upon selecting a trigger, the user is shown a list of available companies.

### Actions

The following actions are available to the user:
* **Get Values**: Takes the `Payload` field from a trigger as input and converts it to a format the user selects. Format options are:
  * ***New Values*** if you want to be able to select specific fields of the new (changed) state in other actions
  * ***Old Values*** if you want to be able to select specific fields of the old (unchanged) state in other actions
  * ***Table*** if you want to get the content as a whole collection for use in other actions, with properties `name`, `oldValue`, `newValue`.

    In addition to the format, the trigger's ***business entity*** needs to be selected. This allows the Flow Designer to load the appropriate schema so the right properties are available to other actions. (at design time, we cannot automatically get the entity type from the selected trigger)

The following actions are for internal use:
* **Get User data**: gets basic info on the logged in user. Used in the connector's script for most other API calls.
* **Get companies for user**: populates the *Company* list in the trigger menu.
* **Get supported entity types**: populates the *Entity Type* list in various actions.
* **List webhook subscriptions**: shows the currently registered subscriptions. Test purposes only.
* **Unsubscribe from event**: automatic sign-out of trigger events when a flow is disabled or deleted.
* **Get schema definition**: loads the right schema for the Get Values action.

## Known issues and limitations

### Empty message for connectivity test
When setting up a new flow, or when turning on an existing flow, a message is sent to Exact Online to register the flow. Exact Online in turn sends a test message (without content) to confirm the connection.

If you don't deal with this message within your flow, the flow may end in error, or send notifications with incomplete content. Since it's usually a one-off behavior, you can choose to accept it, but you may get warnings about failing flows.

A more elegant way to deal with it is to use a *Condition* action to test if the message is empty. The test `empty(triggerOutputs()?['body'])` | `equals` | `true` is a good way to do this. In the *true* branch, add a *Terminate* action (*Control* built-in action) with outcome *Succeeded*. Then continue the actual flow after the Condition action.

## Frequently Asked Questions

### What exactly does the Get Values action do?
*Get Values* performs a couple of actions that would otherwise require multiple more complex actions in your flow. In order, these are:
* It takes the `Payload` field from the trigger and converts its text to a JSON object. This is equivalent to the `json(...)` function;
* It loads a schema for this JSON object so its properties can be referenced in further actions. You could do this with the *Parse JSON* action, but you'd need to supply the entire schema yourself;
* It can convert the incoming payload either to a complete table with all data, or to an object showing the old or new state of the entity. The latter is relatively complex to achieve in a flow.

### What should the value for Payload be in the Get Values action?
By default, this is set to `triggerOutputs()?['body/Content/Payload']` which is the reference to the Payload field received in the trigger. You shouldn't need to change this.

### Why do I need to supply the Entity type for Get Values?
The Get Values action uses a technique called dynamic schema generation, which allows us to select the  properties of the chosen entity in further actions. This is a feature of the flow designer. Unfortunately, at design time, the trigger can not pass the selected entity type on to other actions. Therefore the entity type needs to be selected again.

### What happens if I select the wrong Entity type for Get Values?
The flow designer will show the wrong properties for the received entity, for example project properties on a sales order. That means some properties that are part of the entity are not shown, and some properties that are shown are not actually part of the entity.

You can still access the properties if they exist on the entity, but you'll have to enter the reference manually.

Choosing a nonexistent property in another action will simply result in an empty value; the consequences of that will depend on the action.

## Deployment Instructions
In order to set up the connector, you need to obtain an OAuth secret from Exact Online and deploy the connector to the Power Automate environment. Then, in order to get notifications, you need to have one or more Flows set up in Exact Online.

### Get an OAuth secret from Exact Online

Your Exact Online administrator will be able to obtain the secret for the OAuth registration.

### Deploy the connector to your Power Automate environment
Open a PowerShell session at the path where your connector files are. Run the following command and follow the prompts:

```
paconn login
```
This will guide you through the device-code login flow.

Next, if your connector files include a settings.json file, run the command below, and replace &lt;secret&gt; with the secret for your OAuth registration.

```
paconn create --settings settings.json --secret <secret>
```

Alternatively, if you don't have a settings.json file, you can use the command below:

```
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --script script.csx --secret <secret>
```

If the connector already exists, you can run `paconn update` instead, with the same parameters.

### Set up Flows in Exact Online
For the relevant business entities, you need to create Flow actions to send the notifications.
* Go to **Master Data** > **Flow** > **Overview: Flows** and either create a new flow or modify an existing one
* Ensure the flow has the correct Entity selected - at this moment we support account, opportunity, project and sales order
* Depending on your needs, select the Event, for example **Account** / **When an account is modified**
  * You can create multiple flows if you need to cover more events
