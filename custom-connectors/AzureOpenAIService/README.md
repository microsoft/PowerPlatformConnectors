# Azure OpenAI Service

Azure OpenAI Service Connector for Power Platform & Azure Logic Apps.

> This is a connector based on the [OpenAI Independent Publisher connector](https://learn.microsoft.com/connectors/openaiip/) built by [Robin Rosegrün](https://linktr.ee/r2power). It has some extra actions, because that's how Azure OpenAI Service works.

## Publishers: Daniel Laskewitz, Andrew Coates & Robin Rosengrün

## Prerequisites

You will need the following to proceed:

* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription and access to Azure OpenAI Service (currently in preview)
* The Power Platform Connectors CLI (Paconn CLI) tools ([link](https://learn.microsoft.com/connectors/custom-connectors/paconn-cli))

To install the connector, you can use the following command (from the same folder as the `apiDefinition.swagger.json` file):

```pwsh
paconn login
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --script AzureOpenAIScript.csx --icon connector-icon.png
```

If you already have the connector installed, you can update it with the following command (from the same folder as the `apiDefinition.swagger.json` file):

```pwsh
paconn login
paconn update --api-def apiDefinition.swagger.json --api-prop apiProperties.json --script AzureOpenAIScript.csx --icon icon.png
```

## Connection setup

![Connection setup](./resources/create-connection.png)

After importing the connector, you should create a  connection. There are two properties you need to fill in when creating the connection:

* The API Key of the Azure OpenAI Service
* Endpoint name, this is the instance name of your Azure OpenAI Service. When the URL is `https://microsoft.openai.azure.com`, you should fill in `microsoft` here.

## Available actions

![Available actions](./resources/available-actions.png)

* `Create a completion` - With this action, you can provide a prompt which the Azure OpenAI Service will complete.
* `Get deployment` - With this action, you are able to get details about a deployment of a model inside of your instance of the Azure OpenAI Service.
* `Chat Completion (Preview)` - With this action, you can provide a prompt which the Azure OpenAI Chat Service will complete.
* `Create deployment` - With this action, you are able to create a deployment of a model inside of your instance of the Azure OpenAI Service.
* `Delete deployment` - With this action, you are able to delete a deployment of a model inside of your instance of the Azure OpenAI Service.
* `Embeddings` - With this action, you are able to create an embedding vector representing the input text.
* `List deployments` - With this action, you can list all deployments inside of your instance of the Azure OpenAI Service.
* `List models` - With this action, you can list all models that are available to your instance of the Azure OpenAI Service.

### Helper actions

There are three actions provided that do not call the Azure OpenAI Service, but are used to either prepare for calling or interpret results from the Azure OpenAI Service.

* `Build a ChatML Completion Prompt` - This action is used to build a prompt for the `Create a Completion` action. It takes a `question`, and `initial_scope` and an array of Question:Answer pairs called `history` and returns a `prompt` that can be used with the `Create a Completion` action.
* `Get the answer and history QA pair from a Completion Response object` - This action is used to extract the answer and history from the response of the `Create a Completion` action. It takes the `response` from the `Create a Completion` action and returns the `answer` and `history` properties.
* `Get the answer and assistant:content pair from a raw Chat Completion response object` - This action is used to extract the answer and history from the response of the `Create a Chat Completion (Preview)` action. It takes the `response` from the `Create a Chat Completion (Preview)` action and returns the `answer` and `history` properties.

## Connector Architecture

![Azure OpenAI Custom Connector Architecture](./resources/azure-openai-custom-connector-architecture.png)

1. The connector is called from a Power Automate Flow, Power App or Logic App. The default policy is used to build the full Azure OpenAI endpoint from the `resource-name` (from the connection) and, if required, the `deployment-id` (passed as a header).
1. The response is received by the custom connector and
1. The response is returned to the calling Power Automate Flow (or Power App, or Logic App)

### Maintaining State for Completions and Chat Completions

TODO: Update with new architecture using helper operations

## Resources

* [Azure OpenAI Service Documentation](https://learn.microsoft.com/azure/cognitive-services/openai/)
* [Azure OpenAI Service API Reference](https://learn.microsoft.com/azure/cognitive-services/openai/reference)
