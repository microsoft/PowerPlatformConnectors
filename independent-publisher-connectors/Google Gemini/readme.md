# Google Gemini

## Publisher: Priyaranjan KS, Vidya Sagar Alti [Tata Consultancy Services]

## Overview
Custom connector for Google Gemini, providing advanced AI multi modal content generation functionalities.Gemini is a family of multimodal large language models developed by Google DeepMind, serving as the successor to LaMDA and PaLM 2. Comprising Gemini Ultra, Gemini Pro, and Gemini Nano, it was announced on December 6, 2023.

## Prerequisites
An API key from Google AI Studio is required to access the Google Gemini API.

### Obtaining Credentials
1. Go to [Google's AI Studio](https://makersuite.google.com/) .
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
| API Version | Yes      | string | API version, e.g., 'v1beta'.            |
| Model Name  | Yes      | string | Model name, e.g., 'gemini-pro'.         |

#### Request Body Parameters
| Name              | Required | Type    | Description                                 |
|-------------------|----------|---------|---------------------------------------------|
| Contents          | Yes      | array   | Contents for generating the text response, formatted as an array of objects with text parts for the prompt. |
| Safety Settings    | No       | array   | Optional. An array of safety settings objects to filter content based on specified categories and thresholds. Each object can specify properties like `category` to indicate the type of content to filter (e.g., violence, adult content) and `threshold` to set the sensitivity of the filter. Detailed structure can be found in the [SafetySetting documentation](https://ai.google.dev/api/rest/v1beta/SafetySetting). |
| Generation Config  | No       | object  | Optional. Configuration settings for vision content generation, including:<br>- `maxOutputTokens`: Integer specifying the maximum number of tokens in the generated content.<br>- `temperature`: Number controlling randomness in the response. Higher values lead to more varied responses.<br>- `topP`: Number controlling diversity of the response. Higher values lead to more diverse responses.<br>- `topK`: Integer limiting the number of high-probability tokens considered at each step.<br>- `stopSequences`: Array of strings specifying character sequences that will stop text output generation. Further configuration details are available in the [GenerationConfig documentation](https://ai.google.dev/api/rest/v1beta/GenerationConfig). | 

#### Responses
- `200`: Successful text response.

### 2. Stream Generate Content
#### POST `/{apiVersion}/models/{modelName}:streamGenerateContent`
Generates content in a streaming manner for faster interactions.

#### Path Parameters
| Name       | Required | Type   | Description                             |
|------------|----------|--------|-----------------------------------------|
| API Version | Yes      | string | API version, e.g., 'v1beta'.            |
| Model Name  | Yes      | string | Model name, e.g., 'gemini-pro'.         |

#### Request Body Parameters
| Name              | Required | Type    | Description                                 |
|-------------------|----------|---------|---------------------------------------------|
| Contents          | Yes      | array   | Contents for stream generating content, formatted as an array of objects with text parts for the prompt. |
| Safety Settings    | No       | array   | Optional. An array of safety settings objects to filter content based on specified categories and thresholds. Each object can specify properties like `category` to indicate the type of content to filter (e.g., violence, adult content) and `threshold` to set the sensitivity of the filter. Detailed structure can be found in the [SafetySetting documentation](https://ai.google.dev/api/rest/v1beta/SafetySetting). |
| Generation Config  | No       | object  | Optional. Configuration settings for vision content generation, including:<br>- `maxOutputTokens`: Integer specifying the maximum number of tokens in the generated content.<br>- `temperature`: Number controlling randomness in the response. Higher values lead to more varied responses.<br>- `topP`: Number controlling diversity of the response. Higher values lead to more diverse responses.<br>- `topK`: Integer limiting the number of high-probability tokens considered at each step.<br>- `stopSequences`: Array of strings specifying character sequences that will stop text output generation. Further configuration details are available in the [GenerationConfig documentation](https://ai.google.dev/api/rest/v1beta/GenerationConfig). | 

#### Responses
- `200`: Successful response with generated content.

  
### 3. Generate Multimodal Content
#### POST `/{apiVersion}/models/{modelName}-vision:generateContent`
Generates a response from the model given text and visual input.

#### Path Parameters
| Name       | Required | Type   | Description                                     |
|------------|----------|--------|-------------------------------------------------|
| API Version | Yes      | string | API version to use for the vision endpoint.     |
| Base Model Name  | Yes      | string | Name of the base model for vision generation.Eg- Enter gemini-pro and corresponding vision model (gemini-pro-vision) will be used |

#### Request Body Parameters
| Name              | Required | Type    | Description                                        |
|-------------------|----------|---------|----------------------------------------------------|
| Contents          | Yes      | array   | Contents for generating the vision response. The array should follow this structure: `[ { "text": "prompt string" }, { "inlineData": { "mimeType": "media type", "data": "base64-encoded media data" } } ]`<br>Where:<br>1. The first object is a 'text' object with a prompt string value.<br>2. The second object is an 'inlineData' object containing 'mimeType' and 'base64-encoded data' of the image or video.Further configuration details are available in the [Content documentation](https://ai.google.dev/api/rest/v1/Content).  |
| Safety Settings    | No       | array   | Optional. An array of safety settings objects to filter content based on specified categories and thresholds. Each object can specify properties like `category` to indicate the type of content to filter (e.g., violence, adult content) and `threshold` to set the sensitivity of the filter. Detailed structure can be found in the [SafetySetting documentation](https://ai.google.dev/api/rest/v1beta/SafetySetting). |
| Generation Config  | No       | object  | Optional. Configuration settings for vision content generation, including:<br>- `maxOutputTokens`: Integer specifying the maximum number of tokens in the generated content.<br>- `temperature`: Number controlling randomness in the response. Higher values lead to more varied responses.<br>- `topP`: Number controlling diversity of the response. Higher values lead to more diverse responses.<br>- `topK`: Integer limiting the number of high-probability tokens considered at each step.<br>- `stopSequences`: Array of strings specifying character sequences that will stop text output generation. Further configuration details are available in the [GenerationConfig documentation](https://ai.google.dev/api/rest/v1beta/GenerationConfig). | 

#### Responses
- `200`: Successful vision response. Returns the generated content based on the provided text and visual inputs.

### 4. Count Tokens
#### POST `/{apiVersion}/models/{modelName}:countTokens`
Counts the number of tokens in a given text using the Generative Language Model.

#### Path Parameters
| Name       | Required | Type   | Description                                 |
|------------|----------|--------|---------------------------------------------|
| API Version | Yes      | string | API version, e.g., 'v1beta'.            |
| Model Name  | Yes      | string | Model name, e.g., 'gemini-pro'.         |

#### Request Body Parameters
| Name              | Required | Type    | Description                                      |
|-------------------|----------|---------|--------------------------------------------------|
| Contents          | Yes      | array   | The content for which token count is determined. |

#### Responses
- `200`: Successful response with token count.
  
### 5. Get All Models
#### GET `/{apiVersion}/models`
Retrieves a list of all available models with their details.

#### Path Parameters
| Name       | Required | Type   | Description                             |
|------------|----------|--------|-----------------------------------------|
| API Version | Yes      | string | API version, e.g., 'v1beta'.            |

#### Responses
- `200`: A list of models with their details.
  
### 6. Get Model Details
#### GET `/{apiVersion}/models/{modelName}`
Retrieves details of a specific model based on the provided model name.

#### Path Parameters
| Name       | Required | Type   | Description                             |
|------------|----------|--------|-----------------------------------------|
| API Version | Yes      | string | API version, e.g., 'v1beta'.            |
| Model Name  | Yes      | string | Model name, e.g., 'gemini-pro'.         |

#### Responses
- `200`: Detailed information of the specified model.
  
### 7. Generate Embedding
#### POST `/{apiVersion}/models/{modelName}:embedContent`
Generates an embedding vector for provided text content, useful for tasks like text similarity, classification, and clustering.

#### Path Parameters
| Name       | Required | Type   | Description                                 |
|------------|----------|--------|---------------------------------------------|
| API Version | Yes      | string | API version, e.g., 'v1beta'.                |
| Model Name  | Yes      | string | Model name, e.g., 'embedding-001'.          |

#### Request Body Parameters
| Name        | Required | Type   | Description                                        |
|-------------|----------|--------|----------------------------------------------------|
| Model Resource Name   | Yes      | string | Model identifier for embedding generation.         |
| Content     | Yes      | object | Content containing text parts for embedding.       |
| Task Type    | No       | string | Type of task for which the embedding is intended.Further configuration details are available in the [Task Type documentation](https://ai.google.dev/api/rest/v1beta/TaskType).  |
| Title       | No       | string | Optional title for the content. This is applicable for certain  task types like RETRIEVAL_DOCUMENT         |

#### Responses
- `200`: Successful response with embedding vector.

### 8. Batch Generate Embeddings
#### POST `/{apiVersion}/models/{modelName}:batchEmbedContents`
This endpoint facilitates generating embedding vectors for a batch of text contents, suitable for various natural language processing tasks such as text similarity, classification, and clustering.

#### Path Parameters
| Name       | Required | Type   | Description                                 |
|------------|----------|--------|---------------------------------------------|
| API Version | Yes      | string | API version, e.g., 'v1beta'.                |
| Model Name  | Yes      | string | Model name, e.g., 'embedding-001'.          |

#### Request Body Parameters
| Name        | Required | Type   | Description                                        |
|-------------|----------|--------|----------------------------------------------------|
| Requests    | Yes      | array  | A batch of embedding request objects. Each object in the array must include the following fields: |

Each object in the `requests` array should contain:

| Field       | Required | Type    | Description |
|-------------|----------|---------|-------------|
| Model       | Yes      | string  | The identifier of the embedding model. Must follow the format 'models/{modelName}'. |
| Content     | Yes      | object  | Contains the text parts for which embeddings are generated. Each `parts` array must contain objects with a `text` field. |
| Task Type    | No       | string  | Type of task for which the embedding is intended.Further configuration details are available in the [Task Type documentation](https://ai.google.dev/api/rest/v1beta/TaskType).  |
| Title       | No       | string  | An optional title for the content, applicable for certain task types like 'RETRIEVAL_DOCUMENT'. |

#### Responses
- `200`: Successful response with batch embeddings. The response contains an array of embedding objects, each with numerical values representing the generated embedding.

  
## Known Issues and Limitations
- **Edge Cases**: The API may not perform optimally in rare or unusual situations.
- **Model Hallucinations**: Outputs might be plausible but factually incorrect.
- **Bias Amplification**: The API might inadvertently amplify biases present in training data.
- **Language Quality**: Performance may vary across different languages.
- **Limited Domain Expertise**: Responses on highly specialized topics may lack accuracy.

## API Documentation
For further details, visit the [Google Gemini API Documentation](https://ai.google.dev/tutorials/rest_quickstart).

