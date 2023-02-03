## Cards for Power Apps
Cards for Power Apps are micro-apps with enterprise data and workflows and interactive, lightweight UI elements that other applications can use as content. Because they're part of the Power Apps ecosystem, cards can add business logic through Power Fx and integration with business data through Power Platform connectors. Using cards, you can quickly build and share rich, actionable apps without any coding or IT expertise.

## Publisher: Microsoft ​

## Prerequisites
You will need the following to proceed:
* A Microsoft PowerApps or Microsoft Flow plan with custom connector feature

## Setting up the connector
1. In the [Power Automate portal](https://make.powerautomate.com/), create a new flow or edit an existing one. 
2. Add a new action to your flow and in the "Choose an operation" menu, search for "Cards For Power Apps" under the Premium tab. 
3. Select your preferred action.
4. You will be prompted to create a connection using your Azure AD account, and add a name for connection.
5. That's it! You can now use Cards for Power Apps actions in all your Power Automate flows. 

## Supported Operations
The connector supports the following operations:
* Create Card Instance - To create the card instance of any card in your accessible environment. 
* Get Card Instance - Get the card instance details.
* Get Card Definition - Get the card definition given the card id.
* Search Cards - Search for cards in the environment. You can also add search string to find a specific card.