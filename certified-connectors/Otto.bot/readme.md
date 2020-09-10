## Otto.bot Connector
Otto is an easy-to-train chatbot which answers questions and automates the tasks you need done, while never forgetting, never leaving the organization, and working 24/7/365 to help you get more accomplished every single day. AI and automation solutions allow you to get more tasks done every day with more productivity, lower costs, without the constraint of time.

Otto.bot connector allows users to rapidly build chatbot skills which can run automation workflows, and return one or more conversational updates and results back into the chat conversation dialog as either plain text, markdown text, or within a visually formatted response. [Otto.bot](https://otto.bot) provides low-code/no-code capabilities for users to quickly deploy a chatbot with capabilities beyond QnA, by leveraging the ability to retrieve requested information, update databases, send file attachments, automate processes, and even perform Robotic Process Automation (RPA) in response to a conversational request made by an end user. With our enterprise authentication options, businesses can build permission-based bot solutions in addition to anonymous chatbot experiences.


## Prerequisites

1. An [Otto.bot](https://otto.bot) account with at least _Client Admin_ role permission.
2. An API Key from [Otto Admin Portal](https://admin.otto.bot/companies).

## How to get credentials

1. Visit [Otto Admin Portal](https://admin.otto.bot/companies) and select the required company from the list.
2. Expand the API Keys panel and generate/copy an existing API Key. We recommend naming your keys according to its usage.
3. Navigate to the [Otto Admin Portal API Skills](https://admin.otto.bot/skills?skillType=apiSkill) and select the required skill.
4. If the skill returns the result to the user after the execution, ensure **Expect Execution Result** is enabled.
5. Select the **View JSON Schema** and copy the schema to clipboard.
6. Create a new automation workflow with HTTP Request trigger and paste the schema copied above.
7. Add required actions to your connector to complete the automation workflow logic.
8. Select **Otto** connector and select **Return results to bot** action.
9. Use the API Key from step 2 and create a new API connection in your automation workflow.

## Known issues and limitations

- Every automation workflow initiated from Otto.bot has a unique Request ID/Return Result URL associated with it which is generated from Otto. The workflow cannot be triggered manually.
- If your automation workflow has a successful run, replaying the run would result in an error. As the Request ID is unique per run, replaying the run would result in reusing the ID which leads to the error.

## Supported Operations
The connector supports the following operations:

* `Return Results To Bot` - Send one or more responses to Otto during the execution of a skill. Compose a [Markdown supported message](https://markdown-it.github.io/) or build a rich, interactive UI using an [Adaptive Card](https://adaptivecards.io/). The returned results are specific to each request.
* `Send File Attachments` - Send file attachments and data to a URL using multipart/form-data encoding. This API is created to support execution flows where the built-in HTTP request connector cannot easily handle multipart/formdata.
