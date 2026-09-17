# UniAI (Independent Publisher)

UniAI is a powerful API that allows you to extract structured data from documents using UniDoc and AI. This connector enables users to automatically parse **Invoices** and **Resumes/CVs** directly within Power Automate flows, converting PDF documents into usable JSON data.

## Publisher: [Roman Bezushko]

## Prerequisites
To use this connector, you need:
* A UniCloud account.
* An active UniCloud API Key.

## Supported Operations

### Extract Data from Invoice
Upload a PDF invoice to extract key details, including:
* Vendor Name
* Total Amount
* Invoice Date
* Invoice Number
* Currency

### Extract Data from Resume
Upload a PDF resume or CV to extract candidate details, including:
* Candidate Name
* Contact Email & Phone
* List of Skills
* Education History

## Obtaining Credentials
1.  Log in to your UniAI Dashboard at [https://cloud.unidoc.io](https://cloud.unidoc.io).
2.  Navigate to the **Settings** or **API Keys** section.
3.  Click **Generate New Key**.
4.  Copy the API Key string.
5.  In Power Automate, when creating the connection, paste this key into the **API Key** field.

## Getting Started
1.  **Create a Flow:** Start a new "Instant Cloud Flow" in Power Automate.
2.  **Add the Action:** Search for "UniAI" and select either **Extract Data from Invoice** or **Extract Data from Resume**.
3.  **File Content:** Pass the file content from a previous step (e.g., "Get file content" from OneDrive or SharePoint).
4.  **Run:** Save and test the flow. The output will be a JSON object containing the extracted fields.

## Known Issues and Limitations
* **File Size:** The API currently supports files up to 3 pages.
* **File Type:** Only PDF files are currently supported.
