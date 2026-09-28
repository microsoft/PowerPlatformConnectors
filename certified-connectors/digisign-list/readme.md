# Digisign Custom Connector – LIST (Documentation)

Author: Lukas Toman  
Developer: Lukas Toman  
Date: June 2026  

This is the official technical documentation for the Digisign - List module, designed for the Microsoft Power Platform Custom Connector (Power Automate and Power Apps). This connector focuses on envelope and document retrieval and allows exchanging credentials for a token, listing envelopes, retrieving envelope, document, recipient, and signature tag details, downloading signed envelopes/documents, listing users, viewing account details, and making restricted universal API calls (GET). This document satisfies Microsoft certification requirements and provides a developer guide for listing and retrieving envelopes, documents, recipients, signature tags, users, and account details.

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
| 2 | getEnvelopes | List Envelopes | GET | /api/envelopes |
| 3 | getEnvelope | Get Envelope | GET | /api/envelopes/{envelopeId} |
| 4 | getDocument | Get Document | GET | /api/envelopes/{envelopeId}/documents/{documentId} |
| 5 | getRecipient | Get Recipient | GET | /api/envelopes/{envelopeId}/recipients/{recipientId} |
| 6 | getTag | Get Signature Tag | GET | /api/envelopes/{envelopeId}/tags/{tagId} |
| 7 | downloadEnvelope | Download Signed Envelope | GET | /api/envelopes/{envelopeId}/download |
| 8 | downloadDocument | Download Document | GET | /api/envelopes/{envelopeId}/documents/{documentId}/download |
| 9 | getAccount | Get Account Details | GET | /api/account |
| 10 | getUsers | List Users | GET | /api/account/users |
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

### 2. List Envelopes (GetEnvelopes)
Retrieves a list of envelopes with support for paging and filtering by status, name, or metadata.

> [!NOTE]
> **Note on Discarded Envelopes (Trash)**:
> This operation also retrieves discarded envelopes (envelopes currently in the trash). Discarded envelopes will still have their `status` set to `draft` but will have a populated `discardedAt` timestamp. To filter for active (non-deleted) drafts, ensure you check that `discardedAt` is `null` (or empty).

#### Request Parameters
* **Query Parameters**:
  * page (integer, Optional): Page number of the collection. Default: 1.
  * itemsPerPage (integer, Optional): Number of envelopes to return per page. Default: 30.
  * status (string, Optional): Filter envelopes by status (e.g. draft, sent, completed, cancelled).
  * name (string, Optional): Filter envelopes by partial name match.
  * metadata (string, Optional): Filter envelopes by custom metadata value.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the envelopes list is retrieved successfully.
* **Response Body (JSON)**:
  * `items` (array of objects): A collection of envelope objects, each containing:
    * `id` (string): The unique GUID identifier of the envelope.
    * `name` (string): The descriptive name of the envelope.
    * `status` (string): The current lifecycle state (e.g. `draft`, `sent`, `completed`, `cancelled`).
    * `createdAt` (string): The timestamp when the envelope was created.
    * `discardedAt` (string): The timestamp when the envelope was discarded (null if not discarded).
    * `metadata` (string): Custom metadata string associated with the envelope.

##### HTTP 400 Bad Request
Returned when query parameters are invalid.

##### HTTP 401 Unauthorized
Returned when authentication credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when endpoint is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 3. Get Envelope (GetEnvelope)
Retrieves the details and current status of a specific envelope by its ID.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the envelope details are successfully retrieved.
* **Response Body (JSON)**:
  * *(Returns a dynamic JSON object describing the envelope status, documents, recipients, and properties).*

##### HTTP 400 Bad Request
Returned when the envelope ID format is invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 4. Get Document (GetDocument)
Retrieves details and metadata of a specific document linked within an envelope.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * documentId (string, Required): Unique Document GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the document metadata is successfully retrieved.
* **Response Body (JSON)**:
  * *(Returns a dynamic JSON object describing the document properties, size, and layout settings).*

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or document is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 5. Get Recipient (GetRecipient)
Retrieves details of a specific recipient in an envelope.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * recipientId (string, Required): Unique Recipient GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the recipient details are successfully retrieved.
* **Response Body (JSON)**:
  * *(Returns a dynamic JSON object detailing the recipient name, email, role, status, and authentication properties).*

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or recipient is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 6. Get Signature Tag (GetTag)
Retrieves coordinates and details of a specific signature tag.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * tagId (string, Required): Unique Tag GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the signature tag properties are successfully retrieved.
* **Response Body (JSON)**:
  * *(Returns a dynamic JSON object containing the tag coordinates, page number, label, and required state).*

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or tag is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 7. Download Signed Envelope (DownloadEnvelope)
Downloads the completed signed documents from the envelope as a PDF or ZIP file.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the signed PDF or ZIP binary stream is returned.
* **Response Body (Binary)**:
  * The raw file contents (PDF or ZIP file).

##### HTTP 400 Bad Request
Returned when the envelope ID format is invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 8. Download Document (DownloadDocument)
Downloads a specific document from the envelope in PDF format.

#### Request Parameters
* **Path Parameters**:
  * envelopeId (string, Required): Unique Envelope GUID.
  * documentId (string, Required): Unique Document GUID.
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the PDF document binary stream is returned.
* **Response Body (Binary)**:
  * The raw PDF file stream.

##### HTTP 400 Bad Request
Returned when parameters are invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when the envelope or document is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 9. Get Account Details (GetAccount)
Retrieves authenticated account details to verify connector health and status.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the account details are retrieved successfully.
* **Response Body (JSON)**:
  * *(Returns a dynamic JSON object containing company details, active features, and account limits).*

##### HTTP 400 Bad Request
Returned when environment parameters are invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when endpoint is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 10. List Users (GetUsers)
Retrieves a list of users associated with the authenticated account.

#### Request Parameters
* **Query Parameters**:
  * id (string, Optional): Filter users by their unique ID.
  * email (string, Optional): Filter users by their email address.
  * role (string, Optional): Filter users by their role (e.g. admin, member).
  * status (string, Optional): Filter users by their account status (e.g. active, invited).
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.

#### Responses & Status Codes

##### HTTP 200 OK
Returned when the users list is retrieved.
* **Response Body (JSON)**:
  * items (array): List of user details including `id`, `email`, `firstName`, and `lastName`.

##### HTTP 400 Bad Request
Returned when input parameters are invalid.

##### HTTP 401 Unauthorized
Returned when credentials are invalid.

##### HTTP 403 Forbidden
Returned when access is denied.

##### HTTP 404 Not Found
Returned when endpoint is not found.

##### HTTP 500 Internal Server Error
Returned when a server-side error occurs.

---

### 11. Universal API Call (CallApi)
Allows users to make arbitrary direct HTTP requests to the Digisign REST API under the current connector authorization context.

#### Request Parameters
* **Header Parameters**:
  * x-environment (string, Optional): Target Environment URL. Default: https://api.digisign.org.
* **Body (JSON)**:
  * method (string, Required): HTTP method: GET.
  * path (string, Required): Relative URL path starting with a forward slash. Example: "/api/envelopes".
  * queryString (string, Optional): URL query parameters (e.g. "page=1").
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
