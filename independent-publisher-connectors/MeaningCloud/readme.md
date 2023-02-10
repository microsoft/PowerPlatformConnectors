# MeaningCloud
MeaningCloud's mission is to make high-quality text analytics accessible to all types of businesses. MeaningCloud is one of the leaders in the field of cloud-based text analytics.

## Publisher: Clément Olivier

## Prerequisites
You need to have a MeaningCloud account. You can [sign up](https://www.meaningcloud.com/developer/login) for free.

## Supported Operations
The connector supports the following operations:
* `Deep Categorization`: Deep Categorization assigns one or more categories to a text, using a very detailed rule-based language that allows you to identify very specific scenarios and patterns using a combination of morphological, semantic and text rules.
* `Topics Extraction`: Topics Extraction extracts relevant information such as Named Entities (people, locations, organizations, etc), concepts as well as facts (dates, time expressions, quantities, etc) from bodies of text.
* `Text Classification`: Text Classification classifies texts based on a hierarchical categorization or taxonomy.
* `Sentiment Analysis`: Sentiment Analysis analyzes texts and detects their polarity, subjectivity, irony and emotional agreement.
* `Language Identification`: Language Identification automatically detects the language of texts obtained from any source.
* `Corporate Reputation`: This API provides a semantic tagging of multilingual content for the purpose of Corporate Reputation analysis.
* `Text Clustering`: This tool automatically groups documents by similarity and detects the most significant subjects.
* `Summarization`: Summarize a given text/document according to the specified length.
* `Document Structure Analysis`: This service extracts different sections of a given document with markup content, including titles, sections, abstract and parts of an email.

## Obtaining Credentials
### Mailjet side
Through the [Subscription page](https://www.meaningcloud.com/developer/account/subscriptions), you can grab the Licence Key.

### Custom connector side
Once you are configuring the connector on the Power Platform.
You will have to use the **API Key** in every request since it can't be send using the Header security parameter.

## Getting Started
Visit MeaningCloud [REST API reference](https://www.meaningcloud.com/developer/apis) documentations page for further details. 

## Deployment Instructions
Import the swagger file in target Power Apps environment.
