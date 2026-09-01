# Olexi MCP (AustLII) — Independent Publisher Connector Submission

## Overview
Olexi MCP is an MCP server that lets MCP-capable hosts (including Copilot Studio) search AustLII for Australian legislation and case law and return titles, URLs, and light metadata. The server provides discovery and shareable result URLs; full text remains on AustLII.

## Public MCP URL
- https://olexi-mcp-root-au-691931843514.australia-southeast1.run.app/
- Transport at root (`/`)

## Publisher
- Olexi — https://olexi.legal — ai@olexi.legal

## Authentication
- None (read-only discovery; no PII). Logs exclude content.

## Tools
- list_databases
- search_austlii
- build_search_url
- search_with_progress

## Documentation
- Manifest: docs/copilot/connector/mcp-manifest.json
- README: docs/copilot/connector/README.md
- TESTING: docs/copilot/connector/TESTING.md
- SUPPORT: docs/copilot/connector/SUPPORT.md
- SECURITY: docs/copilot/connector/SECURITY.md
- CHANGELOG: docs/copilot/connector/CHANGELOG.md
- RIGHTS: docs/copilot/connector/RIGHTS.md
- Submission package index: docs/copilot/connector/SUBMISSION_PACKAGE.md

## Product site
- https://mcp.olexi.legal
- Terms: https://mcp.olexi.legal/terms.html
- Privacy: https://mcp.olexi.legal/privacy.html

## Data rights
- We cite and link to AustLII and return light metadata only.
- Attribution-only justification referencing AustLII Web Developers public materials; see `RIGHTS.md` and PDFs in `docs/austlii/`. We can provide additional assurance if needed.

## Notes
- Endpoint is hosted on Google Cloud Run in australia-southeast1.
- MCP at service root; local combined app may use `/mcp` for development.
