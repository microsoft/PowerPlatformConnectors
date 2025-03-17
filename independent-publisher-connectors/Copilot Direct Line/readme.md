# Copilot Direct Line
Direct Line allows for connecting directly to a Copilot Studio agent.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
Your Copilot Studio (or Bot Framework) agent must have web channel security enabled.

## Obtaining Credentials
In the Security settings of your agent, copy a secret from the two available.

## Supported Operations
### Start a conversation with activity
Starts a new conversation, sends the first activity and waits for the agent to respond.
### Post activity and receive response
Sends activity to this conversation and waits for the agent to respond.
### Start conversation
Starts a new conversation.
### Get conversation
Retrieve information about an existing conversation.
### Get activities
Retrieve activities in this conversation. This method is paged with the 'watermark' parameter.
### Post activity
Sends activity to this conversation.
### Upload file
Uploads file and sends as attachment.

## Known Issues and Limitations
There are no known issues at this time.
