# U.S. Patent and Trademark Office (Independent Publisher)

## Publisher: Dan Romano (swolcat)

## Prerequisites

You must have a registered developer account with the [U.S. Patent and Trademark Office](https://developer.uspto.gov) to obtain an API key.  

You can sign up and retrieve your key from the **USPTO API Developer Hub**.

## Supported Operations

The connector provides access to USPTO’s patent bibliographic and file wrapper data services.  
It supports both **query parameter (GET)** and **structured JSON payload (POST)** searches for flexible, high-volume patent data retrieval.

### Patent Search

- `GET /applications/search` - Search for patent applications using query parameters (`q`, `filters`, `rangeFilters`, `sort`, etc.).  
  Returns the top 25 records by default when no parameters are specified.  

- `POST /applications/search`- Perform an advanced search using a structured JSON payload with filters, range filters, pagination, and field selection.  
  Example:

- `GET /applications/search/download` - Retrieve search results in download format using query parameters.

- `POST /applications/search/download` - Download the search results in json or csv format using a similar payload as the search POST endpoint.

### Patent Application Details

Each of the following endpoints retrieves specific data associated with a given patent application number ({applicationNumberText}):

- `GET /{applicationNumberText}` - Get patent by application number

- `GET /{applicationNumberText}/meta-data` – Bibliographic metadata for a patent application.

- `GET /{applicationNumberText}/adjustment` – Term adjustment information.

- `GET /{applicationNumberText}/assignment` – Assignment and ownership data.

- `GET /{applicationNumberText}/attorney` – Attorney and correspondence details.

- `GET /{applicationNumberText}/continuity` – Continuity data (parent and child applications).

- `GET /{applicationNumberText}/foreign-priority` – Foreign priority claim details.

- `GET /{applicationNumberText}/transactions` – Application transaction history.

- `GET /{applicationNumberText}/documents` – Published file wrapper documents.

- `GET /{applicationNumberText}/associated-documents` – Related publication (PGPub or grant) metadata.

### Patent Reference Data

- `GET /patent/status-codes` – Retrieve status codes and their definitions used in USPTO records.

- `POST /patent/status-codes` – Search patent application status codes via JSON payload.

### Dataset Search

- `GET /datasets/products/search` – Search for USPTO product datasets.

- `GET /datasets/products/{productIdentifier}` – Retrieve metadata for a specific dataset.

### Petition Decision Data

- `GET /petition/decisions/search` – Search petition decision records (query parameters).

- `POST /petition/decisions/search` – Search petition decision records (JSON).

- `GET /petition/decisions/search/download` – Download petition decisions (query parameters).

- `POST /petition/decisions/search/download` – Download petition decisions (JSON).

- `GET /petition/decisions/{petitionDecisionRecordIdentifier}` – Retrieve a specific petition decision record.

### Text-to-Search

- `POST /patent/applications/text-to-search` – Accept text input and return matching patent application results.

## Obtaining Credentials

- Official USPTO API Reference can be found [here](https://developer.uspto.gov/api-catalog).

## Getting Started

[Official site](https://data.uspto.gov/home)

[Official USPTO API Reference](https://data.uspto.gov/swagger/index.html)

- Navigate to the USPTO Developer Hub.
- Register or log in to your developer account.
- Request an API key for the Patent Data APIs.Note: API Key is required. Obtain an API key [here](https://data.uspto.gov/myodp).
- Copy your API key and store it securely. You will use it to authenticate requests.

When creating the connection in Power Platform, enter your USPTO API Key. The connector automatically sends this key in the request header as `x-api-key: {your-api-key}`

## Known Issues and Limitations

1.) Request limitations

The USPTO may rate-limit API usage depending on request volume.

- The /search/download GET endpoint may not be available in sandbox mode; use the POST variant instead.

- Query timeouts may occur when using complex filters or large date ranges.

- Data freshness depends on USPTO’s internal publication schedule.

2.) Transitioning

- From the website: "As of March 14, 2025, Patent Examination Data System (PEDS) is no longer available. To access publicly available records of USPTO patent applications or patent filing status, users can access Open Data Portal’s Patent File Wrapper feature."

- [Read more here[(https://data.uspto.gov/apis/transition-guide/bdss).

3.) Text-to-Search endpoint

The Text-to-Search endpoint can be found in the Swagger doc, but it is not listed in the public site. That endpoint is not included in this connector.

Consider using `/api/v1/patent/applications/search` as an alternative.

4.) Product identifier codes

For bulk data endpoints, a product identifier is required. Examples can be found [here](https://data.uspto.gov/bulkdata/datasets).