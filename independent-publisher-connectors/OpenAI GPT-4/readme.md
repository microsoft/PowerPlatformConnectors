# OpenAI (V2)

OpenAI is an artificial intelligence research laboratory. The company conducts research in the field of AI with the stated goal of promoting and developing friendly AI in a way that benefits humanity as a whole.
Through this connector you can access the Generative Pre-trained Transformer 4 (GPT-4), an autoregressive language model that uses deep learning to produce human-like text, plus many other models.

## Publisher: Troy Taylor, Robin Rosengrün

## Prerequisites

You need an OpenAI account and available tokens. The account comes with a 3 month trial and enough tokens for testing the service.

## Supported Operations

The connector supports the following operations:

- `Create completion`: Creates a completion for the provided prompt and parameters.
- `Retrieve chat completion`: Retrieves a completion for the provided prompt and parameters.
- `Edit prompt`: Creates a new edit for the provided input, instruction, and parameters.
- `Create an image`: Dall-E creates an image from your prompt.
- `List models`: Lists the currently available models, and provides basic information about each one such as the owner and availability.
- `Create fine-tuning job`: Creates a job that fine-tunes a specified model from a given dataset.
- `Retrieve fine-tuning job`: Retrieve info about a fine-tuning job.
- `Cancel fine-tuning`: Immediately cancel a fine-tune job.
- `List fine-tuning events`: Retrieve status updates for a fine-tuning job.
- `Create moderation`: Classifies if text violates OpenAI's Content Policy.
- `Create embedding`: Creates an embedding vector representing the input text.
- `Create speech`: Generates audio from the input text.
- `Create transcription`: Transcribes audio into the input language.
- `Create translation`: Translates audio into English.
- `Create image`: Creates an image given a prompt.
- `Create image edit`: Creates an edited or extended image given an original image and a prompt.
- `Create image variation`: Creates a variation of a given image.

## Obtaining Credentials

In your account you can create an API key [here](https://beta.openai.com/account/api-keys)

## API Documentation

Visit [the OpenAI documentation page](https://beta.openai.com/docs/api-reference/introduction) for further details.

## Known issues and limitations

When entering your API key in the Power Platform, you need to type it as: "Bearer YOUR_API_KEY" (the word "Bearer", a blank, and the actual API_KEY).
