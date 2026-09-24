# TESTING: Olexi MCP Connector

Purpose: Provide deterministic manual steps for Microsoft validation.

Prerequisites
- Public endpoint live at https://olexi-mcp-root-au-691931843514.australia-southeast1.run.app/
- MCP transport served at the service root (POST handshake at "/")
- No auth required on MCP for certification testing

Steps
1) Health check
   - If running the combined local app: GET http://127.0.0.1:3000/status → JSON includes `mcp: true` and `austlii.ok` (true/false)
   - For production Cloud Run: MCP is at the root and may not expose `/status` or `/mcp/health`. Validate by connecting via an MCP-capable host to the base URL.

2) List databases
   - Create a test agent and add the MCP connector pointing to the Cloud Run base URL above
   - In a test chat, invoke tool `list_databases`.
   - Expected: an array of objects with fields `code`, `name`, `description`; length > 10.

3) Search (boolean)
   - Invoke tool `search_austlii` with:
     - query: "privacy AND metadata"
     - databases: ["au/cases/cth/HCA","au/cases/cth/FCA"]
     - method: "boolean"
   - Expected: a list (>= 3 items) with fields `title`, `url`, `metadata` (string or null). `url` hosts on `https://www.austlii.edu.au/`.

4) Build URL
   - Invoke tool `build_search_url` with the same parameters.
   - Expected: a single `https://www.austlii.edu.au/cgi-bin/sinosrch.cgi?...` URL string.

5) Progress search (optional)
   - Invoke tool `search_with_progress` with the same parameters.
   - Expected: progress events (0-1.0) then a list of results like step 3.

Error handling
- If AustLII is down, `search_*` may fail or return 0 results; `build_search_url` still returns a URL.
- HTTP errors map to 4xx/5xx from the upstream where applicable; connector itself responds 200 with content per MCP protocol.
