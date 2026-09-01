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

#### Create collection
Creates a new collection. Requires a project ID from the Dewey dashboard (Settings → Projects). Use this to provision a collection at the start of a flow before uploading documents.

#### Delete collection
Permanently deletes a collection and all of its documents, sections, and chunks.

#### Get collection
Returns metadata for a single collection by ID.

#### List documents
Returns documents in a collection, ordered by newest first. Supports optional filtering by status (`uploading`, `processing`, `ready`, `error`, etc.) and pagination via limit/offset.

#### Delete document
Permanently deletes a document and all of its sections, chunks, and embeddings.

#### Retry document
Re-queues a document that failed processing. Only works on documents with status `error`. Pair with the **When a document has an error** trigger to build automatic retry flows.

#### Upload document
Uploads a file directly to a collection and queues it for processing. This is the simplest way to ingest a document — no hash computation, HTTP PUT, or separate confirmation step required. Pair with **Wait for document** to block until the file is fully indexed.

#### Get document upload URL *(advanced)*
Returns a pre-signed S3 URL for a file upload, along with a document ID. Use this instead of **Upload document** when you need SHA-256 deduplication, want to avoid routing the file payload through Dewey's API (large files), or need to upload from a non-Power Automate client. After uploading directly to that URL with an HTTP PUT, call **Confirm document upload** to begin processing.

#### Confirm document upload
Tells Dewey that the file upload to S3 is complete and processing should begin. Only needed when using the **Get document upload URL** flow.

#### Wait for document
Blocks until the document reaches a terminal state (ready or error) and returns the result. Times out after 5 minutes. Use this after **Upload document** or **Confirm document upload** to pause a flow until the document is searchable.

#### List sections
Returns the heading hierarchy extracted from a document — section IDs, titles, levels, and positions. Use this to navigate document structure or feed section titles into a downstream AI step without loading full content.

#### Scan sections
Searches section titles and summaries across an entire collection using hybrid semantic and keyword matching. Returns a ranked list of the most relevant sections with the document they belong to. Use this to locate the right parts of your corpus before loading content with **Get section** or **Get section chunks**.

#### Get section
Returns a section's metadata and its full Markdown content. Use this after **Scan sections** or **List sections** to read the actual text of a relevant section without loading the entire document.

#### Get section chunks
Returns the individual text chunks that make up a section. Use this instead of **Get section** when you want to feed section content into an AI Builder or Azure OpenAI step as pre-split passages rather than a single Markdown block.

#### Search collection
Runs a hybrid semantic and keyword search against a collection and returns the most relevant text chunks. Ideal for RAG flows where you want to feed context into a subsequent AI Builder or Azure OpenAI step.

## Obtaining Credentials
API keys are created in the Dewey dashboard under **Settings → API Keys**. Keys start with `dwy_live_`. When creating a connection in Power Automate, enter the key in the form `Bearer dwy_live_...` (include the `Bearer ` prefix).

## Getting Started
A typical SharePoint → Dewey ingestion flow:

1. **[SharePoint]** When a file is created in library
2. **[SharePoint]** Get file content
3. **[Dewey]** Upload document *(collectionId, file content, filename)*
4. **[Dewey]** Wait for document *(documentId — blocks until ready or error)*
5. **[Teams / Email]** Notify team — document is now searchable

### Advanced: upload via pre-signed URL
Use this flow when you need explicit deduplication or want to avoid routing large file payloads through Dewey's API:

1. **[SharePoint]** When a file is created in library
2. **[SharePoint]** Get file content
3. **[Dewey]** Get document upload URL *(collectionId, filename, contentType, fileSizeBytes, contentHash)*
4. **[HTTP]** PUT file to `uploadUrl` *(built-in HTTP action, no auth header needed)*
5. **[Dewey]** Confirm document upload *(collectionId, documentId)*
6. **[Dewey]** Wait for document *(documentId — blocks until ready or error)*
7. **[Teams / Email]** Notify team — document is now searchable

API reference: [meetdewey.com/docs](https://meetdewey.com/docs)

## Known Issues and Limitations
- **Wait for document** holds the flow connection open for up to 5 minutes. For high-volume ingestion flows, set trigger concurrency to limit parallel executions.
- **SHA-256 (advanced flow only)**: SHA-256 is not natively available in Power Automate expressions. If you use **Get document upload URL**, compute the hash with `base64(sha256(fileContent))` and `dataUriToString()`, or pass the SharePoint `eTag` as a consistent proxy. The **Upload document** action handles deduplication internally — no hash required.
