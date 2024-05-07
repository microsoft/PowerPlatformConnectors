# SignatureAPI
SignatureAPI streamlines your document signing process with a customizable, secure, and cost-effective electronic signature platform, ensuring compliance and efficiency.

# Publisher
This connector is published by Signature API, Inc.

## Prerequisites
To use this connector you need either a test or live API key.

## Supported Operations

### Create an envelope
Creates a new envelope to which you can add recipients and documents. When ready, use the 'Start an envelope' action to initiate the signing process.

### Get an envelope
Retrieves the details of an envelope using its ID.

### Start an envelope
Initiates the signing proce
ss for an envelope.

### Add a document
Adds a document to an envelope.

### Add a recipient
Adds a recipient to an envelope.

### Wait for envelope
Waits for an envelope to reach a completed state or another final status.

### Get a deliverable
Retrieves a deliverable using its ID. Use it to download a signed copy of the envelope.

## Obtaining Credentials
To obtain an API Key to use this connector, please write to contact@signatureapi.com.

## Known Issues and Limitations
- Max 10 documents per envelope
- Max 10 recipients per envelope

## Deployment Instructions
Please follow the instructions in [Create a custom connector from an OpenAPI definition](https://learn.microsoft.com/en-us/connectors/custom-connectors/define-openapi-definition).