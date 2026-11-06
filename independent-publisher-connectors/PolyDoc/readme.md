# PolyDoc
[PolyDoc](https://polydoc.tech) turns HTML or a URL into pixel-perfect PDFs and screenshots, and generates EU-compliant hybrid e-invoices (ZUGFeRD / Factur-X). Conversions render in a real browser engine, so what you see in the page is what you get in the document.

## Publisher: PolyDoc

## Prerequisites
You need a PolyDoc account and an API key. The free tier is enough to evaluate the connector. Custom connectors are a premium Power Automate feature, so flows that use this connector need a Power Automate Premium plan (per-user or per-flow), the same as the built-in HTTP action.

## Obtaining Credentials
1. Sign in at [dashboard.polydoc.tech](https://dashboard.polydoc.tech).
2. Open **API Keys** and create a key.
3. When you create the connection, paste the key into **PolyDoc API key**. Paste only the key; the connector adds the `Bearer ` prefix for you.

## Supported Operations
### Convert to PDF
Render HTML, a URL, or a saved template into a PDF. Controls for page format, margins, scale, headers and footers, bookmarks, accessible (tagged) PDFs, metadata, encryption, watermarks, and PDF/A. To produce an EU-compliant hybrid e-invoice, fill in the **E-invoice** fields (standard, profile, and the structured `invoice` object) and PolyDoc embeds a ZUGFeRD / Factur-X invoice in the returned PDF, following EN 16931.

### Capture screenshot
Capture a PNG, JPEG, or WebP of HTML, a URL, or a template. Full-page or clipped, with viewport and device-pixel-ratio control.

## Source
Every operation takes a single **Source** that is one of a URL, an inline HTML string, or a saved template reference (`[template:TEMPLATE_ID]`) with optional **Template data** for the Liquid renderer.

## Delivery
By default the file is returned as binary content, ready for **Create file** (OneDrive, SharePoint, Blob) or an email attachment. Two alternative delivery modes are available under advanced options:
- **Cloud storage**: provide a presigned PUT URL; PolyDoc uploads the file and the action returns JSON containing the stored URL.
- **Webhook**: PolyDoc delivers the file to your webhook and the action returns a JSON acknowledgement (HTTP 202 when async).

When either delivery mode is used the response is JSON rather than a binary file.

## Known Issues and Limitations
- The response of each action is declared as a binary file. When **Cloud storage** or **Webhook** delivery is configured, the action returns JSON instead of a file.
- Pages that load JavaScript from an external CDN can slow the converter significantly. For inline HTML, prefer self-contained markup (inline CSS, data-URI images).
- E-invoices follow EN 16931. At minimum provide a due date or payment terms (rule BR-CO-25), the seller tax ID when a line uses VAT category `S`, and consistent totals (net + tax = gross). With **Verify** enabled an invoice that fails validation returns an error.
