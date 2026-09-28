# OpenSanctions

## Overview

The OpenSanctions API provides access to a global database of persons and companies of political, criminal, or economic interest. It aggregates data from hundreds of sanctions lists, politically exposed persons (PEPs),
and watchlists across the world.

You can use this connector to match, search, and fetch entities, explore relationships, and analyze dataset coverage.

Learn more at [OpenSanctions.org](https://www.opensanctions.org/).

## Publisher - Dan Romano (swolcat)

## Prerequisites

You will need an API key from OpenSanctions to use this connector. Register for a free key at:
[https://www.opensanctions.org/api/](https://www.opensanctions.org/api/)

## Supported Operations
### Match entities by dataset

Match entities based on name and optional fields such as birth date, nationality, and identifiers. This endpoint supports fuzzy matching.

### Search entities in a dataset

Search entities using a simple text query. Supports filters, topics, countries, and datasets for refined searches.

### Get entity by ID

Retrieve a single entity record by its unique identifier, including nested and related attributes.

### Get adjacent entities

Return entities adjacent to a specified entity (e.g., associates, family members, or related companies).

### Get adjacent entities by property

Return related entities for a specific property (e.g., familyPerson, ownershipOwner, membershipMember).

### Get dataset catalog

Retrieve the list of all indexed datasets available via OpenSanctions.

### Get system health

Perform a system health check to verify service availability.

### Get matching algorithms

List supported entity-matching algorithms used by the system.

### Force index update

Trigger a re-indexing operation if authorized with a valid update token.

## Getting Started

1. Create an account at [OpenSanctions.org](https://www.opensanctions.org/).
2. An API key
3. In Power Automate or Power Apps, create a new connection using your API key.
4. Use the connector’s actions to search, match, or fetch entities.

## Obtaining Credentials

Visit [https://www.opensanctions.org/api/](https://www.opensanctions.org/api/) to request an API key. You’ll receive an `ApiKey` value that should be used in the `Authorization` header:

Authorization: ApiKey YOUR_API_KEY

## Known Issues and Limitations

- Response structures are dynamic; use "Parse JSON" in Power Automate to access nested values.
- The `search` and `match` endpoints may return large responses depending on the dataset scope.
- Some endpoints require a valid `dataset` parameter (e.g., `sanctions`).
- Sorting by unsupported fields (like `score`) may trigger errors.
- API rate limits may apply depending on your plan.