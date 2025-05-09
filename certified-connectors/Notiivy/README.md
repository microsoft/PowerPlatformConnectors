##Notiivy Connector

Many organisations currently suffer from email fatigue whereby their email channel is being exhausted for mundane everyday notifications and alerts within a department or the business. 
By using browser notifications, this is an effective way to avoid email fatigue and spam when using applications like Dynamics 365 and PowerApps. 
Notiivy is completely integrated with the Microsoft Power Platform allowing users, both internal and external to the organisation, to receive rich persistent notifications based on triggers and data within Dataverse and its data connectors.

## Prerequisites

To use the Notiivy data connector an account at www.notiivy.com is required to optain an API Key. Users can sign up for a free account at https://www.notiivy.com to get started.

## How to get credentials

1. Sign in to your Notiivy account dashboard at https://www.notiivy.com/account.
2. On the Dashboard page you will see your API Key which is used to authenticate the Notiivy data connector and your account.
3. To configure the API Key, log into Microsoft Power Automate or PowerApps.
4. On the left side panel click the Data menu item and then Connections.
5. At the top of the page click New Connection and select the Notiivy Browser Notification connection.
6. When prompted for the API Key, use the API key visible in your Notiivy dashboard and click Create.
7. The connection will now be created and can be used within Power Automate.

## Supported Operations
The connector supports the following operations:
* `Send notification to recipient`: Sends a defined browser notification to an individual recipient.
* `Add a recipient as a subscriber`: Adds a recipient as a subscriber and returns their unique subscription url.
* `Send a notification to many recipients`: Send a notification to all recipients in a specific category.
* `Remove a recipient as a subscriber`: Removes a recipient as a subscriber.

## Known issues and limitations

None