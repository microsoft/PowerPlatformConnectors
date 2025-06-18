# Taktikal Core

The Taktikal Core Connector equips you with the tools to sign, authenticate, and seal documents, while also providing comprehensive insights into your signing processes.

## Publisher: Taktikal

<https://taktikal.com>

## Prerequisites

To utilize the Taktikal Core Connector, it's necessary to have a Business plan with Taktikal. For further information contact us at <sales@taktikal.com> about the correct plan for you.

## How to access the credentials?

As an administrator in the Taktikal Portal navigate `Settings` > `Access-key` you where you will find the keys needed to connect.

## API documentation

For full documentation on the Taktikal Core API please visit <https://docs.taktikal.is/docs/api> .

## Supported Operations

### POST /Auth/Start

Initiates a authentication process with Icelandic e-id.

### POST /Auth/Poll

Checks the status of authentication for a user using their Icelandic e-id.

### POST /management/sealing

Seals a PDF document.

### POST /management/sealing/xml

Seals an XML document.

### DELETE /management/signing

Cancel a Signing process.

### POST /management/signing

Creates a new Signing process with one or more signees.

### DELETE /management/signing/sequential

Cancel the sequential Signing process.

### POST /management/signing/sequential

Creates a new Sequential (Bulk) Signing processes.

### POST /signing/sequential/{SequenceKey}/auth

Starts an auth process with Icelandic e-id for a sequential signing processes.

### GET /signing/{ProcessKey}/signee/{SigneeKey}

Gets the signing process by processKey and signeeKey.

### PUT /signing/{ProcessKey}/signee/{SigneeKey}

Update a Signee for given ProcessKey and SigneeKey.

### GET /signing/activity/{ProcessKey}

Gets activity log for Signing process.

### GET /signing/activity/user/

Gets activity log for a user (email) for a specific period.

### GET /signing/activity/company

Gets activity log for a Company.

## Known Issues and Limitations

Authentication is only possible with Icelandic e-Id.

For detailed documentation consulate <https://docs.taktikal.is> .
