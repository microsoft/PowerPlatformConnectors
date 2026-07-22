# MiniPDF Connector Proposal

## Overview

MiniPDF is a PDF automation service that exposes API operations for common
document-processing tasks. This proposed connector makes those operations
available in Microsoft Power Automate and Power Apps so makers can automate PDF
processing without manually composing HTTP requests.

## API

- Service: MiniPDF
- Website: https://minipdf.org
- API style: REST with JSON request and response bodies
- Authentication: API key passed in the `x-api-key` request header

## What Problem Does This Connector Solve?

Business workflows frequently need to merge, split, watermark, inspect, or
transform PDF files. Without a connector, makers must build and maintain custom
HTTP requests, encode file content, and manually handle the returned data.
MiniPDF provides these capabilities as named Power Platform actions with defined
inputs and outputs.

## Proposed Operations

- Add text watermark
- Merge PDFs
- Split PDF
- Extract text
- Render PDF pages to images
- Add page numbers
- Extract metadata
- Set metadata
- Extract pages
- Delete pages
- Rotate pages
- Fill PDF forms
- Extract PDF form data
- Flatten PDF form fields

## Typical Use Cases

- Combine documents produced by multiple workflow steps into one PDF
- Split incoming documents for routing or archival
- Add watermarks and page numbers before distribution
- Extract PDF text or metadata for downstream processing
- Prepare selected pages for review or storage
- Fill and flatten PDF forms in automated business processes

## Publisher

NGUYEN DINH VAN

## Notes

The connector has been tested as a Power Platform custom connector against the
production MiniPDF API. PDF content is transferred as Base64 JSON content, so
Power Platform and service payload limits apply.
