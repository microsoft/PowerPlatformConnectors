# Datadog

Datadog is a cloud-scale monitoring and security platform for infrastructure, applications, logs, and more. This connector enables Power Automate users to manage monitors, respond to incidents, post events, track SLOs, schedule downtimes, and react to infrastructure alerts — all without leaving the Power Platform.

## Publisher

### Aaron Mah

## Prerequisites

You need a Datadog account (Free, Pro, or Enterprise tier) to use this connector. All MVP operations are available to users with the Standard role (default).

To create the required API credentials:

1. Log in to Datadog at [https://app.datadoghq.com](https://app.datadoghq.com).
2. Navigate to **Organization Settings → API Keys** ([direct link](https://app.datadoghq.com/organization-settings/api-keys)).
3. Click **New Key**, name it (e.g., "Power Automate"), and copy the 32-character key.
4. Navigate to **Personal Settings → Application Keys** ([direct link](https://app.datadoghq.com/personal-settings/application-keys)).
5. Click **New Key**, name it (e.g., "Power Automate"), leave scopes empty (unscoped = full user permissions), and copy the 40-character key **immediately** — it may only be shown once.
6. In Power Automate, create a new Datadog connection and paste both keys.

**Note:** This connector targets the US1 Datadog site (`api.datadoghq.com`). Multi-site support (EU, US3, US5, GOV, AP1, AP2) is planned for a future version.

## Supported Operations

### List Monitors
Retrieves a list of all monitors in your Datadog organization, with optional filtering by name, tags, or state.

### Get Monitor
Retrieves the details of a specific monitor by its ID.

### Mute Monitor
Mutes a monitor to suppress alert notifications, optionally scoped to a specific group and with an expiration time.

### Unmute Monitor
Unmutes a previously muted monitor to resume alert notifications.

### List Incidents
Retrieves a list of incidents from your Datadog organization, sorted by creation date.

### Get Incident
Retrieves the full details of a specific incident by its ID.

### Create Incident
Declares a new incident in Datadog with a title, severity, and incident commander.

### Update Incident
Updates an existing incident's title, severity, state, or customer impact status.

### Post Event
Posts a custom event to the Datadog event stream, useful for tracking deployments, releases, or external system changes.

### Get Host Totals
Returns the total number of active and up hosts in your Datadog organization.

### List SLOs
Retrieves a list of all Service Level Objectives (SLOs) in your Datadog organization.

### Create Downtime
Schedules a downtime to suppress alert notifications for specific monitors or scopes during planned maintenance.

### When a monitor is triggered (Trigger)
Polls periodically for Datadog monitors in Alert or Warn state. Use this trigger to start flows when infrastructure problems are detected.

### When a new incident is created (Trigger)
Polls periodically for newly created Datadog incidents. Use this trigger to start flows when incidents are declared, enabling immediate on-call notification.

## API Documentation

Visit [Datadog API Documentation](https://docs.datadoghq.com/api/latest/) for further details.

## Known Issues and Limitations

- **US1 site only:** This connector targets `api.datadoghq.com` (US1). Users on EU, US3, US5, GOV, AP1, or AP2 sites cannot use this connector. Multi-site support is planned for v2.
- **Rate limits:** Datadog applies rate limits per API endpoint. Monitor listing is limited to 1000 monitors per API call. For large organizations, use filters (name, tags) to narrow results.
- **Incidents API is in public beta:** The Incidents endpoints (`/api/v2/incidents`) are in public beta and may change. Incident operations should be considered beta-quality.
- **Polling trigger latency:** The monitor and incident triggers use polling (not webhooks). There may be a delay of 1–5 minutes between a Datadog state change and the trigger firing in Power Automate, depending on your recurrence interval.
- **No monitor authoring:** Creating, updating, and deleting monitors is not supported in this connector. Monitor query syntax is a Datadog DSL that is too complex for Power Automate flows. Create monitors in the Datadog UI and use this connector to react to them.
- **Application key visibility:** Datadog Application Keys may only be visible at creation time (one-time read). Copy and store your key immediately when generating it.

## License

Distributed under the MIT License.
