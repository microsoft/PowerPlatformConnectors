# MiniPDF (Independent Publisher)

MiniPDF automates common PDF tasks such as merging, splitting, watermarking,
page manipulation, text extraction, rendering, metadata management, and form
processing. It provides structured actions so makers do not need to compose
HTTP requests manually.

## Service

https://minipdf.org

## Authentication

MiniPDF uses an API key supplied through the `x-api-key` request header. Create
or sign in to a MiniPDF account to obtain an API key.

## Features

- Add text watermarks and page numbers.
- Merge or split PDF files.
- Extract, delete, or rotate selected pages.
- Extract text and metadata.
- Update PDF metadata.
- Render PDF pages as images.
- Fill, extract, and flatten AcroForm fields.

## Typical Use Cases

- Combine documents created by multiple workflow steps.
- Add watermarks and page numbers before distributing a document.
- Extract text or metadata for indexing and downstream processing.
- Prepare selected pages for review, routing, or archival.
- Automate PDF form completion and flattening.

## Publisher

NGUYEN DINH VAN

## Support

For connector support, contact vanqn95@gmail.com.

## Known Issues and Limitations

- PDF content is transferred as Base64 JSON, so platform and service payload
  limits apply.
- Large, encrypted, or malformed PDF files might be rejected.
- Rate-limited requests return an HTTP 429 response.
- Some operations can return nonfatal warnings for unsupported PDF features.
