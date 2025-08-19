# SignatureAPI Connector

SignatureAPI streamlines your document signing process with a customizable, secure, and cost-effective electronic signature platform, ensuring compliance and efficiency. Version 1.3 introduces enhanced capabilities to further empower your automation workflows.

## Publisher
This connector is published by Signature API, Inc.

## Prerequisites
To use this connector you need either an [API key](https://signatureapi.com/docs/authentication).

## Supported Operations

### Envelope Operations
- **Create an Envelope**  
  Creates a new Envelope to which you can add Recipients and Documents. When ready, use the **Start an Envelope** action to initiate the signing process.
- **Get an Envelope**  
  Retrieves the details of an Envelope using its ID.
- **Start an Envelope**  
  Initiates the signing process for an Envelope.
- **Wait for Envelope**  
  Waits for an Envelope to reach a completed state or another final status.
- **Get a Captured Value**  
  Retrieves a value entered by a Recipient in an input Place during signing.

### Document & Template Operations
- **Add a Document (PDF)**  
  Adds a PDF Document to an Envelope.
- **Add a Document (DOCX)**  
  Adds a DOCX Document to an Envelope.
- **Add a Template (DOCX)**  
  Adds a DOCX Template to an Envelope.
- **Add Data to Template**  
  Inserts data into a Template.

### Place Operations
- **Add a Place — Signature**  
  Adds a signature Place to a Document.
- **Add a Place — Initials**  
  Adds an initials Place to a Document.
- **Add a Place — Text Input**  
  Adds a text input Place to a Document.
- **Add a Place — Text**  
  Adds a text Place to a Document.
- **Add a Place — Recipient Completed Date**  
  Adds a Place for recording the Recipient completed date.
- **Add a Place — Envelope Completed Date**  
  Adds a Place for recording the Envelope completed date.

### Recipient Operations
- **Add a Recipient**  
  Adds a Recipient to an Envelope.
- **Get a Recipient**  
  Retrieves details of a Recipient using their ID.
- **Create a Ceremony — Email Link Authentication**  
  Creates a ceremony where the Recipient is authenticated via an email link.
- **Create a Ceremony — Custom Authentication**  
  Creates a ceremony where the Recipient is authenticated externally.

### Deliverable Operations
- **Get a Deliverable**  
  Retrieves a Deliverable using its ID. Use it to download a signed copy of the Envelope.

### Webhook Notifications
Version 1.3 also supports webhook notifications for real-time event tracking, including:
- **Envelope Events**: created, started, completed, failed, and canceled.
- **Recipient Events**: released, sent, completed, rejected, bounced, failed, replaced, and resent.
- **Deliverable Events**: generated and failed.

## Obtaining Credentials
To obtain an API Key for this connector, please sign up in https://signatureapi.com

## Known Issues and Limitations
- Maximum of 10 Documents per Envelope.
- Maximum of 10 Recipients per Envelope.

## Deployment Instructions
Please follow the instructions in [Create a custom connector from an OpenAPI definition](https://learn.microsoft.com/en-us/connectors/custom-connectors/define-openapi-definition).
