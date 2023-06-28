## Cards for Power Apps
Cards for Power Apps are micro-apps with enterprise data and workflows and interactive, lightweight UI elements that other applications can use as content. Because they're part of the Power Apps ecosystem, cards can add business logic through Power Fx and integration with business data through Power Platform connectors. Using cards, you can quickly build and share rich, actionable apps without any coding or IT expertise.

## Publisher: Microsoft ​

## Prerequisites
* [Power Automate](https://make.powerautomate.com/) account
* Create a [card](https://learn.microsoft.com/en-us/power-apps/cards/tutorials/hello-world-card)
* The [Power Apps Teams app](https://learn.microsoft.com/en-us/power-apps/cards/send-a-card/send-card-in-teams) installed in the chat, group chat, or channel that the card will be sent to

## Setting up the connector
1. In the [Power Automate portal](https://make.powerautomate.com/) create or modify an existing flow.
2. Make sure the flow has a trigger. In the example screenshot, the trigger is when a new account record is created. 
3. Create or get the card instance you want to send using the Cards for Power Apps connector.
4. Add an action to Post card in a chat or channel using the Teams connector.
5. On the action, set Post as to Power Apps (Preview).
6. On the action, set Post in, Team & Channel, or Group chat to the conversation you want to send the card in. 
7. On the action, set Card to the card or body dynamic content from the Cards for Power Apps connector action you added earlier in Step 3.

## Supported Operations
The Cards for Power Apps connector is used to get or create instances of cards to send using a flow. The connector has no triggers and three actions:
1. Create card instance - Enables the user to select a specific card to create an instance of with customizable input variables. Returns the card instance as card dynamic content.
2. Get the card instance - Returns a card instance when given a user-specified card and a card instance id as card dynamic content.
3. Get the card description - Returns information about a user-specified card, including the id, environmentId, name, description, author, etc.