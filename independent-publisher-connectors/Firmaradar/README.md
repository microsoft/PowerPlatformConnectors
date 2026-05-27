# Firmaradar

Firmaradar is an enrichment platform for Norwegian company intelligence —
built to extend other services with high-quality KYC, AML, credit,
ownership and risk insights through AI/agent flows, REST APIs and a web UI.

The platform fuses data from multiple authoritative sources — the
Norwegian Business Register (Brønnøysundregistrene), the Norwegian Tax
Administration (Skatteetaten), foreign PEP and sanctions registers — and
adds proprietary enrichment on top: distress classification, risk
scoring, fuzzy person-matching, group-structure analytics, announcement
signal-detection, and audit-grade compliance pipelines.

This connector exposes 22 operations that you can drop into any Power
Automate flow to enrich customers, suppliers, partners and counterparties
with structured Norwegian company intelligence — without writing code.

## What you can build

- **KYC onboarding** — verify new customers against AML/PEP lists,
  validate ownership structures, surface sanctions hits, persist
  audit-grade decision logs.
- **Supplier risk monitoring** — daily risk-score sweep across vendor
  portfolio, push alerts to Teams/Slack on rating downgrades or distress
  signals.
- **Credit-decision support** — combined company-financial-health score
  plus group-structure context plus FIV (foretak i vanskeligheter)
  classification for lending committees.
- **Compliance reporting** — recurring snapshots into SharePoint /
  OneDrive / Dataverse with full audit trail of who looked up what,
  when, and under which legal basis.
- **Sales prospecting** — NACE-industry queries plus financial filters,
  enrich CRM with current company status, ownership and key people.
- **Group due-diligence** — ownership-tree expansion with subsidiary
  financials, AML screening across signatories, public-grants exposure.
- **The Norwegian leg in international workflows** — non-Norwegian
  banks, KYC vendors, AML platforms and M&A advisors can call
  Firmaradar as their canonical Norwegian backend and stitch the
  result into a wider multi-jurisdiction view. You do not need to be
  Norwegian to be a Firmaradar customer — the API and Power Automate
  flows are designed to slot into broader compliance pipelines as
  one of many country-modules.

## Why an enrichment platform — not "just a BRREG wrapper"

Public registry data alone rarely answers a real business question.
Firmaradar combines the registry with proprietary signals and
cross-source matching so a single API call returns a decision-ready
view:

| Layer | What we add |
|---|---|
| **Multi-source fusion** | Brønnøysundregistrene, Skatteetaten, foreign PEP/sanctions registers, distress signals, BRREG announcements feed, group-support registries (Innovasjon Norge / SkatteFUNN) — combined and reconciled. |
| **Person matching** | Fuzzy name-match with initial-expansion and middle-name dropping; cross-orgnr role resolution; PEP/sanctions hits with audit logging. |
| **Risk modelling** | Deterministic, transparent risk score (0-100) with component breakdown; distress classification (FIV) following statutory criteria; interim-balance signal detection. |
| **Group analytics** | Bi-directional ownership-tree traversal (up to 12 levels), aggregate metrics across subsidiaries, change-tracking on group composition. |
| **Compliance plumbing** | DPA-gated endpoints, per-call purpose tagging, 60-month audit retention, GDPR-aligned access logs — built in, not bolt-on. |
| **Stable schema** | Same operation contracts powering our MCP server (Claude/ChatGPT integrations), n8n nodes, Make.com app and now Power Automate — so workflows migrate between platforms without data loss. |

## Publisher

- **Publisher:** Lars Kvanum
- **Stack owner:** Firmaradar AS
- **Country:** Norway

## Prerequisites

1. An active Firmaradar account at <https://firmaradar.no>.
2. A personal API key issued via **Min side → API-nøkler**
   (<https://firmaradar.no/min-side/api-keys>). The key is sent as
   `X-API-Key` HTTP header in every request.
3. Active subscription for the operations you intend to use. Some
   operations require additional data-processing agreements (DPA)
   signed in your Firmaradar account — most notably AML/PEP screening
   and tax-list lookups.

The free tier covers a generous slice of company-profile, ownership,
roles, financial-history and announcement operations. Compliance-tier
operations (AML, tax lists, audit-grade risk scoring) require a paid
plan and explicit DPA acceptance.

## Supported operations

The connector exposes 22 production operations across 7 functional
areas. Many operations combine multiple upstream sources behind a
single call.

### Company (Selskap)

| Operation | Method | Path |
|-----------|--------|------|
| `getCompany` | GET | `/api/v1/company/{orgnr}` |
| `getCompanyOwnership` | GET | `/api/v1/company/{orgnr}/ownership` |
| `getCompanyRoles` | GET | `/api/v1/company/{orgnr}/roles` |
| `getCompanyFinancials` | GET | `/api/regnskap/{orgnr}/historikk` |
| `getCompanyChanges` | GET | `/api/v1/company/{orgnr}/changes` |
| `searchCompanies` | GET | `/api/v1/companies/search` |
| `compareCompanies` | POST | `/api/v1/companies/compare` |

### Person

| Operation | Method | Path |
|-----------|--------|------|
| `searchPersons` | GET | `/api/v1/person/search` |
| `getPersonRoles` | GET | `/api/v1/person/roles/{role_person_id}` |
| `getPersonCompanies` | GET | `/api/v1/person/shareholdings/{owner_person_key}` |

### Risk (Risiko)

| Operation | Method | Path |
|-----------|--------|------|
| `getCompanySignals` | GET | `/api/v1/company/{orgnr}/signals` |
| `searchAnnouncements` | GET | `/api/v1/announcements/search` |
| `getRiskScore` | GET | `/api/v1/risikoscoring/score/{orgnr}` |
| `checkForetakIVanskeligheter` | GET | `/api/v1/fiv/assess/{orgnr}` |

### Industry (Bransje)

| Operation | Method | Path |
|-----------|--------|------|
| `listCompaniesInNace` | GET | `/api/v1/nace/{code}/companies` |
| `findRelatedCompanies` | GET | `/api/v1/company/{orgnr}/related` |

### AML

| Operation | Method | Path |
|-----------|--------|------|
| `checkAmlPep` | POST | `/api/v1/aml/check` |
| `getAmlScore` | POST | `/api/v1/aml/score` |

### Tax lists (Skattelister)

| Operation | Method | Path |
|-----------|--------|------|
| `getSkattelister` | GET | `/api/v1/skattelister/selskap/{orgnr}` |

### Marketplace (Markedsplass)

| Operation | Method | Path |
|-----------|--------|------|
| `getKonsernstotte` | GET | `/api/v1/konsernstotte/oversikt/{orgnr}` |

In addition, three documentation aliases are exposed in the spec to
give a 1-to-1 mapping with the Firmaradar MCP-tool catalog
(`getCompanyAnnouncements`, `getRecentChanges`, `getPerson`); these
forward to the same backend as the canonical operations above.

## Obtaining credentials

1. Sign in to <https://firmaradar.no> with a verified Firmaradar account.
2. Open **Min side → API-nøkler** (My account → API keys).
3. Click **Generer ny API-nøkkel** (Generate new key) and copy the key.
   The full key is shown only once — store it in a secrets manager or
   in your Power Automate connection settings.
4. When you create a Power Automate flow that uses this connector,
   paste the key into the **Firmaradar API key** connection field.

You can revoke or rotate the key from the same page at any time.

## Known issues and limitations

- **Norwegian-language responses.** Field labels and human-readable
  strings in responses are written in Norwegian (bokmål). English
  summaries are included where practical, but most enums and
  descriptions remain Norwegian — reflecting the source registries.
- **Rate limits.** Free-tier accounts are limited to 60 requests per
  minute. AML operations (`checkAmlPep`, `getAmlScore`) have a stricter
  limit of 50 requests per 30 minutes regardless of plan.
- **Audit logging.** Calls to AML, tax-list and marketplace risk
  operations are persisted in an audit log for 60 months as required
  by Norwegian KYC regulations. The `X-FR-Purpose` and
  `X-FR-DPA-Confirmed` headers must be set on AML calls; missing
  headers return HTTP 403.
- **Compliance gates.** Two operations (`getRiskScore`, `getSkattelister`)
  may return HTTP 403 with `error_code=marketplace_hidden` until the
  user's account passes the relevant compliance gate (sole-proprietor
  rules for personal risk scoring, Skatteetaten API approval for tax
  lists). The spec marks these operations with the
  `x-firmaradar-compliance-gate` extension so consuming clients can
  hide or disable them dynamically.
- **9-digit org numbers only.** All `orgnr` path parameters must be
  exactly 9 digits (Norwegian organization-number format). The
  connector rejects values matching other patterns at the API layer.
- **Pagination cursors are opaque.** `next_cursor` returned by
  paginated operations should be passed back verbatim — do not parse
  or modify it.

## Deployment instructions

This connector is distributed via the Microsoft Power Platform
Connectors repository under the `independent-publisher-connectors/`
namespace.

End users do not need to install the connector manually after
certification. Once Microsoft accepts the submission, the connector
becomes available in the standard connector catalog in Power Automate,
Power Apps and Logic Apps.

For developers who want to deploy the connector before certification,
run:

```bash
paconn create --api-prop apiProperties.json \
              --api-def apiDefinition.swagger.json \
              --icon icon.png
```

This publishes the connector to your own Power Platform tenant. See
<https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli>
for the full CLI reference.

## Support

- **Issues with the connector definition** (Swagger, icon, metadata):
  file an issue or pull request against
  <https://github.com/microsoft/PowerPlatformConnectors>.
- **Issues with the Firmaradar API** (data, rate limits, account
  access): contact <support@firmaradar.no>.
- **General Firmaradar documentation:**
  <https://firmaradar.no/api-dokumentasjon>.
