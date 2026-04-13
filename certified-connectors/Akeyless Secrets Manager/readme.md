# Akeyless Secrets Manager

This connector calls the Akeyless REST API to retrieve secrets. **Access Id** and **Access Key** are configured **once** on the connection; each action only needs the **secret path** (`secret_name`).

## Publisher: Akeyless

## Prerequisites

- Akeyless account with permission to authenticate and read the target secrets.
- An **API Key** authentication method with **Access ID** and **Access Key** ([Authenticate with API Key](https://docs.akeyless.io/docs/auth-with-api-key)).

## Where to enter Access Id and Access Key

Power Platform’s **Basic authentication** type always uses two underlying slots called **username** and **password** in the HTTP `Authorization: Basic …` header. **That is how the protocol works**, not Akeyless naming. This connector maps them as:

| Basic slot (protocol) | What you enter (Akeyless) |
|----------------------|---------------------------|
| Username             | **Akeyless Access ID**    |
| Password             | **Akeyless Access Key**   |

In **apiProperties.json** the connection fields are labeled **Akeyless Access ID** and **Akeyless Access Key** so the connection dialog is clear. The script decodes the Basic header and sends `access-id` / `access-key` to Akeyless `/auth`.

1. **Custom connector → Security:** **Basic authentication** (must match OpenAPI `security`).  
2. **Flow / app → connection:** create or edit the connection and fill **Akeyless Access ID** and **Akeyless Access Key** (not your Microsoft account).

If the designer still shows the words “Username” / “Password” on the **Security** tab, that is the generic Basic-auth label; the **connection** form should still use the display names above after `apiProperties` is applied. **Re-import** `apiProperties.json` with your connector if labels look wrong.

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
