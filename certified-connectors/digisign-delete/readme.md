# Digisign Custom Connector – DELETE (Documentation)

Author: Lukas Toman  
Developer: Lukas Toman  
Date: June 2026  

This is the official technical documentation for the Digisign - Delete module, designed for the Microsoft Power Platform Custom Connector (Power Automate and Power Apps). This connector focuses on envelope and document deletion and allows exchanging credentials for a token, discarding envelope drafts, deleting documents, deleting recipients, deleting signature tags, and making restricted universal API calls (GET/DELETE). This document satisfies Microsoft certification requirements and provides a developer guide for deleting and discarding elements from draft envelopes.

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
| 2 | discardEnvelope | Discard Envelope Draft | POST | /api/envelopes/{envelopeId}/discard |
| 3 | deleteDocument | Delete Document | DELETE | /api/envelopes/{envelopeId}/documents/{documentId} |
| 4 | deleteRecipient | Delete Recipient | DELETE | /api/envelopes/{envelopeId}/recipients/{recipientId} |
| 5 | deleteTag | Delete Signature Tag | DELETE | /api/envelopes/{envelopeId}/tags/{tagId} |
| 6 | callApi | Universal API Call | POST | /api/call-api |

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

### 2. Discard Envelope Draft (DiscardEnvelope)
Discards and permanently deletes a draft envelope and all of its associated documents, recipients, and tags.

> [!IMPORTANT]
> **Limitations & Constraints**:
> - Only envelopes in the draft state can be discarded. Envelopes that have already been sent, completed, or cancelled cannot be discarded.
> - Already discarded drafts will still appear in the list returned by the GetEnvelopes (List Envelopes) action with `status: draft` but will have a populated `discardedAt` timestamp. When implementing bulk cleanup or deletion loops, always verify that the envelope's `status` is `draft` AND `discardedAt` is `null` (or empty) before calling this discard action to prevent validation errors (`400 Bad Request`).
> - **Note on Permanent Purging**: The `discard` operation acts as a "move to trash" action in the DigiSign portal. Discarded envelopes are not permanently deleted from the database immediately. There is no API endpoint to programmatically empty the trash or purge envelopes permanently. Discarded envelopes will remain in the trash until they are permanently removed either manually via the portal, or automatically after the platform's retention period.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the envelope draft is successfully removed.
* **Response Body (JSON)**:
  * *(An empty JSON object is returned).*

##### HTTP 400 Bad Request
Returned when input parameters are invalid or constraints are violated (e.g. envelope is not in draft state).

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 3. Delete Document (DeleteDocument)
Removes and deletes a linked document from an envelope draft.

> [!IMPORTANT]
> **Limitations & Constraints**:
> - Deleting a linked document is only permitted while the envelope is in the draft state. Once the envelope has been sent, completed, or cancelled, documents cannot be deleted.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * documentId (string, Required): Unique Document GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 204 No Content
Returned when the document is successfully removed.
* **Response Body**:
  * *(No response body content is returned).*

##### HTTP 400 Bad Request
Returned when parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or document is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 4. Delete Recipient (DeleteRecipient)
Removes a recipient (Signer, Approver, or CC) from an envelope draft.

> [!IMPORTANT]
> **Limitations & Constraints**:
> - Deleting a recipient is only permitted while the envelope is in the draft state. Once the envelope has been sent, completed, or cancelled, recipients cannot be deleted.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * recipientId (string, Required): Unique Recipient GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 204 No Content
Returned when the recipient is successfully removed.
* **Response Body**:
  * *(No response body content is returned).*

##### HTTP 400 Bad Request
Returned when parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or recipient is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 5. Delete Signature Tag (DeleteTag)
Removes a signature tag, date field, or text input box from an envelope draft.

> [!IMPORTANT]
> **Limitations & Constraints**:
> - Deleting a signature tag is only permitted while the envelope is in the draft state. Once the envelope has been sent, completed, or cancelled, tags cannot be deleted.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * tagId (string, Required): Unique Tag GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 204 No Content
Returned when the signature tag is successfully removed.
* **Response Body**:
  * *(No response body content is returned).*

##### HTTP 400 Bad Request
Returned when parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or tag is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 6. Universal API Call (CallApi)
Allows users to make arbitrary direct HTTP requests to the Digisign REST API under the current connector authorization context.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * method (string, Required): HTTP method: GET, DELETE.
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
