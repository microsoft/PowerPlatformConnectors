# Typeform

Typeform is a conversational data collection platform for creating beautifully designed forms, surveys, quizzes, and polls. This connector enables Power Automate users to retrieve form responses, create and manage forms, and browse workspaces — powering scenarios like response processing, survey digests, and template management.

## Publisher
### Aaron Mah

## Prerequisites

You need a Typeform account (free tier works) and a Personal Access Token:

1. Log in to [admin.typeform.com](https://admin.typeform.com).
2. Click your avatar → **Account** → **Personal tokens** (or go directly to [admin.typeform.com/user/tokens](https://admin.typeform.com/user/tokens)).
3. Click **Generate a new token**, name it (e.g., "Power Automate").
4. Enable these scopes: `accounts:read`, `forms:read`, `forms:write`, `responses:read`, `responses:write`, `workspaces:read`.
5. Click **Generate token** and copy the value (shown once, starts with `tfp_`).
6. Paste the token into the **API Key** field when creating the connection in Power Automate.

## Supported Operations

### Get My Account Info
Retrieves the authenticated user's account information including alias and email. Useful for connection validation.

### List Forms
Retrieves a paginated list of forms in the authenticated account. Supports filtering by title keyword and workspace ID.

### Get Form
Retrieves the full definition of a specific form including fields, settings, and logic. Essential companion to Get Responses for resolving field IDs to human-readable question labels.

### Get Responses
Retrieves responses submitted to a specific form with filtering, sorting, and pagination. Supports date range filters (`since`/`until`), response type filtering, and cursor-based pagination. The primary operation for response processing flows.

### List Workspaces
Retrieves a paginated list of workspaces the authenticated user has access to. Used for organizational browsing and filtering forms by workspace.

### Delete Responses
Deletes specific responses from a form by response IDs. Supports GDPR compliance and data hygiene workflows. Deletion is asynchronous.

### Create Form
Creates a new form with specified title, fields, and settings. Enables dynamic form creation for feedback, intake, and event registration scenarios.

## API Documentation
Visit [Typeform Developer Portal](https://www.typeform.com/developers/) for further details.

## Known Issues and Limitations

- **Rate limit:** 2 requests per second per Typeform account. Flows with many sequential API calls should include delays.
- **Responses API doesn't include question labels:** `Get Responses` returns field IDs/refs but not question text. Use `Get Form` to resolve field labels.
- **Recent submissions may lag:** Very recent submissions (within the last 30 minutes) may not appear in the Responses API.
- **EU Data Center:** Users on EU Typeform accounts must use `api.eu.typeform.com`. This connector targets the default US endpoint (`api.typeform.com`).
- **Delete responses is asynchronous:** Returns HTTP 200 immediately but actual deletion happens in the background. Not-found IDs are silently ignored.
- **Cursor pagination incompatible with sort:** The `before`/`after` pagination tokens in Get Responses cannot be combined with the `sort` parameter.
- **Insights endpoint removed:** The `/insights/{form_id}/summary` endpoint (from Typeform Postman workspace) is plan-gated (HTTP 403 on free tier) and undocumented. Removed for reliability.
- **Duplicate Form removed:** Typeform has no native `POST /forms/{id}/copy` endpoint. To duplicate a form, use Get Form then Create Form with the same field structure.

## License
Distributed under the MIT License.
