## Text Analytics Connector
The Language Service API is a cloud-based service that provides advanced natural language processing over raw text, and includes four main functions: sentiment analysis, key phrase extraction, language detection and entity recognition. This connector exposes these functions as operations in Microsoft Power Automate and Power Apps.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
* The Power platform CLI tools

## Deploying the connector
Run the following command and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json 
```
## Supported Operations
The connector supports the following operations:
* `Detect Sentiment (V2)`: Returns a numeric score between 0 and 1 for each document - scores close to 1 indicate positive sentiment, while scores close to 0 indicate negative sentiment
* `Key Phrases (V2)`: Returns a list of strings denoting the key talking points in the input text for each document
* `Detect Language (V2)`: Returns the detected language and a numeric score between 0 and 1 for each document - scores close to 1 indicate 100% certainty that the identified language is true
* `Detect Entities (V2)`: eturns a list of known entities and general named entities ("Person", "Location", "Organization" etc) in a given document




