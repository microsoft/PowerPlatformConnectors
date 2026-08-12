# Digisign Custom Connector – CREATE (Documentation)

Author: Lukas Toman  
Date: June 2026  

This is the official technical documentation for the Digisign - Create module, designed for the Microsoft Power Platform Custom Connector (Power Automate and Power Apps). This connector focuses on envelope and document creation and allows exchanging credentials for a token, creating envelope drafts, uploading files, linking documents to envelopes, performing unified document uploads, adding recipients (Signers, Approvers, CCs), adding signature tags, sending envelopes, and making restricted universal API calls (GET/POST). This document satisfies Microsoft certification requirements and provides a developer guide for integrating Digisign's document creation workflow.

## Prerequisites & Authentication

To use this connector, you must have an active Digisign account. You can manage your API credentials directly in the Digisign administration portal.

### Connection Parameters

When establishing a new connection in Power Automate or Power Apps, you will be prompted for three fields:

| Field Name | Type | Description |
| :--- | :--- | :--- |
| **Access Key (accessKey)** | securestring (Required) | API Access Key from the Digisign administration panel. |
| **Secret Key (secretKey)** | securestring (Required) | API Secret Key from the Digisign administration panel. |
| **Environment URL** | string (Required) | Dropdown selection for the target environment. Defaults to Production (https://api.digisign.org). Can also select Staging (https://api.staging.digisign.org). |

### Under the Hood: Token Exchange & Custom C# Script (script.csx)
The custom code script (script.csx) transparently manages your API session and request payloads:
1. It intercepts credentials passed via standard Basic Authentication.
2. It makes a background token request to POST /api/auth-token using the keys.
3. The resulting short-lived Bearer token is cached and attached as an Authorization: Bearer <token> header to all subsequent API requests.
4. The target server host is dynamically rewritten based on the Environment URL parameter chosen in the connection settings.
5. It intercepts all recipient addition requests (Signer, Approver, CC) and automatically normalizes the `identificationNumber` (ICO) parameter. If the provided value consists purely of digits and is shorter than 8 characters, it pads it with leading zeros to exactly 8 characters. This prevents API schema validation failures (Symfony regex `/^\d{8}$/`) when users input shorter Czech ICO values (e.g. `3541321` is automatically corrected to `03541321`).

No manual token generation, refreshing, or management is required within Power Automate flows.

---

## Operations Overview

| # | Operation ID | Display Name (Power Automate) | HTTP Method | API Endpoint |
| :--- | :--- | :--- | :--- | :--- |
| 1 | authToken | Exchange Keys for Bearer Token | POST | /api/auth-token |
| 2 | createEnvelope | Create Envelope Draft | POST | /api/envelopes |
| 3 | uploadDocument | Upload File | POST | /api/files |
| 4 | linkDocument | Link Document | POST | /api/envelopes/{envelopeId}/documents |
| 5 | unifiedUpload | Unified Upload Document | POST | /api/envelopes/{envelopeId}/documents/unified-upload |
| 6 | addRecipientSigner | Add Recipient: Signer | POST | /api/envelopes/{envelopeId}/recipients/signer |
| 7 | addRecipientApprover | Add Recipient: Approver | POST | /api/envelopes/{envelopeId}/recipients/approver |
| 8 | addRecipientCC | Add Recipient: CC | POST | /api/envelopes/{envelopeId}/recipients/cc |
| 9 | addTag | Add Signature Tag | POST | /api/envelopes/{envelopeId}/tags |
| 10 | sendEnvelope | Send Envelope | POST | /api/envelopes/{envelopeId}/send |
| 11 | callApi | Universal API Call | POST | /api/call-api |

---

## Detailed Operation Schemas & Developer Notes

### 1. Exchange Keys for Bearer Token (authToken)
Exchanges API keys for a short-lived Bearer token.
* **Developer Note**: In normal scenarios, this operation is invoked automatically by script.csx on connection start, and developers do not need to call it manually.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * accessKey (string, Required): The API Access Key.
  * secretKey (string, Required): The API Secret Key.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the credentials are valid.
* **Response Body (JSON)**:
  * token (string): Bearer authorization token.
  * expiresIn (integer): Token lifetime in seconds (typically 3600).

##### HTTP 400 Bad Request
Returned when request body or parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the auth endpoint is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 2. Create Envelope Draft (CreateEnvelope)
Creates a new draft envelope to house documents, recipients, and signature tags.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * name (string, Required): Name of the envelope (max 255 chars). Example: "Service Contract - Project Alpha".
  * emailBody (string, Required): Default email message body sent to recipients (max 4096 chars). Example: "Please review and sign the attached agreement."
  * emailBodyCompleted (string, Optional): Email message body sent once signing is completed.
  * sender (string, Optional): Owner IRI of the envelope. Format: /api/account/users/{UUID}.
  * senderName (string, Optional): Display name of the sender.
  * senderEmail (string, Optional): Contact email address of the sender.
  * expiration (integer, Optional): Days until expiration (0 - 365). Default: 30.
  * metadata (string, Optional): Custom metadata string (max 4096 chars).

> [!WARNING]
> **Owner Assignment (sender field)**:
> - The owner IRI must use the account format /api/account/users/{UUID} (not /api/users/{UUID}).
> - If left empty, the envelope defaults to the API key user.
> - The sender field can only be defined during envelope creation and is immutable during updates.

#### Responses & Status Codes

##### HTTP 201 Created / HTTP 200 OK
Returned when the draft envelope is successfully created.
* **Response Body (JSON)**:
  * id (string): Envelope GUID.
  * name (string): Envelope Name.
  * emailBody (string): Email message body text.
  * emailBodyCompleted (string): Completed email message body text.
  * sender (string): Sender resource IRI (e.g. /api/users/019ee470-a0c2-72a9-a666-9ced47485a9b).
  * senderName (string): Display name of the sender.
  * senderEmail (string): Email address of the sender.
  * expiration (integer): Expiration time limit in days.
  * metadata (string): Envelope metadata.
  * status (string): Envelope workflow status (e.g., "draft").

##### HTTP 400 Bad Request
Returned when input properties are invalid or constraints are violated.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the resource path is invalid.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 3. Upload File (UploadDocument)
Uploads a raw binary file to Digisign storage. This creates a temporary file object that must be linked to an envelope in a separate step.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (multipart/form-data)**:
  * file (file, Required): Binary payload (PDF, DOCX, etc.).

#### Responses & Status Codes

##### HTTP 201 Created / HTTP 200 OK
Returned when the file is successfully uploaded.
* **Response Body (JSON)**:
  * id (string): Unique File GUID.

##### HTTP 400 Bad Request
Returned when request parameters or payload are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the files endpoint is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 4. Link Document (LinkDocument)
Binds a previously uploaded file object to a specific draft envelope.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * file (string, Required): The file IRI. Example: "/api/files/019efb94-23db-705f-a7fb-8e3f0cda7dbf".
  * name (string, Required): Display name of the document.
  * position (integer, Optional): Display order position (1-based index).

#### Responses & Status Codes

##### HTTP 201 Created / HTTP 200 OK
Returned when the file is successfully linked as a document.
* **Response Body (JSON)**:
  * id (string): Unique Document GUID.
  * name (string): Display name of the document.
  * metadata (string): Optional custom metadata.
  * position (integer): Ordering index position.
  * signable (boolean): Indicates if the document can be signed.
  * fromTemplate (boolean): Indicates if the document was generated from a template.
  * labelPositioning (string): Label positioning layout strategy.
  * labelPositionX (integer): Horizontal coordinates of the signature labels.
  * labelPositionY (integer): Vertical coordinates of the signature labels.
  * signatureValidity (string): Current cryptographic signature status.
  * invalidatedAt (string): Timestamp when the document was invalidated.
  * invalidate (boolean): Invalidated state.
  * hasSignatures (boolean): Indicates if signatures are present on the document.
  * createdAt (string): Creation timestamp in ISO 8601 format.
  * updatedAt (string): Modification timestamp in ISO 8601 format.
  * file (object): Linked file details:
    * id (string): File GUID.
    * name (string): Generated filename.
    * originalName (string): Original uploaded filename.
    * mimeType (string): MIME type of the file.
    * size (integer): File size in bytes.
    * sha1Checksum (string): SHA1 checksum string.
    * category (string): Document category.
    * createdAt (string): File upload timestamp.
    * updatedAt (string): File modification timestamp.

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 5. Unified Upload Document (UnifiedUpload)
A helper operation that performs file upload and envelope linkage in a single step using a base64 encoded payload. Recommended for Power Automate flow efficiency.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * fileName (string, Required): Filename with extension. Example: "Contract.pdf".
  * fileContent (string, Required): Base64 encoded string of the file content.
  * position (integer, Optional): Index order of the document in the envelope.
  * metadata (string, Optional): Custom metadata string.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the base64 file is uploaded and linked.
* **Response Body (JSON)**:
  * id (string): Unique Document GUID.
  * name (string): Display name of the document.
  * metadata (string): Optional custom metadata.
  * position (integer): Ordering index position.
  * signable (boolean): Indicates if the document can be signed.
  * fromTemplate (boolean): Indicates if the document was generated from a template.
  * labelPositioning (string): Label positioning layout strategy.
  * labelPositionX (integer): Horizontal coordinates of the signature labels.
  * labelPositionY (integer): Vertical coordinates of the signature labels.
  * signatureValidity (string): Current cryptographic signature status.
  * invalidatedAt (string): Timestamp when the document was invalidated.
  * invalidate (boolean): Invalidated state.
  * hasSignatures (boolean): Indicates if signatures are present on the document.
  * createdAt (string): Creation timestamp in ISO 8601 format.
  * updatedAt (string): Modification timestamp in ISO 8601 format.
  * file (object): Linked file details:
    * id (string): File GUID.
    * name (string): Generated filename.
    * originalName (string): Original uploaded filename.
    * mimeType (string): MIME type of the file.
    * size (integer): File size in bytes.
    * sha1Checksum (string): SHA1 checksum string.
    * category (string): Document category.
    * createdAt (string): File upload timestamp.
    * updatedAt (string): File modification timestamp.

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 6. Add Recipient: Signer (AddRecipientSigner)
Adds a signing party to the envelope. Signers must place signatures on the documents.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * role (string, Required): Set to "signer".
  * name (string, Required): Recipient's full name.
  * email (string, Required): Recipient's email address.
  * mobile (string, Required): Mobile phone number in E.164 format. Example: "+420777111222".
  * signatureType (string, Required): Signature level: "simple", "biometric", "qualified". Default: "simple".
  * authenticationOnOpen (string, Required): Access control verification: "none", "sms", "bankId". Default: "none".
  * authenticationOnSignature (string, Required): Signing verification: "none", "sms", "bankId". Default: "none".
  * emailBody (string, Optional): Personal message override.
  * birthdate (string, Optional): Date of birth (YYYY-MM-DD).
  * company (string, Optional): Organization name.
  * function (string, Optional): Job title/function.
  * contractingParty (string, Optional): Legal role. Example: "Buyer".
  * identificationNumber (string, Optional): Organization ID.
  * birthnumber (string, Optional): Personal identity number.
  * address (string, Optional): Mailing address.
  * signingOrder (integer, Optional): Step sequence index (1-based).
  * metadata (string, Optional): Custom metadata.

#### Responses & Status Codes

##### HTTP 201 Created / HTTP 200 OK
Returned when the signer recipient is successfully added.
* **Response Body (JSON)**:
  * id (string): Recipient GUID.
  * status (string): Current status of the recipient.
  * metadata (string): Optional custom metadata.
  * name (string): Recipient's full name.
  * email (string): Recipient's email.
  * mobile (string): Recipient's mobile number.
  * company (string): Organization name.
  * function (string): Job title/function.
  * contractingParty (string): Legal role of recipient.
  * birthdate (string): Date of birth.
  * birthnumber (string): Personal identity number.
  * role (string): Recipient workflow role (e.g. "signer").
  * signatureType (string): Signature level standard.
  * authenticationOnOpen (string): Open action verification level.
  * authenticationOnSignature (string): Signature execution verification level.
  * emailBody (string): Personal email body text.
  * language (string): Default communications language.
  * signingOrder (integer): Order of workflow execution.
  * identificationNumber (string): Organization ID.
  * address (string): Mailing address.
  * createdAt (string): Creation timestamp in ISO 8601.
  * updatedAt (string): Modification timestamp in ISO 8601.

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 7. Add Recipient: Approver (AddRecipientApprover)
Adds an approval-only recipient. Approvers review the documents before they are released to signers, but do not sign them.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * role (string, Required): Set to "approver".
  * name (string, Required): Full name.
  * email (string, Required): Email address.
  * mobile (string, Required): Mobile phone (E.164).
  * approvalMode (string, Required): Mode of approval ("per_envelope", "per_document", "per_document_hidden"). Default: "per_envelope".
  * authenticationOnOpen (string, Required): Verification on open: "none", "sms", "bankId". Default: "none".
  * authenticationOnSignature (string, Required): Verification on approval: "none", "sms", "bankId". Default: "none".
  * emailBody (string, Optional): Personal message.
  * birthdate (string, Optional): Date of birth (YYYY-MM-DD).
  * company (string, Optional): Company name.
  * function (string, Optional): Job title.
  * contractingParty (string, Optional): Legal group description.
  * identificationNumber (string, Optional): Company ID.
  * birthnumber (string, Optional): Personal identity number.
  * address (string, Optional): Mailing address.
  * signingOrder (integer, Optional): Signing order sequence index.
  * metadata (string, Optional): Custom metadata.

#### Responses & Status Codes

##### HTTP 201 Created / HTTP 200 OK
Returned when the approver recipient is successfully added.
* **Response Body (JSON)**:
  * id (string): Recipient GUID.
  * status (string): Current status of the recipient.
  * metadata (string): Optional custom metadata.
  * name (string): Recipient's full name.
  * email (string): Recipient's email.
  * mobile (string): Recipient's mobile number.
  * company (string): Organization name.
  * function (string): Job title/function.
  * contractingParty (string): Legal role of recipient.
  * birthdate (string): Date of birth.
  * birthnumber (string): Personal identity number.
  * role (string): Recipient workflow role (e.g. "approver").
  * signatureType (string): Signature level standard.
  * authenticationOnOpen (string): Open action verification level.
  * authenticationOnSignature (string): Signature execution verification level.
  * emailBody (string): Personal email body text.
  * language (string): Default communications language.
  * signingOrder (integer): Order of workflow execution.
  * identificationNumber (string): Organization ID.
  * address (string): Mailing address.
  * createdAt (string): Creation timestamp in ISO 8601.
  * updatedAt (string): Modification timestamp in ISO 8601.

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 8. Add Recipient: CC (AddRecipientCC)
Adds a recipient to receive final copies of signed documents. CC parties are not part of the active approval or signing process.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * role (string, Required): Set to "cc".
  * name (string, Required): Full name.
  * email (string, Required): Email address.
  * mobile (string, Required): Mobile phone (E.164).
  * birthdate (string, Optional): Date of birth (YYYY-MM-DD).
  * company (string, Optional): Company name.
  * function (string, Optional): Job title.
  * contractingParty (string, Optional): Legal role.
  * identificationNumber (string, Optional): Organization ID.
  * birthnumber (string, Optional): Personal identity number.
  * address (string, Optional): Mailing address.
  * signingOrder (integer, Optional): Sequence index.
  * metadata (string, Optional): Custom metadata.

> [!NOTE]
> **Field Name Discrepancies in Power Automate Designer**:
> Due to differences in the connector's metadata summaries (`x-ms-summary`), some fields are labeled differently in the Power Automate visual designer depending on the recipient type, even though they map to the exact same backend API parameter.
> 
> | API JSON Key | Visually Labeled in **Signer** | Visually Labeled in **Approver** | Visually Labeled in **CC** |
> | :--- | :--- | :--- | :--- |
> | `name` | **Full Name** | **Name** | **Name** |
> | `email` | **Email Address** | **Email** | **Email** |
> | `mobile` | **Mobile Phone** | **Mobile** | **Mobile** |
> | `company` | **Company Name** | **Company** | **Company** |
> | `function` | **Job Title / Position** | **Function** | **Function** |
> | `identificationNumber` | **Company ID** | **Identification Number** | **Identification Number** |
> | `birthnumber` | **Personal ID** | **Birth Number** | **Birth Number** |
> | `authenticationOnOpen` | **Authentication on Open** | **Authentication On Open** | **Authentication On Open** |
> | `authenticationOnSignature` | **Authentication on Signature** | **Authentication On Signature** | *N/A* |




#### Responses & Status Codes

##### HTTP 201 Created / HTTP 200 OK
Returned when the CC recipient is successfully added.
* **Response Body (JSON)**:
  * id (string): Recipient GUID.
  * status (string): Current status of the recipient.
  * metadata (string): Optional custom metadata.
  * name (string): Recipient's full name.
  * email (string): Recipient's email.
  * mobile (string): Recipient's mobile number.
  * company (string): Organization name.
  * function (string): Job title/function.
  * contractingParty (string): Legal role of recipient.
  * birthdate (string): Date of birth.
  * birthnumber (string): Personal identity number.
  * role (string): Recipient workflow role (e.g. "cc").
  * signatureType (string): Signature level standard.
  * authenticationOnOpen (string): Open action verification level.
  * authenticationOnSignature (string): Signature execution verification level.
  * emailBody (string): Personal email body text.
  * language (string): Default communications language.
  * signingOrder (integer): Order of workflow execution.
  * identificationNumber (string): Organization ID.
  * address (string): Mailing address.
  * createdAt (string): Creation timestamp in ISO 8601.
  * updatedAt (string): Modification timestamp in ISO 8601.

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 9. Add Signature Tag (AddTag)
Places a signature box or date/text field onto a linked document for a specific signer.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * document (string, Required): Document IRI path. Example: "/api/envelopes/{envelopeId}/documents/{documentId}".
  * recipient (string, Required): Recipient IRI path. Example: "/api/envelopes/{envelopeId}/recipients/{recipientId}".
  * type (string, Required): Tag type: "signature", "document", "text", "checkbox", "attachment", "radio_button", "approval", "date_of_signature". Default: "signature".
  * placeholder (string, Optional): Text placeholder string in document to position the tag automatically. Example: "{signer1}".
  * page (integer, Optional): Page index (1-based index) if positioning by coordinates.
  * xPosition (integer, Optional): Horizontal coordinate in points.
  * yPosition (integer, Optional): Vertical coordinate in points.
  * positioning (string, Optional): Alignment relative to coordinates or placeholder (e.g. "top_left", "center"). Default: "top_left".
  * required (boolean, Optional): Whether the signer is required to interact with this field. Default: true.
  * scale (integer, Optional): Scale resizing percentage of the tag (20 to 1000). Default: 100.

> [!WARNING]
> **Role Constraints**:
> - Placement tags (signatures, text inputs, checkboxes, approvals) can only be assigned to recipients with the `signer` role (including `signer`, `in_person`, `autosign`, `semi_autosign`).
> - Recipients with the `approver` or `cc` role cannot have any tags assigned. Attempting to place or update a tag for an `approver` or `cc` recipient will fail with a `400 Bad Request` validation error from the Digisign API (e.g., `"Tag with type approval can not be assigned to recipient with role approver."`).

#### Responses & Status Codes

##### HTTP 201 Created / HTTP 200 OK
Returned when the signature tag is successfully created.
* **Response Body (JSON)**:
  * id (string): Unique Tag GUID.
  * scale (integer): Resizing scale.
  * type (string): Tag type.
  * resolved (boolean): Whether placeholder coordinates were successfully resolved.
  * placeholder (string): The text anchor template string.
  * page (integer): Page index.
  * xPosition (integer): Final X coordinate points.
  * yPosition (integer): Final Y coordinate points.
  * positioning (string): Position alignment.
  * required (boolean): Required status.
  * fromTemplate (boolean): Indicates if generated from template.
  * createdAt (string): Creation timestamp.
  * updatedAt (string): Modification timestamp.

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope, document, or recipient is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 10. Send Envelope (SendEnvelope)
Locks the draft envelope and transitions it to the active signing flow. Notifications are automatically sent out.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the envelope is successfully transitioned out of draft and sent.
* **Response Body (JSON)**:
  * *(An empty object is returned).*

##### HTTP 400 Bad Request
Returned if the envelope is missing documents or recipients, or is not in draft state.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 11. Universal API Call (CallApi)
Allows users to make arbitrary direct HTTP requests to the Digisign REST API under the current connector authorization context.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * method (string, Required): HTTP method: GET, POST.
  * path (string, Required): Relative URL path starting with a forward slash. Example: "/api/envelopes".
  * queryString (string, Optional): URL query parameters (e.g. "page=1").
  * body (object, Optional): JSON body to pass along with the request.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the API call is completed successfully.
* **Response Body (JSON)**:
  * *(Returns the dynamic JSON structure returned by the targeted Digisign API endpoint).*

##### HTTP 400 Bad Request
Returned when the requested method or path is invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the specified path does not exist.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.
