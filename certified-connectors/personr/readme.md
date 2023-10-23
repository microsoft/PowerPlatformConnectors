# Personr
The Personr connector provides access to a simple, powerful REST API. Using this API, you can create, verify and request the status or more details of an applicant.

## Publisher: Personr Pty Ltd

## Prerequisites
You will need the following to proceed:
- An active/subscribed Personr account.

## Supported Operations
### Create an Applicant
This operation allows you to create a new applicant on a specified verification flow, ready to be verified. You will receive an applicantId, which you will be able to use in other operations. 

### Create a Verification Link
This operation allows you to create a verification link, where applicants can go through the verification process. You can display this in an iFrame, or send this link to an individual.

### Upload a Document
This operation allows you to upload an image of a document, or selfie, to an applicant profile. Ensure that both sides of a document are uploaded, and provide images using a base64 string.

### Request Applicant Check
This operation allows you to tell the server to start the verification process on an applicant, once all documents have been uploaded. Note, you do not need to use this operation when verifying applicants via a link, only when you are using the document upload operation.

### Retrieve Applicant Status
This operation allows you to retrieve the status of an applicant, and if verified, any additional information.

### Retrieve Applicant Details
This operation allows you to retrieve additional information on an applicant.

## Obtaining Credentials
In order to start using the connector, you will need to generate an authentication token. To do this, make sure you have a subscribed account and send a POST request to https://enterprise.personr.co/api/1.1/wf/api-token-generate. The body should include your account email and password. For example...
```json
{
  "email": "example@personr.co",
  "password": "example123"
}
```
The API will return a response with your authentication token, like in the example below.
```json
{
  "status": "success",
  "response": {
    "token": "1670035114445x631333625381113500",
    "user_id": "1665086295388x405886519539808060",
    "expires": 31536000
  }
}
```
The token should look like this when entering it into the connector setup: Bearer 1670035114445x631333625381113500

## Known Issues and Limitations
When using the 'Upload a Document' operation to upload a selfie image, ensure the idDocType is 'SELFIE' and idDocSubType is 'NULL'.
Verifications won't proceed if the image is extremely blurry or out-of-focus. The API supports jpeg, png and pdf file types.

## Deployment Instructions
To deploy this connector, ensure you have an active/subscribed Personr account. Once you have generated an authentication token, search for the Personr connector within PowerAutomate and enter the token when prompted. You can now start using the connector.
