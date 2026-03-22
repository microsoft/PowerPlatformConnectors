# Fulcrum

Fulcrum is a field-first solution that empowers both GIS experts and non-GIS team members to easily capture and share geospatial data.
This connector enables integration with Fulcrum for managing field data, photos, videos, and more.

## Publisher

Fulcrum

## Prerequisites

- Active Fulcrum subscription with API access enabled

## Supported Operations

### Triggers

#### When a Fulcrum event occurs
Triggers when a Fulcrum resource is created, updated, or deleted. Supports events for records, forms, choice lists, and classification sets. Configure the webhook in your Fulcrum organization to specify which events to monitor.

### Actions

#### Get a list of attachment metadata
Retrieve a list of attachments.

#### Get an attachment
Retrieve metadata for a single attachment.

#### Get a list of audio metadata
Retrieve metadata for a list of audio files.

#### Get an audio original file
Download the original audio file.

#### Get a list of photo metadata
Retrieve metadata for a list of photos.

#### Get a photo original file
Download the original photo file.

#### Get photo metadata
Retrieve metadata for a single photo.

#### Make a Query POST request
Execute a Query API request using HTTP POST. Provide a SQL like query to query against your organization's data.

#### Get a list of records
Get a list of records from your organization that can be filtered by dimensions such as form, project, changeset, bounding box, and date ranges.

#### Create a record
Create a new record in the specified form using the provided form values, location information, and any associated metadata.

#### Delete a record
Delete a record from your organization.

#### Get a record
Retrieve detailed information about a specific record by its ID. This includes all form field values, location data, timestamps, and associated metadata.

#### Partially update a record
Update specific fields of an existing record without requiring the complete record object. Only the fields included in the request body will be modified, while all other fields remain unchanged. This is useful for updating individual field values or metadata.

#### Update a record
Update a record with a provided record object. The record object is expected to be the complete representation of the record. Any fields not included are assumed null.

#### Get the history of a record
Retrieve the complete version history of a record.

#### Create a report
Generate a new report for a specific record, optionally using a report template.

#### Get a report file
Download the generated PDF report file.

#### Get a list of signature metadata
Retrieve metadata for a list of signatures.

#### Get signature metadata
Retrieve metadata for a single signature.

#### Get a signature original file
Download the original signature file.

#### Get a list of sketch metadata
Retrieve metadata for a list of sketches.

#### Get a sketch original file
Download the original sketch file.

#### Get sketch metadata
Retrieve metadata for a single sketch.

#### Get a list of video metadata
Retrieve metadata for a list of videos.

#### Get a video original file
Download the original video file.

## Obtaining Credentials

To use the Fulcrum connector, you need a Fulcrum API token:
1. Log in to your Fulcrum account at https://web.fulcrumapp.com
2. Navigate to Settings > API
3. Generate a new API token or use an existing one
4. Copy the token and use it when creating a connection

## Getting Started

Create a new connection and enter your Fulcrum API token when prompted.

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

## Deployment Instructions

Please use [these instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
