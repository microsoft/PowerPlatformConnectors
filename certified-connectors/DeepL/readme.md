# Publisher: DeepL
# DeepL API
Our goal is to overcome language barriers and bring cultures closer together. In order to achieve that goal, 
we are using our expertise in artificial intelligence and neural networks to create technologies that make communication faster, better, and easier.

The DeepL API provides programmatic access to DeepL’s machine translation technology, making it possible to bring high quality translation capabilities directly to your websites and applications.

## Prerequisites
* A DeepL API subscription is required, you can sign up for the Free or Pro plan on our Website:  [DeepL API](https://www.deepl.com/pro-api?cta=header-pro-api/).

## Obtaining Credentials

* After signing up you can retrieve your API key directly from your account overview
[Account Overview](https://www.deepl.com/pro-account/)

## Supported Operations

The connector can use all operations possible using DeepL API. 
You can find more details about all operations and explanations about the various parameters here: [DeepL API Docs](https://www.deepl.com/docs-api)

### Translate
The main translate function allows translating any text. 
Currently only UTF-8 encoded plain text is supported. 

### Manage Glossaries
The glossary functions allow you to create, inspect, and delete glossaries.
Glossaries created with the glossary function can be used in translate requests by specifying the glossary_id parameter

### Translate Documents
The document translation API allows you to translate whole documents.

Translating a document usually involves three steps:
* upload the document to be translated,
* periodically check the status of the document translation,
* once the status call reports done, download the translated document.

Please note that with every submitted document of type .pptx, .docx, or .pdf, you are billed a minimum of 50,000 characters with the DeepL API plan, no matter how many characters are included in the document.

# Known issues and limitations
Some endpoints currently only accept URL-Encoded form data, this will be changed to JSON soon. 

# Deployment Instructions
After deployment, you need to setup the connector with your API Key and Subscription. This will make sure the connector is using the
correct URL. 