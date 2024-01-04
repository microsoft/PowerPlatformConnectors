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

### When an account is created or changed
This trigger activates when a change notification is received for an Account business entity.

Parameters:
* **Company**: select the company for which you want to receive the notifications. The list is populated automatically for the logged in user.

### When an opportunity is created or changed
This trigger activates when a change notification is received for an Opportunity business entity.

Parameters:
* **Company**: select the company for which you want to receive the notifications. The list is populated automatically for the logged in user.

### When an project is created or changed
This trigger activates when a change notification is received for a Project business entity.

Parameters:
* **Company**: select the company for which you want to receive the notifications. The list is populated automatically for the logged in user.

### When an sales order is created or changed
This trigger activates when a change notification is received for a Sales Order business entity.

Parameters:
* **Company**: select the company for which you want to receive the notifications. The list is populated automatically for the logged in user.

### Get Values
Takes the `Payload` field from a trigger as input and converts it to a format the user selects.

Parameters:

* **Entity Type**: select the same entity type as your trigger, this allows the Flow Designer to show the correct properties.

  (note: in the Flow Designer we cannot automatically get the entity type from the selected trigger)

* **Format**: select one of the following:
  * ***New Values***: to select specific fields of the new (changed) state in other actions
  * ***Old Values***: to select specific fields of the old (unchanged) state in other actions
  * ***Table***: if you want to get the content as a whole collection for use in other actions, with properties `name`, `oldValue`, `newValue`. 

* **Payload**: this is the content received in the trigger. Leave this to its default value of `triggerOutputs()?['body/Content/Payload']`

* **Content Type**: tells the API which data format is used in the request. Leave this to its default value of `application/json`.

* **Accept**: tells the API which data format is expected in the response. Leave this to its default value of `application/json`.

### Get user data
Internal operation. Gets basic info on the logged in user. Used in the connector's script for most other API calls.

### Get companies for user
Internal operation. Populates the *Company* list in the *When a (entity) is updated* triggers.

### Get supported entity types
Internal operation. Populates the *Entity Type* list in the *Get Values* action.

### List webhook subscriptions
Internal operation, test purposes only. Shows the currently registered subscriptions that are visible to the user.

### Unsubscribe from event
Internal operation. Automatic sign-out of trigger events when a flow is disabled or deleted.

### Get schema definition
Internal operation. Loads the right schema for the *Get Values* action.

## Obtaining Credentials
You require an Exact Online Premium account in order to log in. When you create a new connection, the Exact Online login screen will appear. Use your Exact Online username and password.

## Getting Started

### Set up Flows in Exact Online
For the relevant business entities, you need to create Flow actions to send the notifications.
* In Exact Online, go to **Master Data** > **Flow** > **Overview: Flows** and either create a new flow or modify an existing one
* Ensure the flow has the correct Entity selected, for example **Sales order** - at this moment we support the entities account, opportunity, project and sales order
* Depending on your needs, select the Event, for example **When a sales order is modified**
* Ensure that one of the assigned Actions is **Send notifications to Power Automate**

These flows are set up for a *company*, they only need to be set up once and will then apply to all users in that company.

Note: you need a flow for each event that you want to support. With the 4 entities and 2 event types currently supported, you may need up to 8 flows to support every option.

### Set up your flow in Power Automate
From the Power Automate main screen, navigate to **My flows** and from there, select **New flow** > **Automated cloud flow**.
In the popup window, select **Skip** to immediately open the flow designer.

Click the shape with the text **Add a trigger** and find the Exact Online Premium connector under the **Custom** runtime. Select any of the available triggers, for example, **When a sales order is created or changed**. The trigger will have a parameter **Company**, you can select one of the available companies from the dropdown menu.

Next, add a new action and select **Get Values** from the Exact Online Premium connector. As the **Entity type**, select the same entity type as your trigger, in this example *Sales order*. As the **Format**, select **New values**. Rename the action to **Get New Values**.

Continue with the design of your flow. For example, you can now add an action to send an email through Outlook, or post a message in Teams. Note that for selecting dynamic values in your messages, you can select outputs from *Get New Values*, such as Description and AmountDC.

Save your flow and make sure it is active.

### Test the flow
If you followed the steps above, you are now ready to receive updates to your sales orders. In Exact Online, update a sales order - either make an actual update or simply add "test" to its description - and save it.

## Known issues and limitations

### Empty message for connectivity test
When setting up a new flow, or when turning on an existing flow, a message is sent to Exact Online to register the flow. Exact Online in turn sends a test message (without content) to confirm the connection.

If you don't deal with this message within your flow, the flow may end in error, or send notifications with incomplete content. Since it's usually a one-off behavior, you can choose to accept it, but you may get warnings about failing flows.

A more elegant way to deal with it is to use a *Condition* action to test if the message is empty. The test `empty(triggerOutputs()?['body'])` | `equals` | `true` is a good way to do this. In the *true* branch, add a *Terminate* action (*Control* built-in action) with outcome *Succeeded*. Then continue the actual flow after the Condition action.

## Frequently Asked Questions

### What exactly does the Get Values action do?
*Get Values* performs a couple of actions that would otherwise require multiple more complex actions in your flow. In order, these are:
* It takes the `Payload` field from the trigger and converts its text to a JSON object. This is equivalent to the `json(...)` function;
* It loads a schema for this JSON object so its properties can be referenced in further actions. This is equivalent to the *Parse JSON* action (built-in action) with the JSON schema supplied;
* It can convert the incoming payload either to a complete table with all data, or to an object showing the old or new state of the entity. The table is equivalent to selecting the `Changes` property of the payload's JSON object, the old/new states can be achieved with a couple of complex conversion actions.

### What should the value for Payload be in the Get Values action?
By default, this is set to `triggerOutputs()?['body/Content/Payload']` which is the reference to the Payload field received in the trigger. You shouldn't need to change this.

### Why do I need to supply the Entity type for Get Values?
The Get Values action uses a technique called dynamic schema loading, which allows us to select the  properties of the chosen entity in further actions. This is a feature of the flow designer. Unfortunately, at design time, the trigger can not pass the selected entity type on to other actions. Therefore the entity type needs to be selected.

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
