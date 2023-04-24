# Dokobit Universal API
Dokobit Universal API is a versatile tool that enables signing documents internally, within your system, and collecting signatures from 3rd parties, outside your system, across Europe. Document signing occurs in an accountless Dokobit signing (where invited participants can sign documents without registering on the Dokobit portal) interface with your organisation’s branding.

## Publisher: Dokobit

## Prerequisites
In order to use Dokobit Universal API contact us at sales@dokobit.com to obtain the API access token.

## Supported Operations

### Create External Signing
External signing initiates asynchronous signing request from your system. In this case user receives an email for signing and you will get a callback once he signs the document.

### Check External Signing Status
Returns document's information with signing status.

### Download Document External
Downloads the signed document.

### Send Reminder
Allows to send a reminder e-mail for participant, only if person has not signed document yet. Only works with external signing flow.

### Add External Signer
Indicated participants will gain access to the document with the selected role.

### Delete External Signer
Access to the document will be revoked for the indicated participant.

### Delete External Signing
The document will be removed from the owner's account. It will remain accessible to other participants.

### Update External Signing
Allows to update attributes of an existing document signing.

### Create Internal Signing
Internal signing initiates signing request from your system. In this case signing URL is generated where participant should be redirected.

### Check Internal Signing Status
Returns document's information with signing status.

### Download Document Internal
Downloads the signed document.

### Add Internal Signer
Indicated participants will gain access to the document with the selected role.

### Delete Internal Signer
Access to the document will be revoked for the indicated participant.

### Delete Internal Signing
The document will be removed from the owner's account. It will remain accessible to other participants.

### Update Internal Signing
Allows to update attributes of an existing document signing.

### Seal signing
Document will be sealed with your organizations' electronic seal. Please contact our support at developers@dokobit.com to get a qualified certificate for e-sealing.

### e-Delivery
Document e-Delivery is used for securely sharing documents via Dokobit Universal API.

### Validation
Dokobit is an eIDAS-certified service provider for qualified e-signature and e-seal validations. Use this action to upload a signed file for validation. Supported file types: `asice`, `asics`, `sce`, `bdoc`, `edoc`, `pdf`, `p7s`, `xml`.

### Validation - Detailed PDF Report
Download detailed validation report in PDF.

### Validation - XML Diagnostic Data
Download diagnostic data in XML.

### Validation - PDF Report
Download validation report in PDF.

### Validation - XML Report
Download validation report in XML.

## Obtaining Credentials
[Contact](mailto:sales@dokobit.com) our Sales to get the API access token.

## Known Issues and Limitations
The current implementation supports signature gathering on PDF files and supported by ASiCe, ADoc, BDoc or EDoc containers.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.
