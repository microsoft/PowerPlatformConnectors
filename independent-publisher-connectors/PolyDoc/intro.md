# PolyDoc (Independent Publisher)

[PolyDoc](https://polydoc.tech) turns HTML or a URL into pixel-perfect PDFs and
screenshots, and generates EU-compliant hybrid e-invoices (ZUGFeRD / Factur-X).
Conversions render in a real browser engine, so what you see in the page is what
you get in the document.

## Publisher

PolyDoc

## Contact

hello@polydoc.tech

## Supported operations

| Operation | What it does |
| --- | --- |
| **Convert to PDF** | Render HTML, a URL, or a saved template into a PDF. Controls for page format, margins, scale, headers and footers, bookmarks, accessible (tagged) PDFs, metadata, encryption, watermarks, and PDF/A. |
| **Capture screenshot** | Capture a PNG, JPEG, or WebP of HTML, a URL, or a template. Full-page or clipped, with viewport and device-pixel-ratio control. |

**E-invoices** are produced with **Convert to PDF**: fill in the E-invoice
fields (standard, profile, and the structured invoice object) and PolyDoc embeds
a ZUGFeRD / Factur-X invoice in the returned PDF, following EN 16931.

## Authentication

API key. The user creates a key at dashboard.polydoc.tech and pastes it into the
connection; the connector adds the `Bearer` prefix.

## Use cases

- Generate branded PDF documents (invoices, reports, certificates, contracts)
  from HTML or saved templates inside a flow.
- Capture screenshots of web pages or rendered HTML for archiving, monitoring,
  or thumbnails.
- Produce EU-compliant hybrid e-invoices (ZUGFeRD / Factur-X) directly from
  structured invoice data, ready for sending or archiving.

## Changes in this update (September 2026)

Only the request body of **Convert to PDF** changes. Operations, authentication
and responses stay the same, and every added field is optional.

- Invoice: `orderReference` (purchase order number, BT-13), `deliveryDate`
  (actual delivery date, BT-72) and `deliverTo`, the deliver-to party name and
  address (BT-70, BT-75 to BT-78, BT-80). A deliver-to address requires only its
  country code (EN 16931 rule BR-57).
- Seller and buyer: `contactName` (BT-41, BT-56) and `electronicAddress`
  (BT-34, BT-49).
- Invoice line: `buyerItemId` (BT-156).
- The e-invoice profile list drops `minimum` and `basicwl`. Neither carries
  invoice lines, so neither produces an EN 16931 e-invoice, and the PolyDoc API
  has rejected both with a validation error since September 2026.

## Reference

The full connector definition is also published at
https://github.com/polydoc-tech/power-automate-polydoc for reference.
