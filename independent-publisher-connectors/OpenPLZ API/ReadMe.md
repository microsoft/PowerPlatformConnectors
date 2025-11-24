
# OpenPLZ API (Independent Publisher)

## Summary

The **OpenPLZ API** is an open data REST service that provides a public street and postal code directory for **Austria, Germany, Liechtenstein, and Switzerland**. It exposes:

- Administrative divisions (e.g. federal states, cantons, districts, municipalities)
- Postal codes and localities
- Streets
- Country-specific full-text search endpoints

The data comes from public sources, is regularly updated, and is available under the ODbL open database license. Usage is free of charge, including for commercial projects.

Official documentation:  
`https://www.openplzapi.org/`  
English docs: `https://www.openplzapi.org/en/`  
API overview: `https://www.openplzapi.org/en/api/`  

---

## Publisher

STÜBER SYSTEMS GmbH - OpenPLZ API project

Project website: `https://www.openplzapi.org/`  

---

## Prerequisites

- A Microsoft Power Apps or Power Automate environment.
- Network access to `https://openplzapi.org`.
- No API key or authentication is required (public open data API).

---

## Connection

**Authentication type:** None  
**Base URL:** `https://openplzapi.org`

The API supports **JSON** responses (default and recommended) and also **CSV**. In the connector, use `Accept: text/json` for all actions.

---

## Supported Operations

> Note: The exact list of actions depends on how you map the OpenAPI definition into the custom connector. The operations below describe the main API groups you typically expose.

### 1. Administrative Units

Retrieve subnational administrative units for each country.

Typical endpoints (examples for Germany):

- `GET /de/FederalStates`  
  List all German federal states.

- `GET /de/FederalStates/{stateCode}/GovernmentRegions`  
  List government regions for a specific state.

- `GET /de/FederalStates/{stateCode}/Districts`  
  List districts for a state.

- `GET /de/FederalStates/{stateCode}/Municipalities`  
  List municipalities for a state.

- Similar endpoints exist for Austria (`/at/...`), Switzerland (`/ch/...`) and Liechtenstein (`/li/...`), e.g.:
  - `GET /at/FederalProvinces`
  - `GET /ch/Cantons`
  - `GET /li/Communes`

**Typical use cases**

- Lookups for structured address data (e.g. select a federal state, then district, then municipality).
- Building reference tables in Dataverse or Excel for downstream address validation.

---

### 2. Postal Codes and Localities

Search and list postal codes and localities.

Example endpoints (Germany):

- `GET /de/Localities?postalCode={postalCode}`  
  Get all localities for a given postal code.

- `GET /de/Localities?postalCode={postalCode}&name={localityName}`  
  Filter by postal code and/or locality name. Both parameters support regular expressions.

- `GET /de/FederalStates/{stateCode}/Localities`  
  List all localities in a federal state (paged).

Equivalent endpoints exist for the other countries using `at`, `ch`, and `li` prefixes.

**Typical use cases**

- Validate the combination of postal code and locality.
- Populate dropdowns (e.g. user types postal code, app loads matching localities).

---

### 3. Streets

Search and list streets for a given locality and/or postal code.

Example endpoints (Germany):

- `GET /de/Streets?name={streetName}&postalCode={postalCode}&locality={localityName}`  
  Search streets by name, postal code, and/or locality. All parameters support regular expressions.

- Country-specific variants:
  - `GET /at/Streets?...`
  - `GET /ch/Streets?...`
  - `GET /li/Streets?...` (if available in the live API)

**Typical use cases**

- Address autocompletion in Power Apps.
- Validation flows in Power Automate to check if a street exists for a given locality and postal code.

---

### 4. Full-Text Search

Perform a combined full-text search across street, postal code, and locality.

Example endpoints:

- `GET /de/FullTextSearch?searchTerm={searchTerm}`
- `GET /at/FullTextSearch?searchTerm={searchTerm}`
- `GET /ch/FullTextSearch?searchTerm={searchTerm}`
- `GET /li/FullTextSearch?searchTerm={searchTerm}`

`searchTerm` can be a free-form string such as  
`Berlin, Pariser Platz` or `9490 Alte Landstrasse`.

**Typical use cases**

- Single search box in apps (user types “10115 Berlin, Invalidenstrasse” → connector returns matching rows).
- Quick lookup flows in Power Automate where the exact split into postal code / locality / street is not known.

---

## Parameters and Query Options

### Common query parameters

- `postalCode` (string) – postal code filter, supports regular expressions.
- `name` (string) – locality or street name, supports regular expressions.
- `locality` (string) – locality name, supports regular expressions.
- `searchTerm` (string) – free-form term for full-text search.
- `page` (integer) – page index (1-based, default `1`).
- `pageSize` (integer) – number of items per page (default `50`, max `50`).

### Regular expressions

For many search endpoints, query parameters such as `postalCode`, `name`, and `locality` support **POSIX regular expressions**.  
Examples:

- All German postal codes starting with `13`:
  - `GET /de/Localities?postalCode=%5E13` (URL-encoded `^13`)
- All streets in Berlin starting with `G` and ending with `allee`:
  - `GET /de/Streets?name=%5EG.*allee%24&locality=Berlin` (URL-encoded `^G.*allee$`)

When using the connector, ensure the expressions are URL-encoded. In most cases, Power Platform will handle encoding for you when parameters are passed as normal text.

---

## Pagination

Most endpoints are paged.

### Request

Use these query parameters:

- `page` – Page number (1-based). Default: `1`.
- `pageSize` – Items per page. Default: `50`. Maximum: `50`.

Example:

```http
GET https://openplzapi.org/de/Streets?locality=Berlin&page=1&pageSize=10

