# LaunchDarkly

Manage feature flags, projects, and environments in LaunchDarkly from Power Automate. Create flags, toggle them on and off across environments, check flag statuses, copy configurations between environments, and monitor changes through the audit log — enabling automated feature flag management as part of your CI/CD and operational workflows.

## Publisher
### Aaron Mah

## Prerequisites

You need a LaunchDarkly account with an API access token to use this connector:

1. **Sign up** for a free LaunchDarkly Developer account at [https://app.launchdarkly.com/signup](https://app.launchdarkly.com/signup) (supports Google and GitHub SSO; no credit card required).
2. **Create an API access token:**
   - Log in to LaunchDarkly at [https://app.launchdarkly.com](https://app.launchdarkly.com).
   - Click the **gear icon** in the left sidebar → **Organization settings**.
   - Click **Authorization**.
   - In the "Access tokens" section, click **Create token**.
   - Enter a name (e.g., `Power Automate Connector`).
   - Select the **Writer** role (required for create, toggle, and delete operations; Reader suffices for read-only).
   - Click **Save token** and **copy the token immediately** — it is shown only once.
3. In Power Automate, create a new LaunchDarkly connection and paste the token.

## Supported Operations

### List Projects
List all projects in your LaunchDarkly account.

### Get Project
Get the details of a single LaunchDarkly project.

### List Environments
List all environments in a LaunchDarkly project.

### Get Environment
Get the details of a single environment in a project.

### List Feature Flags
List all feature flags in a LaunchDarkly project.

### Create Feature Flag
Create a new feature flag in a LaunchDarkly project.

### Get Feature Flag
Get the details of a single feature flag including its per-environment configuration.

### Turn Flag On
Turn on targeting for a feature flag in a specific environment.

### Turn Flag Off
Turn off targeting for a feature flag in a specific environment.

### Delete Feature Flag
Permanently delete a feature flag from a project.

### Copy Flag Configuration
Copy a feature flag's configuration from one environment to another.

### Get Flag Status
Get the status of a feature flag in a specific environment (active, inactive, launched, or new).

### List Audit Log Entries
List recent audit log entries for your LaunchDarkly account.

## API Documentation
Visit [LaunchDarkly API Docs](https://launchdarkly.com/docs/api) for further details.

## Known Issues and Limitations

- **Rate limits:** The LaunchDarkly API has rate limits that vary by endpoint and plan. If you hit rate limits, add delays between actions in your flows.
- **Token visibility:** API access tokens are shown only once when created. If lost, you must create a new token.
- **Plan-gated features:** Some advanced features (custom roles, service tokens, workflows, approvals, scheduled changes) require an Enterprise plan and are not available through this connector.
- **Semantic patch operations:** The Turn Flag On and Turn Flag Off operations use LaunchDarkly's Semantic Patch format. They cannot be combined with other flag modifications in a single call.
- **Pagination:** List operations return a maximum of 20 items by default. Use the Limit and Offset parameters to paginate through larger result sets.
- **Variation values:** Feature flag variation values can be boolean, string, number, or JSON. The connector returns them as-is from the API.

## License
Distributed under the MIT License.
