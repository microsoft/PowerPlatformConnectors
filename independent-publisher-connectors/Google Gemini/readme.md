# Google Gemini

## Publisher: Priyaranjan KS, Vidya Sagar Alti [Tata Consultancy Services]

## Overview
Custom connector for Google Gemini, providing advanced AI multi modal content generation functionalities.Gemini is a family of multimodal large language models developed by Google DeepMind, serving as the successor to LaMDA and PaLM 2. Comprising Gemini Ultra, Gemini Pro, and Gemini Nano, it was announced on December 6, 2023

## Prerequisites
An API key from Google is required to access the Google Gemini API.

### Obtaining Credentials
1. Go to [Google's AI Studio](https://makersuite.google.com/).
2. Create an API key in a new or existing project.
3. Keep your API key secure and do not share it publicly.

## API Base URL
generativelanguage.googleapis.com

## Supported Operations

- `Generate Text Content`
- `Stream Generate Content`
- `Generate Multimodal Content`
- `Count Tokens`
- `Get All Models`
- `Get Model Details`
- `Generate Embedding`
- `Generate Batch Embeddings` 
  
### 1. Generate Text Content
#### POST `/{apiVersion}/models/{modelName}:generateContent`
Generates a text response from the model given an input message.

#### Path Parameters
| Name       | Required | Type   | Description                                 |
|------------|----------|--------|---------------------------------------------|
| apiVersion | Yes      | string | API version to use for the endpoint.        |
| modelName  | Yes      | string | Name of the model to be used for generation.|

#### Request Body Parameters
| Name              | Required | Type    | Description                                 |
|-------------------|----------|---------|---------------------------------------------|
| contents          | Yes      | array   | Contents for generating the text response.  |
| safetySettings    | No       | array   | Safety settings for text content generation.|
| generationConfig  | No       | object  | Configuration settings for text generation. |

### 2. Stream Generate Content
#### POST `/{apiVersion}/models/{modelName}:streamGenerateContent`
Generates content in a streaming manner for faster interactions.

#### Path Parameters
| Name       | Required | Type   | Description                             |
|------------|----------|--------|-----------------------------------------|
| apiVersion | Yes      | string | API version, e.g., 'v1beta'.            |
| modelName  | Yes      | string | Model name, e.g., 'gemini-pro'.         |

#### Request Body Parameters
| Name              | Required | Type    | Description                                 |
|-------------------|----------|---------|---------------------------------------------|
| contents          | Yes      | array   | Contents for stream generating content.     |
| safetySettings    | No       | array   | Safety settings for stream content generation.|
| generationConfig  | No       | object  | Configuration settings for stream generation. |

### 3. Generate Multimodal Content
#### POST `/{apiVersion}/models/{modelName}-vision:generateContent`
Generates a response from the model given text and visual input.

#### Path Parameters
| Name       | Required | Type   | Description                                     |
|------------|----------|--------|-------------------------------------------------|
| apiVersion | Yes      | string | API version to use for the vision endpoint.     |
| modelName  | Yes      | string | Name of the base model for vision generation.   |

#### Request Body Parameters
| Name              | Required | Type    | Description                                        |
|-------------------|----------|---------|----------------------------------------------------|
| contents          | Yes      | array   | Contents for generating the vision response.       |
| safetySettings    | No       | array   | Safety settings for vision content generation.     |
| generationConfig  | No       | object  | Configuration settings for vision content generation. |

### 4. Count Tokens
#### POST `/{apiVersion}/models/{modelName}:countTokens`
Counts the number of tokens in a given text using the Generative Language Model.

#### Path Parameters
| Name       | Required | Type   | Description                                 |
|------------|----------|--------|---------------------------------------------|
| apiVersion | Yes      | string | API version to use for the endpoint.        |
| modelName  | Yes      | string | Model name, e.g., 'gemini-pro'.             |

#### Request Body Parameters
| Name              | Required | Type    | Description                                      |
|-------------------|----------|---------|--------------------------------------------------|
| contents          | Yes      | array   | The content for which token count is determined. |

### 5. Get All Models
#### GET `/{apiVersion}/models`
Retrieves a list of all available models with their details.

#### Path Parameters
| Name       | Required | Type   | Description                             |
|------------|----------|--------|-----------------------------------------|
| apiVersion | Yes      | string | API version, e.g., 'v1beta'.            |

### 6. Get Model Details
#### GET `/{apiVersion}/models/{modelName}`
Retrieves details of a specific model based on the provided model name.

#### Path Parameters
| Name       | Required | Type   | Description                             |
|------------|----------|--------|-----------------------------------------|
| apiVersion | Yes      | string | API version, e.g., 'v1beta'.            |
| modelName  | Yes      | string | Model name, e.g., 'gemini-pro'.         |

### 7. Generate Embedding
#### POST `/{apiVersion}/models/{modelName}:embedContent`
Generates an embedding vector for provided text content, useful for tasks like text similarity, classification, and clustering.

#### Path Parameters
| Name       | Required | Type   | Description                                 |
|------------|----------|--------|---------------------------------------------|
| apiVersion | Yes      | string | API version, e.g., 'v1beta'.                |
| modelName  | Yes      | string | Model name, e.g., 'embedding-001'.          |

#### Request Body Parameters
| Name        | Required | Type   | Description                                        |
|-------------|----------|--------|----------------------------------------------------|
| model       | Yes      | string | Model identifier for embedding generation.         |
| content     | Yes      | object | Content containing text parts for embedding.       |
| taskType    | No       | string | Type of task for which the embedding is intended.  |
| title       | No       | string | Optional title for the content.                    |

### 8. Generate Batch Embeddings
#### POST `/{apiVersion}/models/{modelName}:batchEmbedContents`
Generates embedding vectors for a batch of text contents.

#### Path Parameters
| Name       | Required | Type   | Description                                 |
|------------|----------|--------|---------------------------------------------|
| apiVersion | Yes      | string | API version, e.g., 'v1beta'.                |
| modelName  | Yes      | string | Model name, e.g., 'embedding-001'.          |

#### Request Body Parameters
| Name       | Required | Type   | Description                                      |
|------------|----------|--------|--------------------------------------------------|
| requests   | Yes      | array  | A batch of requests for embedding generation.    |

#### Request Body Schema
- `model`: String, model identifier for embedding generation. Default: 'models/embedding-001'
- `content`: Object, containing the `parts` array with text content for embedding.

## Known Issues and Limitations
- **Edge Cases**: The API may not perform optimally in rare or unusual situations.
- **Model Hallucinations**: Outputs might be plausible but factually incorrect.
- **Bias Amplification**: The API might inadvertently amplify biases present in training data.
- **Language Quality**: Performance may vary across different languages.
- **Limited Domain Expertise**: Responses on highly specialized topics may lack accuracy.

## API Documentation
For further details, visit the [Google Gemini API Documentation](https://ai.google.dev/tutorials/rest_quickstart).

