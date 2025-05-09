# OpenAI

OpenAI is an artificial intelligence research laboratory. The company conducts research in the field of AI with the stated goal of promoting and developing friendly AI in a way that benefits humanity as a whole.
Through this connector you can access the Generative Pre-trained Transformer 3 (GPT-3), an autoregressive language model that uses deep learning to produce human-like text.

## Publisher: Robin Rosengrün | R2Power

## Prerequisites

You need an OpenAI account and available tokens. The account comes with a 3 month trial and enough tokens for testing the service.

## Supported Operations

The connector supports the following operations:

- `Completion_v2`: This action will complete your prompt with the GPT-3 models.
- `ChatCompletion`: Chat models take a series of messages as input, and return a model-generated message as output.
- `CreateImage`: The image generations endpoint allows you to create an original image given a text prompt.
- `Embeddings`: Get a vector representation of a given text input.
- `Completion`: Deprecated completion action, use the other one.

## Obtaining Credentials

In your account you can create an API key [here](https://platform.openai.com/account/api-keys)

## API Documentation

Visit [the OpenAI documentation page](https://platform.openai.com/docs/api-reference) for further details.

## Known issues and limitations

When entering your API key in the Power Platform, you need to type it as: "Bearer YOUR_API_KEY" (the word "Bearer" a blank and the actual API_KEY)
Only the "Completion" action is supported right now, feel free to add other actions.

## Deployment Instructions

When entering your API key in the Power Platform, you need to type it as: "Bearer YOUR_API_KEY" (the word "Bearer" a blank and the actual API_KEY)
