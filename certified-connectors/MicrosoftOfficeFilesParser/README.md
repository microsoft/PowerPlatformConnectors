# MicrosoftOffice Files Parser Connector

Custom connector for parsing Microsoft Word and Excel documents by submitting base64-encoded file content. Designed for use with Power Automate, Power Apps, and Azure Logic Apps to enable easy document content extraction.

## Publisher: Steven Soe  
Certified Connector Owner

## Prerequisites  
No special licenses or plans are required. The connector does **not** require authentication. Suitable for users with access to Power Automate, Power Apps, or Azure Logic Apps.

## Supported Operations  

### Parse Word Document  
Accepts base64-encoded Word document content and returns the extracted text content.

### Parse Excel Spreadsheet  
Accepts base64-encoded Excel file content and returns worksheet names along with the parsed worksheet data as 2D arrays.

## Obtaining Credentials  
This connector does **not** require authentication or credentials.

## Getting Started  
To use the connector, POST a JSON payload containing the base64-encoded file content to the respective endpoint for Word or Excel parsing. Ensure the content type and filename (for Excel) are provided.

## Known Issues and Limitations  
- The connector expects base64 encoding for the file content.  
- The `$filename` parameter is required for Excel files and must include the file extension.  
- The connector does **not** store, retain, or use any submitted data.  
- No authentication is required; users should exercise caution when sharing sensitive data through flows using this connector.

## Frequently Asked Questions  

### Does this connector store my data?  
No, the connector does not store, retain, or use any submitted file content.

### Is authentication required?  
No, this connector does not require any authentication.

## Deployment Instructions  
Place the connector files (`swagger.yaml`, `manifest.json`, `README.md`, `icon.png`) in your folder structure and import the custom connector into your Power Platform environment via the Power Automate or Power Apps portal.

## Support  
For questions or support, contact [stevens@ssdevkit.onmicrosoft.com](mailto:stevens@ssdevkit.onmicrosoft.com).
