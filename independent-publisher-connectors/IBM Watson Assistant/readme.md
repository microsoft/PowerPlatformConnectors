# **IBM Watson Assistant**

The IBM Watson Assistant service combines machine learning, natural language understanding, and an integrated dialog editor to create conversation flows between your apps and your users.

### Publishers: Lucas Titus, Kin Cheung, Ivan Leong, Andrew Lau

# Pre-requisites

- A Microsoft Power Apps or Power Automate plan.
- An [IBM Cloud](https://cloud.ibm.com) account with access to Watson services.
- A provisioned [Watson Assistant](https://cloud.ibm.com/catalog/services/watson-assistant) resource.

# Obtaining Credentials

Retrieve your IBM Watson Assistant API key and service URL from the **manage** resource page as seen in the image below.

![Credentials](https://i.gyazo.com/58170c137c0362b9fff72bb4a3795e3f.png)

Then retrieve your Assistant Environment ID by going to the Assistant dashboard then **Assistant Settings** as seen in the image below.

![Credentials](https://i.gyazo.com/727ee9c5e637cafbbb063bf1b4a7ce8a.png)

Copy your API key, URL and environment ID and paste them in the connector connection configuration.

# Supported Operations

### [Create a session](https://cloud.ibm.com/apidocs/assistant/assistant-v2#createsession)
Create a new session. A session is used to send user input to a skill and receive responses. It also maintains the state of the conversation. A session persists until it is deleted, or until it times out because of inactivity.

### [Delete session](https://cloud.ibm.com/apidocs/assistant/assistant-v2#deletesession)
Deletes a session explicitly before it times out.

### [Send user input to assistant (stateful)](https://cloud.ibm.com/apidocs/assistant/assistant-v2#message)
Send user input to an assistant and receive a response, with conversation state (including context data) stored by Watson Assistant for the duration of the session.

### [Send user input to assistant (stateless)](https://cloud.ibm.com/apidocs/assistant/assistant-v2#messagestateless)
Send user input to an assistant and receive a response, with conversation state (including context data) managed by your application.

# Known Issues and Limitations

Currently this connector does not support certain operations that are only available with Enterprise plans (Logs, Skills, Releases...)