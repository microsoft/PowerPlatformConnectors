# MAQ Text Analytics Connector
MAQ Text Analytics uses Natural Language Processing (NLP) algorithms to mine text data and surface hidden insights. This connector supports Sentiment Analysis, Key Phrase Extraction and PII Scrubbing functionalities which are exposed as operations in Microsoft Power Automate and Power Apps.

# Pre-requisites
You will need the following to proceed:
- A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
- Generate an __API Key__ by using the _Get Free Trial_ button in the Developer Zone section on [MAQ Text Analytics](https://maqtextanalytics.azurewebsites.net/#/DevelopersZone).

# Deploying the connector
```
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

# Supported Operations
The connector supports the following operations:
- `Sentiment Classifier`: Performs Sentiment Analysis of your text data. Returns sentiment score between 0 and 1 for each document. Scores close to 1 indicate positive sentiment, while scores close to 0 indicate negative sentiment
- `PII Scrubber`: Scrubs Personally Identifiable Information (PII) data from your text based on configured entities. Returns text devoid of PII data
- `Key Phrase Extractor`: Extracts Key Phrases from your text data. Returns a comma separated list of Key Phrases for each document
