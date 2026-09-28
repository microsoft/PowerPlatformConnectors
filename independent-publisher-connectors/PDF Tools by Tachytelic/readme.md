# PDF Tools by Tachytelic

PDF Tools is a free set of actions for working with PDF files in Power Automate. It provides capabilities to merge, split, extract pages, extract text, optimize file size, and manage PDF metadata. Designed to simplify PDF operations in your flows without requiring a premium license.

## Publisher: Paul Murana

## Prerequisites

There are no prerequisites to use this connector. It is completely free and requires no API keys or authentication.

## Supported Operations

### Optimize PDF
Optimise a PDF by cleaning and recompressing internal objects. Takes a base64-encoded PDF and returns an optimised base64-encoded PDF. Supports aggressive and safe compression modes.

### Extract Specific Pages
Extract specific pages from a PDF file using a page range (e.g., "1-3,7"). Returns a new PDF containing only the specified pages.

### Extract Info
Extracts detailed metadata from a PDF file including Title, Author, CreationDate, PDF version, Page count, and more.

### Set Metadata
Sets metadata values (title, author, subject, keywords, dates) on a PDF file. Returns the PDF with updated metadata.

### Extract Text
Extracts all text from a PDF file, with optional StartPage and EndPage parameters. Returns both combined text and text organized by page.

### Split PDF
Splits a PDF file either by a fixed number of pages or by specified page ranges. Returns an array of PDF files.

### Merge PDFs
Merges multiple PDF files provided as base64 strings into a single PDF.

## Obtaining Credentials

No credentials are needed for this connector. It is a free service with no authentication required.

## Getting Started

1. Add the connector to your Power Automate flow
2. No connection setup is required - simply start using the actions
3. All PDF content must be provided as base64-encoded strings
4. Use the "Get file content" action from OneDrive, SharePoint, or other connectors to retrieve PDFs and convert them to base64

### Example: Optimize a PDF from SharePoint

1. Add "Get file content" from SharePoint
2. Add "Optimize PDF" action
3. Set PdfFileContent to the output from Get file content (it will be automatically base64 encoded)
4. Use the OptimizedPdf output to save back to SharePoint using "Create file"

## Known Issues and Limitations

- All PDF operations work with base64-encoded content, which increases the data size by approximately 33%
- Very large PDFs (over 100MB) may experience timeouts
- Password-protected PDFs cannot be processed by most operations
- The Extract Text operation may not work well with scanned documents (image-based PDFs)

## Frequently Asked Questions

### Is this connector really free?
Yes! This connector is completely free with no usage limits, no API keys, and no premium license required.

### What is the maximum file size supported?
The connector can handle most typical business documents. Very large files (over 100MB) may experience timeout issues due to processing time.

### Can I process scanned PDFs?
The connector works best with text-based PDFs. Scanned documents (image PDFs) will not have extractable text. Consider using an OCR service first.

### How do I convert a file to base64?
When you use actions like "Get file content" from SharePoint or OneDrive, the content is automatically available in a format that can be passed to this connector.

## Deployment Instructions

This is a published Independent Publisher connector and is available directly in Power Automate. No deployment is required.

If you wish to deploy this as a custom connector for testing:

1. Download the `apiDefinition.swagger.json` and `apiProperties.json` files
2. Run: `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json`
