# MiniPDF (Independent Publisher)

MiniPDF automates common PDF tasks such as merging, splitting, watermarking,
page manipulation, text extraction, rendering, metadata management, and form
processing. PDF files are transferred as Base64-encoded content so they can move
between document storage actions and MiniPDF actions in a cloud workflow.

## Publisher: NGUYEN DINH VAN

## Support

For connector support, contact vanqn95@gmail.com.

## Prerequisites

- A MiniPDF account.
- A MiniPDF API key.
- A PDF file for actions that process document content.

## Supported Operations

### Add text watermark

Adds a text watermark to every page of a PDF.

### Merge PDFs

Merges multiple PDF files into a single PDF in the specified order.

### Split PDF

Splits a PDF by individual pages, selected pages, or page ranges.

### Extract text

Extracts text from a PDF as joined text or as text grouped by page.

### Render PDF to images

Renders selected PDF pages as images in the requested format.

### Add page numbers

Adds page numbers to selected pages of a PDF.

### Extract metadata

Reads standard metadata values from a PDF.

### Set metadata

Updates standard metadata values in a PDF.

### Extract pages

Extracts selected pages into a new PDF.

### Delete pages

Deletes selected pages from a PDF.

### Rotate pages

Rotates selected PDF pages by the requested angle.

### Fill form

Fills AcroForm fields in a PDF with the supplied values.

### Extract form data

Extracts AcroForm field names, types, and values from a PDF.

### Flatten form fields

Flattens PDF form fields into static page content.

## Obtaining Credentials

Create or sign in to your MiniPDF account and obtain an API key. When creating a
MiniPDF connection, enter the key in the **API Key** field. The connector sends
the credential in the `x-api-key` request header. Never include an API key in a
workflow definition, screenshot, source file, or public pull request.

## Getting Started

1. Create a MiniPDF connection with your API key.
2. Use a file storage action to retrieve the PDF file content.
3. Pass the file name and the Base64 file content to a MiniPDF action.
4. Use the returned file content or extracted data in later workflow steps.

## Known Issues and Limitations

- PDF content is transferred as Base64 JSON, so platform and service payload
  limits apply.
- Large PDF files can exceed request or response payload limits.
- Rate-limited requests return an HTTP 429 response.
- Encrypted or malformed PDFs might be rejected.
- Some operations can return nonfatal warnings for unsupported PDF features.
- Page numbers are one-based unless an action explicitly states otherwise.

## Deployment Instructions

Import `apiDefinition.swagger.json` as an OpenAPI 2.0 custom connector. Create a
connection with a MiniPDF API key, then test an operation before using the
connector in a workflow.
