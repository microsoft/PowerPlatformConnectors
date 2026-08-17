# Leadinfo

Connect Leadinfo to your automated workflows to transform anonymous website visitors into actionable leads. This connector lets you sync real-time company data, visitor behaviour, and contact information with your favourite CRM, ERP, and communication tools — so you can automate lead assignments, trigger instant notifications for hot prospects, and keep your sales pipeline up to date without manual entry.

## Publisher

Leadinfo (team.blue)

## Prerequisites

You will need the following to use this connector:

- A Leadinfo account with API/webhook access. Sign up at [www.leadinfo.com](https://www.leadinfo.com).
- A Microsoft Power Automate, Power Apps, or Azure Logic Apps plan that supports premium connectors.
- Permission in Leadinfo to create and manage webhook integrations.

## How to get credentials

This connector uses OAuth 2.0 (authorization code flow). When you create a connection, you are redirected to Leadinfo to sign in and authorize access. Power Platform then stores and refreshes the access token automatically — there are no keys to copy or paste.

- **Authorization URL:** `https://portal.leadinfo.com/oauth2`
- **Token / Refresh URL:** `https://api.leadinfo.com/oauth2/token`
- **Scope:** `webhook`

If you administer the Leadinfo OAuth application, ensure the redirect URL `https://global.consent.azure-apim.net/redirect/<connectorId>` is registered as an allowed redirect for the app.

## Supported operations

This connector provides webhook **triggers** that start a flow when an event occurs in Leadinfo. When you add a trigger, the connector registers a webhook with Leadinfo; Leadinfo then calls back with the event payload in real time.

| Trigger | Description |
| --- | --- |
| **When a company is identified** | Fires when a company is created or linked in Leadinfo, returning the identified company's details (name, address, domain, industry, visit metrics, and more). |
| **When company data is shared** | Fires when company data is shared from Leadinfo, returning the full company record. |
| **When a contact is identified** | Fires when a contact is identified, returning the contact's details together with the associated company data. |
| **When a contact is shared** | Fires when a contact is shared, returning the contact's details together with the associated company data. |

## Known issues and limitations

- The connector provides triggers only; it does not currently expose actions for querying or updating Leadinfo data on demand.
- Webhook subscriptions are created automatically when a flow is turned on. If a flow is deleted while Leadinfo is unreachable, the subscription may need to be removed manually in Leadinfo.
- Event payloads reflect the data available in Leadinfo at the moment the event fires; some fields may be empty when the underlying enrichment data is not available.

## Deployment instructions

Use these files with the Power Platform CLI or `paconn`:

1. Update the OAuth client ID/secret in your Leadinfo OAuth application and in the connector connection settings.
2. Validate the definition with `paconn validate --api-def apiDefinition.swagger.json`.
3. Create or update the connector with `paconn create` / `paconn update`.

## Support

For connector or API support, contact Leadinfo at [www.leadinfo.com](https://www.leadinfo.com) or support@leadinfo.com.
