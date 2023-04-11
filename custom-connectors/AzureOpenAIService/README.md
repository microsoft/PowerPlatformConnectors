# Azure OpenAI Service

Azure OpenAI Service Connector for Power Platform & Azure Logic Apps.

> This is a connector based on the [OpenAI Independent Publisher connector](https://learn.microsoft.com/connectors/openaiip/) built by [Robin Rosegrün](https://linktr.ee/r2power). It has some extra actions, because that's how Azure OpenAI Service works.

## Prerequisites

You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription and access to Azure OpenAI Service (currently in preview)
* The Power Platform Connectors CLI (Paconn CLI) tools ([link](https://learn.microsoft.com/connectors/custom-connectors/paconn-cli))

## Connection setup

![Connection setup](./Assets/Connection.png)

After importing the connector, you should create a  connection. There are two properties you need to fill in when creating the connection:

* The API Key of the Azure OpenAI Service
* Endpoint name, this is the instance name of your Azure OpenAI Service. When the URL is `https://microsoft.openai.azure.com`, you should fill in `microsoft` here.

## Available actions

![Available actions](./Assets/AzureOpenAIService.png)

* `Create a completion` - With this action, you can provide a prompt which the Azure OpenAI Service will complete.
* `Create deployment` - With this action, you are able to create a deployment of a model inside of your instance of the Azure OpenAI Service.
* `Delete deployment` - With this action, you are able to delete a deployment of a model inside of your instance of the Azure OpenAI Service.
* `Get deployment` - With this action, you are able to get details about a deployment of a model inside of your instance of the Azure OpenAI Service.
* `List deployments` - With this action, you can list all deployments inside of your instance of the Azure OpenAI Service.
* `List models` - With this action, you can list all models that are available to your instance of the Azure OpenAI Service.

## Resources

* [Azure OpenAI Service Documentation](https://learn.microsoft.com/azure/cognitive-services/openai/)