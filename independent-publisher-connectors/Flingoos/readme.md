# Flingoos

Flingoos is a procedural knowledge database for AI Agents. It stores, retrieves, and executes enterprise workflows and teaching sessions, enabling AI agents to follow step-by-step operational procedures.

## Publisher: Diligent4

## Prerequisites

1. **Flingoos Account (Free Tier Available):**
   - You must have an active account to use this connector.
   - **Testers & New Users:** Go to [flingoos.diligent4.com](https://flingoos.diligent4.com) and sign up for the **Free Tier** using your Microsoft Work/School account. This ensures your user profile is created before you attempt to connect.
2. **Organization Membership:** Ensure you are part of an active organization within Flingoos (you will be assigned to a default organization on first login on free tier membership).
3. **Azure AD App Registration:** You must register an app to enable authentication (see "Obtaining Credentials" below).

## Supported Operations

### List Contexts
Returns a unified list of projects and sessions (workflows/teaching) accessible to the authenticated user.
- **Parameters:**
  - `limit` (optional): Maximum number of contexts to return (1-100, default: 50)
  - `scope` (optional): Filter by visibility - "all", "mine", or "public" (default: "all")
  - `kind` (optional): Filter by context kind - "all", "project", or "session" (default: "all")
  - `session_type` (optional): Filter sessions by type - "workflow_recording" or "teaching_session"

### Get Context
Retrieves a context by ID. The ID can be either a project or session - auto-detected.
- **Parameters:**
  - `id` (required): The unique identifier of the context (project or session)
  - `detail` (optional): "summary" or "full" (default: "full" for sessions, "summary" for projects)

### Search Contexts
Search for contexts using natural language semantic matching across projects and sessions.
- **Parameters:**
  - `q` (required): Natural language description of what you want to accomplish
  - `top_k` (optional): Number of matches to return (1-20, default: 5)
  - `min_confidence` (optional): Minimum similarity score 0.0-1.0 (default: 0.3)
  - `scope` (optional): Filter by visibility - "all", "mine", or "public"

### Modify Context
Modify context content using natural language. Supports workflow steps, phases, knowledge items, and project metadata.
- **Parameters:**
  - `id` (required): The unique identifier of the context to modify
  - `target_type` (required): What to modify - "step", "phase" (for workflows), "knowledge_item" (for teaching), or "metadata" (for projects)
  - `target_number` (optional): Step or phase number (required for step/phase targets)
  - `target_id` (optional): Knowledge item ID (required for knowledge_item targets)
  - `change_prompt` (required): Natural language description of the change
  - `auto_confirm` (optional): If true, save immediately without preview (default: false)

### Invoke MCP Server
Sends JSON-RPC commands to the Flingoos Model Context Protocol server. Used by Copilot Studio agents for agentic workflows.

## Obtaining Credentials

This connector uses Azure AD (Entra ID) OAuth authentication. You must create an app registration in your Azure portal and configure it to access the Flingoos API.

### Step 1: Create an App Registration

1. Go to the [Azure Portal](https://portal.azure.com)
2. Navigate to **Microsoft Entra ID** > **App registrations**
3. Click **New registration**
4. Configure the app:
   - **Name:** `Flingoos Power Platform Connector` (or any name you prefer)
   - **Supported account types:** Select "Accounts in any organizational directory (Any Microsoft Entra ID tenant - Multitenant)"
   - **Redirect URI:** Select "Web" and enter: `https://global.consent.azure-apim.net/redirect`
5. Click **Register**

### Step 2: Configure API Permissions

1. In your app registration, go to **API permissions**
2. Click **Add a permission**
3. Select **APIs my organization uses**
4. Search for `Flingoos by Diligent4` or use the App ID: `a53cbac8-079c-486f-9848-e763a4da1652`
5. Select the **user_impersonation** scope (or equivalent delegated permission)
6. Click **Add permissions**
7. Click **Grant admin consent** if required by your organization

### Step 3: Create a Client Secret

1. Go to **Certificates & secrets**
2. Click **New client secret**
3. Enter a description (e.g., "Power Platform Connector")
4. Select an expiration period
5. Click **Add**
6. **Copy the secret value immediately** - you won't be able to see it again

### Step 4: Note Your Credentials

You will need:
- **Client ID (Application ID):** Found on the app's Overview page
- **Client Secret:** The value you copied in Step 3

### Step 5: Create the Connection in Power Platform

1. When adding the Flingoos connector, you'll be prompted for credentials
2. Enter your **Client ID** and **Client Secret**
3. Sign in with your Microsoft account that's linked to your Flingoos account

## Known Issues and Limitations

- Rate limiting applies per organization (configurable by plan)
- Semantic search requires embeddings to be enabled for your organization
- Users must have a Flingoos account with the same email as their Microsoft account
- Your Azure AD app must have permission to access the Flingoos API

## Frequently Asked Questions

### What is MCP?
MCP (Model Context Protocol) is an open protocol that standardizes how AI applications interact with external tools and data sources. Flingoos implements MCP to provide a consistent interface for AI agents.

### What's the difference between workflows and teaching sessions?
- **Workflow recordings** are step-by-step procedures with phases, actions, and success criteria
- **Teaching sessions** are knowledge artifacts containing concepts, facts, and relationships

### What is a "context" in Flingoos?
A context is the unit of retrieval for agent execution. There are two kinds:
- **Projects** (containers): Group related sessions together
- **Sessions** (units): Individual workflow recordings or teaching sessions

### Why do I need to create my own app registration?
As an Independent Publisher connector, users must provide their own OAuth credentials. This ensures your organization controls access to the Flingoos API.

### I can't find the Flingoos API in "APIs my organization uses"
Contact support@diligent4.com to ensure the Flingoos API is properly configured for external access, or ask your Azure AD administrator to add the Flingoos API as an enterprise application.

## Deployment Instructions

1. Create an Azure AD app registration (see Obtaining Credentials above)
2. Import this connector into your Power Platform environment
3. Create a new connection using your Client ID and Client Secret
4. Sign in with your Microsoft account
5. Start using Flingoos operations in your Flows or Copilot Studio agents
