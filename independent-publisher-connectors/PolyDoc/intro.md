# PolyDoc (Independent Publisher)

[PolyDoc](https://polydoc.tech) turns HTML or a URL into pixel-perfect PDFs and
screenshots, and generates EU-compliant hybrid e-invoices (ZUGFeRD / Factur-X).
Conversions render in a real browser engine, so what you see in the page is what
you get in the document.

This is a proposal to reserve the connector name and start the verified
credentials process. The complete, validated connector files
(`apiDefinition.swagger.json`, `apiProperties.json`, `readme.md`) are ready and
will be added to this same pull request, together with the required operation
screenshots, once verification is set up.

## Publisher

PolyDoc

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

## Reference

The full connector definition is also published at
https://github.com/polydoc-tech/power-automate-polydoc for reference.
