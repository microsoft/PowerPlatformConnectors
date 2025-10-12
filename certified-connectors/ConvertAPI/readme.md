# ConvertAPI Certified Connector for PowerAutomate

[ConvertAPI](https://convertapi.com) is a powerful file conversion and manipulation service supporting over 200 document, ebook, archive, image, spreadsheet, and presentation formats. This certified connector for PowerAutomate integrates ConvertAPI's robust capabilities directly into your workflows, enabling you to convert files, compress documents, and apply various PDF manipulations with ease.

## Publisher

**ConvertAPI**

## Prerequisites

Before you begin, ensure you have:
- A free ConvertAPI account. [Sign up here](https://www.convertapi.com/a/signup)
- Access to your ConvertAPI **Secret Key**, which can be found on the [Secret Key page](https://www.convertapi.com/a/secret).

## Supported Operations

The connector currently supports the following operations:

### Conversion Operations
- Convert PDF to DOCX
- Convert PDF to PDF/A
- Convert PDF to PNG
- Convert PDF to PPTX
- Convert PDF to CSV
- Convert PDF to XLSX
- Convert PDF to JPG

### Manipulation Operations
- Extract text from PDF
- Compress a PDF file
- Remove pages from PDF
- Extract images from PDF
- Flatten PDF
- Merge multiple PDFs
- Add OCR layer to PDF
- Protect a PDF
- Redact PDF
- Rotate PDF pages
- Split PDF
- Remove PDF protection
- Add watermark to PDF

### Document Conversion & Manipulation

Easily convert one input file to another output format or apply advanced PDF operations — such as compressing, merging, rotating, watermarking, and protecting—directly within your PowerAutomate workflows.

## Obtaining Credentials

To start using the connector, follow these steps:
1. [Sign up for a free ConvertAPI account](https://www.convertapi.com/a/signup).
2. Navigate to **Authentication** → [**Secret Key**](https://www.convertapi.com/a/secret).
3. Your **Secret Key** will be displayed.
4. Copy your Secret Key and paste it when setting up the connector.

## API Documentation

For complete details on how to use the ConvertAPI, please refer to the [ConvertAPI Documentation](https://www.convertapi.com/api) and [ConvertAPI Developer Hub](https://docs.convertapi.com/docs/getting-started).

---

Integrate this connector into your PowerAutomate environment to streamline file conversions and document manipulations, boosting productivity and workflow efficiency.

## Known Issues and Limitations

Currently our integration is limited to the PDF tools listed above.
To use our converter's that are not listed here, please use an HTTP call from PowerAutomate to our API.

The PDF Merge operation currently allows merging up to 5 files only.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.