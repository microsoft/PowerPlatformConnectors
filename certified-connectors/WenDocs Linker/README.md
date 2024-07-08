# WenDocs Linker

WenDocs Linker connects to WenDocs Publish REST API Service. Registered users to call service to assemble their WenDocs templates(made by our Word add-in: Wendocs Template Designer) and json data, then publish to Word, PDF, HTML formats. By integrating with other connecters, it is easy to produce documents driven by data automatically, like a welcome letter, resume, business report...

## Prerequisites

You need the following steps:

1. A WenDocs word template file, please search "WenDocs Template Designer" in the Word office add-in store, and create a template file by instructions on [Help](https://help.wendocs.com) or start from the [Getting Started](https://help.wendocs.com/#/doc/chapter-2).

2. The json format data file which is compliance with your template file in step 1.

3. The API_Key and API_Secret obtained from support@wendocs.com for making a basic authenticate connection to REST Service.

4. A Microsoft Power Apps or Power Automate plan with custom connector feature

5. The Power platform CLI tools: paconn

## Building the connector

Deploying the connector with Power Apps CLI tools:

```
paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json
```

## Known Limitations

Publish API call is limited by the total numbers of subscription you obtained from WenDocs site.

## Supported Actions

- Publish a Docx file: Publish a docx file with template and json data.

Request Data Sample:

```json
{
  "docName": "name of document",
  "documentTemplateData": "Base64 String of template file, please use a Base64 expression to encode your file content",
  "jsonData": "JSON format customer data",
  "logLevel": "level of publish log: DEBUG, ERROR, WARNING, INFO",
  "language": "supported language code of error message: en, zh",
  "country": "supported country code of error message: US, CN",
  "clientType": "For API service users, please use 'API Service'"
}
```

Response Data Sample:

```json
{
  "documentName": "name of published docx file",
  "document": "Base64 String of published docx file content, please use a Base64ToBinary expression to decode it",
  "errorMessage": "message of error",
  "errorCode": "code of error",
  "messages": "A message object of exception stack for helping user track issue"
}
```

- Publish a PDF file: Publish a PDF file with template and json data

Request Data Sample:

```json
{
  "docName": "name of document",
  "documentTemplateData": "Base64 String of template file, please use a Base64 expression to encode your file content",
  "jsonData": "JSON format customer data",
  "logLevel": "level of publish log: DEBUG, ERROR, WARNING, INFO",
  "language": "supported language code of error message: en, zh",
  "country": "supported country code of error message: US, CN",
  "clientType": "For API service users, please use 'API Service'"
}
```

Response Data Sample:

```json
{
  "documentName": "name of published pdf file",
  "document": "Base64 String of published pdf file content, please use a Base64ToBinary expression to decode it",
  "errorMessage": "message of error",
  "errorCode": "code of error",
  "messages": "A message object of exception stack for helping user track issue"
}
```

- Publish a Html file: Publish a Html file with template and json data

Request Data Sample:

```json
{
  "docName": "name of document",
  "documentTemplateData": "Base64 String of template file, please use a Base64 expression to encode your file content",
  "jsonData": "JSON format customer data",
  "logLevel": "level of publish log: DEBUG, ERROR, WARNING, INFO",
  "language": "supported language code of error message: en, zh",
  "country": "supported country code of error message: US, CN",
  "clientType": "For API service users, please use 'API Service'"
}
```

Response Data Sample:

```json
{
  "documentName": "name of published html file",
  "document": "Base64 String of published html file content, please use a Base64ToBinary expression to decode it",
  "errorMessage": "message of error",
  "errorCode": "code of error",
  "messages": "A message object of exception stack for helping user track issue"
}
```
