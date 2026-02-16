# PDF Export by PDFCrowd

PDFCrowd is a cloud-based HTML to PDF conversion service. This connector allows you to convert web pages, HTML content, and HTML files to high-quality PDF documents directly from your Power Automate flows.

## Publisher: PDFCrowd

## Prerequisites

- A PDFCrowd account ([sign up for free](https://pdfcrowd.com/user/sign_up/))
- Your PDFCrowd username and API key (found at [pdfcrowd.com/user/account/](https://pdfcrowd.com/user/account/))

## Supported Operations

### Convert HTML to PDF

Convert a web page URL, HTML string, or HTML file to a PDF document.

**Inputs:**
- **URL** - The URL of a publicly accessible web page to convert
- **HTML Content** - Raw HTML string to convert
- **HTML File** - An HTML file or archive (.zip, .tar.gz) containing HTML with assets
- **Page Size** - Output page size (A4, Letter, Legal, etc.)
- **Orientation** - Portrait or landscape
- **Margins** - Top, right, bottom, left margins
- **Custom CSS** - Additional CSS to apply during conversion
- **Custom JavaScript** - JavaScript to execute before conversion
- And 100+ more options for advanced customization

**Outputs:**
- PDF file content
- Response headers with conversion metadata:
  - `x-pdfcrowd-job-id` - Unique job identifier
  - `x-pdfcrowd-pages` - Number of pages in the PDF
  - `x-pdfcrowd-output-size` - PDF file size in bytes
  - `x-pdfcrowd-consumed-credits` - Credits used
  - `x-pdfcrowd-remaining-credits` - Credits remaining

### Get Account Info

Retrieve your PDFCrowd account information including remaining credits balance.

**Outputs:**
- `status` - Request status
- `credits` - Number of credits remaining on your account

## Obtaining Credentials

1. Go to [pdfcrowd.com/user/sign_up/](https://pdfcrowd.com/user/sign_up/) to create a free account
2. After signing in, visit [pdfcrowd.com/user/account/](https://pdfcrowd.com/user/account/)
3. Your **Username** and **API Key** are displayed on the account page
4. For testing, you can use username `demo` with API key `demo` (output will have a watermark)

## Getting Started

1. Add the **PDF Export by PDFCrowd** connector to your flow
2. Create a connection using your PDFCrowd username and API key
3. Configure the conversion action:
   - Enter a URL, HTML content, or upload an HTML file
   - Adjust page settings as needed
4. Use subsequent actions to save, email, or process the PDF output

## Known Issues and Limitations

- **Input requirement**: You must provide at least one of: URL, HTML Content, or HTML File
- **URL accessibility**: URLs must be publicly accessible (no authentication required)
- **File size**: Maximum input file size is 200MB
- **Timeout**: Conversions timeout after 40 seconds by default (configurable up to 300 seconds)
- **Rate limiting**: API requests are rate-limited based on your subscription plan

## Frequently Asked Questions

### How do I convert a password-protected page?

You can use the HTTP Username and HTTP Password parameters to provide Basic HTTP authentication credentials for the source URL.

### How do I add headers and footers?

Use the Header HTML and Footer HTML parameters to add custom headers and footers with dynamic page numbers. Use the special CSS classes `pdfcrowd-page-number` (current page) and `pdfcrowd-page-count` (total pages). For example:
```html
<div style="text-align:center">Page <span class="pdfcrowd-page-number"></span> of <span class="pdfcrowd-page-count"></span></div>
```

### Can I control which part of a page gets converted?

Yes, use the **Element To Convert** parameter with a CSS selector to convert only a specific element from the page (e.g., article content), excluding navigation, headers, and sidebars.

### What file formats can I convert?

The connector converts HTML, including:
- Web pages (via URL)
- Raw HTML strings
- HTML files
- ZIP archives containing HTML with CSS, images, and other assets

## Deployment Instructions

This connector is available in the Power Automate connector gallery. Search for "PDF Export by PDFCrowd" when adding a new action to your flow.

## Support

- **Documentation**: [pdfcrowd.com/api/](https://pdfcrowd.com/api/)
- **Support**: [pdfcrowd.com/contact/](https://pdfcrowd.com/contact/)
- **Email**: support@pdfcrowd.com
- **Privacy Policy**: [pdfcrowd.com/privacy/](https://pdfcrowd.com/privacy/)
- **Terms of Service**: [pdfcrowd.com/legal/](https://pdfcrowd.com/legal/)
