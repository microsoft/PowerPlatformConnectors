## Otto.bot Connector
Otto is an easy-to-train chatbot which answers questions and automates the tasks you need done, while never forgetting, never leaving the organization, and working 24/7/365 to help you get more accomplished every single day. AI and automation solutions allow you to get more tasks done every day with more productivity, lower costs, without the constraint of time.


## Pre-requisites

1. An [Otto.bot](https://otto.bot) account with atleast _Client Admin_ role permission and an API Key from admin portal.
2. A Microsoft Power Automate or a Logic App with a valid Azure subscription.

## Get Started
1. Visit [Otto Admin Portal](https://admin.otto.bot) and navigate to your company detail page.
2. Expand the API Keys panel and generate/copy an existing API Key. We recommend naming your keys according to its usage.
3. Create a new Flow or Logic app with required operations for the flow and use the available actions from this connector.


## Supported Operations
The connector supports the following operations:

* `Return Results To Bot` - Send flow execution result to the user using Otto. Compose a [Markdown supported message](https://markdown-it.github.io/) or build a rich, interactive UI using  an [Adaptive Card](https://adaptivecards.io/). The returned results are specific to each request.
* `Send File Attachments` - This is a helper function created to simplify sending file attachments to a URL using `multipart/form-data` type.
