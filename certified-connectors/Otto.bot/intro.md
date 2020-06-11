Otto.bot connector allows users to rapidly build chatbot skills which can run Power Automate Flows or Logic Apps, and return one or more conversational updates and results back into the chat conversation dialog as either plain text, markdown text, or within a [Microsoft Adaptive Card](https://adaptivecards.io). [Otto.bot](https://otto.bot) gives low-code/no-code capabilities for users to quickly deploy a chatbot with capabilities beyond QnA, by leveraging the ability to retrieve requested information, update databases, send file attachments, automate processes, and even perform Robotic Process Automation (RPA) in response to conversational request made by an end user. With our integrated Azure AD and B2C authentication options, businesses can build fine grained permission-based bot solutions in addition to anonymous chatbot experiences.

## Prerequisites

1. An [Otto.bot](https://otto.bot) account with at least _Client Admin_ role permission and an API Key from admin portal.
2. A Microsoft Power Automate or a Logic App with a valid Azure subscription.

## How to get credentials

1. Visit [Otto Admin Portal](https://admin.otto.bot/companies) and select the required company from the list.
2. Expand the API Keys panel and generate/copy an existing API Key. We recommend naming your keys according to its usage.
3. Navigate to the [Otto Admin Portal API Skills](https://admin.otto.bot/skills?skillType=apiSkill) and select the required skill.
4. If the skill returns the result to the user after the execution, ensure **Expect Execution Result** is enabled.
5. Select the **View JSON Schema** and copy the schema to clipboard.
6. Create a new Flow or Logic app with HTTP Request trigger and paste the schema copied above.
7. Add required actions to your connector to complete the Flow logic.
8. Select **Otto** action and select **Return results to bot** action.
9. Use the API Key from step 2 and create a new API connection in Flow.

## Known issues and limitations

- Every logic app run has a unique Request ID/Return Result URL associated with it which is generated from Otto. The Logic app cannot be triggered manually using the Run interface in the logic app designer.
- If your Logic app has a successful run, replaying the run would result in an error. As the Request ID is unique per logic app run, replaying the run would result in reusing the ID which leads to the error.