# Velaro AI Live Chat

Connect Microsoft Power Automate to Velaro AI Live Chat. Trigger flows on chat events (chat
started, chat ended, lead created, case created) and take actions such as looking up contacts,
creating leads or support cases, sending messages into active conversations, and retrieving full
chat transcripts.

## Prerequisites

You'll need a Velaro account (https://velaro.com) and a Velaro service key.

## Obtaining credentials

Create a Velaro service key from your Velaro account CLI:

```
velaro mcp-key create --label "Power Automate"
```

The key starts with `vel_live_`. It is site-scoped and shown only once — copy it and enter it as
the connector's API key.

## Known issues and limitations

None at this time.

## Common errors and remedies

| Error | Cause | Remedy |
|---|---|---|
| 401 Unauthorized | Invalid or missing API key | Regenerate a service key via `velaro mcp-key create` and re-enter it in the connection |
| 403 Forbidden | The API key's site does not have the requested feature enabled | Confirm the relevant integration/feature flag is enabled for the site in Velaro Admin |

## Frequently asked questions

**Does this connector support multiple Velaro sites?**
Each connection is scoped to the single Velaro site the API key was created for. Create a separate
connection per site.
