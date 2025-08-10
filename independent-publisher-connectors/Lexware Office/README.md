
# Lexware API Connector (Independent Publisher)

The Lexware API is a RESTful interface allowing developers to integrate Lexware business functions such as contacts, invoices, articles, and files into their own applications. This connector enables direct interaction with these resources through secure, authenticated API requests.

## Publisher: Independent Publisher

## Prerequisites
- A registered Lexware Developer account
- A valid API key (access token) obtained via the Lexware Developer Portal
- OAuth 2.0 Authorization Code Flow is required for authentication

## Supported Operations
This connector supports the following Lexware API endpoints:
- **Articles**: Create, retrieve, update, delete, and filter articles
- **Contacts**: Manage customer and vendor data
- **Invoices**: Create and retrieve invoice data, including rendering PDFs
- **Credit Notes, Delivery Notes, Orders, Quotations, and Vouchers**
- **Countries, Payment Conditions, Print Layouts, and Profiles**
- **Event Subscriptions and Files**
- Full list documented at: [Lexware API Docs](https://developers.lexware.io/docs/)

## Obtaining Credentials
Register your app at [Lexware Developer Portal](https://app.lexware.de/addons/public-api) to receive:
- `Client ID`
- `Client Secret`
- OAuth redirect URI

These are needed to generate a valid access token via OAuth 2.0.

## API Gateway
Please use the new Lexware API gateway:
```
https://api.lexware.io
```

## Rate Limits
- **Max 2 requests/second**
- Requests exceeding this limit receive HTTP `429 Too Many Requests`
- Recommended: Use token bucket algorithm or exponential backoff

## Authentication
The Lexware API uses OAuth 2.0 Authorization Code Flow.
Include the token in the `Authorization: Bearer {accessToken}` header of each request.

## Example Usage
Creating an article:
```bash
curl https://api.lexware.io/v1/articles \
  -X POST \
  -H "Authorization: Bearer {accessToken}" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Lexware Buchhaltung Premium 2024",
    "type": "PRODUCT",
    "unitName": "Download-Code",
    "articleNumber": "LXW-BUHA-2024-001",
    "price": {
      "netPrice": 61.90,
      "leadingPrice": "NET",
      "taxRate": 19
    }
  }'
```

## Known Issues and Limitations
- Max 2 requests/second
- Some endpoints require additional permissions or roles
- Token expiry and renewal must be handled in your app

## Deployment Instructions
1. Import connector to Power Platform or Logic Apps
2. Configure OAuth 2.0 settings (Client ID, Secret, Redirect URI)
3. Test API access and handle all status codes (e.g. 401, 429)
4. Optional: Customize icons and metadata for publication

## API Documentation
Full API reference is available at:  
[https://developers.lexware.io/docs/](https://developers.lexware.io/docs/)
