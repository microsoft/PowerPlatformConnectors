Personr Connector

The Personr connector provides access to a simple, powerful REST API. Using this API, you can create, verify and request the status or more details of an applicant.
Prerequisites
You will need the following to proceed:
An active/subscribed Personr account.
Generating an Authorization Token
In order to start using the connector, you will need to generate an authentication token. To do this, make sure you have a subscribed account and send a POST request to https://enterprise.personr.co/api/1.1/wf/api-token-generate. The body should include your account email and password. For example...

{
  "email": "example@personr.co",
  "password": "example123"
}

The API will return a response with your authentication token, like in the example below.

{
  "status": "success",
  "response": {
    "token": "1670035114445x631333625381113500",
    "user_id": "1665086295388x405886519539808060",
    "expires": 31536000
  }
}
The token should look like this when entering it into the connector setup: Bearer 1670035114445x631333625381113500

Supported Operations

This connector supports the following actions:
Create an Applicant: Create an applicant, ready to be verified.
Create a Verification Link: Create a verification link, for you to send to an individual or display in an iFrame.
Upload a Document: Upload an image of a document, or selfie, to an applicant.
Request Applicant Check: Once all document images have been uploaded, run the verification on the applicant.
Retrieve Applicant Status: Check the status, and retrieve any information on an applicant.
Retrieve Applicant Details: Retrieve any information on an applicant.
