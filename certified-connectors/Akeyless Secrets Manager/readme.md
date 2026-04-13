# Akeyless Secrets Manager

This connector calls the Akeyless REST API to retrieve secrets. **Akeyless Access ID** and **Akeyless Access Key** are configured **once** on the connection; each action only needs the **secret path** (`secret_name`).

## Publisher: Akeyless

## Prerequisites

- An Akeyless account and permissions to read the secrets you will call from Power Platform.
- An **API Key** authentication method that provides an **Access ID** and **Access Key** pair (see below).

## How to create the Access ID and Access Key (API Key authentication)

This connector uses Akeyless **API Key** authentication: a programmatic **Access ID** plus **Access Key**, not a human “email + password” for the Akeyless web console (unless you choose that flow elsewhere). Full product documentation: **[Authenticate with API Key](https://docs.akeyless.io/docs/auth-with-api-key)**.

### Create an API Key authentication method (Akeyless Console)

These steps match the official guide above:

1. Sign in to the **[Akeyless Console](https://console.akeyless.io)**.
2. Under **Administration**, open **Users & Auth Methods**.
3. Select **+ New** to open **Create Authentication Method**.
4. On the **Type** screen, choose **API Key**, then continue (**Next** / **Finish** as prompted).
5. Enter a **Name** (you can use path-style names with `/` if you organize auth methods in folders).
6. Complete the wizard. **Download the generated CSV** (or copy the values from the confirmation flow). It contains the **Access ID** and **Access Key**.

**Important:**

- The **Access Key** is shown **only once**. Store it in a secure vault or password manager. If you lose it, use **Reset Access Key** on that authentication method in the Console (documented in the same [API Key](https://docs.akeyless.io/docs/auth-with-api-key) page).
- Associate the new authentication method with an **Access Role** in Akeyless so it has permission to read the paths and secrets you use from Power Platform (see **What’s Next** on the [API Key](https://docs.akeyless.io/docs/auth-with-api-key) page and [RBAC](https://docs.akeyless.io/docs/rbac) in Akeyless documentation).

### Optional: CLI

You can also create an API Key auth method with the Akeyless CLI; see the **“Creating an API Key Authentication Method with the CLI”** section on **[Authenticate with API Key](https://docs.akeyless.io/docs/auth-with-api-key)**. The CLI returns credentials once—store them securely.

### How this maps to Power Platform

After you have the pair from Akeyless:

- Put **Access ID** in the first Basic-auth field (labeled **Akeyless Access ID** or “username” depending on screen).
- Put **Access Key** in the second field (**Akeyless Access Key** or “password”).

The connector sends them to Akeyless `/auth` with `access-type` **`access_key`**, consistent with Akeyless CLI usage (`akeyless auth --access-type access_key --access-id … --access-key …`) described in the [same documentation](https://docs.akeyless.io/docs/auth-with-api-key).

### Akeyless guidance on use of API Key auth

Akeyless documents API Key auth as suited to **CLI, SDK, and automation** (including integrations like this connector). Their page also notes it is **not recommended for direct interactive Console sign-in** and is **not recommended for production** for some scenarios—evaluate against your security standards and consider other [authentication methods](https://docs.akeyless.io/docs/auth-with-api-key) if your organization requires them.

## Why the product may say “Username” and “Password” (important)

Power Platform and the HTTP standard use a connection type called **Basic authentication**. In that standard, the two credential fields are always named **username** and **password** in the protocol. **Those names are fixed by Microsoft and the web standard — they are not Akeyless-specific.**

**What you should do:** Ignore the generic words *username* and *password* as meaning “Microsoft account” or “Windows login.” For this connector they mean:

| What you might see in the UI | What you actually enter |
|-------------------------------|-------------------------|
| Username (or “Akeyless Access ID” if labels updated) | Your **Akeyless Access ID** from the Akeyless Console (often starts with `p-`) |
| Password (or “Akeyless Access Key” if labels updated) | Your **Akeyless Access Key** from the same API Key authentication method |

**Do not enter:** your Microsoft 365 email, your Power Platform sign-in, or any password other than the **Akeyless Access Key** secret.

**Do enter:** exactly the **Access ID** and **Access Key** that Akeyless issued for API Key authentication, as described in [Akeyless API Key documentation](https://docs.akeyless.io/docs/auth-with-api-key).

The connector receives those two values in the standard Basic header, then calls Akeyless `/auth` using `access-id` and `access-key` as Akeyless expects.

### Where to type them in Power Automate / Power Apps

1. **Custom connector (authoring):** **Security** → authentication type **Basic authentication** (required for this connector).
2. **When you use the connector in a flow or app:** when you **create or edit the connection** (Sign in / Connections), you will see two fields. Use them only for **Akeyless Access ID** and **Akeyless Access Key**, in that order (first field = Access ID, second field = Access Key).

On some screens the **Security** tab of the connector designer may still show the generic labels “Username” and “Password.” That is normal for Basic auth. The **connection** experience in flows should show clearer names (**Akeyless Access ID** / **Akeyless Access Key**) when the connector’s `apiProperties.json` is imported; if not, use the table above as the mapping.

### What you enter on each action

For **Get Secret** and **Get Password**, you only need **`secret_name`** (the full path to the secret in Akeyless), unless you intentionally override credentials in the body for advanced scenarios.

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
3. Create a **connection** and enter your **Akeyless Access ID** and **Akeyless Access Key** once; test **Get Secret** and **Get Password** with a real `secret_name`.

## Certification

Open source under `certified-connectors/`, PR to `dev`, label `certified-connector`; after merge, submit via [ISV Studio](https://isvstudio.powerapps.com) per [Submit your connector for certification](https://learn.microsoft.com/connectors/custom-connectors/submit-certification).
