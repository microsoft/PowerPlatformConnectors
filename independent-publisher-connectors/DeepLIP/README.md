# DeepLIP
DeepL Translator is a neural machine translation service.

## Publisher: Michał Romiszewski

## Prerequisites
You will need a free or paid [DeepL](https://www.deepl.com/pro?cta=header-prices/) account in order to use this connector.

## Obtaining Credentials
Your API key will be shown in your dashboard in the Account section.

## Supported Operations
The connector supports the following operations:

### Translate text
- `Request Translation`: Translates input text, detects the language of text, returning translated text.

### Translate documents
- `Upload and Translate a Document`: Uploads a document and queues it for translation.
- `Check Document Status`: Retrieve the current status of a document translation process.
- `Download Translated Document`: Once the status of the document translation process is done, the result can be downloaded.

### Manage glossaries
- `List all Glossaries`: List all glossaries and their meta-information, but not the glossary entries.
- `List Language Pairs Supported by Glossaries`: Retrieve the list of language pairs supported by the glossary feature.
- `Create a Glossary`: Create a Glossary.
- `Retrieve Glossary Details`: Retrieve meta information for a single glossary, omitting the glossary entries.
- `Retrieve Glossary Entries`: List the entries of a single glossary in the format specified by the Accept header.
- `Delete a Glossary`: Deletes the specified glossary.

### Information
- `Retrieve Supported Languages`: Retrieve the list of languages that are currently supported for translation.
- `Check Usage and Limits`: Retrieve usage information within the current billing period together with the corresponding account limits.

## Known Issues and Limitations
There are no known issues at this time.