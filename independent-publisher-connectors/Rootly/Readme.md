# Rootly

Rootly is an AI-powered incident management platform that helps engineering teams declare, manage, and resolve incidents. This connector enables Power Automate makers to create and track incidents, manage alerts, and follow up on post-incident action items — connecting Rootly to the broader Microsoft ecosystem including Teams, Azure DevOps, SharePoint, and Planner.

## Publisher
### Aaron Mah

## Prerequisites

You will need the following to use this connector:

1. A Rootly account (14-day free trial available at [rootly.com/users/sign_up](https://rootly.com/users/sign_up), or an active paid subscription).
2. A Global API Key generated from your Rootly organization:
   - Log in to Rootly at [rootly.com](https://rootly.com).
   - Click the organization dropdown → **Organization Settings** → **API Keys**.
   - Click **Generate New API Key** and select **Global API Key**.
   - Copy the token immediately (it is shown only once).

## Supported Operations

### List Incidents
Retrieves a list of incidents from Rootly with optional filtering by status and severity.

### Get Incident
Retrieves the details of a specific incident by its ID.

### Create Incident
Creates a new incident in Rootly with a title and severity.

### Update Incident
Updates an existing incident's status, severity, or other fields.

### List Severities
Retrieves all severity levels defined in your Rootly organization.

### List Services
Retrieves all services from the Rootly service catalog.

### Create Alert
Creates a new alert in Rootly from an external source.

### List Alerts
Retrieves a list of alerts from Rootly with optional filtering.

### Acknowledge Alert
Acknowledges an open alert in Rootly, indicating someone is looking at it.

### Resolve Alert
Resolves an alert in Rootly, marking it as no longer active.

### List Action Items
Retrieves all incident action items (tasks and follow-ups) across incidents.

### Create Action Item
Creates a new action item (task or follow-up) for an incident.

### Update Action Item
Updates an existing action item's status, priority, or other fields.

### List Teams
Retrieves all teams (groups) defined in your Rootly organization.

## API Documentation

Visit [Rootly Developer Docs](https://docs.rootly.com/api-reference/overview) for further details.

## Known Issues and Limitations

- **No free tier:** Rootly requires a 14-day trial or paid subscription. API keys cannot be generated without an active account.
- **Rate limits:** 3,000 API calls per minute for GET/HEAD/OPTIONS requests; 3,000 per minute for POST/PUT/PATCH/DELETE requests. Alert creation is limited to 50 alerts per minute.
- **Pagination:** List operations return a maximum of 25 items per page by default. Use the Page Number and Page Size parameters to paginate through larger result sets.
- **JSON:API format:** All request and response bodies follow the JSON:API specification. Response data is nested under `data.attributes` — use expressions like `body('action')?['data']?['attributes']?['title']` to access fields in Power Automate.
- **Bearer token format:** When creating a connection, enter your raw API key (without the "Bearer " prefix). The connector handles the Bearer prefix automatically.

## License

Distributed under the MIT License.
