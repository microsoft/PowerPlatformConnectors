# MicrosoftOffice Files Parser Connector

## Overview  
This custom connector enables easy parsing of Microsoft Word and Excel documents by submitting base64-encoded file content. It is designed for use in Power Automate, Power Apps, and Azure Logic Apps.

## Features  
- Parse Word documents to extract text content.  
- Parse Excel spreadsheets and return worksheet names along with parsed data arrays.

## Authentication  
This connector does **not** require authentication.

## Usage  

### Parse Word Document

**Request**  
POST JSON payload to `/apps/parser/word.php` with:

```json
{
  "$content-type": "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
  "base64$content": "BASE64_ENCODED_WORD_DOCUMENT_HERE"
}
```

**Respond**
```json
{
  "message": "Text extracted successfully",
  "text": "Extracted text content from the Word document."
}
```

### Parse Excel Spreadsheet

**Request**
POST JSON payload to /apps/parser/excel.php with:

```json
{
  "$content-type": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  "$filename": "Workbook.xlsx",
  "base64$content": "BASE64_ENCODED_EXCEL_FILE_HERE"
}
```

**Respond**
```json
{
  "Worksheets": ["Sheet1", "Sheet2"],
  "Sheet1": [
    ["Header1", "Header2"],
    ["Row1Value1", "Row1Value2"]
  ],
  "Sheet2": [
    ["HeaderA", "HeaderB"],
    ["Row1ValueA", "Row1ValueB"]
  ]
}
```


## Response  
- **Word endpoint:** Returns extracted text and a status message.  
- **Excel endpoint:** Returns an array of worksheet names and their respective parsed data as 2D arrays.

## Notes  
- The `$filename` parameter must include the file extension (e.g., `Workbook.xlsx`).  
- The `base64$content` parameter must contain the base64-encoded file content.  
- The connector does not store or retain any submitted data.

## Support  
For questions or support, please contact me at [stevens@ssdevkit.onmicrosoft.com](mailto:stevens@ssdevkit.onmicrosoft.com).

