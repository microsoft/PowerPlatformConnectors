# Annature

Annature is the leading Australian-owned eSigning solution with an integrated digital signature provider. This connector helps businesses to create envelopes and integrate document signing workflow using Annature API (v1).

## Publisher: Strategik

## Prerequisites

To use this connector, you need the following

- A Microsoft Power Apps or Power Automate plan with a custom connector feature
- You need to have an Annature account configured with APIKeys.

## Obtaining Credentials

- To get started you will need to create a free account to obtain a pair of API keys. [sign up](https://dashboard.annature.com.au/sign-up)
- Deploy the Connector and Configure the Data Connections using the APIKeys.
- For more details check out this [article](https://www.strategik.com.au/post/using-annature-with-the-power-platform)

### Supported Operations

Annature API has more than 40 operations. Currently, this connector supports the most important operations needed for Signing.

## Actions

#### 1. Documents

- Get Documents : Get documents created for an envelope.

#### 2. Envelopes

- Create Envelope : Creates a New Envelope
- Get Envelopes : Get envelopes.
- Get Envelope : Get the details of an existing envelope
- Send Envelope : Sends an existing draft envelope to all eligible recipients.
- Void Envelope : Voids an envelope cancelling all outstanding signature requests and preventing the envelope from being completed

#### 3. Recipients

- Get Recipient : Get details of a Recipient based on Id

#### 4. Fields

- Get Fields : Get fields from an envelope
- Get Field : Get a single field created for an envelope

#### 5. Templates

- Get Templates : Get templates defined in your annature account
- Get Template : Get the template based on template Id.
- Create Envelope From Template : Create a Envelope based on a specific template Id. You can also pass values to fields in a template

#### 6. Others

- Get Accounts : Get accounts from your annature account
- Get Organization : Get the details of your organisation.

## Triggers

### Endpoints : OnDocumentEvent

Annature uses webhooks to notify your application via HTTP POST when an event happens on envelopes sent from your organisation.

Please read about webhook responses here https://documentation.annature.com.au/#/webhooks/responding

## Known Issues and Limitations

There are no known issues at the time of publishing.

## Deployment Instructions

Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).

#### Not all operations provided by Annature are part of the first IP connector submission. We will keep adding/updating/supporting this connector based on your feedback/requests :)
