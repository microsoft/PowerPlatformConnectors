# SECURITY

- Transport: HTTPS/TLS enforced at the hosting platform (e.g., Cloud Run with managed certs).
- Data handling: No user PII stored by the MCP endpoint. Minimal logs include timestamps and technical diagnostics. See `PRIVACY_MCP.md`.
- Authentication: MCP path open for certification; optional API key supported for other endpoints. If enabled for MCP, header: `X-API-Key: <token>`.
- Rate limiting: Daily per-key quotas and IP variance checks exist for non-MCP flows. MCP path relies on platform-level throttling; document if enabling API key.
- Dependencies: FastAPI/Starlette, uvicorn. Keep patched via `requirements.txt`. Use pinned versions for stability.
