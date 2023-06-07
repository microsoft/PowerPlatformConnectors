# monday
monday is a work OS where teams create and shape their own workflows in minutes, code-free. 

## Publisher: Plugin Genie
Plugin Genie LLC is committed to providing the best possible experience using Monday with Power Automate.

## Prerequisites
In order to use this connector, you will need the following:

- An active monday.com account
- **Have installed the Microsoft Power Automate app from the monday.com marketplace and subscribed to one of the plans**
- A Power Automate plan

## Supported Operations
### Triggers
- `When an item is created` - Triggers when an item in monday.com is created. It returns the item name and ID.
- `When a column changes` - Triggers when a specific column in monday.com changes for a specific workspace and board. For example, if the Date column is changed for an item. It returns the item name, ID, column values, along with the previous and new column value.
- `When any column changes` - Triggers when any column in monday.com changes for a specific workspace and board. It returns the item name, ID, and column values, along with the previous and new column value.
- `When an item's name changes` - Triggers when the name for a monday.com item is changed for a specific workspace and board. It returns the item name, ID, the previous item name, and the column values.
- `When a new update is posted` - Triggers when a new update is posted on a monday.com item for a specific workspace and board. It returns the item name, ID, the update body, the text update body, the column values for the item, and the name/email of the user who added the update.
- `When a subitem is created` - Triggers when a monday.com subitem is created for a specific workspace and board. It returns the name and ID of the subitem.
- `When a subitem's name changes` - Triggers when the name of a monday.com subitem is changed for a specific workspace and board. It returns the subitem name, ID, the previous subitem name, and the column values of the subitem.
- `When any subitem column changes` - Triggers when any column for a monday.com subitem changes for a specific workspace and board. It returns the subitem name, ID, the new column value, the previous column value, and the rest of the subitem's column values.

### Actions
#### Creation
- `Create an item` - Creates an item in monday.com with all the supported column values. It returns the ID of the created item.
- `Create a board` - Creates a monday.com board in the specified workspace. It returns the ID of the created board.
- `Create a column` - Creates a monday.com column in the specified workspace and board. It returns the ID of the created column, along with the title and description.
- `Create a group` - Creates a monday.com group in the specified workspace and board. It returns the ID of the created group.
- `Create a notification` - Creates a notification in monday.com for a specific user. The notification can be added to either a board or an item. 
- `Create a subitem` - Creates a subitem in monday.com for a specific parent item in the specified workspace and board. It allows the user to specify all the supported column values for the subitem. Returns the ID of the subitem and the ID of the subitem board.
- `Create a workspace` - Creates an open workspace in monday.com. Returns the ID of the workspace.
- `Create an update` - Creates an update for a specified item in monday.com. Returns the ID of the update.

#### Data Retrieval
- `Get tags` - Gets all the tags in your monday.com account. Returns the name of each tag.
- `Get users` - Gets all the users in your monday.com account. Returns the name and email of each user.
- `Get items` - Gets all the items for a specified workspace, board, and group. Returns the name and ID of each item.
- `Get an item by ID` - Gets an item for a specified item ID. Returns the item ID, name, and column values.

### Obtaining Credentials
You need an active monday.com account. You also need to have installed the Microsoft Power Automate app in monday.com and signed up for a subscription to it. 

Once this has been done, sign in to the monday connector in Power Automate. This will authenticate your connection between monday.com and Power Automate to allow you to start using the connector.

### Getting Started
1. In monday.com, search the marketplace for the Microsoft Power Automate app.
2. Subscribe to one of the app's plans and install the app. 
3. If desired, add the board views: Power Automate Help or Power Automate App Usage. The Power Automate Help view links to the documentation for the connector. The Power Automate App Usage view can be used to track the number of executions for each trigger/action for this connector to help avoid reaching your subscription limit.
4. In Power Automate, search for the "monday" connector for an action or a trigger. 
5. After adding the monday action/trigger, sign in to start using it.

## Known Issues and Limitations
### Supported Column Types
Currently, only the following column types can be read or created in actions and triggers:
- Boolean
- Date
- Dropdown
- Email
- Hour
- Label
- Link
- Location
- Long Text
- Numeric
- People
- Phone
- Priority
- Rating
- Status
- Tag
- Text
- Timeline
- Timezone
- Week
- World Clock

The following column types are not supported:
- Auto number
- Button
- Color Picker
- Connect Boards
- Country
- Creation Log
- Dependency
- Files (assets)
- Formula
- Last Updated At
- Mirror
- Person (deprecated)
- Progress Tracking
- Team (deprecated)
- Time Tracking
- Vote

## App Usage
The Microsoft Power Automate app installed from the monday.com marketplace contains a Power Automate App Usage view. This view can be added to a board to track the number of executions of each trigger/action per month. It's useful to make sure you don't exceed your subscription limits. If you need more executions than your subscription currently allows, the board also contains a button to upgrade your plan to a higher tier.

## Frequently Asked Questions
### I uninstalled the Microsoft Power Automate app in monday.com and reinstalled it again. Now it is no longer working and I'm unable to successfully execute actions/triggers in the monday connector. How do I fix the connector?
When the Microsoft Power Automate app is uninstalled and reinstalled, the credentials are reset. To fix the connector, you need to re-authenticate your monday connector in Power Automate. 

This can be done by going to Data -> Connections in the Power Automate sidebar. Then find the "monday" connection and click on it. At the top, click the "Switch account" button. This brings up a popup to re-authenticate your connection between monday.com and Power Automate. Click the Authorize button at the bottom of the popup. Your connection should now be restored and your actions/triggers should work again.

### I'm seeing a "Not Authenticated" error when executing a monday action/trigger in Power Automate and my actions/triggers have stopped working. How can I fix this?
This can occur when your connection between monday.com and Power Automate has been invalidated. One way this can occur is by uninstalling the Microsoft Power Automate app in monday.com.

To fix this issue, go to Data -> Connections in the Power Automate sidebar. Then find the "monday" connection and click on it. At the top, click the "Switch account" button. This brings up a popup to re-authenticate your connection between monday.com and Power Automate. Click the Authorize button at the bottom of the popup. Your connection should now be restored and your actions/triggers should now work again.

### When executing a monday action/trigger in Power Automate, it does not work. I see the error "Your monday.com Power Automate app is over the execution limit. If you need more executions, please purchase a higher plan for the Microsoft Power Automate app in monday.com". How do I fix this?
This occurs when you've used all the executions your Microsoft Power Automate subscription plan in monday.com currently supports. When this happens, actions/triggers will fail to execute until the next month, unless a higher level subscription is purchased. 

If you need more executions, you can purchase a higher level subscription to the Microsoft Power Automate app in monday.com. Once you've done so, your actions/triggers should start working again within 5 minutes.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further Support
Please view [our documentation](https://plugingenie.com/docs/intro/) for tutorials on how to set up the integration.

For further support, you can contact us at support@plugingenie.com - we're always happy to help.



