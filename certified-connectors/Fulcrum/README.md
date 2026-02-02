# Fulcrum

Fulcrum is a field-first solution that empowers both GIS experts and non-GIS team members to easily capture and share geospatial data.
This connector enables integration with Fulcrum for managing field data, photos, videos, and more.

## Publisher

Fulcrum

## Prerequisites

- Active Fulcrum subscription with API access enabled

## Supported Operations

### Triggers

- When a Fulcrum event occurs

### Actions

- Get a list of attachment metadata
- Get an attachment
- Get a list of audio metadata
- Get an audio original file
- Get a list of photo metadata
- Get a photo original file
- Get photo metadata
- Make a Query POST request
- Get a list of records
- Create a record
- Delete a record
- Get a record
- Partially update a record
- Update a record
- Get the history of a record
- Create a report
- Get a report file
- Get a list of signature metadata
- Get signature metadata
- Get a signature original file
- Get a list of sketch metadata
- Get a sketch original file
- Get sketch metadata
- Get a list of video metadata
- Get a video original file

## Getting Started

Create a new connection in Power Automate and enter your API token when prompted.

### Custom Host URLs

By default, the connector uses the production Fulcrum API at `api.fulcrumapp.com`. For other regions, you can specify a different host URL when creating your connection.

**Regional Endpoints:**
- United States (default): `api.fulcrumapp.com`
- Canada: `api.fulcrumapp-ca.com`
- Australia: `api.fulcrumapp-au.com`
- Europe: `api.fulcrumapp-eu.com`

**Format:** Enter only the hostname without protocol or path. The connector will automatically use HTTPS and the correct API path.

**Troubleshooting:**
- Ensure your custom host is accessible from your network
- Verify the hostname is correct (no typos)
- Confirm your API token is valid for the specified host

## Known Issues and Limitations

- Rate limiting applies based on your Fulcrum plan
