# Dewey
Dewey turns document collections into a research-grade knowledge base. Ingest PDFs, Word docs, and more; then search and extract structured insights — all from your Power Automate flows.

## Publisher: Dewey

## Prerequisites
You need a Dewey account with an active subscription. Sign up at [meetdewey.com](https://meetdewey.com).

## Supported Operations

### Triggers
#### When a document is ready
Fires when a document finishes processing and is fully indexed. Use this to kick off downstream steps (notify a team, search the document, run an AI step) as soon as a newly uploaded file is searchable.

#### When a document has an error
Fires when document processing fails. Use this to alert your team or retry ingestion automatically.

### Actions
#### List collections
Returns all collections in your organisation.

#### Get collection
Returns metadata for a single collection by ID.

#### Get document upload URL
Returns a pre-signed S3 URL for a file upload, along with a document ID. After uploading directly to that URL with an HTTP PUT, call **Confirm document upload** to begin processing.

#### Confirm document upload
Tells Dewey that the file upload to S3 is complete and processing should begin.

#### Wait for document
Blocks until the document reaches a terminal state (ready or error) and returns the result. Times out after 5 minutes. Use this after **Confirm document upload** to pause a flow until the document is searchable.

#### Search collection
Runs a hybrid semantic and keyword search against a collection and returns the most relevant text chunks. Ideal for RAG flows where you want to feed context into a subsequent AI Builder or Azure OpenAI step.

## Obtaining Credentials
API keys are created in the Dewey dashboard under **Settings → API Keys**. Keys start with `dwy_live_`. When creating a connection in Power Automate, enter the key in the form `Bearer dwy_live_...` (include the `Bearer ` prefix).

## Getting Started
A typical SharePoint → Dewey ingestion flow:

1. **[SharePoint]** When a file is created in library
2. **[SharePoint]** Get file content
3. **[Dewey]** Get document upload URL *(collectionId, filename, contentType, fileSizeBytes, contentHash)*
4. **[HTTP]** PUT file to `uploadUrl` *(built-in HTTP action, no auth header needed)*
5. **[Dewey]** Confirm document upload *(collectionId, documentId)*
6. **[Dewey]** Wait for document *(documentId — blocks until ready or error)*
7. **[Teams / Email]** Notify team — document is now searchable

API reference: [meetdewey.com/docs](https://meetdewey.com/docs)

## Known Issues and Limitations
- **Research queries** use Server-Sent Events and cannot be used as a connector action. Use **Search collection** to retrieve relevant chunks and feed them into an AI Builder or Azure OpenAI step instead.
- **Wait for document** holds the flow connection open for up to 5 minutes. For high-volume ingestion flows, set trigger concurrency to limit parallel executions.
- SHA-256 is not natively available in Power Automate expressions. Use `base64(sha256(fileContent))` with `dataUriToString()`, or pass the SharePoint `eTag` as a consistent alternative for deduplication.
