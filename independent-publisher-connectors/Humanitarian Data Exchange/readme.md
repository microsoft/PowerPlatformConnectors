# Humanitarian Data Exchange
Access HDX Humanitarian API (HAPI) data for affected people, coordination context, food security, geography, climate, and metadata.

## Publisher: Richard Wilson

## Prerequisites
You need an application name and email address for HDX HAPI identification.

## Supported Operations
### Generate App Identifier
Create a base64-encoded app identifier from application name and email.

### Affected People
- Get humanitarian needs data
- Get IDPs data
- Get refugees/persons of concern data
- Get returnees data

### Coordination and Context
- Get conflict events
- Get funding data
- Get national risk data
- Get operational presence data

### Climate
- Get rainfall data

### Food Security, Nutrition, and Poverty
- Get food prices
- Get food security data
- Get poverty rate data

### Geography and Infrastructure Baseline
- Get baseline population data

### Metadata
- Get first level administrative divisions (admin1)
- Get second level administrative divisions (admin2)
- Get currency classifications
- Get data availability metadata
- Get dataset metadata
- Get locations
- Get organizations
- Get organization type classifications
- Get resource metadata
- Get sector classifications
- Get WFP commodities
- Get WFP markets

### Utility
- Get API version information

## Obtaining Credentials
1. Choose an app name for your integration.
2. Use an email address associated with your usage of HDX HAPI.
3. Enter both values when creating the connector connection.

## Known Issues and Limitations
The source API is currently in beta and endpoint details may evolve over time.

## Getting Started
1. Create a connector connection and provide `App Name` and `Email`.
2. The connector automatically builds and injects `app_identifier` for each request.
3. Start with metadata endpoints (for example, location and sector) to discover filter values.
4. Use those filter values in thematic endpoints for operational queries.
