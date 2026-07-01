# Whisper Agent Identity

Verify the identity of an AI agent by its Whisper IPv6 address.

Whisper gives every agent a real, routable IPv6 `/128` identity (allocated from `2a04:2a01::/32`, announced by AS219419) that is anchored in DNS and RDAP. This connector wraps Whisper's public, keyless identity surface so a flow, app, or copilot can answer one question — *"is this IP a real Whisper agent, and whose?"* — and inspect the supporting evidence, without stitching four protocols together itself.

## Publisher: Whisper Security

## Prerequisites

There are no prerequisites to use this connector. All operations are public and keyless — no account, credential, or API key is required.

## Obtaining Credentials

There are no credentials needed to use this service. The connector uses anonymous access.

## Supported Operations

### Verify agent identity

Run the full Whisper-agent verification chain server-side for one IPv6 address — reverse DNS (PTR), forward-confirm (AAAA / FCrDNS), DANE-TLSA pin, and the signed identity document — and return a single verdict: whether the address is a real Whisper agent, its canonical hostname, its operator/tenant handle, and the supporting evidence.

### RDAP lookup for an agent address

Return the RDAP (RFC 9082 / 9083) record for a Whisper agent IPv6 `/128`: handle, canonical name, registrant/operator entity, status, country, registration event, behavioural posture, and related links.

### Get identity transparency log

Return the public, append-only, tamper-evident transparency log for an agent `/128`: the ordered feed of issuance and revocation events, each chained to the previous by a SHA-256 proof, with a signed (ES256) chain root and an inclusion proof against the global Merkle ledger.

### Get inbound identity lookups

Return the public feed of who has resolved or queried this agent's name — the PTR / AAAA / TLSA lookups against its authoritative-zone records and the RDAP accesses to it — each row k-anonymised to the source `/48` (IPv6) or `/24` (IPv4) prefix.

## Known Issues and Limitations

- Operations describe IPv6 `/128` agent identities allocated from `2a04:2a01::/32`. An address with no Whisper agent identity returns a clean *not found* (HTTP 404) rather than an error.
- The transparency and inbound-lookups feeds are live and are not cached.
