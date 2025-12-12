# Penneo Sign Sandbox

Penneo transforms complex processes into a seamless, human-centric experience. By integrating digital signatures into automated workflows, our platform makes it effortless to validate who approved what data and when, reducing administrative burdens while ensuring full compliance and security.

## Prerequisites

To use this connector, you will need an active Penneo Sandbox account.

## Supported Actions

### Create a new case file
Creates a new case file in Penneo with the specified documents and signers. The case file will be created in Penneo, and a UUID and a payloadHash will be returned that can be used to check the creation status via the queue status endpoint.

### Penneo check status
Retrieves the current status of a casefile by providing the job UUID and payloadHash. This endpoint is used to poll for job completion status after submitting a case file creation request. The endpoint is rate-limited to 20 requests per minute per uuid-payloadHash combination.

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
    - Use the "Penneo check status" action to poll for job completion
    - Respect the rate limit of 20 requests per minute per uuid-hash combination

Note: You can check what each field does by checking https://penneo.readme.io/reference/createcasefile.

## Known Issues and Limitations

1. **Rate Limiting**: The Penneo check status endpoint is rate-limited to 20 requests per minute per uuid-hash combination. Implement appropriate retry logic with exponential backoff.

2. **Asynchronous Processing**: Case file creation is asynchronous. You must use the job status endpoint to check completion rather than expecting immediate results.

3. **Base64 Encoding**: Documents must be base64 encoded. Ensure proper encoding to avoid request failures(you can use base64 string function in the Power Platform to encode the binary PDF files).

4. **Sandbox Environment**: This connector is configured for the Penneo sandbox environment (`sandbox.penneo.com`).

5. **Document Size**: Large documents may take longer to process. Consider document size limits when encoding to base64.

## Deployment Instructions

Run the following commands and follow the prompts:

```
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png --script script.csx
```
