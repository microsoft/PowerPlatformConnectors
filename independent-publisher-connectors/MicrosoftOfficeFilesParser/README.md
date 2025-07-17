# MicrosoftOffice Files Parser Connector

## Overview
Custom connector for parsing Microsoft Word and Excel documents easily.

## Features
- Parse Word documents by submitting base64 content.
- Parse Excel spreadsheets and return parsed worksheet data.

## Authentication
This connector does not require authentication.

## Usage
- For Word documents, POST JSON payload containing `$content-type` and `base64$content`.
- For Excel spreadsheets, POST JSON payload containing `$content-type`, `$filename`, and `base64$content`.

## Response
- Word endpoint returns extracted text and status message.
- Excel endpoint returns worksheet names and their parsed data as arrays.

## Notes
- Ensure `$filename` contains file extension (e.g., Workbook.xlsx).
- Ensure `base64$content` is the base64content of the file.

## Contact
For questions or support, please contact Steven Soe at stevens@ssdevkit.onmicrosoft.com.

