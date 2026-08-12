# Digisign Custom Connector – UPDATE (Documentation)

Author: Lukas Toman  
Developer: Lukas Toman  
Date: June 2026  

This is the official technical documentation for the Digisign - Update module, designed for the Microsoft Power Platform Custom Connector (Power Automate and Power Apps). This connector focuses on envelope and document updates and allows exchanging credentials for a token, updating envelopes, documents, recipients, signature tags, notifications, and making restricted universal API calls (GET/PUT/PATCH). This document satisfies Microsoft certification requirements and provides a developer guide for updating active elements inside draft envelopes.

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
The custom code script (script.csx) transparently manages your API session:
1. It intercepts credentials passed via standard Basic Authentication.
2. It makes a background token request to POST /api/auth-token using the keys.
3. The resulting short-lived Bearer token is cached and attached as an Authorization: Bearer <token> header to all subsequent API requests.
4. The target server host is dynamically rewritten based on the Environment URL parameter chosen in the connection settings.

No manual token generation, refreshing, or management is required within Power Automate flows.

---

## Operations Overview

| # | Operation ID | Display Name (Power Automate) | HTTP Method | API Endpoint |
| :--- | :--- | :--- | :--- | :--- |
| 1 | authToken | Exchange Keys for Bearer Token | POST | /api/auth-token |
| 2 | updateEnvelope | Update Envelope | PUT | /api/envelopes/{envelopeId} |
| 3 | updateDocument | Update Document | PUT | /api/envelopes/{envelopeId}/documents/{documentId} |
| 4 | updateRecipient | Update Recipient | PUT | /api/envelopes/{envelopeId}/recipients/{recipientId} |
| 5 | updateTag | Update Signature Tag | PUT | /api/envelopes/{envelopeId}/tags/{tagId} |
| 6 | updateNotification | Update Notification | PUT | /api/envelopes/{envelopeId}/notifications/{notificationId} |
| 7 | callApi | Universal API Call | POST | /api/call-api |

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
Returned when request parameters or body are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when endpoint is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 2. Update Envelope (UpdateEnvelope)
Updates properties of an existing draft envelope.

> [!IMPORTANT]
> **Limitations & Constraints**:
> - Envelopes can only be updated while in the draft state. Once sent, declined, or completed, they become immutable.
> - The sender (envelope owner) field cannot be modified during updates. Attempting to change the sender will return an API error.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * name (string, Optional): Name of the envelope (max 255 chars). Example: "Investment Term Sheet - Series A - Updated".
  * emailBody (string, Optional): Default email message body sent to recipients (max 4096 chars).
  * emailBodyCompleted (string, Optional): Email message body sent once signing is completed.
  * senderName (string, Optional): Display name of the sender.
  * senderEmail (string, Optional): Contact email address of the sender.
  * expiration (integer, Optional): Days until expiration (0 - 365).
  * metadata (string, Optional): Custom metadata string (max 4096 chars).

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the envelope is successfully updated.
* **Response Body (JSON)**:
  * id (string): Envelope GUID.

##### HTTP 400 Bad Request
Returned when input properties are invalid or constraints are violated.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 3. Update Document (UpdateDocument)
Updates the properties (such as display name, display position index, or metadata) of a document linked to a draft envelope.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * documentId (string, Required): Unique Document GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * name (string, Optional): Display name of the document. Example: "Updated_Contract_v2.pdf".
  * position (integer, Optional): Display order position (1-based index).
  * metadata (string, Optional): Custom metadata string.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the document properties are successfully updated.
* **Response Body (JSON)**:
  * *(An empty JSON object is returned).*

##### HTTP 400 Bad Request
Returned when parameters or request body properties are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or document is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 4. Update Recipient (UpdateRecipient)
Updates the properties of a recipient in the envelope. This operation is universal across all recipient roles (Signer, Approver, CC). Simply send the fields that need modification.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * recipientId (string, Required): Unique Recipient GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * name (string, Optional): Recipient's full name. Example: "John Smith Jr."
  * email (string, Optional): Recipient's email address. Example: "john.smith.jr@company.com"
  * mobile (string, Optional): Mobile phone number in E.164 format. Example: "+420777999888".
  * role (string, Optional): Recipient role: "signer", "in_person", "cc", "approver", "autosign", "semi_autosign".
  * signatureType (string, Optional): Signature level: "simple", "biometric", "qualified".
  * authenticationOnOpen (string, Optional): Access control verification: "none", "sms", "bankId".
  * authenticationOnSignature (string, Optional): Verification on signing/approval: "none", "sms", "bankId".
  * emailBody (string, Optional): Personal message override.
  * birthdate (string, Optional): Date of birth (YYYY-MM-DD).
  * company (string, Optional): Organization name. Example: "Bohemian Capital"
  * function (string, Optional): Job title/function. Example: "CEO"
  * contractingParty (string, Optional): Legal role description. Example: "Investor"
  * identificationNumber (string, Optional): Company Registration ID.
  * birthnumber (string, Optional): Personal Identity Number. Example: "800515/1234"
  * address (string, Optional): Mailing address. Example: "1 Main Street, Prague"
  * signingOrder (integer, Optional): Step sequence index (1-based).
  * metadata (string, Optional): Custom metadata.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the recipient is successfully updated.
* **Response Body (JSON)**:
  * *(An empty JSON object is returned).*

##### HTTP 400 Bad Request
Returned when input properties are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or recipient is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 5. Update Signature Tag (UpdateTag)
Updates the properties or relocates an existing signature field, date field, or text input box.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * tagId (string, Required): Unique Tag GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * type (string, Optional): Tag type: "signature", "date", "text", "checkbox".
  * page (integer, Optional): Page index (1-based index).
  * xPosition (integer, Optional): Horizontal coordinate (points from left margin).
  * yPosition (integer, Optional): Vertical coordinate (points from top margin).
  * label (string, Optional): Text prompt or tooltip. Example: "Investor Signature - Updated".
  * required (boolean, Optional): Whether the field is mandatory.
  * metadata (string, Optional): Custom metadata.

> [!WARNING]
> **Role Constraints**:
> - Placement tags (signatures, text inputs, checkboxes, approvals) can only be assigned to recipients with the `signer` role (including `signer`, `in_person`, `autosign`, `semi_autosign`).
> - Recipients with the `approver` or `cc` role cannot have any tags assigned. Attempting to place or update a tag for an `approver` or `cc` recipient will fail with a `400 Bad Request` validation error from the Digisign API (e.g., `"Tag with type approval can not be assigned to recipient with role approver."`).

> [!NOTE]
> **Coordinate Alignment**:
> Under certain conditions, updating the coordinates xPosition and yPosition might result in them being returned as null in the immediate JSON response due to specialized rendering calculations on Digisign's API. However, the positions are successfully updated on the document.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the tag properties are successfully updated.
* **Response Body (JSON)**:
  * *(An empty JSON object is returned).*

##### HTTP 400 Bad Request
Returned when parameters or request body properties are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or tag is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 6. Update Notification (UpdateNotification)
Updates the scheduling rules for envelope notifications and automatic reminders.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * notificationId (string, Required): Unique Notification GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * type (string, Optional): Notification type: "toSignAfterSent", "toSignBeforeExpires".
  * days (integer, Optional): Interval days for reminder dispatch.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the notification setting is successfully updated.
* **Response Body (JSON)**:
  * *(An empty JSON object is returned).*

##### HTTP 400 Bad Request
Returned when parameters or request body properties are invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or notification setting is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 7. Universal API Call (CallApi)
Allows users to make arbitrary direct HTTP requests to the Digisign REST API under the current connector authorization context.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * method (string, Required): HTTP method: GET, PUT, PATCH.
  * path (string, Required): Relative URL path starting with a forward slash. Example: "/api/envelopes/019efb93-9b6b-735d-a5a8-e30382b765d1".
  * queryString (string, Optional): URL query parameters (e.g. "embed=documents,recipients").
  * body (object, Optional): JSON body to pass along with the request.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the API call is completed.
* **Response Body (JSON)**:
  * *(Returns the dynamic JSON structure returned by the targeted Digisign API endpoint).*

##### HTTP 400 Bad Request
Returned when parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when endpoint is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.
