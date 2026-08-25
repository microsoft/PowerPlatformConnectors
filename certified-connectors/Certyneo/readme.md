# Certyneo

Certyneo is an eIDAS-compliant electronic signature service. This connector lets a
flow send documents out for signature, react in real time when a signer acts, and
file the signed PDF and its evidence file wherever the rest of the business keeps
its records.

The typical flow is three steps: a document lands somewhere (a document library, a
mailbox, a form submission), the flow creates and sends an envelope, and a second
flow triggered by `envelope.completed` downloads the signed PDF and stores it.

## Publisher: Certyneo

## Prerequisites

You need a Certyneo account on a plan that includes API access. Free and Personal
plans can create API keys and subscribe to events, but they only ever receive
events for sandbox envelopes — see *Known issues and limitations* below.

## Supported operations

### Trigger

**When an envelope event occurs** — fires on the events you select, backed by a
real webhook rather than polling. Available events:

`envelope.created`, `envelope.sent`, `envelope.completed`, `envelope.declined`,
`envelope.voided`, `envelope.expired`, `envelope.returned_to_sender`,
`envelope.resubmitted`, `recipient.signed`, `recipient.viewed`,
`recipient.approved`.

Every payload carries **Envelope ID**, on both envelope and recipient events. That
is the value to feed into the actions below. Deliveries are signed with HMAC-SHA256
in the `X-Certyneo-Signature` header — a hex digest of the raw JSON body, keyed with
the subscription secret. Verify it before a flow acts on the payload.

### Actions

| Action | What it does |
| --- | --- |
| List templates | Your saved envelope templates. Powers the template picker. |
| Create envelope | Creates a DRAFT envelope, from a template or from uploaded documents. |
| Send envelope | Dispatches a DRAFT envelope to its recipients. |
| Get envelope | Current status and recipient details. |
| List envelopes | Paginated list, filterable by status. |
| Upload document | Uploads a PDF or image and returns a document ID. |
| Download signed document | The signed PDF, certificate page included. |
| Download audit trail | The evidence file: who signed, when, from which IP, with which factors. |
| Delete draft envelope | Deletes an envelope that was never sent. |

## Obtaining Credentials

The connector authenticates with a Certyneo API key.

1. Sign in to Certyneo and open **Settings → API Keys**.
2. Create a key. Live keys start with `sk_live_`, sandbox keys with `sk_test_`.
3. When the connector asks for the API key, paste the word `Bearer`, a space, and
   then the key — for example `Bearer sk_live_abc123`. Certyneo expects the word
   `Bearer` in front of the key, and a key pasted on its own is rejected with 401.

The key carries scopes. A key without `envelopes:write` cannot create or send, and
a key without `webhooks:write` cannot back the trigger. Grant the scopes the flow
actually needs.

### Testing without emailing anyone

A sandbox key (`sk_test_`) exercises the whole pipeline without sending a single
invitation email. Nothing reaches a real inbox, which also means nobody can reach
the signing page from an email — so **Create envelope** and **Send envelope**
return an **Access token** for each recipient instead. Open
`https://certyneo.com/sign/<access token>` to sign the test envelope yourself.
Treat that token as a credential: anyone holding it can sign as that recipient.

## Known issues and limitations

**A sent envelope cannot be recalled from a flow.** *Delete draft envelope* only
accepts envelopes still in DRAFT. Once an envelope has gone out it returns 409, and
there is no cancel operation in the API today.

**`recipient.signed` fires before the signed PDF exists**, including for the last
signer. Assembling and sealing the final document happens after that event, so a
flow that triggers on `recipient.signed` and immediately calls *Download signed
document* gets 409 every time. Trigger on `envelope.completed` instead, which fires
once the PDF is on disk.

**Free and Personal plans receive sandbox events only.** Both can create a
subscription and the trigger will show as healthy, but live envelopes never reach
it. If a trigger appears to do nothing on real traffic, check the plan first.

**Use a template or documents, never both.** A template brings its own documents
*and* its own signature fields. When you build from uploaded documents instead, you
must also supply **Fields** — an envelope whose signer has nowhere to sign is
refused at send time with 409 `signer_without_field`.

**Prefer anchor text over coordinates.** Placing a field by quoting a phrase from
the document survives a layout change and needs no measuring. X and Y remain as a
fallback.

**Uploads are capped at 50 MB** per document.

**Files sent as `application/octet-stream` are accepted; the type is detected from
the content.** *Upload document* reads the file's signature (PDF, PNG, JPEG, GIF,
DOC) and stores it under its real type. Word (`.docx`) and OpenDocument (`.odt`)
files cannot be told apart by signature, so for those the declared type must be
correct.

**A purged document returns 410, not 409.** 409 means "not ready yet, come back
after `envelope.completed`". 410 means the signed document was deliberately deleted
under the retention policy and is never coming back. A retry loop should give up on
410.

## Common errors and remedies

| Response | Cause | Remedy |
| --- | --- | --- |
| 401 | Key missing, or pasted without `Bearer` in front | Re-enter the connection as `Bearer sk_live_...` |
| 403 | The key lacks the scope for this operation | Re-issue the key with the scopes the flow needs |
| 409 on download | Envelope is not COMPLETED yet | Trigger on `envelope.completed` rather than polling |
| 409 on send | `signer_without_field` — a signer has no signature field | Add **Fields**, or build from a template |
| 409 on delete | The envelope has already been sent | Sent envelopes cannot be deleted or recalled |
| 410 on download | The document was purged under the retention policy | Stop retrying; the file is gone |

Errors carry both a human-readable `error` message and a stable `code`. Branch on
`code` — the message can be reworded.

## Support

- Documentation: <https://certyneo.com/developers>
- Webhook reference: <https://certyneo.com/developers/webhooks>
- Contact: <https://certyneo.com/fr/contact>
