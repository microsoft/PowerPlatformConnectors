# Akeyless Secrets Manager

This connector integrates **Akeyless Secrets Manager** with Microsoft Power Platform (Power Apps, Power Automate, and Azure Logic Apps where applicable). It lists items in a vault path and retrieves secret values using the Akeyless REST API.

## Publisher: Akeyless

## Prerequisites

- Akeyless tenant with permissions to call `/auth`, `/list-items`, and `/get-secret-value` for the paths and secrets you use.
- An **API Key** authentication method providing **Access ID** and **Access Key** ([Authenticate with API Key](https://docs.akeyless.io/docs/auth-with-api-key)).

## Supported operations

### List Items

Lists folders and items under a vault path. Optional filters and pagination. API reference: [list-items](https://docs.akeyless.io/reference/listitems).

### Get Secret

Retrieves a secret value for a plain or text-oriented secret.

### Get Password

Retrieves structured credential fields (for example `username` and `password`) when the underlying secret is returned as JSON.

## Obtaining credentials

1. Open the [Akeyless Console](https://console.akeyless.io).
2. Navigate to **Access** → **Authentication Methods**.
3. Create or use an **API Key** authentication method.
4. Copy the **Access ID** and **Access Key** into the connector connection (connection stores credentials; actions use `secret_name` and optional list parameters only).

## Known issues and limitations

- This version focuses on **read** scenarios (list and get secret). It does not expose create, update, or delete through the connector.
- **Brand color:** `iconBrandColor` in `apiProperties.json` must remain a valid certified color (not `#ffffff` or `#007ee5`). Update `#0E4D45` only if Akeyless brand guidelines or Microsoft certification require a different approved value.
- Responses follow Akeyless API JSON; flows may need parse steps depending on secret types.

## Deployment instructions

1. Clone or fork [microsoft/PowerPlatformConnectors](https://github.com/microsoft/PowerPlatformConnectors) and work on a branch from `dev`.
2. Validate: `paconn validate --api-def apiDefinition.swagger.json`.
3. Import `apiDefinition.swagger.json`, `apiProperties.json`, and `script.csx` as a custom connector in a development environment, create a connection, and test **List Items**, **Get Secret**, and **Get Password**.

## Certification

New certified connectors must be open sourced in this repository ([README](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/README.md)).

1. Open a pull request to **`dev`** with label **`certified-connector`**.
2. After merge, the connector owner submits certification in **[ISV Studio](https://isvstudio.powerapps.com)** per [Submit your connector for certification](https://learn.microsoft.com/connectors/custom-connectors/submit-certification).
3. Complete the [pull request template](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/.github/pull_request_template.md) attestations. OAuth is not used for this connector (note **N/A** where applicable).

## Migration note (managed solution export)

If you previously exported a **managed solution** from Power Platform (`AkeylessSecretsManager_*_managed`), that package is for **environment import**, not for direct submission as the GitHub source of truth. This folder replaces the problematic values from typical exports (for example `#ffffff` brand color and empty `connectionparameters.json`) with a layout and metadata suitable for certification and community contribution.
