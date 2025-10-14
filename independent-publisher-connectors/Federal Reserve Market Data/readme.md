# Federal Reserve Bank of New York Market Data

The Federal Reserve Bank of New York provides access to its Markets Data API, offering detailed information on various financial operations, including Agency Mortgage-Backed Securities (AMBS) Operations, Treasury Securities Operations, and Securities Lending Operations.

## Publisher: Dan Romano (swolcat)

## Prerequisites

- A Microsoft Power Automate account with custom connector capabilities is necessary to utilize this connector.

- Values for parameters can be found here: https://markets.newyorkfed.org/static/docs/markets-api.html

## Obtaining Credentials

- No API key is required to access the New York Fed's Markets Data API.

## Supported Operations

### 1. Retrieve AMBS Operations

Fetches Agency Mortgage-Backed Securities operations based on their status.

- **Endpoint:** `/api/ambs/operation/{status}`
- **Parameters:**
  - `operation` (required): Specifies the operation type (`all`, `purchases`, `sales`, `roll` or `swap`).
  - `status` (required): Specifies the operation status (`announcements` or `results`).
  - `include` (required): Option to include only the latest operation (`summary` or `details`).
  - `format` (required): Determines the response data format (`json`, `xml`, `csv`,`xlsx`).

### 2. Retrieve Treasury Securities Operations

Fetches Treasury Securities operations based on their status.

- **Endpoint:** `/api/tsy/operation/{status}`
- **Parameters:**
  - `operation` (required): Specifies the operation type (`all`, `purchases` or `sales`).
  - `status` (required): Specifies the operation status (`announcements`, `results` or `operations`).
  - `include` (required): Option to include only the latest operation (`summary` or `details`).
  - `format` (required): Determines the response data format (`json`, `xml`, `csv` or `xlsx`).

### 3. Retrieve Securities Lending Operations

Fetches Securities Lending operations based on their status.

- **Endpoint:** `/api/secLending/operation/{status}`
- **Parameters:**
  - `operation` (required): Specifies the operation status (`all`, `seclending` or `completed`).
  - `include` (required): Option to include only the latest operation (`summary` or `details`).
  - `format` (required): Determines the response data format (`json`, `xml`, `csv` or `xlsx`).

## Known Issues and Limitations

- The API is read-only and does not support data modification operations.
- Response times may vary based on the volume of data requested.
- Ensure that the `status` parameter is correctly specified to avoid errors.
