# US Bureau of Economic Analysis (BEA)

The U.S. Bureau of Economic Analysis (BEA) provides access to critical economic datasets including GDP, regional data, international trade, and industry statistics.

## Publisher: Dan Romano (@krautrocker)

## Prequisites:

- API key - BEA API Signup: https://apps.bea.gov/API/signup/

---

## Supported Operations

This connector supports:

### Metadata Operations
- Get Dataset List
- Get Parameter List

### Dataset-Specific Data Retrieval
- NIPA (National Income and Product Accounts)
- NIUnderlyingDetail
- MNE (Multinational Enterprises)
- FixedAssets
- ITA (International Transactions Accounts)
- IIP (International Investment Position)
- InputOutput
- IntlServTrade
- IntlServSTA
- GDPbyIndustry
- UnderlyingGDPbyIndustry
- Regional
- APIDatasetMetaData

Each dataset is exposed as a **separate action**, making it easier for users to work with the BEA API without needing to manually construct requests.

---

## Obtaining Credentials

- BEA API Signup: https://apps.bea.gov/API/signup/

## Getting Started

- BEA API Signup: https://apps.bea.gov/API/signup/

The BEA API requires a **User ID (API key)**.

1. Register at: https://apps.bea.gov/API/signup/
2. Obtain your API key (UserID)
3. Provide it when configuring the connector

- BEA API Documentation: https://apps.bea.gov/api/_pdf/bea_web_service_api_user_guide.pdf

This provides detailed information on available datasets, parameters, and usage guidelines.

## Frequently Asked Questions

1.) How does the API work?

This connector uses a **Route Request policy** to:

- Map friendly action paths (e.g., `/GetDataNIPA`)
- To the actual BEA endpoint (`/data`)

This allows Power Platform users to interact with BEA using standard actions instead of manually constructing query strings.

Unlike traditional REST APIs with multiple endpoints, the BEA API uses a **single endpoint (`/data`)** and requires dynamically constructed query parameters to retrieve specific datasets.

Actions are **dataset-specific** that map cleanly to Power Platform operations.

- Single endpoint: https://apps.bea.gov/api/data

- Requires:
- `method`
- `datasetname`
- dataset-specific parameters
- `UserID`

---

## Known Limitations

- BEA uses a **non-standard API design** (single endpoint with parameter routing)
- Some parameters, such as Year, may not always be of integer type.