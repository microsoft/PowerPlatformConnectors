# PremiumConnector SF
PremiumConnector SF is an connector for SAP SuccessFactors recruiting and metadata scenarios, exposed through the PremiumConnector API. It is designed for Microsoft Power Platform and can be imported as a custom connector.

PremiumConnector SF is not affiliated with, associated with, authorized by, endorsed by, or officially connected to SAP SE (including SAP SuccessFactors), Microsoft, or any of their affiliates. SAP, SuccessFactors, and related names are trademarks of their respective owners.

## Publisher: Sascha Bajonczak

## Prerequisites
There are no speciel requirements needed. Only the Authentication settings described below.

## Authentication
- Connector authentication in Power Platform: OAuth 2.0 (authorization code flow).
- The client ID is provided 
- I will share the secred on a secure channel with you. please tell me your prefered way to send this secret to you

## Supported Operations

### List plans
Returns all active plans for pricing and onboarding.

### List job postings
Returns job requisitions from SuccessFactors through PremiumConnector.

### Get job posting by id
Returns a single job requisition by id.

### List job applications
Returns paged job applications and supports filtering by jobReqId and status.

### Get job application by id
Returns a single job application by its id.

### Create interview
Creates a new interview for an existing job application.

### List candidates
Returns a list of candidates.

### Create candidate
Creates a candidate in SuccessFactors.

### Get candidate by id
Returns a candidate by SuccessFactors candidate id.

### Update candidate
Updates an existing candidate.

### Get picklist values
Returns all values for the given picklist id.

### Get entity fields
Returns available field names for a SuccessFactors entity.

### Get raw metadata
Returns raw EDMX metadata XML as text.

## Notes
- Operation availability depends on backend configuration and SuccessFactors role permissions.
- If an action fails with authorization or missing field errors, verify SuccessFactors API permissions, entity access, and environment-specific endpoint settings.