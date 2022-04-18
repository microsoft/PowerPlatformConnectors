# OpenAI

OpenAI is an artificial intelligence research laboratory. The company conducts research in the field of AI with the stated goal of promoting and developing friendly AI in a way that benefits humanity as a whole.
Through this connector you can access the Generative Pre-trained Transformer 3 (GPT-3), an autoregressive language model that uses deep learning to produce human-like text.

## Publisher: Robin Rosengrün | R2Power

## Prerequisites

You need an OpenAI account and available tokens. The account comes with a 3 month trial and enough tokens for testing the service.

## Supported Operations

The connector supports the following operations:

- `List engines`: This action will return all available GPT-3 engines.
- `Completion`: This action will complete your prompt and is the main action you will use.

## Obtaining Credentials

In your account you can create an API key [here](https://beta.openai.com/account/api-keys)

## API Documentation

Visit [the OpenAI documentation page](https://beta.openai.com/docs/api-reference/introduction) for further details.

## Known issues and limitations

When entering your API key in the Power Platform, you need to type it as: "Bearer YOUR_API_KEY" (the word "Bearer" a blank and the actual API_KEY)
Only the "Completion" action is supported right now, feel free to add other actions.

## Deployment Instructions

When entering your API key in the Power Platform, you need to type it as: "Bearer YOUR_API_KEY" (the word "Bearer" a blank and the actual API_KEY)
