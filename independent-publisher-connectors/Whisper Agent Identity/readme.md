# Whisper Agent Identity

Give every AI agent a real, routable IPv6 `/128` identity — and operate it from Power Platform.

Whisper allocates each agent an IPv6 `/128` from `2a04:2a01::/32` (announced by AS219419), anchored in DNS, RDAP and a tamper-evident transparency log. This connector is **two-tier** (Postel's law — *be liberal in what you accept*):

- **Keyless (no account, no key).** Verify whether an IP is a real Whisper agent and whose it is, read its RDAP record, its identity transparency log, and who has been resolving its name.
- **Keyed (your Whisper API key).** Unlock the control plane: register agents, list your fleet, set per-tenant resolver policy, read activity logs, and revoke.

The API key is an **optional** connection field. Leave it blank and the keyless operations work exactly as before; fill it in and the control operations light up. The keyless operations keep working with or without a key.

## Publisher: Whisper Security

## Prerequisites

- **Keyless operations:** none. No account, credential, or API key.
- **Control operations:** a Whisper API key (`whisper_live_...`), entered in the optional **Whisper API key** connection field.

## Obtaining Credentials

Run `whisper login` with the Whisper CLI, or copy your key from the Whisper dashboard. The connector accepts the key as an optional connection parameter; it is sent as the `X-API-Key` header **only** on the control operations, and only to the Whisper control plane.

## Supported Operations

### Keyless (public) — `rdap.whisper.online`

| Operation | Description |
|---|---|
| **Verify agent identity** | Full verification chain (PTR + FCrDNS AAAA + DANE-TLSA + signed identity doc) → one verdict. |
| **RDAP lookup for an agent address** | RDAP (RFC 9082/9083) record for a `/128`. |
| **Get identity transparency log** | Append-only, hash-chained, ES256-signed issuance/revocation log. |
| **Get inbound identity lookups** | Who resolved/queried the agent's name (k-anonymised source prefixes). |

### Control (requires your API key) — `graph.whisper.security`

| Operation | Description |
|---|---|
| **Register a new agent** | Mint a new agent with its own `/128` and its own API key (returned once). |
| **List your agents** | List your fleet (or records / identities), confined to your tenant. |
| **Set resolver policy** | Read or set your per-tenant DNS policy (default allow/deny + block/allow lists). |
| **Get activity logs** | Query recent DNS / connection / allocation activity from warm storage. |
| **Revoke an agent** | Fully revoke an agent — withdraw its `/128`, PTR, tokens and key. Irreversible. |

Each control operation carries a **`query`** field pre-filled with the exact `whisper.agents` control call for that operation (a Cypher `CALL`). Edit the values inside `args` (e.g. the agent label, the policy lists); the connector POSTs `{"query": ...}` to the control plane.

## Known Issues and Limitations

- **Single connection, two tiers.** The API key field is optional; keyless operations ignore it and resolve regardless. A control operation invoked with no key returns a clear `400` — *"anonymous callers cannot use the agent control plane — an attributable API key is required"* — never an opaque error.
- Control operations take the control call as a `query` string (pre-filled per operation). Values inside `args` are sent verbatim; keep them well-formed.
- An address with no Whisper agent identity returns a clean *not found* (HTTP 404), not an error.
- No connector icon is submitted (independent-publisher connectors do not ship icons); the brand colour `#7a40ff` is set instead.
