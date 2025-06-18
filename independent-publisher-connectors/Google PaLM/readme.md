# Google PaLM

## Publisher: Priyaranjan KS, Vidya Sagar Alti [Tata Consultancy Services]

## Overview
The Google PaLM API offers advanced text generation and manipulation capabilities. This documentation covers the endpoints, their parameters, and other essential details.

## Prerequisites
You need to get an API key from Google Make Suite site for authenticating the connector. 

### Obtaining an API Key

Follow these steps to get your API key:

1. **Visit the API Key Generation Page**
   - Go to [Google MakerSuite](https://makersuite.google.com/app/apikey) to access the API key generation interface.

2. **Create API Key**
   - Once on the page, you have two options:
     - **Create API Key in New Project**: Select this if you want to create a new project for your API key.
     - **Create API Key in Existing Project**: Choose this if you prefer to use an existing project for your API key.

By following these steps, you will obtain an API key that is required to authenticate and use the PaLM API.

**Note**: Ensure to keep your API key secure and do not share it publicly.

## Supported Operations

### 1. List Models
#### GET `/{APIVersion}/models`
Retrieves a list of available models through the API.

#### Parameters
| Name       | Optional | Description                             |
|------------|----------|-----------------------------------------|
| `Page Size` | Yes      | The maximum number of models to return. |
| `Page Token`| Yes      | A token for pagination across multiple pages. |
| `API Version` | No      | The version number of the PaLM API. |

### 2. Get Model
#### GET `/{APIVersion}/models/{name}`
Retrieves the details of a specific model.

#### Parameters
| Name  | Optional | Description                             |
|-------|----------|-----------------------------------------|
| `Model Name`| No       | The unique identifier of the model.     |
| `API Version` | No      | The version number of the PaLM API. |

### 3. Generate Text
#### POST `/{APIVersion}/{modelType}/{modelName}:generateText`
Generates text based on a provided prompt.

#### Path Parameters
| Name        | Optional | Description                       |
|-------------|----------|-----------------------------------|
| `Model Type` | No       | The type of the model.            |
| `Model Name` | No       | The name of the model.            |
| `API Version` | No      | The version number of the PaLM API. |

#### Request Body Parameters
| Name              | Optional | Description                                       |
|-------------------|----------|---------------------------------------------------|
| `Prompt`          | No       | The input text prompt.                            |
| `Safety Settings`  | Yes      | Settings for filtering unsafe content.            |
| `Stop Sequences`   | Yes      | Sequences that signal the end of text generation. |
| `Temperature`     | Yes      | Controls the randomness of generated text.        |
| `Candidate Count`  | Yes      | Number of response options to generate.           |
| `Max Output Tokens` | Yes      | The maximum size of the generated text.           |
| `Top P`            | Yes      | Cumulative probability for token selection.       |
| `Top K`            | Yes      | The number of top tokens to consider during generation. |

### 4. Generate Message
#### POST `/{APIVersion}/models/{model}:generateMessage`
Generates a response message based on the input prompt.

#### Path Parameters
| Name   | Optional | Description                       |
|--------|----------|-----------------------------------|
| `Model Name`| No       | The name of the model.            |
| `API Version` | No      | The version number of the PaLM API. |

#### Request Body Parameters
| Name        | Optional | Description                                   |
|-------------|----------|-----------------------------------------------|
| `Messages`    | No       | The structured textual input given as a prompt. |
| `Temperature`| Yes     | Controls the randomness of the output.       |
| `Top P`      | Yes      | Cumulative probability for token selection.  |
| `Top K`      | Yes      | The number of top tokens to consider during sampling. |

### 5. Count Text Tokens
#### POST /{APIVersion}/models/{model}:countTextTokens
Counts the number of tokens in the provided text prompt.

#### Parameters

| Parameter      | Optional | Description                                                                                   |
|----------------|----------|-----------------------------------------------------------------------------------------------|
| API Version     | No       | API version to use for the endpoint. Examples: v1beta3.                                       |
| Model Name         | No       | The model's resource name. Eg: text-bison-001.                                                |
| Text | No    | The text prompt to analyze.                                                                   |


### 6. Count Message Tokens
#### POST /{APIVersion}/models/{model}:countMessageTokens
Counts the number of tokens in the provided message prompt.

#### Parameters

| Parameter          | Optional | Description                                                                                 |
|--------------------|----------|---------------------------------------------------------------------------------------------|
| API Version         | No       | API version to use for the endpoint. Examples: v1beta2, v1beta3.                             |
| Model Name              | No       | The model's resource name. Eg: chat-bison-001.                                              |
| Messages | No    | The prompt, whose token count is to be returned.                                            |


### 7. Text Embedding
#### POST /{APIVersion}/models/{model}:embedText
Turns the provided free-form input text into an embedding.

#### Parameters

| Parameter   | Optional | Description                                                                       |
|-------------|----------|-----------------------------------------------------------------------------------|
| API Version  | No       | API version to use for the endpoint. Examples: v1beta3.                           |
| Model       | No       | The model's resource name. Eg: text-bison-001.                                    |
| Text | No  | The free-form input text for embedding.                                          |

*Please replace `{APIVersion}`,`{model}`, `{modelName}`, and `{modelType}` with appropriate values as per your use case.*


## Known Issues and Limitations
The PaLM API has several limitations which users should be aware of:

1. **Edge Cases**
   - The API may struggle with rare or unusual situations that are not well-represented in the training data.
   - This can lead to issues like model overconfidence, misinterpretation of context, or inappropriate outputs.

2. **Model Hallucinations, Grounding, and Factuality**
   - The API can lack grounding in real-world knowledge, leading to outputs that are plausible but factually incorrect, irrelevant, or nonsensical.

3. **Data Quality and Tuning**
   - The performance of the API is heavily dependent on the quality and accuracy of the input data.
   - Inaccurate or biased input data can lead to suboptimal performance or false model outputs.

4. **Bias Amplification**
   - The API may inadvertently amplify existing biases in the training data, leading to outputs that reinforce societal prejudices.

5. **Language Quality**
   - The API's multilingual capabilities are stronger in English, with potential for reduced performance in non-English languages or less-represented English dialects.
   - Fairness evaluations are primarily conducted in English, which may not cover all potential biases in other languages.

6. **Limited Domain Expertise**
   - The API may not provide accurate or detailed responses on highly specialized or technical topics.
   - For complex use cases, domain-specific tuning and human supervision are recommended.

7. **Length and Structure of Inputs and Outputs**
   - The API has a maximum input token limit of 8k and output token limit of 1k.
   - Safety classifiers are not applied if these limits are exceeded, potentially leading to poor model performance.
   - The API's performance can be affected by inputs with unusual or complex structures.

## API Documentation
 Visit [Google PaLM APIs reference](https://developers.generativeai.google/tutorials/curl_quickstart) page for further details.

---
