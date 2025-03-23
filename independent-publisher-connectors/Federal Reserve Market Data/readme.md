# Federal Reserve Bank of New York Market Data

The Federal Reserve Bank of New York provides access to its Markets Data API, offering detailed information on various financial operations, including Agency Mortgage-Backed Securities (AMBS) Operations, Treasury Securities Operations, and Securities Lending Operations.

## Publisher: Dan Romano (swolcat)

## Prerequisites

- A Microsoft Power Automate account with custom connector capabilities is necessary to utilize this connector.

## Obtaining Credentials

- No API key is required to access the New York Fed's Markets Data API.

## Supported Operations

### 1. Retrieve AMBS Operations

Fetches Agency Mortgage-Backed Securities operations based on their status.

- **Endpoint:** `/api/ambs/operation/{status}`
- **Parameters:**
  - `status` (required): Specifies the operation status (`announced` or `completed`).
  - `include` (optional): Option to include only the latest operation (`latest`).
  - `format` (optional): Determines the response data format (`json` or `xml`).

### 2. Retrieve Treasury Securities Operations

Fetches Treasury Securities operations based on their status.

- **Endpoint:** `/api/tsy/operation/{status}`
- **Parameters:**
  - `status` (required): Specifies the operation status (`announced` or `completed`).
  - `include` (optional): Option to include only the latest operation (`latest`).
  - `format` (optional): Determines the response data format (`json` or `xml`).

### 3. Retrieve Securities Lending Operations

Fetches Securities Lending operations based on their status.

- **Endpoint:** `/api/secLending/operation/{status}`
- **Parameters:**
  - `status` (required): Specifies the operation status (`announced` or `completed`).
  - `include` (optional): Option to include only the latest operation (`latest`).
  - `format` (optional): Determines the response data format (`json` or `xml`).

## Known Issues and Limitations

- The API is read-only and does not support data modification operations.
- Response times may vary based on the volume of data requested.
- Ensure that the `status` parameter is correctly specified to avoid errors.
