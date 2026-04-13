# Akeyless Secrets Manager

This connector calls the Akeyless REST API to retrieve secrets. **Access Id** and **Access Key** are configured **once** on the connection; each action only needs the **secret path** (`secret_name`).

## Publisher: Akeyless

## Prerequisites

- Akeyless account with permission to authenticate and read the target secrets.
- An **API Key** authentication method with **Access ID** and **Access Key** ([Authenticate with API Key](https://docs.akeyless.io/docs/auth-with-api-key)).

## Where to enter Access Id and Access Key

The OpenAPI file declares **Basic** security (`securityDefinitions` / `security`) so Power Platform sends **`Authorization: Basic …`** on each request using the connection.

1. **In the custom connector (designer):** open **Security** → **Authentication type** must be **Basic authentication** (it should align with the OpenAPI; if you changed the Swagger, **Update connector** after re-import).  
2. **In a flow or app:** add an action from this connector → **Sign in** / **Create connection** → enter **Access Id** and **Access Key** once. Those values are stored on the **connection**, not on each action.

If you still see the script error about missing credentials, **re-import** or **Update from OpenAPI** so the definition includes Basic security, then **delete the old connection** and create a new one.

In each flow action you only provide **`secret_name`** (full path to the secret in Akeyless).

**Optional override:** You can still pass `access-id` and `access-key` in the action body; if present, they take precedence over the connection (for advanced scenarios).

## Supported operations

### Get Secret

Retrieves a plain or text-oriented secret value (`json: false` on the Akeyless `get-secret-value` call).

### Get Password

Retrieves structured credential-style fields (`json: true` on the Akeyless `get-secret-value` call).

## Known issues and limitations

- **`iconBrandColor`:** `#0E4D45` — not `#ffffff` or `#007ee5` (certified rules). Update if marketing requires a different approved color.
- **Read-focused:** Get secret / password only.

## Deployment instructions

1. Validate: `paconn validate --api-def apiDefinition.swagger.json`.
2. Import `apiDefinition.swagger.json`, `apiProperties.json`, and `script.csx` as a custom connector.
3. Create a **connection** and enter **Access Id** and **Access Key** once; test **Get Secret** and **Get Password** with a real `secret_name`.

## Certification

Open source under `certified-connectors/`, PR to `dev`, label `certified-connector`; after merge, submit via [ISV Studio](https://isvstudio.powerapps.com) per [Submit your connector for certification](https://learn.microsoft.com/connectors/custom-connectors/submit-certification).
