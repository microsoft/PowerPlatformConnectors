# Datadog

Datadog is a cloud-scale monitoring and security platform for infrastructure, applications, logs, and more. This connector enables Power Automate flows to interact with Datadog monitors, incidents, events, hosts, SLOs, and downtimes — powering alert routing, incident management, deployment coordination, and operational health reporting.

## Publisher

### Aaron Mah

## Prerequisites

You need a Datadog account to use this connector. A free trial is available at [https://www.datadoghq.com](https://www.datadoghq.com).

To create the required API credentials:

1. Log in to Datadog at [https://app.datadoghq.com](https://app.datadoghq.com).
2. Navigate to **Organization Settings → API Keys** ([direct link](https://app.datadoghq.com/organization-settings/api-keys)).
3. Click **New Key**, name it (e.g., "Power Automate"), and copy the 32-character key.
4. Navigate to **Personal Settings → Application Keys** ([direct link](https://app.datadoghq.com/personal-settings/application-keys)).
5. Click **New Key**, name it (e.g., "Power Automate"), leave scopes empty (unscoped = full user permissions), and copy the 40-character key immediately — it may only be shown once.
6. In Power Automate, create a new Datadog connection and paste both keys.

**Required permissions:** Standard Datadog role (default). No special roles or scopes needed.

**Note:** This connector targets the US1 Datadog site (`api.datadoghq.com`). Users on other sites (EU, US3, US5, GOV, AP1, AP2) are not supported in this version.

## Supported Operations

### List Monitors
Retrieves a list of all monitors in your Datadog organization, with optional filtering by name, tags, or state.

### Get Monitor
Retrieves the details of a specific monitor by its ID, including thresholds, options, and current state.

### Mute Monitor
Mutes a monitor to suppress alert notifications, optionally scoped to a specific group and with an expiration time.

### Unmute Monitor
Unmutes a previously muted monitor to resume alert notifications.

### List Incidents
Retrieves a list of incidents from your Datadog organization, sorted by creation date.

### Get Incident
Retrieves the full details of a specific incident by its ID, including severity, state, and commander.

### Create Incident
Declares a new incident in Datadog with a title, severity, and incident commander.

### Update Incident
Updates an existing incident's title, severity, state, or customer impact status.

### Post Event
Posts a custom event to Datadog's event stream, useful for tracking deployments, releases, or external system changes.

### Get Host Totals
Returns the total number of active and up hosts in your Datadog organization.

### List SLOs
Retrieves a list of all Service Level Objectives (SLOs) in your Datadog organization.

### Create Downtime
Schedules a downtime to suppress alert notifications for specific monitors or scopes during planned maintenance.

## API Documentation

Visit [Datadog API Documentation](https://docs.datadoghq.com/api/latest/) for further details.

## Known Issues and Limitations

- **US1 site only**: This connector targets `api.datadoghq.com` (US1). Users on EU, US3, US5, GOV, AP1, or AP2 sites must wait for multi-site support in a future version.
- **Incidents API is in public beta**: The Incidents v2 API is in public beta. Behavior may change.
- **Rate limits**: Datadog enforces rate limits on API calls. The default rate limit is 300 requests per hour for most endpoints. Monitor and incident endpoints may have different limits. See [Datadog Rate Limiting](https://docs.datadoghq.com/api/latest/rate-limits/) for details.
- **Pagination**: List operations return paginated results. Use the page/offset parameters to retrieve additional pages.
- **Monitor creation not included**: Creating monitors requires Datadog-proprietary query syntax. Create monitors in the Datadog UI and use this connector to read and manage them.

## License

Distributed under the MIT License.
