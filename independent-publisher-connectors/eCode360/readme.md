# eCode360 Connector (Independent Publisher)

The eCode360 API provides access to municipality codes hosted by General Code. With this connector, you can query customer information, code structure, content, and perform full-text search.

## Publisher

Dan Romano, IDR Consultants

## Prerequisites

1. You must contact the specific municipality you are targeting to obtain an API key and secret.

## Obtaining Credentials

1. Use your API key and secret as headers.
2. Begin querying customers, structures, or content by GUID.
3. Leverage full-text search to explore available code segments.

## Supported Operations

- `GetCustomers` – List all customers accessible to your key.
- `GetCustomerFromCustId` – Get metadata about a specific municipality.
- `GetCodeStructure` – Get structural metadata for a code GUID.
- `GetCodeContent` – Retrieve content text and metadata.
- `SearchCustomer` – Search codes using advanced search syntax.
- `GetPublicStates` – List states with available customers.

## Known Issues and Limitations

- Each API call must include both the key and secret in headers.
- The response model includes HTML (highlighted search results); handle accordingly in apps or flows.

## Frequently Asked Questions
### How do I obtain API key?
If you work for a municipality or live in a municipality - or even a city - you may be able to contact an administrator who can provide a key to you. An annual subscription
will get you over 3500 unique codes. Learn more here: https://www.generalcode.com/subscription-services/