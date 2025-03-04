# OpenRouter

A high-performance unified interface for LLMs, integrating a vast number of models from various providers.

## Publisher: Fördős András

## Prerequisites
You need an OpenRouter account and API Key, potentially available tokens if using non-free models: (https://openrouter.ai)[https://openrouter.ai].

## Operations

### Get credits
Returns the total credits purchased and used for the authenticated user.

### List models
Returns a list of models available through the API.

### List model endpoints
List of available endpoints for the model.

### Chat completion
Send a chat completion request to a selected model.

### Completion
Send a completion request to a selected model (text-only format).

### Get generation
Returns metadata about a specific generation request.

## Obtaining credentials
Once signed up over (https://openrouter.ai)[https://openrouter.ai], you will need to generate an API Key by navigating to your profile and under "Keys" selecting "Create Key".

## Known issues and limitations

Currently there are no known issues with the connector itself, but be aware, that the underlying service might pose a few limitation based on your usage and available tokens. The connector only implements an "as-is" wrapper around the underlying service. In case you are missing an endpoint, or have a request for additional data, feel free to reach out and connect
