# Google Palm API Documentation

## Overview
The Google Palm API offers advanced text generation and manipulation capabilities. This documentation covers all endpoints, their parameters, and other essential details.

## API Information
- **Swagger Version**: 2.0
- **API Version**: 1.0
- **API Title**: Google Text Generation API
- **Host**: generativelanguage.googleapis.com
- **Base Path**: /
- **Schemes**: HTTPS
- **Consumes**: application/json
- **Produces**: application/json

## Security
### Obtaining an API Key
To use the Google Palm API, you need an API key. Follow these steps to obtain one:
1. Sign in to the [Google Cloud Platform Console](https://console.cloud.google.com/).
2. Create a new project or select an existing one.
3. Navigate to "APIs & Services" > "Credentials".
4. Click on "Create credentials" and select "API key".
5. Your new API key will appear. Restrict the API key for security purposes and note it down for use.

**Note**: Ensure to keep your API key secure and do not share it publicly.

## Endpoints

### 1. List Models
#### GET `/v1beta2/models`
Retrieves a list of available models for text generation.

#### Parameters
| Name       | Optional | Description                             |
|------------|----------|-----------------------------------------|
| `pageSize` | Yes      | The maximum number of models to return. |
| `pageToken`| Yes      | A token for pagination across multiple pages. |

### 2. Get Model
#### GET `/v1beta2/models/{name}`
Retrieves the details of a specific model.

#### Parameters
| Name  | Optional | Description                             |
|-------|----------|-----------------------------------------|
| `name`| No       | The unique identifier of the model.     |

### 3. Generate Text
#### POST `/v1beta2/{modelType}/{modelName}:generateText`
Generates text based on a provided prompt.

#### Path Parameters
| Name        | Optional | Description                       |
|-------------|----------|-----------------------------------|
| `modelType` | No       | The type of the model.            |
| `modelName` | No       | The name of the model.            |

#### Request Body Parameters
| Name              | Optional | Description                                       |
|-------------------|----------|---------------------------------------------------|
| `prompt`          | No       | The input text prompt.                            |
| `safetySettings`  | Yes      | Settings for filtering unsafe content.            |
| `stopSequences`   | Yes      | Sequences that signal the end of text generation. |
| `temperature`     | Yes      | Controls the randomness of generated text.        |
| `candidateCount`  | Yes      | Number of response options to generate.           |
| `maxOutputTokens` | Yes      | The maximum size of the generated text.           |
| `topP`            | Yes      | Cumulative probability for token selection.       |
| `topK`            | Yes      | The number of top tokens to consider during generation. |

### 4. Generate Message
#### POST `/v1beta2/models/{model}:generateMessage`
Generates a response message based on the input prompt.

#### Path Parameters
| Name   | Optional | Description                       |
|--------|----------|-----------------------------------|
| `model`| No       | The name of the model.            |

#### Request Body Parameters
| Name        | Optional | Description                                   |
|-------------|----------|-----------------------------------------------|
| `prompt`    | No       | The structured textual input given as a prompt. |
| `context`   | Yes      | Additional input provided to the model.      |
| `temperature`| Yes     | Controls the randomness of the output.       |
| `topP`      | Yes      | Cumulative probability for token selection.  |
| `topK`      | Yes      | The number of top tokens to consider during sampling. |

### 5. Update Model
#### PUT `/v1beta2/models/{model}`
Updates the specified model's details.

#### Path Parameters
| Name   | Optional | Description                       |
|--------|----------|-----------------------------------|
| `model`| No       | The identifier of the model to update. |

#### Request Body Parameters
| Name        | Optional | Description                                   |
|-------------|----------|-----------------------------------------------|
| `description` | Yes    | A detailed description of the model.         |
| `parameters`  | Yes    | The updated parameters for the model.        |

### 6. Delete Model
#### DELETE `/v1beta2/models/{model}`
Deletes a specific model.

#### Path Parameters
| Name   | Optional | Description                       |
|--------|----------|-----------------------------------|
| `model`| No       | The identifier of the model to delete. |

*Please replace `{model}`, `{modelName}`, and `{modelType}` with appropriate values as per your use case.*

---


