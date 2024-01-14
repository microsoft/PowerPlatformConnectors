# OpenAI

OpenAI is an artificial intelligence research laboratory. The company conducts research in the field of AI with the stated goal of promoting and developing friendly AI in a way that benefits humanity as a whole.
Through this connector you can access the Generative Pre-trained Transformer 3 (GPT-3), an autoregressive language model that uses deep learning to produce human-like text.

## Publisher: Robin Rosengrün | R2Power, Marc van de Wijgaart (Speech2Text)

## Prerequisites

You need an OpenAI account and available tokens. The account comes with a 3 month trial and enough tokens for testing the service.

## Supported Operations

The connector supports the following operations:

- `ChatCompletion`: Chat with the models `gpt3.5`, `gpt4` or `gpt4-vision` for image recognition
- `CreateImage`: Create an image from a prompt with them models `dall-e-2` or `dall-e-3`
- `Embeddings`: Create vector embeddings from your prompt for semantic search
- `Speech2Text`: Create highly accurate transcriptions with the `whisper` model
- `CreateSpeech`: Create audio files from text with 6 different voices.

## Obtaining Credentials

In your account you can create an API key [here](https://platform.openai.com/api-keys)

## API Documentation

Visit [the OpenAI documentation page](https://platform.openai.com/docs/overview) for further details.
