# Microsoft Translator 
Translator Service is a cloud-based neural machine translation service that is part of the Azure Cognitive Services family of REST APIs and can be used with any operating system. Translator powers many Microsoft products and services used by thousands of businesses worldwide to perform language translation and other language-related operations

## Text Translation
Text translation is a cloud-based REST API feature of the Translator service that uses neural machine translation technology to enable quick and accurate source-to-target text translation in real time across all supported languages.

## Document Translation
Document Translation is a cloud-based feature of the Azure Translator service and is part of the Azure Cognitive Service family of REST APIs. The Document Translation API can be used to translate multiple and complex documents across all supported languages and dialects, while preserving original document structure and data format. Translate local files or network files in many different formats, to more than 100 different languages.

## Publisher: Microsoft Translator

## Prerequisites
You will need the following to proceed:
* A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
* An Azure subscription
* An Azure Blob storage account. You'll create containers to store and organize your blob data within your storage account.
* A single-service Translator resource

## Supported Operations
All API requests to the Translation service require a custom domain endpoint. The custom domain endpoint is a URL formatted with your resource name, hostname, and Translator subdirectories, ex: https://<NAME-OF-YOUR-RESOURCE>.cognitiveservices.azure.com/translator/text/batch/v1.0

The connector supports the following operations:
### Translate
Translates text to a specific language.
### Transliterate
Translates text in one language from one script to another.
### StartDocumentTranslation
This action starts a document translation job.
### GetTranslationStatus
Gets a summary of the status for a specific document translation request.
### GetTranslationsStatus
Gets a list of all translation requests submitted by a user and their status.
### GetDocumentStatus
Gets the status for a specific document in a job.
### GetDocumentsStatus
Gets the status of all documents in a translation job.
### GetSupportedDocumentFormats
Gets a list of supported document formats.
### GetSupportedGlossaryFormats
Gets a list of supported glossary formats.
### GetSupportedStorageSources
Gets a list of supported storage sources/options.
### CancelTranslation
Cancels a document translation that is currently processing or queued.

## Obtaining Credentials
Requests to the Translator service require a read-only key for authenticating access.
* If you've created a new Translation Azure resource, after it deploys, select Go to resource. If you have an existing Translation resource, navigate directly to your resource page.
* In the left rail, under Resource Management, select Keys and Endpoint.
* Copy and use the resource name to authenticate your request to the Translation service.

## Getting Started
The instructions for getting started on Text Translator can be found [here](https://learn.microsoft.com/en-us/azure/cognitive-services/translator/quickstart-translator?tabs=csharp)
The instructions for getting started on Document Translator can be found [here](https://learn.microsoft.com/en-us/azure/cognitive-services/translator/document-translation/quickstarts/get-started-with-rest-api?pivots=programming-language-csharp)
* Get the Translator Resource name that will be used in the URL template to make API calls.
* Create a new Translator connection with the Azure resource key.
* Now proceed with calling any of the connector actions


## Known Issues and Limitations
Text translation request limits can be found [here](https://learn.microsoft.com/en-us/azure/cognitive-services/translator/request-limits)

Document Translation can't be used to translate secured documents such as those with an encrypted password or with restricted access to copy content.
The below lists the limits for data that you send to Document Translation.
   Document size   ≤ 40 MB
   Total number of files.   ≤ 1000
   Total content size in a batch   ≤ 250 MB
   Number of target languages in a batch   ≤ 10
   Size of Translation memory file   ≤ 10 MB

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```