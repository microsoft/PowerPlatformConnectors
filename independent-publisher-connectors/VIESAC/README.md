# VIESAC — EU VAT Validation

## Overview

[VIESAC](https://viesac.eu) validates EU, UK, Norwegian and Swiss VAT numbers against official government sources and stores each check as a legally-compliant audit record with a PDF and XML certificate.

| Region | Official Source |
|--------|----------------|
| EU member states + Northern Ireland | VIES (European Commission) |
| Great Britain | HMRC |
| Norway | Brønnøysundregistrene |
| Switzerland | UID Register |

Use this connector to automate VAT compliance in Power Automate, Power Apps, and Logic Apps — without writing a single line of code.

## Prerequisites

1. A [VIESAC account](https://viesac.eu/register) (free tier available)
2. An API key from your [VIESAC dashboard](https://viesac.eu/app)

## Supported Actions

| Action | Description |
|--------|-------------|
| **Create Audit** | Validate a VAT number and save a full audit record with PDF + XML certificate |
| **List Audits** | Retrieve filtered audit history (by date, country, VAT number, order/invoice) |
| **Get Audit** | Fetch a single audit by its reference number |
| **Download PDF Certificate** | Download the PDF compliance certificate (available in 22 languages) |
| **Download XML Certificate** | Download raw XML evidence from the official source |
| **Get Account** | Check your plan, monthly limit, and remaining quota |

## Authentication

This connector uses API Key authentication. When creating a connection, enter `Bearer ` followed by your VIESAC API key (for example, `Bearer vac_your_api_key`). Power Automate sends the header value exactly as entered.

## Common Use Cases

- **ERP / Accounting automation** — validate supplier VAT before posting an invoice in Dynamics 365, SAP, or Business Central
- **E-commerce compliance** — auto-validate customer VAT at checkout (Shopify, WooCommerce via Power Automate)
- **Accounts payable** — trigger a VAT check when a new vendor is added in Dataverse
- **Audit trail** — store PDF certificates in SharePoint or OneDrive automatically after each check
- **Alerting** — get a Teams or email notification when a VAT number status changes during monitoring

## Getting Started

1. Install this connector in Power Automate
2. Create a new connection using your VIESAC API key
3. Add the **Create Audit** action to your flow
4. Map the **VAT number** field from your trigger
5. Use **reference_number** from the response to download the certificate

## Links

- [VIESAC Website](https://viesac.eu)
- [API Documentation](https://viesac.eu/api-docs)
- [Support](mailto:support@viesac.eu)
- [Privacy Policy](https://viesac.eu/privacy-policy)
- [Terms of Service](https://viesac.eu/terms-of-service)

## Publisher

VIESAC — support@viesac.eu
