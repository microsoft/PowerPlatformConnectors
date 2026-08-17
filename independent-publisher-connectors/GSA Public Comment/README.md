# GSA Public Comment

The GSA Public Comment connector provides access to the Regulations.gov public commenting apparatus. It allows developers to query federal regulatory dockets, documents, and public comments submitted through the U.S. Government Services Administration (GSA).

## Publisher: Dan Romano

## Prerequisites
You must have a valid [Regulations.gov API Key](https://open.gsa.gov/api/regulationsgov/) to use this connector.

## Supported Operations

- **GetDocuments**: Retrieve a list of public regulatory documents.
- **GetDocumentById**: Fetch detailed information for a specific document.
- **GetComments**: List submitted public comments on regulations.
- **GetCommentById**: Fetch detailed information for a specific public comment.
- **GetDockets**: Retrieve a list of active or archived regulatory dockets.
- **GetDocketById**: Get details about a single docket including metadata.
- **GetAgencyCategories**: List regulatory categories used by agencies (e.g., Notices, Petitions).

## Obtaining Credentials

1. Obtain an API key from [GSA's Developer Hub](https://open.gsa.gov/api/regulationsgov/).
2. Check your email for the API key that the GSA sends once you submit a request.

## Getting Started
1. Obtain an API key from [GSA's Developer Hub](https://open.gsa.gov/api/regulationsgov/).
2. Use this key to authenticate requests.

## Known Issues and Limitations
- Pagination is supported via metadata but must be implemented manually.
- Some endpoints require filtering parameters (e.g., acronym) to return data.
- `application/vnd.api+json` is accepted by the API but `application/json` is used for compatibility.



