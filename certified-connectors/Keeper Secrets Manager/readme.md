# Keeper Secrets Manager

Keeper Secrets Manager (KSM) is a zero-knowledge secrets management platform by [Keeper Security](https://www.keepersecurity.com). This connector enables Microsoft Power Automate flows and Azure Logic Apps workflows to securely retrieve, create, and update secrets stored in a Keeper Vault, eliminating the need for hardcoded credentials in automation pipelines.

## Publisher: Keeper Security, Inc.

## Prerequisites

You will need the following to proceed:

- A [Keeper Security](https://www.keepersecurity.com) **Enterprise** account with Secrets Manager enabled.
- A Keeper Secrets Manager **application** configured in the [Keeper Vault](https://keepersecurity.com/vault) with at least one shared folder granted to the application.
- A **Base64-encoded KSM configuration token** (one-time access token) generated from the Admin Console.
- An [Azure subscription](https://azure.microsoft.com/free/) with permissions to create and manage Azure Function Apps.
- The Keeper Secrets Manager **middleware Azure Function App** deployed and running in your Azure subscription. Deploy it using the [Deploy to Azure](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FKeeper-Security%2Fazure-logic-apps%2Fmain%2Fazuredeploy.json) one-click template or follow the [repository README](https://github.com/Keeper-Security/azure-logic-apps).
- The Azure Function **host key** for authenticating requests to the middleware.

## Supported Operations

### List Secrets

Returns all secret records accessible by the configured Keeper Secrets Manager application, including the record UID, title, type, and folder UID.

### Get Secret

Retrieves the full details of a single secret by its unique record UID, including login, password, URL, notes, and custom fields.

### Create Secret

Creates a new login-type secret record in a specified Keeper Vault shared folder.

### Update Secret

Updates one or more fields on an existing secret identified by its UID. Only the fields included in the request body are modified; omitted fields remain unchanged.

### List Folders

Returns all folders accessible by the configured KSM application, including each folder's UID, name, parent folder UID, and record count.

## Obtaining Credentials

### Step 1: Configure a Keeper Secrets Manager Application

1. Sign in to the [Keeper Vault](https://keepersecurity.com/vault).
2. Go to **Secrets Manager** > **Create Application** > give it a name (e.g., "Azure Logic Apps Connector").
3. Share one or more vault folders with the application. These folders determine which secrets the connector can access.
4. Open the **Devices** tab > **Add Device** > select **Configuration File** > choose **Base64**.
5. Copy the Base64 value immediately -- this is your `KSM_CONFIG`. It can only be used once; if lost, add a new device to generate a fresh token.

> **Important**: Treat this value like a password. It contains your application credentials in an encrypted, Base64-encoded format. Do not commit it to source control.

### Step 2: Deploy the Azure Function Middleware

Deploy the middleware Function App to your Azure subscription using the [Deploy to Azure](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FKeeper-Security%2Fazure-logic-apps%2Fmain%2Fazuredeploy.json) one-click template or by following the [azure-logic-apps README](https://github.com/Keeper-Security/azure-logic-apps). After deployment, note the Function App URL (for example, `yourfunctionapp.azurewebsites.net`).

### Step 3: Retrieve the Azure Function Host Key

For least-privilege access, create a dedicated function key for this connector rather than reusing the default host key. The default host key authenticates every consumer of the Function App, so rotating it impacts unrelated callers.

1. In the [Azure portal](https://portal.azure.com), navigate to your deployed Function App.
2. In the left menu, select **App keys** (under the Functions section).
3. Click **+ New host key**, enter the name `connector`, and click **OK**.
4. Copy the generated value -- this is the **Function App Host Key** you will paste into the connection in Step 4.

If you must use the **default** host key (for example, in a single-purpose Function App), copy it from the same **App keys** page. Plan to rotate it whenever the connector is decommissioned or its access scope changes.

### Step 4: Create the Connection in Logic Apps or Power Automate

When adding the Keeper Secrets Manager connector to your workflow for the first time:

1. You will be prompted for connection parameters.
2. Enter the **Function App URL** -- the hostname of your Azure Function App without the `https://` prefix (e.g., `yourfunctionapp.azurewebsites.net`).
3. Enter the **Function App Host Key** obtained in Step 3.
4. Click **Create** to establish the connection.

All subsequent actions using this connector will reuse the same connection.

## Known Issues and Limitations

- **Login record type only**: The Create secret operation currently supports the `login` record type. Other Keeper record types (SSH keys, database credentials, etc.) are not supported for creation through this connector.
- **One-time access token**: The KSM one-time access token can only be used once. After the initial binding, the middleware uses the derived configuration stored in the `KSM_CONFIG` environment variable (Azure Key Vault reference). To rotate the token, generate a new one from the Admin Console and update the application setting.
- **Folder creation not supported**: You cannot create new folders through this connector. Folders must be created in the Keeper Vault or Admin Console.
- **Shared folder requirement**: The `folder_uid` parameter in the Create secret operation must reference a shared folder that the KSM application has explicit write access to. Private or unshared folders will return a 400 error.
- **File attachments**: Secrets with file attachments are listed but the attachment binary content is not returned through this connector.
- **Rate limits**: Request throughput is governed by your Keeper Security subscription tier and the Azure Function App scaling configuration (Consumption plan, App Service plan, etc.).
- **Cold start latency**: If the Azure Function App runs on a Consumption plan, the first request after an idle period may experience cold start latency (typically 2-5 seconds).

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.