# Penneo Sign

Penneo transforms complex processes into a seamless, human-centric experience. By integrating digital signatures into automated workflows, our platform makes it effortless to validate who approved what data and when, reducing administrative burdens while ensuring full compliance and security.

## Prerequisites

To use this connector, you will need an active Penneo account.

## Supported Actions

### Create a new case file
Creates a new case file in Penneo with the specified documents and signers. The case file will be created in Penneo, and a UUID and a payloadHash will be returned that can be used to check the creation status via the queue status endpoint.

### Check job status
Retrieves the current status of a casefile by providing the job UUID and payloadHash. This endpoint is used to poll for job completion status after submitting a case file creation request. The endpoint is rate-limited to 20 requests per minute per uuid-payloadHash combination.

### Get case file details
Retrieves the details of a specific case file from Penneo based on casefileId. This includes its signers, documents, and current status.

The available case file statuses are described below.

```
// 0 : 'new'
// 1 : 'pending'
// 2 : 'rejected'
// 3 : 'deleted'
// 4 : 'signed'
// 5 : 'completed'
// 6 : 'failed'
// 7 : 'expired'
// 8 : 'anonymized'
```

Use status 5: 'completed' as trigger for starting getting the signed documents.

### Download document
Downloads the content of a document from the Penneo as a base64 encoded string. By default, the signed version of the document is returned; use the `signed` parameter to get the unsigned version instead. If your case file includes more documents ensure to loop over each document.

## Obtaining Credentials

The connector has been configured to use OAuth with Authorization Code Grant. Users will have to login with their regular credentials when they use the connector.
## Getting Started

### Creating a Case File

1. **Prepare Your Documents**:
   - Convert your PDF documents to base64 encoding
   - Ensure documents are valid PDF files

2. **Configure Signers**:
   - Provide signer name and email (required)
   - Optionally provide role, language, signing order, etc.
   - Configure email notifications and custom email templates if needed

3. **Create the Case File**:
   - Use the "Create a new case file" action
   - Fill in the case file details:
     - Title
     - Documents (base64 encoded PDFs)
     - Signers (with names and optional details)
     - Optional settings (expiration, language, visibility, etc.)

4. **Check CaseFile creation status**:
   - After creating a case file, you'll receive a UUID and payloadHash
   - Use the "Check job status" action to poll for job completion
   - Respect the rate limit of 20 requests per minute per uuid-hash combination

Note: You can check what each field does by checking https://penneo.readme.io/reference/createcasefile.

### Retrieving a Case File and its Documents

1. **Get the case file details**:
    - Use the "Get case file details" action with the case file id
    - The response includes the case file status, signers, and the list of documents (with their document ids)

2. **Download a document**:
    - Use the "Download document" action with the document id obtained from the case file details
    - The response contains the document content as a base64 encoded string, which you can decode to retrieve the PDF file

## Known Issues and Limitations

1. **Rate Limiting**: The Check job status endpoint is rate-limited to 20 requests per minute per uuid-hash combination. Implement appropriate retry logic with exponential backoff.

2. **Asynchronous Processing**: Case file creation is asynchronous. You must use the job status endpoint to check completion rather than expecting immediate results.

3. **Base64 Encoding**: Documents must be base64 encoded. Ensure proper encoding to avoid request failures(you can use base64 string function in the Power Platform to encode the binary PDF files).

4. **Production Environment**: This connector is configured for the Penneo production environment (`app.penneo.com`).

5. **Document Size**: Large documents may take longer to process. Consider document size limits when encoding to base64.

## Deployment Instructions

Run the following commands and follow the prompts:

```
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png --script script.csx
```
