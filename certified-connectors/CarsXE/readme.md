# CarsXE Custom Connector for Power Automate & Power Apps

The CarsXE Connector enables users to instantly access powerful automotive data inside Power Automate and Power Apps. With this connector, you can decode VINs, get detailed specs, estimate market value, retrieve vehicle history, access safety recalls, and much more — all through easy, no-code integration. Transform your automotive workflows with comprehensive vehicle intelligence at your fingertips.

## Publisher: CarsXE

## Prerequisites

- A valid **CarsXE API Key** (obtain from [https://api.carsxe.com/dashboard/developer](https://api.carsxe.com/dashboard/developer))
- **Microsoft Power Automate** or **Power Apps** license
- Access to create custom connectors in your Power Platform environment

## Supported Operations

The CarsXE connector provides 11 powerful operations covering comprehensive automotive data retrieval:

### Get Vehicle Specs
Decode a VIN and retrieve full vehicle specifications including make, model, year, engine details, transmission, and more. Supports optional deep data for enhanced information.

### Decode International VIN
Decode Vehicle Identification Numbers with worldwide support for international vehicles, providing comprehensive decoding capabilities beyond standard US VINs.

### Decode License Plate
Decode license plate information by plate number, state, and country. Supports US, Canada, UK, and other international regions with state/province and district refinement options.

### Get Market Value
Estimate vehicle market value based on VIN, with optional state-specific refinements to provide accurate regional pricing data.

### Get Vehicle History
Retrieve comprehensive vehicle history including ownership records, accident reports, title information, odometer readings, and service history.

### Get Vehicle Images
Fetch high-quality vehicle images by make, model, year, and trim. Supports customization options including color, angle, photo type, size, transparent backgrounds, and licensing.

### Get Safety Recalls
Access safety recall data and active campaigns for specific vehicles by VIN, helping ensure vehicle safety compliance.

### Plate Image Recognition
Read and decode license plates from images using advanced OCR (Optical Character Recognition) technology. Simply provide an image URL to extract plate information.

### VIN OCR
Extract Vehicle Identification Numbers from images using OCR technology. Useful for scanning VIN stickers, plates, or documents.

### Year Make Model Query
Query detailed vehicle information by specifying year, make, model, and optionally trim level. Perfect for building vehicle selection workflows.

### OBD Codes Decoder
Decode OBD (On-Board Diagnostics) error and diagnostic codes to understand vehicle issues and maintenance requirements.

## Obtaining Credentials

This connector uses **API Key** authentication.

1. Visit the CarsXE website at [https://api.carsxe.com](https://api.carsxe.com)
2. Sign up for an account or log in to your existing account
3. Navigate to the Developer Dashboard at [https://api.carsxe.com/dashboard/developer](https://api.carsxe.com/dashboard/developer)
4. Generate or copy your API key
5. When creating a new connection in Power Automate or Power Apps, enter this API key when prompted
6. The connector will automatically validate your key with CarsXE's `/auth/validate` endpoint

## Getting Started

### Quick Start Guide

1. **Import the Connector**: Import the CarsXE custom connector into your Power Platform environment
2. **Create a Connection**: Navigate to Data → Custom Connectors → CarsXE, then create a new connection using your API key
3. **Build Your First Flow**: 
   - Create a new automated flow in Power Automate
   - Add a trigger (e.g., "When an item is created" in SharePoint)
   - Add the "Get Vehicle Specs" action from CarsXE connector
   - Provide a VIN number (test with: `WBAFR7C57CC811956`)
   - Add actions to use the returned data (send email, update database, etc.)

### Example Use Cases

**VIN Decoder Flow**
- **Trigger**: When a VIN is submitted in a Microsoft Form or Excel
- **Action**: Call CarsXE → Get Vehicle Specs
- **Output**: Send decoded data (make, model, year, engine details) via email or Teams message

**License Plate Lookup**
- **Trigger**: Upload license plate image to SharePoint
- **Action**: Call CarsXE → Plate Image Recognition
- **Output**: Store extracted plate data in Dataverse or SQL database

**Safety Recall Notification**
- **Trigger**: New vehicle added to inventory (Dataverse or SharePoint)
- **Action**: Call CarsXE → Get Safety Recalls
- **Output**: Send automated Teams alert or email if recalls are found

**Market Value Assessment**
- **Trigger**: Manual trigger or scheduled flow
- **Action**: Call CarsXE → Get Market Value for VINs in inventory
- **Output**: Update pricing in Excel or database with current market values

## Known Issues and Limitations

- **Rate Limits**: API calls are subject to rate limits based on your CarsXE subscription plan. Exceeding these limits may result in temporary throttling.
- **VIN Format**: VINs must be valid 17-character Vehicle Identification Numbers. Invalid or incomplete VINs will return error responses.
- **International Data**: While international VIN decoding is supported, data completeness may vary by region and vehicle manufacturer.
- **Image Recognition**: OCR-based operations (Plate Image Recognition, VIN OCR) require clear, well-lit images for best results. Image quality directly affects accuracy.
- **License Plate Coverage**: License plate decoding coverage varies by country and region. Not all international plates may be supported.
- **Historical Data**: Vehicle history data availability depends on reporting sources and may not be comprehensive for all vehicles.
- **Market Value Accuracy**: Market value estimates are based on available data and may vary from actual market conditions. Regional variations may apply.

## Frequently Asked Questions

### How do I get a CarsXE API key?
Visit [https://api.carsxe.com/dashboard/developer](https://api.carsxe.com/dashboard/developer) to sign up and obtain your API key. Various subscription plans are available based on your usage needs.

### What happens if my API key expires or is invalid?
The connector automatically detects 401 (Unauthorized) responses and marks the connection as unauthenticated. You'll need to create a new connection with a valid API key.

### Can I use this connector in Power Apps?
Yes! After deploying the custom connector and creating a connection, you can use all CarsXE operations in both Power Automate flows and Power Apps canvas/model-driven apps.

### Which VIN formats are supported?
The connector supports standard 17-character VINs used globally. For international vehicles, use the "Decode International VIN" operation for enhanced compatibility.

### How accurate is the market value estimation?
Market value estimates are based on comprehensive data sources and algorithms. For best results, provide the VIN and optionally the state code to refine regional pricing. Values should be used as estimates and verified against current market conditions.

### Does the connector work with European or Asian vehicles?
Yes, the International VIN Decoder operation supports worldwide vehicle decoding. However, data completeness may vary depending on the manufacturer and region.

### What image formats are supported for OCR operations?
Standard web image formats (JPEG, PNG) accessed via public URLs are supported. Ensure images are clear, well-lit, and high-resolution for optimal OCR accuracy.

### Are there any costs associated with using this connector?
The connector itself is free to use, but you need an active CarsXE API subscription. API usage is billed according to your CarsXE plan. Check [https://api.carsxe.com](https://api.carsxe.com) for pricing details.

## Deployment Instructions

### Method 1: Import from Solution File

1. Download the CarsXE custom connector solution file
2. Navigate to [Power Automate](https://make.powerautomate.com) or [Power Apps](https://make.powerapps.com)
3. Go to **Solutions** in the left navigation
4. Click **Import solution**
5. Select the downloaded solution file and follow the import wizard
6. After import, navigate to **Data** → **Custom Connectors**
7. Locate **CarsXE** in the list
8. Click **+ New connection** and enter your API key

### Method 2: Manual Custom Connector Creation

1. Navigate to [Power Automate](https://make.powerautomate.com)
2. Go to **Data** → **Custom Connectors**
3. Click **+ New custom connector** → **Import an OpenAPI file** (if available) or **Create from blank**
4. Configure the connector:
   - **General**: Set connector name to "CarsXE", upload icon, set host to `api.carsxe.com`
   - **Security**: Set authentication type to "API Key", parameter label to "API Key", parameter name to `key`, location to "Query"
   - **Definition**: Add each operation (specs, international-vin-decoder, platedecoder, etc.) with appropriate parameters
   - **Code**: Add policy templates for format=json parameter and 401 handling
   - **Test**: Create a connection and test operations
5. Click **Create connector**
6. Create a new connection using your CarsXE API key

### Method 3: Use in Power Apps

1. After deploying the custom connector (Method 1 or 2)
2. Open your Power Apps canvas app or create a new one
3. Click **Data** in the left panel
4. Click **+ Add data**
5. Search for and select **CarsXE**
6. Create or select an existing connection
7. Use CarsXE operations in your app formulas and galleries

### Verification

After deployment, verify the connector works correctly:

1. Go to **Data** → **Custom Connectors** → **CarsXE**
2. Navigate to the **Test** tab
3. Select your connection
4. Test the "Get Vehicle Specs" operation with VIN: `WBAFR7C57CC811956`
5. Verify you receive a successful 200 response with vehicle data

---

**Need Help?** Visit the [CarsXE API Documentation](https://api.carsxe.com/docs) or contact support through your dashboard at [https://api.carsxe.com/dashboard](https://api.carsxe.com/dashboard).

**Links:**
- **Website**: [https://api.carsxe.com](https://api.carsxe.com)
- **API Documentation**: [https://api.carsxe.com/docs](https://api.carsxe.com/docs)
- **Developer Dashboard**: [https://api.carsxe.com/dashboard/developer](https://api.carsxe.com/dashboard/developer)