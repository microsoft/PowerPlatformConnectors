# Staffbase connector
With the Staffbase connector for Power Automate you bridge the gap between different tools and systems and include the Staffbase platform in your automated workflows. Leverage the power of automated workflows that include your employee app or intranet to automate processes, run tasks on a schedule, or notify users as events take place.

## Prerequisites
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Staffbase license for your organization 
* The role of administrator for your organization on the Staffbase platform
* The [API token](https://support.staffbase.com/hc/en-us/articles/360015755691) to configure authentication
* Information on which Staffasbe infrastructure your application is hosted on, reach out to support@staffbase.com for more information

## Supported Operations

### ChannelsGetList
Get a list of news channels

### ChannelsGetPosts
Get a list of posts within the specified channel

### ChannelsPostPost
Send a post to specific channel

### CommentsGet
Get all comments from your Staffbase instance

### MediaGet
Get all media from your Staffbase instance

### MediaGetByID
Get media by ID

### MediaDelete
Delete media by ID

### NotificationPost
Sends notification to user(s)

### PostsGetAll
Get a list of posts

### PostsGetByID
Get a post by ID

### PostsDelete
Removes a post

### PostsPut
Updates a post

### UserGetAll
Get list of users

### UserPost
Invite an user by firstname, lastname and eMail

### UserGetByID
Get user information

### User_delete
Delete User by ID

### UserPut
Update user information

### UserPostRecovery
Send a recovery email

### ProxyVersionGet
Just for internal usage, to check the current API proxy version

## Obtaining Credentials
For the basic authentication between your Staffbase platform and the workflow created in Power Automate, you need an API token. You can [generate the API](https://support.staffbase.com/hc/en-us/articles/360015755691-Generating-an-API-Token) token from the Staffbase Experience Studio.

## Getting Started
Configure the connector with basic authentication and the hosting URL. 
 
1. In Power Automate, navigate to Connectors.
2. Search for Staffbase connector.
3. Click on the Staffbase connector.
A dialog to configure the connector opens.
5. In the API Token field, add the API token using the following syntax: Basic [API-Token]

__Note__: You need to add an empty space between 'Basic' and the API token.

In the Host ID field, enter the identifier for the infrastructure where your Staffbase platform is hosted:
 * German infrastructure: **de1**
 * International infrastructure: **us1**
 
For detailed information, visit the [Staffbase Support Portal](https://support.staffbase.com/hc/en-us/articles/360017381759-Automating-Workflows-With-the-Staffbase-Connector-for-Power-Automate).

## Known Issues and Limitations
The Staffbase connector currently does not support all Staffbase API features. 
For more detailed information on Staffbase APIs, visit the [Staffbase Developer Portal](http://rs.staffbase.com/#developer-portal).


## Frequently Asked Questions

### What business workflows can be automated using the Staffbase connector?
Many business processes can be automated using the Staffbase connector. For example, notify your employees directly in their employee app or intranet as and when actions take place in other tools. Learn more about such business use cases [here](https://support.staffbase.com/hc/en-us/articles/360017639140-Automate-Business-Workflows-With-Staffbase-and-Power-Automate).

### Are there tutorials to help me get started with an automated workflow using the Staffbase connector?
Staffbase offers tutorials that use our Forms plugin and the Staffbase connector to automate your facility management. Learn all about how to set it up in this [section](https://support.staffbase.com/hc/en-us/sections/360004870179-Integration-Using-Forms-Plugin).


## Deployment Instructions
Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```