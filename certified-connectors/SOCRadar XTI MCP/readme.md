# SOCRadar XTI MCP

The SOCRadar XTI MCP connector brings SOCRadar's Extended Threat Intelligence (XTI) platform into Microsoft Copilot and Copilot Studio through a Model Context Protocol (MCP) server. Security teams can enrich indicators of compromise, investigate threat actors and vulnerabilities, monitor dark web credential leaks, track ransomware activity, and triage incidents directly from a Copilot agent - grounded in SOCRadar's real-time threat intelligence.

## Publisher

SOCRadar Inc.

## Prerequisites

To use this connector you will need the following:

- An active **SOCRadar XTI** subscription (<https://socradar.io>).
- A SOCRadar user account with permission to access the relevant XTI modules (CTI, Attack Surface Management, Dark Web Monitoring, Digital Risk Protection, etc.).
- One or more **SOCRadar API keys**, generated from the SOCRadar platform (**Settings -> API Options -> Generate Key**), and your numeric **Company ID** (visible in your SOCRadar dashboard URL). These are entered on the SOCRadar sign-in screen during connection setup.

## Supported Operations

This is an MCP server connector. It exposes a single streamable HTTP endpoint (`InvokeServer`) that Microsoft Copilot uses to discover and invoke the SOCRadar tools at runtime. The server currently provides 44+ tools across the following domains:

### Indicator (IOC) Enrichment
`enrich_indicator`, `enrich_ip_address`, `enrich_domain`, `enrich_hash`, `enrich_url`, `bulk_enrich_indicators`, `get_bad_reputation`, `quick_threat_lookup`, `get_ioc_enrichment_api_status`

### Cyber Threat Intelligence & Investigation
`investigate_threat`, `deep_investigation`

### Threat Actor Intelligence
`get_threat_actors`, `get_threat_actor_detail`, `get_threat_actor_iocs`, `search_actors_by_cve`, `search_actors_by_malware`, `search_actors_by_sector`

### Vulnerability Intelligence
`search_vulnerabilities`, `get_vulnerability_details`, `analyze_vulnerability_risk`, `get_trending_vulnerabilities`, `get_cve_trends`

### Ransomware Intelligence
`get_ransomware_victims`, `get_recent_ransomware_victims`, `analyze_ransomware_trends`

### Dark Web & Identity Intelligence
`check_credential_exposure`, `get_botnet_data`, `query_stealer_logs_on_sale`, `query_breach_intelligence`, `query_identity_intelligence`, `get_identity_intelligence_summary`, `get_credit_card_exposures`, `get_im_content`

### Attack Surface Management
`get_digital_assets`, `get_digital_footprint_summary`

### Brand Protection & Digital Risk
`get_impersonating_domains`, `get_impersonating_accounts`, `get_social_media_findings`, `get_rogue_mobile_applications`, `get_surface_web_monitoring`

### Incident Management
`search_incidents`, `get_incident_details`, `resolve_incidents`, `mark_incidents_as_false_positive`

## Obtaining Credentials

This connector uses **OAuth 2.0 (authorization code + PKCE)** against the SOCRadar MCP server's own authorization server.

1. In Power Automate / Copilot Studio, add the SOCRadar XTI MCP connector and choose **Create connection**.
2. You are redirected to the SOCRadar sign-in screen (`https://mcp.socradar.com/authorize`).
3. Enter your SOCRadar **API key(s)** and **Company ID** and approve the requested scopes (`tools:read`, `tools:execute`).
4. On approval, SOCRadar issues an access token to Microsoft and the connection is established.

## OAuth Application Registration (for the certification team)

The SOCRadar MCP server acts as the OAuth 2.0 authorization server. Endpoints:

- **Authorization URL:** `https://mcp.socradar.com/authorize`
- **Token URL:** `https://mcp.socradar.com/token`
- **Refresh URL:** `https://mcp.socradar.com/token`
- **Scopes:** `tools:read`, `tools:execute`
- **PKCE:** S256 (required)
- **Redirect URI to allow-list:** `https://global.consent.azure-apim.net/redirect`

A dedicated OAuth client (client ID / client secret) will be registered by SOCRadar for the Microsoft Power Platform connector and supplied through Partner Center. The `clientId` value in `apiProperties.json` is a placeholder (`REPLACE_WITH_CLIENT_ID`) and must not contain a real client ID in source control.

## Known Issues and Limitations

- This is an agent-facing MCP connector; it does not expose individual actions in the classic connector designer. Tools are discovered dynamically by the Copilot agent at runtime.
- Access to specific tools depends on the modules enabled for your SOCRadar subscription and the permissions of the API key you provide.
- Rate limits follow your SOCRadar plan; heavy bulk enrichment may be throttled.

## Deployment Instructions

Package the four files in this folder (`apiDefinition.swagger.json`, `apiProperties.json`, `readme.md`, `icon.png`) and submit through the Power Platform connector certification process (or validate locally with `paconn validate --api-def apiDefinition.swagger.json`).
