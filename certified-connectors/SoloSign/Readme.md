# SoloSign API Documentation

Welcome to the SoloSign API documentation. This guide will help you get started with our API, providing you with the information you need to integrate SoloSign into your applications.

## Table of Contents

- [Overview](#overview)
- [Authentication](#authentication)
- [Endpoints](#endpoints)
- [Usage](#usage)
- [Error Handling](#error-handling)
- [Contact](#contact)

## Overview

SoloSign is a powerful API that allows you to generate HMAC signatures for your data. It is designed to provide a secure way to sign and authenticate your requests.

## Authentication

Authentication is a crucial part of using the SoloSign API. To access our API, you need to include your API key in the request header.

### Request Header

- **X-Api-Key**: Your unique API key that you obtain by registering with SoloSign.

## Endpoints

The SoloSign API provides the following endpoint:

- `/generate-hmac` (POST): Generates an HMAC signature for the given request data.

## Usage

To use the SoloSign API, follow these steps:

1. **Get an API Key**: Contact our team to obtain an API key https://solort.com/solosign.
2. **Include the API Key**: Make sure to include your API key in the `X-Api-Key` request header for all API requests or simply add it when prompted in your flow.
3. **Generate HMAC Signature**: Use the `/generate-hmac` endpoint to create HMAC signatures for your data.

### Error Handling
- 400 Missing API Key: API key missing in request header
- 401 Unauthorized: Invalid API Key or Exceeded API Calls Limit
- Error generating HMAC signature
- 500 Internal Server Error

### Example Request

```http
POST /generate-hmac HTTP/1.1
Host: api-domain.com
Content-Type: application/json
X-Api-Key: your-api-key

{
  "request_string": "Your request data to be signed",
  "secret_key": "Your secret key",
  "output_format": "base64",
  "encode_type": "utf-8",
  "hash_algorithm": "sha256"
}
```

### Example Response
```
{
  "hmac_signature": "Generated HMAC Signature"
}
```

## Contact
If you have questions, need assistance, or want to obtain an API key, please contact our support team at support@solort.com.

We are here to help you make the most of SoloSign API in your applications.

Happy hashing!