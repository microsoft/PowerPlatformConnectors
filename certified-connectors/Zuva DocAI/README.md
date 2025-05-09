# Zuva DocAI connector

The Zuva DocAI connector enables developers to implement contracts AI features into their applications.

## Prerequisites

You will need a Zuva account and a DocAI token. See [Obtaining Credentials](#obtaining-credentials).

## Supported Operations

### File Management

Upload your file for analysis. Note that files automatically expire 48 hours after being uploaded.

- `Submit File`: Upload a file to Zuva's servers
- `Delete File`: Remove a file from Zuva's servers

### Extraction

Extract specific information from your document, such as its title or governing law. You will
need the field ID of the fields you would like to extract. Field IDs can be found in the [Field Library](https://docai.zuva.ai/field-library)
(sign-in required), from [AI trainer](https://zuva.ai/ai-trainer/) if you have trained custom fields, or
programmatically using the [Fields endpoint](#fields).

- `Create Field Extraction Request`: Initiate asynchronous extraction of fields from a document.
- `Get Field Extraction Request Status`: Check the status of an existing extraction request.
- `Get Field Extraction Request Text Results`: Get text results from a completed extraction request.

### Language Classification

Discover the language of your document.

- `Create Language Classification Request`: Initiate asynchronous classification of the document language.
- `Get Language Classification Request Status`: Get status and results of language classification.

### Document Classification

Categorize your document: is it a contract and, if so, what type of contract (real estate agreement, employment agreement etc.)?

- `Create Document Classification Request`: Initiate asynchronous classification of the document type.
- `Get Document Classification Request Status`: Get status and results of document classification.

### Multi-level Classification

Categorize your document's type using a multi-level classifier (e.g. ["Contract", "Real Estate Agt", "Real Estate Lease"]`).

- `Create Multi-Level Classification Request`: Initiate asynchronous classification of the document.
- `Get Multi-Level Classification Request Status`: Get status and results of multi-level classification.

### Optical Character Recognition (OCR)

Obtain the text of your documents, regardless of their original file type (pdf, docx, png ...), as well
as images of the document.

- `Create OCR Request`: Initiate asynchronous OCR processing of a file.
- `Get OCR Request Status`: Check whether OCR processing of a file is complete.
- `Get OCR Results Text`: Retrieve the text from a processed file.
- `Get OCR Results Images`: Retrieve the images from a processed file.

### Fields

Find out what fields are available to you.

- `Get Field List`: Return a list of all available fields (both built-in and custom).

### Normalization

Normalize a string of text to a standard format.

- `Normalize Dates`: Convert dates found in text to an integer representation (e.g. year/month/day).

## Obtaining Credentials

Zuva DocAI has multiple regional servers. You will need to create an API token for the
specific region that you wish to use.

1. Create and/or sign into your account at [zuva.ai](https://zuva.ai/).
2. Navigate to the [DocAI console](https://docai.zuva.ai/)
3. Select the region you would like to use.
4. Copy the base URL for the region to your Zuva DocAI connection.
5. Create a token and copy it to use in your Zuva DocAI connection.

See the  [DocAI documentation](https://zuva.ai/documentation/) for more information about Zuva accounts and tokens.

## Differences between the connector and the DocAI API

This connector makes use of a C# script to modify both requests and responses. As a result, the
connector functionality does not correspond one-to-one with the documentation for
the underlying API. In particular, the connector:
1. Exposes an extra `is_finished` boolean, which can be used to tell if the request is either complete or failed
2. All requests operate on single files, rather than batches of multiple files.

## Known issues and limitations

The connector does not support any of Zuva's endpoints related to training custom fields.
