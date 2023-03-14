# The Bot Platform's Power Automate connector
Connect your bots with any of your favorite apps in just a few clicks. Send messages to your users with data extracted from other apps, set attribute values in the platform as well as create new user attributes.
## Publisher: The Bot Platform
The Bot Platform 
## Prerequisites
You will need an account at [The Bot Platform](https://thebotplatform.com)
## Supported Operations
The following operations are supported:
### Create a new user attribute
Setup a new attribute for users in the bot
### Update an attribute for a user in the bot
Update an attribute value for a user in the bot
### Send a simple text message to a bot user
Compose and send a simple text to a bot user. User can be identified via email address.
## Obtaining the credentials
After logging into your account, go to the bot you want to integrate and click on 'API Access' to obtain your client ID and Secret key. After obtaining this information, create a new Power Automate Flow, you will be prompted to enter the client ID and secret key there. After entering this information, a pop up will appear asking for permission to access the bot. Hit 'Allow' and you are all set!
## Known Issues & Limitations
- Currently users can only be identified via email address to be able to communicate information back to the bot.
- You cannot send an image url using the simple text message operation.
## Deployment Instructions
After logging in to Power Automate. Create a new power automate flow and search for 'The Bot Platform' to find this connector. Choose a relevant operation and begin building your integration.
## Use cases
To check out some use cases, visit our [Dev Docs page](https://dev.thebotplatform.com)
