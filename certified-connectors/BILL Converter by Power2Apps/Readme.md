# BILL Converter by Power2Apps for Power Automate
Enables seamless processing and transformation of electronic invoices in full compliance with the ZUGFeRD, XRechnung, EN 16931 and PDF/A-3 standards. It provides powerful tools to convert XML invoices to PDF, generate ZUGFeRD-compliant hybrid PDF/A-3 documents, and extract embedded invoice XML from ZUGFeRD or PDF/A-3 files. Designed for maximum interoperability, the connector supports automated workflows for validating, transforming, and exchanging structured electronic invoice data across modern e-invoicing ecosystems.

## Publisher
Power2Apps

## Prerequisites
- A Power Automate or Power Apps environment
- A Power2Apps API Key

## Setup
- Get your free API Key on https://www.power2apps.de/apikey/
- When creating the connection, paste the API Key into the **API Key** field

## Supported Actions

### Convert Invoice to ZUGFeRD PDF
Creates a fully compliant ZUGFeRD PDF/A-3 invoice by embedding structured invoice data (XML) into a readable PDF. Supports the ZUGFeRD standard (EN 16931) and generates hybrid PDF/A-3 documents suitable for digital archiving and electronic invoice exchange - https://support.power2apps.de/T10_BillConverter/t10-a001-av01-cv01-convertinvoicetozugferdpdf

### Extract XML from ZUGFeRD PDF
Extracts the embedded ZUGFeRD XML file from a ZUGFeRD-compliant PDF and returns it as a standalone XML document for further processing - https://support.power2apps.de/T10_BillConverter/t10-a002-av01-cv01-extractxmlfrominvoicedpdf

### Convert XML Invoice to PDF
Generates a readable PDF document from an electronic XML invoice (e.g., ZUGFeRD or XRechnung) by rendering the XML data into a formatted PDF layout - https://support.power2apps.de/T10_BillConverter/t10-a003-av01-cv01-convertxmlinvoicetopdf

## Supported Standards
- ZUGFeRD (German hybrid e-invoice standard, based on EN 16931)
- XRechnung (German public-sector e-invoice standard)
- EN 16931 (European e-invoicing standard)
- PDF/A-3 (long-term archival PDF with embedded structured data)

## Known Issues and Limitations
None.

## Frequently Asked Questions
**Where do I get my API Key?**
On https://www.power2apps.de/apikey/.

**Where can I find documentation for each action?**
The links above point to the detailed support pages for every action on https://support.power2apps.de.

**Where do I get support?**
Reach the Power2Apps team via https://www.power2apps.de/en/contact or by email at getintouch@power2apps.de.
