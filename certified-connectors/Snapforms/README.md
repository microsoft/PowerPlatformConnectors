# Snapforms Connector

Snapforms helps teams create and share online forms, collect structured response data, and automate downstream processes. The Snapforms connector for Microsoft Power Automate lets you trigger flows when form responses are submitted and retrieve temporary download URLs for response files and PDFs.

## Publisher: Snapforms

## Prerequisites

You need:

1. An active Snapforms account with access to at least one form.
2. A Microsoft Power Automate or Power Apps environment.
3. Permission to view the forms and form responses you want to use in flows.

## Supported Operations

The connector supports the following user-facing operations:

### When a form response is submitted

Triggers a flow when a response is submitted to a selected Snapforms form. The trigger payload uses the live schema of the selected form so the response fields appear as dynamic content in Power Automate.

### Retrieve file URL for download

Returns a temporary download URL for a file uploaded in a Snapforms response.

### Retrieve PDF URL for download

Returns a temporary download URL for a PDF generated from a Snapforms response.

The connector also includes internal helper operations that populate form pickers and dynamic response schemas.

## Obtaining Credentials

When the certified connector is published, no customer-managed API credentials are required. Users create a connection by signing in with their Snapforms account and authorizing access.

If you deploy the connector directly from this repository as a custom connector for local testing, replace the placeholder OAuth client ID in `apiProperties.json` with a valid Snapforms OAuth client ID and provide the matching client secret during connector creation or update.

## Getting Started

1. Create a new automated cloud flow in Power Automate.
2. Search for the Snapforms trigger named `When a form response is submitted`.
3. Select the Snapforms form you want to monitor.
4. Add downstream actions that use the response fields as dynamic content.
5. If your form includes uploaded files or you need a generated PDF, call the relevant Snapforms action to retrieve a temporary download URL and pass that URL into the next action in your flow.

## Known Issues and Limitations

1. File and PDF actions return temporary URLs rather than binary file content. The URLs expire after a short period, so use them immediately in the same flow run.
2. The response schema is dynamic and reflects the current structure of the selected form. If the form changes, refresh the flow step configuration so Power Automate can pick up the latest schema.
3. The legacy direct-download file and PDF actions are intentionally not included in this certified connector. Use the temporary URL actions instead.
4. PDF generation is unavailable for Snapforms response types that do not support PDF output.

## Frequently Asked Questions

### Do I need to create an OAuth app in Snapforms?

No. The published certified connector is intended to use a Snapforms-managed OAuth application.

### Why do I see temporary URLs instead of downloadable files?

Snapforms returns signed temporary URLs to improve reliability and avoid the size limits that affected older direct-download actions.

## Deployment Instructions

To deploy this connector as a custom connector for testing before publication:

1. Import `apiDefinition.swagger.json`, `apiProperties.json`, and `icon.png` into Power Automate or Power Apps as a custom connector.
2. Configure OAuth 2.0 using the Snapforms authorization and token endpoints at `https://user.snapforms.com.au/oauth/authorize` and `https://user.snapforms.com.au/oauth/token`.
3. Replace the placeholder OAuth client ID in `apiProperties.json` with a valid Snapforms OAuth client ID and provide the matching client secret when you create or update the connector.
4. Create a connection and authorize it with a Snapforms user who has access to the forms you want to use.
