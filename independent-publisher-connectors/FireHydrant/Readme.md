# FireHydrant

FireHydrant is a leading incident management platform for engineering and DevOps teams. This connector enables you to manage the full incident lifecycle — declare, track, update, resolve, and close incidents — directly from Power Automate. Coordinate response teams, post status updates, and build automated workflows that keep your organization informed during incidents.

## Publisher

### Aaron Mah

## Prerequisites

You need a FireHydrant account (paid plan or free trial) with Owner permissions to create a bot token.

1. Log in to FireHydrant at [https://app.firehydrant.io](https://app.firehydrant.io).
2. Navigate to **Organization Settings → Bot users** ([https://app.firehydrant.io/organizations/bots](https://app.firehydrant.io/organizations/bots)).
3. Click **+ Create bot user**, enter a name (e.g., "Power Automate Bot"), and click Save.
4. **Copy the token immediately** — it is shown only once. The token starts with `fhb-`.
5. When creating the connection in Power Automate, enter the token as `Bearer fhb-yourtoken`.

**Note:** FireHydrant does not have a permanent free tier. A paid plan (Starter at $20/user/month) or active free trial is required for API access. See [FireHydrant Pricing](https://firehydrant.com/pricing/) for details.

## Supported Operations

### List Incidents
Retrieves a list of incidents from FireHydrant, optionally filtered by status, severity, or search query.

### Get Incident
Retrieves the full details of a specific incident by its ID.

### Create Incident
Declares a new incident in FireHydrant with the specified name, severity, and optional details.

### Update Incident
Updates the fields of an existing incident, such as severity, description, or customer impact summary.

### List Severities
Retrieves all severity levels configured in the FireHydrant organization.

### List Teams
Retrieves all teams configured in the FireHydrant organization.

### Create Incident Note
Adds a status update note to an incident's timeline.

### List Incident Timeline
Retrieves the timeline of events for a specific incident, including notes, milestone changes, and status updates.

### Resolve Incident
Marks an incident as resolved, setting the resolved milestone timestamp.

### Close Incident
Closes an incident, marking it as fully complete after post-incident review.

### List Users
Retrieves all users in the FireHydrant organization.

### Get Current User
Retrieves the profile of the currently authenticated bot user, useful for verifying the connection is working.

## API Documentation

Visit [FireHydrant Developer Docs](https://developers.firehydrant.com/) for further details.

## Known Issues and Limitations

- **Rate limiting:** The FireHydrant API enforces a rate limit of 300 requests per minute per account. Flows with high-frequency polling or large batch operations should include appropriate delays.
- **Pagination:** List operations return 20 items per page by default, with a maximum of 200 per page. Use the `page` and `per_page` parameters to retrieve additional results.
- **No free tier:** FireHydrant requires a paid plan or active free trial for API access. Bot tokens cannot be created without an active subscription.
- **Bot token permissions:** Bot tokens have full API access. There is no way to restrict a token to specific operations or resources.
- **Tag format:** Tags must be provided as a comma-separated string when creating or updating incidents.

## License

Distributed under the MIT License.
