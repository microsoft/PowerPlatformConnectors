# GitHub Models (Independent Publisher)

GitHub Models provides free AI inference service access to various large language models including GPT-4o, DeepSeek-R1, Llama 3, and more. This connector enables seamless integration with the OpenAI-compatible GitHub Models service.

## Publisher: Troy Taylor

## Prerequisites

You will need the following to proceed:
- A GitHub account
- A GitHub Personal Access Token (PAT) with `models:read` scope

## Obtaining Credentials

1. Go to [GitHub Settings > Developer settings > Personal access tokens](https://github.com/settings/tokens)
2. Click "Generate new token" > "Generate new token (classic)"
3. Give your token a descriptive name (e.g., "Power Platform GitHub Models")
4. Select the `models:read` scope under "Select scopes"
5. Click "Generate token"
6. **Important**: Copy the token immediately as it won't be shown again

## Supported Operations

### Create Chat Completion
Creates a chat completion using GitHub Models. This endpoint is compatible with OpenAI's chat completions API and supports various models including GPT-4o, DeepSeek-R1, and Llama 3.

### Create Embeddings
Creates embeddings for the given input text using GitHub Models embedding models. Embeddings are useful for semantic search, text classification, and similarity analysis.

### List Available Models
Retrieves a list of all available models in the GitHub Models catalog. Use this to discover available models and their capabilities.

### Get Model Details
Retrieves detailed information about a specific model including capabilities, context length, and ownership details.

## Available Models

### Chat Completion Models
- `openai/gpt-4o` - OpenAI's most capable model
- `meta-llama/llama-3-8b-instruct` - Meta's Llama 3 model
- `deepseek/deepseek-r1` - DeepSeek's reasoning model
- And more models available in the catalog

### Embedding Models
- `text-embedding-3-small` - Smaller, faster embedding model
- `text-embedding-3-large` - Larger, more capable embedding model

## API Documentation

For more information about the GitHub Models API, visit:
- [GitHub Models Documentation](https://docs.github.com/en/github-models)
- [GitHub Models API Reference](https://docs.github.com/en/rest/models)

## Known Issues and Limitations

- Free tier has usage limits per GitHub account
- Some models may have restricted availability based on region
- Streaming responses are not supported in this connector
- Rate limits apply based on your GitHub account tier
- Model availability may change over time